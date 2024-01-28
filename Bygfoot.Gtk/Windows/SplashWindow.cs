using Bygfoot.Store;
using Gtk;

namespace Bygfoot.Gtk.Windows;

public class SplashWindow : Window
{
    [Connect("listview_splash_contributors")] private ListView _listViewContributors;
    [Connect("label_splash_hintcounter")] private Label _labelHintCounter;
    [Connect("label_splash_hint")] private Label _labelHint;
    [Connect("button_splash_hint_back")] private Button _buttonHintBack;
    [Connect("button_splash_hint_next")] private Button _buttonHintNext;
    [Connect("button_splash_new_game")] private Button _buttonNewGame;
    [Connect("button_splash_load_game")] private Button _buttonLoadGame;
    [Connect("button_splash_resume_game")] private Button _buttonResumeGame;
    [Connect("button_splash_quit")] private Button _buttonQuit;

    private readonly IHelpStore _helpStore;
    private readonly IHintsStore _hintsStore;
    private readonly List<string> _hints;
    private int _hintNum;

    private SplashWindow(Builder builder, string name, IHelpStore helpStore, IHintsStore hintsStore)
        : base(builder.GetPointer(name), false)
    {
        builder.Connect(this);

        this.OnCloseRequest += OnCloseRequestHandler;

        _buttonHintBack!.OnClicked += OnHintBackClicked;
        _buttonHintNext!.OnClicked += OnHintNextClicked;
        _buttonNewGame!.OnClicked += OnNewGameClicked;
        _buttonLoadGame!.OnClicked += OnLoadGameClicked;
        _buttonResumeGame!.OnClicked += OnResumeGameClicked;
        _buttonQuit!.OnClicked += OnQuitClicked;

        _helpStore = helpStore;
        ShowContributors();

        _hintsStore = hintsStore;
        _hints = _hintsStore.GetHints();
        _hintNum = _hintsStore.LoadHintNumber();
        ShowHint();
    }

    public SplashWindow(IHelpStore helpStore, IHintsStore hintsStore)
        : this(new Builder("splash.ui"), "SplashWindow", helpStore, hintsStore)
    { }

    private void WindowDestroy()
    {
        var hintNum = (_hintNum + 1) % _hints.Count;
        _hintsStore.SaveHintNumber(hintNum);
        Destroy();
    }

    private bool OnCloseRequestHandler(Window sender, EventArgs args)
    {
        WindowDestroy();
        return false;
    }

    private void ShowContributors()
    {
        var contributors = _helpStore.GetContributors();
        
        var stringList = StringList.New(null);
        foreach (var contributor in contributors)
        {
            stringList.Append($"\n<span size='large'>{contributor.Title}</span>");
            foreach (var entry in contributor.Entries)
            {
                stringList.Append(entry);
            }
        }

        var selectionModel = NoSelection.New(stringList);
        _listViewContributors.SetModel(selectionModel);

        var factory = SignalListItemFactory.New();
        factory.OnSetup += (_, args) => {
            var label = Label.New("");
            label.Halign = Align.Start;
            label.CanTarget = false;
            ((ListItem)args.Object).Child = label;
        };
        factory.OnBind += (_, args) => {
            var listItem = (ListItem)args.Object;
            var pos = listItem.Position;
            var label = (Label)listItem.Child;
            label.SetMarkup(stringList.GetString(pos));
        };
        _listViewContributors.Factory = factory;
    }

    private void ShowHint()
    {
        var totalHints = _hints.Count;

        var hint = _hints[_hintNum];
        _labelHint.SetLabel(hint);

        var hintCount = $"({_hintNum+1}/{totalHints})";
        _labelHintCounter.SetLabel(hintCount);
    }

    private void OnHintBackClicked(Button sender, EventArgs args)
    {
        _hintNum = _hintNum == 0 ? _hints.Count - 1 : _hintNum - 1;
        ShowHint();
    }

    private void OnHintNextClicked(Button sender, EventArgs args)
    {
        _hintNum = (_hintNum + 1) % _hints.Count;
        ShowHint();
    }

    private void OnNewGameClicked(Button sender, EventArgs args)
    {

    }

    private void OnLoadGameClicked(Button sender, EventArgs args)
    {

    }

    private void OnResumeGameClicked(Button sender, EventArgs args)
    {

    }

    private void OnQuitClicked(Button sender, EventArgs args)
    {
        var application = Application;
        WindowDestroy();
        application?.Quit();
    }
}
