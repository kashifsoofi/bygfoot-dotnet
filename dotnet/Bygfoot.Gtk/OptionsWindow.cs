namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class OptionsWindow : Window
    {
        public OptionsWindow() : this(new Builder("OptionsWindow.glade")) { }

        private OptionsWindow(Builder builder) : base(builder.GetObject("window_options").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_options_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_font_name_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_reload_constants_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_edit_constants_clicked(object sender, EventArgs a)
        {
        }

        private void on_spinbutton_recreation_value_changed(object sender, EventArgs a)
        {
        }

        private void on_checkbutton_save_global_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_checkbutton_save_user_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_button_options_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_options_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}