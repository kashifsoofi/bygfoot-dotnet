namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Bets window of the game. It shows the bets list with the filter check
/// buttons and a close button. Corresponds to the C function create_window_bets
/// and the widget "window_bets" of the bygfoot_misc3.glade support file, which
/// has been converted into the GTK4 builder UI file bets.ui.
/// </summary>
public class BetsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public BetsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("bets.ui");

        var window = (Gtk.Window)_builder.GetObject("window_bets") ?? throw new InvalidOperationException("Could not find the widget 'window_bets' in the UI file 'bets.ui'.");

        window.Modal = true;
        window.DefaultWidth = 600;
        window.DefaultHeight = 700;

        Variables.Window.Bets = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonBetClose = (Gtk.Button)_builder.GetObject("button_bet_close") ?? throw new InvalidOperationException("Could not find the widget 'button_bet_close' in the UI file 'bets.ui'.");
        buttonBetClose.OnClicked += OnButtonBetCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_bets_delete_event of the close-request signal, which itself
    /// calls on_button_bet_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_bets_delete_event (which calls on_button_bet_close_clicked and destroys the bets window).
        return true;
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_bet_close_clicked of the "clicked" signal, which destroys the bets window.
    /// </summary>
    private void OnButtonBetCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_bet_close_clicked (which destroys the bets window).
    }
}
