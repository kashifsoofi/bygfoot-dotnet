namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Main window of the game. It is shown while a game is running and lets the
/// user load/save/quit, browse leagues and manage the team.
/// Corresponds to the C function create_main_window and the widget
/// "main_window" of the bygfoot.glade support file, which has been
/// converted into the GTK4 builder UI file main.ui.
/// </summary>
public class MainWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public MainWindow()
    {
        _builder = Gtk.Builder.NewFromFile("main.ui");

        var window = (Gtk.Window)_builder.GetObject("main_window") ?? throw new InvalidOperationException("Could not find the widget 'main_window' in the UI file 'main.ui'.");

        window.DefaultWidth = 300;
        window.DefaultHeight = 600;

        Variables.Window.Main = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonLoad = (Gtk.Button)_builder.GetObject("button_load") ?? throw new InvalidOperationException("Could not find the widget 'button_load' in the UI file 'main.ui'.");
        buttonLoad.OnClicked += OnButtonLoadClicked;

        var buttonSave = (Gtk.Button)_builder.GetObject("button_save") ?? throw new InvalidOperationException("Could not find the widget 'button_save' in the UI file 'main.ui'.");
        buttonSave.OnClicked += OnButtonSaveClicked;

        var buttonQuit = (Gtk.Button)_builder.GetObject("button_quit") ?? throw new InvalidOperationException("Could not find the widget 'button_quit' in the UI file 'main.ui'.");
        buttonQuit.OnClicked += OnButtonQuitClicked;

        var buttonBackToMain = (Gtk.Button)_builder.GetObject("button_back_to_main") ?? throw new InvalidOperationException("Could not find the widget 'button_back_to_main' in the UI file 'main.ui'.");
        buttonBackToMain.OnClicked += OnButtonBackToMainClicked;

        var buttonTransfers = (Gtk.Button)_builder.GetObject("button_transfers") ?? throw new InvalidOperationException("Could not find the widget 'button_transfers' in the UI file 'main.ui'.");
        buttonTransfers.OnClicked += OnButtonTransfersClicked;

        var buttonNewWeek = (Gtk.Button)_builder.GetObject("button_new_week") ?? throw new InvalidOperationException("Could not find the widget 'button_new_week' in the UI file 'main.ui'.");
        buttonNewWeek.OnClicked += OnButtonNewWeekClicked;

        var buttonResetPlayers = (Gtk.Button)_builder.GetObject("button_reset_players") ?? throw new InvalidOperationException("Could not find the widget 'button_reset_players' in the UI file 'main.ui'.");
        buttonResetPlayers.OnClicked += OnButtonResetPlayersClicked;

        var buttonClBack = (Gtk.Button)_builder.GetObject("button_cl_back") ?? throw new InvalidOperationException("Could not find the widget 'button_cl_back' in the UI file 'main.ui'.");
        buttonClBack.OnClicked += OnButtonClBackClicked;

        var buttonClForward = (Gtk.Button)_builder.GetObject("button_cl_forward") ?? throw new InvalidOperationException("Could not find the widget 'button_cl_forward' in the UI file 'main.ui'.");
        buttonClForward.OnClicked += OnButtonClForwardClicked;

        var buttonBrowseBack = (Gtk.Button)_builder.GetObject("button_browse_back") ?? throw new InvalidOperationException("Could not find the widget 'button_browse_back' in the UI file 'main.ui'.");
        buttonBrowseBack.OnClicked += OnButtonBrowseBackClicked;

        var buttonBrowseForward = (Gtk.Button)_builder.GetObject("button_browse_forward") ?? throw new InvalidOperationException("Could not find the widget 'button_browse_forward' in the UI file 'main.ui'.");
        buttonBrowseForward.OnClicked += OnButtonBrowseForwardClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_button_quit_clicked of the delete_event signal of the main window.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_button_quit_clicked (which exits the program).
        return true;
    }

    /// <summary>
    /// The "Load game" button was clicked. Corresponds to the C handler
    /// on_button_load_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonLoadClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_load_clicked (which loads the game).
    }

    /// <summary>
    /// The "Save game" button was clicked. Corresponds to the C handler
    /// on_button_save_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonSaveClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_save_clicked (which saves the game).
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_button_quit_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonQuitClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_quit_clicked (which exits the program).
    }

    /// <summary>
    /// The "Back to main" button was clicked. Corresponds to the C handler
    /// on_button_back_to_main_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonBackToMainClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_back_to_main_clicked (which goes back to the main window).
    }

    /// <summary>
    /// The "Transfers" button was clicked. Corresponds to the C handler
    /// on_button_transfers_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonTransfersClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_transfers_clicked (which shows the transfer list).
    }

    /// <summary>
    /// The "Begin a new week" button was clicked. Corresponds to the C handler
    /// on_button_new_week_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonNewWeekClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_new_week_clicked (which begins a new week).
    }

    /// <summary>
    /// The "Reset player list" button was clicked. Corresponds to the C handler
    /// on_button_reset_players_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonResetPlayersClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_reset_players_clicked (which resets the player list).
    }

    /// <summary>
    /// The "Previous league/cup" button was clicked. Corresponds to the C handler
    /// on_button_cl_back_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonClBackClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_cl_back_clicked (which goes to the previous league/cup).
    }

    /// <summary>
    /// The "Next league/cup" button was clicked. Corresponds to the C handler
    /// on_button_cl_forward_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonClForwardClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_cl_forward_clicked (which goes to the next league/cup).
    }

    /// <summary>
    /// The "Previous" button was clicked. Corresponds to the C handler
    /// on_button_browse_back_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonBrowseBackClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_browse_back_clicked (which goes to the previous browse position).
    }

    /// <summary>
    /// The "Next" button was clicked. Corresponds to the C handler
    /// on_button_browse_forward_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonBrowseForwardClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_browse_forward_clicked (which goes to the next browse position).
    }
}
