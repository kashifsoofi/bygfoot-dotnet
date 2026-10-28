namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// User management window of the game. It shows the list of users, the list of
/// available teams, an entry to add a user, and buttons to add a user or to
/// close the window.
/// Corresponds to the C function create_window_user_management and the widget
/// "window_user_management" of the bygfoot_misc2.glade support file, which has
/// been converted into the GTK4 builder UI file user_management.ui.
/// </summary>
public class UserManagementWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public UserManagementWindow()
    {
        _builder = Gtk.Builder.NewFromFile("user_management.ui");

        var window = (Gtk.Window)_builder.GetObject("window_user_management") ?? throw new InvalidOperationException("Could not find the widget 'window_user_management' in the UI file 'user_management.ui'.");

        window.Modal = true;
        window.DefaultWidth = 600;
        window.DefaultHeight = 550;

        Variables.Window.UserManagement = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonUserManagementAdd = (Gtk.Button)_builder.GetObject("button_user_management_add") ?? throw new InvalidOperationException("Could not find the widget 'button_user_management_add' in the UI file 'user_management.ui'.");
        buttonUserManagementAdd.OnClicked += OnButtonUserManagementAddClicked;

        var buttonUserManagementClose = (Gtk.Button)_builder.GetObject("button_user_management_close") ?? throw new InvalidOperationException("Could not find the widget 'button_user_management_close' in the UI file 'user_management.ui'.");
        buttonUserManagementClose.OnClicked += OnButtonUserManagementCloseClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_user_management_delete_event of the close-request signal, which
    /// itself calls on_button_user_management_close_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
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
