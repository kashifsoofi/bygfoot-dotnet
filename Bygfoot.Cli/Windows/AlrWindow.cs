namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Automatic loan repayment window of the game. It shows the current debt and
/// the current and the new repayment settings and lets the user calculate and
/// confirm a new repayment schedule.
/// Corresponds to the C function create_window_alr and the widget
/// "window_alr" of the bygfoot_misc3.glade support file, which
/// has been converted into the GTK4 builder UI file alr.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(AlrWindow))]
[Gtk.Template<Gtk.AssemblyResource>("alr.ui")]
public partial class AlrWindow
{
    [Gtk.Connect("button_alr_confirm")]
    private Gtk.Button _buttonAlrConfirm;

    [Gtk.Connect("button_alr_cancel")]
    private Gtk.Button _buttonAlrCancel;

    [Gtk.Connect("button_calculate_installment")]
    private Gtk.Button _buttonCalculateInstallment;

    [Gtk.Connect("button_calculate_start_week")]
    private Gtk.Button _buttonCalculateStartWeek;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 400;
        Modal = true;

        Variables.Window.Alr = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonAlrConfirm.OnClicked += OnButtonAlrConfirmClicked;
        _buttonAlrCancel.OnClicked += OnButtonAlrCancelClicked;
        _buttonCalculateInstallment.OnClicked += OnButtonCalculateInstallmentClicked;
        _buttonCalculateStartWeek.OnClicked += OnButtonCalculateStartWeekClicked;

        Show();
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
    /// The "Calculate start week" button was clicked. Corresponds to the C
    /// handler on_button_calculate_start_week_clicked of the "clicked" signal, which
    /// calculates the start week depending on the weekly installment and schedules the
    /// repayment.
    /// </summary>
    private void OnButtonCalculateStartWeekClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_calculate_start_week_clicked (which calculates the weekly installment depending on the start week and schedules the repayment).
    }
}