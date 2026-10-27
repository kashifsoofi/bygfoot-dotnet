namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of team XML files.
/// Corresponds to the C module src/xml_loadsave_teams.h and src/xml_loadsave_teams.c
/// (xml_loadsave_teams_read, xml_loadsave_teams_write, xml_loadsave_teams_write_team and
/// the xml_loadsave_teams_start_element, xml_loadsave_teams_end_element and
/// xml_loadsave_teams_text functions, as well as the player handling functions of
/// the src/xml_loadsave_players.c module).
/// </summary>
public static class TeamsXmlLoadSave
{
    // Team tags. Corresponds to the tag enum of the teams module in src/xml_loadsave_teams.c.
    private const int TagTeams = 4000;
    private const int TagTeam = 4001;
    private const int TagTeamNamesFile = 4002;
    private const int TagTeamStructure = 4003;
    private const int TagTeamStyle = 4004;
    private const int TagTeamBoost = 4005;
    private const int TagTeamClid = 4006;
    private const int TagTeamStrategySid = 4007;
    private const int TagTeamStadium = 4008;
    private const int TagTeamStadiumName = 4009;
    private const int TagTeamStadiumCapacity = 4010;
    private const int TagTeamStadiumAverageAttendance = 4011;
    private const int TagTeamStadiumPossibleAttendance = 4012;
    private const int TagTeamStadiumGames = 4013;
    private const int TagTeamStadiumSafety = 4014;
    private const int TagTeamLuck = 4015;
    private const int TagTeamStadiumTicketPrice = 4016;
    private const int TagEnd = 4017;

    // Player tags. Corresponds to the tag enum of the players module in src/xml_loadsave_players.c.
    private const int TagPlayers = 9000;
    private const int TagPlayer = 9001;
    private const int TagPlayerName = 9002;
    private const int TagPlayerPos = 9003;
    private const int TagPlayerCPos = 9004;
    private const int TagPlayerHealth = 9005;
    private const int TagPlayerRecovery = 9006;
    private const int TagPlayerId = 9007;
    private const int TagPlayerValue = 9008;
    private const int TagPlayerWage = 9009;
    private const int TagPlayerOffers = 9010;
    private const int TagPlayerSkill = 9011;
    private const int TagPlayerCSkill = 9012;
    private const int TagPlayerTalent = 9013;
    private const int TagPlayerEtal = 9014;
    private const int TagPlayerFitness = 9015;
    private const int TagPlayerLsu = 9016;
    private const int TagPlayerAge = 9017;
    private const int TagPlayerPeakAge = 9018;
    private const int TagPlayerPeakRegion = 9019;
    private const int TagPlayerContract = 9020;
    private const int TagPlayerParticipation = 9021;
    private const int TagPlayerGamesGoal = 9022;
    private const int TagPlayerGamesGoalClid = 9023;
    private const int TagPlayerGamesGoalGames = 9024;
    private const int TagPlayerGamesGoalGoals = 9025;
    private const int TagPlayerGamesGoalShots = 9026;
    private const int TagPlayerCard = 9027;
    private const int TagPlayerCardClid = 9028;
    private const int TagPlayerCardYellow = 9029;
    private const int TagPlayerCardRed = 9030;
    private const int TagPlayerCareer = 9031;
    private const int TagPlayerStreak = 9032;
    private const int TagPlayerStreakCount = 9033;
    private const int TagPlayerStreakProb = 9034;

