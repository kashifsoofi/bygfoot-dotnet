namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Yes/no window of the game. It shows a question, a yes and a no button.
/// Corresponds to the C function create_window_yesno and the widget
/// "window_yesno" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file yesno.ui.
/// </summary>
public class YesNoWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public YesNoWindow()
    {
        _builder = Gtk.Builder.NewFromFile("yesno.ui");

        var window = (Gtk.Window)_builder.GetObject("window_yesno") ?? throw new InvalidOperationException("Could not find the widget 'window_yesno' in the UI file 'yesno.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.Yesno = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonYesnoYes = (Gtk.Button)_builder.GetObject("button_yesno_yes") ?? throw new InvalidOperationException("Could not find the widget 'button_yesno_yes' in the UI file 'yesno.ui'.");
        buttonYesnoYes.OnClicked += OnButtonYesnoYesClicked;

        var buttonYesnoNo = (Gtk.Button)_builder.GetObject("button_yesno_no") ?? throw new InvalidOperationException("Could not find the widget 'button_yesno_no' in the UI file 'yesno.ui'.");
        buttonYesnoNo.OnClicked += OnButtonYesnoNoClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_yesno_delete_event of the close-request signal, which itself
    /// calls on_button_yesno_no_clicked and shows the next event.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
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
