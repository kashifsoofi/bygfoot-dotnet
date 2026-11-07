namespace Bygfoot.Cli;

/// <summary>
/// Player types. Corresponds to C enum GamePlayerType.
/// </summary>
public enum GamePlayerType
{
    Defend = (int)LiveGameUnitArea.Defend,     // GAME_PLAYER_TYPE_DEFEND
    Midfield = (int)LiveGameUnitArea.Midfield, // GAME_PLAYER_TYPE_MIDFIELD
    Attack = (int)LiveGameUnitArea.Attack,     // GAME_PLAYER_TYPE_ATTACK
    Penalty,                                          // GAME_PLAYER_TYPE_PENALTY
    End                                               // GAME_PLAYER_TYPE_END
}

/// <summary>
/// Static class to handle the game logic functions. Corresponds to the C module src/game.h and src/game.c
/// (the game_get_values, game_get_player_contribution, game_get_player, game_get_player_probs, game_get_penalty_taker,
/// game_initialize, game_save_team_states, game_check_live_game_resume_state, game_get_subs, game_substitute_player,
/// game_player_injury, game_get_foul_possession_factor, game_get_foul_prob, game_find_to_substitute,
/// game_substitute_player_send_off, game_decrease_fitness, game_update_stats, game_update_stats_player, game_post_match,
/// game_assign_attendance, game_assign_attendance_neutral, game_stadium_event, game_get_max_values, game_reset_players and
/// game_get_default_penalty_shooter functions).
/// </summary>
public static class Game
{
    /// <summary>
    /// Calculate attacking, defending and goalie values for the two teams of a fixture.
    /// Corresponds to C: game_get_values.
    /// </summary>
    /// <param name="fix">The fixture we calculate.</param>
    /// <param name="teamValues">The place to store the values.</param>
    /// <param name="homeAdvantage">The home advantage factor.</param>
    public static void GetValues(Fixture fix, float[,] teamValues, float homeAdvantage)
    {
        Team[] tm = { fix.Teams[0], fix.Teams[1] };
        float styleFactor;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < (int)GameTeamValue.End; j++)
                teamValues[i, j] = 0;

            styleFactor = tm[i].Style * OptionHelper.ConstFloat("float_game_style_factor");

            teamValues[i, (int)GameTeamValue.Goalie] =
                Player.GetGameSkill(tm[i].Players[0], false, true) *
                (1 + homeAdvantage * (i == 0 ? 1 : 0));

            for (int j = 1; j < 11; j++)
            {
                if (tm[i].Players[j].Cskill > 0)
                {
                    teamValues[i, (int)GameTeamValue.Attack] +=
                        GetPlayerContribution(tm[i].Players[j], (int)GameTeamValue.Attack, true);
                    teamValues[i, (int)GameTeamValue.Midfield] +=
                        GetPlayerContribution(tm[i].Players[j], (int)GameTeamValue.Midfield, true);
                    teamValues[i, (int)GameTeamValue.Defend] +=
                        GetPlayerContribution(tm[i].Players[j], (int)GameTeamValue.Defend, true);
                }
            }

            for (int j = (int)GameTeamValue.Defend; j < (int)GameTeamValue.Defend + 3; j++)
                teamValues[i, j] *= (1 + homeAdvantage * (i == 0 ? 1 : 0));

