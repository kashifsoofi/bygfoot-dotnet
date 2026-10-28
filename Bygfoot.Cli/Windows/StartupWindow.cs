namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Startup window of the game. It is shown before a game is started and lets
/// the user choose the country, team, league and username.
/// Corresponds to the C function create_window_startup and the widget
/// "window_startup" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file startup.ui.
/// </summary>
public class StartupWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public StartupWindow()
    {
        _builder = Gtk.Builder.NewFromFile("startup.ui");

        var window = (Gtk.Window)_builder.GetObject("window_startup") ?? throw new InvalidOperationException("Could not find the widget 'window_startup' in the UI file 'startup.ui'.");

        window.Modal = true;
        window.DefaultWidth = 550;
        window.DefaultHeight = 600;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonAddPlayer = (Gtk.Button)_builder.GetObject("button_add_player") ?? throw new InvalidOperationException("Could not find the widget 'button_add_player' in the UI file 'startup.ui'.");
        buttonAddPlayer.OnClicked += OnButtonAddPlayerClicked;

        var teamSelectionOk = (Gtk.Button)_builder.GetObject("team_selection_ok") ?? throw new InvalidOperationException("Could not find the widget 'team_selection_ok' in the UI file 'startup.ui'.");
        teamSelectionOk.OnClicked += OnTeamSelectionOkClicked;

        var teamSelectionBack = (Gtk.Button)_builder.GetObject("button_team_selection_back") ?? throw new InvalidOperationException("Could not find the widget 'button_team_selection_back' in the UI file 'startup.ui'.");
        teamSelectionBack.OnClicked += OnButtonTeamSelectionBackClicked;

        var teamSelectionCancel = (Gtk.Button)_builder.GetObject("team_selection_cancel") ?? throw new InvalidOperationException("Could not find the widget 'team_selection_cancel' in the UI file 'startup.ui'.");
        teamSelectionCancel.OnClicked += OnTeamSelectionCancelClicked;

        var comboCountry = (Gtk.ComboBox)_builder.GetObject("combo_country") ?? throw new InvalidOperationException("Could not find the widget 'combo_country' in the UI file 'startup.ui'.");
        comboCountry.OnChanged += OnComboCountryChanged;

        var entryPlayerName = (Gtk.Entry)_builder.GetObject("entry_player_name") ?? throw new InvalidOperationException("Could not find the widget 'entry_player_name' in the UI file 'startup.ui'.");
        entryPlayerName.OnActivate += OnEntryPlayerNameActivate;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_team_selection_cancel_clicked of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
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
