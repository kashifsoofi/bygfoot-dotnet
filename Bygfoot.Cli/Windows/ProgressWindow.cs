namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Progress window of the game. It shows the match progress image together with
/// the progress bar during the game setup / loading phase.
/// Corresponds to the C function create_window_progress and the widget
/// "window_progress" of the bygfoot_misc2.glade support file, which has been
/// converted into the GTK4 builder UI file progress.ui.
/// </summary>
public class ProgressWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public ProgressWindow()
    {
        _builder = Gtk.Builder.NewFromFile("progress.ui");

        var window = (Gtk.Window)_builder.GetObject("window_progress") ?? throw new InvalidOperationException("Could not find the widget 'window_progress' in the UI file 'progress.ui'.");

        window.Modal = true;
        window.DefaultWidth = 300;
        window.DefaultHeight = 600;

        Variables.Window.Progress = this;

        var imageMatch = (Gtk.Image)_builder.GetObject("image_match") ?? throw new InvalidOperationException("Could not find the widget 'image_match' in the UI file 'progress.ui'.");
        var progressbar = (Gtk.ProgressBar)_builder.GetObject("progressbar") ?? throw new InvalidOperationException("Could not find the widget 'progressbar' in the UI file 'progress.ui'.");

        window.Show();
    }
}
