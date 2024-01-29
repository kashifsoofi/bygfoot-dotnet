using Bygfoot.Store;
using Gtk;

namespace Bygfoot.Gtk.Windows;

public class StartupWindow : Window
{
    // [Connect("button_team_selection_back")] private Button _buttonBackToSplash;

    public App App { get; set; }

    private StartupWindow(Builder builder, string name)
        : base(builder.GetPointer(name), false)
    {
        builder.Connect(this);

        // _buttonBackToSplash.OnClicked += OnBackToSplashClicked;
    }

    public StartupWindow()
        : this(new Builder("startup.ui"), "StartupWindow")
    { }

    private void OnBackToSplashClicked(Button sender, EventArgs args)
    {
        Destroy();
        this.App.ShowSplashWindow();
    }
}