namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of league XML files.
/// Corresponds to the C module src/xml_league.h and src/xml_league.c
/// (xml_league_read, xml_league_read_start_element, xml_league_read_end_element
/// and xml_league_read_text).
/// </summary>
public static class LeagueXml
{
    private const string TagLeague = "league";
    private const string TagLayer = "layer";
    private const string TagFirstWeek = "first_week";
    private const string TagRoundRobins = "round_robins";
    private const string TagAverageTalent = "average_talent";
    private const string TagNamesFile = "names_file";
    private const string TagBreak = "break";
    private const string TagJoinedLeague = "joined_league";
    private const string TagNewTable = "new_table";
    private const string TagPromRel = "prom_rel";
    private const string TagPromGames = "prom_games";
    private const string TagPromGamesDestSid = "prom_games_dest_sid";
    private const string TagPromGamesLoserSid = "prom_games_loser_sid";
    private const string TagPromGamesNumberOfAdvance = "prom_games_number_of_advance";
    private const string TagPromGamesCupSid = "prom_games_cup_sid";
    private const string TagPromRelElement = "prom_rel_element";
    private const string TagPromRelElementRankStart = "prom_rel_element_rank_start";
    private const string TagPromRelElementRankEnd = "prom_rel_element_rank_end";
    private const string TagPromRelElementDestSid = "prom_rel_element_dest_sid";
    private const string TagPromRelElementType = "prom_rel_element_type";
    private const string TagPromRelElementFromTable = "prom_rel_element_from_table";
    private const string TagTeams = "teams";
    private const string TagTeam = "team";
    private const string TagTeamName = "team_name";
    private const string TagTeamSymbol = "team_symbol";
    private const string TagTeamNamesFile = "team_names_file";
    private const string TagTeamAverageTalent = "team_average_talent";
    private const string TagTeamDefFile = "team_def_file";
    private const string TagTwoMatchWeekStart = "two_match_week_start";
    private const string TagTwoMatchWeekEnd = "two_match_week_end";
    private const string TagDefName = "name";
    private const string TagDefShortName = "short_name";
    private const string TagDefSid = "sid";
    private const string TagDefSymbol = "symbol";
    private const string TagDefWeekGap = "week_gap";
    private const string TagDefWeekBreak = "break_in";
    private const string TagDefYellowRed = "yellow_red";
    private const string TagDefProperty = "property";
    private const string TagDefSkipWeeksWith = "skip_weeks_with";
    private const string TagSkipWeeksWith = "skip_weeks_with";

    private const string AttNameJoinedLeagueRr = "rr";
    private const string AttNameNewTableName = "name";

    private const float MaxPlayerSkill = 9900000f;

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlLeagueStates.
    /// </summary>
    public enum XmlLeagueStates
    {
        StateLeague = 0,
        StateSid,
        StateName,
        StateShortName,
        StateSymbol,
        StateLayer,
        StateFirstWeek,
        StateWeekGap,
        StateWeekBreak,
        StateSkipWeeksWith,
        StateRoundRobins,
        StateYellowRed,
        StateAverageTalent,
        StateNamesFile,
        StatePromRel,
        StatePromGames,
        StatePromGamesDestSid,
        StatePromGamesLoserSid,
        StatePromGamesNumberOfAdvance,
        StatePromGamesCupSid,
        StatePromRelElement,
        StatePromRelElementRankStart,
        StatePromRelElementRankEnd,
        StatePromRelElementDestSid,
        StatePromRelElementType,
        StatePromRelElementFromTable,
        StateTeams,
        StateTeam,
        StateTeamName,
        StateTeamSymbol,
        StateTeamNamesFile,
        StateTeamAverageTalent,
        StateTeamDefFile,
        StateBreak,
        StateJoinedLeague,
        StateProperty,
        StateNewTable,
        StateTwoMatchWeekStart,
        StateTwoMatchWeekEnd,
        StateEnd
    }

    private static XmlLeagueStates State = XmlLeagueStates.StateLeague;
    private static League NewLeague = new League();

