namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Events happening during a live game. 
/// Corresponds to C enum LiveGameEventType.
/// </summary>
public enum LiveGameEventType
{
    General = 0,                // LIVE_GAME_EVENT_GENERAL
    StartMatch,                 // LIVE_GAME_EVENT_START_MATCH
    HalfTime,                   // LIVE_GAME_EVENT_HALF_TIME
    ExtraTime,                  // LIVE_GAME_EVENT_EXTRA_TIME
    EndMatch,                   // LIVE_GAME_EVENT_END_MATCH
    LostPossession,             // LIVE_GAME_EVENT_LOST_POSSESSION
    ScoringChance,              // LIVE_GAME_EVENT_SCORING_CHANCE
    Header,                     // LIVE_GAME_EVENT_HEADER
    Penalty,                    // LIVE_GAME_EVENT_PENALTY
    FreeKick,                   // LIVE_GAME_EVENT_FREE_KICK
    Goal,                       // LIVE_GAME_EVENT_GOAL
    OwnGoal,                    // LIVE_GAME_EVENT_OWN_GOAL
    Post,                       // LIVE_GAME_EVENT_POST
    Miss,                       // LIVE_GAME_EVENT_MISS
    Save,                       // LIVE_GAME_EVENT_SAVE
    KeeperPushedInCorner,       // LIVE_GAME_EVENT_KEEPER_PUSHED_IN_CORNER
    CrossBar,                   // LIVE_GAME_EVENT_CROSS_BAR
    PlayerPushedInCorner,       // LIVE_GAME_EVENT_PLAYER_PUSHED_IN_CORNER
    CornerKick,                 // LIVE_GAME_EVENT_CORNER_KICK
    Foul,                       // LIVE_GAME_EVENT_FOUL
    FoulYellow,                 // LIVE_GAME_EVENT_FOUL_YELLOW
    FoulRed,                    // LIVE_GAME_EVENT_FOUL_RED
    FoulRedInjury,              // LIVE_GAME_EVENT_FOUL_RED_INJURY
    SendOff,                    // LIVE_GAME_EVENT_SEND_OFF
    Injury,                     // LIVE_GAME_EVENT_INJURY
    TempInjury,                 // LIVE_GAME_EVENT_TEMP_INJURY
    Penalties,                  // LIVE_GAME_EVENT_PENALTY
    Stadium,                    // LIVE_GAME_EVENT_STADIUM
    StadiumBreakdown,           // LIVE_GAME_EVENT_STADIUM_BREAKDOWN
    StadiumRiots,               // LIVE_GAME_EVENT_STADIUM_RIOTS
    StadiumFire,                // LIVE_GAME_EVENT_STADIUM_FIRE
    Substitution,               // LIVE_GAME_EVENT_SUBSTITUTION
    StructureChange,            // LIVE_GAME_EVENT_STRUCTURE_CHANGE
    StyleChangeAllOutDefend,    // LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_DEFEND
    StyleChangeDefend,          // LIVE_GAME_EVENT_STYLE_CHANGE_DEFEND
    StyleChangeBalanced,        // LIVE_GAME_EVENT_STYLE_CHANGE_BALANCED
    StyleChangeAttack,          // LIVE_GAME_EVENT_STYLE_CHANGE_ATTACK
    StyleChangeAllOutAttack,    // LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_ATTACK
    BoostChangeAnti,            // LIVE_GAME_EVENT_BOOST_CHANGE_ANTI
    BoostChangeOff,             // LIVE_GAME_EVENT_BOOST_CHANGE_OFF
    BoostChangeOn,              // LIVE_GAME_EVENT_BOOST_CHANGE_ON
    End                         // LIVE_GAME_EVENT_END
}

/// <summary>
/// Area of the pitch. Corresponds to C enum LiveGameUnitArea.
/// </summary>
public enum LiveGameUnitArea
{
    Defend = 0,                 // LIVE_GAME_UNIT_AREA_DEFEND
    Midfield,                   // LIVE_GAME_UNIT_AREA_MIDFIELD
    Attack,                     // LIVE_GAME_UNIT_AREA_ATTACK
    End                         // LIVE_GAME_UNIT_AREA_END
}

/// <summary>
/// Time progression in the game. Corresponds to C enum LiveGameUnitTime.
/// </summary>
public enum LiveGameUnitTime
{
    FirstHalf = 0,              // LIVE_GAME_UNIT_TIME_FIRST_HALF
    SecondHalf,                 // LIVE_GAME_UNIT_TIME_SECOND_HALF
    ExtraTime,                  // LIVE_GAME_UNIT_TIME_EXTRA_TIME
    Penalties,                  // LIVE_GAME_UNIT_TIME_PENALTIES
    End                         // LIVE_GAME_UNIT_TIME_END
}

