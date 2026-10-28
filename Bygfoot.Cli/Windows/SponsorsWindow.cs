namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Sponsors window of the game. It is shown when the game offers sponsors for
/// the team and lets the user accept or reject them.
/// Corresponds to the C function game_gui_show_sponsors and the widget
/// "window_sponsors" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file sponsors.ui.
/// </summary>
public class SponsorsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public SponsorsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("sponsors.ui");

        var window = (Gtk.Window)_builder.GetObject("window_sponsors") ?? throw new InvalidOperationException("Could not find the widget 'window_sponsors' in the UI file 'sponsors.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.Sponsors = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonSponsors = (Gtk.Button)_builder.GetObject("button_sponsors") ?? throw new InvalidOperationException("Could not find the widget 'button_sponsors' in the UI file 'sponsors.ui'.");
        buttonSponsors.OnClicked += OnButtonSponsorsClicked;

        var buttonSponsorsWait = (Gtk.Button)_builder.GetObject("button_sponsors_wait") ?? throw new InvalidOperationException("Could not find the widget 'button_sponsors_wait' in the UI file 'sponsors.ui'.");
        buttonSponsorsWait.OnClicked += OnButtonSponsorsWaitClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_sponsors_delete_event of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_sponsors_delete_event (which closes the sponsors window).
        return true;
    }

    /// <summary>
    /// The "Accept" button was clicked. Corresponds to the C handler
    /// on_button_sponsors_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonSponsorsClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_sponsors_clicked (which accepts the sponsor via misc_callback_new_sponsor and destroys the window).
    }

    /// <summary>
    /// The "Reject for now" button was clicked. Corresponds to the C handler
    /// on_button_sponsors_wait_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonSponsorsWaitClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_sponsors_wait_clicked (which rejects the sponsor and destroys the window).
    }
}
