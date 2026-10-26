namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of team definition XML files.
/// Corresponds to the C module src/xml_team.h and src/xml_team.c
/// (xml_team_read, xml_team_read_start_element, xml_team_read_end_element
/// and xml_team_read_text).
/// </summary>
public static class TeamXml
{
    private const string TagTeam = "team";
    private const string TagTeamName = "team_name";
    private const string TagStadiumName = "stadium_name";
    private const string TagSymbol = "symbol";
    private const string TagAverageTalent = "average_talent";
    private const string TagFormation = "formation";
    private const string TagNamesFile = "names_file";
    private const string TagPlayer = "player";
    private const string TagPlayerName = "player_name";
    private const string TagPlayerBirthYear = "birth_year";
    private const string TagPlayerBirthMonth = "birth_month";
    private const string TagPlayerSkill = "skill";
    private const string TagPlayerTalent = "talent";
    private const string TagPlayerPosition = "position";

    private const float MaxPlayerSkill = 9900000f;
    private const int MaxCpuPlayers = 20;
    private const float FloatPlayerAverageTalentVariance = 7500f;
    private const float FloatPlayerPeakAgeLower = 2900000f;
    private const float FloatPlayerPeakAgeUpper = 3200000f;
    private const float FloatPlayerPeakAgeGoalieAddition = 200000f;
    private const float FloatPlayerPeakRegionLower = 150000f;
    private const float FloatPlayerPeakRegionUpper = 400000f;
    private const float FloatPlayerFitnessLower = 85000f;
    private const float FloatPlayerFitnessUpper = 100000f;
    private const float FloatPlayerContractLower = 100000f;
    private const float FloatPlayerContractUpper = 400000f;
    private const float FloatPlayerLsuLower = 200000f;
    private const float FloatPlayerLsuUpper = 1000000f;
    private const float FloatPlayerPosBound1 = 37550f;
    private const float FloatPlayerPosBound2 = 75100f;
    private const float FloatTeamStadiumSafetyLower = 80000f;
    private const float FloatTeamStadiumSafetyUpper = 100000f;
    private const float FloatTeamStadiumSizeWageFactor = 180000f;

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlTeamStates.
    /// </summary>
    public enum XmlTeamStates
    {
        StateTeam = 0,
        StateTeamName,
        StateStadiumName,
        StateSymbol,
        StateAverageTalent,
        StateFormation,
        StateNamesFile,
        StatePlayer,
        StatePlayerName,
        StatePlayerBirthYear,
        StatePlayerBirthMonth,
        StatePlayerSkill,
        StatePlayerTalent,
        StatePlayerPosition,
        StateEnd
    }

    private static XmlTeamStates State = XmlTeamStates.StateTeam;
    private static int BirthYear = 0;
    private static Player NewPlayer = new Player();
    private static Team Tm = new Team();
    private static string DefFile = string.Empty;

    /// <summary>
    /// Function reading a team definition file and writing the team accordingly.
    /// @param tm The team we read the definition for.
    /// @param defFile The team definition file to read (e.g. 'team_defenders.xml').
    /// </summary>
    /// <param name="tm">The team to read the definition for.</param>
    /// <param name="defFile">The team definition file to read.</param>
    public static void XmlTeamRead(Team tm, string defFile)
    {
        if (string.IsNullOrEmpty(defFile) || !File.Exists(defFile))
            return;

        State = XmlTeamStates.StateTeam;
        Tm = tm;
        DefFile = defFile;

        using (var reader = XmlReader.Create(defFile))
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
                            XmlTeamReadStartElement(reader, reader.LocalName);
                            XmlTeamReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlTeamReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlTeamReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlTeamReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }

