using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Yes/no window of the game. It shows a question, a yes and a no button.
/// Corresponds to the C function create_window_yesno and the widget
/// "window_yesno" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file yesno.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(YesNoWindow))]
[Gtk.Template<Gtk.AssemblyResource>("yesno.ui")]
public partial class YesNoWindow
{
    [Gtk.Connect("button_yesno_yes")]
    private Gtk.Button _buttonYesnoYes;

    [Gtk.Connect("button_yesno_no")]
    private Gtk.Button _buttonYesnoNo;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.Yesno = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonYesnoYes.OnClicked += OnButtonYesnoYesClicked;
        _buttonYesnoNo.OnClicked += OnButtonYesnoNoClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_yesno_delete_event of the close-request signal, which itself
    /// calls on_button_yesno_no_clicked and shows the next event.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_yesno_delete_event (which calls on_button_yesno_no_clicked and shows the next event).
        return true;
    }

    /// <summary>
    /// The "Yes" button was clicked. Corresponds to the C handler
    /// on_button_yesno_yes_clicked of the "clicked" signal, which destroys the yes/no window and performs the yes action.
    /// </summary>
    private void OnButtonYesnoYesClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_yesno_yes_clicked (which destroys the yes/no window and performs the yes action).
    }

    /// <summary>
    /// The "No" button was clicked. Corresponds to the C handler
    /// on_button_yesno_no_clicked of the "clicked" signal, which destroys the yes/no window and shows the next event.
    /// </summary>
    private void OnButtonYesnoNoClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_yesno_no_clicked (which destroys the yes/no window and shows the next event).
    }
}
