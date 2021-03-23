namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class StartupWindow : Window
    {
        [UI] private ComboBox combo_country = null;
        [UI] private TreeView treeview_startup = null;
        [UI] private ComboBox combobox_start_league = null;
        [UI] private Button button_add_player = null;
        [UI] private TreeView treeview_users = null;
        [UI] private Entry entry_player_name = null;
        [UI] private Button team_selection_ok = null;
        [UI] private RadioButton radiobutton_team_def_load = null;
        [UI] private RadioButton radiobutton_team_def_names = null;
        [UI] private CheckButton checkbutton_randomise_teams = null;

        public StartupWindow() : this(new Builder("StartupWindow.glade")) { }

        private StartupWindow(Builder builder) : base(builder.GetObject("window_startup").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_startup_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_combo_country_changed(object sender, EventArgs a)
        {
        }

        private void on_treeview_users_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_entry_player_name_activate(object sender, EventArgs a)
        {
        }

        private void on_button_add_player_clicked(object sender, EventArgs a)
        {
        }

        private void on_team_selection_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_team_selection_back_clicked(object sender, EventArgs a)
        {
        }

        private void on_team_selection_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}