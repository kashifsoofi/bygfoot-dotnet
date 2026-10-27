namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Events happening during a live game. 
/// Corresponds to C enum LiveGameEventType.
/// </summary>
public enum LiveGameEventType
{
    General = 0,                // LIVE_GAME_EVENT_GENERAL
    StartMatch,                 // LIVE_GAME_EVENT_START_MATCH
    HalfTime,                   // LIVE_GAME_EVENT_HALF_TIME
    ExtraTime,                  // LIVE_GAME_EVENT_EXTRA_TIME
    EndMatch,                   // LIVE_GAME_EVENT_END_MATCH
    LostPossession,             // LIVE_GAME_EVENT_LOST_POSSESSION
    ScoringChance,              // LIVE_GAME_EVENT_SCORING_CHANCE
    Header,                     // LIVE_GAME_EVENT_HEADER
    Penalty,                    // LIVE_GAME_EVENT_PENALTY
    FreeKick,                   // LIVE_GAME_EVENT_FREE_KICK
    Goal,                       // LIVE_GAME_EVENT_GOAL
    OwnGoal,                    // LIVE_GAME_EVENT_OWN_GOAL
    Post,                       // LIVE_GAME_EVENT_POST
    Miss,                       // LIVE_GAME_EVENT_MISS
    Save,                       // LIVE_GAME_EVENT_SAVE
    KeeperPushedInCorner,       // LIVE_GAME_EVENT_KEEPER_PUSHED_IN_CORNER
    CrossBar,                   // LIVE_GAME_EVENT_CROSS_BAR
    PlayerPushedInCorner,       // LIVE_GAME_EVENT_PLAYER_PUSHED_IN_CORNER
    CornerKick,                 // LIVE_GAME_EVENT_CORNER_KICK
    Foul,                       // LIVE_GAME_EVENT_FOUL
    FoulYellow,                 // LIVE_GAME_EVENT_FOUL_YELLOW
    FoulRed,                    // LIVE_GAME_EVENT_FOUL_RED
    FoulRedInjury,              // LIVE_GAME_EVENT_FOUL_RED_INJURY
    SendOff,                    // LIVE_GAME_EVENT_SEND_OFF
    Injury,                     // LIVE_GAME_EVENT_INJURY
    TempInjury,                 // LIVE_GAME_EVENT_TEMP_INJURY
    Penalties,                  // LIVE_GAME_EVENT_PENALTY
    Stadium,                    // LIVE_GAME_EVENT_STADIUM
    StadiumBreakdown,           // LIVE_GAME_EVENT_STADIUM_BREAKDOWN
    StadiumRiots,               // LIVE_GAME_EVENT_STADIUM_RIOTS
    StadiumFire,                // LIVE_GAME_EVENT_STADIUM_FIRE
    Substitution,               // LIVE_GAME_EVENT_SUBSTITUTION
    StructureChange,            // LIVE_GAME_EVENT_STRUCTURE_CHANGE
    StyleChangeAllOutDefend,    // LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_DEFEND
    StyleChangeDefend,          // LIVE_GAME_EVENT_STYLE_CHANGE_DEFEND
    StyleChangeBalanced,        // LIVE_GAME_EVENT_STYLE_CHANGE_BALANCED
    StyleChangeAttack,          // LIVE_GAME_EVENT_STYLE_CHANGE_ATTACK
    StyleChangeAllOutAttack,    // LIVE_GAME_EVENT_STYLE_CHANGE_ALL_OUT_ATTACK
    BoostChangeAnti,            // LIVE_GAME_EVENT_BOOST_CHANGE_ANTI
    BoostChangeOff,             // LIVE_GAME_EVENT_BOOST_CHANGE_OFF
    BoostChangeOn,              // LIVE_GAME_EVENT_BOOST_CHANGE_ON
    End                         // LIVE_GAME_EVENT_END
}

/// <summary>
/// Area of the pitch. Corresponds to C enum LiveGameUnitArea.
/// </summary>
public enum LiveGameUnitArea
{
    Defend = 0,                 // LIVE_GAME_UNIT_AREA_DEFEND
    Midfield,                   // LIVE_GAME_UNIT_AREA_MIDFIELD
    Attack,                     // LIVE_GAME_UNIT_AREA_ATTACK
    End                         // LIVE_GAME_UNIT_AREA_END
}

