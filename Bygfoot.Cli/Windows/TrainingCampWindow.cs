namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Training camp window of the game. It shows the training and recreation
/// values for the current team and lets the user adjust them.
/// Corresponds to the C function game_gui_show_training_camp and the widget
/// "window_training_camp" of the bygfoot_training.glade support file, which has
/// been converted into the GTK4 builder UI file training_camp.ui.
/// </summary>
public class TrainingCampWindow : Gtk.Window
{
    private readonly Gtk.Builder _builder;

    public TrainingCampWindow()
    {
        _builder = Gtk.Builder.NewFromFile("training_camp.ui");

        var window = (Gtk.Window)_builder.GetObject("window_training_camp") ?? throw new InvalidOperationException("Could not find the widget 'window_training_camp' in the UI file 'training_camp.ui'.");

        window.Modal = true;
        window.DefaultWidth = 550;
        window.DefaultHeight = 600;
        window.Resizable = false;

        Variables.Window.TrainingCamp = this;

        window.OnCloseRequest += OnWindowCloseRequest;

        var buttonOk = (Gtk.Button)_builder.GetObject("b_ok") ?? throw new InvalidOperationException("Could not find the widget 'b_ok' in the UI file 'training_camp.ui'.");
        buttonOk.OnClicked += OnButtonOkClicked;

        var buttonCancel = (Gtk.Button)_builder.GetObject("b_cancel") ?? throw new InvalidOperationException("Could not find the widget 'b_cancel' in the UI file 'training_camp.ui'.");
        buttonCancel.OnClicked += OnButtonCancelClicked;

        var buttonIncRecreation = (Gtk.Button)_builder.GetObject("b_inc_recreation") ?? throw new InvalidOperationException("Could not find the widget 'b_inc_recreation' in the UI file 'training_camp.ui'.");
        buttonIncRecreation.OnClicked += OnBIncRecreationClicked;

        var buttonDecTraining = (Gtk.Button)_builder.GetObject("b_dec_training") ?? throw new InvalidOperationException("Could not find the widget 'b_dec_training' in the UI file 'training_camp.ui'.");
        buttonDecTraining.OnClicked += OnBDecTrainingClicked;

        var buttonIncTraining = (Gtk.Button)_builder.GetObject("b_inc_training") ?? throw new InvalidOperationException("Could not find the widget 'b_inc_training' in the UI file 'training_camp.ui'.");
        buttonIncTraining.OnClicked += OnBIncTrainingClicked;

        var buttonDecRecreation = (Gtk.Button)_builder.GetObject("b_dec_recreation") ?? throw new InvalidOperationException("Could not find the widget 'b_dec_recreation' in the UI file 'training_camp.ui'.");
        buttonDecRecreation.OnClicked += OnBDecRecreationClicked;

        window.Show();
    }

    /// <summary>
    /// The window was asked to be closed (GTK4 equivalent of the former
    /// "delete_event" signal of GTK3). Corresponds to the C handler
    /// on_window_training_camp_delete_event of the close-request signal, which
    /// itself calls on_b_cancel_clicked.
    /// </summary>
    private bool OnWindowCloseRequest(Window sender, EventArgs args)
    {
        // TODO: Call the C function on_window_training_camp_delete_event (which calls on_b_cancel_clicked and destroys the training camp window).
        return true;
    }

    /// <summary>
    /// The "Apply" button was clicked. Corresponds to the C handler
    /// on_b_ok_clicked of the "clicked" signal, which reads the radio button
    /// values, the training/recreation scales, saves the values and destroys the window.
    /// </summary>
    private void OnButtonOkClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_ok_clicked (which calculates the training camp, saves the values and destroys the window).
    }

    /// <summary>
    /// The "Cancel" button was clicked. Corresponds to the C handler
    /// on_b_cancel_clicked of the "clicked" signal, which destroys the window.
    /// </summary>
    private void OnButtonCancelClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_cancel_clicked (which destroys the training camp window).
    }

    /// <summary>
    /// The "Increase" button of the recreation was clicked. Corresponds to the
    /// C handler on_b_inc_recreation_clicked of the "clicked" signal, which
    /// increases the recreation value and decreases the camp points value.
    /// </summary>
    private void OnBIncRecreationClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_inc_recreation_clicked (which updates the recreation and camp points values).
    }

    /// <summary>
    /// The "Decrease" button of the training was clicked. Corresponds to the
    /// C handler on_b_dec_training_clicked of the "clicked" signal, which
    /// decreases the training value and increases the camp points value.
    /// </summary>
    private void OnBDecTrainingClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_dec_training_clicked (which updates the training and camp points values).
    }

    /// <summary>
    /// The "Increase" button of the training was clicked. Corresponds to the
    /// C handler on_b_inc_training_clicked of the "clicked" signal, which
    /// increases the training value and decreases the camp points value.
    /// </summary>
    private void OnBIncTrainingClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_inc_training_clicked (which updates the training and camp points values).
    }

    /// <summary>
    /// The "Decrease" button of the recreation was clicked. Corresponds to the
    /// C handler on_b_dec_recreation_clicked of the "clicked" signal, which
    /// decreases the recreation value and increases the camp points value.
    /// </summary>
    private void OnBDecRecreationClicked(Button sender, EventArgs args)
    {
        // TODO: Call the C function on_b_dec_recreation_clicked (which updates the recreation and camp points values).
    }
}

