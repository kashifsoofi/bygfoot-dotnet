namespace Bygfoot.Cli;

/// <summary>
/// Types of windows we can create.
/// Corresponds to C enum Windows in src/window.h.
/// </summary>
public enum WindowsType
{
    /// <summary>Corresponds to C: WINDOW_MAIN.</summary>
    Main = 0,
    /// <summary>Corresponds to C: WINDOW_STARTUP.</summary>
    Startup,
    /// <summary>Corresponds to C: WINDOW_LIVE.</summary>
    Live,
    /// <summary>Corresponds to C: WINDOW_WARNING.</summary>
    Warning,
    /// <summary>Corresponds to C: WINDOW_PROGRESS.</summary>
    Progress,
    /// <summary>Corresponds to C: WINDOW_DIGITS.</summary>
    Digits,
    /// <summary>Corresponds to C: WINDOW_STADIUM.</summary>
    Stadium,
    /// <summary>Corresponds to C: WINDOW_JOB_OFFER.</summary>
    JobOffer,
    /// <summary>Corresponds to C: WINDOW_YESNO.</summary>
    Yesno,
    /// <summary>Corresponds to C: WINDOW_OPTIONS.</summary>
    Options,
    /// <summary>Corresponds to C: WINDOW_CONSTANTS.</summary>
    Constants,
    /// <summary>Corresponds to C: WINDOW_FONT_SEL.</summary>
    FontSel,
    /// <summary>Corresponds to C: WINDOW_FILE_CHOOSER.</summary>
    FileChooser,
    /// <summary>Corresponds to C: WINDOW_CONTRACT.</summary>
    Contract,
    /// <summary>Corresponds to C: WINDOW_USER_MANAGEMENT.</summary>
    UserManagement,
    /// <summary>Corresponds to C: WINDOW_DEBUG.</summary>
    Debug,
    /// <summary>Corresponds to C: WINDOW_HELP.</summary>
    Help,
    /// <summary>Corresponds to C: WINDOW_TRANSFER_DIALOG.</summary>
    TransferDialog,
    /// <summary>Corresponds to C: WINDOW_SPONSORS.</summary>
    Sponsors,
    /// <summary>Corresponds to C: WINDOW_MMATCHES.</summary>
    MMatches,
    /// <summary>Corresponds to C: WINDOW_BETS.</summary>
    Bets,
    /// <summary>Corresponds to C: WINDOW_SPLASH.</summary>
    Splash,
    /// <summary>Corresponds to C: WINDOW_END.</summary>
    End,
    /// <summary>Corresponds to C: WINDOW_TRAINING_CAMP.</summary>
    TrainingCamp,
    /// <summary>Corresponds to C: WINDOW_NEWS.</summary>
    News,
    /// <summary>Corresponds to C: WINDOW_ALR.</summary>
    Alr
}

/// <summary>
/// Struct used for having all the windows in one place.
/// Corresponds to C struct Windows in bygfoot.h.
/// </summary>
public class Windows
{
    /// <summary>Corresponds to C: GtkWidget *main.</summary>
    public Gtk.Widget? Main { get; set; }
    /// <summary>Corresponds to C: GtkWidget *startup.</summary>
    public Gtk.Widget? Startup { get; set; }
    /// <summary>Corresponds to C: GtkWidget *live.</summary>
    public Gtk.Widget? Live { get; set; }
    /// <summary>Corresponds to C: GtkWidget *warning.</summary>
    public Gtk.Widget? Warning { get; set; }
    /// <summary>Corresponds to C: GtkWidget *progress.</summary>
    public Gtk.Widget? Progress { get; set; }
    /// <summary>Corresponds to C: GtkWidget *digits.</summary>
    public Gtk.Widget? Digits { get; set; }
    /// <summary>Corresponds to C: GtkWidget *stadium.</summary>
    public Gtk.Widget? Stadium { get; set; }
    /// <summary>Corresponds to C: GtkWidget *job_offer.</summary>
    public Gtk.Widget? JobOffer { get; set; }
    /// <summary>Corresponds to C: GtkWidget *yesno.</summary>
    public Gtk.Widget? Yesno { get; set; }
    /// <summary>Corresponds to C: GtkWidget *options.</summary>
    public Gtk.Widget? Options { get; set; }
    /// <summary>Corresponds to C: GtkWidget *constants.</summary>
    public Gtk.Widget? Constants { get; set; }
    /// <summary>Corresponds to C: GtkWidget *font_sel.</summary>
    public Gtk.Widget? FontSel { get; set; }
    /// <summary>Corresponds to C: GtkWidget *file_chooser.</summary>
    public Gtk.Widget? FileChooser { get; set; }
    /// <summary>Corresponds to C: GtkWidget *contract.</summary>
    public Gtk.Widget? Contract { get; set; }
    /// <summary>Corresponds to C: GtkWidget *menu_player.</summary>
    public Gtk.Widget? MenuPlayer { get; set; }
    /// <summary>Corresponds to C: GtkWidget *menu_youth.</summary>
    public Gtk.Widget? MenuYouth { get; set; }
    /// <summary>Corresponds to C: GtkWidget *user_management.</summary>
    public Gtk.Widget? UserManagement { get; set; }
    /// <summary>Corresponds to C: GtkWidget *wdebug.</summary>
    public Gtk.Widget? WDebug { get; set; }
    /// <summary>Corresponds to C: GtkWidget *help.</summary>
    public Gtk.Widget? Help { get; set; }
    /// <summary>Corresponds to C: GtkWidget *transfer_dialog.</summary>
    public Gtk.Widget? TransferDialog { get; set; }
    /// <summary>Corresponds to C: GtkWidget *sponsors.</summary>
    public Gtk.Widget? Sponsors { get; set; }
    /// <summary>Corresponds to C: GtkWidget *mmatches.</summary>
    public Gtk.Widget? MMatches { get; set; }
    /// <summary>Corresponds to C: GtkWidget *bets.</summary>
    public Gtk.Widget? Bets { get; set; }
    /// <summary>Corresponds to C: GtkWidget *splash.</summary>
    public Gtk.Widget? Splash { get; set; }
    /// <summary>Corresponds to C: GtkWidget *training_camp.</summary>
    public Gtk.Widget? TrainingCamp { get; set; }
    /// <summary>Corresponds to C: GtkWidget *league_table.</summary>
    public Gtk.Widget? LeagueTable { get; set; }
    /// <summary>Corresponds to C: GtkWidget *news.</summary>
    public Gtk.Widget? News { get; set; }
    /// <summary>Corresponds to C: GtkWidget *alr.</summary>
    public Gtk.Widget? Alr { get; set; }

