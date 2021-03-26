namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class BetsWindow : Window
    {
        public BetsWindow() : this(new Builder("BetsWindow.glade")) { }

        private BetsWindow(Builder builder) : base(builder.GetObject("window_bets").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_bets_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_checkbutton_bet_all_leagues_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_checkbutton_bet_cups_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_checkbutton_bet_user_recent_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_treeview_bets_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_button_bet_close_clicked(object sender, EventArgs a)
        {
            ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
        }
    }
}