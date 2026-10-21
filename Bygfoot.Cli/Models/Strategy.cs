namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Lineup types for a CPU team (ie. which players are preferred when putting together the first 11). 
/// Corresponds to C enum StratLineupType.
/// </summary>
public enum StratLineupType
{
    Best = 1,        // STRAT_LINEUP_BEST
    Weakest,         // STRAT_LINEUP_WEAKEST
    Fittest,         // STRAT_LINEUP_FITTEST
    Unfittest,       // STRAT_LINEUP_UNFITTEST
    End              // STRent_lineup_end (sentinel)
}

/// <summary>
/// A struct describing the pre-match strategy settings of a CPU team. 
/// Corresponds to C struct StrategyPrematch.
/// </summary>
public class StrategyPrematch
{
    /// <summary>A condition describing when the strategy should be applied. Corresponds to C: gchar *condition.</summary>
    public string Condition { get; set; }

    /// <summary>Array of possible formations, sorted by preference. Corresponds to C: GArray *formations.</summary>
    public List<string> Formations { get; set; } = new List<string>();

    /// <summary>Boost, style values and lineup type. Corresponds to C: gint boost, style, lineup.</summary>
    public int Boost { get; set; }
    public int Style { get; set; }
    public int Lineup { get; set; }

    /// <summary>The fitness value below which a player gets substituted if there's a sub with better fitness. Corresponds to C: gfloat min_fitness.</summary>
    public float MinFitness { get; set; }
}

/// <summary>
/// A structure describing a own-team action during a match. 
/// Corresponds to C struct StrategyMatchAction.
/// </summary>
public class StrategyMatchAction
{
    /// <summary>A condition describing when the action should be taken. Corresponds to C: gchar *condition, *sub_condition.</summary>
    public string Condition { get; set; }
    public string SubCondition { get; set; }

    /// <summary>New boost and style values. Corresponds to C: gint boost, style.</summary>
    public int Boost { get; set; }
    public int Style { get; set; }

    /// <summary>Substitution specifiers (position and property). Corresponds to C: gint sub_in_pos, sub_in_prop, sub_out_pos, sub_out_prop.</summary>
    public int SubInPos { get; set; }
    public int SubInProp { get; set; }
    public int SubOutPos { get; set; }
    public int SubOutProp { get; set; }

    /// <summary>An id to prevent actions from being applied again and again during a match. Corresponds to C: gint id.</summary>
    public int Id { get; set; }
}

/// <summary>
/// A CPU strategy. 
/// Corresponds to C struct Strategy.
/// </summary>
public class Strategy
{
    /// <summary>String id and description of the strategy. Corresponds to C: gchar *sid, *desc.</summary>
    public string Sid { get; set; }
    public string Desc { get; set; }

    /// <summary>How often this strategy gets picked, relative to the other strategies. Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>Array with prematch settings. Corresponds to C: GArray *prematch.</summary>
    public List<StrategyPrematch> Prematch { get; set; } = new List<StrategyPrematch>();

    /// <summary>Array with match settings. Corresponds to C: GArray *match_action.</summary>
    public List<StrategyMatchAction> MatchAction { get; set; } = new List<StrategyMatchAction>();
}