/// <summary>
/// Time progression in the game. Corresponds to C enum LiveGameUnitTime.
/// </summary>
public enum LiveGameUnitTime
{
    FirstHalf = 0,              // LIVE_GAME_UNIT_TIME_FIRST_HALF
    SecondHalf,                 // LIVE_GAME_UNIT_TIME_SECOND_HALF
    ExtraTime,                  // LIVE_GAME_UNIT_TIME_EXTRA_TIME
    Penalties,                  // LIVE_GAME_UNIT_TIME_PENALTIES
    End                         // LIVE_GAME_UNIT_TIME_END
}

/// <summary>
/// Statistical categories for live games. Corresponds to C enum LiveGameStatValue.
/// </summary>
public enum LiveGameStatValue
{
    GoalsRegular = 0,           // LIVE_GAME_STAT_VALUE_GOALS_REGULAR
    Shots,                      // LIVE_GAME_STAT_VALUE_SHOTS
    ShotPercentage,             // LIVE_GAME_STAT_VALUE_SHOT_PERCENTAGE
    Possession,                 // LIVE_VOLUME_STAT_VALUE_POSSESSION
    Penalties,                  // LIVE_GAME_STAT_VALUE_PENALTIES
    Fouls,                      // LIVE_GAME_STAT_VALUE_FOULS
    Cards,                      // LIVE_GAME_STAT_VALUE_CARDS
    Reds,                       // LIVE_GAME_STAT_VALUE_REDS
    Injuries,                   // LIVE_GAME_STAT_VALUE_INJURIES
    End                         // LIVE_GAME_STAT_VALUE_END
}

/// <summary>
/// Detailed tracking categories for live games. Corresponds to C enum LiveGameStatArray.
/// </summary>
public enum LiveGameStatArray
{
    ScorersForDisplay = 0,      // LIVE_GAME_STAT_ARRAY_SCORERS_FOR_DISPLAY
    Scorers,                    // LIVE_GAME_STAT_ARRAY_SCORERS
    Yellows,                    // LIVE_GAME_STAT_ARRAY_YELLOWS
    Reds,                       // LIVE_GAME_STAT_ARRAY_REDS
    Injured,                    // LIVE_GAME_STAT_ARRAY_INJURED
    End                         // LIVE_GAME_STAT_ARRAY_END
}

/// <summary>
/// Team value categories. Corresponds to C enum GameTeamValue.
/// </summary>
public enum GameTeamValue
{
    Goalie = 0,                 // GAME_TEAM_VALUE_GOALIE
    Defend,                     // GAME_TEAM_VALUE_DEFEND
    Midfield,                   // GAME_TEAM_VALUE_MIDFIELD
    Attack,                     // GAME_TEAM_VALUE_ATTACK
    End                         // GAME_TEAM_VALUE_END
}

/// <summary>
/// Statistics for a live game. Corresponds to C struct LiveGameStats.
/// </summary>
public class LiveGameStats
{
    public float Possession { get; set; }
    public int[,] Values { get; set; } = new int[2, 10]; // [team][stat_type]
    public List<object>[,] Players { get; set; } = new List<object>[2, 5]; // [team][stat_array]
}

/// <summary>
/// An event occurring during a live game. Corresponds to C struct LiveGameEvent.
/// </summary>
public class LiveGameEvent
{
    public int Type { get; set; }
    public int Verbosity { get; set; }
    public int Team { get; set; }
    public int Player { get; set; }
    public int Player2 { get; set; }
    public string Commentary { get; set; }
    public int CommentaryId { get; set; }
}

/// <summary>
/// A fraction of a live game. Corresponds to C struct LiveGameUnit.
/// </summary>
public class LiveGameUnit
{
    public int Possession { get; set; }
    public int Area { get; set; }
    public int Minute { get; set; }
    public int Time { get; set; }
    public int[] Result { get; set; } = new int[2];
    public LiveGameEvent Event { get; set; }
}

/// <summary>
/// Team settings during a live game pause. Corresponds to C struct LiveGameTeamState.
/// </summary>
public class LiveGameTeamState
{
    public int Structure { get; set; }
    public int Style { get; set; }
    public bool Boost { get; set; }
    public int[] PlayerIds { get; set; } = new int[11];
}

