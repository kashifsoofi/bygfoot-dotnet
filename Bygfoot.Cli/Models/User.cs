namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Indices for the money_in array. Corresponds to C enum MonIn.
/// </summary>
public enum MoneyInType
{
    Prize = 0,
    Ticket,
    Sponsor,
    Bets,
    Transfers,
    End
}

/// <summary>
/// Indices for the money_out array. Corresponds to C enum MonOut.
/// </summary>
public enum MoneyOutType
{
    Wage = 0,
    Physio,
    Scout,
    YellowCard,
    YouthAcademy,
    Journey,
    Compensations,
    Bets,
    Boost,
    Transfers,
    StadiumImprovement,
    StadiumBills,
    TrainingCamp,
    End
}

/// <summary>
/// Indices for the counters variable in #User. Corresponds to C enum CounterValue.
/// </summary>
public enum CounterType
{
    Loan = 0,               // COUNT_USER_LOAN
    Overdrawn,              // COUNT_USER_OVERDRAWN
    Positive,               // COUNT_USER_POSITIVE
    Success,                // COUNT_USER_SUCCESS
    Warning,                // COUNT_USER_WARNING
    IncreaseCapacity,       // COUNT_USER_INC_CAP
    IncreaseSafety,         // COUNT_USER_INC_SAF
    StadiumCapacity,        // COUNT_USER_STADIUM_CAPACITY
    StadiumSafety,          // COUNT_USER_STADIUM_SAFETY
    ShowResult,             // COUNT_USER_SHOW_RES
    TookTurn,               // COUNT_USER_TOOK_TURN
    NewSponsor,             // COUNT_USER_NEW_SPONSOR
    TrainingCampThisWeek,   // COUNT_USER_TRAININGS_WEEK
    TrainingCampsLeftSeason,// COUNT_USER_TRAININGS_LEFT_SEASON
    End                     // COUNT_USER_END
}

/// <summary>
/// User-related things that get recorded. Corresponds to C enum UserHistoryType.
/// </summary>
public enum UserHistoryType
{
    StartGame = 0,          // USER_HISTORY_START_GAME
    FireFinance,            // USER_HISTORY_FIRE_FINANCE
    FireFailure,            // USER_HISTORY_FIRE_FAILURE
    JobOfferAccepted,       // USER_HISTORY_JOB_OFFER_ACCEPTED
    EndSeason,              // USER_HISTORY_END_SEASON
    Promoted,               // USER_HISTORY_PROMOTED
    Relegated,              // USER_HISTORY_RELEGATED
    WinFinal,               // USER_HISTORY_WIN_FINAL
    LoseFinal,              // USER_HISTORY_LOSE_FINAL
    ReachCupRound,          // USER_HISTORY_REACH_CUP_ROUND
    Champion,               // USER_HISTORY_CHAMPION
    End                     // USER_HISTORY_END
}

/// <summary>
/// A memorable match (a recording of a live game) of a user. 
/// Corresponds to C struct MemMatch.
/// </summary>
public class MemMatch
{
    /// <summary>Name of the country the user was playing with. Corresponds to C: gchar *country_name.</summary>
    public string CountryName { get; set; }

    /// <summary>The name of the competition, including the cup round name. Corresponds to C: GString *competition_name.</summary>
    public string CompetitionName { get; set; }

    /// <summary>Whether the match was on neutral ground. Only relevant for MM list display. Corresponds to C: gboolean neutral.</summary>
    public bool Neutral { get; set; }

    /// <summary>0 or 1, indicating which team was the user's. Corresponds to C: gint user_team.</summary>
    public int UserTeam { get; set; }

    /// <summary>The recording. Corresponds to C: LiveGame lg.</summary>
    public LiveGame LiveGame { get; set; }
}

/// <summary>
/// A structure holding an element of a user's history, e.g. the event of being fired. 
/// Corhand to C struct UserHistory.
/// </summary>
public class UserHistory
{
    /// <summary>When the event happened. Corresponds to C: gint season, week.</summary>
    public int Season { get; set; }

