namespace Bygfoot.Cli;

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
    public List<LiveGameTeamState> TeamState { get; set; } = new List<LiveGameTeamState> { new LiveGameTeamState(), new LiveGameTeamState() };
    public List<int>[] ActionIds { get; set; } = new List<int>[2];
    public List<string>[] Waivers { get; set; } = new List<string>[2]; // For completeness

    /// <summary>
    /// Calculate the result of a fixture using the live game variable.
    /// Corresponds to C: live_game_calculate_fixture.
    /// </summary>
    /// <param name="fix">The fixture we calculate.</param>
    /// <param name="liveGame">The live game used for calculation.</param>
    public static void CalculateFixture(Fixture fix, LiveGame liveGame)
    {
        Variables.Statp = liveGame;

        if (Bygfoot.Stat0 != (int)Status0Value.StatusLiveGamePause &&
            Bygfoot.Stat0 != (int)Status0Value.StatusLiveGameChange)
            Initialize(fix, liveGame);
        else
            Bygfoot.Stat0 = (int)Status0Value.StatusShowLiveGame;

        Team tm0 = liveGame.Fix.Teams[0];
        Team tm1 = liveGame.Fix.Teams[1];

        UpdateTeamValues(liveGame);

        if ((Variables.DebugLevel > 80 && Bygfoot.Stat2 != -1) ||
            Variables.DebugLevel > 130)
            Debug.DebugPrintMessage("\n\nlive_game_calculate_fixture\n{0:0000} {1} {2} {3:0000}\n\n",
                tm0.Id, tm0.Name, tm1.Name, tm1.Id);

        do
        {
            CreateUnit(liveGame);
            EvaluateUnit(liveGame, liveGame.Units[liveGame.Units.Count - 1]);
        }
        while (liveGame.Units[liveGame.Units.Count - 1].Event.Type != (int)LiveGameEventType.EndMatch &&
            Bygfoot.Stat0 != (int)Status0Value.StatusLiveGamePause &&
            Bygfoot.Stat0 != (int)Status0Value.StatusLiveGameChange);

        if (liveGame.Units[liveGame.Units.Count - 1].Event.Type == (int)LiveGameEventType.EndMatch)
        {
            if (Bygfoot.Stat2 != -1 || Bygfoot.Stat5 < -1000)
                LgCommentary.FreeTokens();

            Game.PostMatch(fix);
            Bygfoot.Stat0 = (int)Status0Value.StatusNone;
        }
        else if (Bygfoot.Stat0 == (int)Status0Value.StatusLiveGameChange)
            Resume();
    }

    /// <summary>
    /// Reset the live game variable before we begin a new live game.
    /// Corresponds to C: live_game_reset.
    /// </summary>
    /// <param name="liveGame">The pointer to the live game.</param>
    /// <param name="fix">The fixture we'll show.</param>
    /// <param name="freeVariable">Whether or not to free the variable before resetting (a no-op in C#).</param>
    public static void Reset(LiveGame liveGame, Fixture fix, bool freeVariable)
    {
        if (freeVariable)
            Free.FreeLiveGame(liveGame);

        liveGame.StartedGame = -1;
        liveGame.Units = new List<LiveGameUnit>();
        liveGame.ActionIds[0] = new List<int>();
        liveGame.ActionIds[1] = new List<int>();

        for (int i = 0; i < (int)LiveGameStatArray.End; i++)
        {
            liveGame.Stats.Players[0, i] = new List<object>();
            liveGame.Stats.Players[1, i] = new List<object>();
        }
        liveGame.Stats.Possession = 0;
        for (int i = 0; i < (int)LiveGameStatValue.End; i++)
            liveGame.Stats.Values[0, i] = liveGame.Stats.Values[1, i] = 0;

        liveGame.Fix = fix;
        if (fix != null)
        {
            liveGame.FixId = fix.Id;
            liveGame.TeamNames[0] = fix.Teams[0].Name;
            liveGame.TeamNames[1] = fix.Teams[1].Name;
        }
        else
        {
            liveGame.FixId = -1;
            liveGame.Attendance = -1;
            liveGame.TeamNames[0] = null;
            liveGame.TeamNames[1] = null;
        }

        liveGame.SubsLeft[0] = 3;
        liveGame.SubsLeft[1] = 3;
        liveGame.StadiumEvent = -1;

        if (fix != null && fix.HomeAdvantage)
            liveGame.HomeAdvantage = (float)MathHelper.Rnd(
                OptionHelper.ConstFloat("float_game_home_advantage_lower"),
                OptionHelper.ConstFloat("float_game_home_advantage_upper"));
        else
            liveGame.HomeAdvantage = 0;
    }

    /// <summary>
    /// Initialize a few things at the beginning of a live game.
    /// Corresponds to C: live_game_initialize.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="liveGame">The live game.</param>
    public static void Initialize(Fixture fix, LiveGame liveGame)
    {
        Bygfoot.Stat2 = Fixture.UserTeamInvolved(fix);
        Variables.Statp = liveGame;
        show = Bygfoot.Stat2 != -1 &&
            Variables.Users[Bygfoot.Stat2].Options.GetInt("int_opt_user_show_live_game") != 0;

        Reset(liveGame, fix, Bygfoot.Stat2 != -1);

        if (show)
        {
            Variables.CurUser = Bygfoot.Stat2;
            // C: on_button_back_to_main_clicked(NULL, NULL); (GUI, not yet converted).

            if (Variables.Window.Live == null)
            {
                Window.WindowCreate(WindowsType.Live);
                // WindowCreate calls WindowLiveSetUp internally.
            }
            else
            {
                (Variables.Window.Live as Gtk.Window)?.Title =
                    League.LeagueCupGetNameString(liveGame.Fix.Clid);
                Window.WindowLiveSetUp();
            }

            // C: game_gui_live_game_show_opponent(); (src/game_gui.c not yet converted).
        }

        fix.LiveGame = liveGame;
        Game.Initialize(fix);
        liveGame.Attendance = fix.Attendance;

        if (Bygfoot.Stat2 != -1 || Bygfoot.Stat5 < -1000)
            LgCommentary.Initialize(fix);
    }

    /// <summary>
    /// Whether the events of the current live game are actually shown.
    /// Corresponds to the C global variable show in src/live_game.c.
    /// </summary>
    private static bool show;

    /// <summary>
    /// Compute the attacking/defending values of both teams and store
    /// them in the live game variable.
    /// Corresponds to the game_get_values call in live_game.c.
    /// </summary>
    private static void UpdateTeamValues(LiveGame liveGame)
    {
        float[,] teamValues = new float[2, (int)GameTeamValue.End];
        Game.GetValues(liveGame.Fix, teamValues, liveGame.HomeAdvantage);
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < (int)GameTeamValue.End; j++)
                liveGame.TeamValues[i * (int)GameTeamValue.End + j] = (int)teamValues[i, j];
    }

    /// <summary>
    /// Deep copy of a live game unit (the C original copies the struct by value).
    /// </summary>
    private static LiveGameUnit CopyUnit(LiveGameUnit unit)
    {
        return new LiveGameUnit
        {
            Possession = unit.Possession,
            Area = unit.Area,
            Minute = unit.Minute,
            Time = unit.Time,
            Result = new[] { unit.Result[0], unit.Result[1] },
            Event = new LiveGameEvent
            {
                Type = unit.Event.Type,
                Verbosity = unit.Event.Verbosity,
                Team = unit.Event.Team,
                Player = unit.Event.Player,
                Player2 = unit.Event.Player2,
                Commentary = unit.Event.Commentary,
                CommentaryId = unit.Event.CommentaryId
            }
        };
    }

    /// <summary>
    /// Stop the live game so that users can make subs etc.
    /// Corresponds to C: misc_callback_pause_live_game (src/misc_callback_func.c).
    /// </summary>
    private static void PauseLiveGame()
    {
        if (Bygfoot.Stat1 == (int)Status0Value.StatusShowLastMatch)
        {
            Bygfoot.Stat4 = (int)Status0Value.StatusShowLastMatchPause;
            return;
        }

        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;

        if (liveGame.Units[liveGame.Units.Count - 1].Event.Type == (int)LiveGameEventType.EndMatch)
            return;

        // C: gtk_widget_set_sensitive(lookup_widget(window.live, "button_pause"), FALSE);
        // C: gtk_widget_set_sensitive(lookup_widget(window.live, "button_resume"), TRUE);
        // C: gtk_widget_grab_focus(button_resume);
        // C: game_gui_set_main_window_sensitivity(TRUE); (src/game_gui.c not yet converted).
        // C: if (stat2 == cur_user) treeview_show_user_player_list(); (GUI not yet converted).

        Game.SaveTeamStates();

        Bygfoot.Stat0 = (int)Status0Value.StatusLiveGamePause;
    }

    /// <summary>
    /// Create a game unit for the live game.
    /// Corresponds to C: live_game_create_unit.
    /// </summary>
    public static void CreateUnit(LiveGame liveGame)
    {
        if (liveGame.Units.Count == 0)
        {
            CreateStartUnit(liveGame);
            return;
        }

        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];

        if (lastUnit.Event.Type == (int)LiveGameEventType.EndMatch)
        {
            Debug.DebugPrintMessage("live_game_create_unit: called after end of match.");
            return;
        }

        LiveGameUnit newUnit = new LiveGameUnit
        {
            Minute = GetMinute(liveGame),
            Time = GetTime(lastUnit),
            Area = lastUnit.Area,
            Result = new[] { lastUnit.Result[0], lastUnit.Result[1] },
            Event = new LiveGameEvent
            {
                Team = -1,
                Player = -1,
                Player2 = -1
            }
        };

        if (lastUnit.Event.Type == (int)LiveGameEventType.HalfTime ||
            lastUnit.Event.Type == (int)LiveGameEventType.ExtraTime)
        {
            EventGeneral(liveGame, true);
            return;
        }
        else if (QueryLiveGameEventIsBreak(liveGame, newUnit.Minute, newUnit.Time))
        {
            newUnit.Event.Type = GetBreak(liveGame);
            newUnit.Possession = lastUnit.Possession;
            liveGame.Units.Add(newUnit);
            return;
        }
        else if (newUnit.Time == (int)LiveGameUnitTime.Penalties)
            newUnit.Event.Type = (int)LiveGameEventType.Penalties;
        else
            FillNewUnit(liveGame, newUnit);

        liveGame.Units.Add(newUnit);
    }

    /// <summary>
    /// Fill in a new unit depending on the team values and the constants from above.
    /// Corresponds to C: live_game_fill_new_unit.
    /// </summary>
    public static void FillNewUnit(LiveGame liveGame, LiveGameUnit newUnit)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team tm0 = liveGame.Fix.Teams[0];
        Team tm1 = liveGame.Fix.Teams[1];
        float rndom = (float)MathHelper.Rnd(0, 1);
        float stadiumEvent =
            1 - (float)Math.Pow(tm0.Stadium.Safety,
                OptionHelper.ConstFloat("float_live_game_stadium_event_exponent"));
        float possessionChange, scoringChance = 0, injuryEventProb, foulEventProb;

        possessionChange = OptionHelper.ConstFloat("float_live_game_event_general") *
            OptionHelper.ConstFloat("float_live_game_possession_changes") /
            PitTeams(liveGame, lastUnit, OptionHelper.ConstFloat("float_live_game_possession_team_exponent"));

        injuryEventProb = OptionHelper.ConstFloat("float_live_game_injury") *
            (1 + (OptionHelper.ConstFloat("float_player_boost_injury_effect") *
            ((tm0.Boost != 0 || tm1.Boost != 0) ? 1f : 0f)));

        newUnit.Possession = lastUnit.Possession;

        if (lastUnit.Event.Type == (int)LiveGameEventType.General)
            newUnit.Area = GetArea(liveGame, newUnit);

        if (newUnit.Area == (int)LiveGameUnitArea.Attack)
            scoringChance = OptionHelper.ConstFloat("float_live_game_scoring_chance") *
                PitTeams(liveGame, newUnit, OptionHelper.ConstFloat("float_live_game_scoring_chance_team_exponent"));

        foulEventProb = Game.GetFoulProb(liveGame, newUnit);

        if (rndom < foulEventProb)
            newUnit.Event.Type = (int)LiveGameEventType.Foul;
        else if (rndom < foulEventProb + injuryEventProb)
            newUnit.Event.Type = (int)LiveGameEventType.Injury;
        else if (rndom < foulEventProb + injuryEventProb + stadiumEvent &&
            liveGame.StadiumEvent == -1)
            newUnit.Event.Type = (int)LiveGameEventType.Stadium;
        else if (rndom < foulEventProb + injuryEventProb + stadiumEvent + possessionChange)
        {
            newUnit.Event.Type = (int)LiveGameEventType.LostPossession;
            newUnit.Possession = 1 - lastUnit.Possession;
            if (newUnit.Area == (int)LiveGameUnitArea.Attack)
                newUnit.Area = (int)LiveGameUnitArea.Defend;
            else if (newUnit.Area == (int)LiveGameUnitArea.Defend)
                newUnit.Area = (int)LiveGameUnitArea.Attack;
        }
        else if (rndom < foulEventProb + injuryEventProb + stadiumEvent + possessionChange + scoringChance)
            newUnit.Event.Type = (int)LiveGameEventType.ScoringChance;
        else
            newUnit.Event.Type = (int)LiveGameEventType.General;
    }

    /// <summary>
    /// Create the first unit of a match.
    /// Corresponds to C: live_game_create_start_unit.
    /// </summary>
    public static void CreateStartUnit(LiveGame liveGame)
    {
        LiveGameUnit newUnit = new LiveGameUnit
        {
            Minute = 0,
            Time = (int)LiveGameUnitTime.FirstHalf,
            Possession = Misc.MathRndi(0, 1),
            Area = (int)LiveGameUnitArea.Midfield,
            Result = new[] { 0, 0 },
            Event = new LiveGameEvent
            {
                Type = (int)LiveGameEventType.StartMatch,
                Team = 0,
                Player = -1,
                Player2 = -1
            }
        };

        newUnit.Event.Team = newUnit.Possession;
        liveGame.StartedGame = newUnit.Possession;

        liveGame.Units.Add(newUnit);
    }

    /// <summary>
    /// Evaluate a live game unit. This means we find out what happens
    /// after the unit, depending on its type.
    /// Corresponds to C: live_game_evaluate_unit.
    /// </summary>
    public static void EvaluateUnit(LiveGame liveGame, LiveGameUnit unit)
    {
        int type = unit.Event.Type;

        if (type == (int)LiveGameEventType.Foul)
            EventFoul(liveGame);
        else if (type == (int)LiveGameEventType.LostPossession)
            EventLostPossession(liveGame);
        else if (type == (int)LiveGameEventType.Injury)
            EventInjury(liveGame, -1, -1, false);
        else if (type == (int)LiveGameEventType.Stadium)
            EventStadium(liveGame);
        else if (type == (int)LiveGameEventType.ScoringChance)
            EventScoringChance(liveGame);
        else if (type == (int)LiveGameEventType.Penalty)
            EventPenalty(liveGame);
        else if (type == (int)LiveGameEventType.General)
            EventGeneral(liveGame, false);
        else if (type == (int)LiveGameEventType.StartMatch)
            FinishUnit(liveGame);
        else if (type == (int)LiveGameEventType.HalfTime ||
            type == (int)LiveGameEventType.ExtraTime ||
            type == (int)LiveGameEventType.Penalties ||
            type == (int)LiveGameEventType.EndMatch)
        {
            FinishUnit(liveGame);
            if (type != (int)LiveGameEventType.EndMatch && show &&
                Variables.Users[Bygfoot.Stat2].Options.GetInt("int_opt_user_pause_break") != 0)
                PauseLiveGame();
        }
        else if (type != (int)LiveGameEventType.EndMatch)
            Debug.DebugPrintMessage("live_game_evaluate_unit: unknown event type {0}", type);
    }

    /// <summary>
    /// Calculate a foul event.
    /// Corresponds to C: live_game_event_foul.
    /// </summary>
    public static void EventFoul(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        float rndom = (float)MathHelper.Rnd(0, 1);
        float reductionFactor = 1;
        int type, fouledPlayer, foulPlayer, foulTeam;

        if ((float)MathHelper.Rnd(0, 1) > OptionHelper.ConstFloat("float_live_game_foul_by_possession") *
            Game.GetFoulPossessionFactor(tms[lastUnit.Possession].Boost, tms[1 - lastUnit.Possession].Boost))
        {
            foulTeam = lastUnit.Event.Team = 1 - lastUnit.Possession;
            if (liveGame.Units[liveGame.Units.Count - 2].Event.Type == (int)LiveGameEventType.General)
                fouledPlayer = lastUnit.Event.Player =
                    liveGame.Units[liveGame.Units.Count - 2].Event.Player;
            else
                fouledPlayer = lastUnit.Event.Player =
                    Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0, -1, false);

            foulPlayer = lastUnit.Event.Player2 =
                Game.GetPlayer(tms[1 - lastUnit.Possession], lastUnit.Area, 0, -1, false);
        }
        else
        {
            foulTeam = lastUnit.Event.Team = lastUnit.Possession;
            fouledPlayer = lastUnit.Event.Player =
                Game.GetPlayer(tms[1 - lastUnit.Possession], lastUnit.Area, 0, -1, false);
            foulPlayer = lastUnit.Event.Player2 =
                Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0, -1, false);
        }

        /* Probability of hard foul gets reduced if the player is already booked, except when boost is on. */
        if (tms[foulTeam].Boost != 1 &&
            Player.OfIdTeam(tms[foulTeam], foulPlayer).CardStatus == PlayerCardStatus.Yellow)
            reductionFactor = 1 - OptionHelper.ConstFloat("float_live_game_foul_booked_reduction");

        if (rndom < OptionHelper.ConstFloat("float_live_game_foul_red_injury") * reductionFactor)
            type = (int)LiveGameEventType.FoulRedInjury;
        else if (rndom < OptionHelper.ConstFloat("float_live_game_foul_red") * reductionFactor)
            type = (int)LiveGameEventType.FoulRed;
        else if (rndom < OptionHelper.ConstFloat("float_live_game_foul_yellow") * reductionFactor)
        {
            type = (int)LiveGameEventType.FoulYellow;
            Player pl = Player.OfIdTeam(tms[foulTeam], foulPlayer);
            Player.CardSet(pl, liveGame.Fix.Clid, (int)PlayerValue.CardYellow, 1, true);
            pl.Career[(int)PlayerValue.CardYellow]++;
            pl.CardStatus = PlayerCardStatus.Yellow;
        }
        else
            type = (int)LiveGameEventType.Foul;

        lastUnit.Event.Type = type;

        FinishUnit(liveGame);

        if (type == (int)LiveGameEventType.FoulRed ||
            type == (int)LiveGameEventType.FoulRedInjury ||
            (type == (int)LiveGameEventType.FoulYellow &&
             QueryLiveGameSecondYellow(liveGame, foulTeam, foulPlayer)))
        {
            EventSendOff(liveGame, foulTeam, foulPlayer,
                QueryLiveGameSecondYellow(liveGame, foulTeam, foulPlayer));
            if (type == (int)LiveGameEventType.FoulRedInjury)
                EventInjury(liveGame, 1 - foulTeam, fouledPlayer, true);
            Player.OfIdTeam(tms[foulTeam], foulPlayer).CardStatus = PlayerCardStatus.Red;
        }

        /* In C, the last_unit macro re-resolves to the current last unit
           after the send-off/injury events possibly appended units. */
        lastUnit = liveGame.Units[liveGame.Units.Count - 1];

        if (lastUnit.Area == (int)LiveGameUnitArea.Attack && foulTeam != lastUnit.Possession)
        {
            rndom = (float)MathHelper.Rnd(0, 1);
            if (rndom < OptionHelper.ConstFloat("float_live_game_penalty_prob"))
                EventPenalty(liveGame);
            else if (rndom < OptionHelper.ConstFloat("float_live_game_free_kick_prob"))
                EventFreeKick(liveGame);
            else
                lastUnit.Possession = 1 - foulTeam;
        }
        else
            lastUnit.Possession = 1 - foulTeam;
    }

    /// <summary>
    /// Calculate a lost possession event.
    /// Corresponds to C: live_game_event_lost_possession.
    /// </summary>
    public static void EventLostPossession(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };

        lastUnit.Event.Player =
            Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0, -1, true);

        if (liveGame.Units[liveGame.Units.Count - 2].Event.Type == (int)LiveGameEventType.General)
            lastUnit.Event.Player2 =
                liveGame.Units[liveGame.Units.Count - 2].Event.Player;
        else
            lastUnit.Event.Player2 =
                Game.GetPlayer(tms[1 - lastUnit.Possession],
                    liveGame.Units[liveGame.Units.Count - 2].Area, 0, -1, false);

        FinishUnit(liveGame);

        EventGeneral(liveGame, true);
    }

    /// <summary>
    /// Calculate an injury event.
    /// Corresponds to C: live_game_event_injury.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="team">The team the player is from.</param>
    /// <param name="player">The player that's injured, or -1 if we have to choose a random one.</param>
    /// <param name="createNew">Whether to put the event into a new unit instead of the last one.</param>
    public static void EventInjury(LiveGame liveGame, int team, int player, bool createNew)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        int oldStructure = -1, subIn = -1;
        int usrIdx = -1;

        if (createNew)
        {
            LiveGameUnit newUnit = CopyUnit(lastUnit);
            newUnit.Event.Commentary = null;
            liveGame.Units.Add(newUnit);

            // In C, the last_unit macro re-resolves to the newly appended unit.
            lastUnit = newUnit;

            lastUnit.Event.Player = player;
            lastUnit.Event.Team = team;
        }
        else
            InjuryGetPlayer(liveGame);

        usrIdx = Team.IsUser(tms[lastUnit.Event.Team]);

        lastUnit.Minute = -1;
        lastUnit.Event.Type = (int)LiveGameEventType.Injury;

        if ((float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_injury_is_temp"))
        {
            lastUnit.Event.Type = (int)LiveGameEventType.TempInjury;

            if (Variables.DebugLevel < 50 ||
                usrIdx == -1)
            {
                Player pl = Player.OfIdTeam(tms[lastUnit.Event.Team], lastUnit.Event.Player);
                pl.Fitness = Math.Max(0, pl.Fitness -
                    (float)MathHelper.Rnd(
                        OptionHelper.ConstFloat("float_live_game_temp_injury_fitness_decrease_lower"),
                        OptionHelper.ConstFloat("float_live_game_temp_injury_fitness_decrease_upper")));
            }
        }

        FinishUnit(liveGame);

        if (Variables.DebugLevel >= 50 &&
            usrIdx != -1)
            return;

        if (lastUnit.Event.Type == (int)LiveGameEventType.Injury)
        {
            Team tm = tms[lastUnit.Event.Team];
            Game.PlayerInjury(Player.OfIdTeam(tm, lastUnit.Event.Player));

            if (liveGame.SubsLeft[lastUnit.Event.Team] > 0)
            {
                if (show &&
                    usrIdx != -1 &&
                    ((Variables.Users[usrIdx].Options.GetInt("int_opt_user_pause_injury") != 0 &&
                      Variables.Users[usrIdx].Options.GetInt("int_opt_user_auto_sub") == 0) ||
                     tm.Players.Count == 11))
                    PauseLiveGame();
                else if (tm.Players.Count > 11)
                {
                    subIn = Game.SubstitutePlayer(tm,
                        Player.IdIndex(tm, lastUnit.Event.Player, true));
                    if (subIn != -1)
                    {
                        oldStructure = tm.Structure;
                        EventSubstitution(liveGame, lastUnit.Event.Team, subIn,
                            lastUnit.Event.Player);

                        if (oldStructure != tm.Structure)
                            EventTeamChange(liveGame, lastUnit.Event.Team,
                                (int)LiveGameEventType.StructureChange);
                    }
                }
            }
        }

        UpdateTeamValues(liveGame);
    }

    /// <summary>
    /// Calculate a stadium event.
    /// Corresponds to C: live_game_event_stadium.
    /// </summary>
    public static void EventStadium(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team tm0 = liveGame.Fix.Teams[0];
        float rndom = (float)MathHelper.Rnd(0, 1);
        float[] probs =
        {
            OptionHelper.ConstFloat("float_live_game_stadium_event_breakdown"),
            OptionHelper.ConstFloat("float_live_game_stadium_event_riots"),
            OptionHelper.ConstFloat("float_live_game_stadium_event_fire")
        };

        for (int i = 1; i < 3; i++)
            probs[i] += probs[i - 1];

        if (rndom <= probs[0])
            lastUnit.Event.Type = (int)LiveGameEventType.StadiumBreakdown;
        else if (rndom <= probs[1])
            lastUnit.Event.Type = (int)LiveGameEventType.StadiumRiots;
        else if (rndom <= probs[2])
            lastUnit.Event.Type = (int)LiveGameEventType.StadiumFire;

        FinishUnit(liveGame);

        if (Team.IsUser(tm0) != -1 && Variables.DebugLevel < 50)
            Game.StadiumEvent(tm0.Stadium, lastUnit.Event.Type);

        liveGame.StadiumEvent = lastUnit.Event.Type;

        EventGeneral(liveGame, true);
    }

    /// <summary>
    /// Calculate a scoring chance event.
    /// Corresponds to C: live_game_event_scoring_chance.
    /// </summary>
    public static void EventScoringChance(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        LiveGameUnit prevUnit = liveGame.Units[liveGame.Units.Count - 2];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        int resIdx = (lastUnit.Time == (int)LiveGameUnitTime.ExtraTime) ? 1 : 0;

        if ((float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_scoring_chance_is_own_goal"))
        {
            lastUnit.Event.Type = (int)LiveGameEventType.OwnGoal;
            lastUnit.Event.Player =
                Game.GetPlayer(tms[1 - lastUnit.Possession], (int)GamePlayerType.Defend, 0, -1, false);
            lastUnit.Event.Team = 1 - lastUnit.Possession;
            liveGame.Fix.Result[lastUnit.Possession, resIdx]++;
            lastUnit.Result[lastUnit.Possession]++;

            Player.StreakAddToProb(Player.OfIdTeam(tms[lastUnit.Event.Team],
                lastUnit.Event.Player),
                OptionHelper.ConstFloat("float_player_streak_add_own_goal"));
        }
        else
        {
            lastUnit.Event.Team = lastUnit.Possession;

            if (prevUnit.Event.Player != -1 &&
                (float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_player_in_poss_shoots") &&
                Player.QueryIdInTeam(prevUnit.Event.Player, tms[lastUnit.Possession]))
                lastUnit.Event.Player = prevUnit.Event.Player;
            else
            {
                if (prevUnit.Event.Player != -1 &&
                    Player.QueryIdInTeam(prevUnit.Event.Player, tms[lastUnit.Possession]))
                {
                    lastUnit.Event.Player =
                        Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0,
                            prevUnit.Event.Player, true);
                    lastUnit.Event.Player2 = prevUnit.Event.Player;
                }
                else
                {
                    lastUnit.Event.Player =
                        Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0, -1, true);
                    lastUnit.Event.Player2 =
                        Game.GetPlayer(tms[lastUnit.Possession], lastUnit.Area, 0,
                            lastUnit.Event.Player, true);
                }
            }

            if ((float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_scoring_chance_is_header"))
                lastUnit.Event.Type = (int)LiveGameEventType.Header;
        }

        FinishUnit(liveGame);

        if (lastUnit.Event.Type != (int)LiveGameEventType.OwnGoal)
            EventDuel(liveGame);
        else
            EventGeneral(liveGame, true);
    }

    /// <summary>
    /// Calculate a penalty event.
    /// Corresponds to C: live_game_event_penalty.
    /// </summary>
    public static void EventPenalty(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];

        if (lastUnit.Time != (int)LiveGameUnitTime.Penalties)
        {
            LiveGameUnit newUnit = CopyUnit(lastUnit);
            newUnit.Event.Commentary = null;
            newUnit.Minute = -1;
            newUnit.Event.Type = (int)LiveGameEventType.Penalty;

            liveGame.Units.Add(newUnit);

            // In C, the last_unit macro re-resolves to the newly appended unit.
            lastUnit = newUnit;
        }

        lastUnit.Event.Player2 = -1;
        lastUnit.Event.Team = -1;

        if (lastUnit.Time == (int)LiveGameUnitTime.Penalties)
        {
            if (PenaltiesTaken(liveGame) == 1)
            {
                lastUnit.Possession = Misc.MathRndi(0, 1);
                lastUnit.Event.Player =
                    Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                        (int)GamePlayerType.Penalty, -1, -1, false);
            }
            else if (PenaltiesTaken(liveGame) == 2)
            {
                lastUnit.Possession = 1 - liveGame.Units[liveGame.Units.Count - 3].Possession;
                lastUnit.Event.Player =
                    Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                        (int)GamePlayerType.Penalty, -1, -1, false);
            }
            else
            {
                lastUnit.Possession = 1 - liveGame.Units[liveGame.Units.Count - 3].Possession;
                lastUnit.Event.Player =
                    Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                        (int)GamePlayerType.Penalty,
                        liveGame.Units[liveGame.Units.Count - 4].Event.Player, -1, false);
            }
        }
        else
        {
            lastUnit.Event.Team = lastUnit.Possession;

            lastUnit.Event.Player =
                Game.GetDefaultPenaltyShooter(liveGame.Fix.Teams[lastUnit.Possession]);
            if (lastUnit.Event.Player == -1)
                lastUnit.Event.Player =
                    Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                        (int)GamePlayerType.Penalty, -1, -1, false);
        }

        FinishUnit(liveGame);

        EventDuel(liveGame);
    }

    /// <summary>
    /// Calculate a general event.
    /// Corresponds to C: live_game_event_general.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="createNew">Whether we create a new unit for the event.</param>
    public static void EventGeneral(LiveGame liveGame, bool createNew)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };

        if (createNew && Bygfoot.Stat0 == (int)Status0Value.StatusLiveGamePause)
            return;

        if (createNew)
        {
            LiveGameUnit newUnit = new LiveGameUnit
            {
                Minute = GetMinute(liveGame),
                Time = lastUnit.Time,
                Result = new[] { lastUnit.Result[0], lastUnit.Result[1] },
                Event = new LiveGameEvent
                {
                    Type = (int)LiveGameEventType.General,
                    Team = -1
                }
            };

            if (lastUnit.Event.Type == (int)LiveGameEventType.General ||
                lastUnit.Event.Type == (int)LiveGameEventType.StartMatch ||
                lastUnit.Event.Type == (int)LiveGameEventType.LostPossession ||
                lastUnit.Event.Type == (int)LiveGameEventType.Foul ||
                lastUnit.Event.Type == (int)LiveGameEventType.FoulYellow ||
                lastUnit.Event.Type == (int)LiveGameEventType.SendOff ||
                lastUnit.Event.Type == (int)LiveGameEventType.Injury ||
                lastUnit.Event.Type == (int)LiveGameEventType.TempInjury ||
                lastUnit.Event.Type == (int)LiveGameEventType.Stadium ||
                lastUnit.Event.Type == (int)LiveGameEventType.StadiumBreakdown ||
                lastUnit.Event.Type == (int)LiveGameEventType.StadiumFire ||
                lastUnit.Event.Type == (int)LiveGameEventType.StadiumRiots ||
                (lastUnit.Event.Type >= (int)LiveGameEventType.StructureChange &&
                 lastUnit.Event.Type <= (int)LiveGameEventType.BoostChangeOn) ||
                ((lastUnit.Event.Type == (int)LiveGameEventType.Post ||
                  lastUnit.Event.Type == (int)LiveGameEventType.CrossBar) &&
                 (float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_possession_after_post")))
            {
                newUnit.Possession = lastUnit.Possession;
                newUnit.Area = (lastUnit.Event.Type == (int)LiveGameEventType.General) ?
                    GetArea(liveGame, lastUnit) : lastUnit.Area;
            }
            else if (lastUnit.Event.Type == (int)LiveGameEventType.Goal ||
                lastUnit.Event.Type == (int)LiveGameEventType.OwnGoal ||
                lastUnit.Event.Type == (int)LiveGameEventType.Miss ||
                lastUnit.Event.Type == (int)LiveGameEventType.Save ||
                lastUnit.Event.Type == (int)LiveGameEventType.Post ||
                lastUnit.Event.Type == (int)LiveGameEventType.CornerKick ||
                lastUnit.Event.Type == (int)LiveGameEventType.CrossBar)
            {
                newUnit.Possession = 1 - lastUnit.Possession;
                if (lastUnit.Event.Type == (int)LiveGameEventType.Goal ||
                    lastUnit.Event.Type == (int)LiveGameEventType.OwnGoal)
                    newUnit.Area = (int)LiveGameUnitArea.Midfield;
                else
                    newUnit.Area = (int)LiveGameUnitArea.Defend;
            }
            else if (lastUnit.Event.Type == (int)LiveGameEventType.HalfTime)
            {
                newUnit.Possession = 1 - liveGame.StartedGame;
                newUnit.Time = (int)LiveGameUnitTime.SecondHalf;
                newUnit.Area = (int)LiveGameUnitArea.Midfield;
            }
            else if (lastUnit.Event.Type == (int)LiveGameEventType.ExtraTime)
            {
                newUnit.Possession = Misc.MathRndi(0, 1);
                newUnit.Time = (int)LiveGameUnitTime.ExtraTime;
                newUnit.Area = (int)LiveGameUnitArea.Midfield;
            }
            else
                Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                    "live_game_event_general: unknown event type: {0}", lastUnit.Event.Type);

            liveGame.Units.Add(newUnit);
        }

        EventGeneralGetPlayers(liveGame);

        FinishUnit(liveGame);

        /* First, check whether CPU strategy changes are made. */
        if (Team.IsUser(tms[0]) == -1)
            Strategy.LiveGameCheck(liveGame, 0);

        if (Team.IsUser(tms[1]) == -1)
            Strategy.LiveGameCheck(liveGame, 1);

        /* In C, the last_unit macro re-resolves to the current last unit
           after the strategy checks possibly appended team-change units. */
        LiveGameUnit finalUnit = liveGame.Units[liveGame.Units.Count - 1];

        if (finalUnit.Event.Type >= (int)LiveGameEventType.StructureChange &&
            finalUnit.Event.Type <= (int)LiveGameEventType.BoostChangeOn)
            EventGeneral(liveGame, true);
    }

    /// <summary>
    /// Fill in the players values in a general unit.
    /// Corresponds to C: live_game_event_general_get_players.
    /// </summary>
    public static void EventGeneralGetPlayers(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        int oldPl1 = liveGame.Units[liveGame.Units.Count - 2].Event.Player;
        int type = liveGame.Units[liveGame.Units.Count - 2].Event.Type;

        lastUnit.Event.Player = -1;
        lastUnit.Event.Player2 = -1;

        if (type == (int)LiveGameEventType.LostPossession)
        {
            lastUnit.Event.Player2 = oldPl1;
            lastUnit.Event.Player = Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                lastUnit.Area, 0, lastUnit.Event.Player2, true);
        }
        else if (type != (int)LiveGameEventType.General)
        {
            lastUnit.Event.Player = Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                lastUnit.Area, 0, -1, true);
            if ((float)MathHelper.Rnd(0, 1) < OptionHelper.ConstFloat("float_live_game_general_event_second_player"))
                lastUnit.Event.Player2 = Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                    lastUnit.Area, 0, lastUnit.Event.Player, true);
        }
        else
        {
            lastUnit.Event.Player2 = Player.QueryIdInTeam(oldPl1, liveGame.Fix.Teams[lastUnit.Possession]) ? oldPl1 : -1;
            lastUnit.Event.Player = Game.GetPlayer(liveGame.Fix.Teams[lastUnit.Possession],
                lastUnit.Area, 0, lastUnit.Event.Player2, true);
        }
    }

    /// <summary>
    /// Calculate a free kick event.
    /// Corresponds to C: live_game_event_free_kick.
    /// </summary>
    public static void EventFreeKick(LiveGame liveGame)
    {
        LiveGameUnit newUnit = CopyUnit(liveGame.Units[liveGame.Units.Count - 1]);

        newUnit.Event.Player = -1;
        newUnit.Event.Player2 = -1;

        newUnit.Minute = -1;

        newUnit.Event.Type = (int)LiveGameEventType.FreeKick;
        newUnit.Event.Team = newUnit.Possession;
        newUnit.Event.Commentary = null;

        newUnit.Event.Player =
            Game.GetDefaultPenaltyShooter(liveGame.Fix.Teams[newUnit.Possession]);

        if (newUnit.Event.Player == -1)
            newUnit.Event.Player =
                Game.GetPlayer(liveGame.Fix.Teams[newUnit.Possession], newUnit.Area, 0, -1, true);

        liveGame.Units.Add(newUnit);

        FinishUnit(liveGame);

        EventDuel(liveGame);
    }

    /// <summary>
    /// Calculate a send-off event.
    /// Corresponds to C: live_game_event_send_off.
    /// </summary>
    public static void EventSendOff(LiveGame liveGame, int team, int player, bool secondYellow)
    {
        Team tm = liveGame.Fix.Teams[team];
        LiveGameUnit newUnit = CopyUnit(liveGame.Units[liveGame.Units.Count - 1]);
        int substitute = -1, toSubstitute = -1;
        int usrIdx = Team.IsUser(tm);

        newUnit.Event.Player = -1;
        newUnit.Event.Player2 = -1;

        newUnit.Minute = -1;

        newUnit.Event.Type = (int)LiveGameEventType.SendOff;
        newUnit.Event.Player = player;
        newUnit.Event.Team = team;
        newUnit.Event.Commentary = null;

        liveGame.Units.Add(newUnit);

        FinishUnit(liveGame);

        if (Variables.DebugLevel >= 50 && usrIdx != -1)
            return;

        Player pl = Player.OfIdTeam(tm, player);
        Player.StreakAddToProb(pl, OptionHelper.ConstFloat("float_player_streak_add_sendoff"));
        if (pl.Streak == PlayerStreak.Hot)
        {
            pl.Streak = PlayerStreak.None;
            Player.StreakResetCount(pl);
        }

        pl.Cskill = 0;

        if (secondYellow)
            Player.CardSet(pl, liveGame.Fix.Clid, (int)PlayerValue.CardRed, 2, false);
        else
            Player.CardSet(pl, liveGame.Fix.Clid, (int)PlayerValue.CardRed,
                Game.GetPlayerBanDuration(), false);

        pl.Career[(int)PlayerValue.CardRed]++;

        if (usrIdx != -1)
        {
            tm.Structure = Team.FindAppropriateStructure(tm);
            Team.Rearrange(tm);
            EventTeamChange(liveGame, team, (int)LiveGameEventType.StructureChange);
        }

        if (liveGame.SubsLeft[team] > 0)
        {
            if (show && usrIdx != -1 &&
                ((Variables.Users[usrIdx].Options.GetInt("int_opt_user_pause_red") != 0 &&
                  Variables.Users[usrIdx].Options.GetInt("int_opt_user_auto_sub") == 0) ||
                 tm.Players.Count == 1))
                PauseLiveGame();
            else if (tm.Players.Count > 11)
            {
                Game.SubstitutePlayerSendOff(liveGame.Fix.Clid, tm,
                    Player.IdIndex(tm, player, true),
                    out toSubstitute, out substitute);

                if (toSubstitute != -1)
                    EventSubstitution(liveGame, team, substitute, toSubstitute);
                else
                {
                    tm.Structure = Team.FindAppropriateStructure(tm);
                    Team.Rearrange(tm);
                }
                EventTeamChange(liveGame, team, (int)LiveGameEventType.StructureChange);
            }
        }
        else if (usrIdx == -1)
        {
            tm.Structure = Team.FindAppropriateStructure(tm);
            Team.Rearrange(tm);
            EventTeamChange(liveGame, team, (int)LiveGameEventType.StructureChange);
        }

        UpdateTeamValues(liveGame);
    }

    /// <summary>
    /// Calculate whether a player who tries to score succeeds.
    /// Corresponds to C: live_game_event_duel.
    /// </summary>
    public static void EventDuel(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        float rndom = (float)MathHelper.Rnd(0, 1);
        float scoringProb;
        float duelFactor;
        LiveGameUnit newUnit = CopyUnit(lastUnit);
        Player attacker, goalie, assistant = null;
        int resIdx1, resIdx2;

        newUnit.Minute = -1;
        newUnit.Event.Team = newUnit.Possession;
        newUnit.Event.Commentary = null;

        attacker = Player.OfIdTeam(tms[newUnit.Possession], newUnit.Event.Player);
        goalie = Player.OfIdxTeam(tms[1 - newUnit.Possession], 0);

        if (newUnit.Event.Player2 != -1)
            assistant = Player.OfIdTeam(tms[newUnit.Possession], newUnit.Event.Player2);

        newUnit.Event.Player2 = goalie.Id;

        duelFactor = Player.GetGameSkill(attacker, false, true) /
            Player.GetGameSkill(goalie, false, true);

        resIdx1 = newUnit.Possession;
        if (newUnit.Time == (int)LiveGameUnitTime.Penalties)
            resIdx2 = 2;
        else if (newUnit.Time == (int)LiveGameUnitTime.ExtraTime)
            resIdx2 = 1;
        else
            resIdx2 = 0;

        if (lastUnit.Event.Type == (int)LiveGameEventType.Penalty)
            scoringProb = OptionHelper.ConstFloat("float_live_game_score_penalty") * duelFactor * attacker.Team.Luck;
        else if (lastUnit.Event.Type == (int)LiveGameEventType.FreeKick)
            scoringProb = OptionHelper.ConstFloat("float_live_game_score_free_kick") * duelFactor * attacker.Team.Luck;
        else
            scoringProb = OptionHelper.ConstFloat("float_live_game_score_base_prob") *
                (float)Math.Pow(duelFactor, OptionHelper.ConstFloat("float_live_game_score_duel_exponent")) *
                (float)Math.Pow(liveGame.TeamValues[newUnit.Possession * (int)GameTeamValue.End + (int)GameTeamValue.Attack] / (float)liveGame.TeamValues[(1 - newUnit.Possession) * (int)GameTeamValue.End + (int)GameTeamValue.Defend],
                    OptionHelper.ConstFloat("float_live_game_score_team_exponent")) * attacker.Team.Luck;

        if (newUnit.Time != (int)LiveGameUnitTime.Penalties)
        {
            Player.GamesGoalsSet(attacker, liveGame.Fix.Clid, (int)PlayerValue.Shots, 1);
            attacker.Career[(int)PlayerValue.Shots]++;
        }

        if (rndom < scoringProb)
        {
            newUnit.Event.Type = (int)LiveGameEventType.Goal;
            liveGame.Fix.Result[resIdx1, resIdx2]++;
            newUnit.Result[resIdx1]++;

            if (newUnit.Time != (int)LiveGameUnitTime.Penalties)
            {
                Player.GamesGoalsSet(attacker, liveGame.Fix.Clid, (int)PlayerValue.Goals, 1);
                Player.GamesGoalsSet(goalie, liveGame.Fix.Clid, (int)PlayerValue.Goals, 1);
                attacker.Career[(int)PlayerValue.Goals]++;
                goalie.Career[(int)PlayerValue.Goals]++;

                Player.StreakAddToProb(attacker, OptionHelper.ConstFloat("float_player_streak_add_goal"));
                Player.StreakAddToProb(goalie, OptionHelper.ConstFloat("float_player_streak_add_goalie_goal"));
                if (assistant != null)
                    Player.StreakAddToProb(assistant, OptionHelper.ConstFloat("float_player_streak_add_assist"));
            }
        }
        else
        {
            /* In penalties a player can't push the ball in corner */
            if (newUnit.Time == (int)LiveGameUnitTime.Penalties)
                newUnit.Event.Type = MathHelper.GaussDisti((int)LiveGameEventType.Post, (int)LiveGameEventType.CrossBar);
            else
                newUnit.Event.Type = MathHelper.GaussDisti((int)LiveGameEventType.Post, (int)LiveGameEventType.PlayerPushedInCorner);
        }

        if (newUnit.Time != (int)LiveGameUnitTime.Penalties &&
            (newUnit.Event.Type == (int)LiveGameEventType.Save ||
             newUnit.Event.Type == (int)LiveGameEventType.KeeperPushedInCorner ||
             newUnit.Event.Type == (int)LiveGameEventType.Goal))
        {
            Player.GamesGoalsSet(goalie, liveGame.Fix.Clid, (int)PlayerValue.Shots, 1);
            goalie.Career[(int)PlayerValue.Shots]++;

            if (newUnit.Event.Type == (int)LiveGameEventType.Save ||
                newUnit.Event.Type == (int)LiveGameEventType.KeeperPushedInCorner)
                Player.StreakAddToProb(goalie, OptionHelper.ConstFloat("float_player_streak_add_goalie_save"));
        }

        liveGame.Units.Add(newUnit);

        // In C, the last_unit macro re-resolves to the newly appended unit.
        lastUnit = newUnit;

        FinishUnit(liveGame);

        if (lastUnit.Time != (int)LiveGameUnitTime.Penalties)
        {
            if (lastUnit.Event.Type == (int)LiveGameEventType.KeeperPushedInCorner ||
                lastUnit.Event.Type == (int)LiveGameEventType.PlayerPushedInCorner)
                EventCornerKick(liveGame);
            else
                EventGeneral(liveGame, true);
        }
    }

    /// <summary>
    /// Create an event for the corner kick.
    /// Corresponds to C: live_game_event_corner_kick.
    /// </summary>
    public static void EventCornerKick(LiveGame liveGame)
    {
        if (Bygfoot.Stat0 == (int)Status0Value.StatusLiveGamePause)
            return;

        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        LiveGameUnit newUnit = new LiveGameUnit
        {
            Minute = GetMinute(liveGame),
            Time = lastUnit.Time,
            Area = lastUnit.Area,
            Possession = lastUnit.Possession,
            Result = new[] { lastUnit.Result[0], lastUnit.Result[1] },
            Event = new LiveGameEvent
            {
                Type = (int)LiveGameEventType.CornerKick,
                Player = -1,
                Player2 = -1
            }
        };

        newUnit.Event.Team = newUnit.Possession;

        newUnit.Event.Player =
            Game.GetDefaultPenaltyShooter(liveGame.Fix.Teams[newUnit.Possession]);

        if (newUnit.Event.Player == -1)
            newUnit.Event.Player =
                Game.GetPlayer(liveGame.Fix.Teams[newUnit.Possession], newUnit.Area, 0, -1, true);

        liveGame.Units.Add(newUnit);

        FinishUnit(liveGame);

        /* The corner is taken, define what happens next */
        lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        LiveGameUnit newUnit2 = CopyUnit(lastUnit);
        newUnit2.Minute = -1;
        newUnit2.Event.Team = newUnit2.Possession;
        newUnit2.Event.Commentary = null;
        newUnit2.Event.Type = (int)LiveGameEventType.General;
        newUnit2.Event.CommentaryId = -1;

        FillNewUnit(liveGame, newUnit2);

        liveGame.Units.Add(newUnit2);

        EvaluateUnit(liveGame, newUnit2);
    }

    /// <summary>
    /// Find a random player (influenced by fitness) who gets injured.
    /// Corresponds to C: live_game_injury_get_player.
    /// </summary>
    public static void InjuryGetPlayer(LiveGame liveGame)
    {
        LiveGameUnit lastUnit = liveGame.Units[liveGame.Units.Count - 1];
        Team[] tms = { liveGame.Fix.Teams[0], liveGame.Fix.Teams[1] };
        float[] probs = new float[22];
        float fitnessFactor;
        float goalieFactor = OptionHelper.ConstFloat("float_live_game_injury_goalie_factor");
        float boostFactor = OptionHelper.ConstFloat("float_player_boost_injury_effect");

        for (int j = 0; j < 2; j++)
        {
            fitnessFactor = (Player.OfIdxTeam(tms[j], 0).Fitness < 0.025) ?
                40 : 1 / Player.OfIdxTeam(tms[j], 0).Fitness;
            probs[j * 11] = goalieFactor * fitnessFactor *
                (Player.OfIdxTeam(tms[j], 0).Cskill != 0 ? 1f : 0f) *
                (1 + tms[j].Boost * boostFactor);
            if (j == 1)
                probs[11] += probs[10];

            for (int i = 1; i < 11; i++)
            {
                fitnessFactor = (Player.OfIdxTeam(tms[j], i).Fitness < 0.025) ?
                    40 : 1 / Player.OfIdxTeam(tms[j], i).Fitness;
                probs[i + j * 11] = probs[i + j * 11 - 1] + (1 - goalieFactor) * fitnessFactor *
                    (Player.OfIdxTeam(tms[j], i).Cskill != 0 ? 1f : 0f) *
                    (1 + tms[j].Boost * boostFactor);
            }
        }

        float rndom = (float)MathHelper.Rnd(0, probs[21]);

        if (rndom < probs[0])
        {
            lastUnit.Event.Player = Player.OfIdxTeam(tms[0], 0).Id;
            lastUnit.Event.Team = 0;
        }
        else
            for (int i = 1; i < 22; i++)
                if (probs[i - 1] <= rndom && rndom < probs[i])
                {
                    lastUnit.Event.Player =
                        Player.OfIdxTeam(tms[(i > 10) ? 1 : 0], i % 11).Id;
                    lastUnit.Event.Team = (i > 10) ? 1 : 0;
                }
    }

    /// <summary>
    /// Resume a live game. Show team changes.
    /// Corresponds to C: live_game_resume.
    /// </summary>
    public static void Resume()
    {
        LiveGame liveGame = Variables.Users[Bygfoot.Stat2].LiveGame;

        Variables.Statp = liveGame;

        for (int i = 0; i < 2; i++)
        {
            int[] subsIn = new int[3];
            int[] subsOut = new int[3];

            Game.GetSubs(i, subsIn, subsOut);
            for (int j = 0; j < 3; j++)
            {
                if (subsIn[j] != -1)
                    EventSubstitution(liveGame, i, subsIn[j], subsOut[j]);
            }

            Team tm = liveGame.Fix.Teams[i];
            LiveGameTeamState teamState = liveGame.TeamState[i];

            if (tm.Structure != teamState.Structure)
                EventTeamChange(liveGame, i, (int)LiveGameEventType.StructureChange);

            if (tm.Style != teamState.Style)
                EventTeamChange(liveGame, i, (int)LiveGameEventType.StyleChangeAllOutDefend +
                    tm.Style + 2);

            if (tm.Boost != (teamState.Boost ? 1 : 0))
                EventTeamChange(liveGame, i, (int)LiveGameEventType.BoostChangeAnti +
                    tm.Boost + 1);
        }

        CalculateFixture(liveGame.Fix, liveGame);
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
        LiveGameUnit lastUnit = GetLastUnit(liveGame);
        int type;

        if (lastUnit.Time == (int)LiveGameUnitTime.FirstHalf)
            type = (int)LiveGameEventType.HalfTime;
        else if (lastUnit.Time == (int)LiveGameUnitTime.SecondHalf)
        {
            if (Fixture.QueryFixtureIsDraw(liveGame.Fix))
                type = (int)LiveGameEventType.ExtraTime;
            else
                type = (int)LiveGameEventType.EndMatch;
        }
        else if (lastUnit.Time == (int)LiveGameUnitTime.ExtraTime)
        {
            if (Fixture.QueryFixtureIsDraw(liveGame.Fix))
                type = (int)LiveGameEventType.Penalties;
            else
                type = (int)LiveGameEventType.EndMatch;
        }
        else
            type = (int)LiveGameEventType.EndMatch;

        return type;
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
    /// Show a substitution event.
    /// Corresponds to C: live_game_event_substitution.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="teamNumber">The team that substitutes.</param>
    /// <param name="subIn">The id of the player who moves into the team.</param>
    /// <param name="subOut">The id of the player who gets replaced.</param>
    public static void EventSubstitution(LiveGame liveGame, int teamNumber, int subIn, int subOut)
    {
        LiveGameUnit lastUnit = GetLastUnit(liveGame);
        LiveGameUnit newUnit = new LiveGameUnit
        {
            Possession = lastUnit.Possession,
            Area = lastUnit.Area,
            Result = new[] { lastUnit.Result[0], lastUnit.Result[1] },
            Event = new LiveGameEvent
            {
                Verbosity = lastUnit.Event.Verbosity,
                Team = lastUnit.Event.Team,
                Player = lastUnit.Event.Player,
                Player2 = lastUnit.Event.Player2,
                Commentary = lastUnit.Event.Commentary,
                CommentaryId = lastUnit.Event.CommentaryId
            }
        };

        newUnit.Minute = -1;
        newUnit.Time = GetTime(lastUnit);

        newUnit.Event.Type = (int)LiveGameEventType.Substitution;
        newUnit.Event.Team = teamNumber;
        newUnit.Event.Player = subIn;
        newUnit.Event.Player2 = subOut;
        newUnit.Event.Commentary = null;

        Team tm = liveGame.Fix.Teams[teamNumber];
        Player plIn = Player.OfIdTeam(tm, subIn);

        if (plIn.Cskill > 0)
        {
            liveGame.SubsLeft[teamNumber]--;

            Player.StreakAddToProb(plIn, OptionHelper.ConstFloat("float_player_streak_add_sub_out"));
            Player.StreakAddToProb(plIn, OptionHelper.ConstFloat("float_player_streak_add_sub_in"));

            Player.GamesGoalsSet(plIn, liveGame.Fix.Clid, (int)PlayerValue.Games, 1);
            plIn.Career[(int)PlayerValue.Games]++;
            plIn.Participation = true;

            // C: if (show) game_gui_live_game_show_opponent(); (GUI not yet converted).
        }

        liveGame.Units.Add(newUnit);

        FinishUnit(liveGame);
    }

    /// <summary>
    /// Show a team change event, e.g. structure change.
    /// Corresponds to C: live_game_event_team_change.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="teamNumber">The index of the team.</param>
    /// <param name="eventType">The event type.</param>
    public static void EventTeamChange(LiveGame liveGame, int teamNumber, int eventType)
    {
        LiveGameUnit lastUnit = GetLastUnit(liveGame);
        LiveGameUnit newUnit = new LiveGameUnit
        {
            Possession = lastUnit.Possession,
            Area = lastUnit.Area,
            Result = new[] { lastUnit.Result[0], lastUnit.Result[1] },
            Event = new LiveGameEvent
            {
                Verbosity = lastUnit.Event.Verbosity,
                Team = lastUnit.Event.Team,
                Player = lastUnit.Event.Player,
                Player2 = lastUnit.Event.Player2,
                Commentary = lastUnit.Event.Commentary,
                CommentaryId = lastUnit.Event.CommentaryId
            }
        };

        newUnit.Minute = -1;
        newUnit.Time = GetTime(lastUnit);

        newUnit.Event.Team = teamNumber;
        newUnit.Event.Player = newUnit.Event.Player2 = -1;
        newUnit.Event.Type = eventType;
        newUnit.Event.Commentary = null;

        liveGame.Units.Add(newUnit);

        FinishUnit(liveGame);

        // C: if (show) game_gui_live_game_show_opponent(); (GUI not yet converted).
    }

    /// <summary>
    /// Finish a unit of the live game.
    /// Corresponds to C: live_game_finish_unit.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    public static void FinishUnit(LiveGame liveGame)
    {
        LiveGameUnit unit = GetLastUnit(liveGame);

        if (unit.Minute != -1 && unit.Time != (int)LiveGameUnitTime.Penalties)
        {
            if (Bygfoot.Stat2 != -1 && Variables.Users[Bygfoot.Stat2].Team.Boost == 1)
                Game.BoostCost();

            Game.DecreaseFitness(liveGame.Fix);

            float[,] teamValues = new float[2, (int)GameTeamValue.End];
            Game.GetValues(liveGame.Fix, teamValues, liveGame.HomeAdvantage);
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < (int)GameTeamValue.End; j++)
                    liveGame.TeamValues[i * (int)GameTeamValue.End + j] = (int)teamValues[i, j];

            // C: if (stat2 != -1 && stat2 == cur_user && show &&
            //     unit->minute % opt_int("int_opt_live_game_player_list_refresh") == 0)
            //     treeview_show_user_player_list(); (GUI not yet converted).
        }

        Game.UpdateStats(liveGame, unit);

        if (Bygfoot.Stat2 != -1 || Bygfoot.Stat5 < -1000)
        {
            // C: if (unit->time != LIVE_GAME_UNIT_TIME_PENALTIES && show)
            //     treeview_show_game_stats(...); (GUI not yet converted).

            unit.Event.Verbosity = EventGetVerbosity(unit.Event.Type);
        }

        // C: if (show) game_gui_live_game_show_unit(unit); (GUI not yet converted).
    }

    /// <summary>
    /// Return the verbosity of a live game event.
    /// Corresponds to C: live_game_event_get_verbosity.
    /// </summary>
    /// <param name="eventType">The event type.</param>
    /// <returns>The verbosity of the event.</returns>
    public static int EventGetVerbosity(int eventType)
    {
        int returnValue = -1;

        if (eventType == (int)LiveGameEventType.StartMatch ||
            eventType == (int)LiveGameEventType.HalfTime ||
            eventType == (int)LiveGameEventType.ExtraTime ||
            eventType == (int)LiveGameEventType.EndMatch ||
            eventType == (int)LiveGameEventType.Penalties ||
            eventType == (int)LiveGameEventType.Goal ||
            eventType == (int)LiveGameEventType.OwnGoal)
            returnValue = 0;
        else if (eventType == (int)LiveGameEventType.Penalty ||
            eventType == (int)LiveGameEventType.ScoringChance ||
            eventType == (int)LiveGameEventType.Header ||
            eventType == (int)LiveGameEventType.FreeKick ||
            eventType == (int)LiveGameEventType.Post ||
            eventType == (int)LiveGameEventType.Miss ||
            eventType == (int)LiveGameEventType.CornerKick ||
            eventType == (int)LiveGameEventType.KeeperPushedInCorner ||
            eventType == (int)LiveGameEventType.PlayerPushedInCorner ||
            eventType == (int)LiveGameEventType.CrossBar ||
            eventType == (int)LiveGameEventType.Save)
            returnValue = 1;
        else if (eventType == (int)LiveGameEventType.SendOff ||
            eventType == (int)LiveGameEventType.Injury)
            returnValue = 2;
        else if (eventType == (int)LiveGameEventType.FoulRed ||
            eventType == (int)LiveGameEventType.FoulYellow ||
            eventType == (int)LiveGameEventType.FoulRedInjury)
            returnValue = 3;
        else if (eventType == (int)LiveGameEventType.Foul ||
            eventType == (int)LiveGameEventType.TempInjury ||
            eventType == (int)LiveGameEventType.StadiumRiots ||
            eventType == (int)LiveGameEventType.StadiumBreakdown ||
            eventType == (int)LiveGameEventType.StadiumFire ||
            eventType == (int)LiveGameEventType.LostPossession)
            returnValue = 4;
        else if (eventType == (int)LiveGameEventType.Substitution ||
            eventType == (int)LiveGameEventType.StructureChange ||
            eventType == (int)LiveGameEventType.StyleChangeAllOutDefend ||
            eventType == (int)LiveGameEventType.StyleChangeDefend ||
            eventType == (int)LiveGameEventType.StyleChangeBalanced ||
            eventType == (int)LiveGameEventType.StyleChangeAttack ||
            eventType == (int)LiveGameEventType.StyleChangeAllOutAttack ||
            eventType == (int)LiveGameEventType.BoostChangeAnti ||
            eventType == (int)LiveGameEventType.BoostChangeOff ||
            eventType == (int)LiveGameEventType.BoostChangeOn)
            returnValue = 5;
        else if (eventType == (int)LiveGameEventType.General)
            returnValue = 6;
        else
            Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                "live_game_event_get_verbosity: unknown event type {0} ", eventType);

        return returnValue;
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

    /// <summary>
    /// Return the live game unit 'gap' places before or after a given one.
    /// Corresponds to C: live_game_unit_before.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <param name="unit">The unit we start from.</param>
    /// <param name="gap">The number of places to move (positive: earlier, negative: later).</param>
    /// <returns>The neighbour unit or null if it's out of bounds.</returns>
    public static LiveGameUnit UnitBefore(LiveGame liveGame, LiveGameUnit unit, int gap)
    {
        int i;

        for (i = liveGame.Units.Count - 1; i >= 0; i--)
        {
            if (!ReferenceEquals(liveGame.Units[i], unit))
                continue;

            if (gap > 0)
            {
                if (i - gap > 0)
                    return liveGame.Units[i - gap];

                Debug.DebugPrintMessage("live_game_unit_before: no unit found for gap {0}", gap);
            }
            else
            {
                if (i + gap < liveGame.Units.Count - 1)
                    return liveGame.Units[i + gap];

                Debug.DebugPrintMessage("live_game_unit_before: no unit found for gap {0}", gap);
            }
        }

        return null;
    }
}
