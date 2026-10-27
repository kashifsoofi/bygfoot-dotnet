namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Player positions. Corresponds to C enum PlayerPos.
/// </summary>
public enum PlayerPos
{
    Goalie = 0,       // PLAYER_POS_GOALIE
    Defender,         // PLAYER_POS_DEFENDER
    Midfielder,       // PLAYER_POS_MIDFIELDER
    Forward,          // PLAYER_POS_FORWARD
    Any,              // PLAYER_POS_ANY
    End               // PLAYER_POS_END
}

/// <summary>
/// Streaks a player can go on. Corresponds to C enum PlayerStreak.
/// </summary>
public enum PlayerStreak
{
    Cold = -1,        // PLAYER_STREAK_COLD
    None = 0,         // PLAYER_STREAK_NONE
    Hot = 1           // PLAYER_STREAK_HOT
}

/// <summary>
/// Injury types. Corresponds to C enum PlayerInjury.
/// </summary>
public enum PlayerInjury
{
    None = 0,             // PLAYER_INJURY_NONE
    Concussion,           // PLAYER_INJURY_CONCUSSION
    PulledMuscle,         // PLAYER_INJURY_PULLED_MUSCLE
    Hamstring,            // PLAYER_INJURY_HAMSTRING
    Groin,                // PLAYER_INJURY_GROIN
    FracturedAnkle,       // PLAYER_INJURY_FRAC_ANKLE
    Rib,                  // PLAYER_INJURY_RIB
    Leg,                  // PLAYER_INJURY_LEG
    BrokenAnkle,          // PLAYER_INJURY_BROK_ANKLE
    Arm,                  // PLAYER_INJURY_ARM
    Shoulder,             // PLAYER_INJURY_SHOULDER
    Ligament,             // PLAYER_INJURY_LIGAMENT
    CareerStop,           // PLAYER_INJURY_CAREER_STOP
    End                   // PLAYER_INJURY_END
}

/// <summary>
/// Indices for the player data. Corresponds to C enum PlayerValue.
/// </summary>
public enum PlayerValue
{
    Games = 0,            // PLAYER_VALUE_GAMES
    Goals,                // PLAYER_VALUE_GOALS
    Shots,                // PLAYER_VALUE_SHOTS
    CardYellow,           // PLAYER_VALUE_CARD_YELLOW
    CardRed,              // PLAYER_VALUE_CARD_RED
    End                   // PLAYER_VALUE_END
}

/// <summary>
/// Card status during the live game. Corresponds to C enum PlayerCardStatus.
/// </summary>
public enum PlayerCardStatus
{
    None = 0,             // PLAYER_CARD_STATUS_NONE
    Yellow,               // PLAYER_CARD_STATUS_YELLOW
    Red                   // PLAYER_CARD_STATUS_RED
}

/// <summary>
/// Card tracking for a specific league or cup. Corresponds to C struct PlayerCard.
/// </summary>
public class PlayerCard
{
    public int Clid { get; set; }
    public int Yellow { get; set; }
    public int Red { get; set; }
}

/// <summary>
/// Games and goals tracking for a specific league or cup. Corresponds to C struct PlayerGamesGoals.
/// </summary>
public class PlayerGamesGoals
{
    public int Clid { get; set; }
    public int Games { get; set; }
    public int Goals { get; set; }
    public int Shots { get; set; }
}

/// <summary>
/// Attribute visibility for player lists. Corresponds to C enum PlayerListAttributeValue.
/// </summary>
public enum PlayerListAttribute
{
    Name = 0,             // PLAYER_LIST_ATTRIBUTE_NAME
    CPos,                 // PLAYER_LIST_ATTRIBUTE_CPOS
    Pos,                  // PLAYER_LIST_ATTRIBUTE_POS
    CSkill,               // PLAYER_LIST_ATTRIBUTE_CSKILL
    Skill,                // PLAYER_LIST_ATTRIBUTE_SKILL
    Fitness,              // PLAYER_LIST_ATTRIBUTE_FITNESS
    Games,                // PLAYER_LIST_ATTRIBUTE_GAMES
    Shots,                // PLAYER_LIST_ATTRIBUTE_SHOTS
    Goals,                // PLAYER_LIST_ATTRIBUTE_GOALS
    Status,               // PLAYER_LIST_ATTRIBUTE_STATUS
    Cards,                // PLAYER_LIST_ATTRIBUTE_CARDS
    Age,                  // PLAYER_LIST_ATTRIBUTE_AGE
    Etal,                 // PLAYER_LIST_ATTRIBUTE_ETAL
    Value,                // PLAYER_LIST_ATTRIBUTE_VALUE
    Wage,                 // PLAYER_LIST_ATTRIBUTE_WAGE
    Contract,             // PLAYER_LIST_ATTRIBUTE_CONTRACT
    Team,                 // PLAYER_LIST_ATTRIBUTE_TEAM
    LeagueCup,            // PLAYER_LIST_ATTRIBUTE_LEAGUE_CUP
    End                   // PLAYER_LIST_ATTRIBUTE_END
}

