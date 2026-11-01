namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Debug window of the game. It shows a debug text entry, an "Apply" button and
/// a "Close" button. Corresponds to the C function create_window_debug and the
/// widget "window_debug" of the bygfoot_misc2.glade support file, which
/// has been converted into the GTK4 builder UI file debug.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(DebugWindow))]
[Gtk.Template<Gtk.AssemblyResource>("debug.ui")]
public partial class DebugWindow
{
    [Gtk.Connect("button_debug_apply")]
    private Gtk.Button _buttonDebugApply;

    [Gtk.Connect("button_debug_close")]
    private Gtk.Button _buttonDebugClose;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.WDebug = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonDebugApply.OnClicked += OnButtonDebugApplyClicked;
        _buttonDebugClose.OnClicked += OnButtonDebugCloseClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_debug_delete_event of the close-request signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_debug_delete_event (which destroys the debug window).
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