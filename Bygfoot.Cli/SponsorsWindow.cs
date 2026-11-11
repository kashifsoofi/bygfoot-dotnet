using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Sponsors window of the game. It is shown when the game offers sponsors for
/// the team and lets the user accept or reject them.
/// Corresponds to the C function game_gui_show_sponsors and the widget
/// "window_sponsors" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file sponsors.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(SponsorsWindow))]
[Gtk.Template<Gtk.AssemblyResource>("sponsors.ui")]
public partial class SponsorsWindow
{
    [Gtk.Connect("button_sponsors")]
    private Gtk.Button _buttonSponsors;

    [Gtk.Connect("button_sponsors_wait")]
    private Gtk.Button _buttonSponsorsWait;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.Sponsors = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonSponsors.OnClicked += OnButtonSponsorsClicked;
        _buttonSponsorsWait.OnClicked += OnButtonSponsorsWaitClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_sponsors_delete_event of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
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
