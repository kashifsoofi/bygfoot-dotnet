namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Stadium window of the game. It is shown when the user wants to improve the
/// stadium of the current team and lets him adjust capacity, safety, costs and
/// ticket prices.
/// Corresponds to the C function window_show_stadium and the widget
/// "window_stadium" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file stadium.ui.
/// </summary>
public class StadiumWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public StadiumWindow()
    {
        _builder = Gtk.Builder.NewFromFile("stadium.ui");

        var window = (Gtk.Window)_builder.GetObject("window_stadium") ?? throw new InvalidOperationException("Could not find the widget 'window_stadium' in the UI file 'stadium.ui'.");

        window.Modal = true;
        window.DefaultWidth = 550;
        window.DefaultHeight = 600;

        Variables.Window.Stadium = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonStadiumOk = (Gtk.Button)_builder.GetObject("button_stadium_ok") ?? throw new InvalidOperationException("Could not find the widget 'button_stadium_ok' in the UI file 'stadium.ui'.");
        buttonStadiumOk.OnClicked += OnButtonStadiumOkClicked;

        var buttonStadiumCancel = (Gtk.Button)_builder.GetObject("button_stadium_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_stadium_cancel' in the UI file 'stadium.ui'.");
        buttonStadiumCancel.OnClicked += OnButtonStadiumCancelClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_stadium_delete_event of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_stadium_delete_event (which closes the stadium window).
        return true;
    }

    /// <summary>
    /// The "Confirm!" button was clicked. Corresponds to the C handler
    /// on_button_stadium_ok_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonStadiumOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_stadium_ok_clicked (which improves the stadium and possibly shows the finances).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_stadium_cancel_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonStadiumCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_stadium_cancel_clicked (which destroys the stadium window).
    }
}
