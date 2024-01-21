using Gtk;

namespace Bygfoot.Gtk.Windows;

public class SplashWindow : Window
{
    [Connect("button_splash_quit")]
    private Button buttonQuit;

    private SplashWindow(Builder builder, string name)
        : base(builder.GetPointer(name), false)
    {
        builder.Connect(this);
    }

    public SplashWindow()
        : this(new Builder("splash.ui"), "SplashWindow")
    { }
}
