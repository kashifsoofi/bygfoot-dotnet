namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class ProgressWindow : Window
    {
        public ProgressWindow() : this(new Builder("ProgressWindow.glade")) { }

        private ProgressWindow(Builder builder) : base(builder.GetObject("window_progress").Handle)
        {
            builder.Autoconnect(this);
        }
    }
}