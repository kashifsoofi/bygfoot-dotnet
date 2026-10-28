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
public class OptionsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public OptionsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("options.ui");

        var window = (Gtk.Window)_builder.GetObject("window_options") ?? throw new InvalidOperationException("Could not find the widget 'window_options' in the UI file 'options.ui'.");

        window.DefaultWidth = 700;
        window.DefaultHeight = 600;

        Variables.Window.Options = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonOptionsOk = (Gtk.Button)_builder.GetObject("button_options_ok") ?? throw new InvalidOperationException("Could not find the widget 'button_options_ok' in the UI file 'options.ui'.");
        buttonOptionsOk.OnClicked += OnButtonOptionsOkClicked;

        var buttonOptionsCancel = (Gtk.Button)_builder.GetObject("button_options_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_options_cancel' in the UI file 'options.ui'.");
        buttonOptionsCancel.OnClicked += OnButtonOptionsCancelClicked;

        var buttonFontName = (Gtk.Button)_builder.GetObject("button_font_name") ?? throw new InvalidOperationException("Could not find the widget 'button_font_name' in the UI file 'options.ui'.");
        buttonFontName.OnClicked += OnButtonFontNameClicked;

        var buttonReloadConstants = (Gtk.Button)_builder.GetObject("button_reload_constants") ?? throw new InvalidOperationException("Could not find the widget 'button_reload_constants' in the UI file 'options.ui'.");
        buttonReloadConstants.OnClicked += OnButtonReloadConstantsClicked;

        var buttonEditConstants = (Gtk.Button)_builder.GetObject("button_edit_constants") ?? throw new InvalidOperationException("Could not find the widget 'button_edit_constants' in the UI file 'options.ui'.");
        buttonEditConstants.OnClicked += OnButtonEditConstantsClicked;

        window.Show();
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
