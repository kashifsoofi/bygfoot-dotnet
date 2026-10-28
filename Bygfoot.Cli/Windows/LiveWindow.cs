namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Live game window of the game. It is shown while a live game is running and
/// lets the user pause, resume, close or inspect the live game.
/// Corresponds to the C function create_window_live and the widget
/// "window_live" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file live.ui.
/// </summary>
public class LiveWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public LiveWindow()
    {
        _builder = Gtk.Builder.NewFromFile("live.ui");

        var window = (Gtk.Window)_builder.GetObject("window_live") ?? throw new InvalidOperationException("Could not find the widget 'window_live' in the UI file 'live.ui'.");

        window.Modal = true;
        window.DefaultWidth = 620;
        window.DefaultHeight = 550;

        Variables.Window.Live = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonLivePause = (Gtk.Button)_builder.GetObject("button_pause") ?? throw new InvalidOperationException("Could not find the widget 'button_pause' in the UI file 'live.ui'.");
        buttonLivePause.OnClicked += OnButtonLivePauseClicked;

        var buttonLiveResume = (Gtk.Button)_builder.GetObject("button_resume") ?? throw new InvalidOperationException("Could not find the widget 'button_resume' in the UI file 'live.ui'.");
        buttonLiveResume.OnClicked += OnButtonLiveResumeClicked;

        var buttonLiveClose = (Gtk.Button)_builder.GetObject("button_live_close") ?? throw new InvalidOperationException("Could not find the widget 'button_live_close' in the UI file 'live.ui'.");
        buttonLiveClose.OnClicked += OnButtonLiveCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_live_window_delete_event of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_live_window_delete_event (which closes the live game window).
        return true;
    }

    /// <summary>
    /// The "Pause" button was clicked. Corresponds to the C handler
    /// on_button_pause_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonLivePauseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_pause_clicked (which pauses the live game).
    }

    /// <summary>
    /// The "Resume" button was clicked. Corresponds to the C handler
    /// on_button_resume_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonLiveResumeClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_resume_clicked (which resumes the live game).
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_live_close_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonLiveCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_live_close_clicked (which closes the live game window).
    }
}
