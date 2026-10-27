namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of player XML files.
/// Corresponds to the C module src/xml_loadsave_players.h and src/xml_loadsave_players.c
/// (xml_loadsave_players_start_element, xml_loadsave_players_end_element and
/// xml_loadsave_players_text functions and the
/// xml_loadsave_players_write, xml_loadsave_players_write_player functions).
/// </summary>
public static class PlayersXmlLoadSave
{
    // Tags. Corresponds to the tag enum of the players module in src/xml_loadsave_players.c.
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
    private const int TagEnd = 9035;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the players module.
    /// </summary>
    public enum XmlPlayersStates
    {
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
        TagPlayerStreakProb = 9034,
        TagEnd = 9035
    }

    private static XmlPlayersStates State = XmlPlayersStates.TagPlayers;
    private static Player NewPlayer = new Player();
    private static PlayerGamesGoals NewGamesGoals = new PlayerGamesGoals();
    private static PlayerCard NewCard = new PlayerCard();
    private static int EtalIdx = 0;
    private static int CareerIdx = 0;

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// </summary>
    /// <param name="tag">The tag of the opening element.</param>
    /// <param name="team">The team the player belongs to.</param>
    public static void PlayersReadStartElement(int tag, Team team)
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
    /// The function called by the reader when a closing tag is read.
    /// The state variable is changed in this function and the
    /// new player gets added to the list of players.
    /// </summary>
    /// <param name="tag">The tag of the closing element.</param>
    /// <param name="players">The list of players the player gets added to.</param>
    public static void PlayersReadEndElement(int tag, List<Player> players)
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
                 tag == TagPlayerCareer ||
                 tag == TagPlayerCard ||
                 tag == TagPlayerStreak ||
                 tag == TagPlayerStreakCount ||
                 tag == TagPlayerStreakProb)
        {
            State = XmlPlayersStates.TagPlayer;
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
            State = XmlPlayersStates.TagPlayerGamesGoal;
        }
        else if (tag == TagPlayerCardClid ||
                 tag == TagPlayerCardYellow ||
                 tag == TagPlayerCardRed)
        {
            State = XmlPlayersStates.TagPlayerCard;
        }
        else if (tag != TagPlayers)
        {
            Debug.DebugPrintMessage($"xml_loadsave_players_end_element: unknown tag. I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the player variables
    /// when a file gets loaded.
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void PlayersReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlPlayersStates.TagPlayerName:
                NewPlayer.Name = text;
                break;
            case XmlPlayersStates.TagPlayerPos:
                NewPlayer.Pos = (PlayerPos)intValue;
                break;
            case XmlPlayersStates.TagPlayerCPos:
                NewPlayer.Cpos = (PlayerPos)intValue;
                break;
            case XmlPlayersStates.TagPlayerHealth:
                NewPlayer.Health = (PlayerInjury)intValue;
                break;
            case XmlPlayersStates.TagPlayerRecovery:
                NewPlayer.Recovery = intValue;
                break;
            case XmlPlayersStates.TagPlayerId:
                NewPlayer.Id = intValue;
                break;
            case XmlPlayersStates.TagPlayerValue:
                NewPlayer.Value = intValue;
                break;
            case XmlPlayersStates.TagPlayerWage:
                NewPlayer.Wage = intValue;
                break;
            case XmlPlayersStates.TagPlayerOffers:
                NewPlayer.Offers = intValue;
                break;
            case XmlPlayersStates.TagPlayerParticipation:
                NewPlayer.Participation = intValue != 0;
                break;
            case XmlPlayersStates.TagPlayerSkill:
                NewPlayer.Skill = floatValue;
                break;
            case XmlPlayersStates.TagPlayerCSkill:
                NewPlayer.Cskill = floatValue;
                break;
            case XmlPlayersStates.TagPlayerTalent:
                NewPlayer.Talent = floatValue;
                break;
            case XmlPlayersStates.TagPlayerEtal:
                NewPlayer.Etal[EtalIdx] = floatValue;
                break;
            case XmlPlayersStates.TagPlayerFitness:
                NewPlayer.Fitness = floatValue;
                break;
            case XmlPlayersStates.TagPlayerLsu:
                NewPlayer.Lsu = floatValue;
                break;
            case XmlPlayersStates.TagPlayerAge:
                NewPlayer.Age = floatValue;
                break;
            case XmlPlayersStates.TagPlayerPeakAge:
                NewPlayer.PeakAge = floatValue;
                break;
            case XmlPlayersStates.TagPlayerPeakRegion:
                NewPlayer.PeakRegion = floatValue;
                break;
            case XmlPlayersStates.TagPlayerContract:
                NewPlayer.Contract = floatValue;
                break;
            case XmlPlayersStates.TagPlayerGamesGoalClid:
                NewGamesGoals.Clid = intValue;
                break;
            case XmlPlayersStates.TagPlayerGamesGoalGames:
                NewGamesGoals.Games = intValue;
                break;
            case XmlPlayersStates.TagPlayerGamesGoalGoals:
                NewGamesGoals.Goals = intValue;
                break;
            case XmlPlayersStates.TagPlayerGamesGoalShots:
                NewGamesGoals.Shots = intValue;
                break;
            case XmlPlayersStates.TagPlayerCardClid:
                NewCard.Clid = intValue;
                break;
            case XmlPlayersStates.TagPlayerCardYellow:
                NewCard.Yellow = intValue;
                break;
            case XmlPlayersStates.TagPlayerCardRed:
                NewCard.Red = intValue;
                break;
            case XmlPlayersStates.TagPlayerCareer:
                NewPlayer.Career[CareerIdx] = intValue;
                break;
            case XmlPlayersStates.TagPlayerStreak:
                NewPlayer.Streak = (PlayerStreak)intValue;
                break;
            case XmlPlayersStates.TagPlayerStreakCount:
                NewPlayer.StreakCount = floatValue;
                break;
            case XmlPlayersStates.TagPlayerStreakProb:
                NewPlayer.StreakProb = floatValue;
                break;
        }
    }

    /// <summary>
    /// Write the players into the given file.
    /// </summary>
    /// <param name="filename">The players xml file to write.</param>
    /// <param name="players">The players to write.</param>
    public static void PlayersXmlLoadSaveWrite(string filename, List<Player> players)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("\t<_9000>");
            foreach (Player player in players)
            {
                PlayersXmlLoadSaveWritePlayer(fil, player);
            }
            fil.WriteLine("\t</_9000>");
        }
    }

    /// <summary>
    /// Write a single player into the given stream.
    /// </summary>
    /// <param name="fil">The stream to write into.</param>
    /// <param name="player">The player to write.</param>
    public static void PlayersXmlLoadSaveWritePlayer(TextWriter fil, Player player)
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
