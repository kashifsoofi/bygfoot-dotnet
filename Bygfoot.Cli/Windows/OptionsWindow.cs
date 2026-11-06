namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Options window of the game. It shows the game options notebook with the
/// global, gameplay, live game, player attributes, betting, training camp and
/// news pages and the save buttons.
/// Corresponds to the C function create_window_options and the widget
/// "window_options" of the bygfoot_options.glade support file, which has been
/// converted into the GTK4 builder UI file options.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(OptionsWindow))]
[Gtk.Template<Gtk.AssemblyResource>("options.ui")]
public partial class OptionsWindow
{
    [Gtk.Connect("button_ok")]
    private Gtk.Button _buttonOptionsOk;

    [Gtk.Connect("button_cancel")]
    private Gtk.Button _buttonOptionsCancel;

    [Gtk.Connect("button_font_name")]
    private Gtk.Button _buttonFontName;

    [Gtk.Connect("button_reload_constants")]
    private Gtk.Button _buttonReloadConstants;

    [Gtk.Connect("button_edit_constants")]
    private Gtk.Button _buttonEditConstants;

    [Gtk.Connect("entry_font_name")]
    private Gtk.Entry _entryFontName;

    public void SetFontName(string name)
    {
        _entryFontName.SetText(name);
    }

    partial void Initialize()
    {
        DefaultWidth = 700;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.Options = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonOptionsOk.OnClicked += OnButtonOptionsOkClicked;
        _buttonOptionsCancel.OnClicked += OnButtonOptionsCancelClicked;
        _buttonFontName.OnClicked += OnButtonFontNameClicked;
        _buttonReloadConstants.OnClicked += OnButtonReloadConstantsClicked;
        _buttonEditConstants.OnClicked += OnButtonEditConstantsClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_options_delete_event of the close-request signal, which itself
    /// calls on_button_options_cancel_clicked and destroys the options window.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_options_delete_event (which calls on_button_options_cancel_clicked and destroys the options window).
        return true;
    }

    /// <summary>
    /// The "Apply" button was clicked. Corresponds to the C handler
    /// on_button_options_ok_clicked of the "clicked" signal, which writes the
    /// options and destroys the options window.
    /// </summary>
    private void OnButtonOptionsOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_options_ok_clicked (which writes the options and destroys the options window).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_options_cancel_clicked of the "clicked" signal, which destroys
    /// the options window.
    /// </summary>
    private void OnButtonOptionsCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_options_cancel_clicked (which destroys the options window).
    }

    /// <summary>
    /// The "Font name" button was clicked. Corresponds to the C handler
    /// on_button_font_name_clicked of the "clicked" signal, which opens the font
    /// selection window.
    /// </summary>
    private void OnButtonFontNameClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_font_name_clicked (which opens the font selection window).
    }

    /// <summary>
    /// The "Reload" button was clicked. Corresponds to the C handler
    /// on_button_reload_constants_clicked of the "clicked" signal, which reloads
    /// the constants file.
    /// </summary>
    private void OnButtonReloadConstantsClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_reload_constants_clicked (which reloads the constants file).
    }

    /// <summary>
    /// The "Edit" button was clicked. Corresponds to the C handler
    /// on_button_edit_constants_clicked of the "clicked" signal, which shows the
    /// constants window.
    /// </summary>
    private void OnButtonEditConstantsClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_edit_constants_clicked (which shows the constants window).
    }
}