        TeamCompleteDef(Tm);
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void XmlTeamReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlTeamReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the team XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagTeam:
                State = XmlTeamStates.StateTeam;
                break;
            case TagTeamName:
                State = XmlTeamStates.StateTeamName;
                break;
            case TagStadiumName:
                State = XmlTeamStates.StateStadiumName;
                break;
            case TagSymbol:
                State = XmlTeamStates.StateSymbol;
                break;
            case TagAverageTalent:
                State = XmlTeamStates.StateAverageTalent;
                break;
            case TagFormation:
                State = XmlTeamStates.StateFormation;
                break;
            case TagNamesFile:
                State = XmlTeamStates.StateNamesFile;
                break;
            case TagPlayer:
                State = XmlTeamStates.StatePlayer;
                NewPlayer = PlayerNew(Tm, (Tm.AverageTalent / 10000) * MaxPlayerSkill, true);
                break;
            case TagPlayerName:
                State = XmlTeamStates.StatePlayerName;
                break;
            case TagPlayerBirthYear:
                State = XmlTeamStates.StatePlayerBirthYear;
                break;
            case TagPlayerBirthMonth:
                State = XmlTeamStates.StatePlayerBirthMonth;
                break;
            case TagPlayerSkill:
                State = XmlTeamStates.StatePlayerSkill;
                break;
            case TagPlayerTalent:
                State = XmlTeamStates.StatePlayerTalent;
                break;
            case TagPlayerPosition:
                State = XmlTeamStates.StatePlayerPosition;
                break;
            default:
                Debug.DebugPrintMessage($"xml_team_read_start_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void XmlTeamReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagTeamName:
            case TagStadiumName:
            case TagSymbol:
            case TagAverageTalent:
            case TagFormation:
            case TagNamesFile:
            case TagPlayer:
                State = XmlTeamStates.StateTeam;
                if (elementName == TagPlayer)
                {
                    if (Tm.Players.Count == MaxCpuPlayers)
                    {
                        Debug.DebugPrintMessage($"xml_team_read_end_element: too many players in team definition '{DefFile}' (only {MaxCpuPlayers} allowed).");
                    }
                    else
                    {
                        Tm.Players.Add(NewPlayer);
                    }
                }
                break;
            case TagPlayerName:
            case TagPlayerBirthYear:
            case TagPlayerBirthMonth:
            case TagPlayerSkill:
            case TagPlayerTalent:
            case TagPlayerPosition:
                State = XmlTeamStates.StatePlayer;
                break;
            case TagTeam:
                break;
            default:
                Debug.DebugPrintMessage($"xml_team_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the team and player variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void XmlTeamReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlTeamStates.StateTeamName:
                Tm.Name = text;
                break;
            case XmlTeamStates.StateStadiumName:
                Tm.Stadium.Name = text;
                break;
            case XmlTeamStates.StateSymbol:
                Tm.Symbol = text;
                break;
            case XmlTeamStates.StateAverageTalent:
                if (Variables.Options.GetInt("int_opt_load_defs") == 1)
                    Tm.AverageTalent = (floatValue / 10000) * MaxPlayerSkill;
                break;
            case XmlTeamStates.StateFormation:
                Tm.Structure = intValue;
                break;
            case XmlTeamStates.StateNamesFile:
                Tm.NamesFile = text;
                break;
            case XmlTeamStates.StatePlayerName:
                NewPlayer.Name = text;
                break;
            case XmlTeamStates.StatePlayerBirthYear:
                if (Variables.Options.GetInt("int_opt_load_defs") == 1)
                    BirthYear = intValue;
                break;
            case XmlTeamStates.StatePlayerBirthMonth:
                if (Variables.Options.GetInt("int_opt_load_defs") == 1)
                    NewPlayer.Age = (float)Misc.MiscGetAgeFromBirth(BirthYear, intValue);
                break;
            case XmlTeamStates.StatePlayerSkill:
                if (Variables.Options.GetInt("int_opt_load_defs") == 1)
                    NewPlayer.Skill = ((float)intValue / 10000) * MaxPlayerSkill;
                break;
            case XmlTeamStates.StatePlayerTalent:
                if (Variables.Options.GetInt("int_opt_load_defs") == 1)
                    NewPlayer.Talent = ((float)intValue / 10000) * MaxPlayerSkill;
                break;
            case XmlTeamStates.StatePlayerPosition:
                NewPlayer.Pos = (PlayerPos)intValue;
                break;
        }
    }

    /// <summary>
    /// Complete the definition of the team (add players,
    /// calculate wages etc. Called after reading a team def file).
    /// </summary>
    /// <param name="team">The team to complete.</param>
    private static void TeamCompleteDef(Team team)
    {
        int i, newPos, posSum;
        int[] positions = { 0, 0, 0, 0 };
        Player newPlayer;

        int add = MaxCpuPlayers - team.Players.Count;

        for (i = 0; i < team.Players.Count; i++)
        {
            PlayerCompleteDef(team.Players[i], team.AverageTalent);
            positions[(int)team.Players[i].Pos]++;

            if (TeamIsUser(team) != -1)
                team.Players[i].Recovery = 1;
        }

        for (i = 0; i < add; i++)
        {
            posSum = positions[0] + positions[1] + positions[2] + positions[3];
            newPlayer = PlayerNew(team, team.AverageTalent, true);

            if (positions[0] < 2)
                newPos = 0;
            else if ((float)positions[1] / posSum < FloatPlayerPosBound1)
                newPos = 1;
            else if ((float)positions[2] / posSum < FloatPlayerPosBound1)
                newPos = 2;
            else
                newPos = 3;

            newPlayer.Pos = newPlayer.Cpos = (PlayerPos)newPos;
            positions[newPos]++;

            team.Players.Add(newPlayer);
        }

        TeamCompleteDefSort(team);
    }

