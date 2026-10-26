namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of cup XML files.
/// Corresponds to the C module src/xml_cup.h and src/xml_cup.c
/// (xml_cup_read, xml_cup_read_start_element, xml_cup_read_end_element
/// and xml_cup_read_text).
/// </summary>
public static class CupXml
{
    private const string TagCup = "cup";
    private const string TagGroup = "group";
    private const string TagLastWeek = "last_week";
    private const string TagAddWeek = "add_week";
    private const string TagTalentDiff = "talent_diff";
    private const string TagCupRounds = "cup_rounds";
    private const string TagCupRound = "cup_round";
    private const string TagCupRoundName = "round_name";
    private const string TagCupRoundNewTeams = "new_teams";
    private const string TagCupRoundByes = "byes";
    private const string TagCupRoundHomeAway = "home_away";
    private const string TagCupRoundReplay = "replay";
    private const string TagCupRoundNeutral = "neutral";
    private const string TagCupRoundDelay = "delay";
    private const string TagCupRoundRandomiseTeams = "randomise_teams";
    private const string TagCupRoundNumberOfGroups = "number_of_groups";
    private const string TagCupRoundNumberOfAdvance = "number_of_advance";
    private const string TagCupRoundNumberOfBestAdvance = "number_of_best_advance";
    private const string TagCupRoundRoundRobins = "round_robins";
    private const string TagCupRoundBreak = "break";
    private const string TagCupRoundWait = "wait_for_cup";
    private const string TagCupRoundTwoMatchWeekStart = "two_match_week_start";
    private const string TagCupRoundTwoMatchWeekEnd = "two_match_week_end";
    private const string TagCupRoundTwoMatchWeek = "two_match_week";
    private const string TagChooseTeams = "choose_teams";
    private const string TagChooseTeam = "choose_team";
    private const string TagChooseTeamSid = "choose_team_sid";
    private const string TagChooseTeamNumberOfTeams = "number_of_teams";
    private const string TagChooseTeamStartIdx = "start_idx";
    private const string TagChooseTeamEndIdx = "end_idx";
    private const string TagChooseTeamRandomly = "randomly";
    private const string TagChooseTeamGenerate = "generate";
    private const string TagChooseTeamSkipGroupCheck = "skip_group_check";
    private const string TagChooseTeamFromTable = "from_table";
    private const string TagChooseTeamPreload = "preload";
    private const string TagDefName = "name";
    private const string TagDefShortName = "short_name";
    private const string TagDefSymbol = "symbol";
    private const string TagDefSid = "sid";
    private const string TagDefProperty = "property";
    private const string TagDefWeekGap = "week_gap";
    private const string TagDefWeekBreak = "break_in";
    private const string TagDefSkipWeeksWith = "skip_weeks_with";
    private const string TagSkipWeeksWith = "skip_weeks_with";
    private const string TagDefYellowRed = "yellow_red";
    private const string TagWeekGap = "week_gap";
    private const string TagYellowRed = "yellow_red";
    private const string TagProperty = "property";

    private const string AttNameCupRoundWaitRound = "round";

    private const float MaxTalentDiffDivisor = 10000f;

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlCupStates.
    /// </summary>
    public enum XmlCupStates
    {
        StateCup = 0,
        StateName,
        StateShortName,
        StateSymbol,
        StateSid,
        StateGroup,
        StateProperty,
        StateLastWeek,
        StateAddWeek,
        StateWeekGap,
        StateWeekBreak,
        StateSkipWeeksWith,
        StateYellowRed,
        StateTalentDiff,
        StateCupRounds,
        StateCupRound,
        StateCupRoundName,
        StateCupRoundNewTeams,
        StateCupRoundByes,
        StateCupRoundHomeAway,
        StateCupRoundReplay,
        StateCupRoundNeutral,
        StateCupRoundDelay,
        StateCupRoundRandomiseTeams,
        StateCupRoundNumberOfGroups,
        StateCupRoundNumberOfAdvance,
        StateCupRoundNumberOfBestAdvance,
        StateCupRoundRoundRobins,
        StateCupRoundBreak,
        StateCupRoundWait,
        StateCupRoundTwoMatchWeekStart,
        StateCupRoundTwoMatchWeekEnd,
        StateCupRoundTwoMatchWeek,
        StateChooseTeams,
        StateChooseTeam,
        StateChooseTeamSid,
        StateChooseTeamNumberOfTeams,
        StateChooseTeamStartIdx,
        StateChooseTeamEndIdx,
        StateChooseTeamRandomly,
        StateChooseTeamGenerate,
        StateChooseTeamSkipGroupCheck,
        StateChooseTeamFromTable,
        StateChooseTeamPreload,
        StateEnd
    }