/// <summary>
/// Attribute visibility for player info. Corresponds to C enum PlayerInfoAttributeValue.
/// </summary>
public enum PlayerInfoAttribute
{
    Name = 0,             // PLAYER_INFO_ATTRIBUTE_NAME
    Pos,                  // PLAYER_INFO_ATTRIBUTE_POS
    CPos,                 // PLAYER_INFO_ATTRIBUTE_CPOS
    Skill,                // PLAYER_INFO_ATTRIBUTE_SKILL
    CSkill,               // PLAYER_INFO_ATTRIBUTE_CSKILL
    Fitness,              // PLAYER_INFO_ATTRIBUTE_FITNESS
    Etal,                 // PLAYER_INFO_ATTRIBUTE_ETAL
    Age,                  // PLAYER_INFO_ATTRIBUTE_AGE
    Health,               // PLAYER_INFO_ATTRIBUTE_HEALTH
    Value,                // PLAYER_INFO_ATTRIBUTE_VALUE
    Wage,                 // PLAYER_INFO_ATTRIBUTE_WAGE
    Contract,             // PLAYER_INFO_ATTRIBUTE_CONTRACT
    GamesGoals,           // PLAYER_INFO_ATTRIBUTE_GAMES_GOALS
    YellowCards,          // PLAYER_INFO_ATTRIBUTE_YELLOW_CARDS
    Banned,               // PLAYER_info_ATTRIBUTE_BANNED
    Streak,               // PLAYER_INFO_ATTRIBUTE_STREAK
    Career,               // PLAYER_INFO_ATTRIBUTE_CAREER
    Offers,               // PLAYER_INFO_ATTRIBUTE_OFFERS
    End                   // PLAYER_INFO_ATTRIBUTE_END
}

/// <summary>
/// Representation of a player. Corresponds to C struct Player.
/// </summary>
public class Player
{
    public string Name { get; set; }
    public PlayerPos Pos { get; set; }
    public PlayerPos Cpos { get; set; }
    public PlayerInjury Health { get; set; }
    public int Recovery { get; set; }
    public int Id { get; set; }
    public int Value { get; set; }
    public int Wage { get; set; }
    public int Offers { get; set; }
    public PlayerStreak Streak { get; set; }
    public PlayerCardStatus CardStatus { get; set; }

    public float Skill { get; set; }
    public float Cskill { get; set; }
    public float Talent { get; set; }
    public float[] Etal { get; set; } = new float[5]; // Assuming QUALITY_END is 5
    public float Fitness { get; set; }
    public float Lsu { get; set; }
    public float Age { get; set; }
    public float PeakAge { get; set; }
    public float PeakRegion { get; set; }
    public float Contract { get; set; }
    public float StreakProb { get; set; }
    public float StreakCount { get; set; }

    public bool Participation { get; set; }
    public List<PlayerGamesGoals> GamesGoals { get; set; } = new List<PlayerGamesGoals>();
    public List<PlayerCard> Cards { get; set; } = new List<PlayerCard>();
    public int[] Career { get; set; } = new int[5]; // PLAYER_VALUE_END is 5
    public Team Team { get; set; }