/// <summary>
/// The complete record of a match simulation. Corresponds to C struct LiveGame.
/// </summary>
public class LiveGame
{
    public Fixture Fix { get; set; }
    public int FixId { get; set; }
    public string[] TeamNames { get; set; } = new string[2];
    public int Attendance { get; set; }
    public int[] SubsLeft { get; set; } = new int[2];
    public int StartedGame { get; set; }
    public int StadiumEvent { get; set; }
    public int[] TeamValues { get; set; } = new int[8]; // [team][value]
    public float HomeAdvantage { get; set; }
    public List<LiveGameUnit> Units { get; set; } = new List<LiveGameUnit>();
    public LiveGameStats Stats { get; set; } = new LiveGameStats();
    public List<LiveGameTeamState> TeamState { get; set; } = new List<LiveGameTeamState>();
    public List<List<int>>[] ActionIds { get; set; } = new List<List<int>>[2];
    public List<string>[] Waivers { get; set; } = new List<string>[2]; // For completeness

    /// <summary>
    /// Calculate the result of a fixture using the live game variable.
    /// Corresponds to C: live_game_calculate_fixture.
    /// </summary>
    /// <param name="fix">The fixture we calculate.</param>
    /// <param name="liveGame">The live game used for calculation.</param>
    public static void CalculateFixture(Fixture fix, LiveGame liveGame)
    {
        // Implementation depends on the not yet converted game and lg_commentary functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initialize a few things at the beginning of a live game.
    /// Corresponds to C: live_game_initialize.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="liveGame">The live game.</param>
    public static void Initialize(Fixture fix, LiveGame liveGame)
    {
        // Implementation depends on the not yet converted user, window, game and lg_commentary functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there should be a half-time break or extra time break or so.
    /// Corresponds to C: query_live_game_event_is_break.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="minute">The minute of the live game unit.</param>
    /// <param name="time">The live game unit time.</param>
    /// <returns>Whether there is a break.</returns>
    public static bool QueryLiveGameEventIsBreak(LiveGame liveGame, int minute, int time)
    {
        if (time == (int)LiveGameUnitTime.ExtraTime)
            return minute >= 120;

        if (time == (int)LiveGameUnitTime.Penalties)
            return QueryLiveGamePenaltiesOver(liveGame);

        float rndom = Misc.MathRndi(0, 1);

        if (time == (int)LiveGameUnitTime.FirstHalf)
            return minute >= 45 && rndom > (float)Math.Pow(Variables.Constants.GetFloat("float_live_game_break_base"), (float)(minute - 44) * Variables.Constants.GetFloat("float_live_game_45_break_exponent_factor"));

        return minute >= 90 && rndom > (float)Math.Pow(Variables.Constants.GetFloat("float_live_game_break_base"), (float)(minute - 89) * Variables.Constants.GetFloat("float_live_game_90_break_exponent_factor"));
    }

    /// <summary>
    /// Find out whether the final result of the penalties is already reached.
    /// Corresponds to C: query_live_game_penalties_over.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>Whether the penalties are over.</returns>
    public static bool QueryLiveGamePenaltiesOver(LiveGame liveGame)
    {
        List<LiveGameUnit> units = liveGame.Units;
        int[] penAttempted = { 0, 0 };

        for (int i = units.Count - 1; i > 0; i--)
        {
            if (units[i].Time == (int)LiveGameUnitTime.Penalties)
            {
                if (units[i].Event.Type == (int)LiveGameEventType.Penalty)
                    penAttempted[units[i].Possession]++;
            }
            else
                break;
        }

        if (penAttempted[0] + penAttempted[1] >= 10)
            return liveGame.Fix.Result[0, 2] != liveGame.Fix.Result[1, 2] && penAttempted[0] == penAttempted[1];

        return liveGame.Fix.Result[0, 2] - liveGame.Fix.Result[1, 2] > 5 - penAttempted[1] ||
            liveGame.Fix.Result[1, 2] - liveGame.Fix.Result[0, 2] > 5 - penAttempted[0];
    }

    /// <summary>
    /// Return the break event of the live game.
    /// Corresponds to C: live_game_get_break.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>The break event type.</returns>
    public static int GetBreak(LiveGame liveGame)
    {
        // Implementation depends on the not yet converted fixture and live game functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the time for the unit depending of time and event of the last one.
    /// Corresponds to C: live_game_get_time.
    /// </summary>
    /// <param name="unit">The unit before the one we create.</param>
    /// <returns>A live game unit time.</returns>
    public static int GetTime(LiveGameUnit unit)
    {
        if (unit.Event.Type == (int)LiveGameEventType.HalfTime)
            return (int)LiveGameUnitTime.SecondHalf;
        else if (unit.Event.Type == (int)LiveGameEventType.ExtraTime)
            return (int)LiveGameUnitTime.ExtraTime;
        else if (unit.Event.Type == (int)LiveGameEventType.Penalties)
            return (int)LiveGameUnitTime.Penalties;
        else
            return unit.Time;
    }

    /// <summary>
    /// Return the minute for the next game unit.
    /// Corresponds to C: live_game_get_minute.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>A new minute for a live game unit.</returns>
    public static int GetMinute(LiveGame liveGame)
    {
        List<LiveGameUnit> units = liveGame.Units;
        LiveGameUnit lastUnit = GetLastUnit(liveGame);

        if (lastUnit == null)
            return -1;

        if (lastUnit.Event.Type == (int)LiveGameEventType.HalfTime)
            return 46;
        else if (lastUnit.Event.Type == (int)LiveGameEventType.ExtraTime)
            return 91;
        else if (lastUnit.Event.Type == (int)LiveGameEventType.Penalties || lastUnit.Time == (int)LiveGameUnitTime.Penalties)
            return 120;

        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units[i].Minute != -1)
                return units[i].Minute + 1;
        }

        return -1;
    }

    /// <summary>
    /// Return the minutes remaining in the match.
    /// Corresponds to C: live_game_get_minutes_remaining.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The unit.</param>
    /// <returns>The minutes remaining.</returns>
    public static int GetMinutesRemaining(LiveGame liveGame, LiveGameUnit unit)
    {
        int currentMin = UnitGetMinute(liveGame, unit);
        int returnValue = -1;

        if (unit.Time == (int)LiveGameUnitTime.ExtraTime)
            returnValue = 120 - currentMin;
        else if (unit.Time == (int)LiveGameUnitTime.SecondHalf)
            returnValue = 90 - currentMin;
        else if (unit.Time == (int)LiveGameUnitTime.FirstHalf)
            returnValue = 45 - currentMin;

        return returnValue;
    }

    /// <summary>
    /// Return the minute of the unit (ie. look up the last unit with a 'normal' minute value if minute = -1).
    /// Corresponds to C: live_game_unit_get_minute.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The unit we examine.</param>
    /// <returns>A minute between 1 and 120.</returns>
    public static int UnitGetMinute(LiveGame liveGame, LiveGameUnit unit)
    {
        int idx = -1;

        for (int i = 0; i < liveGame.Units.Count; i++)
        {
            if (ReferenceEquals(liveGame.Units[i], unit))
            {
                idx = i;
                break;
            }
        }

        if (idx == -1)
            throw new Exception("live_game_unit_get_minute: reached end of units array.");

        for (int j = idx; j >= 0; j--)
        {
            if (liveGame.Units[j].Minute != -1)
                return liveGame.Units[j].Minute;
        }

        return -1;
    }

    /// <summary>
    /// Calculate which area the ball is going to be in in the next unit.
    /// Corresponds to C: live_game_get_area.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The previous unit.</param>
    /// <returns>An area, defend, midfield or attack.</returns>
    public static int GetArea(LiveGame liveGame, LiveGameUnit unit)
    {
        int newArea = unit.Area;
        float rndom = Misc.MathRndi(0, 1);
        float probs = (float)Math.Pow(Variables.Constants.GetFloat("float_live_game_area_def_mid") * PitTeams(liveGame, unit, Variables.Constants.GetFloat("float_live_game_area_def_mid_team_exponent")), 1);
        float prob1 = Variables.Constants.GetFloat("float_live_game_area_def_mid") * PitTeams(liveGame, unit, Variables.Constants.GetFloat("float_live_game_area_def_mid_team_exponent"));
        float prob2 = Variables.Constants.GetFloat("float_live_game_area_mid_att") * PitTeams(liveGame, unit, Variables.Constants.GetFloat("float_live_game_area_mid_att_team_exponent"));
        float prob3 = Variables.Constants.GetFloat("float_live_game_area_mid_def") / PitTeams(liveGame, unit, Variables.Constants.GetFloat("float_live_game_area_mid_team_exponent"));
        float prob4 = Variables.Constants.GetFloat("float_live_game_area_att_mid") / PitTeams(liveGame, unit, Variables.Constants.GetFloat("float_live_game_area_att_team_exponent"));

        if (unit.Area == (int)LiveGameUnitArea.Defend && rndom < prob1)
            newArea = (int)LiveGameUnitArea.Midfield;
        else if (unit.Area == (int)LiveGameUnitArea.Midfield)
        {
            if (rndom < prob2)
                newArea = (int)LiveGameUnitArea.Attack;
            else if (rndom < prob2 + prob3)
                newArea = (int)LiveGameUnitArea.Defend;
        }
        else if (rndom < prob4)
            newArea = (int)LiveGameUnitArea.Midfield;

        return newArea;
    }

    /// <summary>
    /// Return the team values factor weighted with the given exponent and depending on the pitch area.
    /// Corresponds to C: live_game_pit_teams.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The unit we calculate the value for.</param>
    /// <param name="exponent">The weighting exponent.</param>
    /// <returns>The team values factor.</returns>
    public static float PitTeams(LiveGame liveGame, LiveGameUnit unit, float exponent)
    {
        float factor;

        float luck0 = liveGame.Fix.Teams[unit.Possession].Luck;
        float luck1 = liveGame.Fix.Teams[1 - unit.Possession].Luck;
        float ownValue = (float)liveGame.TeamValues[unit.Possession * 4 + (int)GameTeamValue.Defend] * luck0;
        float oppValue = (float)liveGame.TeamValues[(1 - unit.Possession) * 4 + (int)GameTeamValue.Attack] * luck1;
        float ownValueMid = (float)liveGame.TeamValues[unit.Possession * 4 + (int)GameTeamValue.Midfield] * luck0;
        float oppValueMid = (float)liveGame.TeamValues[(1 - unit.Possession) * 4 + (int)GameTeamValue.Midfield] * luck1;
        float ownValueAtt = (float)liveGame.TeamValues[unit.Possession * 4 + (int)GameTeamValue.Attack] * luck0;
        float oppValueAtt = (float)liveGame.TeamValues[(1 - unit.Possession) * 4 + (int)GameTeamValue.Defend] * luck1;

        if (unit.Area == (int)LiveGameUnitArea.Defend)
            factor = (float)Math.Pow(ownValue / oppValue, exponent);
        else if (unit.Area == (int)LiveGameUnitArea.Midfield)
            factor = (float)Math.Pow(ownValueMid / oppValueMid, exponent);
        else
            factor = (float)Math.Pow(ownValueAtt / oppValueAtt, exponent);

        return factor;
    }

    /// <summary>
    /// Write a result like '2:3 e.t.' into the buffer.
    /// Corresponds to C: live_game_unit_result_to_buf.
    /// </summary>
    /// <param name="unit">The unit.</param>
    /// <param name="buf">The buffer.</param>
    /// <param name="swap">Whether to swap the scores.</param>
    public static void UnitResultToBuf(LiveGameUnit unit, ref string buf, bool swap)
    {
        int idx = swap ? 1 : 0;
        int other = idx == 1 ? 1 : 0;

        if (unit.Time == (int)LiveGameUnitTime.Penalties)
            buf = $"{(int)unit.Result[idx]} : {(int)unit.Result[other]} p.";
        else if (unit.Time == (int)LiveGameUnitTime.ExtraTime)
            buf = $"{(int)unit.Result[idx]} : {(int)unit.Result[other]} e.t.";
        else
            buf = $"{(int)unit.Result[idx]} : {(int)unit.Result[other]}";
    }

    /// <summary>
    /// Count the number of penalties taken during penalty shoot-out.
    /// Corresponds to C: live_game_penalties_taken.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>The number of penalties taken.</returns>
    public static int PenaltiesTaken(LiveGame liveGame)
    {
        List<LiveGameUnit> units = liveGame.Units;
        int penalties = 0;

        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units[i].Event.Type == (int)LiveGameEventType.Penalties)
                break;
            else if (units[i].Time == (int)LiveGameUnitTime.Penalties && units[i].Event.Type == (int)LiveGameEventType.Penalty)
                penalties++;
        }

        return penalties;
    }

    /// <summary>
    /// Find out whether the specified player already has a yellow card in this game.
    /// Corresponds to C: query_live_game_second_yellow.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="team">The team index, 0 or 1.</param>
    /// <param name="player">The player index.</param>
    /// <returns>Whether there is a second yellow.</returns>
    public static bool QueryLiveGameSecondYellow(LiveGame liveGame, int team, int player)
    {
        List<LiveGameUnit> units = liveGame.Units;

        for (int i = 0; i < units.Count - 1; i++)
        {
            if (units[i].Event.Type == (int)LiveGameEventType.FoulYellow &&
                units[i].Possession != team &&
                units[i].Event.Player2 == player)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Return the last unit of the live game.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>The last unit or null.</returns>
    private static LiveGameUnit GetLastUnit(LiveGame liveGame)
    {
        if (liveGame?.Units == null || liveGame.Units.Count == 0)
            return null;

        return liveGame.Units[liveGame.Units.Count - 1];
    }
}
