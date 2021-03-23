namespace Bygfoot
{
    using System;
    using Gtk;
    using UI = Gtk.Builder.ObjectAttribute;

    class TrainingCampWindow : Window
    {
        public TrainingCampWindow() : this(new Builder("TrainingCampWindow.glade")) { }

        private TrainingCampWindow(Builder builder) : base(builder.GetObject("window_training_camp").Handle)
        {
            builder.Autoconnect(this);
        }

        private void on_window_training_camp_delete_event(object sender, DeleteEventArgs a)
        {
        }

        private void on_b_inc_recreation_clicked(object sender, EventArgs a)
        {
        }

        private void on_b_dec_training_clicked(object sender, EventArgs a)
        {
        }

        private void on_b_inc_training_clicked(object sender, EventArgs a)
        {
        }

        private void on_b_dec_recreation_clicked(object sender, EventArgs a)
        {
        }

        private void on_rb_camp1_clicked(object sender, EventArgs a)
        {
        }

        private void on_rb_camp2_clicked(object sender, EventArgs a)
        {
        }

        private void on_rb_camp3_clicked(object sender, EventArgs a)
        {
        }

        private void on_b_ok_clicked(object sender, EventArgs a)
        {
        }

        private void on_b_cancel_clicked(object sender, EventArgs a)
        {
        }
    }
}