    /// <summary>The position of the horizontal divider is stored here. Corresponds to C: gint paned_pos.</summary>
    public int PanedPos { get; set; }
}

/// <summary>
/// Static class for creating, showing and destroying the game windows.
/// Corresponds to the C module src/window.h and src/window.c.
/// </summary>
public static class Window
{
    /// <summary>Corresponds to C: #define CAMP_SCALE_MIN 0 in src/training_struct.h.</summary>
    private const int CampScaleMin = 0;
    /// <summary>Corresponds to C: #define CAMP_SCALE_MAX 10 in src/training_struct.h.</summary>
    private const int CampScaleMax = 10;

    /// <summary>
    /// Show the splash screen window.
    /// Corresponds to C: window_show_splash(void).
    /// </summary>
    public static void WindowShowSplash()
    {
        Debug.DebugPrintMessage("window_show_splash\n");

        Window.WindowCreate(WindowsType.Splash);

        // TODO: treeview_show_contributors (src/treeview.c not yet converted).
        // C: treeview_show_contributors(GTK_TREE_VIEW(
        //     lookup_widget(window.splash, "treeview_splash_contributors")));

        Window.WindowLoadHintNumber();
        Window.WindowSplashShowHint();
    }

    /// <summary>
    /// Show the hint determined by the hint counter in the splash window.
    /// Corresponds to C: window_splash_show_hint(void).
    /// </summary>
    public static void WindowSplashShowHint()
    {
        Debug.DebugPrintMessage("window_splash_show_hint\n");

        int hintNumber = Variables.Counters[(int)Counters.CountHintNumber];

        (Gui.LookupWidget(Variables.Window.Splash, "label_splash_hint") as Gtk.Label)!.SetLabel(
            Variables.Hints.List[hintNumber].StringValue ?? "");

        (Gui.LookupWidget(Variables.Window.Splash, "label_splash_hintcounter") as Gtk.Label)!.SetLabel(
            $"({hintNumber + 1}/{Variables.Hints.List.Count})");
    }

    /// <summary>
    /// Load the index of the hint to show in the splash screen.
    /// Corresponds to C: window_load_hint_number(void).
    /// </summary>
    public static void WindowLoadHintNumber()
    {
        Debug.DebugPrintMessage("window_load_hint_number\n");

        string filename = Path.Combine(FileHelper.GetBygfootDir(), "hint_num");

        if (!FileHelper.MyFopen(filename, "r", out Stream? fil, false))
        {
            Variables.Counters[(int)Counters.CountHintNumber] = 0;
            return;
        }

        // C: fscanf(fil, "%d", &counters[COUNT_HINT_NUMBER]).
        string content = new StreamReader(fil).ReadToEnd().Trim();
        fil.Dispose();
        if (int.TryParse(content, out int hintNumber))
            Variables.Counters[(int)Counters.CountHintNumber] = hintNumber;

        if (Variables.Counters[(int)Counters.CountHintNumber] < 0 ||
            Variables.Counters[(int)Counters.CountHintNumber] >= Variables.Hints.List.Count)
        {
            Debug.DebugPrintMessage(
                $"Hint counter out of bounds: {Variables.Counters[(int)Counters.CountHintNumber]} " +
                $"(bounds 0 and {Variables.Hints.List.Count - 1}).\n");
            Variables.Counters[(int)Counters.CountHintNumber] = 0;
        }
    }

    /// <summary>
    /// Save the index of the current hint.
    /// Corresponds to C: window_save_hint_number(void).
    /// </summary>
    public static void WindowSaveHintNumber()
    {
        Debug.DebugPrintMessage("window_save_hint_number\n");

        string filename = Path.Combine(FileHelper.GetBygfootDir(), "hint_num");

        if (!FileHelper.MyFopen(filename, "w", out Stream? fil, false))
            return;

        using (var writer = new StreamWriter(fil))
            writer.Write(Variables.Counters[(int)Counters.CountHintNumber]);
    }

    /// <summary>
    /// Show the window with the progress bar, sometimes with a nice picture.
    /// Corresponds to C: window_show_progress(gint pictype).
    /// </summary>
    /// <param name="pictype">What kind of picture to show above the progress bar.</param>
    public static void WindowShowProgress(int pictype)
    {
        Debug.DebugPrintMessage("window_show_progress\n");

        if (OptionHelper.SettInt("int_opt_goto_mode") != 0)
            return;

        Window.WindowCreate(WindowsType.Progress);

        if (pictype == (int)PicType.PicTypeNone ||
            OptionHelper.OptInt("int_opt_progressbar_pics") == 0)
            return;

        Gtk.Image? image = Gui.LookupWidget(Variables.Window.Progress, "image_match") as Gtk.Image;

        switch (pictype)
        {
            default:
                Debug.DebugPrintMessage($"window_show_progress: unknown picture type {pictype}");
                break;
            case (int)PicType.PicTypeSave:
                // C: gtk_image_set_from_stock(GTK_STOCK_FLOPPY, GTK_ICON_SIZE_DIALOG) —
                // stock icons do not exist in GTK4, use the matching icon name instead.
                image!.SetFromIconName("document-save");
                break;
            case (int)PicType.PicTypeLoad:
                // C: gtk_image_set_from_stock(GTK_STOCK_OPEN, GTK_ICON_SIZE_DIALOG)
                image!.SetFromIconName("document-open");
                break;
            case (int)PicType.PicTypeMatchpic:
                string? buf2 = null;
                if (Debug.DebugEggForwardsBoostStyle())
                {
                    buf2 = FileHelper.FindSupportFile("soccerbabes.jpg", false);
                    if (buf2 != null)
                    {
                        image!.SetFromFile(buf2);
                        // C: g_free(buf2) — no-op under C# GC.
                    }
                }

                if (buf2 == null)
                {
                    string? picDir = FileHelper.GetFirstSupportDirSuffix("pics");
                    if (picDir == null)
                        return;

                    List<string> pics = FileHelper.GetDirectoryContents(picDir, "match", "");

                    if (pics.Count != 0)
                    {
                        string buf = Path.Combine(picDir, pics[MathHelper.Rndi(0, pics.Count - 1)]);
                        image!.SetFromFile(buf);
                    }

                    // C: free_gchar_array(&pics) — no-op under C# GC.
                }
                break;
        }
    }

