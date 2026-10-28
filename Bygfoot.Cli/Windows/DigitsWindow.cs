namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Digits window of the game. It shows the numbers (main, two values and buttons).
/// Corresponds to the C function create_window_digits and the widget
/// "window_digits" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file digits.ui.
/// </summary>
public class DigitsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public DigitsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("digits.ui");

        var window = (Gtk.Window)_builder.GetObject("window_digits") ?? throw new InvalidOperationException("Could not find the widget 'window_digits' in the UI file 'digits.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.Digits = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonDigitsOk = (Gtk.Button)_builder.GetObject("button_digits_ok") ?? throw new InvalidOperationException("Could not find the widget 'button_digits_ok' in the UI file 'digits.ui'.");
        buttonDigitsOk.OnClicked += OnButtonDigitsOkClicked;

        var buttonDigitsAlr = (Gtk.Button)_builder.GetObject("button_digits_alr") ?? throw new InvalidOperationException("Could not find the widget 'button_digits_alr' in the UI file 'digits.ui'.");
        buttonDigitsAlr.OnClicked += OnButtonDigitsAlrClicked;

        var buttonDigitsCancel = (Gtk.Button)_builder.GetObject("button_digits_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_digits_cancel' in the UI file 'digits.ui'.");
        buttonDigitsCancel.OnClicked += OnButtonDigitsCancelClicked;

        window.Show();
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
