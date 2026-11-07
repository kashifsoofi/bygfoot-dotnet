using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// League table window of the game. It shows the league table with the headline
/// label, the separators and the OK button. Corresponds to the C function
/// create_window_league_table and the widget "window_league_table" of the
/// bygfoot_table.glade support file, which has been converted into the GTK4
/// builder UI file league_table.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(LeagueTableWindow))]
[Gtk.Template<Gtk.AssemblyResource>("league_table.ui")]
public partial class LeagueTableWindow
{
    [Gtk.Connect("b_ok")]
    private Gtk.Button _buttonOk;

    partial void Initialize()
    {
        DefaultWidth = 500;
        DefaultHeight = 290;
        Modal = true;
        Resizable = false;

        Variables.Window.LeagueTable = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonOk.OnClicked += OnButtonOkClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_league_table_delete_event of the close-request signal, which
    /// ignores the close event and does nothing.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_league_table_delete_event (which ignores the close event and does nothing).
        return true;
    }

    /// <summary>
    /// The "OK" button was clicked. Corresponds to the C handler
    /// on_lt_b_ok_clicked of the "clicked" signal, which does nothing yet.
    /// </summary>
    private void OnButtonOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_lt_b_ok_clicked (which does nothing yet).
    }
}
