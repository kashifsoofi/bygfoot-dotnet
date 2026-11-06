namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Font selection window of the game. It is shown when the user picks a font
/// in the options and lets him choose the font to use.
/// Corresponds to the C function create_window_font_sel and the widget
/// "window_font_sel" of the bygfoot_misc.glade support file, which has been
/// converted into the GTK4 builder UI file font_sel.ui.
/// </summary>
[GObject.Subclass<Gtk.FontChooserDialog>(qualifiedName: nameof(FontSelWindow))]
[Gtk.Template<Gtk.AssemblyResource>("font_sel.ui")]
public partial class FontSelWindow
{
    [Gtk.Connect("font_sel")]
    private Gtk.FontChooserWidget _fontSel;

    [Gtk.Connect("button_font_sel_cancel")]
    private Gtk.Button _buttonFontSelCancel;

    [Gtk.Connect("button_font_sel_apply")]
    private Gtk.Button _buttonFontSelApply;

    [Gtk.Connect("button_font_sel_ok")]
    private Gtk.Button _buttonFontSelOk;

    partial void Initialize()
    {
        DefaultWidth = 550;
        DefaultHeight = 600;
        Modal = true;

        Variables.Window.FontSel = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonFontSelCancel.OnClicked += OnButtonFontSelCancelClicked;
        _buttonFontSelApply.OnClicked += OnButtonFontSelApplyClicked;
        _buttonFontSelOk.OnClicked += OnButtonFontSelOkClicked;

        Show();
    }

    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        this.Close();
        return true;
    }

    private void OnButtonFontSelOkClicked(Button sender, EventArgs args)
    {
        var font = _fontSel.Font;
        if (!string.IsNullOrEmpty(font))
        {
            if (Variables.Window.Options is OptionsWindow options)
            {
                options.SetFontName(font);
            }
        }
        this.Close();
    }

    private void OnButtonFontSelApplyClicked(Button sender, EventArgs args)
    {
        var font = _fontSel.Font;
        if (!string.IsNullOrEmpty(font))
        {
            Variables.Options.SetString("string_opt_font_name", font);
            if (Variables.Window.Options is OptionsWindow options)
            {
                options.SetFontName(font);
            }
        }
        this.Close();
    }

    private void OnButtonFontSelCancelClicked(Button sender, EventArgs args)
    {
        this.Close();
    }
}
