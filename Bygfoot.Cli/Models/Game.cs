namespace Bygfoot.Cli.Models;

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
        // Implementation depends on the not yet converted player_get_game_skill and
        // game_get_player_contribution functions.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted player_get_game_skill function.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted game_get_player_probs and
        // game_get_penalty_taker functions.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted player_get_game_skill function.
        throw new NotImplementedException();
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
        // Implementation depends on the player and sorting functions that are yet to be converted.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Do some things that have to be done at the beginning of a
    /// game, e.g. calculate attendance etc.
    /// Corresponds to C: game_initialize.
    /// </summary>
    /// <param name="fix">The fixture we examine.</param>
    public static void Initialize(Fixture fix)
    {
        // Implementation depends on the not yet converted finance and player functions.
        throw new NotImplementedException();
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
