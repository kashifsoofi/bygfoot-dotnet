namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class SplashWindow : Window
    {
        public SplashWindow() : this(new Builder("SplashWindow.glade")) { }

        private SplashWindow(Builder builder) : base(builder.GetObject("window_splash").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_splash_delete_event(object sender, DeleteEventArgs a)
        {
            Dispose();
        }

        private void on_button_splash_hint_back_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_splash_hint_next_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_splash_new_game_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_splash_load_game_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_splash_resume_game_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_splash_quit_clicked(object sender, EventArgs a)
        {
            Application.Quit();
        }
    }
}