            teamValues[i, (int)GameTeamValue.Defend] *= (1 - styleFactor);
            teamValues[i, (int)GameTeamValue.Attack] *= (1 + styleFactor);
        }
    }

    /// <summary>
    /// Return the contribution of a player to the attack, midfield or defend.
    /// Corresponds to C: game_get_player_contribution.
    /// </summary>
    /// <param name="pl">The player.</param>
    /// <param name="type">Whether we have defend, midfield or attack value.</param>
    /// <param name="special">Whether to count special influence like boost and streak.</param>
    /// <returns>The player's contribution depending on position and fitness.</returns>
    public static float GetPlayerContribution(Player pl, int type, bool special)
    {
        // How the cskill of field players get weighted for the team values in
        // a match. Rows are player position, columns value type.
        float[,] playerWeights =
            {
                { OptionHelper.ConstFloat("float_player_team_weight_defender_defense"),
                  OptionHelper.ConstFloat("float_player_team_weight_defender_midfield"),
                  OptionHelper.ConstFloat("float_player_team_weight_defender_attack") },
                { OptionHelper.ConstFloat("float_player_team_weight_midfielder_defense"),
                  OptionHelper.ConstFloat("float_player_team_weight_midfielder_midfield"),
                  OptionHelper.ConstFloat("float_player_team_weight_midfielder_attack") },
                { OptionHelper.ConstFloat("float_player_team_weight_forward_defense"),
                  OptionHelper.ConstFloat("float_player_team_weight_forward_midfield"),
                  OptionHelper.ConstFloat("float_player_team_weight_forward_attack") }
            };

        return Player.GetGameSkill(pl, false, special) *
            playerWeights[(int)pl.Cpos - 1, type - (int)GameTeamValue.Defend];
    }

    /// <summary>
    /// Return a random attacking or defending player
    /// or the player that has to shoot the number_of_penalty'th penalty.
    /// Corresponds to C: game_get_player.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <param name="playerType">Whether we concentrate on attacking or defending players or would like to have a penalty shooting player.</param>
    /// <param name="lastPenalty">The player that shot the last penalty.</param>
    /// <param name="notThisOne">A player to exclude.</param>
    /// <param name="skills">Whether to weight with skills, too.</param>
    /// <returns>A player index.</returns>
    public static int GetPlayer(Team tm, int playerType, int lastPenalty, int notThisOne, bool skills)
    {
        int player = notThisOne;
        float[] weights = new float[3];
        float[] probs = new float[10];
        double rndom;

        if (playerType == (int)GamePlayerType.Attack)
        {
            weights[0] = OptionHelper.ConstFloat("float_game_player_weight_attack_def");
            weights[1] = OptionHelper.ConstFloat("float_game_player_weight_attack_mid");
            weights[2] = OptionHelper.ConstFloat("float_game_player_weight_attack_fwd");
        }
        else if (playerType == (int)GamePlayerType.Midfield)
        {
            weights[0] = OptionHelper.ConstFloat("float_game_player_weight_midfield_def");
            weights[1] = OptionHelper.ConstFloat("float_game_player_weight_midfield_mid");
            weights[2] = OptionHelper.ConstFloat("float_game_player_weight_midfield_fwd");
        }
        else if (playerType == (int)GamePlayerType.Defend)
        {
            weights[0] = OptionHelper.ConstFloat("float_game_player_weight_defend_def");
            weights[1] = OptionHelper.ConstFloat("float_game_player_weight_defend_mid");
            weights[2] = OptionHelper.ConstFloat("float_game_player_weight_defend_fwd");
        }
        else if (playerType == (int)GamePlayerType.Penalty)
            return GetPenaltyTaker(tm, lastPenalty);
        else
            throw new Exception($"game_get_player: unknown player type {playerType}");

        GetPlayerProbs(tm.Players, probs, weights, skills);

        if (probs[9] > 0)
        {
            while (player == notThisOne)
            {
                rndom = MathHelper.Rnd(0, probs[9]);

                if (rndom < probs[0])
                    player = tm.Players[1].Id;
                else
                    for (int i = 1; i < 10; i++)
                        if (rndom < probs[i] && rndom > probs[i - 1])
                            player = tm.Players[i + 1].Id;
            }
        }
        else
        {
            Debug.DebugPrintMessage("game_get_player: All players injured or banned, apparently.");
            // The C version also prints the league/cup name (league_cup_get_name_string),
            // which is not converted yet.
            Console.WriteLine($"{tm.Name} player list:");
            for (int i = 0; i < tm.Players.Count; i++)
            {
                if (i < 10)
                    Console.Write($"prob {probs[i]:0.000}  ");
                Console.WriteLine($"{i} {tm.Players[i].Name,20} health {(int)tm.Players[i].Health} cskill {tm.Players[i].Cskill:0.00}");
            }
            throw new Exception("game_get_player: All players injured or banned, apparently.");
        }

        return player;
    }

    /// <summary>
    /// Write the probabilities for field players being picked
    /// (e.g. a random defender, or a player who gets injured) into
    /// the float array. Depends on skill mostly.
    /// Corresponds to C: game_get_player_probs.
    /// </summary>
    /// <param name="players">The players.</param>
    /// <param name="probs">Array with probabilities that gets filled.</param>
    /// <param name="weights">How to weight the players depending on their position.</param>
    /// <param name="skills">Whether to weight with skills, too.</param>
    public static void GetPlayerProbs(List<Player> players, float[] probs, float[] weights, bool skills)
    {
        probs[0] = (skills) ?
            Player.GetGameSkill(players[1], false, true) *
            weights[(int)players[1].Cpos - 1] :
            weights[(int)players[1].Cpos - 1] *
            (players[1].Cskill != 0 ? 1 : 0);

        probs[0] *= (1 + (float)players[1].Streak *
            OptionHelper.ConstFloat("float_player_streak_influence_skill"));

        for (int i = 1; i < 10; i++)
        {
            probs[i] = probs[i - 1] +
                ((skills) ?
                    Player.GetGameSkill(players[i + 1], false, true) *
                    weights[(int)players[i + 1].Cpos - 1] :
                    weights[(int)players[i + 1].Cpos - 1] *
                    (players[i + 1].Cskill != 0 ? 1 : 0));
            probs[i] *= (1 + (float)players[i + 1].Streak *
                OptionHelper.ConstFloat("float_player_streak_influence_skill"));
        }
    }

    /// <summary>
    /// Return the player who's shooting the following penalty
    /// (at penalty shoot-out).
    /// Corresponds to C: game_get_penalty_taker.
    /// </summary>
    /// <param name="tm">The team that has the penalty.</param>
    /// <param name="lastPenalty">The player that shot the last penalty.</param>
    /// <returns>A player id.</returns>
    public static int GetPenaltyTaker(Team tm, int lastPenalty)
    {
        int returnValue = -1;
        List<Player> players = new List<Player>();

        for (int i = 0; i < 11; i++)
            if (tm.Players[i].Cskill != 0)
                players.Add(tm.Players[i]);

        players.Sort((pl1, pl2) => Player.CompareFunc(pl1, pl2, (int)PlayerCompareAttrib.GameSkill));

        if (lastPenalty == -1 ||
            lastPenalty == players[players.Count - 1].Id)
            returnValue = players[0].Id;

        for (int i = 0; i < players.Count - 1; i++)
            if (lastPenalty == players[i].Id)
            {
                returnValue = players[i + 1].Id;
                break;
            }

        return returnValue;
    }

    /// <summary>
    /// Do some things that have to be done at the beginning of a
    /// game, e.g. calculate attendance etc.
    /// Corresponds to C: game_initialize.
    /// </summary>
    /// <param name="fix">The fixture we examine.</param>
    public static void Initialize(Fixture fix)
    {
        int[] userIdx = { Team.IsUser(fix.Teams[0]), Team.IsUser(fix.Teams[1]) };

        if (fix.HomeAdvantage)
            AssignAttendance(fix);
        else
            AssignAttendanceNeutral(fix);

        if (OptionHelper.SettInt("int_opt_disable_finances") == 0)
            Finance.AssignGameMoney(fix);

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < fix.Teams[i].Players.Count; j++)
            {
                Player player = fix.Teams[i].Players[j];

                if (j < 11)
                {
                    if (player.Cskill > 0)
                    {
                        Player.GamesGoalsSet(player, fix.Clid, (int)PlayerValue.Games, 1);
                        player.Career[(int)PlayerValue.Games]++;
                        player.Participation = true;

                        if (Player.QueryIsYouth(player))
                            player.Lsu += OptionHelper.ConstFloat("float_youth_lsu_addition_match");

                        Player.StreakAddToProb(player, OptionHelper.ConstFloat("float_player_streak_add_startup"));
                    }
                }
                else
                {
                    Player.StreakAddToProb(player, OptionHelper.ConstFloat("float_player_streak_add_no_startup"));
                }
            }

            if (userIdx[i] != -1)
                Variables.Users[userIdx[i]].Counters[(int)CounterType.ShowResult] = 1;
        }
    }

    /// <summary>
    /// Save the team states in the current live game
    /// when a pause occurs.
    /// Corresponds to C: game_save_team_states.
    /// </summary>
    public static void SaveTeamStates()
    {
        // Implementation depends on the not yet converted user and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check whether the number of substitutions
    /// during a live game pause isn't too high.
    /// @return TRUE if the subs were ok, FALSE else.
    /// </summary>
    public static bool CheckLiveGameResumeState()
    {
        // Implementation depends on the not yet converted team and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find pairs of substituted players after a live game pause.
    /// Corresponds to C: game_get_subs.
    /// </summary>
    /// <param name="teamNumber">The team number.</param>
    /// <param name="subsIn">The substitute players that went in.</param>
    /// <param name="subsOut">The substitute players that went out.</param>
    public static void GetSubs(int teamNumber, int[] subsIn, int[] subsOut)
    {
        // Implementation depends on the not yet converted user and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Substitute a player during a match.
    /// Corresponds to C: game_substitute_player.
    /// </summary>
    /// <param name="tm">The team we work on.</param>
    /// <param name="playerNumber">The index of the player.</param>
    /// <returns>The substitute player id.</returns>
    public static int SubstitutePlayer(Team tm, int playerNumber)
    {
        // Implementation depends on the not yet converted player, team and
        // finance functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Choose an injury for a player and adjust health values.
    /// Corresponds to C: game_player_injury.
    /// </summary>
    /// <param name="pl">The player that gets injured.</param>
    public static void PlayerInjury(Player pl)
    {
        // Implementation depends on the not yet converted maths and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate the probability of a foul event occurring.
    /// Corresponds to C: game_get_foul_prob.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The live game unit.</param>
    /// <returns>The foul probability.</returns>
    public static float GetFoulProb(LiveGame liveGame, LiveGameUnit unit)
    {
        float prob;
        int i;

        // Base probability (a linear function of match time).
        prob = Variables.Constants.GetFloat("float_live_game_foul_base") +
            Variables.Constants.GetFloat("float_live_game_foul_max_inc") *
            Math.Min(1, (float)(unit.Minute) / 90);

        // Add possible boost influence of the team not in possession.
        Team teamNotInPossession = unit.Possession == 0 ? liveGame.Fix.Teams[1] : liveGame.Fix.Teams[0];
        prob *= 1 + teamNotInPossession.Boost * Variables.Constants.GetFloat("float_team_boost_foul_factor");

        // Reduce probability depending on the current cards of
        // the team not in possession, except if their boost is on.
        if (teamNotInPossession.Boost != 1)
        {
            for (i = 0; i < teamNotInPossession.Players.Count; i++)
            {
                Player player = teamNotInPossession.Players[i];
                if (player.CardStatus == PlayerCardStatus.Yellow)
                    prob *= 1 - Variables.Constants.GetFloat("float_live_game_foul_prob_reduction_yellow");
                else if (player.CardStatus == PlayerCardStatus.Red)
                    prob *= 1 - Variables.Constants.GetFloat("float_live_game_foul_prob_reduction_red");
            }
        }

        return prob;
    }

    /// <summary>
    /// Return a factor influencing who's fouled whom
    /// depending on the states of the team boosts.
    /// Corresponds to C: game_get_foul_possession_factor.
    /// </summary>
    /// <param name="boost1">Boost of the team in possession.</param>
    /// <param name="boost2">Boost of the team not in possession.</param>
    /// <returns>A factor.</returns>
    public static float GetFoulPossessionFactor(int boost1, int boost2)
    {
        if (boost1 == boost2)
            return 1;

        if (Math.Abs(boost1 - boost2) == 1)
            return 1 + Variables.Constants.GetFloat("float_team_boost_foul_by_possession_factor1") *
                (1 - 2 * (boost1 < boost2 ? 1 : 0));

        return 1 + Variables.Constants.GetFloat("float_team_boost_foul_by_possession_factor2") *
            (1 - 2 * (boost1 < boost2 ? 1 : 0));
    }

    /// <summary>
    /// Find out whether we substitute a player to balance
    /// a team after a red card.
    /// Corresponds to C: game_find_to_substitute.
    /// </summary>
    /// <param name="clid">The cup/league id of the fixture.</param>
    /// <param name="tm">The team.</param>
    /// <returns>A player index or -1 if we don't substitute.</returns>
    public static int FindToSubstitute(int clid, Team tm)
    {
        // Implementation depends on the not yet converted player and sorting functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out how long a player is banned.
    /// Corresponds to C: game_player_get_ban_duration.
    /// </summary>
    /// <returns>A player id or -1.</returns>
    public static int GetPlayerBanDuration()
    {
        // Implementation depends on the not yet converted maths and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Make some team updates after a match.
    /// Corresponds to C: game_post_match.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void PostMatch(Fixture fix)
    {
        // Implementation depends on the not yet converted team and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Reduce stadium capacity and safety after a stadium event.
    /// Corresponds to C: game_stadium_event.
    /// </summary>
    /// <param name="stadium">The stadium.</param>
    /// <param name="type">The event type.</param>
    public static void StadiumEvent(Stadium stadium, int type)
    {
        float[,] reduceFactor = new float[3, 2];
        int eventType = type - (int)LiveGameEventType.StadiumBreakdown;

        reduceFactor[0, 0] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_breakdown_lower");
        reduceFactor[0, 1] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_breakdown_upper");
        reduceFactor[1, 0] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_riots_lower");
        reduceFactor[1, 1] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_riots_upper");
        reduceFactor[2, 0] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_fire_lower");
        reduceFactor[2, 1] = Variables.Constants.GetFloat("float_game_stadium_safety_reduce_fire_upper");

        float reduce = Misc.MathRndi((int)reduceFactor[eventType, 0], (int)reduceFactor[eventType, 1]);

        stadium.Safety *= 1 - reduce;
        stadium.Capacity = (int)(stadium.Capacity * (1 - reduce * Variables.Constants.GetFloat("float_game_stadium_capacity_reduce_factor")));
    }

    /// <summary>
    /// Return the maximum possible values for defence, midfield and attack.
    /// Used in the opponent preview.
    /// Corresponds to C: game_get_max_values.
    /// </summary>
    /// <param name="maxValues">The place to store the values.</param>
    public static void GetMaxValues(float[] maxValues)
    {
        // Implementation depends on the not yet converted game_get_player_contribution and
        // player_get_position_from_structure functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there's a default penalty shooter
    /// in the team.
    /// Corresponds to C: game_get_default_penalty_shooter.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>A player id or -1.</returns>
    public static int GetDefaultPenaltyShooter(Team tm)
    {
        // Implementation depends on the option and player functions that are yet to be converted.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Deduce some money for boost during a match.
    /// Corresponds to C: game_boost_cost.
    /// </summary>
    public static void BoostCost()
    {
        // Implementation depends on the not yet converted finance and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Decrease the players' fitness during a live game.
    /// Corresponds to C: game_decrease_fitness.
    /// </summary>
    /// <param name="fix">The match being played.</param>
    public static void DecreaseFitness(Fixture fix)
    {
        // Implementation depends on the not yet converted player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update the live game stats using the live game unit.
    /// Corresponds to C: game_update_stats.
    /// </summary>
    /// <param name="lg">The live game.</param>
    /// <param name="unit">The live game unit.</param>
    public static void UpdateStats(LiveGame lg, LiveGameUnit unit)
    {
        // Implementation depends on the not yet converted live game and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update the player array in the live game stats.
    /// Corresponds to C: game_update_stats_player.
    /// </summary>
    /// <param name="lg">The live game.</param>
    /// <param name="unit">The live game unit.</param>
    public static void UpdateStatsPlayer(LiveGame lg, LiveGameUnit unit)
    {
        // Implementation depends on the not yet converted live game and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign attendance for a match.
    /// Corresponds to C: game_assign_attendance.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void AssignAttendance(Fixture fix)
    {
        // Implementation depends on the not yet converted league and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign attendance for a match on neutral ground.
    /// Corresponds to C: game_assign_attendance_neutral.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void AssignAttendanceNeutral(Fixture fix)
    {
        // Implementation depends on the not yet converted league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Undo the player swaps the user made during a live game pause.
    /// Corresponds to C: game_reset_players.
    /// </summary>
    /// <param name="idx">The team index in the current live game fixture.</param>
    public static void ResetPlayers(int idx)
    {
        // Implementation depends on the not yet converted user, team and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Substitute a player during a match. (see game_substitute_player_send_off).
    /// </summary>
    /// <param name="clid">The id of the fixture.</param>
    /// <param name="tm">The team.</param>
    /// <param name="player">The player index.</param>
    /// <param name="toSubstitute">The return location for the index of the player to substitute.</param>
    /// <param name="substitute">The return location for the player who comes into the game.</param>
    public static void SubstitutePlayerSendOff(int clid, Team tm, int player, out int toSubstitute, out int substitute)
    {
        // Implementation depends on the not yet converted player, team and user functions.
        throw new NotImplementedException();
    }
}
