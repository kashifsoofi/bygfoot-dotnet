namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Information about what cup another cup has to wait for
/// before scheduling matches. Corresponds to C struct CupRoundWait.
/// </summary>
public class CupRoundWait
{
    /// <summary>The cup we wait for. Corresponds to C: gchar *cup_sid.</summary>
    public string CupSid { get; set; }

    /// <summary>The cup round of the cup we wait for. Corresponds to C: gint cup_round.</summary>
    public int CupRound { get; set; }
}

/// <summary>
/// Rules for a round of a cup.
/// Cups consist of rounds, e.g. the final counts as a round or the round robin games.
/// Corresponds to C struct CupRound.
/// </summary>
public class CupRound
{
    /// <summary>Name of the cup round. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Whether there are home and away games or only one leg. Corresponds to C: gboolean home_away.</summary>
    public bool HomeAway { get; set; }

    /// <summary>How many times a match gets repeated if there's a draw. Corresponds to C: gint replay.</summary>
    public int Replay { get; set; }

    /// <summary>Whether the matches are on neutral ground. Corresponds to C: gboolean neutral.</summary>
    public bool Neutral { get; set; }

    /// <summary>Whether the teams array gets randomised before writing the fixtures. Corresponds to C: gboolean randomise_teams.</summary>
    public bool RandomiseTeams { get; set; }

    /// <summary>How many round robin groups there are. Corresponds to C: gint round_robin_number_of_groups.</summary>
    public int RoundRobinNumberOfGroups { get; set; }

    /// <summary>How many teams advance from each group. Corresponds to C: gint round_robin_number_of_advance.</summary>
    public int RoundRobinNumberOfAdvance { get; set; }

    /// <summary>How many teams advance apart from those that are specified already. Corresponds to C: gint round_robin_number_of_best_advance.</summary>
    public int RoundRobinNumberOfBestAdvance { get; set; }

    /// <summary>How many matchdays there are in the round robin phase. Corresponds to C: gint round_robins.</summary>
    public int RoundRobins { get; set; }

    /// <summary>Number of weeks between the parts of a round robin. Corresponds to C: GArray *rr_breaks.</summary>
    public List<int> RrBreaks { get; set; } = new List<int>();

    /// <summary>Number of new teams participating in the cup round. Corresponds to C: gint new_teams.</summary>
    public int NewTeams { get; set; }

    /// <summary>The number of byes to be awarded for this cup round. Corresponds to C: gint byes.</summary>
    public int Byes { get; set; }

    /// <summary>Number of weeks the cup round is delayed (or scheduled sooner). Corresponds to C: gint delay.</summary>
    public int Delay { get; set; }

    /// <summary>Intervals of fixtures during which there should be two matches in a week. Corresponds to C: GArray *two_match_weeks[2].</summary>
    public List<int>[] TwoMatchWeeks { get; set; } = new List<int>[2];

    /// <summary>Whether the two matches of a home/away round are played in one week. Corresponds to C: gboolean two_match_week.</summary>
    public bool TwoMatchWeek { get; set; }

    /// <summary>The teams that got loaded for this cup round. Corresponds to C: GArray *teams.</summary>
    public List<int> Teams { get; set; } = new List<int>();

    /// <summary>Pointers to all teams loaded in the cup round. Corresponds to C: GPtrArray *team_ptrs.</summary>
    public List<int> TeamPtrs { get; set; } = new List<int>();

    /// <summary>Which new teams come into the cup. Corresponds to C: GArray *choose_teams.</summary>
    public List<CupChooseTeam> ChooseTeams { get; set; } = new List<CupChooseTeam>();

    /// <summary>The round robin tables. Corresponds to C: GArray *tables.</summary>
    public List<Table> Tables { get; set; } = new List<Table>();

    /// <summary>Array with CupRoundWaits. Corresponds to C: GArray *waits.</summary>
    public List<CupRoundWait> Waits { get; set; } = new List<CupRoundWait>();
}

/// <summary>
/// Rules for choosing teams from the league files. Corresponds to C struct CupChooseTeam.
/// </summary>
public class CupChooseTeam
{
    /// <summary>The string id of the league we choose from. Corresponds to C: gchar *sid.</summary>
    public string Sid { get; set; }

