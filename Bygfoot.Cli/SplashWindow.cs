using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// Splash screen window of the game. It shows the splash image on the first
/// notebook page and the list of contributors on the second page, the hint text
/// with the back/next buttons, the new game / load game / resume game / quit
/// buttons and the progress bar. Corresponds to the C function create_window_splash
/// and the widget "window_splash" of the bygfoot_misc3.glade support file, which
/// has been converted into the GTK4 builder UI file splash.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(SplashWindow))]
[Gtk.Template<Gtk.AssemblyResource>("splash.ui")]
public partial class SplashWindow
{
    [Gtk.Connect("label_splash_hintcounter")]
    public Gtk.Label LabelSplashHintCounter;
    
    [Gtk.Connect("label_splash_hint")]
    public Gtk.Label LabelSplashHint;
    
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

    [Gtk.Connect("treeview_splash_contributors")]
    private Gtk.Box _treeViewContributors;

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
    /// Show the list of contributors of the game, loaded from the bygfoot_help
    /// support file. Corresponds to the C function treeview_show_contributors
    /// (src/treeview.c), which window_show_splash calls for this window's tree view.
    /// C renders the options in a tree view column with a markup cell renderer;
    /// here each row is a markup label (the GirCore binding cannot feed data to
    /// a GtkListView, and the box replaces the tree view in the UI file).
    /// </summary>
    public void TreeViewShowContributors()
    {
        Debug.DebugPrintMessage("treeview_show_contributors\n");

        string? helpFile = FileHelper.FindSupportFile("bygfoot_help", true);
        if (helpFile == null)
        {
            // C: game_gui_show_warning(_("Didn't find file 'bygfoot_help'."));
            // (src/game_gui.c not yet converted)
            return;
        }

        OptionList helpList = new();
        FileHelper.LoadOptFile(helpFile, helpList, false);

        // C: the title attribute constant for the section headings.
        string titleAttribute = OptionHelper.ConstApp("string_help_window_title_attribute");

        // C: the tree view is non-selectable, without rules and without headers;
        // none of those concepts apply to the label box.

        foreach (Option opt in helpList.List)
        {
            // C: a row is appended for every option; only the contrib entries
            // occur in the file, anything else is skipped.
            if (!opt.Name.StartsWith("string_contrib_", StringComparison.Ordinal))
                continue;

            Gtk.Label label = new();
            if (opt.Name.StartsWith("string_contrib_title", StringComparison.Ordinal))
                // C: sprintf(buf, "\n<span %s>%s</span>", attr, value)
                label.SetMarkup($"\n<span {titleAttribute}>{opt.StringValue}</span>");
            else
                label.SetMarkup(opt.StringValue ?? string.Empty);

            label.Hexpand = true;
            label.Halign = Align.Start;
            _treeViewContributors.Append(label);
        }
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_splash_delete_event of the close-request signal, which ignores
    /// the close event and does nothing.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // The C handler on_window_splash_delete_event is empty. Unlike GTK3's
        // delete-event, GTK4's close-request has no default destruction, so
        // there is nothing to do here.
        return true;
    }

    /// <summary>
    /// The "Previous" hint button was clicked. Corresponds to the C handler
    /// on_button_splash_hint_back_clicked of the "clicked" signal, which updates
    /// the hint counter and shows the hint.
    /// </summary>
    private void OnButtonSplashHintBackClicked(Button sender, EventArgs args)
    {
        Variables.Counters[(int)Counters.CountHintNumber] =
            (Variables.Counters[(int)Counters.CountHintNumber] == 0) ?
            Variables.Hints.List.Count - 1 :
            Variables.Counters[(int)Counters.CountHintNumber] - 1;

        Window.WindowSplashShowHint();
    }

    /// <summary>
    /// The "Next" hint button was clicked. Corresponds to the C handler
    /// on_button_splash_hint_next_clicked of the "clicked" signal, which updates
    /// the hint counter and shows the hint.
    /// </summary>
    private void OnButtonSplashHintNextClicked(Button sender, EventArgs args)
    {
        Variables.Counters[(int)Counters.CountHintNumber] =
            (Variables.Counters[(int)Counters.CountHintNumber] + 1) % Variables.Hints.List.Count;

        Window.WindowSplashShowHint();
    }

    /// <summary>
    /// The "Start new game" button was clicked. Corresponds to the C handler
    /// on_button_splash_new_game_clicked of the "clicked" signal, which destroys
    /// the splash window and shows the startup window.
    /// </summary>
    private void OnButtonSplashNewGameClicked(Button sender, EventArgs args)
    {
        Gtk.Widget? splash = Variables.Window.Splash;
        Window.WindowDestroy(ref splash);
        Variables.Window.Splash = splash;

        Window.WindowShowStartup();
        Bygfoot.Stat0 = (int)Status0Value.StatusTeamSelection;
    }

    /// <summary>
    /// The "Load game" button was clicked. Corresponds to the C handler
    /// on_button_splash_load_game_clicked of the "clicked" signal, which shows
    /// the file chooser window.
    /// </summary>
    private void OnButtonSplashLoadGameClicked(Button sender, EventArgs args)
    {
        Bygfoot.Stat5 = (int)Status0Value.StatusLoadGameSplash;
        Window.WindowShowFileSel();
    }

    /// <summary>
    /// The "Resume game" button was clicked. Corresponds to the C handler
    /// on_button_splash_resume_game_clicked of the "clicked" signal, which resumes
    /// the last game.
    /// </summary>
    private void OnButtonSplashResumeGameClicked(Button sender, EventArgs args)
    {
        // TODO: misc_callback_startup_load("last_save") (src/misc_callback_func.c not yet converted).
    }

    /// <summary>
    /// The "Quit" button was clicked. Corresponds to the C handler
    /// on_button_splash_quit_clicked of the "clicked" signal, which destroys the
    /// splash window and exits the program.
    /// </summary>
    private void OnButtonSplashQuitClicked(Button sender, EventArgs args)
    {
        Gtk.Widget? splash = Variables.Window.Splash;
        Window.WindowDestroy(ref splash);
        Variables.Window.Splash = splash;
        Main.MainExitProgram(Bygfoot.ExitCode.Ok, null);
    }
}
