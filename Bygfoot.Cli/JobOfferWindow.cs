using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Job offer window of the game. It shows the job offer details together with
/// the player list of the offered player and lets the user accept or cancel the offer.
/// Corresponds to the C function game_gui_show_job_offer and the widget
/// "window_job_offer" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file job_offer.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(JobOfferWindow))]
[Gtk.Template<Gtk.AssemblyResource>("job_offer.ui")]
public partial class JobOfferWindow
{
    [Gtk.Connect("button_ok")]
    private Gtk.Button _buttonOk;

    [Gtk.Connect("button_cancel")]
    private Gtk.Button _buttonCancel;

    partial void Initialize()
    {
        DefaultWidth = 500;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.JobOffer = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonOk.OnClicked += OnButtonOfferOkClicked;
        _buttonCancel.OnClicked += OnButtonOfferCancelClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_job_offer_delete_event of the close-request signal, which
    /// itself calls on_button_offer_cancel_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
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