    /// <summary>The week the event happened. Corresponds to C: gint season, week.</summary>
    public int Week { get; set; }

    /// <summary>The type (see #UserHistoryType) of the history event. Corresponds to C: gint type.</summary>
    public UserHistoryType Type { get; set; }

    /// <summary>The team of the user at the time. Corresponds to C: gchar *team_name.</summary>
    public string TeamName { get; set; }

    /// <summary>These can hold various information like team or league/cup ids. Corresponds to C: gchar *string[3].</summary>
    public string[] ContextStrings { get; set; } = new string[3];
}

/// <summary>
/// A user sponsor. Corresponds to C struct UserSponsor.
/// </summary>
public class UserSponsor
{
    /// <summary>Name of the sponsor. Corresponds to C: GString *name.</summary>
    public string Name { get; set; }

    /// <summary>Money per week. Corresponds to C: gint benefit.</summary>
    public int Benefit { get; set; }

    /// <summary>Contract length in weeks. Corresponds to C: gint contract.</summary>
    public int Contract { get; set; }
}

/// <summary>
/// A structure representing a human player. 
/// Corresponds to C struct User.
/// </summary>
public class User
{
    /// <summary>Username. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>The team the user manages. Corresponds to C: Team *tm.</summary>
    public Team Team { get; set; }

    /// <summary>The team id (needed when the team pointer gets invalid). Corresponds to C: gint team_id.</summary>
    public int TeamId { get; set; }

    /// <summary>User options. Corresponds to C: OptionList options.</summary>
    public OptionList Options { get; set; } = new OptionList();

    /// <summary>Events shown each week. Corresponds to C: GArray *events.</summary>
    public List<Event> Events { get; set; } = new List<Event>();

    /// <summary>User history. Corresponds to C: GArray *history.</summary>
    public List<UserHistory> History { get; set; } = new List<UserHistory>();

    /// <summary>
    /// User counters (not changeable by the user), like number of weeks until debt has to be paid back. 
    /// Corresponds to C: gint counters[COUNT_USER_END].
    /// </summary>
    public int[] Counters { get; set; } = new int[15]; // COUNT_USER_END = 15

    /// <summary>
    /// The user's money, debt, income and expenses. 
    /// We have double arrays to store information about the current and the past week. 
    /// Corresponds to C: gint money, debt, money_in[2][MON_IN_END], money_out[2][MON_OUT_END].
    /// </summary>
    public int Money { get; set; }
    public int Debt { get; set; }
    public int[,] MoneyIn { get; set; } = new int[2, 6]; // MON_IN_END = 6
    public int[,] MoneyOut { get; set; } = new int[2, 14]; // MON_OUT_END = 14

    /// <summary>Interest the debt was taken at. Corresponds to C: gfloat debt_interest.</summary>
    public float DebtInterest { get; set; }

    /// <summary>Information about the automatic loan repayment. Corresponds to C: gint alr_start_week, alr_weekly_installment.</summary>
    public int AlrStartWeek { get; set; }
    public int AlrWeeklyInstallment { get; set; }

    /// <summary>The user's scout and physio qualities. Corresponds to C: gint scout, physio.</summary>
    public int Scout { get; set; }
    public int Physio { get; set; }

    /// <summary>The variable for the latest user live game. Corresponds to C: LiveGame live_game.</summary>
    public LiveGame LiveGame { get; set; }

    /// <summary>Sponsor of the user. Corresponds to C: UserSponsor sponsor.</summary>
    public UserSponsor Sponsor { get; set; }

    /// <summary>Youth academy of the user. Corresponds to C: YouthAcademy youth_academy.</summary>
    public YouthAcademy YouthAcademy { get; set; }

    /// <summary>The currently used MM file. Corresponds to C: gchar *mmatches_file.</summary>
    public string MmatchesFile { get; set; }

