namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Contract window of the game. It shows the new contract offer, lets the user
/// choose the contract duration and make or cancel the offer.
/// Corresponds to the C function create_window_contract and the widget
/// "window_contract" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file contract.ui.
/// </summary>
public class ContractWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public ContractWindow()
    {
        _builder = Gtk.Builder.NewFromFile("contract.ui");

        var window = (Gtk.Window)_builder.GetObject("window_contract") ?? throw new InvalidOperationException("Could not find the widget 'window_contract' in the UI file 'contract.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.Contract = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonContractOffer = (Gtk.Button)_builder.GetObject("button_contract_offer") ?? throw new InvalidOperationException("Could not find the widget 'button_contract_offer' in the UI file 'contract.ui'.");
        buttonContractOffer.OnClicked += OnButtonContractOfferClicked;

        var buttonContractCancel = (Gtk.Button)_builder.GetObject("button_contract_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_contract_cancel' in the UI file 'contract.ui'.");
        buttonContractCancel.OnClicked += OnButtonContractCancelClicked;

        window.Show();
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
