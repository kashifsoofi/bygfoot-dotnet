namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class DebugWindow : Window
    {
        public DebugWindow() : this(new Builder("DebugWindow.glade")) { }

        private DebugWindow(Builder builder) : base(builder.GetObject("window_debug").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_debug_delete_event(object sender, DeleteEventArgs a)
        {
            Close();
        }

        private void on_entry_debug_activate(object sender, EventArgs a)
        {
        }

        private void on_button_debug_apply_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_debug_close_activate(object sender, EventArgs a)
        {
            Close();
        }
    }
}