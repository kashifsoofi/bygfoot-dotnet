namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Job offer window of the game. It shows the job offer details together with
/// the player list of the offered player and lets the user accept or cancel the offer.
/// Corresponds to the C function game_gui_show_job_offer and the widget
/// "window_job_offer" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file job_offer.ui.
/// </summary>
public class JobOfferWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public JobOfferWindow()
    {
        _builder = Gtk.Builder.NewFromFile("job_offer.ui");

        var window = (Gtk.Window)_builder.GetObject("window_job_offer") ?? throw new InvalidOperationException("Could not find the widget 'window_job_offer' in the UI file 'job_offer.ui'.");

        window.Modal = true;
        window.DefaultWidth = 500;
        window.DefaultHeight = 600;

        Variables.Window.JobOffer = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonOk = (Gtk.Button)_builder.GetObject("button_ok") ?? throw new InvalidOperationException("Could not find the widget 'button_ok' in the UI file 'job_offer.ui'.");
        buttonOk.OnClicked += OnButtonOfferOkClicked;

        var buttonCancel = (Gtk.Button)_builder.GetObject("button_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_cancel' in the UI file 'job_offer.ui'.");
        buttonCancel.OnClicked += OnButtonOfferCancelClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_job_offer_delete_event of the close-request signal, which
    /// itself calls on_button_offer_cancel_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_job_offer_delete_event (which calls on_button_offer_cancel_clicked and destroys the job offer window).
        return true;
    }

    /// <summary>
    /// The "OK" button was clicked. Corresponds to the C handler
    /// on_button_offer_ok_clicked of the "clicked" signal, which accepts the job offer.
    /// </summary>
    private void OnButtonOfferOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_offer_ok_clicked (which accepts the job offer and destroys the window).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_offer_cancel_clicked of the "clicked" signal, which rejects the job offer and destroys the window.
    /// </summary>
    private void OnButtonOfferCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_offer_cancel_clicked (which rejects the job offer and destroys the window).
    }
}
