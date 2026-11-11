using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Main window of the game. It is shown while a game is running and lets the
/// user load/save/quit, browse leagues and manage the team.
/// Corresponds to the C function create_main_window and the widget
/// "main_window" of the bygfoot.glade support file, which has been
/// converted into the GTK4 builder UI file main.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(MainWindow))]
[Gtk.Template<Gtk.AssemblyResource>("main.ui")]
public partial class MainWindow
{
    [Gtk.Connect("button_load")]
    private Gtk.Button _buttonLoad;

    [Gtk.Connect("button_save")]
    private Gtk.Button _buttonSave;

    [Gtk.Connect("button_quit")]
    private Gtk.Button _buttonQuit;

    [Gtk.Connect("button_back_to_main")]
    private Gtk.Button _buttonBackToMain;

    [Gtk.Connect("button_transfers")]
    private Gtk.Button _buttonTransfers;

    [Gtk.Connect("button_new_week")]
    private Gtk.Button _buttonNewWeek;

    [Gtk.Connect("button_reset_players")]
    private Gtk.Button _buttonResetPlayers;

    [Gtk.Connect("button_cl_back")]
    private Gtk.Button _buttonClBack;

    [Gtk.Connect("button_cl_forward")]
    private Gtk.Button _buttonClForward;

    [Gtk.Connect("button_browse_back")]
    private Gtk.Button _buttonBrowseBack;

    [Gtk.Connect("button_browse_forward")]
    private Gtk.Button _buttonBrowseForward;

    partial void Initialize()
    {
        DefaultWidth = 300;
        DefaultHeight = 600;

        Variables.Window.Main = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonLoad.OnClicked += OnButtonLoadClicked;
        _buttonSave.OnClicked += OnButtonSaveClicked;
        _buttonQuit.OnClicked += OnButtonQuitClicked;
        _buttonBackToMain.OnClicked += OnButtonBackToMainClicked;
        _buttonTransfers.OnClicked += OnButtonTransfersClicked;
        _buttonNewWeek.OnClicked += OnButtonNewWeekClicked;
        _buttonResetPlayers.OnClicked += OnButtonResetPlayersClicked;
        _buttonClBack.OnClicked += OnButtonClBackClicked;
        _buttonClForward.OnClicked += OnButtonClForwardClicked;
        _buttonBrowseBack.OnClicked += OnButtonBrowseBackClicked;
        _buttonBrowseForward.OnClicked += OnButtonBrowseForwardClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_button_quit_clicked of the delete_event signal of the main window.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
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
