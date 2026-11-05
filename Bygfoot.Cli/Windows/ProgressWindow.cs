namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Progress window of the game. It shows the match progress image together with
/// the progress bar during the game setup / loading phase.
/// Corresponds to the C function create_window_progress and the widget
/// "window_progress" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file progress.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(ProgressWindow))]
[Gtk.Template<Gtk.AssemblyResource>("progress.ui")]
public partial class ProgressWindow
{
    [Gtk.Connect("image_match")]
    private Gtk.Image _imageMatch;

    [Gtk.Connect("progressbar")]
    private Gtk.ProgressBar _progressbar;

    partial void Initialize()
    {
        Modal = true;
        DefaultWidth = 300;
        DefaultHeight = 600;

        Variables.Window.Progress = this;

        Show();
    }
}
