namespace Bygfoot.Cli.Models;

/// <summary>
/// Static class to handle the central XML functions and the tags that are shared
/// between the different XML modules. Corresponds to the C module src/xml.h and src/xml.c
/// (the xml_write_string, xml_write_int, xml_write_float and xml_get_tag_from_name functions
/// and the xml_load_users, xml_load_league, xml_load_cup and xml_load_transfers functions).
/// </summary>
public static class Xml
{
    // Indentations for xml files. Corresponds to the I0 ... I8 macros of src/xml.h.
    public const string I0 = "";
    public const string I1 = "\t";
    public const string I2 = "\t\t";
    public const string I3 = "\t\t\t";
    public const string I4 = "\t\t\t\t";
    public const string I5 = "\t\t\t\t\t";
    public const string I6 = "\t\t\t\t\t\t";
    public const string I7 = "\t\t\t\t\t\t\t";
    public const string I8 = "\t\t\t\t\t\t\t\t";

    /// <summary>
    /// Tags that are used in more than one type of xml file.
    /// Corresponds to the C enum XmlTags in src/xml.h.
    /// </summary>
    public enum XmlTags
    {
        TagName = 0,
        TagSymbol,
        TagSid,
        TagShortName,
        TagId,
        TagWeekGap,
        TagWeekBreak,
        TagWeekBreakLength,
        TagSkipWeeksWith,
        TagYellowRed,
        TagTeamId,
        TagNamesFile,
        TagProperty,
        TagRound
    }

    public const int TagStartMisc = 1000;
    public const int TagStartLeague = 2000;
    public const int TagStartCup = 3000;
    public const int TagStartTeams = 4000;
    public const int TagStartFixtures = 5000;
    public const int TagStartTable = 6000;
    public const int TagStartUsers = 7000;
    public const int TagStartLiveGame = 8000;
    public const int TagStartPlayers = 9000;
    public const int TagEndPlayers = 9900;
    public const int TagStartLeagueStat = 10000;
    public const int TagStartSeasonStats = 11000;
    public const int TagStartLeaguesCups = 12000;
    public const int TagStartTransfers = 20000;
    public const int TagStartJobs = 21000;
    public const int TagStartNewsPaper = 22000;

    public const string TagDefName = "name";
    public const string TagDefShortName = "short_name";
    public const string TagDefSid = "sid";
    public const string TagDefSymbol = "symbol";
    public const string TagDefWeekGap = "week_gap";
    public const string TagDefProperty = "property";
    public const string TagDefYellowRed = "yellow_red";
    public const string TagDefBreakIn = "break_in";
    public const string AttDefNameWeekBreakLength = "length";
    public const string TagDefSkipWeeksWith = "skip_weeks_with";

    /// <summary>
    /// Write a string into the file. The string gets skipped when it is null.
    /// Corresponds to C: xml_write_string.
    /// </summary>
    /// <param name="fil">The file to write into.</param>
    /// <param name="value">The string to write.</param>
    /// <param name="tag">The tag to write.</param>
    /// <param name="indent">The indentation.</param>
    public static void XmlWriteString(TextWriter fil, string value, int tag, string indent)
    {
        if (value == null)
            return;

        fil.WriteLine($"{indent}<_{tag}>{value}</_{tag}>");
    }

    /// <summary>
    /// Write an integer into the file. Corresponds to C: xml_write_int.
    /// </summary>
    /// <param name="fil">The file to write into.</param>
    /// <param name="value">The integer to write.</param>
    /// <param name="tag">The tag to write.</param>
    /// <param name="indent">The indentation.</param>
    public static void XmlWriteInt(TextWriter fil, int value, int tag, string indent)
    {
        fil.WriteLine($"{indent}<_{tag}>{value}</_{tag}>");
    }

