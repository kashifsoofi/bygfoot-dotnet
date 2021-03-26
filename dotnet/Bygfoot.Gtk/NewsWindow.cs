namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class NewsWindow : Window
    {
        public NewsWindow() : this(new Builder("NewsWindow.glade")) { }

        private NewsWindow(Builder builder) : base(builder.GetObject("window_news").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_news_destroy_event(object sender, DestroyEventArgs a)
        {
        }

        private void on_window_news_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_button_news_close_clicked(object sender, EventArgs a)
        {
            ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
        }
    }
}