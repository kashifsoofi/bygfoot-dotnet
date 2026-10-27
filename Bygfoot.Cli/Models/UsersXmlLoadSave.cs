namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of user XML files.
/// Corresponds to the C module src/xml_loadsave_users.h and src/xml_loadsave_users.c
/// (xml_loadsave_users_read, xml_loadsave_users_write and the
/// xml_loadsave_users_start_element, xml_loadsave_users_end_element and
/// xml_loadsave_users_text functions).
/// </summary>
public static class UsersXmlLoadSave
{
    // Common tags used in the user xml files. Corresponds to the tag enum of src/xml.h.
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

    // User tags. Corresponds to the tag enum of the users module in src/xml_loadsave_users.c.
    private const int TagUsers = 7000;
    private const int TagUser = 7001;
    private const int TagUserCounter = 7002;
    private const int TagUserMoney = 7003;
    private const int TagUserDebt = 7004;
    private const int TagUserDebtInterest = 7005;
    private const int TagUserAlrStartWeek = 7006;
    private const int TagUserAlrWeeklyInstallment = 7007;
    private const int TagUserMoneyIns = 7008;
    private const int TagUserMoneyIn = 7009;
    private const int TagUserMoneyOuts = 7010;
    private const int TagUserMoneyOut = 7011;
    private const int TagUserScout = 7012;
    private const int TagUserPhysio = 7013;
    private const int TagUserHistory = 7014;
    private const int TagUserHistorySeason = 7015;
    private const int TagUserHistoryWeek = 7016;
    private const int TagUserHistoryType = 7017;
    private const int TagUserHistoryTeamName = 7018;
    private const int TagUserHistoryString = 7019;
    private const int TagUserEvent = 7020;
    private const int TagUserEventType = 7021;
    private const int TagUserEventValue1 = 7022;
    private const int TagUserEventValue2 = 7023;
    private const int TagUserEventValueString = 7024;
    private const int TagUserSponsorName = 7025;
    private const int TagUserSponsorContract = 7026;
    private const int TagUserSponsorBenefit = 7027;
    private const int TagUserYaCoach = 7028;
    private const int TagUserYaPercentage = 7029;
    private const int TagUserYaAvCoach = 7030;
    private const int TagUserYaAvPercentage = 7031;
    private const int TagUserYaCounter = 7032;
    private const int TagUserBet0 = 7033;
    private const int TagUserBet1 = 7034;
    private const int TagUserBetWager = 7035;
    private const int TagUserBetOutcome = 7036;
    private const int TagUserBetFixId = 7037;
    private const int TagUserYaPreference = 7038;
    private const int TagUserDefaultTeam = 7039;
    private const int TagUserDefaultStructure = 7040;
    private const int TagUserDefaultStyle = 7041;
    private const int TagUserDefaultBoost = 7042;
    private const int TagEnd = 7043;

