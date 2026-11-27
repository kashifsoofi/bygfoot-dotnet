using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Startup window of the game. It is shown before a game is started and lets
/// the user choose the country, team, league and username.
/// Corresponds to the C function create_window_startup and the widget
/// "window_startup" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file startup.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(StartupWindow))]
[Gtk.Template<Gtk.AssemblyResource>("startup.ui")]
public partial class StartupWindow
{
    [Gtk.Connect("button_add_player")]
    private Gtk.Button _buttonAddPlayer;

    [Gtk.Connect("team_selection_ok")]
    private Gtk.Button _teamSelectionOk;

    [Gtk.Connect("button_team_selection_back")]
    private Gtk.Button _buttonTeamSelectionBack;

    [Gtk.Connect("team_selection_cancel")]
    private Gtk.Button _teamSelectionCancel;

    [Gtk.Connect("combo_country")]
    private Gtk.ComboBox _comboCountry;

    [Gtk.Connect("combobox_start_league")]
    private Gtk.ComboBox _comboBoxStartLeague;

    [Gtk.Connect("entry_player_name")]
    private Gtk.Entry _entryPlayerName;

    [Gtk.Connect("radiobutton_team_def_load")]
    private Gtk.CheckButton _radioButtonTeamDefLoad;

    [Gtk.Connect("radiobutton_team_def_names")]
    private Gtk.CheckButton _radioButtonTeamDefNames;

    [Gtk.Connect("checkbutton_randomise_teams")]
    private Gtk.CheckButton _checkbuttonRandomiseTeams;

    partial void Initialize()
    {
        DefaultWidth = 550;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.Startup = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonAddPlayer.OnClicked += OnButtonAddPlayerClicked;
        _teamSelectionOk.OnClicked += OnTeamSelectionOkClicked;
        _buttonTeamSelectionBack.OnClicked += OnButtonTeamSelectionBackClicked;
        _teamSelectionCancel.OnClicked += OnTeamSelectionCancelClicked;
        _comboCountry.OnChanged += OnComboCountryChanged;
        _entryPlayerName.OnActivate += OnEntryPlayerNameActivate;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_team_selection_cancel_clicked of the delete_event signal, which
    /// exits the program.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // C: main_exit_program(EXIT_OK, NULL);
        Main.MainExitProgram(Bygfoot.ExitCode.Ok, null);

        // C: return TRUE - the event is handled, so the default handler
        // does not close the window (the program is exiting anyway).
        return true;
    }

    /// <summary>
    /// The "Add user" button was clicked. Corresponds to the C function
    /// misc_callback_add_player (src/misc_callback_func.c).
    /// </summary>
    private void OnButtonAddPlayerClicked(Button sender, EventArgs args)
    {
        Debug.DebugPrintMessage("misc_callback_add_player\n");

        // C: const gchar *player_name = gtk_entry_get_text(entry_player_name);
        string? playerName = _entryPlayerName.GetText();

        // C: User new_user = user_new();
        User newUser = User.New();

        // C: if (strlen(player_name) > 0) misc_string_assign(&new_user.name, player_name);
        if (playerName.Length > 0)
            newUser.Name = playerName;

        // C: gtk_entry_set_text(entry_player_name, "");
        _entryPlayerName.SetText(string.Empty);

        // TODO: the selected team cannot be read - C:
        //     Team *tm = treeview_helper_get_pointer(treeview_startup, 2);
        // (src/treeview_helper.c not yet converted).
        // C: new_user.tm = tm;
        //     new_user.team_id = tm->id;

        // TODO: the start league combo is not filled - C:
        //     gint start_league = gtk_combo_box_get_active(combo_leagues);
        //     new_user.scout = (start_league == 0 || tm->clid == lig(start_league - 1).id)
        //                           ? -1 : start_league - 1;
        // (src/treeview.c not yet converted).

        // C: g_array_append_val(users, new_user);
        Variables.Users.Add(newUser);

        // TODO: treeview_show_users(treeview_users) and
        //       treeview_show_team_list(treeview_startup, FALSE, FALSE)
        // (src/treeview.c not yet converted).

        // C: gtk_combo_box_set_active(combo_leagues, 0);
        _comboBoxStartLeague.SetActive(0);

        if (Variables.Users.Count == 1)
        {
            _teamSelectionOk.Sensitive = true;
            _comboCountry.Sensitive = false;
        }
    }

