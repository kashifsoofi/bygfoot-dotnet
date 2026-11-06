namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Transfer dialog window of the game. It shows a question and buttons to
/// accept, reject or decide later on a transfer offer. Corresponds to the C
/// function create_window_transfer_dialog and the widget "window_transfer_dialog"
/// of the bygfoot_misc2.glade support file, which has been converted into the
/// GTK4 builder UI file transfer_dialog.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(TransferDialogWindow))]
[Gtk.Template<Gtk.AssemblyResource>("transfer_dialog.ui")]
public partial class TransferDialogWindow
{
    [Gtk.Connect("button_transfer_yes")]
    private Gtk.Button _buttonTransferYes;

    [Gtk.Connect("button_transfer_no")]
    private Gtk.Button _buttonTransferNo;

    [Gtk.Connect("button_transfer_later")]
    private Gtk.Button _buttonTransferLater;

    partial void Initialize()
    {
        DefaultWidth = 450;
        DefaultHeight = 350;
        Modal = true;

        Variables.Window.TransferDialog = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonTransferYes.OnClicked += OnButtonTransferYesClicked;
        _buttonTransferNo.OnClicked += OnButtonTransferNoClicked;
        _buttonTransferLater.OnClicked += OnButtonTransferLaterClicked;

        Show();
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