    /// <summary>
    /// Write a float into the file. The value gets multiplied by 10000 and rounded,
    /// since the C version stores the floats as integers (e.g. 50% possession is stored
    /// as 500). Corresponds to C: xml_write_float.
    /// </summary>
    /// <param name="fil">The file to write into.</param>
    /// <param name="value">The float to write.</param>
    /// <param name="tag">The tag to write.</param>
    /// <param name="indent">The indentation.</param>
    public static void XmlWriteFloat(TextWriter fil, float value, int tag, string indent)
    {
        fil.WriteLine($"{indent}<_{tag}>{(int)Math.Round(value * 10000)}</_{tag}>");
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the xml modules are the
    /// numbers following the underscore in the tag names (e.g. '_6000').
    /// Corresponds to C: xml_get_tag_from_name.
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The tag.</returns>
    public static int XmlGetTagFromName(string elementName)
    {
        string tagName = elementName.Substring(1);

        return (int)(double.TryParse(tagName, out double tagValue) ? tagValue : 0);
    }

    /// <summary>
    /// Function reading the users xml file and loading the users, their options and
    /// their live games. Corresponds to C: xml_load_users.
    /// </summary>
    /// <param name="dirname">The directory the xml files are located in.</param>
    /// <param name="prefix">The prefix of the users xml file (e.g. 'user_0').</param>
    public static void XmlLoadUsers(string dirname, string prefix)
    {
        UsersXmlLoadSave.UsersXmlLoadSaveRead(dirname, prefix);

        for (int i = 0; i < Variables.Users.Count; i++)
        {
            string optionsFile = Path.Combine(dirname, $"{prefix}___user_{i:D2}_options");
            FileHelper.LoadOptFile(optionsFile, Variables.Users[i].Options, false);

            string liveGameFile = Path.Combine(dirname, $"{prefix}___user_{i:D2}_live_game.xml");
            LiveGameXmlLoadSave.LiveGameXmlLoadSaveRead(liveGameFile, Variables.Users[i].LiveGame);
        }
    }

    /// <summary>
    /// Function reading a league xml file and loading the league, its teams, fixtures
    /// and statistics. Corresponds to C: xml_load_league.
    /// </summary>
    /// <param name="dirname">The directory the xml files are located in.</param>
    /// <param name="basename">The basename of the league xml file (e.g. 'league_england1.xml').</param>
    public static void XmlLoadLeague(string dirname, string basename)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(basename))
            return;

        string prefix = basename.Substring(0, basename.Length - 4);

        string leagueFile = Path.Combine(dirname, basename);
        string teamFile = Path.Combine(dirname, prefix + "_teams.xml");

        League league = new League();
        LeagueXmlLoadSave.LeagueXmlLoadSaveRead(leagueFile, teamFile, league);

        string fixturesFile = Path.Combine(dirname, prefix + "_fixtures.xml");
        FixturesXmlLoadSave.FixturesXmlLoadSaveRead(fixturesFile, league.Fixtures);

        string statFile = Path.Combine(dirname, prefix + "_stat.xml");
        StatXmlLoadSave.StatXmlLoadSaveRead(statFile, league.Stats);

        Variables.Country.Leagues.Add(league);
    }

    /// <summary>
    /// Function reading a cup xml file and loading the cup and its fixtures.
    /// Corresponds to C: xml_load_cup.
    /// </summary>
    /// <param name="dirname">The directory the xml files are located in.</param>
    /// <param name="basename">The basename of the cup xml file (e.g. 'cup_european_champions_league.xml').</param>
    public static void XmlLoadCup(string dirname, string basename)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(basename))
            return;

        string prefix = basename.Substring(0, basename.Length - 4);

        Cup cup = new Cup();
        CupXml.XmlCupRead(basename, new List<Cup>());
        FixturesXmlLoadSave.FixturesXmlLoadSaveRead(Path.Combine(dirname, prefix + "_fixtures.xml"), cup.Fixtures);
    }

    /// <summary>
    /// Function reading the transfer list xml file. Corresponds to C: xml_load_transfers.
    /// </summary>
    /// <param name="dirname">The directory the xml files are located in.</param>
    /// <param name="prefix">The prefix of the transfer list xml file (e.g. 'user_0').</param>
    public static void XmlLoadTransfers(string dirname, string prefix)
    {
        string filename = Path.Combine(dirname, prefix + "___transfer_list.xml");
        TransfersXmlLoadSave.TransfersXmlLoadSaveRead(filename);
    }
}