    /// <summary>
    /// Show the betting window.
    /// Corresponds to C: window_show_bets(void).
    /// </summary>
    public static void WindowShowBets()
    {
        Debug.DebugPrintMessage("window_show_bets\n");

        Window.WindowCreate(WindowsType.Bets);

        (Gui.LookupWidget(Variables.Window.Bets, "checkbutton_bet_all_leagues") as Gtk.ToggleButton)!.Active =
            OptionHelper.OptUserInt("int_opt_user_bet_show_all_leagues") != 0;
        (Gui.LookupWidget(Variables.Window.Bets, "checkbutton_bet_cups") as Gtk.ToggleButton)!.Active =
            OptionHelper.OptUserInt("int_opt_user_bet_show_cups") != 0;
        (Gui.LookupWidget(Variables.Window.Bets, "checkbutton_bet_user_recent") as Gtk.ToggleButton)!.Active =
            OptionHelper.OptUserInt("int_opt_user_bet_show_my_recent") != 0;

        // TODO: treeview2_show_bets (src/treeview2.c not yet converted).
    }

    /// <summary>
    /// Show the help/about window.
    /// Corresponds to C: window_show_help(gint page).
    /// </summary>
    /// <param name="page">Which notebook page to display.</param>
    public static void WindowShowHelp(int page)
    {
        Debug.DebugPrintMessage("window_show_help\n");

        Window.WindowCreate(WindowsType.Help);

        string buf = string.Format(
            "<span {0}>Bygfoot Football Manager {1}</span>\n(c) {2} Győző Both (gyboth@bygfoot.com)\nhttp://bygfoot.sourceforge.net",
            OptionHelper.ConstApp("string_help_window_program_name_attribute"),
            Bygfoot.Version, Bygfoot.Year);
        (Gui.LookupWidget(Variables.Window.Help, "label_about") as Gtk.Label)?.SetMarkup(buf);

        // TODO: treeview_show_contributors (src/treeview.c not yet converted).
        // C: treeview_show_contributors(GTK_TREE_VIEW(
        //     lookup_widget(window.help, "treeview_contributors")));

        // TODO: game_gui_set_help_labels (src/game_gui.c not yet converted).

        (Gui.LookupWidget(Variables.Window.Help, "notebook1") as Gtk.Notebook)?.SetCurrentPage(page);
    }

    /// <summary>
    /// Cell data function that makes the country combo box entries sensitive
    /// only for top-level (country) rows.
    /// Corresponds to C: static void is_capital_sensitive(GtkCellLayout *cell_layout,
    /// GtkCellRenderer *cell, GtkTreeModel *tree_model, GtkTreeIter *iter, gpointer data)
    /// in src/window.c.
    /// </summary>
    private static void IsCapitalSensitive(Gtk.CellLayout cellLayout, Gtk.CellRenderer cell,
        Gtk.TreeModel treeModel, Gtk.TreeIter iter)
    {
        bool sensitive = !treeModel.IterHasChild(iter);
        cell.Sensitive = sensitive;
    }

    /// <summary>
    /// Show the country selection window. All files with prefix 'country_' from
    /// $HOME/.bygfoot/definitions are appended to a combo box.
    /// Corresponds to C: window_show_startup(void).
    /// </summary>
    public static void WindowShowStartup()
    {
        Debug.DebugPrintMessage("window_show_startup\n");

        Gtk.Window? windowStartup = Window.WindowCreate(WindowsType.Startup);
        Gtk.ComboBox? comboCountry =
            Gui.LookupWidget(windowStartup, "combo_country") as Gtk.ComboBox;
        List<string> countryFiles = FileHelper.GetCountryFiles();
        string? lastCountry = FileHelper.LoadTextFromSaves("last_country");

        if (countryFiles.Count == 0)
            Main.MainExitProgram(Bygfoot.ExitCode.NoCountryFiles,
                "Didn't find any country definition files in the support directories.");

        // TODO: cell layout setup (src/treeview_helper.c not yet converted).
        // C: gtk_cell_layout_clear(combo_country);
        //     renderer = gtk_cell_renderer_pixbuf_new(); pack_start; set_attributes "pixbuf", 0;
        //     renderer = treeview_helper_cell_renderer_text_new(); pack_start; set_attributes "text", 1;
        //     model = treeview_create_country_list(country_files);
        //     gtk_combo_box_set_model(combo_country, model);
        //     gtk_cell_layout_set_cell_data_func(combo_country, renderer, is_capital_sensitive, NULL, NULL);
        // See IsCapitalSensitive for the converted cell data function.

        Language.LanguagePickCountry(countryFiles);

        // TODO: misc_callback_show_team_list (src/misc_callback_func.c not yet converted).
        // C: if (country.sid != NULL)
        //        misc_callback_show_team_list(combo_country, country.sid);
        //    else if (last_country != NULL)
        //        misc_callback_show_team_list(combo_country, last_country);
        //    else
        //        misc_callback_show_team_list(combo_country, country_files[len - 1]);

        // C: free_gchar_array(&country_files) — no-op under C# GC.
    }

