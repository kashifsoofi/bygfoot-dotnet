using System;
using Gtk;

namespace Bygfoot
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.Bygfoot.Bygfoot", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new StartupWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}
