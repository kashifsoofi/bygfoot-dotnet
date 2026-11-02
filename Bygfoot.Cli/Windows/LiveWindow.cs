namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Live game window of the game. It is shown while a live game is running and
/// lets the user pause, resume, close or inspect the live game.
/// Corresponds to the C function create_window_live and the widget
/// "window_live" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file live.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(LiveWindow))]
[Gtk.Template<Gtk.AssemblyResource>("live.ui")]
public partial class LiveWindow
{
    [Gtk.Connect("button_pause")]
    private Gtk.Button _buttonLivePause;

    [Gtk.Connect("button_resume")]
    private Gtk.Button _buttonLiveResume;

    [Gtk.Connect("button_live_close")]
    private Gtk.Button _buttonLiveClose;

    partial void Initialize()
    {
        DefaultWidth = 620;
        DefaultHeight = 550;
        Modal = true;

        Variables.Window.Live = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonLivePause.OnClicked += OnButtonLivePauseClicked;
        _buttonLiveResume.OnClicked += OnButtonLiveResumeClicked;
        _buttonLiveClose.OnClicked += OnButtonLiveCloseClicked;

        Show();
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