    /// <summary>
    /// Show the file selection window.
    /// Corresponds to C: window_show_file_sel(void).
    /// </summary>
    public static void WindowShowFileSel()
    {
        Debug.DebugPrintMessage("window_show_file_sel\n");

        Window.WindowCreate(WindowsType.FileChooser);

        bool loadGame = Bygfoot.Stat5 == (int)Status0Value.StatusLoadGame ||
                        Bygfoot.Stat5 == (int)Status0Value.StatusLoadGameSplash;

        // C: gtk_file_chooser_set_action(GTK_FILE_CHOOSER(window.file_chooser), ...).
        // GTK4: the file chooser embedded in the window is replaced by a native
        // file chooser dialog; the C flow below (set up, run, dispatch, destroy)
        // is kept with the native dialog standing in for the embedded chooser.
        Gtk.FileChooserAction action = loadGame ? Gtk.FileChooserAction.Open : Gtk.FileChooserAction.Save;
        Gtk.FileChooserNative chooser = Gtk.FileChooserNative.New(
            "Choose file", Variables.Window.Main as Gtk.Window, action, "OK", "Cancel");

        Gtk.FileFilter filter = Gtk.FileFilter.New();
        if (Bygfoot.Stat5 == (int)Status0Value.StatusSaveGame || loadGame)
        {
            filter.SetName("Bygfoot Save Files");
            filter.AddPattern("*.zip");
            filter.AddPattern("last_save");
        }
        else
        {
            filter.SetName("Bygfoot Memorable Matches");
            filter.AddPattern("*.bmm.zip");
        }
        chooser.AddFilter(filter);

        Gtk.FileFilter filterAllFiles = Gtk.FileFilter.New();
        filterAllFiles.SetName("All Files");
        filterAllFiles.AddPattern("*");
        chooser.AddFilter(filterAllFiles);

        // C: gtk_file_chooser_set_filename / gtk_file_chooser_set_current_folder —
        // the GirCore bindings here cannot construct Gio.File objects, so only the
        // initial file name is set (the folder stays at the default location).
        if (loadGame && Variables.SaveFile != null)
            chooser.SetCurrentName(System.IO.Path.GetFileName(Variables.SaveFile));
        else if (Variables.Users.Count > 0 &&
                 Variables.Users[Variables.CurUser].MmatchesFile != null)
            chooser.SetCurrentName(System.IO.Path.GetFileName(Variables.Users[Variables.CurUser].MmatchesFile));
        // else C: set the current folder to <bygfoot dir>/saves (omitted, see above).

        chooser.OnResponse += OnFileChooserResponse;
        chooser.Show();
    }

