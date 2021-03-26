namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class JobOfferWindow : Window
    {
        public JobOfferWindow() : this(new Builder("JobOfferWindow.glade")) { }

        private JobOfferWindow(Builder builder) : base(builder.GetObject("window_job_offer").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_job_offer_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_button_offer_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_offer_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}