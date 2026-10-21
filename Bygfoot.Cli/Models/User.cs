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
    public List<object>[] Bets { get; set; } = new List<object>[2];

    /// <summary>An array of gint that will be used to store the default team of a user. Corresponds to C: GArray *default_team.</summary>
    public List<int> DefaultTeam { get; set; } = new List<int>();

    /// <summary>The default structure of a user team. Corresponds to C: gint default_structure.</summary>
    public int DefaultStructure { get; set; }

    /// <summary>The default playing style of a user team. Corresponds to C: gint default_style.</summary>
    public int DefaultStyle { get; set; }

    /// <summary>The default boost of a user team. Corresponds to C: gint default_boost.</summary}
    public int DefaultBoost { get; set; }
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