    private static XmlCupStates State = XmlCupStates.StateCup;
    private static Cup NewCup = new Cup();
    private static CupRound NewRound = new CupRound();
    private static CupChooseTeam NewChooseTeam = new CupChooseTeam();
    private static WeekBreak NewWeekBreak = new WeekBreak();
    private static CupRoundWait NewWait = new CupRoundWait();

    /// <summary>
    /// Function reading an XML file specifying a cup.
    /// A new cup struct is filled in and appended to the
    /// IList of cups.
    /// @param cupName name of the xml file ( e.g. 'cup_england_fa.xml')
    /// to be read. Full path is not necessary, if the file is located in
    /// one of the support directories; neither are the prefix 'cup_'
    /// or the suffix '.xml'.
    /// @param cups The list we append the new cup to.
    /// </summary>
    /// <param name="cupName">Name of the cup xml file.</param>
    /// <param name="cups">The list to append the new cup to.</param>
    public static void XmlCupRead(string cupName, IList<Cup> cups)
    {
        string fileName = FileHelper.FindSupportFile(cupName, false);
        if (fileName == null)
            fileName = FileHelper.FindSupportFile($"cup_{cupName}.xml", true);
        if (string.IsNullOrEmpty(fileName))
            return;

        State = XmlCupStates.StateCup;
        NewCup = new Cup();
        NewRound = new CupRound();
        NewChooseTeam = new CupChooseTeam();
        NewWeekBreak = new WeekBreak();
        NewWait = new CupRoundWait();

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
                            XmlCupReadStartElement(reader, reader.LocalName);
                            XmlCupReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlCupReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlCupReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlCupReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }

        NewCup.Id = Bygfoot.CupIdNew();