    // Common tags used in the team xml files. Corresponds to the tag enum of src/xml.h.
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

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the teams and players modules.
    /// </summary>
    public enum XmlTeamsLoadSaveStates
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
        TagTeams = 4000,
        TagTeam = 4001,
        TagTeamNamesFile = 4002,
        TagTeamStructure = 4003,
        TagTeamStyle = 4004,
        TagTeamBoost = 4005,
        TagTeamClid = 4006,
        TagTeamStrategySid = 4007,
        TagTeamStadium = 4008,
        TagTeamStadiumName = 4009,
        TagTeamStadiumCapacity = 4010,
        TagTeamStadiumAverageAttendance = 4011,
        TagTeamStadiumPossibleAttendance = 4012,
        TagTeamStadiumGames = 4013,
        TagTeamStadiumSafety = 4014,
        TagTeamLuck = 4015,
        TagTeamStadiumTicketPrice = 4016,
        TagEnd = 4017,
        TagPlayers = 9000,
        TagPlayer = 9001,
        TagPlayerName = 9002,
        TagPlayerPos = 9003,
        TagPlayerCPos = 9004,
        TagPlayerHealth = 9005,
        TagPlayerRecovery = 9006,
        TagPlayerId = 9007,
        TagPlayerValue = 9008,
        TagPlayerWage = 9009,
        TagPlayerOffers = 9010,
        TagPlayerSkill = 9011,
        TagPlayerCSkill = 9012,
        TagPlayerTalent = 9013,
        TagPlayerEtal = 9014,
        TagPlayerFitness = 9015,
        TagPlayerLsu = 9016,
        TagPlayerAge = 9017,
        TagPlayerPeakAge = 9018,
        TagPlayerPeakRegion = 9019,
        TagPlayerContract = 9020,
        TagPlayerParticipation = 9021,
        TagPlayerGamesGoal = 9022,
        TagPlayerGamesGoalClid = 9023,
        TagPlayerGamesGoalGames = 9024,
        TagPlayerGamesGoalGoals = 9025,
        TagPlayerGamesGoalShots = 9026,
        TagPlayerCard = 9027,
        TagPlayerCardClid = 9028,
        TagPlayerCardYellow = 9029,
        TagPlayerCardRed = 9030,
        TagPlayerCareer = 9031,
        TagPlayerStreak = 9032,
        TagPlayerStreakCount = 9033,
        TagPlayerStreakProb = 9034
    }

    private static XmlTeamsLoadSaveStates State = XmlTeamsLoadSaveStates.TagTeams;
    private static Team NewTeam = new Team();
    private static List<Team> Teams = new List<Team>();
    private static Player NewPlayer = new Player();
    private static PlayerGamesGoals NewGamesGoals = new PlayerGamesGoals();
    private static PlayerCard NewCard = new PlayerCard();
    private static int EtalIdx = 0;
    private static int CareerIdx = 0;

    /// <summary>
    /// Function reading a team file and adding the teams
    /// to the given array.
    /// @param filename Name of the team xml file (e.g. '___league_0_teams.xml') to be read.
    /// @param teams The array we add the teams to.
    /// </summary>
    /// <param name="filename">The team xml file to read.</param>
    /// <param name="teams">The array to add the teams to.</param>
    public static void TeamsXmlLoadSaveRead(string filename, List<Team> teams)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlTeamsLoadSaveStates.TagTeams;
        NewTeam = new Team();
        Teams = teams;

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
                            TeamsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            TeamsXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            TeamsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        TeamsXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        TeamsXmlLoadSaveReadText(reader.ReadString());
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
    public static void TeamsXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void TeamsXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the team XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);

        if (tag >= TagPlayers && tag <= TagPlayerStreakProb)
        {
            State = (XmlTeamsLoadSaveStates)tag;
            PlayersReadStartElement(tag, NewTeam);
            return;
        }

        switch (tag)
        {
            case TagTeams:
                State = XmlTeamsLoadSaveStates.TagTeams;
                break;
            case TagTeam:
                State = XmlTeamsLoadSaveStates.TagTeam;
                NewTeam = new Team();
                break;
            case TagTeamNamesFile:
                State = XmlTeamsLoadSaveStates.TagTeamNamesFile;
                break;
            case TagTeamStructure:
                State = XmlTeamsLoadSaveStates.TagTeamStructure;
                break;
            case TagTeamStyle:
                State = XmlTeamsLoadSaveStates.TagTeamStyle;
                break;
            case TagTeamBoost:
                State = XmlTeamsLoadSaveStates.TagTeamBoost;
                break;
            case TagTeamClid:
                State = XmlTeamsLoadSaveStates.TagTeamClid;
                break;
            case TagTeamStrategySid:
                State = XmlTeamsLoadSaveStates.TagTeamStrategySid;
                break;
            case TagTeamStadium:
                State = XmlTeamsLoadSaveStates.TagTeamStadium;
                break;
            case TagTeamStadiumName:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumName;
                break;
            case TagTeamStadiumCapacity:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumCapacity;
                break;
            case TagTeamStadiumAverageAttendance:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumAverageAttendance;
                break;
            case TagTeamStadiumPossibleAttendance:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumPossibleAttendance;
                break;
            case TagTeamStadiumGames:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumGames;
                break;
            case TagTeamStadiumSafety:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumSafety;
                break;
            case TagTeamLuck:
                State = XmlTeamsLoadSaveStates.TagTeamLuck;
                break;
            case TagTeamStadiumTicketPrice:
                State = XmlTeamsLoadSaveStates.TagTeamStadiumTicketPrice;
                break;
            case TagName:
                State = XmlTeamsLoadSaveStates.TagName;
                break;
            case TagSymbol:
                State = XmlTeamsLoadSaveStates.TagSymbol;
                break;
            case TagSid:
                State = XmlTeamsLoadSaveStates.TagSid;
                break;
            case TagShortName:
                State = XmlTeamsLoadSaveStates.TagShortName;
                break;
            case TagId:
                State = XmlTeamsLoadSaveStates.TagId;
                break;
            case TagWeekGap:
                State = XmlTeamsLoadSaveStates.TagWeekGap;
                break;
            case TagWeekBreak:
                State = XmlTeamsLoadSaveStates.TagWeekBreak;
                break;
            case TagWeekBreakLength:
                State = XmlTeamsLoadSaveStates.TagWeekBreakLength;
                break;
            case TagSkipWeeksWith:
                State = XmlTeamsLoadSaveStates.TagSkipWeeksWith;
                break;
            case TagYellowRed:
                State = XmlTeamsLoadSaveStates.TagYellowRed;
                break;
            case TagTeamId:
                State = XmlTeamsLoadSaveStates.TagTeamId;
                break;
            case TagNamesFile:
                State = XmlTeamsLoadSaveStates.TagNamesFile;
                break;
            case TagProperty:
                State = XmlTeamsLoadSaveStates.TagProperty;
                break;
            case TagRound:
                State = XmlTeamsLoadSaveStates.TagRound;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_teams_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the teams and players
    /// modules are the numbers following the underscore in the tag names (e.g. '_4000').
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
    public static void TeamsXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag >= TagPlayers && tag <= TagPlayerStreakProb)
        {
            State = XmlTeamsLoadSaveStates.TagPlayers;
            PlayersReadEndElement(tag, NewTeam);
            return;
        }

        if (tag == TagTeam)
        {
            State = XmlTeamsLoadSaveStates.TagTeams;
            Teams.Add(NewTeam);
            NewTeam = new Team();
        }
        else if (tag == TagName ||
                 tag == TagSymbol ||
                 tag == TagTeamNamesFile ||
                 tag == TagTeamId ||
                 tag == TagTeamClid ||
                 tag == TagTeamStrategySid ||
                 tag == TagTeamStructure ||
                 tag == TagTeamStyle ||
                 tag == TagTeamBoost ||
                 tag == TagTeamStadium)
        {
            State = XmlTeamsLoadSaveStates.TagTeam;
        }
        else if (tag == TagTeamStadiumName ||
                 tag == TagTeamStadiumCapacity ||
                 tag == TagTeamStadiumAverageAttendance ||
                 tag == TagTeamStadiumPossibleAttendance ||
                 tag == TagTeamStadiumGames ||
                 tag == TagTeamStadiumSafety ||
                 tag == TagTeamStadiumTicketPrice)
        {
            State = XmlTeamsLoadSaveStates.TagTeamStadium;
        }
        else if (tag != TagTeams)
        {
            Debug.DebugPrintMessage($"xml_loadsave_teams_end_element: unknown tag: {tag}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the team variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void TeamsXmlLoadSaveReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        if (State >= XmlTeamsLoadSaveStates.TagPlayers && State <= XmlTeamsLoadSaveStates.TagPlayerStreakProb)
        {
            PlayersReadText(text);
            return;
        }

        switch (State)
        {
            case XmlTeamsLoadSaveStates.TagName:
                NewTeam.Name = text;
                break;
            case XmlTeamsLoadSaveStates.TagSymbol:
                NewTeam.Symbol = text;
                break;
            case XmlTeamsLoadSaveStates.TagTeamNamesFile:
                NewTeam.NamesFile = text;
                break;
            case XmlTeamsLoadSaveStates.TagTeamClid:
                NewTeam.Clid = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStrategySid:
                NewTeam.StrategySid = text;
                break;
            case XmlTeamsLoadSaveStates.TagTeamId:
                NewTeam.Id = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStructure:
                NewTeam.Structure = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStyle:
                NewTeam.Style = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamBoost:
                NewTeam.Boost = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumName:
                NewTeam.Stadium.Name = text;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumCapacity:
                NewTeam.Stadium.Capacity = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumAverageAttendance:
                NewTeam.Stadium.AverageAttendance = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumPossibleAttendance:
                NewTeam.Stadium.PossibleAttendance = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumGames:
                NewTeam.Stadium.Games = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumSafety:
                NewTeam.Stadium.Safety = floatValue / 10000f;
                break;
            case XmlTeamsLoadSaveStates.TagTeamStadiumTicketPrice:
                NewTeam.Stadium.TicketPrice = floatValue / 10000f;
                break;
            case XmlTeamsLoadSaveStates.TagTeamLuck:
                NewTeam.Luck = floatValue / 10000f;
                break;
        }
    }

    /// <summary>
    /// The function called by the players module handlers when an opening
    /// tag of a player is read. The state variable is changed in this function
    /// and sometimes memory allocated for the information that's going to be read.
    /// </summary>
    /// <param name="tag">The tag of the opening element.</param>
    /// <param name="team">The team the player belongs to.</param>
    private static void PlayersReadStartElement(int tag, Team team)
    {
        if (tag == TagPlayer)
        {
            NewPlayer = new Player();
            NewPlayer.Team = team;
            EtalIdx = 0;
            CareerIdx = 0;
        }
    }

    /// <summary>
    /// The function called by the players module handlers when a closing
    /// tag is read. The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// </summary>
    /// <param name="tag">The tag of the closing element.</param>
    /// <param name="team">The team the player belongs to.</param>
    private static void PlayersReadEndElement(int tag, Team team)
    {
        if (tag == TagPlayer)
        {
            team.Players.Add(NewPlayer);
        }
        else if (tag == TagPlayerName ||
                 tag == TagPlayerPos ||
                 tag == TagPlayerCPos ||
                 tag == TagPlayerHealth ||
                 tag == TagPlayerRecovery ||
                 tag == TagPlayerId ||
                 tag == TagPlayerValue ||
                 tag == TagPlayerWage ||
                 tag == TagPlayerOffers ||
                 tag == TagPlayerParticipation ||
                 tag == TagPlayerSkill ||
                 tag == TagPlayerCSkill ||
                 tag == TagPlayerTalent ||
                 tag == TagPlayerEtal ||
                 tag == TagPlayerFitness ||
                 tag == TagPlayerLsu ||
                 tag == TagPlayerAge ||
                 tag == TagPlayerPeakAge ||
                 tag == TagPlayerPeakRegion ||
                 tag == TagPlayerContract ||
                 tag == TagPlayerGamesGoal ||
                 tag == TagPlayerGamesGoalClid ||
                 tag == TagPlayerGamesGoalGames ||
                 tag == TagPlayerGamesGoalGoals ||
                 tag == TagPlayerGamesGoalShots ||
                 tag == TagPlayerCard ||
                 tag == TagPlayerCardClid ||
                 tag == TagPlayerCardYellow ||
                 tag == TagPlayerCardRed ||
                 tag == TagPlayerCareer ||
                 tag == TagPlayerStreak ||
                 tag == TagPlayerStreakCount ||
                 tag == TagPlayerStreakProb)
        {
            State = XmlTeamsLoadSaveStates.TagPlayer;
            if (tag == TagPlayerEtal)
                EtalIdx++;
            else if (tag == TagPlayerCareer)
                CareerIdx++;
            else if (tag == TagPlayerCard)
                NewPlayer.Cards.Add(NewCard);
            else if (tag == TagPlayerGamesGoal)
                NewPlayer.GamesGoals.Add(NewGamesGoals);
        }
        else if (tag == TagPlayerGamesGoalClid ||
                 tag == TagPlayerGamesGoalGames ||
                 tag == TagPlayerGamesGoalGoals ||
                 tag == TagPlayerGamesGoalShots)
        {
            State = XmlTeamsLoadSaveStates.TagPlayerGamesGoal;
        }
        else if (tag == TagPlayerCardClid ||
                 tag == TagPlayerCardYellow ||
                 tag == TagPlayerCardRed)
        {
            State = XmlTeamsLoadSaveStates.TagPlayerCard;
        }
        else if (tag != TagPlayers)
        {
            Debug.DebugPrintMessage($"xml_loadsave_teams_end_element: unknown tag: {tag}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the players module handlers when the text between
    /// tags is read. This function is responsible for filling in the player variables
    /// when a file gets loaded.
    /// </see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    private static void PlayersReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlTeamsLoadSaveStates.TagPlayerName:
                NewPlayer.Name = text;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerPos:
                NewPlayer.Pos = (PlayerPos)intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCPos:
                NewPlayer.Cpos = (PlayerPos)intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerHealth:
                NewPlayer.Health = (PlayerInjury)intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerRecovery:
                NewPlayer.Recovery = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerId:
                NewPlayer.Id = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerValue:
                NewPlayer.Value = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerWage:
                NewPlayer.Wage = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerOffers:
                NewPlayer.Offers = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerParticipation:
                NewPlayer.Participation = intValue != 0;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerSkill:
                NewPlayer.Skill = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCSkill:
                NewPlayer.Cskill = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerTalent:
                NewPlayer.Talent = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerEtal:
                NewPlayer.Etal[EtalIdx] = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerFitness:
                NewPlayer.Fitness = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerLsu:
                NewPlayer.Lsu = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerAge:
                NewPlayer.Age = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerPeakAge:
                NewPlayer.PeakAge = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerPeakRegion:
                NewPlayer.PeakRegion = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerContract:
                NewPlayer.Contract = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerGamesGoalClid:
                NewGamesGoals.Clid = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerGamesGoalGames:
                NewGamesGoals.Games = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerGamesGoalGoals:
                NewGamesGoals.Goals = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerGamesGoalShots:
                NewGamesGoals.Shots = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCardClid:
                NewCard.Clid = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCardYellow:
                NewCard.Yellow = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCardRed:
                NewCard.Red = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerCareer:
                NewPlayer.Career[CareerIdx] = intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerStreak:
                NewPlayer.Streak = (PlayerStreak)intValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerStreakCount:
                NewPlayer.StreakCount = floatValue;
                break;
            case XmlTeamsLoadSaveStates.TagPlayerStreakProb:
                NewPlayer.StreakProb = floatValue;
                break;
        }
    }

    /// <summary>
    /// <summary>
    /// Write the teams into the given file.
    /// </summary>
    /// <param name="filename">The team xml file to write.</param>
    /// <param name="teams">The teams to write.</param>
    public static void TeamsXmlLoadSaveWrite(string filename, List<Team> teams)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_4000>");
            foreach (Team team in teams)
            {
                TeamsXmlLoadSaveWriteTeam(fil, team);
            }
            fil.WriteLine("</_4000>");
        }
    }

    /// <summary>
    /// Write a single team into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="team">The team to write.</param>
    public static void TeamsXmlLoadSaveWriteTeam(TextWriter fil, Team team)
    {
        fil.WriteLine("<_4001>");
        if (team.Name != null)
            fil.WriteLine("<_0>" + team.Name + "</_0>");
        if (team.Symbol != null)
            fil.WriteLine("<_1>" + team.Symbol + "</_1>");
        if (team.NamesFile != null)
            fil.WriteLine("<_4002>" + team.NamesFile + "</_4002>");
        if (team.StrategySid != null)
            fil.WriteLine("<_4007>" + team.StrategySid + "</_4007>");
        fil.WriteLine("<_4006>" + team.Clid + "</_4006>");
        fil.WriteLine("<_10>" + team.Id + "</_10>");
        fil.WriteLine("<_4003>" + team.Structure + "</_4003>");
        fil.WriteLine("<_4004>" + team.Style + "</_4004>");
        fil.WriteLine("<_4005>" + team.Boost + "</_4005>");
        fil.WriteLine("<_4008>");
        if (team.Stadium.Name != null)
            fil.WriteLine("<_4009>" + team.Stadium.Name + "</_4009>");
        fil.WriteLine("<_4010>" + team.Stadium.Capacity + "</_4010>");
        fil.WriteLine("<_4011>" + team.Stadium.AverageAttendance + "</_4011>");
        fil.WriteLine("<_4012>" + team.Stadium.PossibleAttendance + "</_4012>");
        fil.WriteLine("<_4013>" + team.Stadium.Games + "</_4013>");
        fil.WriteLine("<_4014>" + (int)(team.Stadium.Safety * 10000) + "</_4014>");
        fil.WriteLine("<_4016>" + (int)(team.Stadium.TicketPrice * 10000) + "</_4016>");
        fil.WriteLine("</_4008>");
        fil.WriteLine("<_9000>");
        foreach (Player player in team.Players)
        {
            TeamsXmlLoadSaveWritePlayer(fil, player);
        }
        fil.WriteLine("</_9000>");
        fil.WriteLine("<_4015>" + (int)(team.Luck * 10000) + "</_4015>");
        fil.WriteLine("</_4001>");
    }

    /// <summary>
    /// Write a single player into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="player">The player to write.</param>
    public static void TeamsXmlLoadSaveWritePlayer(TextWriter fil, Player player)
    {
        fil.WriteLine("<_9001>");
        if (player.Name != null)
            fil.WriteLine("<_9002>" + player.Name + "</_9002>");
        fil.WriteLine("<_9003>" + (int)player.Pos + "</_9003>");
        fil.WriteLine("<_9004>" + (int)player.Cpos + "</_9004>");
        fil.WriteLine("<_9005>" + (int)player.Health + "</_9005>");
        fil.WriteLine("<_9006>" + player.Recovery + "</_9006>");
        fil.WriteLine("<_9007>" + player.Id + "</_9007>");
        fil.WriteLine("<_9008>" + player.Value + "</_9008>");
        fil.WriteLine("<_9009>" + player.Wage + "</_9009>");
        fil.WriteLine("<_9010>" + player.Offers + "</_9010>");
        fil.WriteLine("<_9021>" + (int)(player.Participation ? 1 : 0) + "</_9021>");
        fil.WriteLine("<_9011>" + (int)(player.Skill * 10000) + "</_9011>");
        fil.WriteLine("<_9012>" + (int)(player.Cskill * 10000) + "</_9012>");
        fil.WriteLine("<_9013>" + (int)(player.Talent * 10000) + "</_9013>");
        for (int i = 0; i < 4; i++)
        {
            fil.WriteLine("<_9014>" + (int)(player.Etal[i] * 10000) + "</_9014>");
        }
        fil.WriteLine("<_9015>" + (int)(player.Fitness * 10000) + "</_9015>");
        fil.WriteLine("<_9016>" + (int)(player.Lsu * 10000) + "</_9016>");
        fil.WriteLine("<_9017>" + (int)(player.Age * 10000) + "</_9017>");
        fil.WriteLine("<_9018>" + (int)(player.PeakAge * 10000) + "</_9018>");
        fil.WriteLine("<_9019>" + (int)(player.PeakRegion * 10000) + "</_9019>");
        fil.WriteLine("<_9020>" + (int)(player.Contract * 10000) + "</_9020>");
        for (int i = 0; i < 5; i++)
        {
            fil.WriteLine("<_9031>" + player.Career[i] + "</_9031>");
        }
        fil.WriteLine("<_9032>" + (int)player.Streak + "</_9032>");
        fil.WriteLine("<_9033>" + (int)(player.StreakCount * 10000) + "</_9033>");
        fil.WriteLine("<_9034>" + (int)(player.StreakProb * 10000) + "</_9034>");
        fil.WriteLine("</_9001>");
    }
}