    /// <summary>
    /// Complete the player structure (some values have been read
    /// from a team def file).
    /// </summary>
    /// <param name="player">The player to complete.</param>
    /// <param name="averageTalent">The average talent of the team.</param>
    private static void PlayerCompleteDef(Player player, float averageTalent)
    {
        float skillFactor = (float)(new Random().NextDouble() * 2 - 1) * FloatPlayerAverageTalentVariance;

        player.PeakAge = (float)(FloatPlayerPeakAgeLower + (player.Pos == PlayerPos.Goalie ? FloatPlayerPeakAgeGoalieAddition : 0) + (FloatPlayerPeakAgeUpper - FloatPlayerPeakAgeLower) * (float)(new Random().NextDouble()));
        player.PeakRegion = (float)(FloatPlayerPeakRegionLower + (FloatPlayerPeakRegionUpper - FloatPlayerPeakRegionLower) * (float)(new Random().NextDouble()));

        if (Variables.Options.GetInt("int_opt_load_defs") == 2)
        {
            player.Age = (float)(FloatPlayerFitnessLower + (FloatPlayerFitnessUpper - FloatPlayerFitnessLower) * (float)(new Random().NextDouble()));
            player.Talent = (float)Math.Min(averageTalent * skillFactor, MaxPlayerSkill);
            player.Skill = (float)Math.Min(averageTalent * skillFactor, MaxPlayerSkill);
        }

        player.Cpos = player.Pos;
        player.Cskill = player.Skill;
        player.Fitness = (float)(FloatPlayerFitnessLower + (FloatPlayerFitnessUpper - FloatPlayerFitnessLower) * (float)(new Random().NextDouble()));
        player.Health = 0;
        player.Recovery = 0;
        player.Value = 0;
        player.Wage = 0;
        player.Contract = (float)(FloatPlayerContractLower + (FloatPlayerContractUpper - FloatPlayerContractLower) * (float)(new Random().NextDouble()));
        player.Lsu = (float)(FloatPlayerLsuLower + (FloatPlayerLsuUpper - FloatPlayerLsuLower) * (float)(new Random().NextDouble()));
        player.Cards = new List<PlayerCard>();
        player.GamesGoals = new List<PlayerGamesGoals>();
        player.Participation = false;
        player.Offers = 0;
    }

    /// <summary>
    /// Sort the players in the team according to the team structure
    /// and the player positions.
    /// </summary>
    /// <param name="team">The team to sort.</param>
    private static void TeamCompleteDefSort(Team team)
    {
        int i, j;
        int[] positions = { 0, 0, 0, 0 };
        int[] structure = { 1, MathGetPlace(team.Structure, 3), MathGetPlace(team.Structure, 2), MathGetPlace(team.Structure, 1) };
        Player playerTmp = new Player();
        Player playerTmp2 = new Player();

        for (i = 0; i < team.Players.Count && i < 11; i++)
            positions[(int)team.Players[i].Pos]++;

        for (i = 0; i < 4; i++)
        {
            while (positions[i] > structure[i])
            {
                for (j = 0; j < team.Players.Count; j++)
                {
                    if ((int)team.Players[j].Pos == i)
                    {
                        playerTmp = team.Players[j];
                        team.Players.RemoveAt(j);
                        break;
                    }
                }

                for (j = team.Players.Count - 1; j >= 0; j--)
                {
                    if ((int)team.Players[j].Pos != i)
                    {
                        playerTmp2 = team.Players[j];
                        team.Players.RemoveAt(j);
                        break;
                    }
                }

                positions[i]--;
                positions[(int)playerTmp2.Pos]++;

                team.Players.Add(playerTmp);
                team.Players.Insert(0, playerTmp2);
            }

            while (positions[i] < structure[i])
            {
                for (j = 0; j < team.Players.Count; j++)
                {
                    if ((int)team.Players[j].Pos > i)
                    {
                        playerTmp = team.Players[j];
                        team.Players.RemoveAt(j);
                        break;
                    }
                }

                for (j = team.Players.Count - 1; j >= 0; j--)
                {
                    if ((int)team.Players[j].Pos == i)
                    {
                        playerTmp2 = team.Players[j];
                        team.Players.RemoveAt(j);
                        break;
                    }
                }

                positions[i]++;
                positions[(int)playerTmp.Pos]--;

                team.Players.Add(playerTmp);
                team.Players.Insert(0, playerTmp2);
            }
        }

        TeamRearrange(team);
    }

