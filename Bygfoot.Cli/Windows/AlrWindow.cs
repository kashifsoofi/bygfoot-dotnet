namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Automatic loan repayment window of the game. It shows the current debt and
/// the current and the new repayment settings and lets the user calculate and
/// confirm a new repayment schedule.
/// Corresponds to the C function create_window_alr and the widget
/// "window_alr" of the bygfoot_misc3.glade support file, which has been
/// converted into the GTK4 builder UI file alr.ui.
/// </summary>
public class AlrWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public AlrWindow()
    {
        _builder = Gtk.Builder.NewFromFile("alr.ui");

        var window = (Gtk.Window)_builder.GetObject("window_alr") ?? throw new InvalidOperationException("Could not find the widget 'window_alr' in the UI file 'alr.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 400;

        Variables.Window.Alr = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonAlrConfirm = (Gtk.Button)_builder.GetObject("button_alr_confirm") ?? throw new InvalidOperationException("Could not find the widget 'button_alr_confirm' in the UI file 'alr.ui'.");
        buttonAlrConfirm.OnClicked += OnButtonAlrConfirmClicked;

        var buttonAlrCancel = (Gtk.Button)_builder.GetObject("button_alr_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_alr_cancel' in the UI file 'alr.ui'.");
        buttonAlrCancel.OnClicked += OnButtonAlrCancelClicked;

        var buttonCalculateInstallment = (Gtk.Button)_builder.GetObject("button_calculate_installment") ?? throw new InvalidOperationException("Could not find the widget 'button_calculate_installment' in the UI file 'alr.ui'.");
        buttonCalculateInstallment.OnClicked += OnButtonCalculateInstallmentClicked;

        var buttonCalculateStartWeek = (Gtk.Button)_builder.GetObject("button_calculate_start_week") ?? throw new InvalidOperationException("Could not find the widget 'button_calculate_start_week' in the UI file 'alr.ui'.");
        buttonCalculateStartWeek.OnClicked += OnButtonCalculateStartWeekClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_alr_delete_event of the close-request signal, which itself
    /// calls on_button_alr_cancel_clicked and destroys the alr window.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_alr_delete_event (which calls on_button_alr_cancel_clicked and destroys the alr window).
        return true;
    }

    /// <summary>
    /// The "Apply" button was clicked. Corresponds to the C handler
    /// on_button_alr_confirm_clicked of the "clicked" signal, which stores the
    /// repayment schedule and schedules the repayment.
    /// </summary>
    private void OnButtonAlrConfirmClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_alr_confirm_clicked (which stores the repayment schedule and schedules the repayment).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_alr_cancel_clicked of the "clicked" signal, which destroys the alr window.
    /// </summary>
    private void OnButtonAlrCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_alr_cancel_clicked (which destroys the alr window).
    }

    /// <summary>
    /// The "Calculate weekly installment" button was clicked. Corresponds to the C
    /// handler on_button_calculate_installment_clicked of the "clicked" signal, which
    /// calculates the weekly installment depending on the start week and schedules the
    /// repayment.
    /// </summary>
    private void OnButtonCalculateInstallmentClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_calculate_installment_clicked (which calculates the weekly installment depending on the start week and schedules the repayment).
    }

    /// <summary>
    /// The "Calculate start week" button was clicked. Corresponds to the C handler
    /// on_button_calculate_start_week_clicked of the "clicked" signal, which calculates the
    /// start week depending on the weekly installment and schedules the repayment.
    /// </summary>
    private void OnButtonCalculateStartWeekClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_calculate_start_week_clicked (which calculates the start week depending on the weekly installment and schedules the repayment).
    }
}