/// <summary>
/// Statistical categories for live games. Corresponds to C enum LiveGameStatValue.
/// </summary>
public enum LiveGameStatValue
{
    GoalsRegular = 0,           // LIVE_GAME_STAT_VALUE_GOALS_REGULAR
    Shots,                      // LIVE_GAME_STAT_VALUE_SHOTS
    ShotPercentage,             // LIVE_GAME_STAT_VALUE_SHOT_PERCENTAGE
    Possession,                 // LIVE_VOLUME_STAT_VALUE_POSSESSION
    Penalties,                  // LIVE_GAME_STAT_VALUE_PENALTIES
    Fouls,                      // LIVE_GAME_STAT_VALUE_FOULS
    Cards,                      // LIVE_GAME_STAT_VALUE_CARDS
    Reds,                       // LIVE_GAME_STAT_VALUE_REDS
    Injuries,                   // LIVE_GAME_STAT_VALUE_INJURIES
    End                         // LIVE_GAME_STAT_VALUE_END
}

/// <summary>
/// Detailed tracking categories for live games. Corresponds to C enum LiveGameStatArray.
/// </summary>
public enum LiveGameStatArray
{
    ScorersForDisplay = 0,      // LIVE_GAME_STAT_ARRAY_SCORERS_FOR_DISPLAY
    Scorers,                    // LIVE_GAME_STAT_ARRAY_SCORERS
    Yellows,                    // LIVE_GAME_STAT_ARRAY_YELLOWS
    Reds,                       // LIVE_GAME_STAT_ARRAY_REDS
    Injured,                    // LIVE_GAME_STAT_ARRAY_INJURED
    End                         // LIVE_GAME_STAT_ARRAY_END
}

/// <summary>
/// Team value categories. Corresponds to C enum GameTeamValue.
/// </summary>
public enum GameTeamValue
{
    Goalie = 0,                 // GAME_TEAM_VALUE_GOALIE
    Defend,                     // GAME_TEAM_VALUE_DEFEND
    Midfield,                   // GAME_TEAM_VALUE_MIDFIELD
    Attack,                     // GAME_TEAM_VALUE_ATTACK
    End                         // GAME_TEAM_VALUE_END
}

/// <summary>
/// Statistics for a live game. Corresponds to C struct LiveGameStats.
/// </summary>
public class LiveGameStats
{
    public float Possession { get; set; }
    public int[,] Values { get; set; } = new int[2, 10]; // [team][stat_type]
    public List<object>[,] Players { get; set; } = new List<object>[2, 5]; // [team][stat_array]
}

/// <summary>
/// An event occurring during a live game. Corresponds to C struct LiveGameEvent.
/// </summary>
public class LiveGameEvent
{
    public int Type { get; set; }
    public int Verbosity { get; set; }
    public int Team { get; set; }
    public int Player { get; set; }
    public int Player2 { get; set; }
    public string Commentary { get; set; }
    public int CommentaryId { get; set; }
}

/// <summary>
/// A fraction of a live game. Corresponds to C struct LiveGameUnit.
/// </summary>
public class LiveGameUnit
{
    public int Possession { get; set; }
    public int Area { get; set; }
    public int Minute { get; set; }
    public int Time { get; set; }
    public int[] Result { get; set; } = new int[2];
    public LiveGameEvent Event { get; set; }
}

/// <summary>
/// Team settings during a live game pause. Corresponds to C struct LiveGameTeamState.
/// </summary>
public class LiveGameTeamState
{
    public int Structure { get; set; }
    public int Style { get; set; }
    public bool Boost { get; set; }
    public int[] PlayerIds { get; set; } = new int[11];
}

/// <summary>
/// The complete record of a match simulation. Corresponds to C struct LiveGame.
/// </summary>
public class LiveGame
{
    public Fixture Fix { get; set; }
    public int FixId { get; set; }
    public string[] TeamNames { get; set; } = new string[2];
    public int Attendance { get; set; }
    public int[] SubsLeft { get; set; } = new int[2];
    public int StartedGame { get; set; }
    public int StadiumEvent { get; set; }
    public int[] TeamValues { get; set; } = new int[8]; // [team][value]
    public float HomeAdvantage { get; set; }
    public List<LiveGameUnit> Units { get; set; } = new List<LiveGameUnit>();
    public LiveGameStats Stats { get; set; } = new LiveGameStats();
    public List<LiveGameTeamState> TeamState { get; set; } = new List<LiveGameTeamState>();
    public List<List<int>>[] ActionIds { get; set; } = new List<List<int>>[2];
    public List<string>[] Waivers { get; set; } = new List<string>[2]; // For completeness
}
