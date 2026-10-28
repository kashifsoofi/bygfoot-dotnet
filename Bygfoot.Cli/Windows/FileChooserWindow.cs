namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// File selection window of the game. It is shown when the user wants to load
/// or save a game or a memorable match and lets him choose the file.
/// Corresponds to the C function window_show_file_sel and the widget
/// "window_file_chooser" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file file_chooser.ui.
/// </summary>
public class FileChooserWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public FileChooserWindow()
    {
        _builder = Gtk.Builder.NewFromFile("file_chooser.ui");

        var window = (Gtk.Window)_builder.GetObject("window_file_chooser") ?? throw new InvalidOperationException("Could not find the widget 'window_file_chooser' in the UI file 'file_chooser.ui'.");

        window.Modal = true;
        window.DefaultWidth = 550;
        window.DefaultHeight = 600;

        Variables.Window.FileChooser = this;

        var buttonFileChooserCancel = (Gtk.Button)_builder.GetObject("button1") ?? throw new InvalidOperationException("Could not find the widget 'button1' in the UI file 'file_chooser.ui'.");
        buttonFileChooserCancel.OnClicked += OnButtonFileChooserCancelClicked;

        var buttonFileChooserOpen = (Gtk.Button)_builder.GetObject("button2") ?? throw new InvalidOperationException("Could not find the widget 'button2' in the UI file 'file_chooser.ui'.");
        buttonFileChooserOpen.OnClicked += OnButtonFileChooserOpenClicked;

        window.Show();
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
}
