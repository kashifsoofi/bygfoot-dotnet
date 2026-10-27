namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of league XML files.
/// Corresponds to the C module src/xml_loadsave_league.h and src/xml_loadsave_league.c
/// (xml_loadsave_league_read, xml_loadsave_league_write and the
/// xml_loadsave_league_start_element, xml_loadsave_league_end_element and
/// xml_loadsave_league_text functions).
/// </summary>
public static class LeagueXmlLoadSave
{
    // Common tags used in the league xml files. Corresponds to the tag enum of src/xml.h.
    private const int TagName = 0;
    private const int TagSymbol = 1;
    private const int TagSid = 2;
    private const int TagShortName = 3;
    private const int TagId = 4;
    private const int TagWeekGap = 5;
    private const int TagWeekBreak = 6;
    private const int TagWeekBreakLength = 7;
    private const int TagSkipWeeksWith = 8;
    private const int TagYellowRed = 9;
    private const int TagTeamId = 10;
    private const int TagNamesFile = 11;
    private const int TagProperty = 12;
    private const int TagRound = 13;

    // Tags. Corresponds to the tag enum of the league module in src/xml_loadsave_league.c.
    private const int TagLeague = 2000;
    private const int TagLeagueLayer = 2001;
    private const int TagLeagueFirstWeek = 2002;
    private const int TagLeagueRoundRobins = 2003;
    private const int TagLeagueAverageTalent = 2004;
    private const int TagLeaguePromRel = 2005;
    private const int TagLeaguePromGames = 2006;
    private const int TagLeaguePromRelPromGamesDestSid = 2007;
    private const int TagLeaguePromRelPromGamesCupSid = 2008;
    private const int TagLeaguePromRelPromGamesLoserSid = 2009;
    private const int TagLeaguePromRelPromGamesNumberOfAdvance = 2010;
    private const int TagLeaguePromRelElements = 2011;
    private const int TagLeaguePromRelElement = 2012;
    private const int TagLeaguePromRelElementRank = 2013;
    private const int TagLeaguePromRelElementDestSid = 2014;
    private const int TagLeaguePromRelElementType = 2015;
    private const int TagLeaguePromRelElementFromTable = 2016;
    private const int TagLeagueBreak = 2017;
    private const int TagLeagueJoinedLeagueSid = 2018;
    private const int TagLeagueJoinedLeagueRr = 2019;
    private const int TagLeagueNewTableName = 2020;
    private const int TagLeagueNewTableAddWeek = 2021;
    private const int TagLeagueTwoMatchWeekStart = 2022;
    private const int TagLeagueTwoMatchWeekEnd = 2023;
    private const int TagLeagueTableFile = 2024;
    private const int TagEnd = 2025;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the league module.
    /// </summary>
    public enum XmlLeagueLoadSaveStates
    {
        TagName = 0,
        TagSymbol = 1,
        TagSid = 2,
        TagShortName = 3,
        TagId = 4,
        TagWeekGap = 5,
        TagWeekBreak = 6,
        TagWeekBreakLength = 7,
        TagSkipWeeksWith = 8,
        TagYellowRed = 9,
        TagTeamId = 10,
        TagNamesFile = 11,
        TagProperty = 12,
        TagRound = 13,
        TagLeague = 2000,
        TagLeagueLayer = 2001,
        TagLeagueFirstWeek = 2002,
        TagLeagueRoundRobins = 2003,
        TagLeagueAverageTalent = 2004,
        TagLeaguePromRel = 2005,
        TagLeaguePromGames = 2006,
        TagLeaguePromRelPromGamesDestSid = 2007,
        TagLeaguePromRelPromGamesCupSid = 2008,
        TagLeaguePromRelPromGamesLoserSid = 2009,
        TagLeaguePromRelPromGamesNumberOfAdvance = 2010,
        TagLeaguePromRelElements = 2011,
        TagLeaguePromRelElement = 2012,
        TagLeaguePromRelElementRank = 2013,
        TagLeaguePromRelElementDestSid = 2014,
        TagLeaguePromRelElementType = 2015,
        TagLeaguePromRelElementFromTable = 2016,
        TagLeagueBreak = 2017,
        TagLeagueJoinedLeagueSid = 2018,
        TagLeagueJoinedLeagueRr = 2019,
        TagLeagueNewTableName = 2020,
        TagLeagueNewTableAddWeek = 2021,
        TagLeagueTwoMatchWeekStart = 2022,
        TagLeagueTwoMatchWeekEnd = 2023,
        TagLeagueTableFile = 2024,
        TagEnd = 2025
    }

