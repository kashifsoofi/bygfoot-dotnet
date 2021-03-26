namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class UserManagementWindow : Window
    {
        public UserManagementWindow() : this(new Builder("UserManagementWindow.glade")) { }

        private UserManagementWindow(Builder builder) : base(builder.GetObject("window_user_management").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_user_management_delete_event(object sender, DeleteEventArgs a)
        {
            Close();
        }

        private void on_treeview_user_management_users_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_treeview_user_management_teams_row_activated(object sender, RowActivatedArgs a)
        {
        }

        private void on_entry_user_management_activate(object sender, EventArgs a)
        {
        }

        private void on_button_user_management_add_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_user_management_close_clicked(object sender, EventArgs a)
        {
            Close();
        }
    }
}