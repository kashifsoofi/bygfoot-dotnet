namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class MainWindow : Window
    {
        [UI] private Label _label1 = null;
        [UI] private Button _button1 = null;

        private int _counter;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("main_window").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_main_window_delete_event(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void on_menu_new_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_open_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_load_last_save_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_save_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_save_as_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_quit_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_preferences_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_check_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_menu_save_window_geometry_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_fixtures_week_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_fixtures_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_tables_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_my_league_results_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_season_results_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_team_button_release_event(object sender, ButtonReleaseEventArgs a)
        {
        }

        private void on_menu_show_youth_academy_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_set_investment_activate(object sender, EventArgs a)
        {
        }

        private void on_default_team1_activate(object sender, EventArgs a)
        {
        }

        private void on_default_team_store_activate(object sender, EventArgs a)
        {
        }

        private void on_default_team_restore_activate(object sender, EventArgs a)
        {
        }

        private void on_training_camp_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_reset_players_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_custom_structure_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_rearrange_team_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_browse_teams_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_show_job_exchange_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_show_info_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_put_on_transfer_list_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_remove_from_transfer_list_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_offer_new_contract_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_fire_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_shoots_penalties_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_move_to_youth_academy_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_edit_name_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_browse_players_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_user_show_last_match_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_user_show_last_stats_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_user_show_coming_matches_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_next_user_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_previous_user_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_manage_users_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_show_finances_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_show_stadium_activate(object sender, EventArgs a)
        {
        }

        private void on_automatic_loan_repayment_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_betting_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_news_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_league_stats_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_season_history_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_user_show_history_activate(object sender, EventArgs a)
        {
        }

        private void on_mm_add_last_match_activate(object sender, EventArgs a)
        {
        }

        private void on_mm_manage_matches_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_about_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_contributors_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_help_activate(object sender, EventArgs a)
        {
        }

        private void on_button_load_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_save_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_quit_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_button_quit_clicked(object sender, EventArgs a)
        {
            ProcessEvent(Gdk.EventHelper.New(Gdk.EventType.Delete));
        }

        private void on_button_back_to_main_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_transfers_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_new_week_clicked(object sender, EventArgs a)
        {
        }

        private void on_eventbox_style_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_eventbox_boost_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_hpaned2_button_release_event(object sender, ButtonReleaseEventArgs a)
        {
        }

        private void on_player_list1_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_player_list1_key_press_event(object sender, KeyPressEventArgs a)
        {
        }

        private void on_button_reset_players_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_cl_back_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_cl_forward_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_browse_back_clicked(object sender, EventArgs a)
        {
        }

        private void on_button_browse_forward_clicked(object sender, EventArgs a)
        {
        }

        private void on_treeview_right_button_press_event(object sender, ButtonPressEventArgs a)
        {
        }

        private void on_player_menu_show_info_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_put_on_transfer_list_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_remove_from_transfer_list_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_offer_new_contract_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_fire_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_shoots_penalties_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_move_to_youth_academy_activate(object sender, EventArgs a)
        {
        }

        private void on_player_menu_edit_name_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_youth_move_to_team_activate(object sender, EventArgs a)
        {
        }

        private void on_menu_youth_kick_out_of_academy_activate(object sender, EventArgs a)
        {
        }
    }
}