    /// <summary>
    /// Rearrange the players of the team according to the team structure.
    /// </summary>
    /// <param name="team">The team to rearrange.</param>
    private static void TeamRearrange(Team team)
    {
        for (int i = 0; i < team.Players.Count && i < 11; i++)
        {
            team.Players[i].Cpos = (PlayerPos)PosFromStructure(team.Structure, i);
            if (team.Players[i].Cskill > 0)
                team.Players[i].Cskill = team.Players[i].Skill;
        }
    }

    /// <summary>
    /// Return the appropriate position for the player with the given number.
    /// The position depends on the team structure if the player number is &lt; 11
    /// and on some constants otherwise.
    /// </summary>
    /// <param name="structure">The team structure, something like 442 or 352.</param>
    /// <param name="playerNumber">The number of the player within the team.</param>
    /// <returns>A new position for the player.</returns>
    private static int PosFromStructure(int structure, int playerNumber)
    {
        int position = (int)PlayerPos.Goalie;
        int bound0 = MathGetPlace(structure, 3) + 1;
        int bound1 = MathGetPlace(structure, 3) + MathGetPlace(structure, 2) + 1;

        if (playerNumber == 0 || playerNumber == 11)
            position = (int)PlayerPos.Goalie;
        else if (playerNumber < bound0 || (playerNumber > 10 && playerNumber < (11 + (MaxCpuPlayers - 11) * FloatPlayerPosBound1)))
            position = (int)PlayerPos.Defender;
        else if (playerNumber < bound1 || (playerNumber > 10 && playerNumber < (11 + (MaxCpuPlayers - 11) * FloatPlayerPosBound2)))
            position = (int)PlayerPos.Midfielder;
        else
            position = (int)PlayerPos.Forward;

        return position;
    }

    /// <summary>
    /// Return the nth place of the given value. Corresponds to the C function math_get_place.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="place">The place.</param>
    /// <returns>The place value.</returns>
    private static int MathGetPlace(int value, int place)
    {
        if (place < 10)
            return (int)((value % (int)Math.Pow(10, place) - value % (int)Math.Pow(10, place - 1)) / Math.Pow(10, place - 1));

        if (place < 20)
        {
            while (value >= (int)Math.Pow(10, place % 10))
                value = (value - value % 10) / 10;

            return value;
        }

        return value % (int)Math.Pow(10, place % 10);
    }

    /// <summary>
    /// Create a new player for the team. Corresponds to the C function player_new.
    /// </summary>
    /// <param name="team">The team the player belongs to.</param>
    /// <param name="averageTalent">The average talent of the team.</param>
    /// <param name="newId">Whether the player gets a new id.</param>
    /// <returns>A new player.</returns>
    private static Player PlayerNew(Team team, float averageTalent, bool newId)
    {
        var player = new Player
        {
            Team = team,
            Name = newId ? team.NamesFile : string.Empty,
            Skill = (float)Math.Min(averageTalent, MaxPlayerSkill),
            Talent = (float)Math.Min(averageTalent, MaxPlayerSkill),
            Fitness = (float)(FloatPlayerFitnessLower + (FloatPlayerFitnessUpper - FloatPlayerFitnessLower) * (float)(new Random().NextDouble())),
            Contract = (float)(FloatPlayerContractLower + (FloatPlayerContractUpper - FloatPlayerContractLower) * (float)(new Random().NextDouble())),
            Lsu = (float)(FloatPlayerLsuLower + (FloatPlayerLsuUpper - FloatPlayerLsuLower) * (float)(new Random().NextDouble()))
        };

        player.Pos = (PlayerPos)PosFromStructure(team.Structure, team.Players.Count);
        player.Cpos = player.Pos;

        return player;
    }

    /// <summary>
    /// Check whether the team is a user team. Corresponds to the C function team_is_user.
    /// </summary>
    /// <param name="team">The team to check.</param>
    /// <returns>The index of the user team or -1 if the team is not a user team.</returns>
    private static int TeamIsUser(Team team)
    {
        for (int i = 0; i < Variables.Users.Count; i++)
        {
            User user = Variables.Users[i];
            if (user != null && user.Team != null && user.Team == team)
                return i;
        }

        return -1;
    }
}
