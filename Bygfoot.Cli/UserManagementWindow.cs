using Gtk;

namespace Bygfoot.Cli;

/// <summary>
/// User management window of the game. It shows the list of users, the list of
/// available teams, an entry to add a user, and buttons to add a user or to
/// close the window.
/// Corresponds to the C function create_window_user_management and the widget
/// "window_user_management" of the bygfoot_misc2.glade support file, which has
/// been converted into the GTK4 builder UI file user_management.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(UserManagementWindow))]
[Gtk.Template<Gtk.AssemblyResource>("user_management.ui")]
public partial class UserManagementWindow
{
    [Gtk.Connect("button_user_management_add")]
    private Gtk.Button _buttonUserManagementAdd;

    [Gtk.Connect("button_user_management_close")]
    private Gtk.Button _buttonUserManagementClose;

    partial void Initialize()
    {
        DefaultWidth = 600;
        DefaultHeight = 550;
        Modal = true;

        Variables.Window.UserManagement = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonUserManagementAdd.OnClicked += OnButtonUserManagementAddClicked;
        _buttonUserManagementClose.OnClicked += OnButtonUserManagementCloseClicked;

        Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_user_management_delete_event of the close-request signal, which
    /// itself calls on_button_user_management_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Gtk.Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_user_management_delete_event (which calls on_button_user_management_close_clicked and destroys the user management window).
        return true;
    }

    /// <summary>
    /// The "Add user" button was clicked. Corresponds to the C handler
    /// on_button_user_management_add_clicked of the "clicked" signal, which
    /// adds the user via the misc2 callback.
    /// </summary>
    private void OnButtonUserManagementAddClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_user_management_add_clicked (which adds the user via misc2_callback_add_user).
    }

    /// <summary>
    /// The "Close" button was clicked. Corresponds to the C handler
    /// on_button_user_management_close_clicked of the "clicked" signal, which
    /// destroys the user management window and returns to the main window.
    /// </summary>
    private void OnButtonUserManagementCloseClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_button_user_management_close_clicked (which destroys the user management window and returns to the main window).
    }
}