    // Players tags. Corresponds to the tag enum of the players module in src/xml_loadsave_players.c.
    private const int TagStartPlayers = 9000;
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
    private const int TagEndPlayers = 9900;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the users module.
    /// </summary>
    public enum XmlUsersLoadSaveStates
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
        TagUsers = 7000,
        TagUser = 7001,
        TagUserCounter = 7002,
        TagUserMoney = 7003,
        TagUserDebt = 7004,
        TagUserDebtInterest = 7005,
        TagUserAlrStartWeek = 7006,
        TagUserAlrWeeklyInstallment = 7007,
        TagUserMoneyIns = 7008,
        TagUserMoneyIn = 7009,
        TagUserMoneyOuts = 7010,
        TagUserMoneyOut = 7011,
        TagUserScout = 7012,
        TagUserPhysio = 7013,
        TagUserHistory = 7014,
        TagUserHistorySeason = 7015,
        TagUserHistoryWeek = 7016,
        TagUserHistoryType = 7017,
        TagUserHistoryTeamName = 7018,
        TagUserHistoryString = 7019,
        TagUserEvent = 7020,
        TagUserEventType = 7021,
        TagUserEventValue1 = 7022,
        TagUserEventValue2 = 7023,
        TagUserEventValueString = 7024,
        TagUserSponsorName = 7025,
        TagUserSponsorContract = 7026,
        TagUserSponsorBenefit = 7027,
        TagUserYaCoach = 7028,
        TagUserYaPercentage = 7029,
        TagUserYaAvCoach = 7030,
        TagUserYaAvPercentage = 7031,
        TagUserYaCounter = 7032,
        TagUserBet0 = 7033,
        TagUserBet1 = 7034,
        TagUserBetWager = 7035,
        TagUserBetOutcome = 7036,
        TagUserBetFixId = 7037,
        TagUserYaPreference = 7038,
        TagUserDefaultTeam = 7039,
        TagUserDefaultStructure = 7040,
        TagUserDefaultStyle = 7041,
        TagUserDefaultBoost = 7042,
        TagEnd = 7043,
        TagStartPlayers = 9000,
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
        TagPlayerStreakProb = 9034,
        TagEndPlayers = 9900
    }

    private static XmlUsersLoadSaveStates State = XmlUsersLoadSaveStates.TagUsers;
    private static User NewUser = new User();
    private static UserHistory NewHistory = new UserHistory();
    private static Event NewEvent = new Event();
    private static BetUser NewBet = new BetUser();
    private static Player NewPlayer = new Player();
    private static PlayerCard NewCard = new PlayerCard();
    private static PlayerGamesGoals NewGamesGoals = new PlayerGamesGoals();
    private static int EtalIdx = 0;
    private static int CareerIdx = 0;
    private static int Idx = 0;
    private static int IdxMonIn = 0;
    private static int IdxMonOut = 0;
    private static int IdxCnt = 0;
    private static int IdxBet = 0;

    /// <summary>
    /// Function reading a user xml file and adding the user to the list of users.
    /// @param dirname The directory the user xml file is located in.
    /// @param basename The basename of the user xml file (e.g. 'user_00').
    /// </summary>
    /// <param name="dirname">The directory the user xml file is located in.</param>
    /// <param name="basename">The basename of the user xml file.</param>
    public static void UsersXmlLoadSaveRead(string dirname, string basename)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(basename))
            return;

        State = XmlUsersLoadSaveStates.TagUsers;
        NewUser = new User();
        NewHistory = new UserHistory();
        NewEvent = new Event();
        NewBet = new BetUser();
        NewPlayer = new Player();
        NewCard = new PlayerCard();
        NewGamesGoals = new PlayerGamesGoals();
        EtalIdx = 0;
        CareerIdx = 0;
        Idx = 0;
        IdxMonIn = 0;
        IdxMonOut = 0;
        IdxCnt = 0;
        IdxBet = 0;

        string filename = Path.Combine(dirname, basename + "___users.xml");
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
                            UsersXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            UsersXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            UsersXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        UsersXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        UsersXmlLoadSaveReadText(reader.ReadString());
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
    public static void UsersXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void UsersXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the user XML parser, dispatching
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
            case TagName:
                State = XmlUsersLoadSaveStates.TagName;
                validTag = true;
                break;
            case TagSymbol:
                State = XmlUsersLoadSaveStates.TagSymbol;
                validTag = true;
                break;
            case TagSid:
                State = XmlUsersLoadSaveStates.TagSid;
                validTag = true;
                break;
            case TagShortName:
                State = XmlUsersLoadSaveStates.TagShortName;
                validTag = true;
                break;
            case TagId:
                State = XmlUsersLoadSaveStates.TagId;
                validTag = true;
                break;
            case TagWeekGap:
                State = XmlUsersLoadSaveStates.TagWeekGap;
                validTag = true;
                break;
            case TagWeekBreak:
                State = XmlUsersLoadSaveStates.TagWeekBreak;
                validTag = true;
                break;
            case TagWeekBreakLength:
                State = XmlUsersLoadSaveStates.TagWeekBreakLength;
                validTag = true;
                break;
            case TagSkipWeeksWith:
                State = XmlUsersLoadSaveStates.TagSkipWeeksWith;
                validTag = true;
                break;
            case TagYellowRed:
                State = XmlUsersLoadSaveStates.TagYellowRed;
                validTag = true;
                break;
            case TagTeamId:
                State = XmlUsersLoadSaveStates.TagTeamId;
                validTag = true;
                break;
            case TagNamesFile:
                State = XmlUsersLoadSaveStates.TagNamesFile;
                validTag = true;
                break;
            case TagProperty:
                State = XmlUsersLoadSaveStates.TagProperty;
                validTag = true;
                break;
            case TagRound:
                State = XmlUsersLoadSaveStates.TagRound;
                validTag = true;
                break;
            case TagUsers:
                State = XmlUsersLoadSaveStates.TagUsers;
                validTag = true;
                break;
            case TagUser:
                State = XmlUsersLoadSaveStates.TagUser;
                validTag = true;
                break;
            case TagUserCounter:
                State = XmlUsersLoadSaveStates.TagUserCounter;
                validTag = true;
                break;
            case TagUserMoney:
                State = XmlUsersLoadSaveStates.TagUserMoney;
                validTag = true;
                break;
            case TagUserDebt:
                State = XmlUsersLoadSaveStates.TagUserDebt;
                validTag = true;
                break;
            case TagUserDebtInterest:
                State = XmlUsersLoadSaveStates.TagUserDebtInterest;
                validTag = true;
                break;
            case TagUserAlrStartWeek:
                State = XmlUsersLoadSaveStates.TagUserAlrStartWeek;
                validTag = true;
                break;
            case TagUserAlrWeeklyInstallment:
                State = XmlUsersLoadSaveStates.TagUserAlrWeeklyInstallment;
                validTag = true;
                break;
            case TagUserMoneyIns:
                State = XmlUsersLoadSaveStates.TagUserMoneyIns;
                validTag = true;
                break;
            case TagUserMoneyIn:
                State = XmlUsersLoadSaveStates.TagUserMoneyIn;
                validTag = true;
                break;
            case TagUserMoneyOuts:
                State = XmlUsersLoadSaveStates.TagUserMoneyOuts;
                validTag = true;
                break;
            case TagUserMoneyOut:
                State = XmlUsersLoadSaveStates.TagUserMoneyOut;
                validTag = true;
                break;
            case TagUserScout:
                State = XmlUsersLoadSaveStates.TagUserScout;
                validTag = true;
                break;
            case TagUserPhysio:
                State = XmlUsersLoadSaveStates.TagUserPhysio;
                validTag = true;
                break;
            case TagUserHistory:
                State = XmlUsersLoadSaveStates.TagUserHistory;
                validTag = true;
                break;
            case TagUserHistorySeason:
                State = XmlUsersLoadSaveStates.TagUserHistorySeason;
                validTag = true;
                break;
            case TagUserHistoryWeek:
                State = XmlUsersLoadSaveStates.TagUserHistoryWeek;
                validTag = true;
                break;
            case TagUserHistoryType:
                State = XmlUsersLoadSaveStates.TagUserHistoryType;
                validTag = true;
                break;
            case TagUserHistoryTeamName:
                State = XmlUsersLoadSaveStates.TagUserHistoryTeamName;
                validTag = true;
                break;
            case TagUserHistoryString:
                State = XmlUsersLoadSaveStates.TagUserHistoryString;
                validTag = true;
                break;
            case TagUserEvent:
                State = XmlUsersLoadSaveStates.TagUserEvent;
                validTag = true;
                break;
            case TagUserEventType:
                State = XmlUsersLoadSaveStates.TagUserEventType;
                validTag = true;
                break;
            case TagUserEventValue1:
                State = XmlUsersLoadSaveStates.TagUserEventValue1;
                validTag = true;
                break;
            case TagUserEventValue2:
                State = XmlUsersLoadSaveStates.TagUserEventValue2;
                validTag = true;
                break;
            case TagUserEventValueString:
                State = XmlUsersLoadSaveStates.TagUserEventValueString;
                validTag = true;
                break;
            case TagUserSponsorName:
                State = XmlUsersLoadSaveStates.TagUserSponsorName;
                validTag = true;
                break;
            case TagUserSponsorContract:
                State = XmlUsersLoadSaveStates.TagUserSponsorContract;
                validTag = true;
                break;
            case TagUserSponsorBenefit:
                State = XmlUsersLoadSaveStates.TagUserSponsorBenefit;
                validTag = true;
                break;
            case TagUserYaCoach:
                State = XmlUsersLoadSaveStates.TagUserYaCoach;
                validTag = true;
                break;
            case TagUserYaPercentage:
                State = XmlUsersLoadSaveStates.TagUserYaPercentage;
                validTag = true;
                break;
            case TagUserYaAvCoach:
                State = XmlUsersLoadSaveStates.TagUserYaAvCoach;
                validTag = true;
                break;
            case TagUserYaAvPercentage:
                State = XmlUsersLoadSaveStates.TagUserYaAvPercentage;
                validTag = true;
                break;
            case TagUserYaCounter:
                State = XmlUsersLoadSaveStates.TagUserYaCounter;
                validTag = true;
                break;
            case TagUserBet0:
                State = XmlUsersLoadSaveStates.TagUserBet0;
                validTag = true;
                break;
            case TagUserBet1:
                State = XmlUsersLoadSaveStates.TagUserBet1;
                validTag = true;
                break;
            case TagUserBetWager:
                State = XmlUsersLoadSaveStates.TagUserBetWager;
                validTag = true;
                break;
            case TagUserBetOutcome:
                State = XmlUsersLoadSaveStates.TagUserBetOutcome;
                validTag = true;
                break;
            case TagUserBetFixId:
                State = XmlUsersLoadSaveStates.TagUserBetFixId;
                validTag = true;
                break;
            case TagUserYaPreference:
                State = XmlUsersLoadSaveStates.TagUserYaPreference;
                validTag = true;
                break;
            case TagUserDefaultTeam:
                State = XmlUsersLoadSaveStates.TagUserDefaultTeam;
                validTag = true;
                break;
            case TagUserDefaultStructure:
                State = XmlUsersLoadSaveStates.TagUserDefaultStructure;
                validTag = true;
                break;
            case TagUserDefaultStyle:
                State = XmlUsersLoadSaveStates.TagUserDefaultStyle;
                validTag = true;
                break;
            case TagUserDefaultBoost:
                State = XmlUsersLoadSaveStates.TagUserDefaultBoost;
                validTag = true;
                break;
            case TagStartPlayers:
                State = XmlUsersLoadSaveStates.TagStartPlayers;
                validTag = true;
                break;
            case TagPlayer:
                State = XmlUsersLoadSaveStates.TagPlayer;
                validTag = true;
                break;
            case TagPlayerName:
                State = XmlUsersLoadSaveStates.TagPlayerName;
                validTag = true;
                break;
            case TagPlayerPos:
                State = XmlUsersLoadSaveStates.TagPlayerPos;
                validTag = true;
                break;
            case TagPlayerCPos:
                State = XmlUsersLoadSaveStates.TagPlayerCPos;
                validTag = true;
                break;
            case TagPlayerHealth:
                State = XmlUsersLoadSaveStates.TagPlayerHealth;
                validTag = true;
                break;
            case TagPlayerRecovery:
                State = XmlUsersLoadSaveStates.TagPlayerRecovery;
                validTag = true;
                break;
            case TagPlayerId:
                State = XmlUsersLoadSaveStates.TagPlayerId;
                validTag = true;
                break;
            case TagPlayerValue:
                State = XmlUsersLoadSaveStates.TagPlayerValue;
                validTag = true;
                break;
            case TagPlayerWage:
                State = XmlUsersLoadSaveStates.TagPlayerWage;
                validTag = true;
                break;
            case TagPlayerOffers:
                State = XmlUsersLoadSaveStates.TagPlayerOffers;
                validTag = true;
                break;
            case TagPlayerSkill:
                State = XmlUsersLoadSaveStates.TagPlayerSkill;
                validTag = true;
                break;
            case TagPlayerCSkill:
                State = XmlUsersLoadSaveStates.TagPlayerCSkill;
                validTag = true;
                break;
            case TagPlayerTalent:
                State = XmlUsersLoadSaveStates.TagPlayerTalent;
                validTag = true;
                break;
            case TagPlayerEtal:
                State = XmlUsersLoadSaveStates.TagPlayerEtal;
                validTag = true;
                break;
            case TagPlayerFitness:
                State = XmlUsersLoadSaveStates.TagPlayerFitness;
                validTag = true;
                break;
            case TagPlayerLsu:
                State = XmlUsersLoadSaveStates.TagPlayerLsu;
                validTag = true;
                break;
            case TagPlayerAge:
                State = XmlUsersLoadSaveStates.TagPlayerAge;
                validTag = true;
                break;
            case TagPlayerPeakAge:
                State = XmlUsersLoadSaveStates.TagPlayerPeakAge;
                validTag = true;
                break;
            case TagPlayerPeakRegion:
                State = XmlUsersLoadSaveStates.TagPlayerPeakRegion;
                validTag = true;
                break;
            case TagPlayerContract:
                State = XmlUsersLoadSaveStates.TagPlayerContract;
                validTag = true;
                break;
            case TagPlayerParticipation:
                State = XmlUsersLoadSaveStates.TagPlayerParticipation;
                validTag = true;
                break;
            case TagPlayerGamesGoal:
                State = XmlUsersLoadSaveStates.TagPlayerGamesGoal;
                validTag = true;
                break;
            case TagPlayerGamesGoalClid:
                State = XmlUsersLoadSaveStates.TagPlayerGamesGoalClid;
                validTag = true;
                break;
            case TagPlayerGamesGoalGames:
                State = XmlUsersLoadSaveStates.TagPlayerGamesGoalGames;
                validTag = true;
                break;
            case TagPlayerGamesGoalGoals:
                State = XmlUsersLoadSaveStates.TagPlayerGamesGoalGoals;
                validTag = true;
                break;
            case TagPlayerGamesGoalShots:
                State = XmlUsersLoadSaveStates.TagPlayerGamesGoalShots;
                validTag = true;
                break;
            case TagPlayerCard:
                State = XmlUsersLoadSaveStates.TagPlayerCard;
                validTag = true;
                break;
            case TagPlayerCardClid:
                State = XmlUsersLoadSaveStates.TagPlayerCardClid;
                validTag = true;
                break;
            case TagPlayerCardYellow:
                State = XmlUsersLoadSaveStates.TagPlayerCardYellow;
                validTag = true;
                break;
            case TagPlayerCardRed:
                State = XmlUsersLoadSaveStates.TagPlayerCardRed;
                validTag = true;
                break;
            case TagPlayerCareer:
                State = XmlUsersLoadSaveStates.TagPlayerCareer;
                validTag = true;
                break;
            case TagPlayerStreak:
                State = XmlUsersLoadSaveStates.TagPlayerStreak;
                validTag = true;
                break;
            case TagPlayerStreakCount:
                State = XmlUsersLoadSaveStates.TagPlayerStreakCount;
                validTag = true;
                break;
            case TagPlayerStreakProb:
                State = XmlUsersLoadSaveStates.TagPlayerStreakProb;
                validTag = true;
                break;
            case TagEndPlayers:
                State = XmlUsersLoadSaveStates.TagEndPlayers;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_users_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagUser)
        {
            NewUser = new User();
            NewUser.Sponsor = new UserSponsor();
            NewUser.YouthAcademy = new YouthAcademy();
            NewUser.LiveGame = new LiveGame();
            Idx = IdxMonOut = IdxMonIn = IdxCnt = 0;
        }
        else if (tag >= TagStartPlayers && tag <= TagEndPlayers)
        {
            State = XmlUsersLoadSaveStates.TagStartPlayers;
            validTag = true;

            if (tag == TagStartPlayers)
                NewUser.YouthAcademy.Players = new List<Player>();

            PlayersStartElement(tag, NewUser.Team);
        }
        else if (tag == TagUserHistory)
        {
            Idx = 0;
            NewHistory = new UserHistory();
            NewHistory.ContextStrings = new string[3];
        }
        else if (tag == TagUserEvent)
            NewEvent.ValueString = null;
        else if (tag == TagUserMoneyOuts || tag == TagUserMoneyIns)
            Idx = 0;
        else if (tag == TagUserBet0 || tag == TagUserBet1)
            IdxBet = (tag == TagUserBet1) ? 1 : 0;

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_users_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the users module
    /// are the numbers following the underscore in the tag names (e.g. '_7000').
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
    public static void UsersXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagUser)
        {
            State = XmlUsersLoadSaveStates.TagUsers;
            Variables.Users.Add(NewUser);
        }
        else if (tag == TagUserCounter ||
                 tag == TagUserMoney ||
                 tag == TagUserDebt ||
                 tag == TagUserDebtInterest ||
                 tag == TagUserAlrStartWeek ||
                 tag == TagUserAlrWeeklyInstallment ||
                 tag == TagUserMoneyIns ||
                 tag == TagUserMoneyIn ||
                 tag == TagUserMoneyOuts ||
                 tag == TagUserMoneyOut ||
                 tag == TagUserScout ||
                 tag == TagUserPhysio ||
                 tag == TagUserSponsorName ||
                 tag == TagUserSponsorContract ||
                 tag == TagUserSponsorBenefit ||
                 tag == TagName ||
                 tag == TagTeamId ||
                 tag == TagUserHistory ||
                 tag == TagUserEvent ||
                 tag == TagUserYaCoach ||
                 tag == TagUserYaPercentage ||
                 tag == TagUserYaAvCoach ||
                 tag == TagUserYaAvPercentage ||
                 tag == TagUserYaCounter ||
                 tag == TagUserBet0 ||
                 tag == TagUserBet1 ||
                 tag == TagUserDefaultTeam ||
                 tag == TagUserDefaultStructure ||
                 tag == TagUserDefaultStyle ||
                 tag == TagUserDefaultBoost)
        {
            State = XmlUsersLoadSaveStates.TagUser;
            if (tag == TagUserCounter)
                IdxCnt++;
            else if (tag == TagUserMoneyOuts)
                IdxMonOut++;
            else if (tag == TagUserMoneyIns)
                IdxMonIn++;
            else if (tag == TagUserHistory)
                NewUser.History.Add(NewHistory);
            else if (tag == TagUserEvent)
                NewUser.Events.Add(NewEvent);
            else if (tag == TagUserBet0 || tag == TagUserBet1)
                NewUser.Bets[IdxBet].Add(NewBet);
        }
        else if (tag == TagUserMoneyOut)
        {
            State = XmlUsersLoadSaveStates.TagUserMoneyOuts;
            Idx++;
        }
        else if (tag == TagUserMoneyIn)
        {
            State = XmlUsersLoadSaveStates.TagUserMoneyIns;
            Idx++;
        }
        else if (tag == TagUserHistorySeason ||
             tag == TagUserHistoryWeek ||
             tag == TagUserHistoryType ||
             tag == TagUserHistoryTeamName ||
             tag == TagUserHistoryString)
        {
            State = XmlUsersLoadSaveStates.TagUserHistory;

            if (tag == TagUserHistoryString)
                Idx++;
        }
        else if (tag == TagUserEventType ||
                 tag == TagUserEventValue1 ||
                 tag == TagUserEventValue2 ||
                 tag == TagUserEventValueString)
        {
            State = XmlUsersLoadSaveStates.TagUserEvent;
        }
        else if (tag == TagUserBetFixId ||
                 tag == TagUserBetWager ||
                 tag == TagUserBetOutcome)
        {
            State = (IdxBet == 0) ? XmlUsersLoadSaveStates.TagUserBet0 : XmlUsersLoadSaveStates.TagUserBet1;
        }
        else if (tag >= TagStartPlayers && tag <= TagEndPlayers)
        {
            PlayersEndElement(tag, NewUser.YouthAcademy.Players);
            if (tag == TagStartPlayers)
                State = XmlUsersLoadSaveStates.TagUser;
        }
        else if (tag != TagUsers)
        {
            Debug.DebugPrintMessage($"xml_loadsave_users_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the user variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void UsersXmlLoadSaveReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlUsersLoadSaveStates.TagName:
                NewUser.Name = text;
                break;
            case XmlUsersLoadSaveStates.TagTeamId:
                NewUser.TeamId = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserMoney:
                NewUser.Money = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserDebt:
                NewUser.Debt = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserDebtInterest:
                NewUser.DebtInterest = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagUserAlrStartWeek:
                NewUser.AlrStartWeek = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserAlrWeeklyInstallment:
                NewUser.AlrWeeklyInstallment = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserScout:
                NewUser.Scout = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserPhysio:
                NewUser.Physio = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserSponsorName:
            {
                string buf = text.Replace("AND", "&amp;");
                if (NewUser.Sponsor != null)
                    NewUser.Sponsor.Name = buf;
                break;
            }
            case XmlUsersLoadSaveStates.TagUserSponsorContract:
                if (NewUser.Sponsor != null)
                    NewUser.Sponsor.Contract = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserSponsorBenefit:
                if (NewUser.Sponsor != null)
                    NewUser.Sponsor.Benefit = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserCounter:
                NewUser.Counters[IdxCnt] = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserMoneyIn:
                NewUser.MoneyIn[IdxMonIn, Idx] = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserMoneyOut:
                NewUser.MoneyOut[IdxMonOut, Idx] = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserHistorySeason:
                NewHistory.Season = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserHistoryWeek:
                NewHistory.Week = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserHistoryType:
                NewHistory.Type = (UserHistoryType)intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserHistoryTeamName:
                NewHistory.TeamName = text;
                break;
            case XmlUsersLoadSaveStates.TagUserHistoryString:
                NewHistory.ContextStrings[Idx] = (text.Length == 0) ? null : text;
                break;
            case XmlUsersLoadSaveStates.TagUserEventType:
                NewEvent.Type = (EventType)intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserEventValue1:
                NewEvent.Value1 = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserEventValue2:
                NewEvent.Value2 = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserEventValueString:
                NewEvent.ValueString = text;
                break;
            case XmlUsersLoadSaveStates.TagUserYaCoach:
                NewUser.YouthAcademy.Coach = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserYaPercentage:
                NewUser.YouthAcademy.Percentage = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserYaAvCoach:
                NewUser.YouthAcademy.AvCoach = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagUserYaAvPercentage:
                NewUser.YouthAcademy.AvPercentage = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagUserYaCounter:
                NewUser.YouthAcademy.CounterYouth = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagUserYaPreference:
                NewUser.YouthAcademy.PosPref = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserBetWager:
                NewBet.Wager = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserBetOutcome:
                NewBet.Outcome = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserBetFixId:
                NewBet.FixId = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserDefaultTeam:
                NewUser.DefaultTeam.Add(intValue);
                break;
            case XmlUsersLoadSaveStates.TagUserDefaultStructure:
                NewUser.DefaultStructure = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserDefaultStyle:
                NewUser.DefaultStyle = intValue;
                break;
            case XmlUsersLoadSaveStates.TagUserDefaultBoost:
                NewUser.DefaultBoost = intValue;
                break;
            case XmlUsersLoadSaveStates.TagStartPlayers:
            case XmlUsersLoadSaveStates.TagPlayer:
            case XmlUsersLoadSaveStates.TagPlayerName:
            case XmlUsersLoadSaveStates.TagPlayerPos:
            case XmlUsersLoadSaveStates.TagPlayerCPos:
            case XmlUsersLoadSaveStates.TagPlayerHealth:
            case XmlUsersLoadSaveStates.TagPlayerRecovery:
            case XmlUsersLoadSaveStates.TagPlayerId:
            case XmlUsersLoadSaveStates.TagPlayerValue:
            case XmlUsersLoadSaveStates.TagPlayerWage:
            case XmlUsersLoadSaveStates.TagPlayerOffers:
            case XmlUsersLoadSaveStates.TagPlayerSkill:
            case XmlUsersLoadSaveStates.TagPlayerCSkill:
            case XmlUsersLoadSaveStates.TagPlayerTalent:
            case XmlUsersLoadSaveStates.TagPlayerEtal:
            case XmlUsersLoadSaveStates.TagPlayerFitness:
            case XmlUsersLoadSaveStates.TagPlayerLsu:
            case XmlUsersLoadSaveStates.TagPlayerAge:
            case XmlUsersLoadSaveStates.TagPlayerPeakAge:
            case XmlUsersLoadSaveStates.TagPlayerPeakRegion:
            case XmlUsersLoadSaveStates.TagPlayerContract:
            case XmlUsersLoadSaveStates.TagPlayerParticipation:
            case XmlUsersLoadSaveStates.TagPlayerGamesGoal:
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalClid:
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalGames:
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalGoals:
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalShots:
            case XmlUsersLoadSaveStates.TagPlayerCard:
            case XmlUsersLoadSaveStates.TagPlayerCardClid:
            case XmlUsersLoadSaveStates.TagPlayerCardYellow:
            case XmlUsersLoadSaveStates.TagPlayerCardRed:
            case XmlUsersLoadSaveStates.TagPlayerCareer:
            case XmlUsersLoadSaveStates.TagPlayerStreak:
            case XmlUsersLoadSaveStates.TagPlayerStreakCount:
            case XmlUsersLoadSaveStates.TagPlayerStreakProb:
                UsersXmlLoadSaveReadPlayerText(text);
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between player tags is read.
    /// This function is responsible for filling in the player variables
    /// when a file gets loaded.
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    private static void UsersXmlLoadSaveReadPlayerText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlUsersLoadSaveStates.TagPlayerName:
                NewPlayer.Name = text;
                break;
            case XmlUsersLoadSaveStates.TagPlayerPos:
                NewPlayer.Pos = (PlayerPos)intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCPos:
                NewPlayer.Cpos = (PlayerPos)intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerHealth:
                NewPlayer.Health = (PlayerInjury)intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerRecovery:
                NewPlayer.Recovery = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerId:
                NewPlayer.Id = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerValue:
                NewPlayer.Value = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerWage:
                NewPlayer.Wage = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerOffers:
                NewPlayer.Offers = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerParticipation:
                NewPlayer.Participation = intValue != 0;
                break;
            case XmlUsersLoadSaveStates.TagPlayerSkill:
                NewPlayer.Skill = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCSkill:
                NewPlayer.Cskill = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerTalent:
                NewPlayer.Talent = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerEtal:
                NewPlayer.Etal[EtalIdx] = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerFitness:
                NewPlayer.Fitness = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerLsu:
                NewPlayer.Lsu = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerAge:
                NewPlayer.Age = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerPeakAge:
                NewPlayer.PeakAge = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerPeakRegion:
                NewPlayer.PeakRegion = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerContract:
                NewPlayer.Contract = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalClid:
                NewGamesGoals.Clid = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalGames:
                NewGamesGoals.Games = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalGoals:
                NewGamesGoals.Goals = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerGamesGoalShots:
                NewGamesGoals.Shots = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCardClid:
                NewCard.Clid = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCardYellow:
                NewCard.Yellow = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCardRed:
                NewCard.Red = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerCareer:
                NewPlayer.Career[CareerIdx] = intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerStreak:
                NewPlayer.Streak = (PlayerStreak)intValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerStreakCount:
                NewPlayer.StreakCount = floatValue;
                break;
            case XmlUsersLoadSaveStates.TagPlayerStreakProb:
                NewPlayer.StreakProb = floatValue;
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the players module handlers
    /// are called with an opening tag. The state variable is changed in this function
    /// and sometimes memory allocated for the information that's going to be read.
    /// </summary>
    /// <param name="tag">The tag of the opening element.</param>
    /// <param name="team">The team the player belongs to.</param>
    private static void PlayersStartElement(int tag, Team team)
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
    /// The function called by the reader when the players module handlers
    /// are called with a closing tag. The state variable is changed in this function.
    /// </summary>
    /// <param name="tag">The tag of the closing element.</param>
    /// <param name="players">The list of players the player gets added to.</param>
    private static void PlayersEndElement(int tag, List<Player> players)
    {
        if (tag == TagPlayer)
        {
            players.Add(NewPlayer);
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
            State = XmlUsersLoadSaveStates.TagPlayer;
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
            State = XmlUsersLoadSaveStates.TagPlayerGamesGoal;
        }
        else if (tag == TagPlayerCardClid ||
                 tag == TagPlayerCardYellow ||
                 tag == TagPlayerCardRed)
        {
            State = XmlUsersLoadSaveStates.TagPlayerCard;
        }
        else if (tag != TagStartPlayers)
        {
            Debug.DebugPrintMessage($"xml_loadsave_users_end_element: unknown tag: {tag}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Write the users into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the user xml file (e.g. 'user_0').</param>
    public static void UsersXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___users.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_7000>");
            foreach (User user in Variables.Users)
            {
                UsersXmlLoadSaveWriteUser(fil, user);
            }
            fil.WriteLine("</_7000>");
        }
    }

    /// <summary>
    /// Write a single user into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="user">The user to write.</param>
    public static void UsersXmlLoadSaveWriteUser(TextWriter fil, User user)
    {
        fil.WriteLine("<_7001>");
        if (user.Name != null)
            fil.WriteLine("<_0>" + user.Name + "</_0>");
        fil.WriteLine("<_10>" + user.TeamId + "</_10>");
        fil.WriteLine("<_7003>" + user.Money + "</_7003>");
        fil.WriteLine("<_7004>" + user.Debt + "</_7004>");
        fil.WriteLine("<_7005>" + (int)(user.DebtInterest * 10000) + "</_7005>");
        fil.WriteLine("<_7006>" + user.AlrStartWeek + "</_7006>");
        fil.WriteLine("<_7007>" + user.AlrWeeklyInstallment + "</_7007>");
        fil.WriteLine("<_7012>" + user.Scout + "</_7012>");
        fil.WriteLine("<_7013>" + user.Physio + "</_7013>");
        if (user.Sponsor != null && user.Sponsor.Name != null)
        {
            string sponsorName = user.Sponsor.Name.Replace("&amp;", "AND");
            fil.WriteLine("<_7025>" + sponsorName + "</_7025>");
        }
        if (user.Sponsor != null)
        {
            fil.WriteLine("<_7026>" + user.Sponsor.Contract + "</_7026>");
            fil.WriteLine("<_7027>" + user.Sponsor.Benefit + "</_7027>");
        }
        UsersXmlLoadSaveWriteHistory(fil, user.History);
        UsersXmlLoadSaveWriteEvents(fil, user.Events);
        UsersXmlLoadSaveWriteBets(fil, user.Bets);
        for (int j = 0; j < user.Counters.Length; j++)
        {
            fil.WriteLine("<_7002>" + user.Counters[j] + "</_7002>");
        }
        for (int j = 0; j < 2; j++)
        {
            fil.WriteLine("\t<_7008>");
            for (int k = 0; k < 6; k++)
            {
                fil.WriteLine("\t\t<_7009>" + user.MoneyIn[j, k] + "</_7009>");
            }
            fil.WriteLine("\t</_7008>");
        }
        for (int j = 0; j < 2; j++)
        {
            fil.WriteLine("\t<_7010>");
            for (int k = 0; k < 14; k++)
            {
                fil.WriteLine("\t\t<_7011>" + user.MoneyOut[j, k] + "</_7011>");
            }
            fil.WriteLine("\t</_7010>");
        }
        if (user.YouthAcademy != null)
        {
            fil.WriteLine("<_7028>" + user.YouthAcademy.Coach + "</_7028>");
            fil.WriteLine("<_7029>" + user.YouthAcademy.Percentage + "</_7029>");
            fil.WriteLine("<_7030>" + (int)(user.YouthAcademy.AvCoach * 10000) + "</_7030>");
            fil.WriteLine("<_7031>" + (int)(user.YouthAcademy.AvPercentage * 10000) + "</_7031>");
            fil.WriteLine("<_7032>" + (int)(user.YouthAcademy.CounterYouth * 10000) + "</_7032>");
            fil.WriteLine("<_7038>" + user.YouthAcademy.PosPref + "</_7038>");
        }
        if (user.DefaultTeam != null)
        {
            foreach (int teamId in user.DefaultTeam)
            {
                fil.WriteLine("<_7039>" + teamId + "</_7039>");
            }
        }
        fil.WriteLine("<_7040>" + user.DefaultStructure + "</_7040>");
        fil.WriteLine("<_7041>" + user.DefaultStyle + "</_7041>");
        fil.WriteLine("<_7042>" + user.DefaultBoost + "</_7042>");
        UsersXmlLoadSaveWritePlayers(fil, user.YouthAcademy?.Players ?? new List<Player>());
        fil.WriteLine("</_7001>");
    }

    /// <summary>
    /// Write the history of a user into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="history">The history to write.</param>
    public static void UsersXmlLoadSaveWriteHistory(TextWriter fil, List<UserHistory> history)
    {
        foreach (UserHistory entry in history)
        {
            fil.WriteLine("\t<_7014>");
            fil.WriteLine("\t\t<_7015>" + entry.Season + "</_7015>");
            fil.WriteLine("\t\t<_7016>" + entry.Week + "</_7016>");
            fil.WriteLine("\t\t<_7017>" + (int)entry.Type + "</_7017>");
            if (entry.TeamName != null)
                fil.WriteLine("\t\t<_7018>" + entry.TeamName + "</_7018>");
            for (int j = 0; j < 3; j++)
            {
                fil.WriteLine("\t\t<_7019>" + entry.ContextStrings[j] + "</_7019>");
            }
            fil.WriteLine("\t</_7014>");
        }
    }

    /// <summary>
    /// Write the events of a user into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="events">The events to write.</param>
    public static void UsersXmlLoadSaveWriteEvents(TextWriter fil, List<Event> events)
    {
        foreach (Event entry in events)
        {
            fil.WriteLine("\t<_7020>");
            fil.WriteLine("\t\t<_7021>" + (int)entry.Type + "</_7021>");
            fil.WriteLine("\t\t<_7022>" + entry.Value1 + "</_7022>");
            fil.WriteLine("\t\t<_7023>" + entry.Value2 + "</_7023>");
            fil.WriteLine("\t\t<_7024>" + entry.ValueString + "</_7024>");
            fil.WriteLine("\t</_7020>");
        }
    }

    /// <summary>
    /// Write the bets of a user into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="bets">The bets to write.</param>
    public static void UsersXmlLoadSaveWriteBets(TextWriter fil, List<BetUser>[] bets)
    {
        for (int i = 0; i < 2; i++)
        {
            int betTag = (i == 0) ? TagUserBet0 : TagUserBet1;
            fil.WriteLine($"\t<_{betTag}>");
            for (int j = 0; j < bets[i].Count; j++)
            {
                BetUser bet = bets[i][j];
                fil.WriteLine($"\t\t<_7035>" + bet.Wager + "</_7035>");
                fil.WriteLine($"\t\t<_7036>" + bet.Outcome + "</_7036>");
                fil.WriteLine($"\t\t<_7037>" + bet.FixId + "</_7037>");
            }
            fil.WriteLine($"\t</_{betTag}>");
        }
    }

    /// <summary>
    /// Write the players of a user into the file.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="players">The players to write.</param>
    public static void UsersXmlLoadSaveWritePlayers(TextWriter fil, List<Player> players)
    {
        fil.WriteLine("\t<_9000>");
        foreach (Player player in players)
        {
            UsersXmlLoadSaveWritePlayer(fil, player);
        }
        fil.WriteLine("\t</_9000>");
    }

    /// <summary>
    /// Write a single player into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="player">The player to write.</param>
    public static void UsersXmlLoadSaveWritePlayer(TextWriter fil, Player player)
    {
        fil.WriteLine("\t\t<_9001>");
        if (player.Name != null)
            fil.WriteLine("\t\t\t<_9002>" + player.Name + "</_9002>");
        fil.WriteLine("\t\t\t<_9003>" + (int)player.Pos + "</_9003>");
        fil.WriteLine("\t\t\t<_9004>" + (int)player.Cpos + "</_9004>");
        fil.WriteLine("\t\t\t<_9005>" + (int)player.Health + "</_9005>");
        fil.WriteLine("\t\t\t<_9006>" + player.Recovery + "</_9006>");
        fil.WriteLine("\t\t\t<_9007>" + player.Id + "</_9007>");
        fil.WriteLine("\t\t\t<_9008>" + player.Value + "</_9008>");
        fil.WriteLine("\t\t\t<_9009>" + player.Wage + "</_9009>");
        fil.WriteLine("\t\t\t<_9010>" + player.Offers + "</_9010>");
        fil.WriteLine("\t\t\t<_9021>" + (int)(player.Participation ? 1 : 0) + "</_9021>");
        fil.WriteLine("\t\t\t<_9011>" + (int)(player.Skill * 10000) + "</_9011>");
        fil.WriteLine("\t\t\t<_9012>" + (int)(player.Cskill * 10000) + "</_9012>");
        fil.WriteLine("\t\t\t<_9013>" + (int)(player.Talent * 10000) + "</_9013>");
        for (int i = 0; i < 4; i++)
        {
            fil.WriteLine("\t\t\t<_9014>" + (int)(player.Etal[i] * 10000) + "</_9014>");
        }
        fil.WriteLine("\t\t\t<_9015>" + (int)(player.Fitness * 10000) + "</_9015>");
        fil.WriteLine("\t\t\t<_9016>" + (int)(player.Lsu * 10000) + "</_9016>");
        fil.WriteLine("\t\t\t<_9017>" + (int)(player.Age * 10000) + "</_9017>");
        fil.WriteLine("\t\t\t<_9018>" + (int)(player.PeakAge * 10000) + "</_9018>");
        fil.WriteLine("\t\t\t<_9019>" + (int)(player.PeakRegion * 10000) + "</_9019>");
        fil.WriteLine("\t\t\t<_9020>" + (int)(player.Contract * 10000) + "</_9020>");
        for (int i = 0; i < 5; i++)
        {
            fil.WriteLine("\t\t\t<_9031>" + player.Career[i] + "</_9031>");
        }
        fil.WriteLine("\t\t\t<_9032>" + (int)player.Streak + "</_9032>");
        fil.WriteLine("\t\t\t<_9033>" + (int)(player.StreakCount * 10000) + "</_9033>");
        fil.WriteLine("\t\t\t<_9034>" + (int)(player.StreakProb * 10000) + "</_9034>");
        fil.WriteLine("\t\t</_9001>");
    }
}
