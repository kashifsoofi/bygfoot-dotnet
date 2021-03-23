namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class LiveWindow : Window
    {
        public LiveWindow() : this(new Builder("LiveWindow.glade")) { }

        private LiveWindow(Builder builder) : base(builder.GetObject("window_live").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_live_window_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_pause_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_resume_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_live_close_clicked(object sender, EventArgs a)
        {
        }

        private void on_eventbox_lg_style_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_eventbox_lg_boost_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_spinbutton_speed_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_spinbutton_speed_value_changed(object sender, ChangeValueArgs a)
        {
        }

        private void on_spinbutton_verbosity_value_changed(object sender, ChangeValueArgs a)
        {
        }
    }
}