    /// <summary>
    /// The "Start" button was clicked. Corresponds to the C function
    /// misc_callback_start_game (src/misc_callback_func.c), which starts
    /// a new game after users and teams are selected.
    /// </summary>
    private void OnTeamSelectionOkClicked(Button sender, EventArgs args)
    {
        Debug.DebugPrintMessage("misc_callback_start_game\n");

        // C: if (gtk_toggle_button_get_active(checkbutton_randomise_teams))
        //        opt_set_int("int_opt_randomise_teams", 1);
        if (_checkbuttonRandomiseTeams.Active)
            OptionHelper.OptSetInt("int_opt_randomise_teams", 1);

        // C: if (gtk_toggle_button_get_active(radio_load))
        //        opt_set_int("int_opt_load_defs", 1);
        //     else if (gtk_toggle_button_get_active(radio_names))
        //        opt_set_int("int_opt_load_defs", 2);
        //     else
        //        opt_set_int("int_opt_load_defs", 0);
        if (_radioButtonTeamDefLoad.Active)
            OptionHelper.OptSetInt("int_opt_load_defs", 1);
        else if (_radioButtonTeamDefNames.Active)
            OptionHelper.OptSetInt("int_opt_load_defs", 2);
        else
            OptionHelper.OptSetInt("int_opt_load_defs", 0);

        // C: stat0 = STATUS_MAIN;
        Bygfoot.Stat0 = (int)Status0Value.StatusMain;

        // C: start_new_game();
        StartEnd.StartNewGame();

        // C: window_destroy(&window.startup);
        Gtk.Widget? startup = Variables.Window.Startup;
        Window.WindowDestroy(ref startup);
        Variables.Window.Startup = startup;

        // C: file_store_text_in_saves("last_country", country.sid);
        FileHelper.StoreTextInSaves("last_country", Variables.Country.SId);

        if (OptionHelper.OptInt("int_opt_calodds") == 0)
        {
            // C: for (i = 0; i < users->len; i++) user_set_up_team_new_game(&usr(i));
            foreach (User user in Variables.Users)
                User.SetUpTeamNewGame(user);

            // C: window_create(WINDOW_MAIN);
            Window.WindowCreate(WindowsType.Main);

            // TODO: game_gui_show_main (src/game_gui.c not yet converted).

            // C: if (statp != NULL) { debug_action(statp); g_free(statp); statp = NULL; }
            if (Variables.Statp is string action)
            {
                Debug.DebugAction(action);
                Variables.Statp = null;
            }
        }
        else
        {
            // C: free_users(TRUE);
            Free.FreeUsers(true);

            // C: debug_calibrate_betting_odds(opt_int(...), opt_int(...));
            Debug.DebugCalibrateBettingOdds(OptionHelper.OptInt("int_opt_calodds_skilldiffmax"),
                OptionHelper.OptInt("int_opt_calodds_matches"));

            // C: main_exit_program(EXIT_OK, NULL);
            Main.MainExitProgram(Bygfoot.ExitCode.Ok, null);
        }
    }

    /// <summary>
    /// The "Back to splash" button was clicked. Corresponds to the C handler
    /// on_button_team_selection_back_clicked of the "clicked" signal, which
    /// destroys the startup window and shows the splash window again.
    /// </summary>
    private void OnButtonTeamSelectionBackClicked(Button sender, EventArgs args)
    {
        // C: window_destroy(&window.startup);
        Gtk.Widget? startup = Variables.Window.Startup;
        Window.WindowDestroy(ref startup);
        Variables.Window.Startup = startup;

        // C: stat0 = STATUS_SPLASH;
        Bygfoot.Stat0 = (int)Status0Value.StatusSplash;

        // C: window_show_splash();
        Window.WindowShowSplash();
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_team_selection_cancel_clicked of the "clicked" signal, which
    /// exits the program.
    /// </summary>
    private void OnTeamSelectionCancelClicked(Button sender, EventArgs args)
    {
        // C: main_exit_program(EXIT_OK, NULL);
        Main.MainExitProgram(Bygfoot.ExitCode.Ok, null);
    }

    /// <summary>
    /// The country combo box changed. Corresponds to the C handler
    /// on_combo_country_changed of the "changed" signal.
    /// </summary>
    private void OnComboCountryChanged(ComboBox sender, EventArgs args)
    {
        Debug.DebugPrintMessage("on_combo_country_changed\n");

        // C: GtkTreeIter iter; gtk_combo_box_get_active_iter(combobox, &iter);
        if (!sender.GetActiveIter(out Gtk.TreeIter iter))
            return;

        // C: if (!gtk_tree_model_iter_has_child(model, &iter))
        //        misc_callback_show_team_list(combobox, buf);
        //     else
        //        gtk_widget_set_sensitive(button_add_player, FALSE);
        if (sender.Model is not null && sender.Model.IterHasChild(iter))
            _buttonAddPlayer.Sensitive = false;
        else
        {
            // TODO: misc_callback_show_team_list (src/misc_callback_func.c not yet
            // converted) - the country file of the active row (C: column 1 of the
            // model, src/treeview.c not yet converted) cannot be read back through
            // the GirCore binding.
        }
    }

    /// <summary>
    /// The player name entry was activated. Corresponds to the C handler
    /// on_entry_player_name_activate of the "activate" signal.
    /// </summary>
    private void OnEntryPlayerNameActivate(Entry sender, EventArgs args)
    {
        Debug.DebugPrintMessage("on_entry_player_name_activate\n");

        // C: on_button_add_player_clicked(NULL, NULL);
        OnButtonAddPlayerClicked(null, EventArgs.Empty);
    }
}
