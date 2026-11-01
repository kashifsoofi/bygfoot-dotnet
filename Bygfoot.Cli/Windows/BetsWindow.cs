namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Bets window of the game. It shows the bets list with the filter check
/// buttons and a close button. Corresponds to the C function create_window_bets
/// and the widget "window_bets" of the bygfoot_misc3.glade support file, which
/// has been converted into the GTK4 builder UI file bets.ui.
/// </summary>
[Gtk.Template<Gtk.Window>(qualifiedName: nameof(BetsWindow))]
[Gtk.Template<Gtk.AssemblyResource>("bets.ui")]
public partial class BetsWindow : Gtk.Window
{
    [Gtk.Connect("button_bet_close")]
    private Gtk.Button _buttonBetClose;

    partial void Initialize()
    {
        DefaultWidth = 600;
        DefaultHeight = 700;
        Modal = true;

        Variables.Window.Bets = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonBetClose.OnClicked += OnButtonBetCloseClicked;

        Show();
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