    /// <summary>The array of MMs. Corresponds to C: GArray *mmatches.</summary>
    public List<MemMatch> Mmatches { get; set; } = new List<MemMatch>();

    /// <summary>Array of current and recent bets. Corresponds to C: GArray *bets[2].</summary>
    public List<BetUser>[] Bets { get; set; } = new List<BetUser>[2];

    /// <summary>An array of gint that will be used to store the default team of a user. Corresponds to C: GArray *default_team.</summary>
    public List<int> DefaultTeam { get; set; } = new List<int>();

    /// <summary>The default structure of a user team. Corresponds to C: gint default_structure.</summary>
    public int DefaultStructure { get; set; }

    /// <summary>The default playing style of a user team. Corresponds to C: gint default_style.</summary>
    public int DefaultStyle { get; set; }

    /// <summary>The default boost of a user team. Corresponds to C: gint default_boost.</summary}
    public int DefaultBoost { get; set; }

    /// <summary>
    /// Create a new user with default values.
    /// Corresponds to C: user_new.
    /// </summary>
    /// <returns>A new user.</returns>
    public static User New()
    {
        User newUser = new User();
        newUser.Name = "NONAME";
        newUser.TeamId = -1;
        newUser.LiveGame = new LiveGame();
        newUser.Events = new List<Event>();
        newUser.History = new List<UserHistory>();
        newUser.Options = new OptionList();
        newUser.Sponsor = new UserSponsor();
        newUser.Sponsor.Contract = -1;
        newUser.Sponsor.Benefit = -1;
        newUser.YouthAcademy = new YouthAcademy();
        newUser.Mmatches = new List<MemMatch>();
        newUser.Bets = new List<BetUser>[2];
        newUser.DefaultTeam = new List<int>();
        newUser.DefaultStructure = 0;
        newUser.DefaultStyle = 0;
        newUser.DefaultBoost = 0;

        return newUser;
    }

    /// <summary>
    /// Set up finances, remove some players etc. for a new user team.
    /// Corresponds to C: user_set_up_finances.
    /// </summary>
    /// <param name="user">The user whose team we set up.</param>
    public static void SetUpFinances(User user)
    {
        for (int i = 0; i < 14; i++)
        {
            user.MoneyOut[0, i] = user.MoneyOut[1, i];
            user.MoneyOut[1, i] = 0;
        }

        for (int i = 0; i < 6; i++)
        {
            user.MoneyIn[0, i] = user.MoneyIn[1, i];
            user.MoneyIn[1, i] = 0;
        }

        user.Debt = 0;
        user.DebtInterest = 0;
        user.AlrStartWeek = 0;
        user.AlrWeeklyInstallment = 0;
        user.Money = 0;
    }

    /// <summary>
    /// Set the counters of the user to their initial values.
    /// Corresponds to C: user_set_up_counters.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void SetUpCounters(User user)
    {
        for (int i = 0; i < 15; i++)
            user.Counters[i] = 0;

        user.Counters[(int)CounterType.Loan] = -1;
        user.Counters[(int)CounterType.Positive] = -1;
        user.Counters[(int)CounterType.TrainingCampsLeftSeason] = Variables.Constants.GetInt("int_training_camps_per_season");
    }

