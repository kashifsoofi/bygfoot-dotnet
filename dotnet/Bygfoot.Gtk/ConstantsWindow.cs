namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class ConstantsWindow : Window
    {
        public ConstantsWindow() : this(new Builder("ConstantsWindow.glade")) { }

        private ConstantsWindow(Builder builder) : base(builder.GetObject("window_constants").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_constants_destroy_event(object sender, DestroyEventArgs a)
        {
        }

        private void on_window_constants_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_constants_save_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_constants_reload_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_constants_close_clicked(object sender, EventArgs a)
        {
        }
    }
}