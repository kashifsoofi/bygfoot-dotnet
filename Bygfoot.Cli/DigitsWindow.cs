using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Digits window of the game. It shows the numbers (main, two values and buttons).
/// Corresponds to the C function create_window_digits and the widget
/// "window_digits" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file digits.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(DigitsWindow))]
[Gtk.Template<Gtk.AssemblyResource>("digits.ui")]
public partial class DigitsWindow
{
    [Gtk.Connect("button_digits_ok")]
    private Gtk.Button _buttonDigitsOk;

    [Gtk.Connect("button_digits_alr")]
    private Gtk.Button _buttonDigitsAlr;

    [Gtk.Connect("button_digits_cancel")]
    private Gtk.Button _buttonDigitsCancel;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.Digits = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonDigitsOk.OnClicked += OnButtonDigitsOkClicked;
        _buttonDigitsAlr.OnClicked += OnButtonDigitsAlrClicked;
        _buttonDigitsCancel.OnClicked += OnButtonDigitsCancelClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_digits_delete_event of the close-request signal, which itself
    /// calls on_button_digits_cancel_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_digits_delete_event (which calls on_button_digits_cancel_clicked and destroys the digits window).
        return true;
    }

    /// <summary>
    /// The "OK" button was clicked. Corresponds to the C handler
    /// on_button_digits_ok_clicked of the "clicked" signal, which applies the entered numbers and schedules the repayment.
    /// </summary>
    private void OnButtonDigitsOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_digits_ok_clicked (which applies the entered numbers and schedules the repayment).
    }

    /// <summary>
    /// The "Confirm & schedule repayment" button was clicked. Corresponds to the
    /// C handler on_button_digits_alr_clicked of the "clicked" signal, which
    /// confirms the numbers and schedules the repayment.
    /// </summary>
    private void OnButtonDigitsAlrClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_digits_alr_clicked (which confirms the numbers and schedules the repayment).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_digits_cancel_clicked of the "clicked" signal, which destroys the digits window.
    /// </summary>
    private void OnButtonDigitsCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_digits_cancel_clicked (which destroys the digits window).
    }
}