        cups.Add(NewCup);
    }

    /// <summary>
    /// Function reading a cup from an XML node.
    /// A new cup struct is filled in and returned.
    /// @param node The XML node of the cup to be read.
    /// </summary>
    /// <param name="node">The cup XML node.</param>
    /// <returns>The cup we read.</returns>
    public static Cup XmlCupReadFromNode(XmlNode node)
    {
        State = XmlCupStates.StateCup;
        NewCup = new Cup();
        NewRound = new CupRound();
        NewChooseTeam = new CupChooseTeam();
        NewWeekBreak = new WeekBreak();
        NewWait = new CupRoundWait();

        ProcessCupElement(node);

        NewCup.Id = Bygfoot.CupIdNew();

        return NewCup;
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void XmlCupReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlCupReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the cup XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagCup:
                NewCup = new Cup();
                State = XmlCupStates.StateCup;
                break;
            case TagGroup:
                State = XmlCupStates.StateGroup;
                break;
            case TagLastWeek:
                State = XmlCupStates.StateLastWeek;
                break;
            case TagAddWeek:
                State = XmlCupStates.StateAddWeek;
                break;
            case TagTalentDiff:
                State = XmlCupStates.StateTalentDiff;
                break;
            case TagCupRounds:
                State = XmlCupStates.StateCupRounds;
                break;
            case TagCupRound:
                State = XmlCupStates.StateCupRound;
                NewRound = new CupRound();
                break;
            case TagCupRoundName:
                State = XmlCupStates.StateCupRoundName;
                break;
            case TagCupRoundNewTeams:
                State = XmlCupStates.StateCupRoundNewTeams;
                break;
            case TagCupRoundByes:
                State = XmlCupStates.StateCupRoundByes;
                break;
            case TagCupRoundHomeAway:
                State = XmlCupStates.StateCupRoundHomeAway;
                break;
            case TagCupRoundReplay:
                State = XmlCupStates.StateCupRoundReplay;
                break;
            case TagCupRoundNeutral:
                State = XmlCupStates.StateCupRoundNeutral;
                break;
            case TagCupRoundDelay:
                State = XmlCupStates.StateCupRoundDelay;
                break;
            case TagCupRoundRandomiseTeams:
                State = XmlCupStates.StateCupRoundRandomiseTeams;
                break;
            case TagCupRoundNumberOfGroups:
                State = XmlCupStates.StateCupRoundNumberOfGroups;
                break;
            case TagCupRoundNumberOfAdvance:
                State = XmlCupStates.StateCupRoundNumberOfAdvance;
                break;
            case TagCupRoundNumberOfBestAdvance:
                State = XmlCupStates.StateCupRoundNumberOfBestAdvance;
                break;
            case TagCupRoundRoundRobins:
                State = XmlCupStates.StateCupRoundRoundRobins;
                break;
            case TagCupRoundBreak:
                State = XmlCupStates.StateCupRoundBreak;
                break;
            case TagCupRoundWait:
                State = XmlCupStates.StateCupRoundWait;
                string roundAttribute = getAttribute(AttNameCupRoundWaitRound);
                NewWait.CupRound = (roundAttribute != null && int.TryParse(roundAttribute, out int cupRound))
                    ? cupRound - 1
                    : -1;
                break;
            case TagCupRoundTwoMatchWeekStart:
                State = XmlCupStates.StateCupRoundTwoMatchWeekStart;
                break;
            case TagCupRoundTwoMatchWeekEnd:
                State = XmlCupStates.StateCupRoundTwoMatchWeekEnd;
                break;
            case TagCupRoundTwoMatchWeek:
                State = XmlCupStates.StateCupRoundTwoMatchWeek;
                break;
            case TagChooseTeams:
                State = XmlCupStates.StateChooseTeams;
                break;
            case TagChooseTeam:
                State = XmlCupStates.StateChooseTeam;
                NewChooseTeam = new CupChooseTeam();
                break;
            default:
                Debug.DebugPrintMessage($"xml_cup_read_start_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void XmlCupReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagDefName:
            case TagDefShortName:
            case TagDefSymbol:
            case TagDefSid:
            case TagGroup:
            case TagProperty:
            case TagLastWeek:
            case TagAddWeek:
            case TagWeekGap:
            case TagDefWeekBreak:
            case TagSkipWeeksWith:
            case TagDefYellowRed:
            case TagTalentDiff:
            case TagCupRounds:
                State = XmlCupStates.StateCup;
                break;
            case TagCupRound:
                State = XmlCupStates.StateCupRounds;
                if (!NewRound.HomeAway)
                    NewRound.RoundRobins = 1;
                for (int i = NewRound.RrBreaks.Count - 1; i >= NewRound.RoundRobins - 1; i--)
                    NewRound.RrBreaks.RemoveAt(i);
                int cupRoundDefaultBreak = NewRound.RrBreaks.Count == 0
                    ? NewCup.WeekGap
                    : NewRound.RrBreaks[NewRound.RrBreaks.Count - 1];
                for (int i = NewRound.RrBreaks.Count; i < NewRound.RoundRobins - 1; i++)
                    NewRound.RrBreaks.Add(cupRoundDefaultBreak);
                NewCup.Rounds.Add(NewRound);
                break;
            case TagCupRoundHomeAway:
            case TagCupRoundReplay:
            case TagCupRoundNeutral:
            case TagCupRoundDelay:
            case TagCupRoundRandomiseTeams:
            case TagCupRoundNumberOfGroups:
            case TagCupRoundNumberOfAdvance:
            case TagCupRoundNumberOfBestAdvance:
            case TagCupRoundRoundRobins:
            case TagCupRoundBreak:
            case TagCupRoundWait:
            case TagCupRoundTwoMatchWeekStart:
            case TagCupRoundTwoMatchWeekEnd:
            case TagCupRoundTwoMatchWeek:
            case TagCupRoundNewTeams:
            case TagCupRoundName:
            case TagCupRoundByes:
            case TagChooseTeams:
                State = XmlCupStates.StateCupRound;
                if (elementName == TagCupRoundWait)
                    NewRound.Waits.Add(NewWait);
                break;
            case TagChooseTeam:
                State = XmlCupStates.StateChooseTeams;
                NewRound.ChooseTeams.Add(NewChooseTeam);
                break;
            case TagChooseTeamSid:
            case TagChooseTeamNumberOfTeams:
            case TagChooseTeamStartIdx:
            case TagChooseTeamEndIdx:
            case TagChooseTeamRandomly:
            case TagChooseTeamGenerate:
            case TagChooseTeamSkipGroupCheck:
            case TagChooseTeamFromTable:
            case TagChooseTeamPreload:
                State = XmlCupStates.StateChooseTeam;
                break;
            case TagCup:
                break;
            default:
                Debug.DebugPrintMessage($"xml_cup_read_end_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void XmlCupReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlCupStates.StateName:
                NewCup.Name = text;
                break;
            case XmlCupStates.StateShortName:
                NewCup.ShortName = text;
                break;
            case XmlCupStates.StateSymbol:
                NewCup.Symbol = text;
                break;
            case XmlCupStates.StateSid:
                NewCup.Sid = text;
                break;
            case XmlCupStates.StateGroup:
                NewCup.Group = intValue;
                break;
            case XmlCupStates.StateProperty:
                NewCup.Properties.Add(text);
                break;
            case XmlCupStates.StateLastWeek:
                NewCup.LastWeek = intValue;
                break;
            case XmlCupStates.StateAddWeek:
                NewCup.AddWeek = intValue;
                break;
            case XmlCupStates.StateWeekGap:
                NewCup.WeekGap = intValue;
                break;
            case XmlCupStates.StateWeekBreak:
                NewWeekBreak.WeekNumber = intValue;
                break;
            case XmlCupStates.StateSkipWeeksWith:
                NewCup.SkipWeeksWith.Add(text);
                break;
            case XmlCupStates.StateYellowRed:
                NewCup.YellowRed = intValue;
                break;
            case XmlCupStates.StateTalentDiff:
                NewCup.TalentDiff = (floatValue / MaxTalentDiffDivisor);
                break;
            case XmlCupStates.StateCupRoundName:
                NewRound.Name = text;
                break;
            case XmlCupStates.StateCupRoundNewTeams:
                NewRound.NewTeams = intValue;
                break;
            case XmlCupStates.StateCupRoundByes:
                NewRound.Byes = intValue;
                break;
            case XmlCupStates.StateCupRoundHomeAway:
                NewRound.HomeAway = intValue != 0;
                break;
            case XmlCupStates.StateCupRoundReplay:
                NewRound.Replay = intValue;
                break;
            case XmlCupStates.StateCupRoundNeutral:
                NewRound.Neutral = intValue != 0;
                break;
            case XmlCupStates.StateCupRoundDelay:
                NewRound.Delay = intValue;
                break;
            case XmlCupStates.StateCupRoundRandomiseTeams:
                NewRound.RandomiseTeams = intValue != 0;
                break;
            case XmlCupStates.StateCupRoundNumberOfGroups:
                NewRound.RoundRobinNumberOfGroups = intValue;
                break;
            case XmlCupStates.StateCupRoundNumberOfAdvance:
                NewRound.RoundRobinNumberOfAdvance = intValue;
                break;
            case XmlCupStates.StateCupRoundNumberOfBestAdvance:
                NewRound.RoundRobinNumberOfBestAdvance = intValue;
                break;
            case XmlCupStates.StateCupRoundRoundRobins:
                NewRound.RoundRobins = intValue;
                break;
            case XmlCupStates.StateCupRoundBreak:
                foreach (string part in text.Split(','))
                {
                    if (int.TryParse(part, out int breakValue))
                        NewRound.RrBreaks.Add(breakValue);
                }
                break;
            case XmlCupStates.StateCupRoundWait:
                NewWait.CupSid = text;
                break;
            case XmlCupStates.StateCupRoundTwoMatchWeekStart:
                NewRound.TwoMatchWeeks[0].Add(intValue);
                break;
            case XmlCupStates.StateCupRoundTwoMatchWeekEnd:
                NewRound.TwoMatchWeeks[1].Add(intValue);
                break;
            case XmlCupStates.StateCupRoundTwoMatchWeek:
                NewRound.TwoMatchWeek = intValue != 0;
                break;
            case XmlCupStates.StateChooseTeamSid:
                NewChooseTeam.Sid = text;
                break;
            case XmlCupStates.StateChooseTeamNumberOfTeams:
                NewChooseTeam.NumberOfTeams = intValue;
                break;
            case XmlCupStates.StateChooseTeamStartIdx:
                NewChooseTeam.StartIdx = intValue;
                break;
            case XmlCupStates.StateChooseTeamEndIdx:
                NewChooseTeam.EndIdx = intValue;
                break;
            case XmlCupStates.StateChooseTeamRandomly:
                NewChooseTeam.Randomly = intValue != 0;
                break;
            case XmlCupStates.StateChooseTeamGenerate:
                NewChooseTeam.Generate = intValue != 0;
                break;
            case XmlCupStates.StateChooseTeamSkipGroupCheck:
                NewChooseTeam.SkipGroupCheck = intValue != 0;
                break;
            case XmlCupStates.StateChooseTeamFromTable:
                NewChooseTeam.FromTable = intValue;
                break;
            case XmlCupStates.StateChooseTeamPreload:
                NewChooseTeam.Preload = intValue != 0;
                break;
        }
    }

    /// <summary>
    /// Process an XML element and its children, starting with the
    /// XmlCupReadStartElement function, reading the text in between and
    /// finishing with the XmlCupReadEndElement function.
    /// </summary>
    /// <param name="element">The XML element to process.</param>
    private static void ProcessCupElement(XmlNode element)
    {
        XmlCupReadStartElement(element.LocalName, GetAttributesFromNode(element));

        foreach (XmlNode child in element.ChildNodes)
        {
            if (child.NodeType == XmlNodeType.Element)
                ProcessCupElement(child);
            else if (child.NodeType == XmlNodeType.Text)
                XmlCupReadText(child.InnerText);
        }

        XmlCupReadEndElement(element.LocalName);
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
    /// Get the value of an attribute from a dictionary of attributes.
    /// </summary>
    /// <param name="attributeNames">The attributes.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The value of the attribute, or null if it is not set.</returns>
}
