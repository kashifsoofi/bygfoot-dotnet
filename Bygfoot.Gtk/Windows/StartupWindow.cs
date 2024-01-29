using Bygfoot.Store;
using Gtk;

namespace Bygfoot.Gtk.Windows;

public class StartupWindow : Window
{
    [Connect("button_team_selection_back")] private Button _buttonBack;
    [Connect("team_selection_cancel")] private Button _buttonCancel;

    public App App { get; set; }

    private StartupWindow(Builder builder, string name)
        : base(builder.GetPointer(name), false)
    {
        builder.Connect(this);

        _buttonBack.OnClicked += OnBackClicked;
        _buttonCancel.OnClicked += OnCancelClicked;
    }

    public StartupWindow()
        : this(new Builder("startup.ui"), "StartupWindow")
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