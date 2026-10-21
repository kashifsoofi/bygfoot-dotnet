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
}
