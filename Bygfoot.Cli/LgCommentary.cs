namespace Bygfoot.Cli;

/// <summary>
/// Structure describing a commentary for the live game.
/// Corresponds to C struct LGCommentary.
/// </summary>
public class LgCommentary
{
    /// <summary>The commentary text (possibly containing tokens). Corresponds to C: gchar *text.</summary>
    public string Text { get; set; }

    /// <summary>A condition (if not fulfilled, the commentary doesn't get shown). Corresponds to C: gchar *condition.</summary>
    public string? Condition { get; set; }

    /// <summary>Priority of the commentary (compared to the other ones for the same event). Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>An id to keep track of already used commentaries in the live game. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>The replacement strings for the live game commentary tokens.
    /// Corresponds to C: GPtrArray *token_rep[2] (index 0: tokens, index 1: replacements).</summary>
    public static List<List<string>> TokenRep { get; } = new() { new(), new() };

    /// <summary>Whether there was a commentary repetition rejection. Corresponds to C: repetition.</summary>
    private static bool Repetition;

    /// <summary>
    /// Generate commentary for the live game event in the unit.
    /// Corresponds to C: lg_commentary_generate.
    /// </summary>
    /// <param name="liveGame">The live game being calculated (or null when testing).</param>
    /// <param name="unit">The live game unit we generate the commentary for (or null when testing).</param>
    /// <param name="eventName">The event name we generate the commentary for (needed when commentary is being tested).</param>
    /// <param name="evType">The event type we generate the commentary for (needed when commentary is being tested).</param>
    public static void Generate(LiveGame? liveGame, LiveGameUnit? unit, string? eventName, int evType)
    {
        int i;
        int eventType = -1;
        int commentaryIdx = -1;
        string buf = string.Empty;
        List<LgCommentary>? commentaries = null;

        if (unit != null)
            eventType = unit.Event.Type;
        else
            eventType = (eventName == null) ? evType :
                LgCommentaryXml.XmlLgCommentaryEventNameToInt(eventName);

        if (eventType == -1)
            return;

        if (liveGame != null)
        {
            SetBasicTokens(liveGame, unit, liveGame.Fix);
            SetTeamTokens(unit, liveGame.Fix);
            SetPlayerTokens(unit, liveGame.Fix);
            SetStatsTokens(liveGame.Stats);
        }

        if (eventType == (int)LiveGameEventType.StyleChangeAllOutDefend ||
            eventType == (int)LiveGameEventType.StyleChangeDefend ||
            eventType == (int)LiveGameEventType.StyleChangeBalanced ||
            eventType == (int)LiveGameEventType.StyleChangeAttack ||
            eventType == (int)LiveGameEventType.StyleChangeAllOutAttack)
            commentaries = Variables.LgCommentary[(int)LiveGameEventType.StyleChangeAllOutDefend];
        else if (eventType == (int)LiveGameEventType.BoostChangeAnti ||
            eventType == (int)LiveGameEventType.BoostChangeOff ||
            eventType == (int)LiveGameEventType.BoostChangeOn)
            commentaries = Variables.LgCommentary[(int)LiveGameEventType.BoostChangeAnti];
        else
            commentaries = Variables.LgCommentary[eventType];

        commentaryIdx = Select(commentaries, ref buf);

        if (commentaryIdx == -1)
            Debug.DebugPrintMessage("lg_commentary_generate: didn't find fitting commentary for unit type {0} ", eventType);

        if (liveGame != null)
        {
            unit.Event.Commentary = buf;
            unit.Event.CommentaryId = (commentaryIdx == -1) ?
                -1 : commentaries[commentaryIdx].Id;

            for (i = 0; i < Variables.Tokens.List.Count; i++)
                if (i != OptionHelper.OptInt("string_token_team_home") &&
                    i != OptionHelper.OptInt("string_token_team_away") &&
                    i != OptionHelper.OptInt("string_token_attendance") &&
                    i != OptionHelper.OptInt("string_token_cup_round_name") &&
                    i != OptionHelper.OptInt("string_token_league_cup_name") &&
                    i != OptionHelper.OptInt("string_token_yellow_limit") &&
                    i != OptionHelper.OptInt("string_token_team_layer0") &&
                    i != OptionHelper.OptInt("string_token_team_layer1"))
                    Misc.MiscTokenRemove(TokenRep, i);
        }
        else
            Console.WriteLine($"{eventName}: \"{buf}\"");
    }