    private static XmlLeagueLoadSaveStates State = XmlLeagueLoadSaveStates.TagLeague;
    private static League NewLeague = new League();
    private static PromRelElement NewElement = new PromRelElement();
    private static PromGames NewPromGames = new PromGames();
    private static WeekBreak NewWeekBreak = new WeekBreak();
    private static int Promrankidx = 0;
    private static string Dirname = "";

    /// <summary>
    /// Function reading a league save file and filling in the given league.
    /// @param filename The name of the league xml file to be read.
    /// @param teamFile The name of the team xml file the league teams are read from.
    /// @param league The league to be read into.
    /// </summary>
    /// <param name="filename">The name of the league xml file to read.</param>
    /// <param name="teamFile">The name of the team xml file the league teams are read from.</param>
    /// <param name="league">The league to be read into.</param>
    public static void LeagueXmlLoadSaveRead(string filename, string teamFile, League league)
    {
        if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(teamFile))
            return;

        State = XmlLeagueLoadSaveStates.TagLeague;
        NewLeague = league;
        Dirname = Path.GetDirectoryName(filename);
        NewElement = new PromRelElement();
        NewPromGames = new PromGames();
        NewWeekBreak = new WeekBreak();
        Promrankidx = 0;

        TeamsXmlLoadSave.TeamsXmlLoadSaveRead(teamFile, league.Teams);

