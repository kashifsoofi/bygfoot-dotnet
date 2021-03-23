namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class StadiumWindow : Window
    {
        public StadiumWindow() : this(new Builder("StadiumWindow.glade")) { }

        private StadiumWindow(Builder builder) : base(builder.GetObject("window_stadium").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_stadium_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_spinbutton_capacity_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_spinbutton_capacity_value_changed(object sender, EventArgs a)
        {
        }

        private void on_spinbutton_safety_value_changed(object sender, ChangeValueArgs a)
        {
        }

        private void on_button_stadium_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_stadium_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}