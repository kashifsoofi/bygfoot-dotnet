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
public class SplashWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public SplashWindow()
    {
        _builder = Gtk.Builder.NewFromFile("splash.ui");

        var window = (Gtk.Window)_builder.GetObject("window_splash") ?? throw new InvalidOperationException("Could not find the widget 'window_splash' in the UI file 'splash.ui'.");

        window.DefaultWidth = 500;
        window.DefaultHeight = 400;

        Variables.Window.Splash = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonSplashHintBack = (Gtk.Button)_builder.GetObject("button_splash_hint_back") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_hint_back' in the UI file 'splash.ui'.");
        buttonSplashHintBack.OnClicked += OnButtonSplashHintBackClicked;

        var buttonSplashHintNext = (Gtk.Button)_builder.GetObject("button_splash_hint_next") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_hint_next' in the UI file 'splash.ui'.");
        buttonSplashHintNext.OnClicked += OnButtonSplashHintNextClicked;

        var buttonSplashNewGame = (Gtk.Button)_builder.GetObject("button_splash_new_game") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_new_game' in the UI file 'splash.ui'.");
        buttonSplashNewGame.OnClicked += OnButtonSplashNewGameClicked;

        var buttonSplashLoadGame = (Gtk.Button)_builder.GetObject("button_splash_load_game") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_load_game' in the UI file 'splash.ui'.");
        buttonSplashLoadGame.OnClicked += OnButtonSplashLoadGameClicked;

        var buttonSplashResumeGame = (Gtk.Button)_builder.GetObject("button_splash_resume_game") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_resume_game' in the UI file 'splash.ui'.");
        buttonSplashResumeGame.OnClicked += OnButtonSplashResumeGameClicked;

        var buttonSplashQuit = (Gtk.Button)_builder.GetObject("button_splash_quit") ?? throw new InvalidOperationException("Could not find the widget 'button_splash_quit' in the UI file 'splash.ui'.");
        buttonSplashQuit.OnClicked += OnButtonSplashQuitClicked;

        window.Show();
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
    }

    /// <summary>
    /// The "Next" hint button was clicked. Corresponds to the C handler
    /// on_button_splash_hint_next_clicked of the "clicked" signal, which updates
    /// the hint counter and shows the hint.
    /// </summary>
    private void OnButtonSplashHintNextClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_hint_next_clicked (which updates the hint counter and shows the hint).
    }

    /// <summary>
    /// The "Start new game" button was clicked. Corresponds to the C handler
    /// on_button_splash_new_game_clicked of the "clicked" signal, which destroys
    /// the splash window and shows the startup window.
    /// </summary>
    private void OnButtonSplashNewGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_new_game_clicked (which destroys the splash window and shows the startup window).
    }

    /// <summary>
    /// The "Load game" button was clicked. Corresponds to the C handler
    /// on_button_splash_load_game_clicked of the "clicked" signal, which shows
    /// the file chooser window.
    /// </summary>
    private void OnButtonSplashLoadGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_load_game_clicked (which shows the file chooser window).
    }

    /// <summary>
    /// The "Resume game" button was clicked. Corresponds to the C handler
    /// on_button_splash_resume_game_clicked of the "clicked" signal, which resumes
    /// the last game.
    /// </summary>
    private void OnButtonSplashResumeGameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_resume_game_clicked (which resumes the last game).
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_button_splash_quit_clicked of the "clicked" signal, which destroys the
    /// splash window and exits the program.
    /// </summary>
    private void OnButtonSplashQuitClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_splash_quit_clicked (which destroys the splash window and exits the program).
    }
}
