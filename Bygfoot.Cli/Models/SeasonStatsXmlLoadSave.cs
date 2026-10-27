namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of season statistics XML files.
/// Corresponds to the C module src/xml_loadsave_season_stats.h and src/xml_loadsave_season_stats.c
/// (xml_loadsave_season_stats_read, xml_loadsave_season_stats_write and the
/// xml_loadsave_season_stats_start_element, xml_loadsave_season_stats_end_element and
/// xml_loadsave_season_stats_text functions).
/// </summary>
public static class SeasonStatsXmlLoadSave
{
    // Tags. Corresponds to the tag enum of the season stats module in src/xml_loadsave_season_stats.c.
    private const int TagSeasonStats = 11000;
    private const int TagSeasonStat = 11001;
    private const int TagSeasonNumber = 11002;
    private const int TagLeagueChamps = 11003;
    private const int TagCupChamps = 11004;
    private const int TagChampStat = 11005;
    private const int TagChampStatTeamName = 11006;
    private const int TagChampStatClName = 11007;
    private const int TagLeagueStatFile = 11008;
    private const int TagEnd = 11009;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the season stats module.
    /// </summary>
    public enum XmlSeasonStatsStates
    {
        TagSeasonStats = 11000,
        TagSeasonStat = 11001,
        TagSeasonNumber = 11002,
        TagLeagueChamps = 11003,
        TagCupChamps = 11004,
        TagChampStat = 11005,
        TagChampStatTeamName = 11006,
        TagChampStatClName = 11007,
        TagLeagueStatFile = 11008,
        TagEnd = 11009
    }

    private static XmlSeasonStatsStates State = XmlSeasonStatsStates.TagSeasonStats;
    private static XmlSeasonStatsStates InState = XmlSeasonStatsStates.TagLeagueChamps;
    private static SeasonStat NewSeasonStat = new SeasonStat();
    private static ChampStat NewChampStat = new ChampStat();
    private static string Dirname = "";