    /// <summary>
    /// Response handler for the file selection native dialog.
    /// Corresponds to the C code after gtk_dialog_run in window_show_file_sel(void).
    /// </summary>
    private static void OnFileChooserResponse(Gtk.NativeDialog sender,
        Gtk.NativeDialog.ResponseSignalArgs args)
    {
        // C: if (gtk_dialog_run(GTK_DIALOG(window.file_chooser)) == GTK_RESPONSE_OK).
        if (args.ResponseId == (int)Gtk.ResponseType.Ok ||
            args.ResponseId == (int)Gtk.ResponseType.Accept)
        {
            string filename = (sender as Gtk.FileChooserNative)?.GetFile()?.GetPath() ?? "";

            if (Bygfoot.Stat5 == (int)Status0Value.StatusLoadGame)
                LoadSave.LoadSaveLoadGame(filename, false);
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusLoadGameSplash)
                // TODO: misc_callback_startup_load (src/misc_callback_func.c not yet converted).
                ;
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusSaveGame)
                LoadSave.LoadSaveSaveGame(filename);
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusSelectMmFileLoad)
            {
                bool mmFileExists = File.Exists(filename);
                if ((mmFileExists && filename.EndsWith(".bmm.zip")) || !mmFileExists)
                {
                    if (mmFileExists)
                        // TODO: user_mm_load_file (src/user.c not yet converted).
                        ;
                    else
                        // C: user_mm_set_filename(filename, NULL).
                        Variables.Users[Variables.CurUser].MmatchesFile = filename;
                    Window.WindowShowMmatches();
                }
                else
                    // TODO: game_gui_show_warning (src/game_gui.c not yet converted).
                    Debug.DebugPrintMessage("Not a valid Bygfoot Memorable Matches filename.");
            }
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusSelectMmFileAdd)
            {
                // C: user_mm_set_filename(filename, NULL).
                Variables.Users[Variables.CurUser].MmatchesFile = filename;
                // TODO: user_mm_add_last_match(file_exists, TRUE) (src/user.c not yet converted).
            }
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusSelectMmFileImport)
            {
                // TODO: user_mm_import_file (src/user.c not yet converted).
                Window.WindowShowMmatches();
            }
            else if (Bygfoot.Stat5 == (int)Status0Value.StatusSelectMmFileExport)
            {
                // TODO: user_mm_export_file (src/user.c not yet converted).
            }

            if (Bygfoot.Stat5 == (int)Status0Value.StatusLoadGame ||
                Bygfoot.Stat5 == (int)Status0Value.StatusLoadGameSplash)
            {
                Variables.CurUser = 0;
                // TODO: on_button_back_to_main_clicked (src/callbacks.c not yet converted).
                // C: setsav1 — make the main window save button/menu insensitive.
                if (Variables.Window.Main != null)
                {
                    if (Gui.LookupWidget(Variables.Window.Main, "button_save") is { } saveButton)
                        saveButton.Sensitive = false;
                    if (Gui.LookupWidget(Variables.Window.Main, "menu_save") is { } menuSave)
                        menuSave.Sensitive = false;
                }
            }
        }

        (sender as Gtk.FileChooserNative)?.Destroy();

        // C: window_destroy(&window.file_chooser).
        var windowFileChooser = Variables.Window.FileChooser;
        Window.WindowDestroy(ref windowFileChooser);
    }

    /// <summary>
    /// Show window with memorable matches list.
    /// Corresponds to C: window_show_mmatches(void).
    /// </summary>
    public static void WindowShowMmatches()
    {
        Debug.DebugPrintMessage("window_show_mmatches\n");

        if (Variables.Window.MMatches == null)
            Window.WindowCreate(WindowsType.MMatches);

        // TODO: treeview2_show_mmatches (src/treeview2.c not yet converted).

        (Gui.LookupWidget(Variables.Window.MMatches, "entry_mm_file") as Gtk.Entry)!.SetText(
            Variables.Users[Variables.CurUser].MmatchesFile ?? "");
    }

    /// <summary>
    /// Show the options window.
    /// Corresponds to C: window_show_options(void).
    /// </summary>
    public static void WindowShowOptions()
    {
        Debug.DebugPrintMessage("window_show_options\n");

        Window.WindowCreate(WindowsType.Options);

        // TODO: option_gui_set_up_window (src/option_gui.c not yet converted).
    }

    /// <summary>
    /// Show the player list context menu, triggered by 'event'.
    /// Corresponds to C: window_show_menu_player(GdkEvent *event).
    /// </summary>
    /// <param name="event">The event that triggered the menu (GdkEvent*, typed as
    /// object since the Gdk bindings are not referenced by this project).</param>
    public static void WindowShowMenuPlayer(object? @event)
    {
        Debug.DebugPrintMessage("window_show_menu_player\n");

        // TODO: create_menu_player (src/option_gui.c not yet converted) and the
        // popup call (src/callbacks.c not yet converted).
        // C: window.menu_player = create_menu_player();
        //     gtk_menu_popup(GTK_MENU(window.menu_player), NULL, NULL, NULL, NULL,
        //                    ((GdkEventButton*)event)->button, gdk_event_get_time(event));
        // gtk_menu_popup was removed in GTK4 — the menu will be shown through a
        // Gtk.Popover based API once the menu creation is converted.
    }

    /// <summary>
    /// Show the youth academy context menu, triggered by 'event'.
    /// Corresponds to C: window_show_menu_youth(GdkEvent *event).
    /// </summary>
    /// <param name="event">The event that triggered the menu (GdkEvent*, typed as
    /// object since the Gdk bindings are not referenced by this project).</param>
    public static void WindowShowMenuYouth(object? @event)
    {
        Debug.DebugPrintMessage("window_show_menu_youth\n");

        // TODO: create_menu_youth (src/option_gui.c not yet converted) and the
        // popup call (src/callbacks.c not yet converted).
        // C: window.menu_youth = create_menu_youth();
        //     gtk_menu_popup(GTK_MENU(window.menu_youth), NULL, NULL, NULL, NULL,
        //                    ((GdkEventButton*)event)->button, gdk_event_get_time(event));
        // gtk_menu_popup was removed in GTK4 — the menu will be shown through a
        // Gtk.Popover based API once the menu creation is converted.
    }

    /// <summary>
    /// Show the digits window with the labels and values set according to the
    /// arguments.
    /// Corresponds to C: window_show_digits(const gchar *text_main, const gchar *text1,
    /// gint value1, const gchar *text2, gint value2, gboolean show_alr).
    /// </summary>
    public static void WindowShowDigits(string? textMain, string? text1, int value1,
        string? text2, int value2, bool showAlr)
    {
        Debug.DebugPrintMessage("window_show_digits\n");

        Gtk.Window? windowDigits = Window.WindowCreate(WindowsType.Digits);

        Gtk.Label? labelMain = Gui.LookupWidget(windowDigits, "label_main") as Gtk.Label;
        Gtk.Label? label1 = Gui.LookupWidget(windowDigits, "label_1") as Gtk.Label;
        Gtk.Label? label2 = Gui.LookupWidget(windowDigits, "label_2") as Gtk.Label;

        Gtk.SpinButton? spinbutton1 = Gui.LookupWidget(windowDigits, "spinbutton1") as Gtk.SpinButton;
        Gtk.SpinButton? spinbutton2 = Gui.LookupWidget(windowDigits, "spinbutton2") as Gtk.SpinButton;

        if (Bygfoot.Stat1 == (int)Status0Value.StatusGetLoan ||
            Bygfoot.Stat1 == (int)Status0Value.StatusPayLoan)
            spinbutton1?.SetRange(1, value1);
        else if (Bygfoot.Stat1 == (int)Status0Value.StatusSetYaPercentage)
            spinbutton2?.SetRange(0, OptionHelper.ConstInt("int_youth_academy_max_percentage"));

        spinbutton1?.SetValue(value1);
        spinbutton2?.SetValue(value2);

        labelMain?.SetLabel(textMain ?? "");

        if (text1 != null)
            label1?.SetLabel(text1);
        else
            label1?.Parent?.Hide();

        if (text2 != null)
            label2?.SetLabel(text2);
        else
            label2?.Parent?.Hide();

        if (showAlr)
            Gui.LookupWidget(windowDigits, "button_digits_alr")?.Show();
    }

    /// <summary>
    /// Show the stadium window for the current user.
    /// Corresponds to C: window_show_stadium(void).
    /// </summary>
    public static void WindowShowStadium()
    {
        Debug.DebugPrintMessage("window_show_stadium\n");

        User user = Variables.Users[Variables.CurUser];
        Team tm = user.Team;

        Window.WindowCreate(WindowsType.Stadium);

        Gtk.Label? labelCapacity = Gui.LookupWidget(Variables.Window.Stadium, "label_capacity") as Gtk.Label;
        Gtk.Label? labelStadiumStatus =
            Gui.LookupWidget(Variables.Window.Stadium, "label_stadium_status") as Gtk.Label;
        Gtk.Label? labelAverageAttendance =
            Gui.LookupWidget(Variables.Window.Stadium, "label_average_attendance") as Gtk.Label;
        Gtk.Label? labelName = Gui.LookupWidget(Variables.Window.Stadium, "label_name") as Gtk.Label;

        Gtk.SpinButton? spinTicketPrice =
            Gui.LookupWidget(Variables.Window.Stadium, "spin_ticket_price") as Gtk.SpinButton;
        spinTicketPrice?.SetValue(tm.Stadium.TicketPrice);
        // Set the lower bound to constant_ticket_price-(1/constant_ticket_attendance)
        // +1 so that we can't divide by 0 or negative numbers.
        double lowerBound = OptionHelper.ConstInt("int_team_stadium_ticket_price")
            - (1f / OptionHelper.ConstFloat("float_team_stadium_price_attendance_factor")) + 1;
        spinTicketPrice?.SetRange(lowerBound, 100);

        Gtk.ProgressBar? progressbarSafety =
            Gui.LookupWidget(Variables.Window.Stadium, "progressbar_safety") as Gtk.ProgressBar;
        Gtk.ProgressBar? progressbarAverageAttendance =
            Gui.LookupWidget(Variables.Window.Stadium, "progressbar_average_attendance") as Gtk.ProgressBar;

        Gui.GuiLabelSetTextFromInt(labelCapacity!, tm.Stadium.Capacity, false);
        Gui.GuiLabelSetTextFromInt(labelAverageAttendance!, tm.Stadium.AverageAttendance, false);

        if (!string.IsNullOrEmpty(tm.Stadium.Name))
            labelName?.SetLabel(tm.Stadium.Name);
        else
            labelName?.Hide();

        float averageAttendancePerc = 0;
        if (tm.Stadium.Games > 0)
            averageAttendancePerc =
                (float)(tm.Stadium.AverageAttendance * tm.Stadium.Games) /
                (float)tm.Stadium.PossibleAttendance;

        progressbarSafety!.Fraction = tm.Stadium.Safety;
        progressbarAverageAttendance!.Fraction = averageAttendancePerc;

        string buf = $"{(int)Math.Round(tm.Stadium.Safety * 100)}%";
        progressbarSafety!.Text = buf;

        buf = $"{(int)Math.Round(averageAttendancePerc * 100)}%";
        progressbarAverageAttendance!.Text = buf;

        if (user.Counters[(int)CounterType.StadiumCapacity] +
            user.Counters[(int)CounterType.StadiumSafety] != 0)
        {
            buf = string.Format(
                "Improvement in progress.\n{0} seats and {1}% safety still to be done.\nExpected finish: {2} weeks.",
                user.Counters[(int)CounterType.StadiumCapacity],
                user.Counters[(int)CounterType.StadiumSafety],
                Math.Max(
                    Finance.GetStadiumImprovementDuration(
                        (float)user.Counters[(int)CounterType.StadiumCapacity], true),
                    Finance.GetStadiumImprovementDuration(
                        (float)user.Counters[(int)CounterType.StadiumSafety] / 100, false)));
            labelStadiumStatus?.SetLabel(buf);
        }
        else
            labelStadiumStatus?.SetLabel("No improvements currently in progress.");
    }

    /// <summary>
    /// Show the window where the user can select between yes and no.
    /// Corresponds to C: window_show_yesno(const gchar *text).
    /// </summary>
    /// <param name="text">The text shown in the window.</param>
    public static void WindowShowYesno(string text)
    {
        Debug.DebugPrintMessage("window_show_yesno\n");

        Window.WindowCreate(WindowsType.Yesno);
        (Gui.LookupWidget(Variables.Window.Yesno, "label_yesno") as Gtk.Label)?.SetLabel(text);
    }

    /// <summary>
    /// Show the transfer dialog (yes/no/later).
    /// Corresponds to C: window_show_transfer_dialog(const gchar *text).
    /// </summary>
    /// <param name="text">The text to put into the label.</param>
    public static void WindowShowTransferDialog(string text)
    {
        Debug.DebugPrintMessage("window_show_transfer_dialog\n");

        Window.WindowCreate(WindowsType.TransferDialog);
        (Gui.LookupWidget(Variables.Window.TransferDialog, "label_transfer_dialog") as Gtk.Label)?.SetLabel(text);
    }

    /// <summary>
    /// Set up the spinbuttons and boost/style images in the live window.
    /// Corresponds to C: window_live_set_up(void).
    /// </summary>
    public static void WindowLiveSetUp()
    {
        Debug.DebugPrintMessage("window_live_set_up\n");

        Gtk.SpinButton? sbSpeed =
            Gui.LookupWidget(Variables.Window.Live, "spinbutton_speed") as Gtk.SpinButton;
        int userOption =
            Variables.Users[Bygfoot.Stat2].Options.GetInt("int_opt_user_live_game_speed");

        sbSpeed?.SetRange(0, -Math.Round(
            (OptionHelper.ConstInt("int_game_gui_live_game_speed_max") - 10) /
            (double)OptionHelper.ConstInt("int_game_gui_live_game_speed_grad")));

        sbSpeed?.SetValue(userOption);

        (Gui.LookupWidget(Variables.Window.Live, "spinbutton_verbosity") as Gtk.SpinButton)!.SetValue(
            Variables.Users[Bygfoot.Stat2].Options.GetInt("int_opt_user_live_game_verbosity"));

        // TODO: game_gui_write_meters (src/game_gui.c not yet converted).
        // C: game_gui_write_meters(usr(stat2).tm);
    }

    /// <summary>
    /// Save main window size and position into a file.
    /// Corresponds to C: window_main_save_geometry(void).
    /// </summary>
    public static void WindowMainSaveGeometry()
    {
        Debug.DebugPrintMessage("window_main_save_geometry\n");

        string filename = Path.Combine(FileHelper.GetBygfootDir(), "window_settings");

        if (Variables.Window.Main != null &&
            FileHelper.MyFopen(filename, "w", out Stream? fil, false))
        {
            Gtk.Window wind = (Gtk.Window)Variables.Window.Main;
            int width = wind.GetWidth();
            int height = wind.GetHeight();
            // C: gtk_window_get_position — removed in GTK4, the window manager
            // decides the position, so 0 is stored.
            int posX = 0;
            int posY = 0;
            int panedPos = (Gui.LookupWidget(wind, "hpaned2") as Gtk.Paned)!.Position;

            using (var writer = new StreamWriter(fil))
            {
                writer.WriteLine($"int_window_settings_width\t{width}");
                writer.WriteLine($"int_window_settings_height\t{height}");
                writer.WriteLine($"int_window_settings_pos_x\t{posX}");
                writer.WriteLine($"int_window_settings_pos_y\t{posY}");
                writer.WriteLine($"int_window_settings_paned_pos\t{panedPos}");
            }
        }
    }

    /// <summary>
    /// Set the main window geometry according to the file settings.
    /// Corresponds to C: window_main_load_geometry(void).
    /// </summary>
    public static void WindowMainLoadGeometry()
    {
        Debug.DebugPrintMessage("window_main_load_geometry\n");

        string filename = Path.Combine(FileHelper.GetBygfootDir(), "window_settings");

        if (File.Exists(filename))
        {
            OptionList optionList = new OptionList();
            FileHelper.LoadOptFile(filename, optionList, false);

            Gtk.Window? wind = Variables.Window.Main as Gtk.Window;

            // C: gtk_window_resize — GTK4 equivalent: set the default size.
            wind?.SetDefaultSize(
                optionList.GetInt("int_window_settings_width"),
                optionList.GetInt("int_window_settings_height"));

            // C: gtk_window_move — removed in GTK4 (the window manager decides
            // the position), so the stored pos_x/pos_y are not applied.

            (Gui.LookupWidget(wind, "hpaned2") as Gtk.Paned)!.Position =
                optionList.GetInt("int_window_settings_paned_pos");

            // C: free_option_list(&optionlist, FALSE) — no-op under C# GC.
        }
    }

    /// <summary>
    /// Create and show a window. Which one depends on the argument.
    /// Corresponds to C: GtkWidget *window_create(gint window_type).
    /// </summary>
    /// <param name="windowType">An integer telling us which window to create.</param>
    /// <returns>The pointer to the new window.</returns>
    public static Gtk.Window? WindowCreate(WindowsType windowType)
    {
        string buf = $"Bygfoot Football Manager {Bygfoot.Version}";
        Gtk.Window? wind = null;

        switch (windowType)
        {
            default:
                Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                    "window_create: unknown window type %d\n", (int)windowType);
                break;
            case WindowsType.Main:
                if (Variables.Window.Main == null)
                {
                    MainWindow.NewWithProperties([]);
                    wind = Variables.Window.Main as Gtk.Window;
                    Window.WindowMainLoadGeometry();
                    Variables.Window.PanedPos =
                        (Gui.LookupWidget(wind, "hpaned2") as Gtk.Paned)!.Position;
                    // TODO: game_gui_print_message (src/game_gui.c not yet converted).
                    // C: game_gui_print_message(_("Welcome to Bygfoot %s"), VERS);
                }
                else
                    wind = Variables.Window.Main as Gtk.Window;
                if (OptionHelper.OptInt("int_opt_maximize_main_window") != 0)
                    wind?.Maximize();
                break;
            case WindowsType.Startup:
                if (Variables.Window.Startup != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    StartupWindow.NewWithProperties([]);
                wind = Variables.Window.Startup as Gtk.Window;
                break;
            case WindowsType.Live:
                if (Variables.Window.Live != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    LiveWindow.NewWithProperties([]);
                if (Variables.Statp is LiveGame liveGame && liveGame.Fix != null)
                    buf = League.LeagueCupGetNameString(liveGame.Fix.Clid);
                wind = Variables.Window.Live as Gtk.Window;
                Window.WindowLiveSetUp();
                break;
            case WindowsType.Warning:
                if (Variables.Window.Warning != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    WarningWindow.NewWithProperties([]);
                wind = Variables.Window.Warning as Gtk.Window;
                buf = "Erm...";
                break;
            case WindowsType.Progress:
                if (Variables.Window.Progress != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    ProgressWindow.NewWithProperties([]);
                wind = Variables.Window.Progress as Gtk.Window;
                buf = "";
                break;
            case WindowsType.Digits:
                if (Variables.Window.Digits != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    DigitsWindow.NewWithProperties([]);
                wind = Variables.Window.Digits as Gtk.Window;
                buf = "Numbers...";
                break;
            case WindowsType.Stadium:
                if (Variables.Window.Stadium != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    StadiumWindow.NewWithProperties([]);
                wind = Variables.Window.Stadium as Gtk.Window;
                buf = "Your stadium";
                break;
            case WindowsType.JobOffer:
                if (Variables.Window.JobOffer != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    JobOfferWindow.NewWithProperties([]);
                wind = Variables.Window.JobOffer as Gtk.Window;
                buf = "Job offer";
                break;
            case WindowsType.Yesno:
                if (Variables.Window.Yesno != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    YesNoWindow.NewWithProperties([]);
                wind = Variables.Window.Yesno as Gtk.Window;
                buf = "???";
                break;
            case WindowsType.Options:
                if (Variables.Window.Options != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    OptionsWindow.NewWithProperties([]);
                wind = Variables.Window.Options as Gtk.Window;
                buf = "Options";
                break;
            case WindowsType.FontSel:
                if (Variables.Window.FontSel != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    FontSelWindow.NewWithProperties([]);
                wind = Variables.Window.FontSel as Gtk.Window;
                buf = "Select font";
                break;
            case WindowsType.FileChooser:
                if (Variables.Window.FileChooser != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    FileChooserWindow.NewWithProperties([]);
                wind = Variables.Window.FileChooser as Gtk.Window;
                break;
            case WindowsType.Contract:
                if (Variables.Window.Contract != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    ContractWindow.NewWithProperties([]);
                wind = Variables.Window.Contract as Gtk.Window;
                buf = "Contract offer";
                break;
            case WindowsType.UserManagement:
                if (Variables.Window.UserManagement != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    UserManagementWindow.NewWithProperties([]);
                wind = Variables.Window.UserManagement as Gtk.Window;
                buf = "User management";
                break;
            case WindowsType.Debug:
                if (Variables.Window.WDebug != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    DebugWindow.NewWithProperties([]);
                wind = Variables.Window.WDebug as Gtk.Window;
                buf = "Bygfoot debug window";
                break;
            case WindowsType.Help:
                if (Variables.Window.Help != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    HelpWindow.NewWithProperties([]);
                wind = Variables.Window.Help as Gtk.Window;
                break;
            case WindowsType.TransferDialog:
                if (Variables.Window.TransferDialog != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    TransferDialogWindow.NewWithProperties([]);
                wind = Variables.Window.TransferDialog as Gtk.Window;
                buf = "Transfer offer";
                break;
            case WindowsType.Sponsors:
                if (Variables.Window.Sponsors != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    SponsorsWindow.NewWithProperties([]);
                wind = Variables.Window.Sponsors as Gtk.Window;
                buf = "Sponsorship offers";
                break;
            case WindowsType.MMatches:
                if (Variables.Window.MMatches != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    MMatchesWindow.NewWithProperties([]);
                wind = Variables.Window.MMatches as Gtk.Window;
                buf = "Memorable matches";
                break;
            case WindowsType.Bets:
                if (Variables.Window.Bets != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    BetsWindow.NewWithProperties([]);
                wind = Variables.Window.Bets as Gtk.Window;
                buf = "Betting";
                break;
            case WindowsType.Splash:
                if (Variables.Window.Splash != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    SplashWindow.NewWithProperties([]);
                wind = Variables.Window.Splash as Gtk.Window;
                break;
            case WindowsType.TrainingCamp:
                if (Variables.Window.TrainingCamp != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    TrainingCampWindow.NewWithProperties([]);
                wind = Variables.Window.TrainingCamp as Gtk.Window;
                buf = "Training camp";
                break;
            case WindowsType.Alr:
                if (Variables.Window.Alr != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    AlrWindow.NewWithProperties([]);
                wind = Variables.Window.Alr as Gtk.Window;
                buf = "Automatic loan repayment";
                break;
            case WindowsType.News:
                if (Variables.Window.News != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    NewsWindow.NewWithProperties([]);
                wind = Variables.Window.News as Gtk.Window;
                buf = "Bygfoot News";
                break;
            case WindowsType.Constants:
                if (Variables.Window.Constants != null)
                    Debug.DebugPrintMessage("window_create: called on already existing window\n");
                else
                    ConstantsWindow.NewWithProperties([]);
                wind = Variables.Window.Constants as Gtk.Window;
                buf = "Bygfoot constants";
                break;
        }

        if (windowType != WindowsType.FileChooser)
            wind?.Title = buf;

        // C: g_timeout_add(20, window_show, wind) for all windows except
        // progress and splash, gtk_widget_show(wind) for those two. The C# window
        // subclasses already call Show() in their template initialization (the
        // splash being the exception, which C also shows immediately), and the
        // GTK4 bindings here have no g_timeout_add, so show directly: a no-op for
        // windows that are already visible.
        wind?.Show();

        return wind;
    }

    /// <summary>
    /// Destroy a window widget and set the popups and main window sensitivity
    /// correctly.
    /// Corresponds to C: void window_destroy(GtkWidget **wind).
    /// </summary>
    /// <param name="wind">The window we destroy.</param>
    public static void WindowDestroy(ref Gtk.Widget? wind)
    {
        Debug.DebugPrintMessage("window_destroy\n");

        if (wind == null)
            return;

        if (wind == Variables.Window.Splash)
        {
            Variables.Counters[(int)Counters.CountHintNumber] =
                (Variables.Counters[(int)Counters.CountHintNumber] + 1) % Variables.Hints.List.Count;

            Window.WindowSaveHintNumber();
        }

        (wind as Gtk.Window)?.Destroy();

        wind = null;
    }

    /// <summary>
    /// Show the training camp window for the current user.
    /// Corresponds to C: window_show_training_camp(void).
    /// </summary>
    public static void WindowShowTrainingCamp()
    {
        User user = Variables.Users[Variables.CurUser];

        Window.WindowCreate(WindowsType.TrainingCamp);

        // Initialize entry costs.
        string buf = Misc.MiscPrintGroupedInt(
            MathHelper.RoundInteger(
                (int)(Finance.WageUnit(user.Team) * OptionHelper.ConstFloat("float_training_camp_factor1")),
                -2));
        (Gui.LookupWidget(Variables.Window.TrainingCamp, "tf_costs") as Gtk.Entry)!.SetText(buf);

        // Initialize radio group camp.
        Gtk.ToggleButton? rbCamp1 =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "rb_camp1") as Gtk.ToggleButton;
        Gtk.ToggleButton? rbCamp2 =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "rb_camp2") as Gtk.ToggleButton;
        Gtk.ToggleButton? rbCamp3 =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "rb_camp3") as Gtk.ToggleButton;

        if (OptionHelper.OptUserInt("int_opt_user_training_camp_hotel") ==
            (int)TrainingCampHotel.TrainingCampHotelGood)
            rbCamp1!.Active = true;
        else if (OptionHelper.OptUserInt("int_opt_user_training_camp_hotel") ==
            (int)TrainingCampHotel.TrainingCampHotelFirst)
            rbCamp2!.Active = true;
        else
            rbCamp3!.Active = true;

        // Initialize labels.
        // C: gtk_misc_set_alignment(l_costs/l_recreation/l_training/l_camp_points,
        // 0.0, 0.5) — GtkMisc does not exist in GTK4 (use Halign instead), omitted.

        // Initialize hScales.
        Gtk.Scale? hsRecreation =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "hs_recreation") as Gtk.Scale;
        Gtk.Scale? hsTraining =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "hs_training") as Gtk.Scale;
        Gtk.Scale? hsCampPoints =
            Gui.LookupWidget(Variables.Window.TrainingCamp, "hs_camp_points") as Gtk.Scale;
        hsRecreation!.SetRange(CampScaleMin, CampScaleMax);
        hsTraining!.SetRange(CampScaleMin, CampScaleMax);
        hsCampPoints!.SetRange(CampScaleMin, CampScaleMax);
        hsRecreation!.SetValue(OptionHelper.OptUserInt("int_opt_user_training_camp_recreation"));
        hsTraining!.SetValue(CampScaleMax - OptionHelper.OptUserInt("int_opt_user_training_camp_recreation"));
        hsCampPoints!.SetValue(CampScaleMin);
        hsRecreation!.SetDigits(0);
        hsTraining!.SetDigits(0);
        hsCampPoints!.SetDigits(0);
        hsRecreation!.Sensitive = false;
        hsTraining!.Sensitive = false;
        hsCampPoints!.Sensitive = false;

        // C: gdk_color_parse("black", &color); gtk_widget_modify_fg/bg(...) —
        // gtk_widget_modify_fg/bg were removed in GTK4 (use CSS styling instead),
        // omitted.
    }

    /// <summary>
    /// Create and set up the debt repayment window.
    /// Corresponds to C: window_show_alr(void).
    /// </summary>
    public static void WindowShowAlr()
    {
        User user = Variables.Users[Variables.CurUser];

        Window.WindowCreate(WindowsType.Alr);

        string weeklyInstallment = Misc.MiscPrintGroupedInt(user.AlrWeeklyInstallment);
        string debt = Misc.MiscPrintGroupedInt(-user.Debt);

        Gui.GuiLabelSetTextFromInt(
            (Gui.LookupWidget(Variables.Window.Alr, "label_current_start_week") as Gtk.Label)!,
            user.AlrStartWeek, false);
        (Gui.LookupWidget(Variables.Window.Alr, "label_current_weekly_installment") as Gtk.Label)!.SetLabel(
            weeklyInstallment);
        (Gui.LookupWidget(Variables.Window.Alr, "label_alr_debt") as Gtk.Label)!.SetLabel(debt);

        (Gui.LookupWidget(Variables.Window.Alr, "spinbutton_start_week") as Gtk.SpinButton)!.SetRange(
            Variables.Week + 1,
            Math.Min(Variables.Week + user.Counters[(int)CounterType.Loan],
                Fixture.GetLastScheduledWeek()));
        (Gui.LookupWidget(Variables.Window.Alr, "spinbutton_weekly_installment") as Gtk.SpinButton)!.SetRange(
            0,
            -user.Debt * (double)MathF.Pow(1 + user.DebtInterest,
                OptionHelper.ConstInt("int_finance_payback_weeks")) + 1);
    }

    /// <summary>
    /// Show the window where the user can edit constants determining gameplay
    /// behaviour.
    /// Corresponds to C: window_show_constants(void).
    /// </summary>
    public static void WindowShowConstants()
    {
        Window.WindowCreate(WindowsType.Constants);

        // TODO: treeview2_show_constants (src/treeview2.c not yet converted).
    }
}
