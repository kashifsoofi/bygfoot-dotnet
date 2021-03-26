namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class AlrWindow : Window
    {
        public AlrWindow() : this(new Builder("AlrWindow.glade")) { }

        private AlrWindow(Builder builder) : base(builder.GetObject("window_alr").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_alr_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_button_calculate_installment_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_calculate_start_week_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_alr_confirm_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_alr_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}