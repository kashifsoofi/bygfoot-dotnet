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
        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;
        Team[] teams = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };

        for (int i = 0; i < 2; i++)
        {
            LiveGameTeamState teamState = liveGame.TeamState[i];
            teamState.Structure = teams[i].Structure;
            teamState.Style = teams[i].Style;
            teamState.Boost = teams[i].Boost != 0;

            for (int j = 0; j < 11; j++)
                teamState.PlayerIds[j] = teams[i].Players[j].Id;
        }
    }

    /// <summary>
    /// Check whether the number of substitutions
    /// during a live game pause isn't too high.
    /// @return TRUE if the subs were ok, FALSE else.
    /// </summary>
    public static bool CheckLiveGameResumeState()
    {
        int[] subs = { 0, 0 };
        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;
        Team[] teams = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        bool returnValue = true;

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 11; j++)
                if (!Misc.QueryIntegerIsInArray(teams[i].Players[j].Id, liveGame.TeamState[i].PlayerIds, 11))
                    subs[i]++;

            if (subs[i] > liveGame.SubsLeft[i])
            {
                ResetPlayers(i);
                returnValue = false;
            }
        }

        return returnValue;
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
        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;
        Team tm = liveGame.Fix.Teams[teamNumber];
        int[] savedIds = liveGame.TeamState[teamNumber].PlayerIds;
        int[] currentPlayers = new int[11];
        int cnt = 0;

        for (int i = 0; i < 3; i++)
            subsIn[i] = subsOut[i] = -1;

        for (int i = 0; i < 11; i++)
        {
            currentPlayers[i] = tm.Players[i].Id;
            if (!Misc.QueryIntegerIsInArray(tm.Players[i].Id, savedIds, 11))
            {
                subsIn[cnt] = tm.Players[i].Id;
                cnt++;
            }
        }

        cnt = 0;
        for (int i = 0; i < 11; i++)
            if (!Misc.QueryIntegerIsInArray(savedIds[i], currentPlayers, 11))
            {
                subsOut[cnt] = savedIds[i];
                cnt++;
            }
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
        int substitute = -1;
        List<Player> substitutes = new List<Player>();
        bool adaptStructure;

        for (int i = 11; i < tm.Players.Count; i++)
            if (tm.Players[i].Cskill > 0)
                substitutes.Add(tm.Players[i]);

        if (substitutes.Count == 0)
        {
            Debug.DebugPrintMessage("game_substitute_player: no suitable substitutes found (all injured/banned?)");
            return -1;
        }

        int oldPos = (int)tm.Players[playerNumber].Cpos;
        substitutes.Sort((a, b) => Player.CompareSubstituteFunc(a, b, oldPos));

        adaptStructure =
            (MathHelper.GetPlace(Team.FindAppropriateStructure(tm), 1) +
             MathHelper.GetPlace(Team.FindAppropriateStructure(tm), 2) +
             MathHelper.GetPlace(Team.FindAppropriateStructure(tm), 3) != 10 ||
             (tm.Players[playerNumber].Cpos != substitutes[0].Pos &&
              Player.SubstitutionGoodStructure(tm.Structure, (int)tm.Players[playerNumber].Cpos, (int)substitutes[0].Pos)));

        substitute = substitutes[0].Id;

        Player.Swap(tm, playerNumber, tm, Player.IdIndex(tm, substitute, true));

        if (adaptStructure)
        {
            Team.ChangeStructure(tm, Team.FindAppropriateStructure(tm));
            Team.Rearrange(tm);
        }

        // The C version also refreshes the user player list for the current user
        // (game_gui_write_av_skills, selected_row = -1, treeview_show_user_player_list),
        // which are GUI-only and not converted.

        return substitute;
    }

    /// <summary>
    /// Choose an injury for a player and adjust health values.
    /// Corresponds to C: game_player_injury.
    /// </summary>
    /// <param name="pl">The player that gets injured.</param>
    public static void PlayerInjury(Player pl)
    {
        int i;
        float rndom;
        float[] injuryProbs =
        {
            0,
            OptionHelper.ConstFloat("float_player_injury_concussion"),
            OptionHelper.ConstFloat("float_player_injury_pulled_muscle"),
            OptionHelper.ConstFloat("float_player_injury_hamstring"),
            OptionHelper.ConstFloat("float_player_injury_groin"),
            OptionHelper.ConstFloat("float_player_injury_frac_ankle"),
            OptionHelper.ConstFloat("float_player_injury_rib"),
            OptionHelper.ConstFloat("float_player_injury_leg"),
            OptionHelper.ConstFloat("float_player_injury_brok_ankle"),
            OptionHelper.ConstFloat("float_player_injury_arm"),
            OptionHelper.ConstFloat("float_player_injury_shoulder"),
            OptionHelper.ConstFloat("float_player_injury_ligament"),
            OptionHelper.ConstFloat("float_player_injury_career_stop")
        };

        int[] duration =
        {
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_concussion") - OptionHelper.ConstInt("int_player_injury_duration_dev_concussion"),
                OptionHelper.ConstInt("int_player_injury_duration_concussion") + OptionHelper.ConstInt("int_player_injury_duration_dev_concussion")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_pulled_muscle") - OptionHelper.ConstInt("int_player_injury_duration_dev_pulled_muscle"),
                OptionHelper.ConstInt("int_player_injury_duration_pulled_muscle") + OptionHelper.ConstInt("int_player_injury_duration_dev_pulled_muscle")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_hamstring") - OptionHelper.ConstInt("int_player_injury_duration_dev_hamstring"),
                OptionHelper.ConstInt("int_player_injury_duration_hamstring") + OptionHelper.ConstInt("int_player_injury_duration_dev_hamstring")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_groin") - OptionHelper.ConstInt("int_player_injury_duration_dev_groin"),
                OptionHelper.ConstInt("int_player_injury_duration_groin") + OptionHelper.ConstInt("int_player_injury_duration_dev_groin")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_frac_ankle") - OptionHelper.ConstInt("int_player_injury_duration_dev_frac_ankle"),
                OptionHelper.ConstInt("int_player_injury_duration_frac_ankle") + OptionHelper.ConstInt("int_player_injury_duration_dev_frac_ankle")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_rib") - OptionHelper.ConstInt("int_player_injury_duration_dev_rib"),
                OptionHelper.ConstInt("int_player_injury_duration_rib") + OptionHelper.ConstInt("int_player_injury_duration_dev_rib")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_leg") - OptionHelper.ConstInt("int_player_injury_duration_dev_leg"),
                OptionHelper.ConstInt("int_player_injury_duration_leg") + OptionHelper.ConstInt("int_player_injury_duration_dev_leg")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_brok_ankle") - OptionHelper.ConstInt("int_player_injury_duration_dev_brok_ankle"),
                OptionHelper.ConstInt("int_player_injury_duration_brok_ankle") + OptionHelper.ConstInt("int_player_injury_duration_dev_brok_ankle")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_arm") - OptionHelper.ConstInt("int_player_injury_duration_dev_arm"),
                OptionHelper.ConstInt("int_player_injury_duration_arm") + OptionHelper.ConstInt("int_player_injury_duration_dev_arm")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_shoulder") - OptionHelper.ConstInt("int_player_injury_duration_dev_shoulder"),
                OptionHelper.ConstInt("int_player_injury_duration_shoulder") + OptionHelper.ConstInt("int_player_injury_duration_dev_shoulder")),
            MathHelper.GaussDisti(
                OptionHelper.ConstInt("int_player_injury_duration_ligament") - OptionHelper.ConstInt("int_player_injury_duration_dev_ligament"),
                OptionHelper.ConstInt("int_player_injury_duration_ligament") + OptionHelper.ConstInt("int_player_injury_duration_dev_ligament")),
            50
        };

        for (i = 1; i < 13; i++)
            injuryProbs[i] += injuryProbs[i - 1];

        rndom = (float)MathHelper.Rnd(0, 1);

        for (i = 1; i < 13; i++)
            if (rndom >= injuryProbs[i - 1] && rndom < injuryProbs[i])
            {
                pl.Health = (global::Bygfoot.Cli.PlayerInjury)i;
                pl.Recovery = duration[i - 1] + 1;
                pl.Cskill = pl.Fitness = 0;
            }

        if (pl.Health == global::Bygfoot.Cli.PlayerInjury.CareerStop && Team.IsUser(pl.Team) != -1)
            User.EventAdd(Variables.Users[Team.IsUser(pl.Team)], (int)EventType.PlayerCareerStop, pl.Id, -1, null, null);

        Player.StreakAddToProb(pl, OptionHelper.ConstFloat("float_player_streak_add_injury"));
        if (pl.Streak == PlayerStreak.Hot)
        {
            pl.Streak = PlayerStreak.None;
            Player.StreakResetCount(pl);
        }
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
        int positionToSubstitute = -1;
        List<Player> players = new List<Player>();
        int returnValue = -1;
        int currentStructure = Team.FindAppropriateStructure(tm);
        int numForw = currentStructure % 10;
        int numMid = MathHelper.GetPlace(currentStructure, 2);
        int numDef = MathHelper.GetPlace(currentStructure, 3);

        for (int i = 0; i < 11; i++)
            if (Player.IsBanned(tm.Players[i]) <= 0)
                players.Add(tm.Players[i]);

        players.Sort((a, b) => Player.CompareFunc(a, b, (int)PlayerCompareAttrib.GameSkill));

        if (numForw > 1 || Math.Max(numMid, numDef) <= 2)
            positionToSubstitute = (int)PlayerPos.Forward;
        else if (Math.Abs(numDef - numMid) > 1 ||
            (numForw == 0 && Math.Max(numMid, numDef) > 2))
            positionToSubstitute = numDef > numMid ? (int)PlayerPos.Defender : (int)PlayerPos.Midfielder;
        else
            return -1;

        for (int i = players.Count - 1; i >= 0; i--)
            if ((int)players[i].Pos == positionToSubstitute)
            {
                returnValue = players[i].Id;
                return returnValue;
            }

        return -1;
    }

    /// <summary>
    /// Find out how long a player is banned.
    /// Corresponds to C: game_player_get_ban_duration.
    /// </summary>
    /// <returns>A player id or -1.</returns>
    public static int GetPlayerBanDuration()
    {
        float[] durationProbs =
        {
            0,
            OptionHelper.ConstFloat("float_live_game_ban_1"),
            OptionHelper.ConstFloat("float_live_game_ban_2"),
            OptionHelper.ConstFloat("float_live_game_ban_3"),
            OptionHelper.ConstFloat("float_live_game_ban_4"),
            OptionHelper.ConstFloat("float_live_game_ban_5")
        };

        for (int i = 1; i < 6; i++)
            durationProbs[i] += durationProbs[i - 1];

        float rndom = (float)MathHelper.Rnd(0, 1);

        for (int i = 1; i < 6; i++)
            if (durationProbs[i - 1] < rndom && rndom < durationProbs[i])
                return i + 1;

        return -1;
    }

    /// <summary>
    /// Make some team updates after a match.
    /// Corresponds to C: game_post_match.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void PostMatch(Fixture fix)
    {
        int i;
        List<Team> teams = null;
        Cup cup = null;
        string buf = string.Empty;
        string buf2 = string.Empty;
        int usrIdx;

        usrIdx = Fixture.UserTeamInvolved(fix);

        if ((Variables.DebugLevel > 100 && usrIdx != -1) || Variables.DebugLevel > 130)
            Debug.DebugPrintMessage("game_post_match: {0} - {1}", fix.Teams[0].Name, fix.Teams[1].Name);

        if (Fixture.QueryFixtureHasTables(fix))
            Table.Update(fix);

        for (i = 0; i < 2; i++)
            Team.UpdatePostMatch(fix.Teams[i], fix);

        if (fix.Clid < Bygfoot.IdCupStart)
            return;

        cup = Cup.CupFromClid(fix.Clid);

        if (fix.Round == cup.Rounds.Count - 1 &&
            ReferenceEquals(fix, cup.Fixtures[cup.Fixtures.Count - 1]))
        {
            teams = Cup.CupGetTeamsSorted(cup);

            int winUserIdx = Team.IsUser(teams[0]);
            if (winUserIdx != -1)
            {
                User.HistoryAdd(Variables.Users[winUserIdx], (int)UserHistoryType.WinFinal,
                    teams[0].Name, League.LeagueCupGetNameString(fix.Clid), teams[1].Name, null);
                User.AddCupSuccess(Variables.Users[winUserIdx], cup, fix.Round, (int)UserHistoryType.WinFinal);
            }
            else
            {
                int loseUserIdx = Team.IsUser(teams[1]);
                if (loseUserIdx != -1)
                {
                    User.HistoryAdd(Variables.Users[loseUserIdx], (int)UserHistoryType.LoseFinal,
                        teams[1].Name, League.LeagueCupGetNameString(fix.Clid), teams[0].Name, null);
                    User.AddCupSuccess(Variables.Users[loseUserIdx], cup, fix.Round, (int)UserHistoryType.LoseFinal);
                }
            }
        }
        else if (usrIdx != -1)
        {
            Cup.CupGetRoundName(cup, fix.Round, ref buf);
            buf2 = (fix.Round + 1).ToString();

            User.HistoryAdd(Variables.Users[usrIdx], (int)UserHistoryType.ReachCupRound,
                Variables.Users[usrIdx].Team.Name, League.LeagueCupGetNameString(fix.Clid), buf, buf2);
            User.AddCupSuccess(Variables.Users[usrIdx], cup, fix.Round, (int)UserHistoryType.ReachCupRound);
        }
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
        int i;
        Player pl = new Player();

        pl.Skill = pl.Cskill = OptionHelper.ConstFloat("float_player_max_skill");
        pl.Fitness = 1;
        pl.Streak = PlayerStreak.None;

        maxValues[0] = OptionHelper.ConstFloat("float_player_max_skill");

        for (i = 1; i < 4; i++)
        {
            maxValues[i] = 0;
            for (int j = 1; j < 11; j++)
            {
                pl.Cpos = Player.GetPositionFromStructure(442, j);
                maxValues[i] += GetPlayerContribution(pl, i, false);
            }
        }
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
        int returnValue = -1;

        int usrIdx = Team.IsUser(tm);
        if (usrIdx == -1)
            return returnValue;

        int shooterId = Variables.Users[usrIdx].Options.GetInt("int_opt_user_penalty_shooter");
        if (shooterId == -1)
            return returnValue;

        int idx = Player.IdIndex(tm, shooterId, false);
        if (idx != -1 && idx < 11 && tm.Players[idx].Cskill != 0)
            returnValue = shooterId;

        return returnValue;
    }

    /// <summary>
    /// Deduce some money for boost during a match.
    /// Corresponds to C: game_boost_cost.
    /// </summary>
    public static void BoostCost()
    {
        float wageUnit = Finance.WageUnit(Variables.Users[Bygfoot.Stat2].Team);
        int deduce = (int)Math.Round(wageUnit * OptionHelper.ConstFloat("float_boost_cost_factor"));

        Variables.Users[Bygfoot.Stat2].Money -= deduce;
        Variables.Users[Bygfoot.Stat2].MoneyOut[1, (int)MonOut.MonOutBoost] -= deduce;
    }

    /// <summary>
    /// Decrease the players' fitness during a live game.
    /// Corresponds to C: game_decrease_fitness.
    /// </summary>
    /// <param name="fix">The match being played.</param>
    public static void DecreaseFitness(Fixture fix)
    {
        for (int i = 0; i < 2; i++)
        {
            if (Variables.DebugLevel < 50 || Team.IsUser(fix.Teams[i]) == -1)
                for (int j = 0; j < 11; j++)
                    if (fix.Teams[i].Players[j].Cskill > 0)
                        Player.DecreaseFitness(fix.Teams[i].Players[j]);
        }
    }

    /// <summary>
    /// Update the live game stats using the live game unit.
    /// Corresponds to C: game_update_stats.
    /// </summary>
    /// <param name="lg">The live game.</param>
    /// <param name="unit">The live game unit.</param>
    public static void UpdateStats(LiveGame lg, LiveGameUnit unit)
    {
        int i;
        LiveGameStats stats = lg.Stats;

        if (unit.Minute != -1)
            stats.Values[unit.Possession, (int)LiveGameStatValue.Possession]++;

        if (unit.Event.Type == (int)LiveGameEventType.ScoringChance ||
            unit.Event.Type == (int)LiveGameEventType.FreeKick ||
            unit.Event.Type == (int)LiveGameEventType.Header)
            stats.Values[unit.Possession, (int)LiveGameStatValue.Shots]++;
        else if (unit.Event.Type == (int)LiveGameEventType.Penalty)
            stats.Values[unit.Possession, (int)LiveGameStatValue.Penalties]++;
        else if (unit.Event.Type == (int)LiveGameEventType.Injury)
        {
            stats.Values[unit.Event.Team, (int)LiveGameStatValue.Injuries]++;
            UpdateStatsPlayer(lg, unit);
        }
        else if (unit.Event.Type == (int)LiveGameEventType.Foul ||
            unit.Event.Type == (int)LiveGameEventType.FoulYellow ||
            unit.Event.Type == (int)LiveGameEventType.FoulRed ||
            unit.Event.Type == (int)LiveGameEventType.FoulRedInjury)
        {
            stats.Values[unit.Event.Team, (int)LiveGameStatValue.Fouls]++;
            if (unit.Event.Type == (int)LiveGameEventType.FoulYellow)
            {
                stats.Values[unit.Event.Team, (int)LiveGameStatValue.Cards]++;
                UpdateStatsPlayer(lg, unit);
            }
        }
        else if (unit.Event.Type == (int)LiveGameEventType.SendOff)
        {
            stats.Values[unit.Event.Team, (int)LiveGameStatValue.Reds]++;
            UpdateStatsPlayer(lg, unit);
        }
        else if (unit.Event.Type == (int)LiveGameEventType.Goal ||
            unit.Event.Type == (int)LiveGameEventType.OwnGoal)
        {
            LiveGameUnit before = LiveGame.UnitBefore(lg, unit, -1);
            if ((before == null || before.Event.Type != (int)LiveGameEventType.Penalty) &&
                unit.Event.Type != (int)LiveGameEventType.OwnGoal)
                stats.Values[unit.Event.Team, (int)LiveGameStatValue.GoalsRegular]++;
            UpdateStatsPlayer(lg, unit);
        }

        for (i = 0; i < 2; i++)
            stats.Values[i, (int)LiveGameStatValue.ShotPercentage] =
                (stats.Values[i, (int)LiveGameStatValue.Shots] > 0) ?
                (int)Math.Round((double)stats.Values[i, (int)LiveGameStatValue.GoalsRegular] /
                (double)stats.Values[i, (int)LiveGameStatValue.Shots] * 100) : 0;
    }

    /// <summary>
    /// Update the player array in the live game stats.
    /// Corresponds to C: game_update_stats_player.
    /// </summary>
    /// <param name="lg">The live game.</param>
    /// <param name="unit">The live game unit.</param>
    public static void UpdateStatsPlayer(LiveGame lg, LiveGameUnit unit)
    {
        int i;
        string buf = string.Empty;
        string buf2 = string.Empty;
        LiveGameStats stats = lg.Stats;
        int minute = LiveGame.UnitGetMinute(lg, unit);
        int arrayIndex = -1;
        bool ownGoal;
        int team = unit.Event.Team;
        int player = unit.Event.Player;
        int player2 = unit.Event.Player2;
        Team[] tms = { lg.Fix.Teams[0], lg.Fix.Teams[1] };
        List<object> players = null;

        if (unit.Event.Type == (int)LiveGameEventType.Goal ||
            unit.Event.Type == (int)LiveGameEventType.OwnGoal)
        {
            ownGoal = unit.Event.Type == (int)LiveGameEventType.OwnGoal;
            arrayIndex = ownGoal ? 1 - team : team;

            LiveGameUnit before = LiveGame.UnitBefore(lg, unit, -1);
            if (before != null && before.Event.Type == (int)LiveGameEventType.Penalty)
                buf2 = " (P)";
            else if (before != null && before.Event.Type == (int)LiveGameEventType.FreeKick)
                buf2 = " (FK)";
            else if (unit.Event.Type == (int)LiveGameEventType.OwnGoal)
                buf2 = " (OG)";
            else
                buf2 = string.Empty;

            string playerName = PlayerOfIdTeam(tms[team], player).Name;
            if (!ownGoal)
                stats.Players[arrayIndex, (int)LiveGameStatArray.Scorers].Add(playerName);

            for (i = 0; i < stats.Players[arrayIndex, (int)LiveGameStatArray.ScorersForDisplay].Count; i++)
            {
                string existing = (string)stats.Players[arrayIndex, (int)LiveGameStatArray.ScorersForDisplay][i];
                if (existing != null && existing.StartsWith(playerName))
                {
                    buf = $"{existing} {minute}{buf2}";
                    stats.Players[arrayIndex, (int)LiveGameStatArray.ScorersForDisplay][i] = buf;
                    return;
                }
            }

            buf = $"{playerName} {minute}{buf2}";
            stats.Players[arrayIndex, (int)LiveGameStatArray.ScorersForDisplay].Add(buf);
        }
        else
        {
            if (unit.Event.Type == (int)LiveGameEventType.Injury)
            {
                buf = PlayerOfIdTeam(tms[team], player).Name;
                players = stats.Players[team, (int)LiveGameStatArray.Injured];
            }
            else if (unit.Event.Type == (int)LiveGameEventType.FoulYellow)
            {
                buf = PlayerOfIdTeam(tms[team], player2).Name;
                players = stats.Players[team, (int)LiveGameStatArray.Yellows];
            }
            else if (unit.Event.Type == (int)LiveGameEventType.SendOff)
            {
                buf = PlayerOfIdTeam(tms[team], player).Name;
                players = stats.Players[team, (int)LiveGameStatArray.Reds];
            }

            if (players != null && buf.Length > 0)
                players.Add(buf);
        }
    }

    /// <summary>
    /// Assign attendance for a match.
    /// Corresponds to C: game_assign_attendance.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void AssignAttendance(Fixture fix)
    {
        Team[] tm = { fix.Teams[0], fix.Teams[1] };
        float factor =
            (float)MathHelper.Rnd(OptionHelper.ConstFloat("float_game_stadium_attendance_percentage_lower"),
                                  OptionHelper.ConstFloat("float_game_stadium_attendance_percentage_upper")) *
            (float)Math.Pow(tm[0].Stadium.Safety, OptionHelper.ConstFloat("float_game_stadium_attendance_safety_exponent"));
        int maxAtt = Math.Min((int)Math.Round(League.LeagueCupAverageCapacity(tm[0].Clid) *
            OptionHelper.ConstFloat("float_game_stadium_attendance_average_exceed_factor") *
            (float)MathHelper.Rnd(0.9, 1.1)),
            tm[0].Stadium.Capacity);

        if (fix.Clid < Bygfoot.IdCupStart &&
            Team.GetLeagueRank(tm[1], fix.Clid) <
            (int)Math.Round(League.LeagueFromClid(fix.Clid).Teams.Count *
            OptionHelper.ConstFloat("float_game_stadium_attendance_rank_percentage")))
            factor *= OptionHelper.ConstFloat("float_game_stadium_attendance_rank_factor");

        if (fix.Clid >= Bygfoot.IdCupStart)
        {
            Cup cup = Cup.CupFromClid(fix.Clid);
            if (cup.Rounds.Count - fix.Round <= OptionHelper.ConstInt("int_game_stadium_attendance_cup_rounds_full_house"))
                factor = 1;
            else if (League.QueryLeagueCupHasProperty(fix.Clid, "national"))
                factor *= OptionHelper.ConstFloat("float_game_stadium_attendance_cup_national_factor");
            else
                factor *= OptionHelper.ConstFloat("float_game_stadium_attendance_cup_international_factor");
        }

        /* Calculate the difference in ticket prices has on attendance */
        float deltaPrice = tm[0].Stadium.TicketPrice - OptionHelper.ConstInt("int_team_stadium_ticket_price");
        float priceFactor = (float)MathHelper.GaussDist(0.92, 1.08) *
            1 / (1 + OptionHelper.ConstFloat("float_team_stadium_price_attendance_factor") * deltaPrice);

        factor = Math.Min(factor * priceFactor, 1);
        float attRegular = (float)(int)Math.Round((float)tm[0].Stadium.Capacity * factor);
        fix.Attendance = (int)Math.Min(Math.Min(attRegular, maxAtt * factor), (float)maxAtt);

        tm[0].Stadium.AverageAttendance =
            (int)Math.Round((float)(tm[0].Stadium.AverageAttendance * tm[0].Stadium.Games + fix.Attendance) /
            (float)(tm[0].Stadium.Games + 1));
        tm[0].Stadium.PossibleAttendance += tm[0].Stadium.Capacity;
        tm[0].Stadium.Games++;
    }

    /// <summary>
    /// Assign attendance for a match on neutral ground.
    /// Corresponds to C: game_assign_attendance_neutral.
    /// </summary>
    /// <param name="fix">The match we examine.</param>
    public static void AssignAttendanceNeutral(Fixture fix)
    {
        List<Team> teamsp = League.LeagueCupGetTeams(fix.Clid);
        float avAtt = (fix.Clid >= Bygfoot.IdCupStart &&
            League.QueryLeagueCupHasProperty(fix.Clid, "international") && teamsp.Count > 0) ?
            League.LeagueCupAverageCapacity(fix.Clid) :
            League.LeagueCupAverageCapacity(Bygfoot.GetLeague(0).Id);

        fix.Attendance = (int)Math.Round(avAtt *
            (float)MathHelper.Rnd(OptionHelper.ConstFloat("float_game_stadium_attendance_neutral_lower"),
                                 OptionHelper.ConstFloat("float_game_stadium_attendance_neutral_upper")));
    }

    /// <summary>
    /// Undo the player swaps the user made during a live game pause.
    /// Corresponds to C: game_reset_players.
    /// </summary>
    /// <param name="idx">The team index in the current live game fixture.</param>
    public static void ResetPlayers(int idx)
    {
        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;
        Team tm = liveGame.Fix.Teams[idx];
        int[] savedIds = liveGame.TeamState[idx].PlayerIds;
        List<Player> players = new List<Player>();

        for (int i = 0; i < 11; i++)
            players.Add(PlayerOfIdTeam(tm, savedIds[i]));

        for (int i = 0; i < tm.Players.Count; i++)
            if (!Misc.QueryIntegerIsInArray(tm.Players[i].Id, savedIds, 11))
                players.Add(tm.Players[i]);

        tm.Players = players;
        Team.ChangeStructure(tm, Team.FindAppropriateStructure(tm));
        Team.Rearrange(tm);

        // The C version also refreshes the user player list (treeview_show_user_player_list),
        // which is GUI-only and not converted.
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
        int position = -1;
        List<Player> substitutes = null;
        int currentStructure = Team.FindAppropriateStructure(tm);
        int numForw = currentStructure % 10;
        int numMid = MathHelper.GetPlace(currentStructure, 2);
        int numDef = MathHelper.GetPlace(currentStructure, 3);

        toSubstitute = -1;
        substitute = -1;

        toSubstitute = FindToSubstitute(clid, tm);

        if (toSubstitute == -1)
            return;

        substitutes = new List<Player>();
        for (int i = 11; i < tm.Players.Count; i++)
            if (tm.Players[i].Cskill > 0)
                substitutes.Add(tm.Players[i]);

        if (substitutes.Count == 0)
        {
            Debug.DebugPrintMessage("game_substitute_player_send_off: no suitable substitutes found (all injured/banned?)");
            toSubstitute = -1;
            return;
        }

        if (numForw == 0 && Math.Max(numDef, numMid) > 2)
            position = (int)PlayerPos.Forward;
        else
            position = numDef > numMid ? (int)PlayerPos.Midfielder : (int)PlayerPos.Defender;

        substitutes.Sort((a, b) => Player.CompareSubstituteFunc(a, b, position));

        substitute = substitutes[0].Id;
        Player.Swap(tm, Player.IdIndex(tm, toSubstitute, true), tm, Player.IdIndex(tm, substitute, true));

        Team.ChangeStructure(tm, Team.FindAppropriateStructure(tm));
        Team.Rearrange(tm);

        // The C version also refreshes the user player list for the current user
        // (game_gui_write_av_skills, selected_row = -1, treeview_show_user_player_list),
        // which are GUI-only and not converted.
    }

    /// <summary>
    /// Return the player of a team with the given id.
    /// Corresponds to C: player_of_id_team.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <param name="id">The player id.</param>
    /// <returns>The player with that id.</returns>
    private static Player PlayerOfIdTeam(Team tm, int id) =>
        tm.Players[Player.IdIndex(tm, id, true)];
}