    /// <summary>
    /// Remove a user from the game.
    /// Corresponds to C: user_remove.
    /// </summary>
    /// <param name="idx">The index of the user in the users array.</param>
    /// <param name="regenerateTeam">Whether the user's team has to be regenerated.</param>
    public static void Remove(int idx, bool regenerateTeam)
    {
        // Implementation depends on the not yet converted user, team and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the player list attributes of the user.
    /// Corresponds to C: user_set_player_list_attributes.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="attribute">The player list attributes.</param>
    /// <param name="listNumber">The list number.</param>
    public static void SetPlayerListAttributes(User user, PlayerListAttribute[] attribute, int listNumber)
    {
        // Implementation depends on the not yet converted option and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the specified user has a match at the specified date.
    /// Corresponds to C: query_user_game_in_week_round.
    /// </summary>
    /// <param name="usrIdx">The user index.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round number.</param>
    /// <returns>Whether the user has a game.</returns>
    public static bool QueryUserGameInWeekRound(int usrIdx, int weekNumber, int weekRoundNumber)
    {
        User user = Variables.Users[usrIdx];

        if (user.Team != null && user.Team.Clid < Bygfoot.IdCupStart)
        {
            League league = null;
            foreach (League candidate in Variables.Country.Leagues)
            {
                if (candidate.Id == user.Team.Clid)
                {
                    league = candidate;
                    break;
                }
            }

            if (league == null)
                return false;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber &&
                    fixture.WeekRoundNumber == weekRoundNumber &&
                    (fixture.Teams[0] == user.Team || fixture.Teams[1] == user.Team))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Find out whether there are user games at the specified date.
    /// Corresponds to C: query_user_games_in_week_round.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round number.</param>
    /// <returns>Whether there are user games.</returns>
    public static bool QueryUserGamesInWeekRound(int weekNumber, int weekRoundNumber)
    {
        foreach (User user in Variables.Users)
        {
            if (QueryUserGameInWeekRound(Variables.CurUser, weekNumber, weekRoundNumber))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Get the user managing the team.
    /// Corresponds to C: user_from_team.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The user or null.</returns>
    public static User FromTeam(Team tm)
    {
        // Implementation depends on the not yet converted user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check the success counter of the user and offer a new job or fire him if the counter says so.
    /// Corresponds to C: user_job_offer.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void JobOffer(User user)
    {
        // Implementation depends on the not yet converted user, team and finance functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update the counters of the users.
    /// Corresponds to C: user_weekly_update_counters.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void WeeklyUpdateCounters(User user)
    {
        // Implementation depends on the not yet converted league and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return a default new user event.
    /// Corresponds to C: user_event_new.
    /// </summary>
    /// <returns>A new user event.</returns>
    public static Event EventNew()
    {
        Event newEvent = new Event();
        newEvent.User = null;
        newEvent.Type = EventType.Warning;
        newEvent.Value1 = -1;
        newEvent.Value2 = -1;
        newEvent.ValuePointer = null;
        newEvent.ValueString = null;

        return newEvent;
    }

    /// <summary>
    /// Add an event with the specified values to the event array of the user.
    /// Corresponds to C: user_event_add.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="type">The event type.</param>
    /// <param name="value1">The value1.</param>
    /// <param name="value2">The value2.</param>
    /// <param name="valuePointer">The value pointer.</param>
    /// <param name="format">The format.</param>
    public static void EventAdd(User user, int type, int value1, int value2, object valuePointer, string format, params object[] args)
    {
        // Implementation depends on the not yet converted user and game_gui functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove an event from the user event array.
    /// Corresponds to C: user_event_remove.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="idx">The index of the event in the array.</param>
    public static void EventRemove(User user, int idx)
    {
        user.Events.RemoveAt(idx);
    }

    /// <summary>
    /// Show the next event for the current user.
    /// Corresponds to C: user_event_show_next.
    /// </summary>
    public static void EventShowNext()
    {
        // Implementation depends on the not yet converted user and game_gui functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there are user events.
    /// Corresponds to C: query_user_events.
    /// </summary>
    /// <returns>Whether there are events.</returns>
    public static bool QueryUserEvents()
    {
        foreach (User user in Variables.Users)
        {
            if (user.Events.Count != 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Change the team of a user.
    /// Corresponds to C: user_change_team.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="tm">The team.</param>
    public static void ChangeTeam(User user, Team tm)
    {
        // Implementation depends on the not yet converted user, team and game_gui functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add an element to the user history filled with the given values.
    /// Corresponds to C: user_history_add.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="type">The history type.</param>
    /// <param name="teamName">The team name.</param>
    /// <param name="string0">The string0.</param>
    /// <param name="string1">The string1.</param>
    /// <param name="string2">The string2.</param>
    public static void HistoryAdd(User user, int type, string teamName, string string0, string string1, string string2)
    {
        // Implementation depends on the not yet converted user and misc functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write the text corresponding to the history event into the buffer.
    /// Corresponds to C: user_history_to_string.
    /// </summary>
    /// <param name="history">The history.</param>
    /// <param name="buf">The buffer.</param>
    public static void HistoryToString(UserHistory history, ref string buf)
    {
        // Implementation depends on the not yet converted user and misc functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compare function for history sorting.
    /// Corresponds to C: user_history_compare.
    /// </summary>
    /// <param name="a">The first history.</param>
    /// <param name="b">The second history.</param>
    /// <returns>The comparison result.</returns>
    public static int HistoryCompare(object a, object b)
    {
        // Implementation depends on the not yet converted user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Increase the user success counter when the user is successful in a cup.
    /// Corresponds to C: user_add_cup_success.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The round.</param>
    /// <param name="type">The history type.</param>
    public static void AddCupSuccess(User user, Cup cup, int round, int type)
    {
        // Implementation depends on the not yet converted league, cup and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there's a user who didn't have his turn before a new week round begins.
    /// Corresponds to C: query_user_no_turn.
    /// </summary>
    /// <returns>Whether there is no turn.</returns>
    public static bool QueryUserNoTurn()
    {
        foreach (User user in Variables.Users)
        {
            if (user.Counters[(int)CounterType.TookTurn] == 0)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Get a random sponsor for the user.
    /// Corresponds to C: user_get_sponsor.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>The sponsor.</returns>
    public static UserSponsor GetSponsor(User user)
    {
        // Implementation depends on the not yet converted finance, maths and name functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Show a list of sponsors for the user to choose from.
    /// Corresponds to C: user_show_sponsors.
    /// </summary>
    public static void ShowSponsors()
    {
        // Implementation depends on the not yet converted user, treeview and window functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// The current sponsor offers to continue.
    /// Corresponds to C: user_show_sponsor_continue.
    /// </summary>
    public static void ShowSponsorContinue()
    {
        // Implementation depends on the not yet converted user and window functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load a MM file.
    /// Corresponds to C: user_mm_load_file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="mmatches">The matches.</param>
    public static void MMLoadFile(string filename, List<MemMatch> mmatches)
    {
        // Implementation depends on the not yet converted mmatches and file functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add the last match to the MM file.
    /// Corresponds to C: user_mm_add_last_match.
    /// </summary>
    /// <param name="loadFile">Whether to load the matches from the file first.</param>
    /// <param name="saveFile">Whether to save the MM array to file afterwards.</param>
    public static void MMAddLastMatch(bool loadFile, bool saveFile)
    {
        // Implementation depends on the not yet converted mmatches, game and user functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Save an MM file.
    /// Corresponds to C: user_mm_save_file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="mmatches">The matches.</param>
    public static void MMSaveFile(string filename, List<MemMatch> mmatches)
    {
        // Implementation depends on the not yet converted mmatches and file functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set an appropriate filename for the memorable matches file.
    /// Corresponds to C: user_mm_set_filename.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="dest">The destination.</param>
    public static void MMSetFilename(string filename, string dest)
    {
        // Implementation depends on the not yet converted misc functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Import an MM file into the user's MMs.
    /// Corresponds to C: user_mm_import_file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    public static void MMImportFile(string filename)
    {
        // Implementation depends on the not yet converted mmatches and file functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Export a match to a file.
    /// Corresponds to C: user_mm_export_file.
    /// </summary>
    /// <param name="filename">The filename.</param>
    public static void MMExportFile(string filename)
    {
        // Implementation depends on the not yet converted mmatches and file functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// This will store the default team of a user.
    /// Corresponds to C: store_default_team.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void StoreDefaultTeam(User user)
    {
        user.DefaultTeam = new List<int>();

        foreach (Player player in user.Team.Players)
        {
            user.DefaultTeam.Add(player.Id);
        }

        user.DefaultStructure = user.Team.Structure;
        user.DefaultStyle = user.Team.Style;
        user.DefaultBoost = user.Team.Boost;
    }

    /// <summary>
    /// This will restore the default team.
    /// Corresponds to C: restore_default_team.
    /// </summary>
    /// <param name="user">The user.</param>
    public static void RestoreDefaultTeam(User user)
    {
        for (int i = 0; i < user.Team.Players.Count; i++)
        {
            // Implementation depends on the not yet converted player and team functions.
        }

        // team_change_structure(user->tm, user->default_structure);
        // team_change_attribute_with_message(user->tm, TEAM_ATTRIBUTE_STYLE, user->default_style);
        // team_change_attribute_with_message(user->tm, TEAM_ATTRIBUTE_BOOST, user->default_boost);
        // game_gui_write_meters(current_user.tm);
        // game_gui_write_radio_items;
    }
}

/// <summary>
/// Types of events for a user. Corresponds to C enum EventType.
/// </summary>
public enum EventType
{
    Warning = 0,                  // EVENT_TYPE_WARNING
    PlayerLeft,                   // EVENT_TYPE_PLAYER_LEFT
    Payback,                      // EVENT_TYPE_PAYBACK
    Overdraw,                     // EVENT_TYPE_OVERDRAW
    JobOffer,                     // EVENT_TYPE_JOB_OFFER
    FireFinance,                  // EVENT_TYPE_FIRE_FINANCE
    FireFailure,                  // EVENT_TYPE_FIRE_FAILURE
    TransferOfferUser,            // EVENT_TYPE_TRANSFER_OFFER_USER
    TransferOfferCpu,             // EVENT_TYPE_TRANSFER_OFFER_CPU
    TransferOfferRejectedBetterOffer, // EVENT_TYPE_TRANSFER_OFFER_REJECTED_BETTER_OFFER
    TransferOfferRejectedWage,    // EVENT_TYPE_TRANSFER_OFFER_REJECTED_WAGE
    TransferOfferRejectedFee,     // EVENT_TYPE_TRANSFER_OFFER_REJECTED_FEE
    TransferOfferRejectedStars,   // EVENT_TYPE_TRANSFER_OFFER_REJECTED_STARS
    TransferOfferMoney,           // EVENT_TYPE_TRANSFER_OFFER_MONEY
    TransferOfferRoster,          // EVENT_TYPE_TRANSFER_OFFER_ROSTER
    PlayerCareerStop,             // EVENT_TYPE_PLAYER_CAREER_STOP
    Charity,                      // EVENT_TYPE_CHARITY
    End                           // EVENT_TYPE_END
}

/// <summary>
/// A structure representing an event for a user. This is used to show information like a successful transfer or a job offer. 
/// Corresponds to C struct Event.
/// </summary>
public class Event
{
    /// <summary>Pointer to the user the event belongs to. Corresponds to C: User *user.</summary>
    public User User { get; set; }

    /// <summary>Type of the event. See #EventType. Corresponds to C: gint type.</summary>
    public EventType Type { get; set; }

    /// <summary>Some values that are used for different purposes. Corresponds to C: gint value1, value2.</summary>
    public int Value1 { get; set; }
    public int Value2 { get; set; }

    /// <summary>A pointer for different purposes. Corresponds to C: gpointer value_pointer.</summary>
    public object ValuePointer { get; set; }

    /// <summary>A string for different purposes. Corresponds to C: gchar *value_string.</summary>
    public string ValueString { get; set; }
}