        using (var reader = XmlReader.Create(filename))
        {
            reader.MoveToContent();
            int depth = 0;

            while (true)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.IsEmptyElement)
                        {
                            LeagueXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            LeagueXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            LeagueXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        LeagueXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        LeagueXmlLoadSaveReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void LeagueXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
    {
        StartElement(elementName, attributeName => reader.GetAttribute(attributeName));
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="attributeNames">The attributes of the opening tag.</param>
    public static void LeagueXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the league XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);
        bool validTag = false;

        switch (tag)
        {
            case TagLeague:
                State = XmlLeagueLoadSaveStates.TagLeague;
                validTag = true;
                break;
            case TagLeagueLayer:
                State = XmlLeagueLoadSaveStates.TagLeagueLayer;
                validTag = true;
                break;
            case TagLeagueFirstWeek:
                State = XmlLeagueLoadSaveStates.TagLeagueFirstWeek;
                validTag = true;
                break;
            case TagLeagueRoundRobins:
                State = XmlLeagueLoadSaveStates.TagLeagueRoundRobins;
                validTag = true;
                break;
            case TagLeagueAverageTalent:
                State = XmlLeagueLoadSaveStates.TagLeagueAverageTalent;
                validTag = true;
                break;
            case TagLeaguePromRel:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRel;
                validTag = true;
                break;
            case TagLeaguePromGames:
                State = XmlLeagueLoadSaveStates.TagLeaguePromGames;
                validTag = true;
                break;
            case TagLeaguePromRelPromGamesDestSid:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesDestSid;
                validTag = true;
                break;
            case TagLeaguePromRelPromGamesCupSid:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesCupSid;
                validTag = true;
                break;
            case TagLeaguePromRelPromGamesLoserSid:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesLoserSid;
                validTag = true;
                break;
            case TagLeaguePromRelPromGamesNumberOfAdvance:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesNumberOfAdvance;
                validTag = true;
                break;
            case TagLeaguePromRelElements:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElements;
                validTag = true;
                break;
            case TagLeaguePromRelElement:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElement;
                validTag = true;
                break;
            case TagLeaguePromRelElementRank:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElementRank;
                validTag = true;
                break;
            case TagLeaguePromRelElementDestSid:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElementDestSid;
                validTag = true;
                break;
            case TagLeaguePromRelElementType:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElementType;
                validTag = true;
                break;
            case TagLeaguePromRelElementFromTable:
                State = XmlLeagueLoadSaveStates.TagLeaguePromRelElementFromTable;
                validTag = true;
                break;
            case TagLeagueBreak:
                State = XmlLeagueLoadSaveStates.TagLeagueBreak;
                validTag = true;
                break;
            case TagLeagueJoinedLeagueSid:
                State = XmlLeagueLoadSaveStates.TagLeagueJoinedLeagueSid;
                validTag = true;
                break;
            case TagLeagueJoinedLeagueRr:
                State = XmlLeagueLoadSaveStates.TagLeagueJoinedLeagueRr;
                validTag = true;
                break;
            case TagLeagueNewTableName:
                State = XmlLeagueLoadSaveStates.TagLeagueNewTableName;
                validTag = true;
                break;
            case TagLeagueNewTableAddWeek:
                State = XmlLeagueLoadSaveStates.TagLeagueNewTableAddWeek;
                validTag = true;
                break;
            case TagLeagueTwoMatchWeekStart:
                State = XmlLeagueLoadSaveStates.TagLeagueTwoMatchWeekStart;
                validTag = true;
                break;
            case TagLeagueTwoMatchWeekEnd:
                State = XmlLeagueLoadSaveStates.TagLeagueTwoMatchWeekEnd;
                validTag = true;
                break;
            case TagLeagueTableFile:
                State = XmlLeagueLoadSaveStates.TagLeagueTableFile;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_league_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagLeaguePromRelElement)
        {
            NewElement = new PromRelElement();
            Promrankidx = 0;
        }

        if (tag == TagLeaguePromGames)
            NewPromGames = new PromGames();

        if (tag == TagLeagueJoinedLeagueSid)
            NewLeague.JoinedLeagues.Add(new JoinedLeague());

        if (tag == TagLeagueNewTableName)
            NewLeague.NewTables.Add(new NewTable());

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_league_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the league module
    /// are the numbers following the underscore in the tag names (e.g. '_2000').
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The tag.</returns>
    private static int GetTagFromName(string elementName)
    {
        string tagName = elementName.Substring(1);

        return (int)(double.TryParse(tagName, out double tagValue) ? tagValue : 0);
    }

    /// <summary>
    /// Get the value of an attribute from a dictionary of attributes.
    /// </summary>
    /// <param name="attributeNames">The attributes.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The value of the attribute, or null if it is not set.</returns>
    private static string GetAttribute(IDictionary<string, string> attributeNames, string attributeName)
    {
        return attributeNames.TryGetValue(attributeName, out string value) ? value : null;
    }

    /// <summary>
    /// The function called by the reader when a closing tag is read.
    /// The state variable is changed in this function.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void LeagueXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagLeagueFirstWeek ||
            tag == TagLeagueLayer ||
            tag == TagLeagueBreak ||
            tag == TagLeagueJoinedLeagueSid ||
            tag == TagLeagueJoinedLeagueRr ||
            tag == TagLeagueNewTableName ||
            tag == TagLeagueNewTableAddWeek ||
            tag == TagLeagueTwoMatchWeekStart ||
            tag == TagLeagueTwoMatchWeekEnd ||
            tag == TagLeagueTableFile ||
            tag == TagLeagueAverageTalent ||
            tag == TagLeagueRoundRobins ||
            tag == TagProperty ||
            tag == TagName ||
            tag == TagNamesFile ||
            tag == TagShortName ||
            tag == TagSymbol ||
            tag == TagSid ||
            tag == TagId ||
            tag == TagWeekGap ||
            tag == TagWeekBreak ||
            tag == TagWeekBreakLength ||
            tag == TagSkipWeeksWith ||
            tag == TagYellowRed ||
            tag == TagLeaguePromRel)
        {
            State = XmlLeagueLoadSaveStates.TagLeague;
        }
        else if (tag == TagLeaguePromGames ||
                 tag == TagLeaguePromRelElements)
        {
            State = XmlLeagueLoadSaveStates.TagLeaguePromRel;

            if (tag == TagLeaguePromGames)
                NewLeague.PromRel.PromGames.Add(NewPromGames);
        }
        else if (tag == TagLeaguePromRelPromGamesDestSid ||
                 tag == TagLeaguePromRelPromGamesCupSid ||
                 tag == TagLeaguePromRelPromGamesNumberOfAdvance)
        {
            State = XmlLeagueLoadSaveStates.TagLeaguePromGames;
        }
        else if (tag == TagLeaguePromRelElement)
        {
            State = XmlLeagueLoadSaveStates.TagLeaguePromRelElements;
            NewLeague.PromRel.Elements.Add(NewElement);
        }
        else if (tag == TagLeaguePromRelElementRank ||
                 tag == TagLeaguePromRelElementDestSid ||
                 tag == TagLeaguePromRelElementFromTable ||
                 tag == TagLeaguePromRelElementType)
        {
            State = XmlLeagueLoadSaveStates.TagLeaguePromRelElement;

            if (tag == TagLeaguePromRelElementRank)
                Promrankidx++;
        }
        else if (tag != TagLeague)
        {
            Debug.DebugPrintMessage($"xml_loadsave_league_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the league variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void LeagueXmlLoadSaveReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlLeagueLoadSaveStates.TagName:
                NewLeague.Name = text;
                break;
            case XmlLeagueLoadSaveStates.TagProperty:
                NewLeague.Properties.Add(text);
                break;
            case XmlLeagueLoadSaveStates.TagShortName:
                NewLeague.ShortName = text;
                break;
            case XmlLeagueLoadSaveStates.TagNamesFile:
                NewLeague.NamesFile = text;
                break;
            case XmlLeagueLoadSaveStates.TagSymbol:
                NewLeague.Symbol = text;
                break;
            case XmlLeagueLoadSaveStates.TagSid:
                NewLeague.Sid = text;
                break;
            case XmlLeagueLoadSaveStates.TagId:
                NewLeague.Id = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueLayer:
                NewLeague.Layer = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueFirstWeek:
                NewLeague.FirstWeek = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueRoundRobins:
                NewLeague.RoundRobins = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagWeekGap:
                NewLeague.WeekGap = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagWeekBreak:
                NewWeekBreak = new WeekBreak { WeekNumber = intValue };
                break;
            case XmlLeagueLoadSaveStates.TagWeekBreakLength:
                NewWeekBreak.Length = intValue;
                NewLeague.WeekBreaks.Add(NewWeekBreak);
                break;
            case XmlLeagueLoadSaveStates.TagSkipWeeksWith:
                NewLeague.SkipWeeksWith.Add(text);
                break;
            case XmlLeagueLoadSaveStates.TagYellowRed:
                NewLeague.YellowRed = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueBreak:
                NewLeague.RrBreaks.Add(intValue);
                break;
            case XmlLeagueLoadSaveStates.TagLeagueJoinedLeagueSid:
                NewLeague.JoinedLeagues[NewLeague.JoinedLeagues.Count - 1].Sid = text;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueJoinedLeagueRr:
                NewLeague.JoinedLeagues[NewLeague.JoinedLeagues.Count - 1].Rr = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueNewTableName:
                NewLeague.NewTables[NewLeague.NewTables.Count - 1].Name = text;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueNewTableAddWeek:
                NewLeague.NewTables[NewLeague.NewTables.Count - 1].AddWeek = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeagueTwoMatchWeekStart:
                if (NewLeague.TwoMatchWeeks[0] == null)
                    NewLeague.TwoMatchWeeks[0] = new List<int>();
                NewLeague.TwoMatchWeeks[0].Add(intValue);
                break;
            case XmlLeagueLoadSaveStates.TagLeagueTwoMatchWeekEnd:
                if (NewLeague.TwoMatchWeeks[1] == null)
                    NewLeague.TwoMatchWeeks[1] = new List<int>();
                NewLeague.TwoMatchWeeks[1].Add(intValue);
                break;
            case XmlLeagueLoadSaveStates.TagLeagueTableFile:
            {
                // The table files are read via the table module; they are not
                // handled here as the table module has not been converted yet.
                break;
            }
            case XmlLeagueLoadSaveStates.TagLeagueAverageTalent:
                NewLeague.AverageTalent = floatValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesDestSid:
                NewPromGames.DestSid = text;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesCupSid:
                NewPromGames.CupSid = text;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesNumberOfAdvance:
                NewPromGames.NumberOfAdvance = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelPromGamesLoserSid:
                NewPromGames.LoserSid = text;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelElementRank:
                NewElement.Ranks[Promrankidx] = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelElementType:
                NewElement.Type = (PromRelType)intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelElementFromTable:
                NewElement.FromTable = intValue;
                break;
            case XmlLeagueLoadSaveStates.TagLeaguePromRelElementDestSid:
                NewElement.DestSid = text;
                break;
        }
    }