    /// <summary>
    /// Select a commentary from the list depending on the tokens
    /// available and write the text into the buffer.
    /// Corresponds to C: lg_commentary_select.
    /// </summary>
    /// <param name="commentaries">The commentaries to select from.</param>
    /// <param name="buf">Receives the commentary text.</param>
    /// <returns>The index of the selected commentary or -1 if none fit.</returns>
    public static int Select(List<LgCommentary> commentaries, ref string buf)
    {
        int i;
        int[] order = new int[commentaries.Count];

        GetOrder(commentaries, order);

        Repetition = false;

        for (i = 0; i < commentaries.Count; i++)
            if (CheckCommentary(commentaries[order[i]], ref buf))
                break;

        if (i == commentaries.Count)
        {
            Repetition = true;
            for (i = 0; i < commentaries.Count; i++)
                if (CheckCommentary(commentaries[order[i]], ref buf))
                    break;
        }

        if (i == commentaries.Count)
        {
            buf = "FIXME!";
            return -1;
        }

        return order[i];
    }

    /// <summary>
    /// Check whether the commentary conditions are fulfilled and whether
    /// we can replace all tokens in the commentary.
    /// Corresponds to C: lg_commentary_check_commentary.
    /// </summary>
    /// <param name="commentary">The commentary we check.</param>
    /// <param name="dest">Receives the commentary text with the tokens replaced.</param>
    /// <returns>Whether the commentary can be used.</returns>
    private static bool CheckCommentary(LgCommentary commentary, ref string dest)
    {
        if (commentary.Text.Length == 0 ||
            (commentary.Condition != null &&
             !Misc.MiscParseCondition(commentary.Condition, TokenRep)) ||
            (!Repetition && QueryIsRepetition(commentary.Id)))
            return false;

        return Misc.MiscStringReplaceAllTokens(TokenRep, commentary.Text, out dest);
    }

    /// <summary>
    /// Check whether a commentary with given id has been used in the last
    /// few commentaries. Return true if so and false otherwise.
    /// Corresponds to C: query_lg_commentary_is_repetition.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="id">The commentary id.</param>
    /// <returns>Whether the commentary was used recently.</returns>
    private static bool QueryIsRepetition(int id)
    {
        List<LiveGameUnit> units = ((LiveGame)Variables.Statp).Units;
        int unitsMin = units.Count - OptionHelper.ConstInt("int_lg_commentary_check_backwards") - 1;
        int min = Math.Max(unitsMin, 0);

        for (int i = units.Count - 2; i >= min; i--)
            if (units[i].Event.CommentaryId == id)
                return true;

        return false;
    }

    /// <summary>
    /// Write a random order of indices into the integer array
    /// (only depending on the priority values of the commentaries).
    /// Corresponds to C: lg_commentary_get_order.
    /// </summary>
    /// <param name="commentaries">The commentaries.</param>
    /// <param name="order">Receives the random order of indices.</param>
    private static void GetOrder(List<LgCommentary> commentaries, int[] order)
    {
        int i, j, orderIdx = 0;
        int prioritySum = 0;
        int[] bounds = new int[commentaries.Count + 1];

        bounds[0] = 0;

        for (i = 0; i < commentaries.Count; i++)
        {
            prioritySum += commentaries[i].Priority;
            bounds[i + 1] = prioritySum;
            order[i] = -1;
        }

        int[] permutation = new int[prioritySum];

        MathHelper.GeneratePermutation(permutation, 1, prioritySum);

        for (i = 0; i < prioritySum; i++)
        {
            if (orderIdx == commentaries.Count)
                break;

            for (j = 0; j < commentaries.Count; j++)
                if (bounds[j] < permutation[i] && permutation[i] <= bounds[j + 1])
                {
                    if (!Misc.QueryIntegerIsInArray(j, order, commentaries.Count))
                    {
                        order[orderIdx] = j;
                        orderIdx++;
                    }

                    break;
                }
        }
    }

