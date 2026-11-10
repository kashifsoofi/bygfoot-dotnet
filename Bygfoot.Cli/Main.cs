using System.Reflection;

namespace Bygfoot.Cli;

/// <summary>
/// Static class for the program's main entry logic.
/// Corresponds to the C module src/main.h and src/main.c
/// (main_parse_cl_arguments, main_parse_debug_cl_arguments,
/// main_init_variables, main_init, c_main, main_exit_program).
/// </summary>
public static class Main
{
    /// <summary>Whether the last save gets loaded at startup (cl switch -l).
    /// Corresponds to C: gboolean load_last_save.</summary>
    public static bool LoadLastSave { get; set; }

    // C: #define DEBUG_LEVEL_DEFAULT 0
    private const int DebugLevelDefault = 0;

    // C: MAC_BUILD is defined at build time when building the Mac OS X app
    // bundle (Xcode). This console build is not a bundle.
    private const bool MacBuild = false;

    // C: DEBUG_OUTPUT_DEFAULT is 2 when MAC_BUILD or G_OS_WIN32, else 0.
    private static int DebugOutputDefault =>
        OperatingSystem.IsMacOS() || OperatingSystem.IsWindows() ? 2 : 0;

    // C: PACKAGE_DATA_DIR "/" PACKAGE "/support_files" — PACKAGE_DATA_DIR is
    // autotools' ${prefix}/share (default /usr/local/share), PACKAGE is "bygfoot".
    private const string PackageDataSupportDir = "/usr/local/share/bygfoot/support_files";

    /// <summary>
    /// Returns the value belonging to an option, given either as --opt=value or as
    /// the next command line argument (advancing <paramref name="i"/> in that case).
    /// </summary>
    /// <param name="argv">The command line arguments.</param>
    /// <param name="i">The current argument index (advanced when the value is the next argument).</param>
    /// <param name="arg">The option argument (e.g. --support-dir or --support-dir=DIR).</param>
    /// <returns>The option value, or null if no value is available.</returns>
    private static string? GetOptionValue(string[] argv, ref int i, string arg)
    {
        int eq = arg.IndexOf('=');
        if (eq >= 0)
            return arg[(eq + 1)..];
        if (i + 1 < argv.Length)
        {
            i++;
            return argv[i];
        }
        return null;
    }

