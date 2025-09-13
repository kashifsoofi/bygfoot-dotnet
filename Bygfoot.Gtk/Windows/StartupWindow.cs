using Bygfoot.Store;
using Gtk;
using WindowHandle = Gtk.Internal.WindowHandle;

namespace Bygfoot.Gtk.Windows;

public class StartupWindow : Window
{
    [Connect("button_team_selection_back")] private Button _buttonBack;
    [Connect("team_selection_cancel")] private Button _buttonCancel;

    public App App { get; }

    private StartupWindow(Builder builder, App app)
        : base(new WindowHandle(builder.GetPointer(nameof(StartupWindow)), false))
    {
        builder.Connect(this);

        App = app;
        _buttonBack!.OnClicked += OnBackClicked;
        _buttonCancel!.OnClicked += OnCancelClicked;
    }

    public StartupWindow(App app)
        : this(new Builder("startup.ui"), app)
    { }

    private void OnBackClicked(Button sender, EventArgs args)
    {
        Destroy();
        this.App.ShowSplashWindow();
    }

    private void OnCancelClicked(Button sender, EventArgs args)
    {
        var application = Application;
        Destroy();
        application?.Quit();
    }
}