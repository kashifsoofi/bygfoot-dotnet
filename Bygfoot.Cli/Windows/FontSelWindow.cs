namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Font selection window of the game. It is shown when the user picks a font
/// in the options and lets him choose the font to use.
/// Corresponds to the C function create_window_font_sel and the widget
/// "window_font_sel" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file font_sel.ui.
/// </summary>
public class FontSelWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public FontSelWindow()
    {
        _builder = Gtk.Builder.NewFromFile("font_sel.ui");

        var window = (Gtk.Window)_builder.GetObject("window_font_sel") ?? throw new InvalidOperationException("Could not find the widget 'window_font_sel' in the UI file 'font_sel.ui'.");

        window.Modal = true;
        window.DefaultWidth = 550;
        window.DefaultHeight = 600;

        Variables.Window.FontSel = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonFontSelOk = (Gtk.Button)_builder.GetObject("button_font_sel_ok") ?? throw new InvalidOperationException("Could not find the widget 'button_font_sel_ok' in the UI file 'font_sel.ui'.");
        buttonFontSelOk.OnClicked += OnButtonFontSelOkClicked;

        var buttonFontSelApply = (Gtk.Button)_builder.GetObject("button_font_sel_apply") ?? throw new InvalidOperationException("Could not find the widget 'button_font_sel_apply' in the UI file 'font_sel.ui'.");
        buttonFontSelApply.OnClicked += OnButtonFontSelApplyClicked;

        var buttonFontSelCancel = (Gtk.Button)_builder.GetObject("button_font_sel_cancel") ?? throw new InvalidOperationException("Could not find the widget 'button_font_sel_cancel' in the UI file 'font_sel.ui'.");
        buttonFontSelCancel.OnClicked += OnButtonFontSelCancelClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_button_font_sel_cancel_clicked of the delete_event signal.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_button_font_sel_cancel_clicked (which destroys the window).
        return true;
    }

    /// <summary>
    /// The "OK" button was clicked. Corresponds to the C handler
    /// on_button_font_sel_ok_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonFontSelOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_font_sel_ok_clicked (which reads the selected font and stores it).
    }

    /// <summary>
    /// The "Apply" button was clicked. Corresponds to the C handler
    /// on_button_font_sel_apply_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonFontSelApplyClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_font_sel_apply_clicked (which stores the selected font).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_button_font_sel_cancel_clicked of the "clicked" signal.
    /// </summary>
    private void OnButtonFontSelCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_font_sel_cancel_clicked (which destroys the window).
    }
}
