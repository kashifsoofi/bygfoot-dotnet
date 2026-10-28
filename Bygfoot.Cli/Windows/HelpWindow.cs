namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Help window of the game. It shows the help notebook with the about, the
/// contributors and the help pages and a close button. Corresponds to the C
/// function create_window_help and the widget "window_help" of the bygfoot_misc2
/// glade support file, which has been converted into the GTK4 builder UI file
/// help.ui.
/// </summary>
public class HelpWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public HelpWindow()
    {
        _builder = Gtk.Builder.NewFromFile("help.ui");

        var window = (Gtk.Window)_builder.GetObject("window_help") ?? throw new InvalidOperationException("Could not find the widget 'window_help' in the UI file 'help.ui'.");

        window.Modal = true;
        window.DefaultWidth = 400;
        window.DefaultHeight = 400;

        Variables.Window.Help = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonHelpClose = (Gtk.Button)_builder.GetObject("button_help_close") ?? throw new InvalidOperationException("Could not find the widget 'button_help_close' in the UI file 'help.ui'.");
        buttonHelpClose.OnClicked += OnButtonHelpCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_help_delete_event of the close-request signal, which itself
    /// calls on_button_help_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
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
