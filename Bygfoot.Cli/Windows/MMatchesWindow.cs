namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Memorable matches window of the game. It shows the current file, the matches
/// tree view, and buttons to open, reload, import, save and reload-and-close the
/// matches file. Corresponds to the C function create_window_mmatches and the
/// widget "window_mmatches" of the bygfoot_misc2.glade support file, which has
/// been converted into the GTK4 builder UI file mmatches.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(MMatchesWindow))]
[Gtk.Template<Gtk.AssemblyResource>("mmatches.ui")]
public partial class MMatchesWindow
{
    [Gtk.Connect("button_mm_file")]
    private Gtk.Button _buttonMmFile;

    [Gtk.Connect("button_mm_reload")]
    private Gtk.Button _buttonMmReload;

    [Gtk.Connect("button_mm_import")]
    private Gtk.Button _buttonMmImport;

    [Gtk.Connect("button_mm_save_close")]
    private Gtk.Button _buttonMmSaveClose;

    [Gtk.Connect("button_mm_reload_close")]
    private Gtk.Button _buttonMmReloadClose;

    partial void Initialize()
    {
        DefaultWidth = 600;
        DefaultHeight = 500;
        Modal = true;

        Variables.Window.MMatches = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonMmFile.OnClicked += OnButtonMmFileClicked;
        _buttonMmReload.OnClicked += OnButtonMmReloadClicked;
        _buttonMmImport.OnClicked += OnButtonMmImportClicked;
        _buttonMmSaveClose.OnClicked += OnButtonMmSaveCloseClicked;
        _buttonMmReloadClose.OnClicked += OnButtonMmReloadCloseClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_mmatches_delete_event of the close-request signal, which
    /// itself calls on_button_mm_reload_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_mmatches_delete_event (which calls on_button_mm_reload_close_clicked and destroys the matches window).
        return true;
    }

    /// <summary>
    /// The "Open" button was clicked. Corresponds to the C handler
    /// on_button_mm_file_clicked of the "clicked" signal, which opens the matches file.
    /// </summary>
    private void OnButtonMmFileClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_mm_file_clicked (which opens the matches file).
    }

    /// <summary>
    /// The "Reload" button was clicked. Corresponds to the C handler
    /// on_button_mm_reload_clicked of the "clicked" signal, which reloads the matches file.
    /// </summary>
    private void OnButtonMmReloadClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_mm_reload_clicked (which reloads the matches file).
    }

    /// <summary>
    /// The "Import file" button was clicked. Corresponds to the C handler
    /// on_button_mm_import_clicked of the "clicked" signal, which imports the matches file.
    /// </summary>
    private void OnButtonMmImportClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_mm_import_clicked (which imports the matches file).
    }

    /// <summary>
    /// The "Save and close" button was clicked. Corresponds to the C handler
    /// on_button_mm_save_close_clicked of the "clicked" signal, which saves the matches file and closes the window.
    /// </summary>
    private void OnButtonMmSaveCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_mm_save_close_clicked (which saves the matches file and closes the window).
    }

    /// <summary>
    /// The "Reload and close" button was clicked. Corresponds to the C handler
    /// on_button_mm_reload_close_clicked of the "clicked" signal, which reloads the matches file and closes the window.
    /// </summary>
    private void OnButtonMmReloadCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_mm_reload_close_clicked (which reloads the matches file and closes the window).
    }
}
