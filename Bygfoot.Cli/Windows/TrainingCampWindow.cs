namespace Bygfoot.Cli.Windows;

using Gtk;

/// <summary>
/// Training camp window of the game. It shows the training and recreation
/// values for the current team and lets the user adjust them.
/// Corresponds to the C function game_gui_show_training_camp and the widget
/// "window_training_camp" of the bygfoot_training.glade support file, which has
/// been converted into the GTK4 builder UI file training_camp.ui.
/// </summary>
[GObject.Subclass<Gtk.Window>(qualifiedName: nameof(TrainingCampWindow))]
[Gtk.Template<Gtk.AssemblyResource>("training_camp.ui")]
public partial class TrainingCampWindow
{
    [Gtk.Connect("b_ok")]
    private Gtk.Button _buttonOk;

    [Gtk.Connect("b_cancel")]
    private Gtk.Button _buttonCancel;

    [Gtk.Connect("b_inc_recreation")]
    private Gtk.Button _buttonIncRecreation;

    [Gtk.Connect("b_dec_training")]
    private Gtk.Button _buttonDecTraining;

    [Gtk.Connect("b_inc_training")]
    private Gtk.Button _buttonIncTraining;

    [Gtk.Connect("b_dec_recreation")]
    private Gtk.Button _buttonDecRecreation;

    partial void Initialize()
    {
        DefaultWidth = 550;
        DefaultHeight = 600;
        Modal = true;
        Resizable = false;

        Variables.Window.TrainingCamp = this;

        OnCloseRequest += OnWindowCloseRequest;

        _buttonOk.OnClicked += OnButtonOkClicked;
        _buttonCancel.OnClicked += OnButtonCancelClicked;
        _buttonIncRecreation.OnClicked += OnBIncRecreationClicked;
        _buttonDecTraining.OnClicked += OnBDecTrainingClicked;
        _buttonIncTraining.OnClicked += OnBIncTrainingClicked;
        _buttonDecRecreation.OnClicked += OnBDecRecreationClicked;

        Show();
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
