namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// Lineup types for a CPU team (ie. which players are preferred when putting together the first 11). 
/// Corresponds to C enum StratLineupType.
/// </summary>
public enum StratLineupType
{
    Best = 1,        // STRAT_LINEUP_BEST
    Weakest,         // STRAT_LINEUP_WEAKEST
    Fittest,         // STRAT_LINEUP_FITTEST,
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
    public string Condition { get; set; } = string.Empty;

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
    public string Condition { get; set; } = string.Empty;
    public string SubCondition { get; set; } = string.Empty;

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
    public string Sid { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;

    /// <summary>How often this strategy gets picked, relative to the other strategies. Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>Array with prematch settings. Corresponds to C: GArray *prematch.</summary>
    public List<StrategyPrematch> Prematch { get; set; } = new List<StrategyPrematch>();

    /// <summary>Array with match action settings. Corresponds to C: GArray *match_action.</summary>
    public List<StrategyMatchAction> MatchAction { get; set; } = new List<StrategyMatchAction>();

    public static List<Strategy> AllStrategies { get; set; } = new List<Strategy>();

    /// <summary>
    /// The replacement strings for the strategy tokens. Corresponds to C: GPtrArray *token_strat[2].
    /// </summary>
    private static readonly List<List<string>> TokenRepStrategy = new List<List<string>>(2);

    /// <summary>
    /// Return the sid of a random strategy from the strategies array (also dependent on the priorities
    /// of the strategies). Corresponds to C: strategy_get_random.
    /// </summary>
    /// <returns>The sid of the random strategy.</returns>
    public static string GetRandom()
    {
        int rndom = Misc.MathRndi(1, Strategy.AllStrategies[Strategy.AllStrategies.Count - 1].Priority);

        if (rndom <= Strategy.AllStrategies[0].Priority)
            return Strategy.AllStrategies[0].Sid;

        for (int i = 1; i < Strategy.AllStrategies.Count; i++)
        {
            if (rndom <= Strategy.AllStrategies[i].Priority)
                return Strategy.AllStrategies[i].Sid;
        }

        Debug.DebugPrintMessage("strategy_get_random: no strategy found.");

        return "";
    }

    /// <summary>
    /// Compare function for sorting the players given a specific strategy. Corresponds to C: strategy_compare_players.
    /// </summary>
    /// <param name="a">The first player.</param>
    /// <param name="b">The second player.</param>
    /// <param name="userData">The user data (the strategy prematch).</param>
    /// <returns>The comparison result.</returns>
    public static int ComparePlayers(object a, object b, object userData)
    {
        // Implementation depends on the not yet converted player, team and strategy functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check whether a lineup described in the prematch using the given formation can be made with the players. Corresponds to C: query_strategy_formation_possible.
    /// </summary>
    /// <param name="players">The players.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    /// <param name="formation">The formation.</param>
    /// <returns>Whether the formation is possible.</returns>
    public static bool QueryStrategyFormationPossible(List<Player> players, StrategyPrematch prematch, int formation)
    {
        int[] pos = { 0, 0, 0 };

        foreach (Player pl in players)
        {
            if (pl.Pos > 0 && pl.Cskill > 0 && pl.Fitness >= prematch.MinFitness)
                pos[(int)pl.Pos - 1]++;
        }

        if (pos[2] >= Misc.MathGetPlace(formation, 1) &&
            pos[1] >= Misc.MathGetPlace(formation, 2) &&
            pos[0] >= Misc.MathGetPlace(formation, 3))
            return true;

        return false;
    }

    /// <summary>
    /// Make the necessary substitutions to satisfy the given prematch and formation requirements. Corresponds to C: strategy_update_lineup.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="players">The players.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    /// <param name="formation">The formation.</param>
    public static void UpdateLineup(Team tm, List<Player> players, StrategyPrematch prematch, int formation)
    {
        // Implementation depends on the not yet converted team and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Delete red cards, cure injuries etc. Used to make sure a CPU team doesn't break. Corresponds to C: strategy_repair_player.
    /// </summary>
    /// <param name="pl">The player.</param>
    public static void RepairPlayer(Player pl)
    {
        for (int i = 0; i < pl.Cards.Count; i++)
            pl.Cards[i].Red = 0;

        pl.Health = PlayerInjury.None;
        pl.Recovery = 0;
        pl.Cskill = Player.GetCskill(pl, (int)pl.Pos, false);
        pl.Fitness = 0;
    }

    /// <summary>
    /// 'Repair' exactly as many players as are required to be able to make a lineup of healthy players with the primary formation of the given prematch. Corresponds to C: strategy_repair_players.
    /// </summary>
    /// <param name="players">The players.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    public static void RepairPlayers(List<Player> players, StrategyPrematch prematch)
    {
        // Implementation depends on the not yet converted player and strategy functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Make team changes according to the rules in the prematch. Corresponds to C: strategy_apply_prematch.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    public static void ApplyPrematch(Team tm, StrategyPrematch prematch)
    {
        // Implementation depends on the not yet converted player and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Make necessary subs etc. for a CPU team. Corresponds to C: strategy_update_team_pre_match.
    /// </summary>
    /// <param name="tm">The team.</param>
    public static void UpdateTeamPreMatch(Team tm)
    {
        // Implementation depends on the not yet converted strategy and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the strategy going with the sid. Corresponds to C: strategy_from_sid.
    /// </summary>
    /// <param name="sid">The sid.</param>
    /// <returns>The strategy.</returns>
    public static Strategy FromSid(string sid)
    {
        foreach (Strategy strategy in Strategy.AllStrategies)
        {
            if (strategy.Sid == sid)
                return strategy;
        }

        Debug.DebugPrintMessage($"strategy_from_sid: strategy '{sid}' not found.");

        return new Strategy();
    }

    /// <summary>
    /// Add tokens that will be evaluated when checking strategy conditions. Corresponds to C: strategy_set_tokens.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="fix">The fixture.</param>
    public static void SetTokens(Team tm, Fixture fix)
    {
        // Implementation depends on the not yet converted option and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Free the token arrays. Corresponds to C: strategy_free_tokens.
    /// </summary>
    public static void FreeTokens()
    {
        for (int i = TokenRepStrategy[0].Count - 1; i >= 0; i--)
        {
            TokenRepStrategy[0][i] = null;
            TokenRepStrategy[1][i] = null;
        }

        TokenRepStrategy[0].Clear();
        TokenRepStrategy[1].Clear();
    }

    /// <summary>
    /// Fill the necessary tokens during a live game. Corresponds to C: strategy_live_game_set_tokens.
    /// </summary>
    /// <param name="match">The live game.</param>
    /// <param name="teamIdx">The team index.</param>
    public static void LiveGameSetTokens(LiveGame match, int teamIdx)
    {
        // Implementation depends on the not yet converted live game, team and option functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compare function for sorting the players when looking for substitutes. Corresponds to C: strategy_compare_players_sub.
    /// </summary>
    /// <param name="a">The first player.</param>
    /// <param name="b">The second player.</param>
    /// <param name="userData">The user data (the position and the property).</param>
    /// <returns>The comparison result.</returns>
    public static int ComparePlayersSub(object a, object b, object userData)
    {
        // Implementation depends on the not yet converted player and strategy functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compare two player positions, taking into account the number of players playing the position in the team. Corresponds to C: strategy_compare_positions.
    /// </summary>
    /// <param name="a">The first position.</param>
    /// <param name="b">The second position.</param>
    /// <param name="userData">The user data (the team).</param>
    /// <returns>The comparison result.</returns>
    public static int ComparePositions(object a, object b, object userData)
    {
        // Implementation depends on the not yet converted player and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find an appropriate player to send out or in. Corresponds to C: strategy_get_sub.
    /// </summary>
    /// <param name="tm">The team we work with.</param>
    /// <param name="position">The position of the player we seek.</param>
    /// <param name="property">According to which property to sort players.</param>
    /// <param name="subIn">Whether we look for a player to send in or out.</param>
    /// <returns>The id of the player.</returns>
    public static int GetSub(Team tm, int position, int property, bool subIn)
    {
        // Implementation depends on the not yet converted player and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Apply the strategy actions specified to the given team. Corresponds to C: strategy_live_game_apply_action.
    /// </summary>
    /// <param name="match">The live game.</param>
    /// <param name="teamIdx">The team index.</param>
    /// <param name="action">The action.</param>
    public static void LiveGameApplyAction(LiveGame match, int teamIdx, StrategyMatchAction action)
    {
        // Implementation depends on the not yet converted team and live game functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Take match actions specified in the team's strategy if necessary. Corresponds to C: strategy_live_game_check.
    /// </summary>
    /// <param name="match">The live game.</param>
    /// <param name="teamIdx">The team index.</param>
    public static void LiveGameCheck(LiveGame match, int teamIdx)
    {
        // Implementation depends on the not yet converted strategy and live game functions.
        throw new NotImplementedException();
    }
}
