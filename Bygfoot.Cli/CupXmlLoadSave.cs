using System.Xml;

namespace Bygfoot.Cli;

/// <summary>
/// Static class to handle the loading and saving of cup XML files.
/// Corresponds to the C module src/xml_loadsave_cup.h and src/xml_loadsave_cup.c
/// (xml_loadsave_cup_read, xml_loadsave_cup_write and the
/// xml_loadsave_cup_start_element, xml_loadsave_cup_end_element, xml_loadsave_cup_text,
/// xml_loadsave_cup_write_round and xml_loadsave_cup_write_choose_team functions).
/// </summary>
public static class CupXmlLoadSave
{
    // Common tags used in the cup xml files. Corresponds to the tag enum of src/xml.h.
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

    // Tags. Corresponds to the tag enum of the cup module in src/xml_loadsave_cup.c.
    private const int TagCup = 3000;
    private const int TagCupLastWeek = 3001;
    private const int TagCupAddWeek = 3002;
    private const int TagCupGroup = 3003;
    private const int TagCupTalentDiff = 3004;
    private const int TagCupChooseTeam = 3005;
    private const int TagCupChooseTeamSid = 3006;
    private const int TagCupChooseTeamNumberOfTeams = 3007;
    private const int TagCupChooseTeamStartIdx = 3008;
    private const int TagCupChooseTeamEndIdx = 3009;
    private const int TagCupChooseTeamRandomly = 3010;
    private const int TagCupChooseTeamGenerate = 3011;
    private const int TagCupChooseTeamSkipGroupCheck = 3012;
    private const int TagCupChooseTeamFromTable = 3013;
    private const int TagCupChooseTeamPreload = 3014;
    private const int TagCupRound = 3015;
    private const int TagCupRoundName = 3016;
    private const int TagCupRoundNewTeams = 3017;
    private const int TagCupRoundByes = 3018;
    private const int TagCupRoundTeamsFile = 3019;
    private const int TagCupRoundTeamPtrId = 3020;
    private const int TagCupRoundTableFile = 3021;
    private const int TagCupRoundHomeAway = 3022;
    private const int TagCupRoundReplay = 3023;
    private const int TagCupRoundNeutral = 3024;
    private const int TagCupRoundRandomiseTeams = 3025;
    private const int TagCupRoundRoundRobinNumberOfGroups = 3026;
    private const int TagCupRoundRoundRobinNumberOfAdvance = 3027;
    private const int TagCupRoundRoundRobinNumberOfBestAdvance = 3028;
    private const int TagCupRoundRoundRobins = 3029;
    private const int TagCupRoundBreak = 3030;
    private const int TagCupRoundWaitCup = 3031;
    private const int TagCupRoundWaitRound = 3032;
    private const int TagCupRoundTwoMatchWeekStart = 3033;
    private const int TagCupRoundTwoMatchWeekEnd = 3034;
    private const int TagCupRoundTwoMatchWeek = 3035;
    private const int TagCupTeamIdBye = 3036;
    private const int TagCupTeamName = 3037;
    private const int TagCupNextFixtureUpdateWeek = 3038;
    private const int TagCupNextFixtureUpdateWeekRound = 3039;
    private const int TagCupRoundDelay = 3040;
    private const int TagEnd = 3041;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the cup module and the common tags of src/xml.h.
    /// </summary>
    public enum XmlCupLoadSaveStates
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
        TagCup = 3000,
        TagCupLastWeek,
        TagCupAddWeek,
        TagCupGroup,
        TagCupTalentDiff,
        TagCupChooseTeam,
        TagCupChooseTeamSid,
        TagCupChooseTeamNumberOfTeams,
        TagCupChooseTeamStartIdx,
        TagCupChooseTeamEndIdx,
        TagCupChooseTeamRandomly,
        TagCupChooseTeamGenerate,
        TagCupChooseTeamSkipGroupCheck,
        TagCupChooseTeamFromTable,
        TagCupChooseTeamPreload,
        TagCupRound,
        TagCupRoundName,
        TagCupRoundNewTeams,
        TagCupRoundByes,
        TagCupRoundTeamsFile,
        TagCupRoundTeamPtrId,
        TagCupRoundTableFile,
        TagCupRoundHomeAway,
        TagCupRoundReplay,
        TagCupRoundNeutral,
        TagCupRoundRandomiseTeams,
        TagCupRoundRoundRobinNumberOfGroups,
        TagCupRoundRoundRobinNumberOfAdvance,
        TagCupRoundRoundRobinNumberOfBestAdvance,
        TagCupRoundRoundRobins,
        TagCupRoundBreak,
        TagCupRoundWaitCup,
        TagCupRoundWaitRound,
        TagCupRoundTwoMatchWeekStart,
        TagCupRoundTwoMatchWeekEnd,
        TagCupRoundTwoMatchWeek,
        TagCupTeamIdBye,
        TagCupTeamName,
        TagCupNextFixtureUpdateWeek,
        TagCupNextFixtureUpdateWeekRound,
        TagCupRoundDelay,
        TagEnd
    }

    private static XmlCupLoadSaveStates State = XmlCupLoadSaveStates.TagCup;
    private static Cup NewCup = new Cup();
    private static CupChooseTeam NewChooseTeam = new CupChooseTeam();
    private static CupRound NewRound = new CupRound();
    private static string DirName = "";
    private static WeekBreak NewWeekBreak = new WeekBreak();
    private static CupRoundWait NewWait = new CupRoundWait();

    /// <summary>
    /// Function reading a cup xml file and filling in the given cup.
    /// @param filename The name of the cup xml file to be read.
    /// @param cup The cup to be read into.
    /// </summary>
    /// <param name="filename">The name of the cup xml file to read.</param>
    /// <param name="cup">The cup to be read into.</param>
    public static void CupXmlLoadSaveRead(string filename, Cup cup)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
        {
            Debug.DebugPrintMessage($"xml_loadsave_cup_read: error reading file {filename}");
            return;
        }

        State = XmlCupLoadSaveStates.TagCup;
        NewCup = cup;
        DirName = Path.GetDirectoryName(filename);

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
                            CupXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            CupXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            CupXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        CupXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        CupXmlLoadSaveReadText(reader.ReadString());
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
    public static void CupXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void CupXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the cup XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);
        bool validTag = false;

        // The cup module tags and the common tags of src/xml.h are valid tags
        // (corresponds to the two tag loops in C's xml_loadsave_cup_start_element).
        if (tag >= TagCup && tag < TagEnd)
        {
            State = (XmlCupLoadSaveStates)tag;
            validTag = true;
        }
        else if (tag >= TagName && tag <= TagRound)
        {
            State = (XmlCupLoadSaveStates)tag;
            validTag = true;
        }

        if (tag == TagCupChooseTeam)
            NewChooseTeam = Cup.CupChooseTeamNew();
        else if (tag == TagCupRound)
            NewRound = Cup.CupRoundNew();

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_cup_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the cup module
    /// are the numbers following the underscore in the tag names (e.g. '_3000').
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
    /// The state variable is changed in this function and
    /// the new round gets added to the cup.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void CupXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagName ||
            tag == TagShortName ||
            tag == TagSymbol ||
            tag == TagSid ||
            tag == TagId ||
            tag == TagYellowRed ||
            tag == TagWeekGap ||
            tag == TagWeekBreak ||
            tag == TagWeekBreakLength ||
            tag == TagProperty ||
            tag == TagCupLastWeek ||
            tag == TagCupAddWeek ||
            tag == TagCupGroup ||
            tag == TagCupTalentDiff ||
            tag == TagCupNextFixtureUpdateWeek ||
            tag == TagCupNextFixtureUpdateWeekRound ||
            tag == TagCupTeamName ||
            tag == TagCupTeamIdBye)
        {
            State = XmlCupLoadSaveStates.TagCup;
        }
        else if (tag == TagCupRound)
        {
            State = XmlCupLoadSaveStates.TagCup;
            NewCup.Rounds.Add(NewRound);
        }
        else if (tag == TagCupChooseTeam)
        {
            State = XmlCupLoadSaveStates.TagCupRound;
            NewRound.ChooseTeams.Add(NewChooseTeam);
        }
        else if (tag == TagCupChooseTeamNumberOfTeams ||
                 tag == TagCupChooseTeamStartIdx ||
                 tag == TagCupChooseTeamEndIdx ||
                 tag == TagCupChooseTeamSid ||
                 tag == TagCupChooseTeamGenerate ||
                 tag == TagCupChooseTeamSkipGroupCheck ||
                 tag == TagCupChooseTeamFromTable ||
                 tag == TagCupChooseTeamPreload ||
                 tag == TagCupChooseTeamRandomly)
        {
            State = XmlCupLoadSaveStates.TagCupChooseTeam;
        }
        else if (tag == TagCupRoundHomeAway ||
                 tag == TagCupRoundTeamsFile ||
                 tag == TagCupRoundTeamPtrId ||
                 tag == TagCupRoundTableFile ||
                 tag == TagCupRoundNewTeams ||
                 tag == TagCupRoundName ||
                 tag == TagCupRoundByes ||
                 tag == TagCupRoundReplay ||
                 tag == TagCupRoundNeutral ||
                 tag == TagCupRoundDelay ||
                 tag == TagCupRoundRandomiseTeams ||
                 tag == TagCupRoundRoundRobinNumberOfGroups ||
                 tag == TagCupRoundRoundRobinNumberOfAdvance ||
                 tag == TagCupRoundRoundRobinNumberOfBestAdvance ||
                 tag == TagCupRoundRoundRobins ||
                 tag == TagCupRoundBreak ||
                 tag == TagCupRoundWaitCup ||
                 tag == TagCupRoundWaitRound ||
                 tag == TagCupRoundTwoMatchWeekStart ||
                 tag == TagCupRoundTwoMatchWeekEnd ||
                 tag == TagCupRoundTwoMatchWeek)
        {
            State = XmlCupLoadSaveStates.TagCupRound;
        }
        else if (tag != TagCup)
        {
            Debug.DebugPrintMessage($"xml_loadsave_cup_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the cup variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void CupXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double value) ? value : 0);
        float floatValue = (float)(double.TryParse(text, out value) ? value / 10000 : 0);

        switch (State)
        {
            case XmlCupLoadSaveStates.TagName:
                NewCup.Name = text;
                break;
            case XmlCupLoadSaveStates.TagShortName:
                NewCup.ShortName = text;
                break;
            case XmlCupLoadSaveStates.TagSymbol:
                NewCup.Symbol = text;
                break;
            case XmlCupLoadSaveStates.TagSid:
                NewCup.Sid = text;
                break;
            case XmlCupLoadSaveStates.TagId:
                NewCup.Id = intValue;
                break;
            case XmlCupLoadSaveStates.TagWeekGap:
                NewCup.WeekGap = intValue;
                break;
            case XmlCupLoadSaveStates.TagWeekBreak:
                NewWeekBreak = new WeekBreak { WeekNumber = intValue };
                break;
            case XmlCupLoadSaveStates.TagWeekBreakLength:
                NewWeekBreak.Length = intValue;
                NewCup.WeekBreaks.Add(NewWeekBreak);
                break;
            case XmlCupLoadSaveStates.TagYellowRed:
                NewCup.YellowRed = intValue;
                break;
            case XmlCupLoadSaveStates.TagProperty:
                NewCup.Properties.Add(text);
                break;
            case XmlCupLoadSaveStates.TagCupLastWeek:
                NewCup.LastWeek = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupAddWeek:
                NewCup.AddWeek = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupGroup:
                NewCup.Group = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupTalentDiff:
                NewCup.TalentDiff = floatValue;
                break;
            case XmlCupLoadSaveStates.TagCupNextFixtureUpdateWeek:
                NewCup.NextFixtureUpdateWeek = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupNextFixtureUpdateWeekRound:
                NewCup.NextFixtureUpdateWeekRound = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupTeamIdBye:
                NewCup.Bye.Add(intValue);
                break;
            case XmlCupLoadSaveStates.TagCupTeamName:
                NewCup.TeamNames.Add(text);
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamSid:
                NewChooseTeam.Sid = text;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamNumberOfTeams:
                NewChooseTeam.NumberOfTeams = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamStartIdx:
                NewChooseTeam.StartIdx = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamEndIdx:
                NewChooseTeam.EndIdx = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamRandomly:
                NewChooseTeam.Randomly = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamGenerate:
                NewChooseTeam.Generate = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamSkipGroupCheck:
                NewChooseTeam.SkipGroupCheck = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamFromTable:
                NewChooseTeam.FromTable = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupChooseTeamPreload:
                NewChooseTeam.Preload = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupRoundHomeAway:
                NewRound.HomeAway = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupRoundNewTeams:
                NewRound.NewTeams = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundName:
                NewRound.Name = text;
                break;
            case XmlCupLoadSaveStates.TagCupRoundByes:
                NewRound.Byes = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundTeamsFile:
            {
                List<Team> teams = new List<Team>();
                TeamsXmlLoadSave.TeamsXmlLoadSaveRead(Path.Combine(DirName, text), teams);

                foreach (Team team in teams)
                {
                    NewRound.Teams.Add(team.Id);
                    NewCup.Teams.Add(team.Id);
                }
                break;
            }
            case XmlCupLoadSaveStates.TagCupRoundTeamPtrId:
                NewRound.TeamPtrs.Add(intValue);
                break;
            case XmlCupLoadSaveStates.TagCupRoundTableFile:
            {
                Table newTable = new Table();
                TableXmlLoadSave.TableXmlLoadSaveRead(Path.Combine(DirName, text), newTable);
                NewRound.Tables.Add(newTable);
                break;
            }
            case XmlCupLoadSaveStates.TagCupRoundReplay:
                NewRound.Replay = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundNeutral:
                NewRound.Neutral = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupRoundDelay:
                NewRound.Delay = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundRandomiseTeams:
                NewRound.RandomiseTeams = intValue != 0;
                break;
            case XmlCupLoadSaveStates.TagCupRoundRoundRobinNumberOfGroups:
                NewRound.RoundRobinNumberOfGroups = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundRoundRobinNumberOfAdvance:
                NewRound.RoundRobinNumberOfAdvance = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundRoundRobinNumberOfBestAdvance:
                NewRound.RoundRobinNumberOfBestAdvance = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundRoundRobins:
                NewRound.RoundRobins = intValue;
                break;
            case XmlCupLoadSaveStates.TagCupRoundBreak:
                NewRound.RrBreaks.Add(intValue);
                break;
            case XmlCupLoadSaveStates.TagCupRoundWaitCup:
                NewWait.CupSid = text;
                break;
            case XmlCupLoadSaveStates.TagCupRoundWaitRound:
                NewWait.CupRound = intValue;
                NewRound.Waits.Add(NewWait);
                break;
            case XmlCupLoadSaveStates.TagCupRoundTwoMatchWeekStart:
                NewRound.TwoMatchWeeks[0].Add(intValue);
                break;
            case XmlCupLoadSaveStates.TagCupRoundTwoMatchWeekEnd:
                NewRound.TwoMatchWeeks[1].Add(intValue);
                break;
            case XmlCupLoadSaveStates.TagCupRoundTwoMatchWeek:
                NewRound.TwoMatchWeek = intValue != 0;
                break;
        }
    }

    /// <summary>
    /// Write the cup and its fixtures into the xml files with the given prefix.
    /// @param prefix The prefix of the cup xml file (e.g. 'user_0').
    /// @param cup The cup to write.
    /// </summary>
    /// <param name="prefix">The prefix of the cup xml file (e.g. 'user_0').</param>
    /// <param name="cup">The cup to write.</param>
    public static void CupXmlLoadSaveWrite(string prefix, Cup cup)
    {
        string fixturesFileName = $"{prefix}___cup_{cup.Id}_fixtures.xml";
        FixturesXmlLoadSave.FixturesXmlLoadSaveWrite(fixturesFileName, cup.Fixtures);

        using (var fil = new StreamWriter($"{prefix}___cup_{cup.Id}.xml"))
        {
            fil.WriteLine("<_3000>");

            if (cup.Name != null)
                fil.WriteLine("<_0>" + cup.Name + "</_0>");
            if (cup.ShortName != null)
                fil.WriteLine("<_3>" + cup.ShortName + "</_3>");
            if (cup.Sid != null)
                fil.WriteLine("<_2>" + cup.Sid + "</_2>");
            if (cup.Symbol != null)
                fil.WriteLine("<_1>" + cup.Symbol + "</_1>");

            fil.WriteLine("<_4>" + cup.Id + "</_4>");
            fil.WriteLine("<_3001>" + cup.LastWeek + "</_3001>");
            fil.WriteLine("<_3002>" + cup.AddWeek + "</_3002>");
            fil.WriteLine("<_3003>" + cup.Group + "</_3003>");
            fil.WriteLine("<_5>" + cup.WeekGap + "</_5>");
            fil.WriteLine("<_9>" + cup.YellowRed + "</_9>");
            fil.WriteLine("<_3004>" + (int)(cup.TalentDiff * 10000) + "</_3004>");
            fil.WriteLine("<_3038>" + cup.NextFixtureUpdateWeek + "</_3038>");
            fil.WriteLine("<_3039>" + cup.NextFixtureUpdateWeekRound + "</_3039>");

            foreach (string property in cup.Properties)
            {
                fil.WriteLine("<_12>" + property + "</_12>");
            }

            for (int i = 0; i < cup.Rounds.Count; i++)
            {
                CupXmlLoadSaveWriteRound(fil, prefix, cup, i);
            }

            foreach (WeekBreak weekBreak in cup.WeekBreaks)
            {
                fil.WriteLine("<_6>" + weekBreak.WeekNumber + "</_6>");
                fil.WriteLine("<_7>" + weekBreak.Length + "</_7>");
            }

            foreach (int bye in cup.Bye)
            {
                fil.WriteLine("<_3036>" + bye + "</_3036>");
            }

            foreach (string teamName in cup.TeamNames)
            {
                fil.WriteLine("<_3037>" + teamName + "</_3037>");
            }

            fil.WriteLine("</_3000>");
        }
    }

    /// <summary>
    /// Write a cup round into the file. The teams and the tables of the cup round
    /// get written into separate files.
    /// @param fil The file to write into.
    /// @param prefix The prefix of the cup xml file (e.g. 'user_0').
    /// @param cup The cup.
    /// @param round The index of the cup round in the cup rounds array.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="prefix">The prefix of the cup xml file (e.g. 'user_0').</param>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The index of the cup round in the cup rounds array.</param>
    public static void CupXmlLoadSaveWriteRound(TextWriter fil, string prefix, Cup cup, int round)
    {
        string buf = "";
        CupRound cupRound = cup.Rounds[round];
        string basename = Path.GetFileName(prefix);

        fil.WriteLine("<_3015>");

        if (cupRound.Teams.Count > 0)
        {
            buf = $"{basename}___cup_{cup.Id}_round_{round:D2}_teams.xml";
            fil.WriteLine("\t<_3019>" + buf + "</_3019>");

            buf = $"{prefix}___cup_{cup.Id}_round_{round:D2}_teams.xml";
            List<Team> teams = new List<Team>();
            foreach (int teamId in cupRound.Teams)
            {
                Team team = Team.OfId(teamId);
                if (team != null)
                    teams.Add(team);
            }
            TeamsXmlLoadSave.TeamsXmlLoadSaveWrite(buf, teams);
        }

        if (cupRound.Name != null)
            fil.WriteLine("\t<_3016>" + cupRound.Name + "</_3016>");
        fil.WriteLine("\t<_3017>" + cupRound.NewTeams + "</_3017>");
        fil.WriteLine("\t<_3018>" + cupRound.Byes + "</_3018>");
        fil.WriteLine("\t<_3022>" + (cupRound.HomeAway ? 1 : 0) + "</_3022>");
        fil.WriteLine("\t<_3023>" + cupRound.Replay + "</_3023>");
        fil.WriteLine("\t<_3024>" + (cupRound.Neutral ? 1 : 0) + "</_3024>");
        fil.WriteLine("\t<_3040>" + cupRound.Delay + "</_3040>");
        fil.WriteLine("\t<_3025>" + (cupRound.RandomiseTeams ? 1 : 0) + "</_3025>");
        fil.WriteLine("\t<_3026>" + cupRound.RoundRobinNumberOfGroups + "</_3026>");
        fil.WriteLine("\t<_3027>" + cupRound.RoundRobinNumberOfAdvance + "</_3027>");
        fil.WriteLine("\t<_3028>" + cupRound.RoundRobinNumberOfBestAdvance + "</_3028>");
        fil.WriteLine("\t<_3029>" + cupRound.RoundRobins + "</_3029>");

        foreach (int rrBreak in cupRound.RrBreaks)
        {
            fil.WriteLine("\t<_3030>" + rrBreak + "</_3030>");
        }

        foreach (CupRoundWait wait in cupRound.Waits)
        {
            if (wait.CupSid != null)
                fil.WriteLine("\t<_3031>" + wait.CupSid + "</_3031>");
            fil.WriteLine("\t<_3032>" + wait.CupRound + "</_3032>");
        }

        fil.WriteLine("\t<_3035>" + (cupRound.TwoMatchWeek ? 1 : 0) + "</_3035>");

        for (int i = 0; i < cupRound.TwoMatchWeeks[0].Count; i++)
        {
            // The C code writes both the start and the end value from
            // the two_match_weeks[0] array.
            fil.WriteLine("\t<_3033>" + cupRound.TwoMatchWeeks[0][i] + "</_3033>");
            fil.WriteLine("\t<_3034>" + cupRound.TwoMatchWeeks[0][i] + "</_3034>");
        }

        for (int i = 0; i < cupRound.Tables.Count; i++)
        {
            buf = $"{basename}___cup_{cup.Id}_round_{round:D2}_table_{i:D2}.xml";
            fil.WriteLine("\t<_3021>" + buf + "</_3021>");

            buf = $"{prefix}___cup_{cup.Id}_round_{round:D2}_table_{i:D2}.xml";
            TableXmlLoadSave.TableXmlLoadSaveWrite(buf, cupRound.Tables[i]);
        }

        foreach (CupChooseTeam chooseTeam in cupRound.ChooseTeams)
        {
            CupXmlLoadSaveWriteChooseTeam(fil, chooseTeam);
        }

        foreach (int teamPtrId in cupRound.TeamPtrs)
        {
            fil.WriteLine("\t<_3020>" + teamPtrId + "</_3020>");
        }

        fil.WriteLine("</_3015>");
    }

    /// <summary>
    /// Write a choose_team definition into the file.
    /// @param fil The file to write into.
    /// @param chooseTeam The choose_team definition to write.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="chooseTeam">The choose_team definition to write.</param>
    public static void CupXmlLoadSaveWriteChooseTeam(TextWriter fil, CupChooseTeam chooseTeam)
    {
        fil.WriteLine("\t<_3005>");

        if (chooseTeam.Sid != null)
            fil.WriteLine("\t\t<_3006>" + chooseTeam.Sid + "</_3006>");
        fil.WriteLine("\t\t<_3007>" + chooseTeam.NumberOfTeams + "</_3007>");
        fil.WriteLine("\t\t<_3008>" + chooseTeam.StartIdx + "</_3008>");
        fil.WriteLine("\t\t<_3009>" + chooseTeam.EndIdx + "</_3009>");
        fil.WriteLine("\t\t<_3010>" + (chooseTeam.Randomly ? 1 : 0) + "</_3010>");
        fil.WriteLine("\t\t<_3011>" + (chooseTeam.Generate ? 1 : 0) + "</_3011>");
        fil.WriteLine("\t\t<_3012>" + (chooseTeam.SkipGroupCheck ? 1 : 0) + "</_3012>");
        fil.WriteLine("\t\t<_3013>" + chooseTeam.FromTable + "</_3013>");
        fil.WriteLine("\t\t<_3014>" + (chooseTeam.Preload ? 1 : 0) + "</_3014>");

        fil.WriteLine("\t</_3005>");
    }
}
