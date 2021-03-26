namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class MemorableMatchesWindow : Window
    {
        public MemorableMatchesWindow() : this(new Builder("MemorableMatchesWindow.glade")) { }

        private MemorableMatchesWindow(Builder builder) : base(builder.GetObject("window_mmatches").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_mmatches_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_mm_file_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_mm_reload_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_mm_import_clicked(object sender, EventArgs a)
        {
        }

        private void on_treeview_mmatches_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_button_mm_save_close_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_mm_reload_close_clicked(object sender, EventArgs a)
        {
        }
    }
}