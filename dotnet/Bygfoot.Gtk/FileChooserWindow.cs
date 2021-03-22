namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class FileChooserWindow : Window
    {
        public FileChooserWindow() : this(new Builder("FileChooserWindow.glade")) { }

        private FileChooserWindow(Builder builder) : base(builder.GetObject("window_file_chooser").Handle)
        {
            builder.Autoconnect(this);
        }
    }
}