    /// <summary>
    /// Create and return a new player. Corresponds to C: player_new.
    /// </summary>
    /// <param name="tm">The team the player will belong to.</param>
    /// <param name="averageTalent">The average talent of the team.</param>
    /// <param name="newId">Whether to create a new id.</param>
    /// <returns>A newly created player.</returns>
    public static Player New(Team tm, float averageTalent, bool newId)
    {
        // Implementation depends on the not yet converted maths, name and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the appropriate position for the player with the given number.
    /// The position depends on the team structure if the player number is < 11
    /// and on some constants otherwise.
    /// Corresponds to C: player_get_position_from_structure.
    /// </summary>
    /// <param name="structure">The team structure, something like 442 or 352.</param>
    /// <param name="playerNumber">The number of the player within the team.</param>
    /// <returns>A new position for the player.</returns>
    public static PlayerPos GetPositionFromStructure(int structure, int playerNumber)
    {
        int position = -1;
        int bound = Misc.MathGetPlace(structure, 3) + 1;

        if (playerNumber == 0 || playerNumber == 11)
            position = (int)PlayerPos.Goalie;
        else if (playerNumber < bound ||
                 (playerNumber > 10 && playerNumber < (11 + (int)(Variables.Constants.GetInt("int_team_cpu_players") - 11) * Variables.Constants.GetFloat("float_player_pos_bound1"))))
            position = (int)PlayerPos.Defender;
        else if (playerNumber < Misc.MathGetPlace(structure, 3) + Misc.MathGetPlace(structure, 2) + 1 ||
                 (playerNumber > 10 && playerNumber < (11 + (int)(Variables.Constants.GetInt("int_team_cpu_players") - 11) * Variables.Constants.GetFloat("float_player_pos_bound2"))))
            position = (int)PlayerPos.Midfielder;
        else
            position = (int)PlayerPos.Forward;

        return (PlayerPos)position;
    }

    /// <summary>
    /// Calculate a player's skill depending on his age and talent.
    /// Corresponds to C: player_skill_from_talent.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <returns>The calculated skill.</returns>
    public static float SkillFromTalent(Player pl)
    {
        float skill = pl.Talent;
        float curAge = pl.PeakAge;

        if (pl.Age < pl.PeakAge)
        {
            while (curAge > pl.Age)
            {
                curAge -= (float)(Misc.MathRndi((int)(1 - Variables.Constants.GetFloat("float_player_lsu_update_limit")), (int)(1 + Variables.Constants.GetFloat("float_player_lsu_update_limit"))) + 2) * (float)0.0192;
                if (pl.PeakAge - curAge > pl.PeakRegion)
                    skill *= 1 - ((pl.PeakAge - curAge) * Variables.Constants.GetFloat("float_player_skill_update_younger_factor") + Variables.Constants.GetFloat("float_player_skill_update_younger_add"));
            }
        }
        else
        {
            while (curAge < pl.Age)
            {
                curAge += (float)(Misc.MathRndi((int)(1 - Variables.Constants.GetFloat("float_player_lsu_update_limit")), (int)(1 + Variables.Constants.GetFloat("float_player_lsu_update_limit"))) + 2) * (float)0.0192;
                if (curAge - pl.PeakAge > pl.PeakRegion)
                    skill *= 1 - ((curAge - pl.PeakAge) * Variables.Constants.GetFloat("float_player_skill_update_older_factor") + Variables.Constants.GetFloat("float_player_skill_update_older_add"));
            }
        }

        if (skill > pl.Talent)
            skill = pl.Talent;

        return skill;
    }

    /// <summary>
    /// Estimate a player's talent.
    /// Corresponds to C: player_estimate_talent.
    /// </summary>
    /// <param name="pl">The player of which we'd like to estimate the talent.</param>
    public static void EstimateTalent(Player pl)
    {
        // Implementation depends on the not yet converted maths and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign a (transfer) value to a player. The value depends on skill, talent and age.
    /// Corresponds to C: player_assign_value.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>The value of the player.</returns>
    public static int AssignValue(Player pl)
    {
        // Implementation depends on the not yet converted maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign a wage to a player. The wage depends mainly on the value.
    /// Corresponds to C: player_assign_wage.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>The wage of the player.</returns>
    public static int AssignWage(Player pl)
    {
        // Implementation depends on the not yet converted maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the number of all games or goals the player's participated in / scored in all cups and leagues.
    /// Corresponds to C: player_all_games_goals.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <param name="type">Whether we sum up the goals.</param>
    /// <returns>The number of goals.</returns>
    public static int AllGamesGoals(Player pl, int type)
    {
        int sum = 0;

        foreach (PlayerGamesGoals gamesGoals in pl.GamesGoals)
        {
            if (type == (int)PlayerValue.Goals)
                sum += gamesGoals.Goals;
            else if (type == (int)PlayerValue.Games)
                sum += gamesGoals.Games;
            else if (type == (int)PlayerValue.Shots)
                sum += gamesGoals.Shots;
        }

        return sum;
    }

    /// <summary>
    /// Return the sum of all the yellow cards in all leagues and cups for the player.
    /// Corresponds to C: player_all_cards.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>The number of all cards.</returns>
    public static int AllCards(Player pl)
    {
        int sum = 0;

        foreach (PlayerCard card in pl.Cards)
            sum += card.Yellow;

        return sum;
    }

    /// <summary>
    /// Find out whether the given player id is in the team.
    /// Corresponds to C: query_player_id_in_team.
    /// </summary>
    /// <param name="playerId">The player's id.</param>
    /// <param name="tm">The team.</param>
    /// <returns>Whether the player is in the team.</returns>
    public static bool QueryIdInTeam(int playerId, Team tm)
    {
        foreach (Player player in tm.Players)
        {
            if (player.Id == playerId)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Return the player's cskill depending on whether he's on his normal position or not.
    /// Corresponds to C: player_get_cskill.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <param name="position">The position we'd like to put the player.</param>
    /// <param name="checkHealth">Whether to check for injury and ban.</param>
    /// <returns>A new cskill.</returns>
    public static float GetCskill(Player pl, int position, bool checkHealth)
    {
        if (checkHealth && (pl.Health != PlayerInjury.None || IsBanned(pl) > 0))
            return 0;

        if (pl.Pos != (PlayerPos)position)
        {
            if (position == (int)PlayerPos.Goalie || pl.Pos == PlayerPos.Goalie)
                return Math.Min(pl.Talent * 0.5f, pl.Skill);
            else if (Math.Abs(position - (int)pl.Pos) == 2)
                return Math.Min(pl.Talent * 0.65f, pl.Skill);
            else
                return Math.Min(pl.Talent * 0.75f, pl.Skill);
        }

        return pl.Skill;
    }

    /// <summary>
    /// Find out whether the player is banned in the next match or whether he will be
    /// if he gets a yellow card. Corresponds to C: player_is_banned.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>Number of games banned, or 0 if not banned, or -1 if yellow.</returns>
    public static int IsBanned(Player pl)
    {
        // Implementation depends on the not yet converted fixture and league/cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the player's skill contribution to his team. Corresponds to C: player_get_game_skill.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="skill">Whether to take his skill or current skill into account.</param>
    /// <param name="countSpecial">Whether to count special influence like boost and streak.</param>
    /// <returns>A float value representing the player's contribution.</returns>
    public static float GetGameSkill(Player pl, bool skill, bool countSpecial)
    {
        float boost = countSpecial ? 1 + Variables.Constants.GetFloat("float_player_boost_skill_effect") * pl.Team.Boost : 1;
        float streak = countSpecial ? 1 + (float)pl.Streak * Variables.Constants.GetFloat("float_player_streak_influence_skill") : 1;

        if (skill)
            return (float)(pl.Skill * boost * streak * Math.Pow(pl.Fitness, Variables.Constants.GetFloat("float_player_fitness_exponent")));

        return (float)(pl.Cskill * boost * streak * Math.Pow(pl.Fitness, Variables.Constants.GetFloat("float_player_fitness_exponent")));
    }

    /// <summary>
    /// Decrease a player's fitness during a match. Corresponds to C: player_decrease_fitness.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void DecreaseFitness(Player pl)
    {
        // Implementation depends on the not yet converted maths and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Increase of player fitness after a match. Depends on the decrease values and the percentage
    /// values that determine how much of the decrease a player gains. Corresponds to C: player_update_fitness.
    /// </summary>
    /// <param name="pl">The player we edit.</param>
    public static void UpdateFitness(Player pl)
    {
        // Implementation depends on the not yet converted maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Decrement the number of weeks until recovery depending on the user's physio. Corresponds to C: player_update_injury.
    /// </summary>
    /// <param name="pl">An injured player.</param>
    public static void UpdateInjury(Player pl)
    {
        // Implementation depends on the not yet converted user and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Weekly test whether a player goes on a hot/cold streak; if he's on a streak, decrease the streak counter.
    /// Corresponds to C: player_update_streak.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void UpdateStreak(Player pl)
    {
        // Implementation depends on the not yet converted maths and youth academy functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update skill and lsu of a user player. Corresponds to C: player_update_skill.
    /// </summary>
    /// <param name="pl">The player we update.</param>
    public static void UpdateSkill(Player pl)
    {
        // Implementation depends on the not yet converted team, user and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update a player in a team (age, skill etc.). Corresponds to C: player_update_weekly.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void UpdateWeekly(Player pl)
    {
        // Implementation depends on the not yet converted user, team and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Make some player updates after a match for players. Corresponds to C: player_update_post_match.
    /// </summary>
    /// <param name="pl">The player we update.</param>
    /// <param name="fix">The fixture.</param>
    public static void UpdatePostMatch(Player pl, Fixture fix)
    {
        // Implementation depends on the not yet converted league/cup and player functions.
        throw new NotImplementedException();
    }
}

