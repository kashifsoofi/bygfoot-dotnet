namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Constants window of the game. It shows the constants notebook with the
/// integer, float, string and appearance pages and the save, reload and close
/// buttons.
/// Corresponds to the C function create_window_constants and the widget
/// "window_constants" of the bygfoot_options.glade support file, which has been
/// converted into the GTK4 builder UI file constants.ui.
/// </summary>
public class ConstantsWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public ConstantsWindow()
    {
        _builder = Gtk.Builder.NewFromFile("constants.ui");

        var window = (Gtk.Window)_builder.GetObject("window_constants") ?? throw new InvalidOperationException("Could not find the widget 'window_constants' in the UI file 'constants.ui'.");

        window.DefaultWidth = 650;
        window.DefaultHeight = 600;

        Variables.Window.Constants = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonConstantsSave = (Gtk.Button)_builder.GetObject("button_constants_save") ?? throw new InvalidOperationException("Could not find the widget 'button_constants_save' in the UI file 'constants.ui'.");
        buttonConstantsSave.OnClicked += OnButtonConstantsSaveClicked;

        var buttonConstantsReload = (Gtk.Button)_builder.GetObject("button_constants_reload") ?? throw new InvalidOperationException("Could not find the widget 'button_constants_reload' in the UI file 'constants.ui'.");
        buttonConstantsReload.OnClicked += OnButtonConstantsReloadClicked;

        var buttonConstantsClose = (Gtk.Button)_builder.GetObject("button_constants_close") ?? throw new InvalidOperationException("Could not find the widget 'button_constants_close' in the UI file 'constants.ui'.");
        buttonConstantsClose.OnClicked += OnButtonConstantsCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_constants_delete_event of the close-request signal, which itself
    /// calls on_button_constants_close_clicked and destroys the constants window.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_constants_delete_event (which calls on_button_constants_close_clicked and destroys the constants window).
        return true;
    }

    /// <summary>
    /// The "Save to file" button was clicked. Corresponds to the C handler
    /// on_button_constants_save_clicked of the "clicked" signal, which saves the
    /// constants to the current file.
    /// </summary>
    private void OnButtonConstantsSaveClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_constants_save_clicked (which saves the constants to the current file).
    }

    /// <summary>
    /// The "Reload from file" button was clicked. Corresponds to the C handler
    /// on_button_constants_reload_clicked of the "clicked" signal, which reloads
    /// the constants from the file.
    /// </summary>
    private void OnButtonConstantsReloadClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_constants_reload_clicked (which reloads the constants from the file).
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_constants_close_clicked of the "clicked" signal, which destroys
    /// the constants window.
    /// </summary>
    private void OnButtonConstantsCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_constants_close_clicked (which destroys the constants window).
    }
}
