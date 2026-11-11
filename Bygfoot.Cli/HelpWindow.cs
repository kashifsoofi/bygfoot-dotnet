using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Help window of the game. It shows the help notebook with the about, the
/// contributors and the help pages and a close button. Corresponds to the C
/// function create_window_help and the widget "window_help" of the bygfoot_misc2
/// glade support file, which has been converted into the GTK4 builder UI file
/// help.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(HelpWindow))]
[Gtk.Template<Gtk.AssemblyResource>("help.ui")]
public partial class HelpWindow
{
    [Gtk.Connect("button_help_close")]
    private Gtk.Button _buttonHelpClose;

    partial void Initialize()
    {
        DefaultWidth = 400;
        DefaultHeight = 400;
        Modal = true;

        Variables.Window.Help = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonHelpClose.OnClicked += OnButtonHelpCloseClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_help_delete_event of the close-request signal, which itself
    /// calls on_button_help_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_help_delete_event (which calls on_button_help_close_clicked and destroys the help window).
        return true;
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_help_close_clicked of the "clicked" signal, which destroys the help window.
    /// </summary>
    private void OnButtonHelpCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_help_close_clicked (which destroys the help window).
    }
}
