namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class SponsorsWindow : Window
    {
        [UI] private TreeView treeview_sponsors = null;

        public SponsorsWindow() : this(new Builder("SponsorsWindow.glade")) { }

        private SponsorsWindow(Builder builder) : base(builder.GetObject("window_sponsors").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_sponsors_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_treeview_sponsors_row_activated(object sender, RowActivatedArgs a)
        {
        }

        private void on_button_sponsors_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_sponsors_wait_clicked(object sender, EventArgs a)
        {
        }
    }
}