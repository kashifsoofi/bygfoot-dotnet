namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class HelpWindow : Window
    {
        public HelpWindow() : this(new Builder("HelpWindow.glade")) { }

        private HelpWindow(Builder builder) : base(builder.GetObject("window_help").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_help_delete_event(object sender, DeleteEventArgs a)
        {
            Close();
        }

        private void on_button_help_close_clicked(object sender, EventArgs a)
        {
            Close();
        }
    }
}