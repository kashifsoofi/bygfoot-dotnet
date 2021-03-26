namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class ContractWindow : Window
    {
        public ContractWindow() : this(new Builder("ContractWindow.glade")) { }

        private ContractWindow(Builder builder) : base(builder.GetObject("window_contract").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_contract_delete_event(object sender, DeleteEventArgs a)
        {
            Close();
        }

        private void on_button_contract_offer_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_contract_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}