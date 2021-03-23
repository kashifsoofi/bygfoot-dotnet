namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class FontSelectionDialog : Window
    {
        public FontSelectionDialog() : this(new Builder("FontSelectionDialog.glade")) { }

        private FontSelectionDialog(Builder builder) : base(builder.GetObject("window_font_sel").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_button_font_sel_cancel_clicked(object sender, DeleteEventArgs a)
        {
        }

        private void on_button_font_sel_cancel_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_font_sel_apply_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_font_sel_ok_clicked(object sender, EventArgs a)
        {
        }
    }
}