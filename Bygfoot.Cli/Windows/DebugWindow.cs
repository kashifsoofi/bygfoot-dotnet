namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Debug window of the game. It shows a debug text entry, an "Apply" button and
/// a "Close" button. Corresponds to the C function create_window_debug and the
/// widget "window_debug" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file debug.ui.
/// </summary>
public class DebugWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public DebugWindow()
    {
        _builder = Gtk.Builder.NewFromFile("debug.ui");

        var window = (Gtk.Window)_builder.GetObject("window_debug") ?? throw new InvalidOperationException("Could not find the widget 'window_debug' in the UI file 'debug.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.WDebug = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonDebugApply = (Gtk.Button)_builder.GetObject("button_debug_apply") ?? throw new InvalidOperationException("Could not find the widget 'button_debug_apply' in the UI file 'debug.ui'.");
        buttonDebugApply.OnClicked += OnButtonDebugApplyClicked;

        var buttonDebugClose = (Gtk.Button)_builder.GetObject("button_debug_close") ?? throw new InvalidOperationException("Could not find the widget 'button_debug_close' in the UI file 'debug.ui'.");
        buttonDebugClose.OnClicked += OnButtonDebugCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_debug_delete_event of the close-request signal, which itself
    /// calls on_button_debug_close_activate.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_debug_delete_event (which calls on_button_debug_close_activate and destroys the debug window).
        return true;
    }

    /// <summary>
    /// The "Apply" button was clicked. Corresponds to the C handler
    /// on_button_debug_apply_clicked of the "clicked" signal, which applies the debug entry.
    /// </summary>
    private void OnButtonDebugApplyClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_debug_apply_clicked (which applies the debug entry).
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_debug_close_activate of the "clicked" signal, which destroys the debug window.
    /// </summary>
    private void OnButtonDebugCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_debug_close_activate (which destroys the debug window).
    }
}