    /// <summary>
    /// Function reading an XML file specifying a league.
    /// A new league struct is filled in and appended to the
    /// IList of leagues.
    /// @param leagueName name of the xml file ( e.g. 'league_england1.xml')
    /// to be read. Full path is not necessary, if the file is located in
    /// one of the support directories; neither are the prefix 'league_'
    /// or the suffix '.xml'.
    /// @param leagues The list we append the new league to.
    /// </summary>
    /// <param name="leagueName">Name of the league xml file.</param>
    /// <param name="leagues">The list to append the new league to.</param>
    public static void XmlLeagueRead(string leagueName, IList<League> leagues)
    {
        string fileName = FileHelper.FindSupportFile(leagueName, false);
        if (fileName == null)
            fileName = FileHelper.FindSupportFile($"league_{leagueName}.xml", true);
        if (string.IsNullOrEmpty(fileName))
            return;

        State = XmlLeagueStates.StateLeague;
        NewLeague = new League();

        using (var reader = XmlReader.Create(fileName))
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
                            XmlLeagueReadStartElement(reader, reader.LocalName);
                            XmlLeagueReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlLeagueReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlLeagueReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlLeagueReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }

        AdjustRrBreaks();
        AdjustWeekBreaks();

        leagues.Add(NewLeague);
    }

    /// <summary>
    /// Function reading a league from an XML node.
    /// A new league struct is filled in and returned.
    /// @param node The XML node of the league to be read.
    /// </summary>
    /// <param name="node">The league XML node.</param>
    /// <returns>The league we read.</returns>
    public static League XmlLeagueReadFromNode(XmlNode node)
    {
        State = XmlLeagueStates.StateLeague;
        NewLeague = new League();

        ProcessLeagueElement(node);

        AdjustRrBreaks();
        AdjustWeekBreaks();

        return NewLeague;
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void XmlLeagueReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlLeagueReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the league XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagLeague:
                NewLeague = new League();
                State = XmlLeagueStates.StateLeague;
                break;
            case TagLayer:
                State = XmlLeagueStates.StateLayer;
                break;
            case TagFirstWeek:
                State = XmlLeagueStates.StateFirstWeek;
                break;
            case TagRoundRobins:
                State = XmlLeagueStates.StateRoundRobins;
                break;
            case TagAverageTalent:
                State = XmlLeagueStates.StateAverageTalent;
                break;
            case TagNamesFile:
                State = XmlLeagueStates.StateNamesFile;
                break;
            case TagBreak:
                State = XmlLeagueStates.StateBreak;
                break;
            case TagJoinedLeague:
                State = XmlLeagueStates.StateJoinedLeague;
                NewLeague.JoinedLeagues.Add(new JoinedLeague());
                var joinedLeague = NewLeague.JoinedLeagues[NewLeague.JoinedLeagues.Count - 1];
                string joinedLeagueRr = getAttribute(AttNameJoinedLeagueRr);
                joinedLeague.Rr = (joinedLeagueRr != null && int.TryParse(joinedLeagueRr, out int joinedLeagueRrValue))
                    ? joinedLeagueRrValue
                    : 2;
                break;
            case TagNewTable:
                State = XmlLeagueStates.StateNewTable;
                NewLeague.NewTables.Add(new NewTable());
                var newTable = NewLeague.NewTables[NewLeague.NewTables.Count - 1];
                string newTableName = getAttribute(AttNameNewTableName);
                newTable.Name = (newTableName != null && newTableName.Length > 0) ? newTableName : NewLeague.Name;
                break;
            case TagTwoMatchWeekStart:
                State = XmlLeagueStates.StateTwoMatchWeekStart;
                break;
            case TagTwoMatchWeekEnd:
                State = XmlLeagueStates.StateTwoMatchWeekEnd;
                break;
            case TagPromRel:
                State = XmlLeagueStates.StatePromRel;
                break;
            case TagPromGames:
                State = XmlLeagueStates.StatePromGames;
                NewLeague.PromRel.PromGames.Add(new PromGames());
                break;
            case TagPromGamesDestSid:
                State = XmlLeagueStates.StatePromGamesDestSid;
                break;
            case TagPromGamesLoserSid:
                State = XmlLeagueStates.StatePromGamesLoserSid;
                break;
            case TagPromGamesNumberOfAdvance:
                State = XmlLeagueStates.StatePromGamesNumberOfAdvance;
                break;
            case TagPromGamesCupSid:
                State = XmlLeagueStates.StatePromGamesCupSid;
                break;
            case TagPromRelElement:
                State = XmlLeagueStates.StatePromRelElement;
                NewLeague.PromRel.Elements.Add(new PromRelElement());
                break;
            case TagPromRelElementRankStart:
                State = XmlLeagueStates.StatePromRelElementRankStart;
                break;
            case TagPromRelElementRankEnd:
                State = XmlLeagueStates.StatePromRelElementRankEnd;
                break;
            case TagPromRelElementDestSid:
                State = XmlLeagueStates.StatePromRelElementDestSid;
                break;
            case TagPromRelElementType:
                State = XmlLeagueStates.StatePromRelElementType;
                break;
            case TagPromRelElementFromTable:
                State = XmlLeagueStates.StatePromRelElementFromTable;
                break;
            case TagTeams:
                State = XmlLeagueStates.StateTeams;
                break;
            case TagTeam:
                State = XmlLeagueStates.StateTeam;
                NewLeague.Teams.Add(new Team
                {
                    Symbol = NewLeague.Symbol,
                    NamesFile = NewLeague.NamesFile,
                    Clid = NewLeague.Id
                });
                break;
            case TagTeamName:
                State = XmlLeagueStates.StateTeamName;
                break;
            case TagTeamSymbol:
                State = XmlLeagueStates.StateTeamSymbol;
                break;
            case TagTeamNamesFile:
                State = XmlLeagueStates.StateTeamNamesFile;
                break;
            case TagTeamAverageTalent:
                State = XmlLeagueStates.StateTeamAverageTalent;
                break;
            case TagTeamDefFile:
                State = XmlLeagueStates.StateTeamDefFile;
                break;
            case TagDefProperty:
                State = XmlLeagueStates.StateProperty;
                break;
            case TagSkipWeeksWith:
                State = XmlLeagueStates.StateSkipWeeksWith;
                break;
            case TagDefWeekGap:
                State = XmlLeagueStates.StateWeekGap;
                break;
            case TagDefWeekBreak:
                State = XmlLeagueStates.StateWeekBreak;
                break;
            case TagDefYellowRed:
                State = XmlLeagueStates.StateYellowRed;
                break;
            case TagDefName:
                State = XmlLeagueStates.StateName;
                break;
            case TagDefShortName:
                State = XmlLeagueStates.StateShortName;
                break;
            case TagDefSid:
                State = XmlLeagueStates.StateSid;
                break;
            case TagDefSymbol:
                State = XmlLeagueStates.StateSymbol;
                break;
            default:
                Debug.DebugPrintMessage($"xml_league_read_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
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
    public static void XmlLeagueReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagDefName:
            case TagDefShortName:
            case TagDefSid:
            case TagDefSymbol:
            case TagLayer:
            case TagFirstWeek:
            case TagDefWeekGap:
            case TagDefWeekBreak:
            case TagDefSkipWeeksWith:
            case TagRoundRobins:
            case TagDefYellowRed:
            case TagAverageTalent:
            case TagNamesFile:
            case TagBreak:
            case TagJoinedLeague:
            case TagDefProperty:
            case TagNewTable:
            case TagTwoMatchWeekStart:
            case TagTwoMatchWeekEnd:
            case TagPromRel:
            case TagTeams:
                State = XmlLeagueStates.StateLeague;
                break;
            case TagPromGames:
            case TagPromRelElement:
                State = XmlLeagueStates.StatePromRel;
                break;
            case TagPromGamesDestSid:
            case TagPromGamesLoserSid:
            case TagPromGamesNumberOfAdvance:
            case TagPromGamesCupSid:
                State = XmlLeagueStates.StatePromGames;
                break;
            case TagPromRelElementRankStart:
            case TagPromRelElementRankEnd:
            case TagPromRelElementDestSid:
            case TagPromRelElementFromTable:
            case TagPromRelElementType:
                State = XmlLeagueStates.StatePromRelElement;
                break;
            case TagTeam:
                State = XmlLeagueStates.StateTeams;
                break;
            case TagTeamName:
            case TagTeamDefFile:
            case TagTeamAverageTalent:
            case TagTeamSymbol:
            case TagTeamNamesFile:
                State = XmlLeagueStates.StateTeam;
                break;
            default:
                Debug.DebugPrintMessage($"xml_league_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the variables (e.g. team names)
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void XmlLeagueReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlLeagueStates.StateName:
                NewLeague.Name = text;
                break;
            case XmlLeagueStates.StateShortName:
                NewLeague.ShortName = text;
                break;
            case XmlLeagueStates.StateSid:
                NewLeague.Sid = text;
                break;
            case XmlLeagueStates.StateSymbol:
                NewLeague.Symbol = text;
                break;
            case XmlLeagueStates.StateLayer:
                NewLeague.Layer = intValue;
                break;
            case XmlLeagueStates.StateFirstWeek:
                NewLeague.FirstWeek = intValue;
                break;
            case XmlLeagueStates.StateWeekGap:
                NewLeague.WeekGap = intValue;
                break;
            case XmlLeagueStates.StateWeekBreak:
                NewLeague.WeekBreaks[NewLeague.WeekBreaks.Count - 1].WeekNumber = intValue;
                break;
            case XmlLeagueStates.StateSkipWeeksWith:
                NewLeague.SkipWeeksWith.Add(text);
                break;
            case XmlLeagueStates.StateRoundRobins:
                NewLeague.RoundRobins = intValue;
                break;
            case XmlLeagueStates.StateYellowRed:
                NewLeague.YellowRed = intValue;
                break;
            case XmlLeagueStates.StateAverageTalent:
                NewLeague.AverageTalent = (floatValue / 10000f) * MaxPlayerSkill;
                break;
            case XmlLeagueStates.StateNamesFile:
                NewLeague.NamesFile = text;
                break;
            case XmlLeagueStates.StateBreak:
                foreach (string part in text.Split(','))
                {
                    if (int.TryParse(part, out int breakValue))
                        NewLeague.RrBreaks.Add(breakValue);
                }
                break;
            case XmlLeagueStates.StateProperty:
                NewLeague.Properties.Add(text);
                break;
            case XmlLeagueStates.StateJoinedLeague:
                NewLeague.JoinedLeagues[NewLeague.JoinedLeagues.Count - 1].Sid = text;
                break;
            case XmlLeagueStates.StateNewTable:
                NewLeague.NewTables[NewLeague.NewTables.Count - 1].AddWeek = intValue;
                break;
            case XmlLeagueStates.StateTwoMatchWeekStart:
                AddTwoMatchWeek(0, intValue);
                break;
            case XmlLeagueStates.StateTwoMatchWeekEnd:
                AddTwoMatchWeek(1, intValue);
                break;
            case XmlLeagueStates.StatePromGamesDestSid:
                NewLeague.PromRel.PromGames[NewLeague.PromRel.PromGames.Count - 1].DestSid = text;
                break;
            case XmlLeagueStates.StatePromGamesLoserSid:
                NewLeague.PromRel.PromGames[NewLeague.PromRel.PromGames.Count - 1].LoserSid = text;
                break;
            case XmlLeagueStates.StatePromGamesNumberOfAdvance:
                NewLeague.PromRel.PromGames[NewLeague.PromRel.PromGames.Count - 1].NumberOfAdvance = intValue;
                break;
            case XmlLeagueStates.StatePromGamesCupSid:
                NewLeague.PromRel.PromGames[NewLeague.PromRel.PromGames.Count - 1].CupSid = text;
                break;
            case XmlLeagueStates.StatePromRelElementRankStart:
                NewLeague.PromRel.Elements[NewLeague.PromRel.Elements.Count - 1].Ranks[0] = intValue;
                break;
            case XmlLeagueStates.StatePromRelElementRankEnd:
                NewLeague.PromRel.Elements[NewLeague.PromRel.Elements.Count - 1].Ranks[1] = intValue;
                break;
            case XmlLeagueStates.StatePromRelElementDestSid:
                NewLeague.PromRel.Elements[NewLeague.PromRel.Elements.Count - 1].DestSid = text;
                break;
            case XmlLeagueStates.StatePromRelElementFromTable:
                NewLeague.PromRel.Elements[NewLeague.PromRel.Elements.Count - 1].FromTable = intValue;
                break;
            case XmlLeagueStates.StatePromRelElementType:
            {
                var element = NewLeague.PromRel.Elements[NewLeague.PromRel.Elements.Count - 1];
                if (text == "promotion")
                    element.Type = PromRelType.Promotion;
                else if (text == "relegation")
                    element.Type = PromRelType.Relegation;
                else
                    element.Type = PromRelType.None;
                break;
            }
            case XmlLeagueStates.StateTeamName:
                NewLeague.Teams[NewLeague.Teams.Count - 1].Name = text;
                break;
            case XmlLeagueStates.StateTeamSymbol:
                NewLeague.Teams[NewLeague.Teams.Count - 1].Symbol = text;
                break;
            case XmlLeagueStates.StateTeamNamesFile:
                NewLeague.Teams[NewLeague.Teams.Count - 1].NamesFile = text;
                break;
            case XmlLeagueStates.StateTeamAverageTalent:
                NewLeague.Teams[NewLeague.Teams.Count - 1].AverageTalent = (floatValue / 10000f) * MaxPlayerSkill;
                break;
            case XmlLeagueStates.StateTeamDefFile:
                NewLeague.Teams[NewLeague.Teams.Count - 1].DefFile = text;
                break;
        }
    }

    /// <summary>
    /// Process an XML element and its children, starting with the
    /// XmlLeagueReadStartElement function, reading the text in between and
    /// finishing with the XmlLeagueReadEndElement function.
    /// </summary>
    /// <param name="element">The XML element to process.</param>
    private static void ProcessLeagueElement(XmlNode element)
    {
        XmlLeagueReadStartElement(element.LocalName, GetAttributesFromNode(element));

        foreach (XmlNode child in element.ChildNodes)
        {
            if (child.NodeType == XmlNodeType.Element)
                ProcessLeagueElement(child);
            else if (child.NodeType == XmlNodeType.Text)
                XmlLeagueReadText(child.InnerText);
        }

        XmlLeagueReadEndElement(element.LocalName);
    }

    /// <summary>
    /// Get the attributes of an XML element as a dictionary.
    /// </summary>
    /// <param name="element">The XML element.</param>
    /// <returns>The attributes of the element.</returns>
    private static IDictionary<string, string> GetAttributesFromNode(XmlNode element)
    {
        var attributes = new Dictionary<string, string>();
        if (element.Attributes != null)
        {
            foreach (XmlAttribute attribute in element.Attributes)
                attributes[attribute.Name] = attribute.Value;
        }
        return attributes;
    }

    /// <summary>
    /// Synchronise the number of round robin breaks with the number of
    /// round robins in the league. Corresponds to league_cup_adjust_rr_breaks.
    /// </summary>
    private static void AdjustRrBreaks()
    {
        for (int i = NewLeague.RrBreaks.Count - 1; i >= NewLeague.RoundRobins - 1; i--)
            NewLeague.RrBreaks.RemoveAt(i);

        int defaultBreak = NewLeague.RrBreaks.Count == 0
            ? NewLeague.WeekGap
            : NewLeague.RrBreaks[NewLeague.RrBreaks.Count - 1];

        for (int i = NewLeague.RrBreaks.Count; i < NewLeague.RoundRobins - 1; i++)
            NewLeague.RrBreaks.Add(defaultBreak);
    }

    /// <summary>
    /// Set the values of the week breaks to the week gap of the league
    /// if necessary. Corresponds to league_cup_adjust_week_breaks.
    /// </summary>
    private static void AdjustWeekBreaks()
    {
        for (int i = 0; i < NewLeague.WeekBreaks.Count; i++)
            if (NewLeague.WeekBreaks[i].Length == -1000)
                NewLeague.WeekBreaks[i].Length = NewLeague.WeekGap;
    }

    /// <summary>
    /// Add a value to one of the two two match week arrays of the league.
    /// Corresponds to the handling of the two_match_week_start and
    /// two_match_week_end tags.
    /// </summary>
    /// <param name="index">The index of the array, 0 or 1.</param>
    /// <param name="value">The value to add.</param>
    private static void AddTwoMatchWeek(int index, int value)
    {
        if (NewLeague.TwoMatchWeeks[index] == null)
            NewLeague.TwoMatchWeeks[index] = new List<int>();
        NewLeague.TwoMatchWeeks[index].Add(value);
    }
}
