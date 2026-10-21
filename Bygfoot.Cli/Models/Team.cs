namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Values for league/cup metadata related to a team. Corresponds to C enum LeagueCupValue.
/// </summary>
public enum LeagueCupValue
{
    Name = 0,               // LEAGUE_CUP_VALUE_NAME
    ShortName,              // LEAGUE_CUP_VALUE_SHORT_NAME
    Sid,                    // LEAGUE_CUP_VALUE_SID
    Symbol,                 // LEAGUE_CUP_VALUE_SYMBOL
    Id,                     // LEAGUE_CUP_VALUE_ID
    FirstWeek,              // LEAGUE_CUP_VALUE_FIRST_WEEK
    LastWeek,               // LEAGUE_CUP_VALUE_LAST_WEEK
    WeekGap,                // LEAGUE_CUP_VALUE_WEEK_GAP
    YellowRed,              // LEAGUE_CUP_VALUE_YELLOW_RED
    AverageSkill,           // LEAGUE_CUP_VALUE_AVERAGE_SKILL
    AverageCapacity,        // LEAGUE_CUP_VALUE_AVERAGE_CAPACITY
    SkillDiff,              // LEAGUE_CUP_VALUE_SKILL_DIFF
    End                     // LEAGUE_CUP_VALUE_END
}

/// <summary>
/// Team attributes. Corresponds to C enum TeamAttribute.
/// </summary>
public enum TeamAttribute
{
    Style = 0,              // TEAM_ATTRIBUTE_STYLE
    Boost,                  // TEAM_ATTRIBUTE_BOOST
    End                     // TEAM_ATTRIBUTE_END
}

/// <summary>
/// The stadium of a team. Corresponds to C struct Stadium.
/// </summary>
public class Stadium
{
    /// <summary>Name of the stadium. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>How many people fit in. Corresponds to C: gint capacity.</summary>
    public int Capacity { get; set; }

    /// <summary>How many people watched on average. Corresponds to C: gint average_attendance.</summary>
    public int AverageAttendance { get; set; }

    /// <summary>How many people would've watched if every game had been sold out. Corresponds to C: gint possible_attendance.</summary>
    public int PossibleAttendance { get; set; }

    /// <summary>Number of games. Corresponds to C: gint games.</summary>
    public int Games { get; set; }

    /// <summary>Safety percentage between 0 and 100. Corresponds to C: gfloat safety.</summary>
    public float Safety { get; set; }

    /// <summary>Ticket price. Corresponds to C: gfloat ticket_price.</summary>
    public float TicketPrice { get; set; }
}

/// <summary>
/// Structure representing a team. Corresponds to C struct Team.
/// </summary>
public class Team
{
    /// <summary>Name of the team. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Symbol of the team. Corresponds to C: gchar *symbol.</summary>
    public string Symbol { get; set; }

    /// <summary>File the team takes the player names from. Corresponds to C: gchar *names_file.</summary>
    public string NamesFile { get; set; }

    /// <summary>The def file. Corresponds to C: gchar *def_file.</summary>
    public string DefFile { get; set; }

    /// <summary>The sid of the strategy if it's a CPU team. Corresponds to C: gchar *strategy_sid.</summary>
    public string StrategySid { get; set; }

    /// <summary>Numerical id of the league or cup the team belongs to. Corresponds to C: gint clid.</summary>
    public int Clid { get; set; }

    /// <summary>Id of the team. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>Playing structure. Corresponds to C: gint structure.</summary>
    public int Structure { get; set; }

    /// <summary>Playing style. Corresponds to C: gint style.</summary>
    public int Style { get; set; }

    /// <summary>Whether player boost or anti-boost is switched on. Corresponds to C: gint boost.</summary>
    public int Boost { get; set; }

    /// <summary>Average talent of the players at generation. Corresponds to C: gfloat average_talent.</summary>
    public float AverageTalent { get; set; }

    /// <summary>A value that influences scoring chances etc. If > 1, the team's lucky, if < 1, it's unlucky. Corresponds to C: gfloat luck.</summary>
    public float Luck { get; set; }

    /// <summary>The stadium of the team. Corresponds to C: Stadium stadium.</summary>
    public Stadium Stadium { get; set; }

    /// <summary>Array of players. Corresponds to C: GArray *players.</summary>
    public List<Player> Players { get; set; } = new List<Player>();
}
