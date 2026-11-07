using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Warning window of the game. It shows a warning image, a warning text and a
/// close button. Corresponds to the C function create_window_warning and the widget
/// "window_warning" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file warning.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(WarningWindow))]
[Gtk.Template<Gtk.AssemblyResource>("warning.ui")]
public partial class WarningWindow
{
    [Gtk.Connect("button_warning")]
    private Gtk.Button _buttonWarning;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.Warning = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonWarning.OnClicked += OnButtonWarningClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" / "destroy_event" signals of GTK3, both of which pointed
    /// to the C handler on_button_warning_clicked). Corresponds to the C handler
    /// on_button_warning_clicked of the close-request signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_button_warning_clicked (which destroys the warning window and shows the next event).
        return true;
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_warning_clicked of the "clicked" signal, which destroys the warning window and shows the next event.
    /// </summary>
    private void OnButtonWarningClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_warning_clicked (which destroys the warning window and shows the next event).
    }
}
