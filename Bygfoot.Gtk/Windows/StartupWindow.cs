using Bygfoot.Store;
using Gio;
using Gtk;
using Object = GObject.Object;
using WindowHandle = Gtk.Internal.WindowHandle;

namespace Bygfoot.Gtk.Windows;

public class StartupWindow : Window
{
    [Connect("dropdown_continent")] private DropDown? _dropdownContinent; 
    [Connect("combo_country")] private DropDown? _comboCountry; 
    [Connect("button_team_selection_back")] private Button _buttonBack;
    [Connect("team_selection_cancel")] private Button _buttonCancel;

    private readonly IDefinitionsStore _definitionsStore;
    
    public App App { get; }

    private StartupWindow(Builder builder, App app, IDefinitionsStore definitionsStore)
        : base(new WindowHandle(builder.GetPointer(nameof(StartupWindow)), false))
    {
        builder.Connect(this);

        App = app;
        _buttonBack!.OnClicked += OnBackClicked;
        _buttonCancel!.OnClicked += OnCancelClicked;

        _definitionsStore = definitionsStore;
        
        ShowContinents();
    }

    public StartupWindow(App app, IDefinitionsStore definitionsStore)
        : this(new Builder("startup.ui"), app, definitionsStore)
    { }

    private void ShowContinents()
    {
        var continents = _definitionsStore.GetContinents();
        
        var continentList = StringList.New(continents);
        var continentSelection = SingleSelection.New(continentList);
        
        _dropdownContinent!.Model = continentSelection;
        DropDown.SelectedPropertyDefinition.Notify(
            _dropdownContinent,
            (_, _) =>
            {
                var selectedItem = (StringObject)_dropdownContinent.SelectedItem!;
                var continent = selectedItem.GetString();
                ShowCountries(continent);
            });
    }

    private void ShowCountries(string continent)
    {
        var countryNames = _definitionsStore.GetCountryNames(continent);
        
        var countryNameList = StringList.New(countryNames);
        var countryNameSelection = SingleSelection.New(countryNameList);
        
        _comboCountry!.Model = countryNameSelection;
        DropDown.SelectedPropertyDefinition.Notify(
            _comboCountry,
            (_, _) =>
            {
                var selectedItem = (StringObject)_dropdownContinent.SelectedItem!;
                var countryName = selectedItem.GetString();
            });
    }

    private void ShowCountries()
    {
        var rootList = StringList.New(["asia", "europe", "south america"]);
        var treeListModel = TreeListModel.New(rootList, false, false, CreateModel);
        var selectionModel = SingleSelection.New(treeListModel);
        
        _comboCountry!.Model = selectionModel;
        
        var factory = SignalListItemFactory.New();
        factory.OnSetup += (_, args) =>
        {
            var label = Label.New(null);
            label.Halign = Align.Start;
            label.CanTarget = false;
            
            var expander = TreeExpander.New();
            expander.Child = label;
            
            var listItem = args.Object as ListItem;
            listItem!.Child = expander;
        };
        factory.OnBind += (_, args) =>
        {
            var listItem = args.Object as ListItem;
            var pos = listItem!.Position;

            var treeListRow = treeListModel.GetChildRow(pos);
            
            var expander = listItem.Child as TreeExpander;
            var label = expander!.Child as Label;
            
            var value = treeListRow.Item as StringObject;
            label.SetMarkup(value.GetString());

            expander.ListRow = treeListRow;
        };
        _comboCountry!.Factory = factory;

        // var countryList = new Dictionary<string, string[]>
        // {
        //     ["asia"] = ["china", "japan"],
        //     ["europe"] = ["england", "france", "scotland"],
        //     ["south america"] = ["argentina", "brazil", "chile"],
        // };
        //
        // var rootList = StringList.New(countryList.Keys.ToArray());
        //
        // var treeListModel = TreeListModel.New(rootList, false, false, createModel);
        // var selectionModel = SingleSelection.New(treeListModel);
        // _comboCountry!.SetModel(treeListModel);
        //
        // var factory = SignalListItemFactory.New();
        // factory.OnBind += (sender, args) => {
        //     var listItem = (ListItem)args.Object;            
        //     var pos = listItem.Position;
        //
        //     var expander = listItem.Child as TreeExpander;
        //     var label = expander!.Child as Label;
        //
        //     var value = rootList.GetString(pos);
        //     label!.SetMarkup(value);
        //
        //     var treeListRow = treeListModel.GetChildRow(pos);
        //     expander.ListRow = treeListRow;
        // };
        // _comboCountry.Factory = factory;
    }

    private ListModel? CreateModel(Object item)
    {
        var countryList = new Dictionary<string, string[]>
        {
            ["asia"] = ["china", "japan"],
            ["europe"] = ["england", "france", "scotland"],
            ["south america"] = ["argentina", "brazil", "chile"],
        };
        var stringObject = item as StringObject;
        if (stringObject is null)
        {
            return null;
        }

        var key = stringObject.String;
        if (!countryList.ContainsKey(key))
        {
            return null;
        }

        var childList = StringList.New(countryList[key]);
        return TreeListModel.New(childList, false, true, CreateModel);
    }

    private void OnBackClicked(Button sender, EventArgs args)
    {
        Destroy();
        this.App.ShowSplashWindow();
    }

    private void OnCancelClicked(Button sender, EventArgs args)
    {
        var application = Application;
        Destroy();
        application?.Quit();
    }
}