    /// <summary>The number of teams chosen. Corresponds to C: gint number_of_teams.</summary>
    public int NumberOfTeams { get; set; }

    /// <summary>Which league table to use. Corresponds to C: gint from_table.</summary>
    public int FromTable { get; set; }

    /// <summary>The range from which the teams are chosen. Corresponds to C: gint start_idx, end_idx.</summary>
    public int StartIdx { get; set; }
    public int EndIdx { get; set; }

    /// <summary>Whether the teams are chosen randomly. Corresponds to C: gboolean randomly.</summary>
    public bool Randomly { get; set; }

    /// <summary>Whether the team is generated and loaded from a league file. Corresponds to C: gboolean generate.</summary>
    public bool Generate { get; set; }

    /// <summary>Whether to skip the checking if a team participates in other of the same cup groupcups. Corresponds to C: gboolean skip_group_check.</summary>
    public bool SkipGroupCheck { get; set; }

    /// <summary>Whether to load the choose_team when the cup fixtures for the first cup round are written. Corresponds to C: gboolean preload.</summary>
    public bool Preload { get; set; }
}

/// <summary>
/// Structure representing a cup. Corresponds to C struct Cup.
/// </summary>
public class Cup
{
    /// <summary>Name, short name, symbol, and sid. Corresponds to C: gchar *name, *short_name, *symbol, *sid.</summary>
    public string Name { get; set; }
    public string ShortName { get; set; }
    public string Symbol { get; set; }
    public string Sid { get; set; }

    /// <summary>Numerical id. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>An integer specifying which cups are mutually exclusive. Corresponds to C: gint group.</summary>
    public int Group { get; set; }

    /// <summary>Last week and weeks between matchdays. Corresponds to C: gint last_week, week_gap.</summary>
    public int LastWeek { get; set; }
    public int WeekGap { get; set; }

    /// <summary>When the cup gets added to the acps pointer array. Corresponds to C: gint add_week.</summary>
    public int AddWeek { get; set; }

    /// <summary>Number of yellow cards that lead to a missed match. Corresponds to C: gint yellow_red.</summary>
    public int YellowRed { get; set; }

    /// <summary>Difference of the average talent for the cup teams. Corresponds to C: gfloat talent_diff.</summary>
    public float TalentDiff { get; set; }

    /// <summary>The week and week_round at the beginning of which the fixtures have to be updated. Corresponds to C: gint next_fixture_update_week, next_fixture_update_week_round.</summary>
    public int NextFixtureUpdateWeek { get; set; }
    public int NextFixtureUpdateWeekRound { get; set; }

    /// <summary>A pointer array of properties. Corresponds to C: GPtrArray *properties.</summary>
    public List<string> Properties { get; set; } = new List<string>();

    /// <summary>The rounds of the cup. Corresponds to C: GArray *rounds.</summary>
    public List<CupRound> Rounds { get; set; } = new List<CupRound>();

    /// <summary>Pointer array containing teams that got a bye for a round of the cup. Corresponds to C: GPtrArray *bye.</summary>
    public List<int> Bye { get; set; } = new List<int>();

    /// <summary>The teams belonging to the cup. Corresponds to C: GPtrArray *teams.</summary>
    public List<int> Teams { get; set; } = new List<int>();

    /// <summary>Pointer array with the names of all the teams in the cup. Corresponds to C: GPtrArray *team_names.</summary>
    public List<string> TeamNames { get; set; } = new List<string>();

    /// <summary>The fixtures of a season for the cup. Corresponds to C: GArray *fixtures.</summary>
    public List<Fixture> Fixtures { get; set; } = new List<Fixture>();

    /// <summary>Array of custom breaks in schedule. Corresponds to C: GArray *week_breaks.</summary>
    public List<int> WeekBreaks { get; set; } = new List<int>();

    /// <summary>Pointer array with the sids of competitions. Corresponds to C: GPtrArray *skip_weeks_with.</summary>
    public List<string> SkipWeeksWith { get; set; } = new List<string>();
}
