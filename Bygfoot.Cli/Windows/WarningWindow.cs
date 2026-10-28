namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Warning window of the game. It shows a warning image, a warning text and a
/// close button. Corresponds to the C function create_window_warning and the widget
/// "window_warning" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file warning.ui.
/// </summary>
public class WarningWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public WarningWindow()
    {
        _builder = Gtk.Builder.NewFromFile("warning.ui");

        var window = (Gtk.Window)_builder.GetObject("window_warning") ?? throw new InvalidOperationException("Could not find the widget 'window_warning' in the UI file 'warning.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.Warning = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonWarning = (Gtk.Button)_builder.GetObject("button_warning") ?? throw new InvalidOperationException("Could not find the widget 'button_warning' in the UI file 'warning.ui'.");
        buttonWarning.OnClicked += OnButtonWarningClicked;

        window.Show();
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
