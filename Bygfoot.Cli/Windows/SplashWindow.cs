namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Splash screen window of the game. It shows the splash image on the first
/// notebook page and the list of contributors on the second page, the hint text
/// with the back/next buttons, the new game / load game / resume game / quit
/// buttons and the progress bar. Corresponds to the C function create_window_splash
/// and the widget "window_splash" of the bygfoot_misc3.glade support file, which
/// has been converted into the GTK4 builder UI file splash.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(SplashWindow))]
[Gtk.Template<Gtk.AssemblyResource>("CompositeBoxWidget.ui")]
public partial class SplashWindow
{
    [Gtk.Connect("button_splash_hint_back")]
    private Gtk.Button _buttonSplashHintBack;

    [Gtk.Connect("button_splash_hint_next")]
    private Gtk.Button _buttonSplashHintNext;
    
    [Gtk.Connect("button_splash_new_game")]
    private Gtk.Button _buttonSplashNewGame;
    
    [Gtk.Connect("button_splash_load_game")]
    private Gtk.Button _buttonSplashLoadGame;
    
    [Gtk.Connect("button_splash_resume_game")]
    private Gtk.Button _buttonSplashResumeGame;
    
    [Gtk.Connect("button_splash_quit")]
    private Gtk.Button _buttonSplashQuit;
    
    partial void Initialize()
    {
        Variables.Window.Splash = this;

        DefaultWidth = 500;
        DefaultHeight = 400;
        
        OnCloseRequest += OnWindowCloseRequest;
        
        _buttonSplashHintBack.OnClicked += OnButtonSplashHintBackClicked;
        _buttonSplashHintNext.OnClicked += OnButtonSplashHintNextClicked;
        _buttonSplashNewGame.OnClicked += OnButtonSplashNewGameClicked;
        _buttonSplashLoadGame.OnClicked += OnButtonSplashLoadGameClicked;
        _buttonSplashResumeGame.OnClicked += OnButtonSplashResumeGameClicked;
        _buttonSplashQuit.OnClicked += OnButtonSplashQuitClicked;
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_splash_delete_event of the close-request signal, which ignores
    /// the close event and does nothing.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_splash_delete_event (which ignores the close event and does nothing).
        return true;
    }

    /// <summary>
    /// The "Previous" hint button was clicked. Corresponds to the C handler
    /// on_button_splash_hint_back_clicked of the "clicked" signal, which updates
    /// the hint counter and shows the hint.
    /// </summary>
    private void OnButtonSplashHintBackClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_hint_back_clicked (which updates the hint counter and shows the hint).
        throw new NotImplementedException();
    }

    /// <summary>
    /// The "Next" hint button was clicked. Corresponds to the C handler
    /// on_button_splash_hint_next_clicked of the "clicked" signal, which updates
    /// the hint counter and shows the hint.
    /// </summary>
    private void OnButtonSplashHintNextClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_hint_next_clicked (which updates the hint counter and shows the hint).
        throw new NotImplementedException();
    }

    /// <summary>
    /// The "Start new game" button was clicked. Corresponds to the C handler
    /// on_button_splash_new_game_clicked of the "clicked" signal, which destroys
    /// the splash window and shows the startup window.
    /// </summary>
    private void OnButtonSplashNewGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_new_game_clicked (which destroys the splash window and shows the startup window).
        throw new NotImplementedException();
    }

    /// <summary>
    /// The "Load game" button was clicked. Corresponds to the C handler
    /// on_button_splash_load_game_clicked of the "clicked" signal, which shows
    /// the file chooser window.
    /// </summary>
    private void OnButtonSplashLoadGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_load_game_clicked (which shows the file chooser window).
        throw new NotImplementedException();
    }

    /// <summary>
    /// The "Resume game" button was clicked. Corresponds to the C handler
    /// on_button_splash_resume_game_clicked of the "clicked" signal, which resumes
    /// the last game.
    /// </summary>
    private void OnButtonSplashResumeGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_resume_game_clicked (which resumes the last game).
        throw new NotImplementedException();
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_button_splash_quit_clicked of the "clicked" signal, which destroys the
    /// splash window and exits the program.
    /// </summary>
    private void OnButtonSplashQuitClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_quit_clicked (which destroys the splash window and exits the program).
        throw new NotImplementedException();
    }
}
