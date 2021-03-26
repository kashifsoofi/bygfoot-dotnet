namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class DigitsWindow : Window
    {
        public DigitsWindow() : this(new Builder("DigitsWindow.glade")) { }

        private DigitsWindow(Builder builder) : base(builder.GetObject("window_digits").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_digits_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_spinbutton1_activate(object sender, EventArgs a)
        {
        }

        private void on_spinbutton2_activate(object sender, EventArgs a)
        {
        }

        private void on_button_digits_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_digits_alr_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_digits_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}