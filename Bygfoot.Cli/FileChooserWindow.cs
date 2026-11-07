using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// File selection window of the game. It is shown when the user wants to load
/// or save a game or a memorable match and lets him choose the file.
/// Corresponds to the C function window_show_file_sel and the widget
/// "window_file_chooser" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file file_chooser.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(FileChooserWindow))]
[Gtk.Template<Gtk.AssemblyResource>("file_chooser.ui")]
public partial class FileChooserWindow
{
    [Gtk.Connect("button_file_chooser_cancel")]
    private Gtk.Button _buttonFileChooserCancel;

    [Gtk.Connect("button_file_chooser_open")]
    private Gtk.Button _buttonFileChooserOpen;

    partial void Initialize()
    {
        DefaultWidth = 550;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.FileChooser = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonFileChooserCancel.OnClicked += OnButtonFileChooserCancelClicked;
        _buttonFileChooserOpen.OnClicked += OnButtonFileChooserOpenClicked;

        Show();
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C flow in
    /// window_show_file_sel and the cancel response of the file chooser dialog.
    /// </summary>
    private void OnButtonFileChooserCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function window_show_file_sel and handle the cancel response (then window_destroy the file chooser window).
    }

    /// <summary>
    /// The "Open" button was clicked. Corresponds to the C flow in
    /// window_show_file_sel and the open response of the file chooser dialog.
    /// </summary>
    private void OnButtonFileChooserOpenClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function window_show_file_sel and handle the open response (then window_destroy the file chooser window).
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_file_chooser_delete_event.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function window_show_file_sel and handle the cancel response (then window_destroy the file chooser window).
        return true;
    }
}