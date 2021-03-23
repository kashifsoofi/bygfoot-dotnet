namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class WarningWindow : Window
    {
        public WarningWindow() : this(new Builder("WarningWindow.glade")) { }

        private WarningWindow(Builder builder) : base(builder.GetObject("window_warning").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_warning_destroy_event(object sender, DestroyEventArgs a)
        {
        }

        private void on_window_warning_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_warning_clicked(object sender, EventArgs a)
        {
        }
    }
}