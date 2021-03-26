namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class TransferWindow : Window
    {
        public TransferWindow() : this(new Builder("TransferWindow.glade")) { }

        private TransferWindow(Builder builder) : base(builder.GetObject("window_transfer_dialog").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_transfer_dialog_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_transfer_yes_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_transfer_no_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_transfer_later_clicked(object sender, EventArgs a)
        {
        }
    }
}