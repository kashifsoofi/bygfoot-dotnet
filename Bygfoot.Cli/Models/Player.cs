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
        int qualityEnd = (int)Quality.QualityEnd;
        float[] scoutDeviance = new float[qualityEnd];
        float[] devianceBound = new float[2];

        // the maximal deviance in both directions
        devianceBound[0] = pl.Talent - pl.Skill;
        devianceBound[1] = Variables.Constants.GetFloat("float_player_max_skill") - pl.Talent;

        for (int i = 0; i < qualityEnd; i++)
        {
            scoutDeviance[i] = (i + 1) * Variables.Constants.GetFloat("float_player_max_skill") * (Variables.Constants.GetFloat("float_player_etal_scout_factor") / 100);
            // adjust deviance bounds with regard to the scout's deviance
            for (int j = 0; j < 2; j++)
                devianceBound[j] = Math.Min(devianceBound[j], scoutDeviance[i]);

            pl.Etal[i] = (float)MathHelper.Rnd(pl.Talent - devianceBound[0], pl.Talent + devianceBound[1]);
        }
    }

    /// <summary>
    /// Assign a (transfer) value to a player. The value depends on skill, talent and age.
    /// Corresponds to C: player_assign_value.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>The value of the player.</returns>
    public static int AssignValue(Player pl)
    {
        float value = (float)Math.Pow((Variables.Constants.GetFloat("float_player_value_skill_weight") * pl.Skill + (1 - Variables.Constants.GetFloat("float_player_value_skill_weight")) * pl.Talent * 0.7), Variables.Constants.GetFloat("float_player_value_power"));

        if (pl.PeakAge > pl.Age)
            value *= 1 + Variables.Constants.GetFloat("float_player_value_younger_factor") * (pl.PeakAge - pl.Age);
        else
            value *= 1 - Variables.Constants.GetFloat("float_player_value_older_factor") * (pl.Age - pl.PeakAge);

        return MathHelper.RoundInteger((int)Math.Round(value), 2);
    }

    /// <summary>
    /// Assign a wage to a player. The wage depends mainly on the value.
    /// Corresponds to C: player_assign_wage.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>The wage of the player.</returns>
    public static int AssignWage(Player pl)
    {
        float wage = (float)((double)pl.Value * Variables.Constants.GetFloat("float_player_wage_value_factor") * MathHelper.Rnd(1 - Variables.Constants.GetFloat("float_player_wage_random_dev"), 1 + Variables.Constants.GetFloat("float_player_wage_random_dev")));

        return MathHelper.RoundInteger((int)Math.Round(wage), 1);
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
    /// Return the value of a card (yellow or red) of a player for a league or cup.
    /// Corresponds to C: player_card_get.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="cardType">The card type, e.g. yellow or red.</param>
    /// <returns>The card value.</returns>
    public static int CardGet(Player pl, int clid, int cardType)
    {
        int returnValue = 0;

        foreach (PlayerCard card in pl.Cards)
        {
            if (card.Clid == clid)
            {
                if (cardType == (int)PlayerValue.CardYellow)
                    returnValue = card.Yellow;
                else
                    returnValue = card.Red;

                break;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Change a card value for a player. Corresponds to C: player_card_set.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="cardType">The card type, e.g. yellow or red.</param>
    /// <param name="value">The new value.</param>
    /// <param name="diff">Whether to add the value to the old one or replace it.</param>
    public static void CardSet(Player pl, int clid, int cardType, int value, bool diff)
    {
        PlayerCard card = null;

        foreach (PlayerCard c in pl.Cards)
        {
            if (c.Clid == clid)
            {
                card = c;
                break;
            }
        }

        if (card == null)
        {
            PlayerCard newCard = new PlayerCard();
            newCard.Clid = clid;
            newCard.Yellow = 0;
            newCard.Red = 0;
            pl.Cards.Add(newCard);
            CardSet(pl, clid, cardType, value, diff);
            return;
        }

        int cardValue = 0;
        bool isYellow = cardType == (int)PlayerValue.CardYellow;
        bool isRed = cardType == (int)PlayerValue.CardRed;

        if (isYellow)
            cardValue = card.Yellow;
        else if (isRed)
            cardValue = card.Red;

        if (isYellow || isRed)
        {
            cardValue = diff ? cardValue + value : value;

            if (cardValue < 0)
                cardValue = 0;

            if (isYellow)
                card.Yellow = cardValue;
            else
                card.Red = cardValue;
        }
    }

    /// <summary>
    /// Find out whether the player is banned in the next match or whether he will be
    /// if he gets a yellow card. Corresponds to C: player_is_banned.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>Number of games banned, or 0 not banned, or -1 if yellow.</returns>
    public static int IsBanned(Player pl)
    {
        Fixture fix = Team.GetFixture(pl.Team, false);
        int yellowRed = -1, yellow, red;

        if (fix == null)
            return 0;

        if (fix.Clid < Bygfoot.IdCupStart)
            yellowRed = League.LeagueFromClid(fix.Clid).YellowRed;
        else
            yellowRed = Cup.CupFromClid(fix.Clid).YellowRed;

        yellow = CardGet(pl, fix.Clid, (int)PlayerValue.CardYellow);
        red = CardGet(pl, fix.Clid, (int)PlayerValue.CardRed);

        if (red > 0)
            return red;

        if (yellow == yellowRed - 1)
            return -1;

        return 0;
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
        float goalieFactor = 1 - Variables.Constants.GetFloat("float_player_fitness_decrease_factor_goalie") * (pl.Cpos == PlayerPos.Goalie ? 1 : 0);
        float boostFactor = 1 + pl.Team.Boost * Variables.Constants.GetFloat("float_player_boost_fitness_effect");
        float streakFactor = 1 + (float)pl.Streak * Variables.Constants.GetFloat("float_player_streak_influence_fitness_decrease");

        if (pl.Age < pl.PeakAge - pl.PeakRegion)
        {
            float youngerFactor = (pl.PeakAge - pl.PeakRegion - pl.Age) * Variables.Constants.GetFloat("float_player_fitness_decrease_younger_factor") + Variables.Constants.GetFloat("float_player_fitness_decrease_add");
            pl.Fitness -= youngerFactor * goalieFactor * boostFactor * streakFactor;
        }
        else if (pl.Age > pl.PeakAge + pl.PeakRegion)
        {
            float olderFactor = (pl.Age - pl.PeakAge - pl.PeakRegion) * Variables.Constants.GetFloat("float_player_fitness_decrease_older_factor") + Variables.Constants.GetFloat("float_player_fitness_decrease_add");
            pl.Fitness -= olderFactor * goalieFactor * boostFactor * streakFactor;
        }
        else
        {
            pl.Fitness -= (Variables.Constants.GetFloat("float_player_fitness_decrease_add") * goalieFactor * boostFactor * streakFactor);
        }

        if (pl.Fitness < 0)
            pl.Fitness = 0;
    }

    /// <summary>
    /// Increase of player fitness after a match. Depends on the decrease values and the percentage
    /// values that determine how much of the decrease a player gains. Corresponds to C: player_update_fitness.
    /// </summary>
    /// <param name="pl">The player we edit.</param>
    public static void UpdateFitness(Player pl)
    {
        float variance = (float)MathHelper.Rnd(1 - Variables.Constants.GetFloat("float_player_fitness_increase_variance"), 1 + Variables.Constants.GetFloat("float_player_fitness_increase_variance"));
        float streakFactor = 1 + (float)pl.Streak * Variables.Constants.GetFloat("float_player_streak_influence_fitness_increase");

        if (pl.Participation)
        {
            pl.Participation = false;
            return;
        }

        if (pl.Age < pl.PeakAge - pl.PeakRegion)
        {
            float youngerFactor = (pl.PeakAge - pl.PeakRegion - pl.Age) * Variables.Constants.GetFloat("float_player_fitness_increase_younger_factor") + Variables.Constants.GetFloat("float_player_fitness_increase_add");
            pl.Fitness += youngerFactor * variance * streakFactor;
        }
        else if (pl.Age > pl.PeakAge + pl.PeakRegion)
        {
            float olderFactor = (pl.Age - pl.PeakAge - pl.PeakRegion) * Variables.Constants.GetFloat("float_player_fitness_increase_older_factor") + Variables.Constants.GetFloat("float_player_fitness_increase_add");
            pl.Fitness += olderFactor * variance * streakFactor;
        }
        else
        {
            pl.Fitness += (Variables.Constants.GetFloat("float_player_fitness_increase_add") * variance * streakFactor);
        }

        pl.Fitness = Math.Min(pl.Fitness, 1);
    }

    /// <summary>
    /// Find out whether the player is a youth. Corresponds to C: query_player_is_youth (macro).
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>Whether the player is a youth.</returns>
    public static bool QueryIsYouth(Player pl)
    {
        return pl.Age <= Variables.Constants.GetFloat("float_player_age_lower");
    }

    /// <summary>
    /// Find out whether the player is in the youth academy. Corresponds to C: query_player_is_in_ya.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>Whether the player is in the youth academy.</returns>
    public static bool QueryIsInYa(Player pl)
    {
        foreach (Player player in pl.Team.Players)
        {
            if (player == pl)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Find out whether the player is a CPU player. Corresponds to C: query_player_is_cpu (macro).
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <returns>Whether the player is a CPU player.</returns>
    public static bool QueryIsCpu(Player pl)
    {
        return Team.IsUser(pl.Team) == -1;
    }

    /// <summary>
    /// Return the number of games, goals or shots for a player in a league or cup.
    /// Corresponds to C: player_games_goals_get.
    /// </summary>
    /// <param name="pl">The player we examine.</param>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="type">Whether we sum up the goals, games or shots.</param>
    /// <returns>The number of games, goals or shots.</returns>
    public static int GamesGoalsGet(Player pl, int clid, int type)
    {
        int returnValue = 0;

        foreach (PlayerGamesGoals gamesGoals in pl.GamesGoals)
        {
            if (gamesGoals.Clid == clid)
            {
                if (type == (int)PlayerValue.Games)
                    returnValue = gamesGoals.Games;
                else if (type == (int)PlayerValue.Goals)
                    returnValue = gamesGoals.Goals;
                else if (type == (int)PlayerValue.Shots)
                    returnValue = gamesGoals.Shots;
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Change the number of games, goals or shots for a player in a league or cup.
    /// Corresponds to C: player_games_goals_set.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="type">Whether we set the games, goals or shots.</param>
    /// <param name="value">The value to add.</param>
    public static void GamesGoalsSet(Player pl, int clid, int type, int value)
    {
        PlayerGamesGoals gamesGoals = null;

        foreach (PlayerGamesGoals g in pl.GamesGoals)
        {
            if (g.Clid == clid)
            {
                gamesGoals = g;
                break;
            }
        }

        if (gamesGoals == null)
        {
            PlayerGamesGoals newGamesGoals = new PlayerGamesGoals();
            newGamesGoals.Clid = clid;
            newGamesGoals.Games = 0;
            newGamesGoals.Goals = 0;
            newGamesGoals.Shots = 0;
            pl.GamesGoals.Add(newGamesGoals);
            GamesGoalsSet(pl, clid, type, value);
            return;
        }

        if (type == (int)PlayerValue.Games)
        {
            gamesGoals.Games += value;
            if (gamesGoals.Games < 0)
                gamesGoals.Games = 0;
        }
        else if (type == (int)PlayerValue.Goals)
        {
            gamesGoals.Goals += value;
            if (gamesGoals.Goals < 0)
                gamesGoals.Goals = 0;
        }
        else if (type == (int)PlayerValue.Shots)
        {
            gamesGoals.Shots += value;
            if (gamesGoals.Shots < 0)
                gamesGoals.Shots = 0;
        }
    }

    /// <summary>
    /// Copy a player and insert him into a team's player array. Corresponds to C: player_copy.
    /// </summary>
    /// <param name="pl">The player to copy.</param>
    /// <param name="tm">The team the player gets inserted into.</param>
    /// <param name="insertAt">The index to insert the player at.</param>
    public static void Copy(Player pl, Team tm, int insertAt)
    {
        Player newPlayer = new Player();
        newPlayer.Name = pl.Name;
        newPlayer.Pos = pl.Pos;
        newPlayer.Cpos = pl.Cpos;
        newPlayer.Health = pl.Health;
        newPlayer.Recovery = pl.Recovery;
        newPlayer.Id = pl.Id;
        newPlayer.Value = pl.Value;
        newPlayer.Wage = pl.Wage;
        newPlayer.Offers = pl.Offers;
        newPlayer.Streak = pl.Streak;
        newPlayer.CardStatus = pl.CardStatus;
        newPlayer.Skill = pl.Skill;
        newPlayer.Cskill = pl.Cskill;
        newPlayer.Talent = pl.Talent;
        newPlayer.Etal = pl.Etal;
        newPlayer.Fitness = pl.Fitness;
        newPlayer.Lsu = pl.Lsu;
        newPlayer.Age = pl.Age;
        newPlayer.PeakAge = pl.PeakAge;
        newPlayer.PeakRegion = pl.PeakRegion;
        newPlayer.Contract = pl.Contract;
        newPlayer.StreakProb = pl.StreakProb;
        newPlayer.StreakCount = pl.StreakCount;
        newPlayer.Participation = pl.Participation;
        newPlayer.GamesGoals = pl.GamesGoals;
        newPlayer.Cards = pl.Cards;
        newPlayer.Career = pl.Career;
        newPlayer.Team = tm;

        tm.Players.Insert(insertAt, newPlayer);

        Player player = tm.Players[insertAt];
        if (insertAt < 11)
            player.Cpos = Player.GetPositionFromStructure(tm.Structure, insertAt);
        else
            player.Cpos = player.Pos;

        player.Cskill = Player.GetCskill(player, (int)player.Cpos, true);
    }

    /// <summary>
    /// Move a player from one player array to another one. Corresponds to C: player_move.
    /// </summary>
    /// <param name="tm1">The source team.</param>
    /// <param name="playerNumber">The index of the player in the players array.</param>
    /// <param name="tm2">The destination team.</param>
    /// <param name="insertAt">The new index of the player in the players array.</param>
    public static void Move(Team tm1, int playerNumber, Team tm2, int insertAt)
    {
        Player pl = tm1.Players[playerNumber];
        pl.Team = tm2;
        tm1.Players.RemoveAt(playerNumber);
        tm2.Players.Insert(insertAt, pl);
    }

    /// <summary>
    /// Swap two players. Corresponds to C: player_swap.
    /// </summary>
    /// <param name="tm1">The source team of the first player.</param>
    /// <param name="playerNumber1">The index of the first player in the players array.</param>
    /// <param name="tm2">The source team of the second player.</param>
    /// <param name="playerNumber2">The index of the second player in the players array.</param>
    public static void Swap(Team tm1, int playerNumber1, Team tm2, int playerNumber2)
    {
        int move = (tm1 == tm2 && playerNumber1 < playerNumber2) ? -1 : 1;

        if (Variables.Status[0] == (int)Status0Value.StatusLiveGamePause)
        {
            if ((playerNumber1 < 11 && Player.IsBanned(tm1.Players[playerNumber1]) > 0 && tm1.Players[playerNumber1].Participation) ||
                (playerNumber2 < 11 && Player.IsBanned(tm2.Players[playerNumber2]) > 0 && tm2.Players[playerNumber2].Participation))
            {
                // game_gui_show_warning(_("You can't replace a banned player."));
                return;
            }
        }

        Move(tm1, playerNumber1, tm2, playerNumber2);

        Team team2 = tm2;
        Player p2 = team2.Players[playerNumber2];
        if (playerNumber2 < 11)
            p2.Cpos = Player.GetPositionFromStructure(team2.Structure, playerNumber2);
        else
            p2.Cpos = p2.Pos;
        p2.Cskill = Player.GetCskill(p2, (int)p2.Cpos, true);

        Move(tm2, playerNumber2 + move, tm1, playerNumber1);

        Team team1 = tm1;
        Player p1 = team1.Players[playerNumber1];
        if (playerNumber1 < 11)
            p1.Cpos = Player.GetPositionFromStructure(team1.Structure, playerNumber1);
        else
            p1.Cpos = p1.Pos;
        p1.Cskill = Player.GetCskill(p1, (int)p1.Cpos, true);
    }

    /// <summary>
    /// Nullify some stuff at the beginning of the season and change the skills and talents of cpu players.
    /// Corresponds to C: player_season_start.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="skillChange">Skill/talent change of the player in percentage.</param>
    public static void SeasonStart(Player pl, float skillChange)
    {
        pl.GamesGoals = new List<PlayerGamesGoals>();
        pl.Cards = new List<PlayerCard>();

        if (skillChange != 0)
        {
            pl.Talent *= (1 + skillChange);
            pl.Talent = Math.Min(Math.Max(pl.Talent, 0), Variables.Constants.GetFloat("float_player_max_skill"));

            pl.Skill *= (1 + skillChange);
            pl.Skill = Math.Min(Math.Max(pl.Skill, 0), pl.Talent);
            pl.Cskill = GetCskill(pl, (int)pl.Cpos, true);
            pl.Value = AssignValue(pl);

            int qualityEnd = (int)Quality.QualityEnd;
            for (int i = 0; i < qualityEnd; i++)
            {
                if (pl.Skill > pl.Etal[i])
                {
                    EstimateTalent(pl);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Return the last name of a player. Corresponds to C: player_get_last_name.
    /// </summary>
    /// <param name="name">The full name of the player.</param>
    /// <returns>The last name of the player.</returns>
    public static string GetLastName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;

        string reversed = new string(name.Reverse().ToArray());

        int i = 0;
        while (i < reversed.Length && !char.IsWhiteSpace(reversed[i]))
            i++;

        int n = reversed.Length - i - 2;
        string buf = n > 0 ? reversed.Substring(0, n) : string.Empty;

        char[] bufChars = buf.ToCharArray();
        Array.Reverse(bufChars);
        return new string(bufChars);
    }

    /// <summary>
    /// Get the index of a player in the players array of a team by his id. Corresponds to C: player_id_index.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="playerId">The player id.</param>
    /// <param name="stopWhenPlayerNotFound">Whether we stop when we don't find the player.</param>
    /// <returns>The array index or -1.</returns>
    public static int IdIndex(Team tm, int playerId, bool stopWhenPlayerNotFound)
    {
        for (int i = 0; i < tm.Players.Count; i++)
        {
            if (tm.Players[i].Id == playerId)
                return i;
        }

        if (stopWhenPlayerNotFound)
            throw new Exception($"player_id_index: didn't find player with id {playerId} of team {tm.Name}");

        return -1;
    }

    /// <summary>
    /// Remove a player from a team. Corresponds to C: player_remove_from_team.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="idx">The player index in the players array.</param>
    public static void RemoveFromTeam(Team tm, int idx)
    {
        int usrIdx = Team.IsUser(tm);

        if (usrIdx != -1)
        {
            User user = User.FromTeam(tm);
            if (user != null && user.Options.GetInt("int_opt_user_penalty_shooter") == tm.Players[idx].Id)
                user.Options.SetInt("int_opt_user_penalty_shooter", -1);
        }

        Transfer.TransferRemovePlayerPtr(tm.Players[idx]);
        tm.Players.RemoveAt(idx);
    }

    /// <summary>
    /// Remove a player from a user team after the contract expired. Corresponds to C: player_remove_contract.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void RemoveContract(Player pl)
    {
        if (Variables.DebugLevel < 50)
            User.EventAdd(User.FromTeam(pl.Team), (int)EventType.PlayerLeft, -1, -1, null, pl.Name);

        Player.RemoveFromTeam(pl.Team, IdIndex(pl.Team, pl.Id, true));
    }

    /// <summary>
    /// Decrement the number of weeks until recovery depending on the user's physio. Corresponds to C: player_update_injury.
    /// </summary>
    /// <param name="pl">An injured player.</param>
    public static void UpdateInjury(Player pl)
    {
        int i, j;
        float rndom = (float)MathHelper.Rnd(0, 1);
        int physio = QueryIsCpu(pl) ? (int)Quality.QualityGood : User.FromTeam(pl.Team).Physio;
        float[,] injuryDecreaseProbs = new float[4, 3]
        {
            { Variables.Constants.GetFloat("float_player_injury_recovery_best0"), Variables.Constants.GetFloat("float_player_injury_recovery_best1"), Variables.Constants.GetFloat("float_player_injury_recovery_best2") },
            { Variables.Constants.GetFloat("float_player_injury_recovery_good0"), Variables.Constants.GetFloat("float_player_injury_recovery_good1"), Variables.Constants.GetFloat("float_player_injury_recovery_good2") },
            { Variables.Constants.GetFloat("float_player_injury_recovery_good2"), Variables.Constants.GetFloat("float_player_injury_recovery_good1"), Variables.Constants.GetFloat("float_player_injury_recovery_good0") },
            { Variables.Constants.GetFloat("float_player_injury_recovery_best2"), Variables.Constants.GetFloat("float_player_injury_recovery_best1"), Variables.Constants.GetFloat("float_player_injury_recovery_best0") }
        };

        for (j = 1; j < 3; j++)
            injuryDecreaseProbs[physio, j] += injuryDecreaseProbs[physio, j - 1];

        for (i = 0; i < 3; i++)
        {
            if (rndom <= injuryDecreaseProbs[physio, i])
            {
                pl.Recovery = Math.Max(pl.Recovery - i, 0);
                break;
            }
        }

        if (pl.Recovery == 0)
        {
            pl.Health = PlayerInjury.None;
            pl.Fitness = (float)MathHelper.Rnd(
                Variables.Constants.GetFloat("float_player_injury_recovery_fitness_lower"),
                Variables.Constants.GetFloat("float_player_injury_recovery_fitness_upper"));
        }
    }

    /// <summary>
    /// Add to the streak probability and keep it within the boundaries. Corresponds to C: player_streak_add_to_prob.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="add">The value to add.</param>
    public static void StreakAddToProb(Player pl, float add)
    {
        pl.StreakProb += add;
        pl.StreakProb = Math.Min(Math.Max(pl.StreakProb, -1), 1);
    }

    /// <summary>
    /// Reset the streak count of a player to a random lock length. Corresponds to C: player_streak_reset_count (macro).
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void StreakResetCount(Player pl)
    {
        pl.StreakCount = (float)-MathHelper.Rnd(
            Variables.Constants.GetInt("int_player_streak_lock_length_lower"),
            Variables.Constants.GetInt("int_player_streak_lock_length_upper"));
    }

    /// <summary>
    /// Weekly test whether a player goes on a hot/cold streak; if he's on a streak, decrease the streak counter.
    /// Corresponds to C: player_update_streak.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void UpdateStreak(Player pl)
    {
        float streakProbFactor = Variables.Constants.GetFloat("float_player_streak_prob_max") - Variables.Constants.GetFloat("float_player_streak_prob_zero");
        float streakProbAdd = Variables.Constants.GetFloat("float_player_streak_prob_zero");
        float decreaseFactor = 0;

        if (pl.StreakCount < 0)
        {
            pl.StreakCount += 1;
            if (pl.StreakCount >= 0)
                pl.StreakCount = 0;

            return;
        }
        else if (pl.StreakCount > 0)
        {
            if (pl.Streak == PlayerStreak.Hot)
                decreaseFactor = -1;
            else if (pl.Streak == PlayerStreak.Cold)
                decreaseFactor = 1;
            else
                Debug.DebugPrintMessage("player_update_streak: streak count is positive ({0}) but player {1} is not on a streak!", pl.StreakCount, pl.Name);

            pl.StreakCount -= (pl.StreakProb * decreaseFactor * Variables.Constants.GetFloat("float_player_streak_count_decrease_factor") + Variables.Constants.GetFloat("float_player_streak_count_decrease_add"));

            if (pl.StreakCount <= 0)
            {
                pl.Streak = PlayerStreak.None;
                StreakResetCount(pl);
            }

            return;
        }

        if (QueryIsInYa(pl))
            return;

        float streakType = (float)MathHelper.Rnd(-1, 1);
        float streakProb = (float)MathHelper.Rnd(0, 1);
        float streakLength = (float)MathHelper.Rnd(
            Variables.Constants.GetFloat("float_player_streak_length_lower"),
            Variables.Constants.GetFloat("float_player_streak_length_upper"));

        if (streakType < pl.StreakProb && ((pl.StreakProb > 0 && streakProb < streakProbFactor * pl.StreakProb + streakProbAdd) ||
            (pl.StreakProb <= 0 && streakProb < streakProbAdd * pl.StreakProb + streakProbAdd)))
        {
            pl.Streak = PlayerStreak.Hot;
            pl.StreakCount = streakLength;
        }
        else if (streakType > pl.StreakProb && ((pl.StreakProb > 0 && streakProb < -streakProbAdd * pl.StreakProb + streakProbAdd) ||
            (pl.StreakProb <= 0 && streakProb < -streakProbFactor * pl.StreakProb + streakProbAdd)))
        {
            pl.Streak = PlayerStreak.Cold;
            pl.StreakCount = streakLength;
        }
    }

    /// <summary>
    /// Update skill and lsu of a user player. Corresponds to C: player_update_skill.
    /// </summary>
    /// <param name="pl">The player we update.</param>
    public static void UpdateSkill(Player pl)
    {
        if (!QueryIsYouth(pl))
        {
            if (pl.Age > pl.PeakAge)
                pl.Lsu += (pl.Health == PlayerInjury.None) ? 1 : Variables.Constants.GetFloat("float_player_lsu_injured_old");
            else if (pl.Health == PlayerInjury.None && ((float)GamesGoalsGet(pl, pl.Team.Clid, (int)PlayerValue.Games) / (float)Variables.Week >= Variables.Constants.GetFloat("float_player_lsu_games_percentage") || (float)MathHelper.Rnd(0, 1) <= Variables.Constants.GetFloat("float_player_lsu_increase_prob")))
                pl.Lsu += 1;
        }
        else
        {
            pl.Lsu += (pl.Health != PlayerInjury.None) ? 0 : Variables.Constants.GetFloat("float_youth_academy_lsu_addition_best") - (User.FromTeam(pl.Team).YouthAcademy.AvCoach * Variables.Constants.GetFloat("float_youth_academy_lsu_penalty"));
        }

        if (pl.Lsu < Variables.Constants.GetFloat("float_player_lsu_update_limit") || (float)MathHelper.Rnd(0, 1) < (float)Math.Pow(Variables.Constants.GetFloat("float_player_lsu_update_base_prob"), pl.Lsu - Variables.Constants.GetFloat("float_player_lsu_update_limit")))
            return;

        pl.Lsu = 0;

        if (pl.Age < pl.PeakAge - pl.PeakRegion)
            pl.Skill *= (1 + ((pl.PeakAge - pl.Age) * Variables.Constants.GetFloat("float_player_skill_update_younger_factor") + Variables.Constants.GetFloat("float_player_skill_update_younger_add")));
        else if (pl.Age > pl.PeakAge + pl.PeakRegion)
            pl.Skill *= (1 - ((pl.Age - pl.PeakAge) * Variables.Constants.GetFloat("float_player_skill_update_older_factor") + Variables.Constants.GetFloat("float_player_skill_update_older_add")));
        else
            pl.Skill *= (float)MathHelper.Rnd(1 - Variables.Constants.GetFloat("float_player_skill_update_peak"), 1 + Variables.Constants.GetFloat("float_player_skill_update_peak"));

        pl.Skill = Math.Clamp(pl.Skill, 0, pl.Talent);
        pl.Cskill = GetCskill(pl, (int)pl.Cpos, true);
        pl.Value = AssignValue(pl);

        for (int i = 0; i < (int)Quality.QualityEnd; i++)
        {
            if (pl.Skill > pl.Etal[i])
            {
                EstimateTalent(pl);
                break;
            }
        }
    }

    /// <summary>
    /// Update a player in a team (age, skill etc.). Corresponds to C: player_update_weekly.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void UpdateWeekly(Player pl)
    {
        if (pl.Fitness < 0)
            pl.Fitness = Variables.Constants.GetFloat("float_player_fitness_lower");

        if (pl.Health != PlayerInjury.None)
            UpdateInjury(pl);
        else
            UpdateStreak(pl);

        if (QueryIsCpu(pl))
            return;

        pl.Age += 0.0192f;

        if (Variables.DebugLevel < 50)
        {
            if (Variables.Options.GetInt("int_opt_disable_contracts") != 0 && pl.Age > Variables.Constants.GetFloat("float_player_age_lower"))
                pl.Contract -= 0.0192f;
        }

        User user = User.FromTeam(pl.Team);

        if (Variables.Options.GetInt("int_opt_disable_contracts") != 0 && Variables.DebugLevel < 50 &&
            pl.Contract * 12 <= user.Options.GetInt("int_opt_user_contract_limit") &&
            (pl.Contract + 0.0192f) * 12 > user.Options.GetInt("int_opt_user_contract_limit"))
        {
            User.EventAdd(user, (int)EventType.Warning, -1, -1, null, $"{pl.Name}'s contract expires in {pl.Contract:F1} years.");
        }

        if (pl.Contract <= 0)
        {
            RemoveContract(pl);
            return;
        }

        UpdateSkill(pl);
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

