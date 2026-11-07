using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Contract window of the game. It shows the new contract offer, lets the user
/// choose the contract duration and make or cancel the offer.
/// Corresponds to the C function create_window_contract and the widget
/// "window_contract" of the bygfoot_misc2.glade support file, which
/// has been converted into the GTK4 builder UI file contract.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(ContractWindow))]
[Gtk.Template<Gtk.AssemblyResource>("contract.ui")]
public partial class ContractWindow
{
    [Gtk.Connect("button_contract_offer")]
    private Gtk.Button _buttonContractOffer;

    [Gtk.Connect("button_contract_cancel")]
    private Gtk.Button _buttonContractCancel;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.Contract = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonContractOffer.OnClicked += OnButtonContractOfferClicked;
        _buttonContractCancel.OnClicked += OnButtonContractCancelClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_contract_delete_event of the close-request signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_contract_delete_event (which destroys the contract window).
        return true;
    }

    /// <summary>
    /// The "Make offer" button was clicked. Corresponds to the C handler
    /// on_button_contract_offer_clicked of the "clicked" signal, which makes the offer.
    /// </summary>
    private void OnButtonContractOfferClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_contract_offer_clicked (which makes the offer and destroys the window).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_contract_cancel_clicked of the "clicked" signal, which destroys the contract window.
    /// </summary>
    private void OnButtonContractCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_contract_cancel_clicked (which destroys the contract window).
    }
}
