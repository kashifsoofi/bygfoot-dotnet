namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// News window of the game. It shows the news in a tree view and a close button.
/// Corresponds to the C function create_window_news and the widget
/// "window_news" of the bygfoot_misc3.glade support file, which has been
/// converted into the GTK4 builder UI file news.ui.
/// </summary>
public class NewsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public NewsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("news.ui");

        var window = (Gtk.Window)_builder.GetObject("window_news") ?? throw new InvalidOperationException("Could not find the widget 'window_news' in the UI file 'news.ui'.");

        window.DefaultWidth = 700;
        window.DefaultHeight = 600;

        Variables.Window.News = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonNewsClose = (Gtk.Button)_builder.GetObject("button_news_close") ?? throw new InvalidOperationException("Could not find the widget 'button_news_close' in the UI file 'news.ui'.");
        buttonNewsClose.OnClicked += OnButtonNewsCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_news_delete_event of the close-request signal, which itself
    /// destroys the news window.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
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