    /// <summary>
    /// Parse the command line arguments given by the user.
    /// Corresponds to C: main_parse_cl_arguments(gint *argc, gchar ***argv).
    /// Recognized options are removed from the argument list (as GOption does in C).
    /// </summary>
    /// <param name="argc">Number of command line arguments (reduced by the parsed options).</param>
    /// <param name="argv">Command line arguments array (reduced by the parsed options).</param>
    public static void MainParseClArguments(ref int argc, ref string[] argv)
    {
        bool testcom = false, calodds = false;
        string? supportDir = null, lang = null, testcomFile = null, tokenFile = null;
        string? eventName = null, countrySid = null;
        int numberOfPasses = 1, numMatches = 100, skilldiffmax = 20;

        List<string> kept = new List<string>();
        for (int i = 1; i < argv.Length; i++)
        {
            string arg = argv[i];
            int eq = arg.IndexOf('=');
            string name = eq >= 0 ? arg[..eq] : arg;
            string? value;

            switch (name)
            {
                case "-l": case "--last-save":
                    LoadLastSave = true;
                    break;
                case "-s": case "--support-dir":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    supportDir = value;
                    break;
                case "-c": case "--country":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    countrySid = value;
                    break;
                case "-L": case "--lang":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    lang = value;
                    break;
                case "-t": case "--testcom":
                    testcom = true;
                    break;
                case "-C": case "--commentary-file":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    testcomFile = value;
                    break;
                case "-T": case "--token-file":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    tokenFile = value;
                    break;
                case "-e": case "--event-name":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    eventName = value;
                    break;
                case "-n": case "--num-passes":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null || !int.TryParse(value, out numberOfPasses))
                    {
                        Misc.MiscPrintError(new ArgumentException($"Invalid integer value for option {name}."), false);
                        return;
                    }
                    break;
                case "-O": case "--calodds":
                    calodds = true;
                    break;
                case "-m": case "--num-matches":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null || !int.TryParse(value, out numMatches))
                    {
                        Misc.MiscPrintError(new ArgumentException($"Invalid integer value for option {name}."), false);
                        return;
                    }
                    break;
                case "-S": case "--num-skilldiff":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null || !int.TryParse(value, out skilldiffmax))
                    {
                        Misc.MiscPrintError(new ArgumentException($"Invalid integer value for option {name}."), false);
                        return;
                    }
                    break;
                default:
                    if (name.StartsWith('-') && name.Length > 1)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Unknown option: {arg}."), false);
                        return;
                    }
                    kept.Add(arg);
                    break;
            }
        }

        // Rebuild the argument list without the parsed options (as GOption does in C).
        string[] newArgv = new string[kept.Count + 1];
        newArgv[0] = argv[0];
        for (int k = 0; k < kept.Count; k++)
            newArgv[k + 1] = kept[k];
        argv = newArgv;
        argc = kept.Count + 1;

        if (calodds)
        {
            Variables.Options.Add("int_opt_calodds", 1);
            Variables.Options.Add("int_opt_calodds_skilldiffmax", skilldiffmax);
            Variables.Options.Add("int_opt_calodds_matches", numMatches);
        }

        if (testcom)
        {
            // TODO: Call the C function lg_commentary_test(testcomFile, tokenFile,
            // eventName, numberOfPasses) (not yet converted).
            MainExitProgram(Bygfoot.ExitCode.Ok, null);
        }

        if (supportDir != null)
        {
            // C: (support_dir[strlen(support_dir)] == G_DIR_SEPARATOR) reads the null
            // terminator and is always false, so a separator is always appended.
            string fullpath = supportDir + Path.DirectorySeparatorChar;
            FileHelper.FileAddSupportDirectoryRecursive(fullpath);
        }

        if (lang != null)
        {
            Language.LanguageSet(Language.LanguageGetCodeIndex(lang) + 1);
            FileHelper.LoadHintsFile();
        }

        if (countrySid != null)
            Variables.Country.SId = countrySid;
    }

    /// <summary>
    /// Parse the debug command line arguments given by the user.
    /// Corresponds to C: main_parse_debug_cl_arguments(gint *argc, gchar ***argv).
    /// Unknown options are ignored and kept in the argument list (as in C).
    /// </summary>
    /// <param name="argc">Number of command line arguments (reduced by the parsed options).</param>
    /// <param name="argv">Command line arguments array (reduced by the parsed options).</param>
    public static void MainParseDebugClArguments(ref int argc, ref string[] argv)
    {
        int debLevel = -1, debOutput = -1;
        string? debugText = null;

        Variables.DebugLevel = DebugLevelDefault;
        Variables.DebugOutput = DebugOutputDefault;

        List<string> kept = new List<string>();
        for (int i = 1; i < argv.Length; i++)
        {
            string arg = argv[i];
            int eq = arg.IndexOf('=');
            string name = eq >= 0 ? arg[..eq] : arg;
            string? value;

            switch (name)
            {
                case "-d": case "--debug-level":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null || !int.TryParse(value, out debLevel))
                    {
                        Misc.MiscPrintError(new ArgumentException($"Invalid integer value for option {name}."), false);
                        return;
                    }
                    break;
                case "-o": case "--debug-output":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null || !int.TryParse(value, out debOutput))
                    {
                        Misc.MiscPrintError(new ArgumentException($"Invalid integer value for option {name}."), false);
                        return;
                    }
                    break;
                case "-D": case "--deb":
                    value = GetOptionValue(argv, ref i, arg);
                    if (value == null)
                    {
                        Misc.MiscPrintError(new ArgumentException($"Option {name} requires an argument."), false);
                        return;
                    }
                    debugText = value;
                    break;
                default:
                    // C: g_option_context_set_ignore_unknown_options(context, TRUE).
                    kept.Add(arg);
                    break;
            }
        }

        // Rebuild the argument list without the parsed options (as GOption does in C).
        string[] newArgv = new string[kept.Count + 1];
        newArgv[0] = argv[0];
        for (int k = 0; k < kept.Count; k++)
            newArgv[k + 1] = kept[k];
        argv = newArgv;
        argc = kept.Count + 1;

        if (debLevel != -1)
        {
            Variables.DebugLevel = debLevel;
            Debug.SyncMinimumLevel();
            // TODO: Call the C function window_create(WINDOW_DEBUG) (GUI not yet converted).
        }

        if (debOutput != -1)
            Variables.DebugOutput = debOutput;

        if (debugText != null)
            Variables.Statp = debugText;
    }

    /// <summary>
    /// Initialize some global variables. Most of them get nullified.
    /// Corresponds to C: main_init_variables(void).
    /// </summary>
    public static void MainInitVariables()
    {
        // C: ligs, cps, acps, users, transfer_list, season_stats, name_lists,
        // strategies, live_games, bets, jobs, job_teams, lg_commentary, news and
        // newspaper.articles are newly allocated here; in C# they are initialized
        // inline in Variables and Country, and the constants_app, settings,
        // constants, options, tokens and hints lists are already initialized.

        // C: country.name, country.symbol and country.sid are set to NULL
        // (in C# they are null by default).

        Variables.Season = 1;
        Variables.Week = 1;
        Variables.WeekRound = 1;

        for (int i = 0; i < (int)Counters.CountEnd; i++)
            Variables.Counters[i] = 0;

        Variables.Counters[(int)Counters.CountLeagueId] = Bygfoot.IdLeagueStart;
        Variables.Counters[(int)Counters.CountCupId] = Bygfoot.IdCupStart;
        Variables.Counters[(int)Counters.CountHintNumber] = -1;

        // C: window.main, window.startup, window.live, ... are set to NULL
        // (GUI not yet converted).

        Variables.SaveFile = null;

        Variables.SelectedRow = -1;

        Variables.TimeoutId = -1;

        FileHelper.LoadConfigFiles();
        StrategyXml.StrategyLoadStrategies();

        Variables.CurrentInterest = (float)Math.Round(MathHelper.Rnd(
                OptionHelper.ConstFloat("float_finance_interest_lower"),
                OptionHelper.ConstFloat("float_finance_interest_upper")) /
                OptionHelper.ConstFloat("float_finance_interest_step")) *
                OptionHelper.ConstFloat("float_finance_interest_step");

        Language.LanguageSet(Language.LanguageGetCodeIndex(OptionHelper.OptStr("string_opt_language_code")) + 1);

        Variables.Options.Add("int_opt_calodds", 0);

        /** Some of these (or all) are disabled (set to 1) in supernational
          country defs. */
        Variables.Settings.Add("int_opt_goto_mode", 0);
        Variables.Settings.Add("int_opt_disable_finances", 0);
        Variables.Settings.Add("int_opt_disable_transfers", 0);
        Variables.Settings.Add("int_opt_disable_stadium", 0);
        Variables.Settings.Add("int_opt_disable_contracts", 0);
        Variables.Settings.Add("int_opt_disable_boost_on", 0);
        Variables.Settings.Add("int_opt_disable_ya", 0);
        Variables.Settings.Add("int_opt_disable_training_camp", 0);
    }

    /// <summary>
    /// Process the command line arguments and do some things
    /// that have to be done at the beginning (like initializing the
    /// random number generator).
    /// Corresponds to C: main_init(gint *argc, gchar ***argv).
    /// </summary>
    /// <param name="argc">Number of command line arguments (reduced by the parsed options).</param>
    /// <param name="argv">Command line arguments array (reduced by the parsed options).</param>
    public static void MainInit(ref int argc, ref string[] argv)
    {
        Variables.SupportDirectories.Clear();
        Variables.RandGenerator = new Random();
        MainParseDebugClArguments(ref argc, ref argv);

        Variables.OsIsUnix = !OperatingSystem.IsWindows();

        if (Variables.OsIsUnix && !OperatingSystem.IsMacOS())
        {
            // C: #if defined(G_OS_UNIX) && !defined(MAC_BUILD)
            FileHelper.FileAddSupportDirectoryRecursive(PackageDataSupportDir);
            FileHelper.FileAddSupportDirectoryRecursive(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Bygfoot.HomeDirName));
        }

        if (!OperatingSystem.IsMacOS())
        {
            // C: #ifndef MAC_BUILD
            string dir = Directory.GetCurrentDirectory();
            FileHelper.FileAddSupportDirectoryRecursive(Path.Combine(dir, "support_files"));
            FileHelper.FileAddSupportDirectoryRecursive(Path.Combine(dir, "saves"));
        }
        else
        {
            // C: #else (MAC_BUILD) — file_add_support_directory_recursive
            // (file_get_mac_resource_path("support_files"));
            // TODO: Call the C function file_get_mac_resource_path (not yet converted).
        }

        MainInitVariables();

        LoadLastSave = false;
        MainParseClArguments(ref argc, ref argv);
    }

    /// <summary>
    /// Initialize the GTK stuff and the gettext stuff.
    /// Start the game.
    /// Corresponds to C: c_main(gint argc, gchar *argv[]).
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>The exit code of the program.</returns>
    public static int CMain(string[] args)
    {
        // C: gettext setup (bindtextdomain, bind_textdomain_codeset, textdomain)
        // is not applicable in C#.

        // C: on Windows, stdout and stderr are redirected to stdout.log and
        // stderr.log; not applicable for the console build.

        // C: gtk_init — the C# GTK4 equivalent is initializing the GTK module,
        // the GirCore integration and the embedded UI resources.
        Gtk.Module.Initialize();
        GirCore.Integration.Initialize();
        RegisterResources();

        int argc = args.Length;
        string[] argv = args;

        MainInit(ref argc, ref argv);

        if ((LoadLastSave && !LoadSave.LoadGameFromCommandLine("last_save")) ||
                (!LoadLastSave && (argc == 1 ||
                             (argc > 1 && !LoadSave.LoadGameFromCommandLine(argv[1])))))
        {
            if (Variables.Country.SId == null)
            {
                Bygfoot.Stat0 = (int)Status0Value.StatusSplash;
                // TODO: Call the C function window_show_splash (GUI not yet converted).

                if (Variables.OsIsUnix)
                    FileHelper.CheckHomeDir();
            }
            else
            {
                // TODO: Call the C function window_show_startup (GUI not yet converted).
                Bygfoot.Stat0 = (int)Status0Value.StatusTeamSelection;
            }
        }

        // TODO: Call gtk_main (GUI not yet converted).

        MainExitProgram(Bygfoot.ExitCode.Ok, null);

        return 0;
    }

    /// <summary>
    /// Exit the program with the given exit code and message. Try to
    /// destroy all widgets and free all memory first.
    /// Corresponds to C: main_exit_program(gint exit_code, gchar *format, ...).
    /// </summary>
    /// <param name="exitCode">The number we return to the shell.</param>
    /// <param name="format">The message we print (or null).</param>
    /// <param name="args">The format arguments.</param>
    public static void MainExitProgram(Bygfoot.ExitCode exitCode, string? format, params object[] args)
    {
        if (format != null)
            Debug.DebugPrintMessage(string.Format(format, args));

        // C: if (gtk_main_level() > 0) gtk_main_quit(); (GUI not yet converted).

        Free.FreeMemory();

        if (!Variables.OsIsUnix && exitCode != Bygfoot.ExitCode.Ok)
        {
            Debug.DebugPrintMessage("Press RETURN. Program will exit.");
            // C: getchar() — read a line here (close enough for the console build).
            Console.ReadLine();
        }

        Environment.Exit((int)exitCode);
    }

    /// <summary>
    /// Registers the embedded UI resources (resources.app.gresource) with GTK.
    /// C#-specific: the original C build loads .ui files from the support files.
    /// </summary>
    private static void RegisterResources()
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("resources.app.gresource");

        var buffer = new byte[stream!.Length];
        stream.ReadExactly(buffer);

        using var bytes = GLib.Bytes.New(buffer);
        using var resource = Gio.Resource.NewFromData(bytes);
        resource.Register();
    }
}
