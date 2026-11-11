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

    [Gtk.Connect("entry_player_name")]
    private Gtk.Entry _entryPlayerName;

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
    /// on_team_selection_cancel_clicked of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // TODO: Call the C function main_exit_program(EXIT_OK, NULL).
        return true;
    }

    /// <summary>
    /// The "Add user" button was clicked. Corresponds to the C handler
    /// on_button_add_player_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonAddPlayerClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function misc_callback_add_player.
    }

    /// <summary>
    /// The "Start" button was clicked. Corresponds to the C handler
    /// on_team_selection_ok_clicked of the "clicked" signal.
    /// </summary>
    private void OnTeamSelectionOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function misc_callback_start_game.
    }

    /// <summary>
    /// The "Back to splash" button was clicked. Corresponds to the C handler
    /// on_button_team_selection_back_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonTeamSelectionBackClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function window_destroy and show the splash window again.
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_team_selection_cancel_clicked of the "clicked" signal.
    /// </summary>
    private void OnTeamSelectionCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function main_exit_program(EXIT_OK, NULL).
    }

    /// <summary>
    /// The country combo box changed. Corresponds to the C handler
    /// on_combo_country_changed of the "changed" signal.
    /// </summary>
    private void OnComboCountryChanged(ComboBox sender, EventArgs args)
    {
        // TODO: Call the C function misc_callback_show_team_list.
    }

    /// <summary>
    /// The player name entry was activated. Corresponds to the C handler
    /// on_entry_player_name_activate of the "activate" signal.
    /// </summary>
    private void OnEntryPlayerNameActivate(Entry sender, EventArgs args)
    {
        // TODO: Call the C function on_button_add_player_clicked.
    }
}