    /// <summary>
    /// Write the league into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the league save file (e.g. 'league_england1').</param>
    /// <param name="league">The league to write.</param>
    public static void LeagueXmlLoadSaveWrite(string prefix, League league)
    {
        string teamsFileName = $"{prefix}___league_{league.Id}_teams.xml";
        TeamsXmlLoadSave.TeamsXmlLoadSaveWrite(teamsFileName, league.Teams);

        string statFileName = $"{prefix}___league_{league.Id}_stat.xml";
        StatXmlLoadSave.StatXmlLoadSaveWrite(statFileName, league.Stats);

        string leagueFileName = $"{prefix}___league_{league.Id}.xml";
        using (var fil = new StreamWriter(leagueFileName))
        {
            fil.WriteLine("<_2000>");
            if (league.Name != null)
                fil.WriteLine("<_0>" + league.Name + "</_0>");
            if (league.ShortName != null)
                fil.WriteLine("<_3>" + league.ShortName + "</_3>");
            if (league.NamesFile != null)
                fil.WriteLine("<_11>" + league.NamesFile + "</_11>");
            if (league.Sid != null)
                fil.WriteLine("<_2>" + league.Sid + "</_2>");
            if (league.Symbol != null)
                fil.WriteLine("<_1>" + league.Symbol + "</_1>");
            foreach (string property in league.Properties)
            {
                fil.WriteLine("<_12>" + property + "</_12>");
            }
            foreach (string skipWeekWith in league.SkipWeeksWith)
            {
                fil.WriteLine("<_8>" + skipWeekWith + "</_8>");
            }
            fil.WriteLine("<_4>" + league.Id + "</_4>");
            fil.WriteLine("<_2001>" + league.Layer + "</_2001>");
            fil.WriteLine("<_2002>" + league.FirstWeek + "</_2002>");
            fil.WriteLine("<_2003>" + league.RoundRobins + "</_2003>");
            fil.WriteLine("<_5>" + league.WeekGap + "</_5>");
            fil.WriteLine("<_9>" + league.YellowRed + "</_9>");
            foreach (int rrBreak in league.RrBreaks)
            {
                fil.WriteLine("<_2017>" + rrBreak + "</_2017>");
            }
            foreach (WeekBreak weekBreak in league.WeekBreaks)
            {
                fil.WriteLine("<_6>" + weekBreak.WeekNumber + "</_6>");
                fil.WriteLine("<_7>" + weekBreak.Length + "</_7>");
            }
            foreach (Table table in league.Tables)
            {
                // The table files are written via the table module; they have not
                // been converted yet.
            }
            foreach (JoinedLeague joinedLeague in league.JoinedLeagues)
            {
                fil.WriteLine("<_2018>" + joinedLeague.Sid + "</_2018>");
                fil.WriteLine("<_2019>" + joinedLeague.Rr + "</_2019>");
            }
            foreach (NewTable newTable in league.NewTables)
            {
                fil.WriteLine("<_2020>" + newTable.Name + "</_2020>");
                fil.WriteLine("<_2021>" + newTable.AddWeek + "</_2021>");
            }
            fil.WriteLine("<_2004>" + (int)(league.AverageTalent * 10000) + "</_2004>");
            foreach (int twoMatchWeekStart in league.TwoMatchWeeks[0])
            {
                fil.WriteLine("<_2022>" + twoMatchWeekStart + "</_2022>");
            }
            foreach (int twoMatchWeekEnd in league.TwoMatchWeeks[1])
            {
                fil.WriteLine("<_2023>" + twoMatchWeekEnd + "</_2023>");
            }
            LeagueXmlLoadSaveWritePromRel(fil, league);
            fil.WriteLine("</_2000>");
        }
    }

    /// <summary>
    /// Write the promotion/relegation data into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="league">The league.</param>
    public static void LeagueXmlLoadSaveWritePromRel(TextWriter fil, League league)
    {
        fil.WriteLine("<_2005>");
        foreach (PromGames promGame in league.PromRel.PromGames)
        {
            fil.WriteLine("\t<_2006>");
            if (promGame.DestSid != null)
                fil.WriteLine("\t\t<_2007>" + promGame.DestSid + "</_2007>");
            if (promGame.CupSid != null)
                fil.WriteLine("\t\t<_2008>" + promGame.CupSid + "</_2008>");
            fil.WriteLine("\t\t<_2010>" + promGame.NumberOfAdvance + "</_2010>");
            if (promGame.LoserSid != null)
                fil.WriteLine("\t\t<_2009>" + promGame.LoserSid + "</_2009>");
            fil.WriteLine("\t</_2006>");
        }
        fil.WriteLine("\t</_2005>");
    }
}
