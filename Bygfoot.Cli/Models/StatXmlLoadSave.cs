namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of league statistics XML files.
/// Corresponds to the C module src/xml_loadsave_league_stat.h and src/xml_loadsave_league_stat.c
/// (xml_loadsave_league_stat_read, xml_loadsave_league_stat_write, xml_loadsave_league_stat_write_stat
/// and the xml_loadsave_league_stat_start_element, xml_loadsave_league_stat_end_element and
/// xml_loadsave_league_stat_text functions).
/// </summary>
public static class StatXmlLoadSave
{
    private const int TagLeagueStat = 10000;
    private const int TagStatLeagueName = 10001;
    private const int TagStatLeagueSymbol = 10002;
    private const int TagStatTeamsOff = 10003;
    private const int TagStatTeamsDef = 10004;
    private const int TagStatPlayerScorers = 10005;
    private const int TagStatPlayerGoalies = 10006;
    private const int TagStat = 10007;
    private const int TagStatTeamName = 10008;
    private const int TagStatValue = 10009;
    private const int TagStatValueString = 10010;
    private const int TagEnd = 10011;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the league statistics module.
    /// </summary>
    public enum XmlStatStates
    {
        TagLeagueStat = 10000,
        TagStatLeagueName = 10001,
        TagStatLeagueSymbol = 10002,
        TagStatTeamsOff = 10003,
        TagStatTeamsDef = 10004,
        TagStatPlayerScorers = 10005,
        TagStatPlayerGoalies = 10006,
        TagStat = 10007,
        TagStatTeamName = 10008,
        TagStatValue = 10009,
        TagStatValueString = 10010,
        TagEnd = 10011
    }

    private static XmlStatStates State = XmlStatStates.TagLeagueStat;
    private static XmlStatStates InState = XmlStatStates.TagLeagueStat;
    private static int ValueIdx = 0;
    private static Stat NewStat = new Stat();
    private static LeagueStat LigStat = new LeagueStat();

    /// <summary>
    /// Function reading a league statistics file and filling in the given league stat.
    /// @param filename Name of the xml file (e.g. '___league_0_stat.xml') to be read.
    /// @param leagueStat The league stat to fill in.
    /// </summary>
    /// <param name="filename">The league statistics xml file to read.</param>
    /// <param name="leagueStat">The league stat to read the file into.</param>
    public static void StatXmlLoadSaveRead(string filename, LeagueStat leagueStat)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlStatStates.TagLeagueStat;
        InState = XmlStatStates.TagLeagueStat;
        ValueIdx = 0;
        NewStat = new Stat();

