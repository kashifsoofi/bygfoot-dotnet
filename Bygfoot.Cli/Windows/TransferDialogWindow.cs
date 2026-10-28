namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Transfer dialog window of the game. It shows a question and buttons to
/// accept, reject or decide later on a transfer offer. Corresponds to the C
/// function create_window_transfer_dialog and the widget "window_transfer_dialog"
/// of the bygfoot_misc2.glade support file, which has been converted into the
/// GTK4 builder UI file transfer_dialog.ui.
/// </summary>
public class TransferDialogWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public TransferDialogWindow()
    {
        _builder = Gtk.Builder.NewFromFile("transfer_dialog.ui");

        var window = (Gtk.Window)_builder.GetObject("window_transfer_dialog") ?? throw new InvalidOperationException("Could not find the widget 'window_transfer_dialog' in the UI file 'transfer_dialog.ui'.");

        window.Modal = true;
        window.DefaultWidth = 450;
        window.DefaultHeight = 350;

        Variables.Window.TransferDialog = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonTransferYes = (Gtk.Button)_builder.GetObject("button_transfer_yes") ?? throw new InvalidOperationException("Could not find the widget 'button_transfer_yes' in the UI file 'transfer_dialog.ui'.");
        buttonTransferYes.OnClicked += OnButtonTransferYesClicked;

        var buttonTransferNo = (Gtk.Button)_builder.GetObject("button_transfer_no") ?? throw new InvalidOperationException("Could not find the widget 'button_transfer_no' in the UI file 'transfer_dialog.ui'.");
        buttonTransferNo.OnClicked += OnButtonTransferNoClicked;

        var buttonTransferLater = (Gtk.Button)_builder.GetObject("button_transfer_later") ?? throw new InvalidOperationException("Could not find the widget 'button_transfer_later' in the UI file 'transfer_dialog.ui'.");
        buttonTransferLater.OnClicked += OnButtonTransferLaterClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_transfer_dialog_delete_event of the close-request signal, which
    /// itself calls on_button_transfer_later_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_transfer_dialog_delete_event (which calls on_button_transfer_later_clicked and destroys the transfer dialog window).
        return true;
    }

    /// <summary>
    /// The "Yes" button was clicked. Corresponds to the C handler
    /// on_button_transfer_yes_clicked of the "clicked" signal, which transfers the player offer and destroys the transfer dialog window.
    /// </summary>
    private void OnButtonTransferYesClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_transfer_yes_clicked (which transfers the player offer and destroys the transfer dialog window).
    }

    /// <summary>
    /// The "No" button was clicked. Corresponds to the C handler
    /// on_button_transfer_no_clicked of the "clicked" signal, which rejects the offer and destroys the transfer dialog window.
    /// </summary>
    private void OnButtonTransferNoClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_transfer_no_clicked (which rejects the offer and destroys the transfer dialog window).
    }

    /// <summary>
    /// The "Decide later" button was clicked. Corresponds to the C handler
    /// on_button_transfer_later_clicked of the "clicked" signal, which destroys the transfer dialog window.
    /// </summary>
    private void OnButtonTransferLaterClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_transfer_later_clicked (which destroys the transfer dialog window).
    }
}
