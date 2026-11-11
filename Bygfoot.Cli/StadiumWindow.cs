using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Stadium window of the game. It is shown when the user wants to improve the
/// stadium of the current team and lets him adjust capacity, safety, costs and
/// ticket prices.
/// Corresponds to the C function window_show_stadium and the widget
/// "window_stadium" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file stadium.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(StadiumWindow))]
[Gtk.Template<Gtk.AssemblyResource>("stadium.ui")]
public partial class StadiumWindow
{
    [Gtk.Connect("button_stadium_ok")]
    private Gtk.Button _buttonStadiumOk;

    [Gtk.Connect("button_stadium_cancel")]
    private Gtk.Button _buttonStadiumCancel;

    partial void Initialize()
    {
        DefaultWidth = 550;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.Stadium = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonStadiumOk.OnClicked += OnButtonStadiumOkClicked;
        _buttonStadiumCancel.OnClicked += OnButtonStadiumCancelClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_stadium_delete_event of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
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
