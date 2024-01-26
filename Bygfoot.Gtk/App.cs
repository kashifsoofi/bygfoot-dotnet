using Bygfoot.Gtk.Windows;
using Bygfoot.Store;
using Gtk;

namespace Bygfoot.Gtk;

public class App
{
    private const string AppId = "com.github.kashifsoofi.bygfoot-dotnet.Bygfoot";

    private readonly Application _application;
    private readonly IStore _store;

    public App(IStore store)
    {
        _store = store;
        _application = Application.New(AppId, Gio.ApplicationFlags.FlagsNone);
        _application.OnActivate += OnActivate;
    }

    public int Run()
    {
        return _application.RunWithSynchronizationContext(null);
    }

    private void OnActivate(Gio.Application sender, EventArgs args)
    {
        var window = new SplashWindow(_store.HelpStore, _store.HintsStore)
        {
            Application = _application
        };
        window.Show();
    }
}
