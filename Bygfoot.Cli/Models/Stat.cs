namespace Bygfoot.Cli.Models;

/// <summary>
/// A statistics element holding some string and integer values. 
/// Corresponds to C Stat struct.
/// </summary>
public class Stat
{
    /// <summary>The team name.</summary>
    public string TeamName { get; set; }

    /// <summary>Primary integer value.</summary>
    public int Value1 { get; set; }

    /// <summary>Secondary integer value.</summary>
    public int Value2 { get; set; }

    /// <summary>Tertiary integer value.</summary>
    public int Value3 { get; set; }

    /// <summary>A descriptive string for this statistic.</summary>
    public string ValueString { get; set; }
}

/// <summary>
/// A structure holding some stat arrays about a league. 
/// Corresponds to C LeagueStat struct.
/// </summary>
public class LeagueStat
{
    /// <summary>The league symbol.</summary>
    public string LeagueSymbol { get; set; }

    /// <summary>The league name.</summary>
    public string LeagueName { get; set; }

    /// <summary>Best offensive and defensive teams. Corresponds to C: GArray *teams_off, *teams_def.</summary>
    public List<TeamRankEntry> TeamsOff { get; set; } = new List<TeamRankEntry>();

    /// <summary>Best defensive and offensive teams. Corresponds to C: GArray *teams_def, *teams_off.</summary>
    public List<TeamRankEntry> TeamsDef { get; set; } = new List<TeamRankEntry>();

    /// <summary>Best goal getters and goalies. Corresponds to C: GArray *player_scorers, *player_goalies.</summary>
    public List<PlayerRankEntry> PlayerScorers { get; set; } = new List<PlayerRankEntry>();

    /// <summary>Best goalies and goal scorers. Corresponds to C: GArray *player_goalies, *player_scorers.</summary>
    public List<PlayerRankEntry> PlayerGoalies { get; set; } = new List<PlayerRankEntry>();
}

/// <summary>
/// A team name and a competition name. 
/// Corresponds to C ChampStat struct.
/// </summary>
public class ChampStat
{
    /// <summary>The team name.</summary>
    public string TeamName { get; set; }

    /// <summary>The competition name.</summary>
    public string ClName { get; set; }
}

/// <summary>
/// A season statistics structure. 
/// Corresponds to C SeasonStat struct.
/// </summary>
public class SeasonStat
{
    /// <summary>Which season.</summary>
    public int SeasonNumber { get; set; }

    /// <summary>League and cup winners. Corresponds to C: GArray *league_champs, *cup_champs.</summary>
    public List<string> LeagueChamps { get; set; } = new List<string>();

    /// <summary>League and cup winners. Corresponds to C: GArray *cup_champs, *league_champs.</summary>
    public List<string> CupChamps { get; set; } = new List<string>();

    /// <summary>The league stats at the end of the season. Corresponds to C: GArray *league_stats.</summary>
    public List<LeagueStat> LeagueStats { get; set; } = new List<LeagueStat>();
}

/// <summary>
/// A rank entry used within a larger statistical body.
/// </summary>
public class TeamRankEntry
{
    public string TeamName { get; set; }
}

/// <summary>
/// A rank entry for a player.
/// </summary>
public class PlayerRankEntry
{
    public string PlayerName { get; set; }
}
