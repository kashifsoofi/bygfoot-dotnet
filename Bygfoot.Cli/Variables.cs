namespace Bygfoot.Cli;

using Models;

/// <summary>
/// The global variables of the game.
/// Corresponds to the C global variables in src/variables.h.
/// </summary>
public static class Variables
{
    /// <summary>The main variable of the game. Corresponds to C: Country country.</summary>
    public static Country Country { get; set; } = new Country();

    /// <summary>The array of human players. Corresponds to C: GArray *users.</summary>
    public static List<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// The season, week and week round numbers.
    /// We keep track of the time in the game with these variables.
    /// Corresponds to C: gint season, week, week_round.
    /// </summary>
    public static int Season { get; set; }
    public static int Week { get; set; }
    public static int WeekRound { get; set; }

    /// <summary>Array of options that get read from bygfoot.conf. Corresponds to C: OptionList options.</summary>
    public static OptionList Options { get; set; } = new OptionList();

    /// <summary>Array of constants that get read from the constants file specified in bygfoot.conf.
    /// Corresponds to C: OptionList constants.</summary>
    public static OptionList Constants { get; set; } = new OptionList();

    /// <summary>Array of constants affecting game appearance rather than behaviour.
    /// Corresponds to C: OptionList constants_app.</summary>
    public static OptionList ConstantsApp { get; set; } = new OptionList();

    /// <summary>Array with internal settings. Corresponds to C: OptionList settings.</summary>
    public static OptionList Settings { get; set; } = new OptionList();

    /// <summary>Array holding string replacement tokens. Corresponds to C: OptionList tokens.</summary>
    public static OptionList Tokens { get; set; } = new OptionList();

    /// <summary>The array containing the live game commentary strings.
    /// Corresponds to C: GArray *lg_commentary[LIVE_GAME_EVENT_END].</summary>
    public static List<LgCommentary>[] LgCommentary { get; set; } =
        new List<LgCommentary>[(int)LiveGameEventType.End];

    /// <summary>The array containing the news article strings.
    /// Corresponds to C: GArray *news[NEWS_ARTICLE_TYPE_END].</summary>
    public static List<NewsArticle>[] News { get; set; } =
        new List<NewsArticle>[(int)NewsArticleType.End];

    /// <summary>Newspaper containing the news articles. Corresponds to C: NewsPaper newspaper.</summary>
    public static NewsPaper Newspaper { get; set; } = new NewsPaper();

    /// <summary>The array containing players to be transfered. Corresponds to C: GArray *transfer_list.</summary>
    public static List<Transfer> TransferList { get; set; } = new List<Transfer>();

    /// <summary>Array with season statistics (updated at the end of each season).
    /// Corresponds to C: GArray *season_stats.</summary>
    public static List<Stat> SeasonStats { get; set; } = new List<Stat>();

    /// <summary>Array of available CPU strategies. Corresponds to C: GArray *strategies.</summary>
    public static List<Strategy> Strategies { get; set; } = new List<Strategy>();

    /// <summary>Array of current and recent bets. Corresponds to C: GArray *bets[2].</summary>
    public static List<BetMatch>[] Bets { get; set; } = new List<BetMatch>[2];

    /// <summary>Loan interest for the current week. Corresponds to C: gfloat current_interest.</summary>
    public static float CurrentInterest { get; set; }

    /// <summary>Array of jobs in the job exchange and teams going with the international jobs.
    /// Corresponds to C: GArray *jobs, *job_teams.</summary>
    public static List<Job> Jobs { get; set; } = new List<Job>();
    public static List<Team> JobTeams { get; set; } = new List<Team>();

    /// <summary>Some counters we use. Corresponds to C: gint counters[COUNT_END].</summary>
    public static int[] Counters { get; set; } = new int[(int)global::Bygfoot.Cli.Counters.CountEnd];

    /// <summary>These help us keep track of what's happening. Corresponds to C: gint status[6].</summary>
    public static int[] Status { get; set; } = new int[6];

    /// <summary>A pointer we store temporary stuff in. Corresponds to C: gpointer statp.</summary>
    public static object Statp { get; set; }

    /// <summary>The currently selected row in the treeview. Corresponds to C: gint selected_row.</summary>
    public static int SelectedRow { get; set; }

    /// <summary>An array of name lists. Corresponds to C: GArray *name_lists.</summary>
    public static List<NameList> NameLists { get; set; } = new List<NameList>();

    /// <summary>The struct containing the window pointers. Corresponds to C: Windows window.</summary>
    public static Windows Window { get; set; } = new Windows();

    /// <summary>The variables for non-user live games (which aren't shown). Corresponds to C: GArray *live_games.</summary>
    public static List<LiveGame> LiveGames { get; set; } = new List<LiveGame>();

    /// <summary>The index of the current user in the users array. Corresponds to C: gint cur_user.</summary>
    public static int CurUser { get; set; }

    /// <summary>Corresponds to C: gint timeout_id.</summary>
    public static int TimeoutId { get; set; }

    /// <summary>Corresponds to C: GRand *rand_generator.</summary>
    public static Random RandGenerator { get; set; } = new Random();

    /// <summary>Debug information. Corresponds to C: gint debug_level, debug_output.</summary>
    public static int DebugLevel { get; set; }
    public static int DebugOutput { get; set; }

    /// <summary>
    /// The list of directories the file_find_support_file() function
    /// searches for support files (e.g. pixmaps or text files).
    /// Corresponds to C: GList *support_directories.
    /// </summary>
    public static List<string> SupportDirectories { get; set; } = new List<string>();

    /// <summary>The list of root defintions directories found (ending in definitions).
    /// Corresponds to C: GList *root_definitions_directories.</summary>
    public static List<string> RootDefinitionsDirectories { get; set; } = new List<string>();

    /// <summary>The list of defintions directories found. Corresponds to C: GList *definitions_directories.</summary>
    public static List<string> DefinitionsDirectories { get; set; } = new List<string>();

    /// <summary>The name of the current save file (gets updated when a game is saved or loaded).
    /// Corresponds to C: gchar *save_file.</summary>
    public static string SaveFile { get; set; }

    /// <summary>Whether we are using a Unix system or Windows. Corresponds to C: gboolean os_is_unix.</summary>
    public static bool OsIsUnix { get; set; }

    /// <summary>The hints displayed in the splash screen. Corresponds to C: OptionList hints.</summary>
    public static OptionList Hints { get; set; } = new OptionList();
}
