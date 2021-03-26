namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class YesNoWindow : Window
    {
        public YesNoWindow() : this(new Builder("YesNoWindow.glade")) { }

        private YesNoWindow(Builder builder) : base(builder.GetObject("window_yesno").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_yesno_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_button_yesno_yes_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_yesno_no_clicked(object sender, EventArgs a)
        {
        }
    }
}