namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class TemplateWindow : Window
    {
        public TemplateWindow() : this(new Builder("TemplateWindow.glade")) { }

        private TemplateWindow(Builder builder) : base(builder.GetObject("window_template").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_template_delete_event(object sender, DeleteEventArgs a)
        {
        }
    }
}