        LigStat = leagueStat;

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
                            StatXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            StatXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            StatXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        StatXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        StatXmlLoadSaveReadText(reader.ReadString());
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
    public static void StatXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void StatXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the league statistics XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);

        switch (tag)
        {
            case TagLeagueStat:
                State = XmlStatStates.TagLeagueStat;
                break;
            case TagStatLeagueName:
                State = XmlStatStates.TagStatLeagueName;
                break;
            case TagStatLeagueSymbol:
                State = XmlStatStates.TagStatLeagueSymbol;
                break;
            case TagStatTeamsOff:
                State = XmlStatStates.TagStatTeamsOff;
                InState = XmlStatStates.TagStatTeamsOff;
                break;
            case TagStatTeamsDef:
                State = XmlStatStates.TagStatTeamsDef;
                InState = XmlStatStates.TagStatTeamsDef;
                break;
            case TagStatPlayerScorers:
                State = XmlStatStates.TagStatPlayerScorers;
                InState = XmlStatStates.TagStatPlayerScorers;
                break;
            case TagStatPlayerGoalies:
                State = XmlStatStates.TagStatPlayerGoalies;
                InState = XmlStatStates.TagStatPlayerGoalies;
                break;
            case TagStat:
                State = XmlStatStates.TagStat;
                ValueIdx = 0;
                NewStat = new Stat();
                NewStat.TeamName = string.Empty;
                NewStat.ValueString = string.Empty;
                break;
            case TagStatTeamName:
                State = XmlStatStates.TagStatTeamName;
                break;
            case TagStatValue:
                State = XmlStatStates.TagStatValue;
                break;
            case TagStatValueString:
                State = XmlStatStates.TagStatValueString;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_league_stat_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the league statistics
    /// module are the numbers following the underscore in the tag names (e.g. '_10000').
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
    public static void StatXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagStatLeagueName ||
            tag == TagStatLeagueSymbol ||
            tag == TagStatTeamsOff ||
            tag == TagStatTeamsDef ||
            tag == TagStatPlayerScorers ||
            tag == TagStatPlayerGoalies)
        {
            State = XmlStatStates.TagLeagueStat;
        }
        else if (tag == TagStat)
        {
            State = InState;
            if (InState == XmlStatStates.TagStatTeamsOff)
                LigStat.TeamsOff.Add(NewStat);
            else if (InState == XmlStatStates.TagStatTeamsDef)
                LigStat.TeamsDef.Add(NewStat);
            else if (InState == XmlStatStates.TagStatPlayerScorers)
                LigStat.PlayerScorers.Add(NewStat);
            else if (InState == XmlStatStates.TagStatPlayerGoalies)
                LigStat.PlayerGoalies.Add(NewStat);
            else
            {
                Debug.DebugPrintMessage($"xml_loadsave_league_stat_end_element: unknown in_state {InState}");
                return;
            }
        }
        else if (tag == TagStatValueString ||
                 tag == TagStatValue ||
                 tag == TagStatTeamName)
        {
            State = XmlStatStates.TagStat;
            if (tag == TagStatValue)
                ValueIdx++;
        }
        else if (tag != TagLeagueStat)
        {
            Debug.DebugPrintMessage($"xml_loadsave_league_stat_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the league stat variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void StatXmlLoadSaveReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlStatStates.TagStatLeagueName:
                LigStat.LeagueName = text;
                break;
            case XmlStatStates.TagStatLeagueSymbol:
                LigStat.LeagueSymbol = text;
                break;
            case XmlStatStates.TagStatTeamName:
                NewStat.TeamName = text;
                break;
            case XmlStatStates.TagStatValue:
                if (ValueIdx == 0)
                    NewStat.Value1 = intValue;
                else if (ValueIdx == 1)
                    NewStat.Value2 = intValue;
                else
                    NewStat.Value3 = intValue;
                break;
            case XmlStatStates.TagStatValueString:
                NewStat.ValueString = text;
                break;
        }
    }

    /// <summary>
    /// Write the league stats into the given file.
    /// </summary>
    /// <param name="filename">The league statistics xml file to write.</param>
    /// <param name="leagueStat">The league stat to write.</param>
    public static void StatXmlLoadSaveWrite(string filename, LeagueStat leagueStat)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine($"<_{(int)TagLeagueStat}>");
            if (leagueStat.LeagueName != null)
                fil.WriteLine($"<_{(int)TagStatLeagueName}>{leagueStat.LeagueName}</_{(int)TagStatLeagueName}>");
            if (leagueStat.LeagueSymbol != null)
                fil.WriteLine($"<_{(int)TagStatLeagueSymbol}>{leagueStat.LeagueSymbol}</_{(int)TagStatLeagueSymbol}>");
            fil.WriteLine($"<_{(int)TagStatTeamsOff}>");
            for (int i = 0; i < leagueStat.TeamsOff.Count; i++)
                StatXmlLoadSaveWriteStat(fil, leagueStat.TeamsOff[i]);
            fil.WriteLine($"</_{(int)TagStatTeamsOff}>");
            fil.WriteLine($"<_{(int)TagStatTeamsDef}>");
            for (int i = 0; i < leagueStat.TeamsDef.Count; i++)
                StatXmlLoadSaveWriteStat(fil, leagueStat.TeamsDef[i]);
            fil.WriteLine($"</_{(int)TagStatTeamsDef}>");
            fil.WriteLine($"<_{(int)TagStatPlayerScorers}>");
            for (int i = 0; i < leagueStat.PlayerScorers.Count; i++)
                StatXmlLoadSaveWriteStat(fil, leagueStat.PlayerScorers[i]);
            fil.WriteLine($"</_{(int)TagStatPlayerScorers}>");
            fil.WriteLine($"<_{(int)TagStatPlayerGoalies}>");
            for (int i = 0; i < leagueStat.PlayerGoalies.Count; i++)
                StatXmlLoadSaveWriteStat(fil, leagueStat.PlayerGoalies[i]);
            fil.WriteLine($"</_{(int)TagStatPlayerGoalies}>");
            fil.WriteLine($"</_{(int)TagLeagueStat}>");
        }
    }

    /// <summary>
    /// Write a single league statistic into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="stat">The statistic to write.</param>
    public static void StatXmlLoadSaveWriteStat(TextWriter fil, Stat stat)
    {
        fil.WriteLine($"\t<_{(int)TagStat}>");
        if (stat.TeamName != null)
            fil.WriteLine($"\t<_{(int)TagStatTeamName}>{stat.TeamName}</_{(int)TagStatTeamName}>");
        fil.WriteLine($"\t<_{(int)TagStatValue}>{stat.Value1}</_{(int)TagStatValue}>");
        fil.WriteLine($"\t<_{(int)TagStatValue}>{stat.Value2}</_{(int)TagStatValue}>");
        fil.WriteLine($"\t<_{(int)TagStatValue}>{stat.Value3}</_{(int)TagStatValue}>");
        if (stat.ValueString != null)
            fil.WriteLine($"\t<_{(int)TagStatValueString}>{stat.ValueString}</_{(int)TagStatValueString}>");
        fil.WriteLine($"\t</_{(int)TagStat}>");
    }
}