    /// <summary>
    /// Function reading a season statistics xml file and adding the
    /// season statistics to the internal list of season statistics.
    /// @param dirname The directory the season statistics xml file is located in.
    /// @param prefix The prefix of the season statistics xml file (e.g. 'user_0').
    /// </summary>
    /// <param name="dirname">The directory the season statistics xml file is located in.</param>
    /// <param name="prefix">The prefix of the season statistics xml file.</param>
    public static void SeasonStatsXmlLoadSaveRead(string dirname, string prefix)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(prefix))
            return;

        State = XmlSeasonStatsStates.TagSeasonStats;
        InState = XmlSeasonStatsStates.TagLeagueChamps;
        NewSeasonStat = new SeasonStat();
        NewChampStat = new ChampStat();
        Dirname = dirname;

        Variables.SeasonStats.Clear();

        string filename = Path.Combine(dirname, prefix + "___season_stats.xml");
        if (!File.Exists(filename))
            return;

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
                            SeasonStatsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            SeasonStatsXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            SeasonStatsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        SeasonStatsXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        SeasonStatsXmlLoadSaveReadText(reader.ReadString());
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
    public static void SeasonStatsXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void SeasonStatsXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the season statistics XML parser, dispatching
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
            case TagSeasonStats:
                State = XmlSeasonStatsStates.TagSeasonStats;
                validTag = true;
                break;
            case TagSeasonStat:
                State = XmlSeasonStatsStates.TagSeasonStat;
                validTag = true;
                break;
            case TagSeasonNumber:
                State = XmlSeasonStatsStates.TagSeasonNumber;
                validTag = true;
                break;
            case TagLeagueChamps:
                State = XmlSeasonStatsStates.TagLeagueChamps;
                InState = XmlSeasonStatsStates.TagLeagueChamps;
                validTag = true;
                break;
            case TagCupChamps:
                State = XmlSeasonStatsStates.TagCupChamps;
                InState = XmlSeasonStatsStates.TagCupChamps;
                validTag = true;
                break;
            case TagChampStat:
                State = XmlSeasonStatsStates.TagChampStat;
                validTag = true;
                break;
            case TagChampStatTeamName:
                State = XmlSeasonStatsStates.TagChampStatTeamName;
                validTag = true;
                break;
            case TagChampStatClName:
                State = XmlSeasonStatsStates.TagChampStatClName;
                validTag = true;
                break;
            case TagLeagueStatFile:
                State = XmlSeasonStatsStates.TagLeagueStatFile;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_season_stats_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagSeasonStat)
        {
            NewSeasonStat = new SeasonStat();
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_season_stats_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the season stats module
    /// are the numbers following the underscore in the tag names (e.g. '_11000').
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
    public static void SeasonStatsXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagSeasonStat)
        {
            State = XmlSeasonStatsStates.TagSeasonStats;
            Variables.SeasonStats.Add(NewSeasonStat);
        }
        else if (tag == TagSeasonNumber ||
                 tag == TagLeagueChamps ||
                 tag == TagCupChamps ||
                 tag == TagLeagueStatFile)
        {
            State = XmlSeasonStatsStates.TagSeasonStat;
        }
        else if (tag == TagChampStat)
        {
            if (InState == XmlSeasonStatsStates.TagCupChamps)
                NewSeasonStat.CupChamps.Add(NewChampStat);
            else if (InState == XmlSeasonStatsStates.TagLeagueChamps)
                NewSeasonStat.LeagueChamps.Add(NewChampStat);
            State = InState;
        }
        else if (tag == TagChampStatTeamName ||
                 tag == TagChampStatClName)
        {
            State = XmlSeasonStatsStates.TagChampStat;
        }
        else if (tag != TagSeasonStats)
        {
            Debug.DebugPrintMessage($"xml_loadsave_season_stats_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the season statistics variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void SeasonStatsXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlSeasonStatsStates.TagSeasonNumber:
                NewSeasonStat.SeasonNumber = intValue;
                break;
            case XmlSeasonStatsStates.TagChampStatTeamName:
                NewChampStat.TeamName = text;
                break;
            case XmlSeasonStatsStates.TagChampStatClName:
                NewChampStat.ClName = text;
                break;
            case XmlSeasonStatsStates.TagLeagueStatFile:
            {
                LeagueStat newLeagueStat = new LeagueStat();
                string leagueStatFileName = Path.Combine(Dirname, text);
                StatXmlLoadSave.StatXmlLoadSaveRead(leagueStatFileName, newLeagueStat);
                NewSeasonStat.LeagueStats.Add(newLeagueStat);
                break;
            }
        }
    }

    /// <summary>
    /// Write the season statistics into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the season statistics xml file (e.g. 'user_0').</param>
    public static void SeasonStatsXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___season_stats.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_11000>");
            int seasonStatsIdx = 0;
            foreach (SeasonStat seasonStat in Variables.SeasonStats)
            {
                fil.WriteLine("<_11001>");
                fil.WriteLine("<_11002>" + seasonStat.SeasonNumber + "</_11002>");
                fil.WriteLine("\t<_11003>");
                foreach (ChampStat champStat in seasonStat.LeagueChamps)
                {
                    fil.WriteLine("\t\t<_11005>");
                    if (champStat.TeamName != null)
                        fil.WriteLine("\t\t\t<_11006>" + champStat.TeamName + "</_11006>");
                    if (champStat.ClName != null)
                        fil.WriteLine("\t\t\t<_11007>" + champStat.ClName + "</_11007>");
                    fil.WriteLine("\t\t</_11005>");
                }
                fil.WriteLine("\t</_11003>");
                fil.WriteLine("\t<_11004>");
                foreach (ChampStat champStat in seasonStat.CupChamps)
                {
                    fil.WriteLine("\t\t<_11005>");
                    if (champStat.TeamName != null)
                        fil.WriteLine("\t\t\t<_11006>" + champStat.TeamName + "</_11006>");
                    if (champStat.ClName != null)
                        fil.WriteLine("\t\t\t<_11007>" + champStat.ClName + "</_11007>");
                    fil.WriteLine("\t\t</_11005>");
                }
                fil.WriteLine("\t</_11004>");
                for (int j = 0; j < seasonStat.LeagueStats.Count; j++)
                {
                    string leagueStatFileName = $"{prefix}___season_stats_league_stat_{seasonStatsIdx:00}-{j:00}.xml";
                    fil.WriteLine("\t<_11008>" + leagueStatFileName + "</_11008>");
                    StatXmlLoadSave.StatXmlLoadSaveWrite(leagueStatFileName, seasonStat.LeagueStats[j]);
                }
                fil.WriteLine("</_11001>");
                seasonStatsIdx++;
            }
            fil.WriteLine("</_11000>");
        }
    }
}
