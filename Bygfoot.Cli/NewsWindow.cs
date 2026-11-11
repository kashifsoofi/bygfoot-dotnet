using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// News window of the game. It shows the news in a tree view and a close button.
/// Corresponds to the C function create_window_news and the widget
/// "window_news" of the bygfoot_misc3.glade support file, which has been
/// converted into the GTK4 builder UI file news.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(NewsWindow))]
[Gtk.Template<Gtk.AssemblyResource>("news.ui")]
public partial class NewsWindow
{
    [Gtk.Connect("button_news_close")]
    private Gtk.Button _buttonNewsClose;

    partial void Initialize()
    {
        DefaultWidth = 700;
        DefaultHeight = 600;

        Variables.Window.News = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonNewsClose.OnClicked += OnButtonNewsCloseClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_news_delete_event of the close-request signal, which itself
    /// destroys the news window.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_news_delete_event (which destroys the news window).
        return true;
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_news_close_clicked of the "clicked" signal, which destroys the news window.
    /// </summary>
    private void OnButtonNewsCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_news_close_clicked (which destroys the news window).
    }
}
