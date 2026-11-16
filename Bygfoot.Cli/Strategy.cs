namespace Bygfoot.Cli;

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
    private static readonly List<List<string>> TokenRepStrategy = new List<List<string>> { new List<string>(), new List<string>() };

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
        Player pl1 = (Player)a;
        Player pl2 = (Player)b;
        StrategyPrematch strat = (StrategyPrematch)userData;
        int returnValue = 0;

        if (pl1.Pos != pl2.Pos)
            returnValue = Misc.MiscIntCompare((int)pl2.Pos, (int)pl1.Pos);
        else if (pl1.Cskill == 0 && pl2.Cskill > 0)
            returnValue = 1;
        else if (pl2.Cskill == 0 && pl1.Cskill > 0)
            returnValue = -1;
        else if (strat.MinFitness != 0 &&
            pl1.Fitness < strat.MinFitness &&
            pl2.Fitness >= strat.MinFitness)
            returnValue = 1;
        else if (strat.MinFitness != 0 &&
            pl1.Fitness >= strat.MinFitness &&
            pl2.Fitness < strat.MinFitness)
            returnValue = -1;
        else
        {
            float skill1 = Player.GetGameSkill(pl1, true, true);
            float skill2 = Player.GetGameSkill(pl2, true, true);

            if (strat.Lineup == (int)StratLineupType.Best)
                returnValue = Misc.MiscFloatCompare(skill1, skill2);
            else if (strat.Lineup == (int)StratLineupType.Weakest)
                returnValue = Misc.MiscFloatCompare(skill2, skill1);
            else if (strat.Lineup == (int)StratLineupType.Fittest)
            {
                returnValue = Misc.MiscFloatCompare(pl1.Fitness, pl2.Fitness);
                if (pl1.Fitness == pl2.Fitness)
                    returnValue = Misc.MiscFloatCompare(skill1, skill2);
            }
            else
                Debug.DebugPrintMessage("strategy_compare_players: unknown lineup type {0}", strat.Lineup);
        }

        return returnValue;
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
        List<int> ids = new List<int>();
        List<Player> newPlayers = new List<Player>();
        int[] form = { Misc.MathGetPlace(formation, 3),
            Misc.MathGetPlace(formation, 2),
            Misc.MathGetPlace(formation, 1) };
        int[] pos = { 0, 0, 0 };

        // Repair goalie if necessary.
        if (players[0].Cskill == 0)
            RepairPlayer(players[0]);

        ids.Add(players[0].Id);

        foreach (Player pl in players)
        {
            if (pl.Pos > 0 && pos[(int)pl.Pos - 1] < form[(int)pl.Pos - 1])
            {
                ids.Add(pl.Id);
                pos[(int)pl.Pos - 1]++;
            }
        }

        foreach (int id in ids)
            newPlayers.Add(Player.OfIdTeam(tm, id));

        foreach (Player pl in tm.Players)
        {
            if (!Misc.QueryMiscIntegerIsInGArray(pl.Id, ids.ToArray()))
                newPlayers.Add(pl);
        }

        tm.Players = newPlayers;

        tm.Structure = formation;
        Team.Rearrange(tm);
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
        pl.Fitness = (float)MathHelper.Rnd(
            OptionHelper.ConstFloat("float_player_fitness_lower"),
            OptionHelper.ConstFloat("float_player_fitness_upper"));
    }

    /// <summary>
    /// 'Repair' exactly as many players as are required to be able to make a lineup of healthy players with the primary formation of the given prematch. Corresponds to C: strategy_repair_players.
    /// </summary>
    /// <param name="players">The players.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    public static void RepairPlayers(List<Player> players, StrategyPrematch prematch)
    {
        int primaryFormation = int.Parse(prematch.Formations[0]);
        int[] form = { Misc.MathGetPlace(primaryFormation, 3),
            Misc.MathGetPlace(primaryFormation, 2),
            Misc.MathGetPlace(primaryFormation, 1) };
        int[] pos = { 0, 0, 0 };

        foreach (Player pl in players)
        {
            if (pl.Pos > 0 && pl.Cskill > 0)
                pos[(int)pl.Pos - 1]++;
        }

        for (int i = 0; i < 3; i++)
        {
            while (pos[i] < form[i])
                for (int j = 0; j < players.Count; j++)
                {
                    if (players[j].Pos == (PlayerPos)(i + 1) && players[j].Cskill == 0)
                    {
                        RepairPlayer(players[j]);
                        pos[i]++;
                    }
                }
        }

        players.Sort((pl1, pl2) => ComparePlayers(pl1, pl2, prematch));
    }

    /// <summary>
    /// Make team changes according to the rules in the prematch. Corresponds to C: strategy_apply_prematch.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="prematch">The pre-match strategy.</param>
    public static void ApplyPrematch(Team tm, StrategyPrematch prematch)
    {
        List<Player> players = Player.GetPointersFromArray(tm.Players);

        tm.Style = prematch.Style;
        tm.Boost = prematch.Boost;

        players.Sort((pl1, pl2) => ComparePlayers(pl1, pl2, prematch));

        int i = 0;

        for (i = 0; i < prematch.Formations.Count; i++)
        {
            if (QueryStrategyFormationPossible(
                players, prematch, int.Parse(prematch.Formations[i])))
            {
                UpdateLineup(
                    tm, players, prematch, int.Parse(prematch.Formations[i]));
                break;
            }
        }

        // We have to repair players to be able to satisfy
        // a formation.
        if (i == prematch.Formations.Count)
        {
            RepairPlayers(players, prematch);
            UpdateLineup(tm, players, prematch, int.Parse(prematch.Formations[0]));
        }
    }

    /// <summary>
    /// Make necessary subs etc. for a CPU team. Corresponds to C: strategy_update_team_pre_match.
    /// </summary>
    /// <param name="tm">The team.</param>
    public static void UpdateTeamPreMatch(Team tm)
    {
        Strategy strategy = FromSid(tm.StrategySid);

        SetTokens(tm, null);

        int i;

        for (i = strategy.Prematch.Count - 1; i >= 0; i--)
        {
            if (string.IsNullOrEmpty(strategy.Prematch[i].Condition) ||
                Misc.MiscParseCondition(strategy.Prematch[i].Condition, TokenRepStrategy))
            {
                ApplyPrematch(tm, strategy.Prematch[i]);
                break;
            }
        }

        FreeTokens();

        if (i == -1)
            Main.MainExitProgram(Bygfoot.ExitCode.StrategyError,
                "strategy_update_team_pre_match: none of the prematch conditions of strategy {0} for team {1} are fulfilled. remember that a strategy should contain an unconditional prematch.",
                tm.StrategySid, tm.Name);
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
        Fixture fixture = (fix == null) ? Team.GetFixture(tm, false) : fix;
        Team opp = (fixture == null) ? null :
            fixture.Teams[fixture.Teams[0] == tm ? 1 : 0];

        TokenRepStrategy[0].Clear();
        TokenRepStrategy[1].Clear();

        if (opp == null)
            return;

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_homeadv"),
            Misc.MiscIntToChar((fixture.Teams[0] == tm ? 1 : -1) * (fixture.HomeAdvantage ? 1 : 0)));

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_cup"),
            Misc.MiscIntToChar(fixture.Clid >= Bygfoot.IdCupStart ? 1 : 0));

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_avskilldiff"),
            Misc.MiscIntToChar((int)Math.Round(Team.GetAverageSkill(tm, false) -
                Team.GetAverageSkill(opp, false))));

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_opponent_skill"),
            Misc.MiscIntToChar((int)Math.Round(Team.GetAverageSkill(opp, false))));

        if (tm.Clid < Bygfoot.IdCupStart &&
            opp.Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRepStrategy,
                Variables.Tokens.GetInt("string_token_team_layerdiff"),
                Misc.MiscIntToChar(League.LeagueFromClid(tm.Clid).Layer -
                    League.LeagueFromClid(opp.Clid).Layer));

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_goals_to_win"),
            Misc.MiscIntToChar(Fixture.GetGoalsToWin(fixture, tm)));
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
        LiveGameUnit lastUnit = match.Units[match.Units.Count - 1];
        int currentMin = LiveGame.UnitGetMinute(match, lastUnit);
        Team tm = match.Fix.Teams[teamIdx];

        SetTokens(tm, match.Fix);

        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_subs_left"),
            Misc.MiscIntToChar(match.SubsLeft[teamIdx]));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_num_def"),
            Misc.MiscIntToChar(Misc.MathGetPlace(tm.Structure, 3)));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_num_mid"),
            Misc.MiscIntToChar(Misc.MathGetPlace(tm.Structure, 2)));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_num_att"),
            Misc.MiscIntToChar(Misc.MathGetPlace(tm.Structure, 1)));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_form"),
            Misc.MiscIntToChar(tm.Structure));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_time"),
            Misc.MiscIntToChar(lastUnit.Time));
        Misc.MiscTokenAdd(TokenRepStrategy,
            Variables.Tokens.GetInt("string_token_minute"),
            Misc.MiscIntToChar(currentMin));

        int tmpInt = LiveGame.GetMinutesRemaining(match, lastUnit);

        if (tmpInt > 0)
            Misc.MiscTokenAdd(TokenRepStrategy,
                Variables.Tokens.GetInt("string_token_minute_remaining"),
                Misc.MiscIntToChar(tmpInt));

        if (Fixture.QueryFixtureIsDraw(match.Fix))
            tmpInt = 120 - currentMin;
        else
            tmpInt = 90 - currentMin;

        if (tmpInt > 0)
            Misc.MiscTokenAdd(TokenRepStrategy,
                Variables.Tokens.GetInt("string_token_minute_total"),
                Misc.MiscIntToChar(tmpInt));
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
        Player pl1 = (Player)a;
        Player pl2 = (Player)b;
        int position = ((int)userData) % 10;
        int property = ((int)userData - position) / 10;
        float skill1, skill2;
        int returnValue = 0;

        if (pl1.Pos != pl2.Pos &&
            (pl1.Pos == (PlayerPos)position || pl2.Pos == (PlayerPos)position))
            returnValue = (pl1.Pos == (PlayerPos)position) ? -1 : 1;
        else
        {
            switch (property)
            {
                default:
                    Debug.DebugPrintMessage("strategy_compare_players_sub: unknown property {0}", property);
                    returnValue = 0;
                    break;
                case (int)StratLineupType.Fittest:
                    returnValue = Misc.MiscFloatCompare(pl1.Fitness, pl2.Fitness);
                    break;
                case (int)StratLineupType.Unfittest:
                    returnValue = Misc.MiscFloatCompare(pl2.Fitness, pl1.Fitness);
                    break;
                case (int)StratLineupType.Best:
                    skill1 = Player.GetGameSkill(pl1, true, true);
                    skill2 = Player.GetGameSkill(pl2, true, true);
                    returnValue = Misc.MiscFloatCompare(skill1, skill2);
                    break;
                case (int)StratLineupType.Weakest:
                    skill1 = Player.GetGameSkill(pl1, true, true);
                    skill2 = Player.GetGameSkill(pl2, true, true);
                    returnValue = Misc.MiscFloatCompare(skill2, skill1);
                    break;
            }
        }

        return returnValue;
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
        int pos1 = (int)a;
        int pos2 = (int)b;
        Team tm = (Team)userData;
        int[] pos = { 0, 0, 0, 0 };

        for (int i = 0; i < 11; i++)
        {
            Player pl = Player.OfIdxTeam(tm, i);
            if (pl.Cskill > 0 && Player.IsBanned(pl) <= 0)
                pos[(int)pl.Pos]++;
        }

        return Misc.MiscIntCompare(pos[pos1], pos[pos2]);
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
        int i, start = subIn ? 11 : 0,
            stop = subIn ? tm.Players.Count : 11;
        List<Player> players = new List<Player>();
        int returnValue = -1;
        List<int> positions = new List<int>();

        if (position < 90)
            positions.Add(position);
        else
        {
            while (position >= 90)
            {
                i = Misc.MathGetPlace(position, 1);
                positions.Add(i);
                position = (position - position % 10) / 10;
            }

            positions.Sort((pos1, pos2) => ComparePositions(pos1, pos2, tm));
        }

        for (i = start; i < stop; i++)
        {
            Player pl = Player.OfIdxTeam(tm, i);
            if (pl.Cskill > 0 &&
                Misc.QueryMiscIntegerIsInGArray((int)pl.Pos, positions.ToArray()) &&
                (i > 10 || Player.IsBanned(pl) <= 0))
                players.Add(pl);
        }

        if (players.Count == 0)
            return -1;

        players.Sort((pl1, pl2) => ComparePlayersSub(pl1, pl2, property * 10 + positions[0]));

        returnValue = players[0].Id;

        return returnValue;
    }

    /// <summary>
    /// Apply the strategy actions specified to the given team. Corresponds to C: strategy_live_game_apply_action.
    /// </summary>
    /// <param name="match">The live game.</param>
    /// <param name="teamIdx">The team index.</param>
    /// <param name="action">The action.</param>
    public static void LiveGameApplyAction(LiveGame match, int teamIdx, StrategyMatchAction action)
    {
        int subInId = -1, subOutId = -1;
        Team tm = match.Fix.Teams[teamIdx];
        int oldForm = tm.Structure;

        match.ActionIds[teamIdx].Add(action.Id);

        if (action.Style != -100 && tm.Style != action.Style)
        {
            tm.Style = action.Style;
            LiveGame.EventTeamChange(match, teamIdx,
                (int)LiveGameEventType.StyleChangeAllOutDefend + tm.Style + 2);
        }

        if ((action.Boost != -100 && tm.Boost != action.Boost) &&
            (action.Boost != 1 || OptionHelper.SettInt("int_opt_disable_boost_on") == 0))
        {
            tm.Boost = action.Boost;
            LiveGame.EventTeamChange(match, teamIdx,
                (int)LiveGameEventType.BoostChangeAnti + tm.Boost + 1);
        }

        if (action.SubInPos != -1 && match.SubsLeft[teamIdx] > 0 &&
            (string.IsNullOrEmpty(action.SubCondition) ||
             Misc.MiscParseCondition(action.SubCondition, TokenRepStrategy)))
        {
            subInId = GetSub(tm, action.SubInPos, action.SubInProp, true);
            subOutId = GetSub(tm, action.SubOutPos, action.SubInProp, false);

            if (subInId > 0 && subOutId > 0)
            {
                Player.Swap(tm, Player.IdIndex(tm, subOutId, true),
                    tm, Player.IdIndex(tm, subInId, true));

                Team.ChangeStructure(tm, Team.FindAppropriateStructure(tm));
                Team.Rearrange(tm);

                LiveGame.EventSubstitution(match, teamIdx, subInId, subOutId);

                if (tm.Structure != oldForm)
                    LiveGame.EventTeamChange(match, teamIdx,
                        (int)LiveGameEventType.StructureChange);
            }
        }
    }

    /// <summary>
    /// Take match actions specified in the team's strategy if necessary. Corresponds to C: strategy_live_game_check.
    /// </summary>
    /// <param name="match">The live game.</param>
    /// <param name="teamIdx">The team index.</param>
    public static void LiveGameCheck(LiveGame match, int teamIdx)
    {
        Team tm = match.Fix.Teams[teamIdx];
        Strategy strat = FromSid(tm.StrategySid);

        LiveGameSetTokens(match, teamIdx);

        for (int i = strat.MatchAction.Count - 1; i >= 0; i--)
        {
            if ((match.SubsLeft[teamIdx] > 0 ||
                strat.MatchAction[i].SubInPos == -1) &&
                !Misc.QueryMiscIntegerIsInGArray(strat.MatchAction[i].Id, match.ActionIds[teamIdx].ToArray()) &&
                (string.IsNullOrEmpty(strat.MatchAction[i].Condition) ||
                 Misc.MiscParseCondition(strat.MatchAction[i].Condition, TokenRepStrategy)))
            {
                LiveGameApplyAction(match, teamIdx, strat.MatchAction[i]);
                break;
            }
        }

        FreeTokens();
    }
}