    /// <summary>Fill the stats tokens. Corresponds to C: lg_commentary_set_stats_tokens.</summary>
    /// <param name="stats">The live game stats.</param>
    public static void SetStatsTokens(LiveGameStats stats)
    {
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_shots0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Shots]));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_shot_per0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.ShotPercentage]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_poss0"),
            Misc.MiscIntToChar((int)Math.Round(100 *
                ((float)stats.Values[0, (int)LiveGameStatValue.Possession] /
                 ((float)stats.Values[0, (int)LiveGameStatValue.Possession] +
                  (float)stats.Values[1, (int)LiveGameStatValue.Possession])))));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_pen0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Penalties]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_fouls0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Fouls]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_yellows0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Cards]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_reds0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Reds]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_injs0"),
            Misc.MiscIntToChar(stats.Values[0, (int)LiveGameStatValue.Injuries]));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_shots1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Shots]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_shot_per1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.ShotPercentage]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_poss1"),
            Misc.MiscIntToChar((int)Math.Round(100 * ((float)stats.Values[1, (int)LiveGameStatValue.Possession] /
                ((float)stats.Values[0, (int)LiveGameStatValue.Possession] +
                 (float)stats.Values[1, (int)LiveGameStatValue.Possession])))));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_pen1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Penalties]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_fouls1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Fouls]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_yellows1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Cards]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_reds1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Reds]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_stat_injs1"),
            Misc.MiscIntToChar(stats.Values[1, (int)LiveGameStatValue.Injuries]));
    }

    /// <summary>Fill the tokens that contain general information.
    /// Corresponds to C: lg_commentary_set_basic_tokens.</summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The unit.</param>
    /// <param name="fix">The fixture.</param>
    public static void SetBasicTokens(LiveGame liveGame, LiveGameUnit unit, Fixture fix)
    {
        int tmpInt = 1, currentMin = LiveGame.UnitGetMinute(liveGame, unit);
        int avskill0 = (int)Math.Round(Team.GetAverageSkill(fix.Teams[0], true));
        int avskill1 = (int)Math.Round(Team.GetAverageSkill(fix.Teams[1], true));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_result"),
            $"{unit.Result[0]} : {unit.Result[1]}");

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_time"),
            Misc.MiscIntToChar(unit.Time));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_minute"),
            Misc.MiscIntToChar(currentMin));

        tmpInt = LiveGame.GetMinutesRemaining(liveGame, unit);

        if (tmpInt > 0)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_minute_remaining"),
                Misc.MiscIntToChar(tmpInt));

        if (Fixture.QueryFixtureIsDraw(fix))
            tmpInt = 120 - currentMin;
        else
            tmpInt = 90 - currentMin;

        if (tmpInt > 0)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_minute_total"),
                Misc.MiscIntToChar(tmpInt));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_goals0"),
            Misc.MiscIntToChar(unit.Result[0]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_goals1"),
            Misc.MiscIntToChar(unit.Result[1]));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_goal_diff"),
            Misc.MiscIntToChar(Math.Abs(unit.Result[0] - unit.Result[1])));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_extra"),
            GetExtraData(unit, fix) ?? string.Empty);

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_possession"),
            Misc.MiscIntToChar(unit.Possession));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_no_possession"),
            Misc.MiscIntToChar(unit.Possession == 0 ? 1 : 0));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_team_avskill0"),
            Misc.MiscIntToChar(avskill0));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_team_avskill1"),
            Misc.MiscIntToChar(avskill1));
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_team_avskilldiff"),
            Misc.MiscIntToChar(Math.Abs(avskill0 - avskill1)));
    }

    /// <summary>Set the tokens containing team data. Corresponds to C: lg_commentary_set_team_tokens.</summary>
    /// <param name="unit">The unit.</param>
    /// <param name="fix">The fixture.</param>
    public static void SetTeamTokens(LiveGameUnit unit, Fixture fix)
    {
        if (unit.Result[0] != unit.Result[1])
        {
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_losing"),
                fix.Teams[unit.Result[0] > unit.Result[1] ? 0 : 1].Name);
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_winning"),
                fix.Teams[unit.Result[0] < unit.Result[1] ? 0 : 1].Name);
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_losingn"),
                Misc.MiscIntToChar(unit.Result[0] > unit.Result[1] ? 1 : 0));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_winningn"),
                Misc.MiscIntToChar(unit.Result[0] < unit.Result[1] ? 1 : 0));
        }

        if (unit.Event.Team != -1)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team"),
                Misc.MiscIntToChar(unit.Event.Team));
    }

    /// <summary>Set the player tokens. Corresponds to C: lg_commentary_set_player_tokens.</summary>
    /// <param name="unit">The unit.</param>
    /// <param name="fix">The fixture.</param>
    public static void SetPlayerTokens(LiveGameUnit unit, Fixture fix)
    {
        Player? pl1 = null;
        Player? pl2 = null;

        if (unit.Event.Type == (int)LiveGameEventType.General ||
            unit.Event.Type == (int)LiveGameEventType.ScoringChance ||
            unit.Event.Type == (int)LiveGameEventType.Header ||
            unit.Event.Type == (int)LiveGameEventType.Penalty ||
            unit.Event.Type == (int)LiveGameEventType.FreeKick)
        {
            pl1 = fix.Teams[unit.Possession].Players[Player.IdIndex(fix.Teams[unit.Possession], unit.Event.Player, true)];

            if (unit.Event.Player2 != -1)
                pl2 = fix.Teams[unit.Possession].Players[Player.IdIndex(fix.Teams[unit.Possession], unit.Event.Player2, true)];
        }
        else if (unit.Event.Type == (int)LiveGameEventType.SendOff ||
            unit.Event.Type == (int)LiveGameEventType.Injury ||
            unit.Event.Type == (int)LiveGameEventType.TempInjury ||
            unit.Event.Type == (int)LiveGameEventType.Substitution)
        {
            pl1 = fix.Teams[unit.Event.Team].Players[Player.IdIndex(fix.Teams[unit.Event.Team], unit.Event.Player, true)];
            if (unit.Event.Player2 != -1)
                pl2 = fix.Teams[unit.Event.Team].Players[Player.IdIndex(fix.Teams[unit.Event.Team], unit.Event.Player2, true)];
        }
        else if (unit.Event.Type == (int)LiveGameEventType.LostPossession ||
            unit.Event.Type == (int)LiveGameEventType.Goal ||
            unit.Event.Type == (int)LiveGameEventType.Miss ||
            unit.Event.Type == (int)LiveGameEventType.Save ||
            unit.Event.Type == (int)LiveGameEventType.Post ||
            unit.Event.Type == (int)LiveGameEventType.PlayerPushedInCorner ||
            unit.Event.Type == (int)LiveGameEventType.KeeperPushedInCorner ||
            unit.Event.Type == (int)LiveGameEventType.CrossBar)
        {
            pl1 = fix.Teams[unit.Possession].Players[Player.IdIndex(fix.Teams[unit.Possession], unit.Event.Player, true)];
            pl2 = fix.Teams[unit.Possession == 0 ? 1 : 0].Players[Player.IdIndex(fix.Teams[unit.Possession == 0 ? 1 : 0], unit.Event.Player2, true)];
        }
        else if (unit.Event.Type == (int)LiveGameEventType.OwnGoal)
            pl1 = fix.Teams[unit.Possession == 0 ? 1 : 0].Players[Player.IdIndex(fix.Teams[unit.Possession == 0 ? 1 : 0], unit.Event.Player, true)];
        else if (unit.Event.Type == (int)LiveGameEventType.CornerKick)
            pl1 = fix.Teams[unit.Event.Team].Players[Player.IdIndex(fix.Teams[unit.Event.Team], unit.Event.Player, true)];
        else if (unit.Event.Type == (int)LiveGameEventType.Foul ||
            unit.Event.Type == (int)LiveGameEventType.FoulRed ||
            unit.Event.Type == (int)LiveGameEventType.FoulRedInjury ||
            unit.Event.Type == (int)LiveGameEventType.FoulYellow)
        {
            pl1 = fix.Teams[unit.Event.Team == 0 ? 1 : 0].Players[Player.IdIndex(fix.Teams[unit.Event.Team == 0 ? 1 : 0], unit.Event.Player, true)];
            pl2 = fix.Teams[unit.Event.Team].Players[Player.IdIndex(fix.Teams[unit.Event.Team], unit.Event.Player2, true)];
        }

        if (pl1 != null)
        {
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player0"),
                Player.GetLastName(pl1.Name));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player_goals0"),
                Misc.MiscIntToChar(Player.GamesGoalsGet(pl1, fix.Clid, (int)PlayerValue.Goals)));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player_goals_all0"),
                Misc.MiscIntToChar(Player.AllGamesGoals(pl1, (int)PlayerValue.Goals)));
        }

        if (pl2 != null)
        {
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player1"),
                Player.GetLastName(pl2.Name));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player_goals1"),
                Misc.MiscIntToChar(Player.GamesGoalsGet(pl2, fix.Clid, (int)PlayerValue.Goals)));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player_goals_all1"),
                Misc.MiscIntToChar(Player.AllGamesGoals(pl2, (int)PlayerValue.Goals)));
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_player_yellows"),
                Misc.MiscIntToChar(Player.CardGet(pl2, fix.Clid, (int)PlayerValue.CardYellow)));
        }
    }

    /// <summary>Return some extra data depending on the unit type.
    /// Corresponds to C: lg_commentary_get_extra_data.</summary>
    /// <param name="unit">The unit.</param>
    /// <param name="fix">The fixture.</param>
    /// <returns>The extra data or null.</returns>
    public static string? GetExtraData(LiveGameUnit unit, Fixture fix)
    {
        switch (unit.Event.Type)
        {
            default:
                return null;

            case (int)LiveGameEventType.StructureChange:
                return $"{fix.Teams[unit.Event.Team].Structure}";

            case (int)LiveGameEventType.StyleChangeAllOutDefend:
                return "ALL OUT DEFEND";

            case (int)LiveGameEventType.StyleChangeDefend:
                return "DEFEND";

            case (int)LiveGameEventType.StyleChangeBalanced:
                return "BALANCED";

            case (int)LiveGameEventType.StyleChangeAttack:
                return "ATTACK";

            case (int)LiveGameEventType.StyleChangeAllOutAttack:
                return "ALL OUT ATTACK";

            case (int)LiveGameEventType.BoostChangeAnti:
                return "ANTI";

            case (int)LiveGameEventType.BoostChangeOff:
                return "OFF";

            case (int)LiveGameEventType.BoostChangeOn:
                return "ON";
        }
    }

    /// <summary>Allocate memory for the token array and fill some tokens
    /// at the beginning of the live game that don't change later.
    /// Corresponds to C: lg_commentary_initialize.</summary>
    /// <param name="fix">The fixture.</param>
    public static void Initialize(Fixture fix)
    {
        TokenRep[0].Clear();
        TokenRep[1].Clear();

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_team_home"),
            fix.Teams[0].Name);
        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_team_away"),
            fix.Teams[1].Name);

        if (fix.Teams[0].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_layer0"),
                Misc.MiscIntToChar(League.LeagueFromClid(fix.Teams[0].Clid)!.Layer));
        if (fix.Teams[1].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_layer1"),
                Misc.MiscIntToChar(League.LeagueFromClid(fix.Teams[1].Clid)!.Layer));

        if (fix.Teams[0].Clid < Bygfoot.IdCupStart &&
            fix.Teams[1].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_team_layerdiff"),
                Misc.MiscIntToChar(League.LeagueFromClid(fix.Teams[0].Clid)!.Layer -
                                  League.LeagueFromClid(fix.Teams[1].Clid)!.Layer));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_league_cup_name"),
            League.LeagueCupGetNameString(fix.Clid));

        if (fix.Clid >= Bygfoot.IdCupStart)
        {
            string buf = string.Empty;
            Cup.CupGetRoundName(Cup.CupFromClid(fix.Clid), fix.Round, ref buf);
            Misc.MiscTokenAdd(TokenRep,
                OptionHelper.OptInt("string_token_cup_round_name"),
                buf);
        }

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_attendance"),
            Misc.MiscPrintGroupedInt(fix.Attendance));

        Misc.MiscTokenAdd(TokenRep,
            OptionHelper.OptInt("string_token_yellow_limit"),
            Misc.MiscIntToChar((fix.Clid < Bygfoot.IdCupStart) ?
                League.LeagueFromClid(fix.Clid)!.YellowRed :
                Cup.CupFromClid(fix.Clid).YellowRed));
    }

    /// <summary>Free the memory occupied by the tokens array and the permanent tokens.
    /// Corresponds to C: lg_commentary_free_tokens.</summary>
    public static void FreeTokens()
    {
        for (int i = TokenRep[0].Count - 1; i >= 0; i--)
        {
            TokenRep[0].RemoveAt(i);
            TokenRep[1].RemoveAt(i);
        }
    }

    /// <summary>Load the appropriate commentary file by evaluating
    /// the language option. Corresponds to C: lg_commentary_load_commentary_file_from_option.</summary>
    public static void LoadCommentaryFileFromOption()
    {
        string buf = string.Empty;
        Language.LanguageGetCode(ref buf);

        string commentaryFile = $"lg_commentary_{buf}.xml";

        LoadCommentaryFile(commentaryFile, false);
    }

    /// <summary>Load the commentary file given. Abort if not found and abort is true,
    /// load default english file otherwise.
    /// Corresponds to C: lg_commentary_load_commentary_file.</summary>
    /// <param name="commentaryFile">The file to load.</param>
    /// <param name="abort">Whether to abort if the file is not found.</param>
    public static void LoadCommentaryFile(string commentaryFile, bool abort)
    {
        if (File.Exists(commentaryFile))
        {
            LgCommentaryXml.XmlLgCommentaryRead(commentaryFile, Variables.LgCommentary);
            return;
        }

        string? fileName = FileHelper.FindSupportFile(commentaryFile, false);

        if (fileName != null)
        {
            LgCommentaryXml.XmlLgCommentaryRead(fileName, Variables.LgCommentary);
        }
        else
        {
            if (abort)
                Main.MainExitProgram(Bygfoot.ExitCode.FileNotFound,
                    "lg_commentary_load_commentary_file: file {0} not found ", commentaryFile);
            else
                LoadCommentaryFile("lg_commentary_en.xml", true);
        }
    }
}
