namespace Bygfoot.Cli;

/// <summary>
/// What kind of picture we on top of the progress bar.
/// Corresponds to C: enum PicType in src/gui.h.
/// </summary>
public enum PicType
{
    /// <summary>Corresponds to C: PIC_TYPE_NONE.</summary>
    PicTypeNone = 0,
    /// <summary>Corresponds to C: PIC_TYPE_SAVE.</summary>
    PicTypeSave,
    /// <summary>Corresponds to C: PIC_TYPE_LOAD.</summary>
    PicTypeLoad,
    /// <summary>Corresponds to C: PIC_TYPE_MATCHPIC.</summary>
    PicTypeMatchpic
}

/// <summary>
/// Static class for the GUI helper functions (progress bar, arrow buttons,
/// live game meters).
/// Corresponds to the C module src/gui.h and src/gui.c.
/// </summary>
public static class Gui
{
    /// <summary>
    /// Set into or append an integer into a label.
    /// Corresponds to C: gui_label_set_text_from_int(GtkLabel *label, gint number, gboolean append).
    /// </summary>
    /// <param name="label">The label.</param>
    /// <param name="number">The number.</param>
    /// <param name="append">Whether or not to append.</param>
    public static void GuiLabelSetTextFromInt(Gtk.Label label, int number, bool append)
    {
        string currentText = label.GetLabel() ?? "";
        string buf = number > 1000 ? Misc.MiscPrintGroupedInt(number) : number.ToString();

        label.SetLabel(append ? currentText + buf : buf);
    }

    /// <summary>
    /// Find a widget in a widget tree by its name.
    /// Corresponds to C: lookup_widget(GtkWidget *widget, const gchar *widget_name)
    /// in src/support.c (not yet converted) — inlined here since it is needed
    /// by the gui functions.
    /// </summary>
    /// <param name="widget">The widget to search in.</param>
    /// <param name="name">The name of the widget to search for.</param>
    /// <returns>The found widget or null.</returns>
    internal static Gtk.Widget? LookupWidget(Gtk.Widget? widget, string name)
    {
        if (widget == null)
            return null;

        if (widget.Name == name)
            return widget;

        for (Gtk.Widget? child = widget.GetFirstChild(); child != null; child = child.GetNextSibling())
        {
            Gtk.Widget? found = LookupWidget(child, name);
            if (found != null)
                return found;
        }

        Serilog.Log.Warning("Widget not found: {Name}", name);
        return null;
    }

    /// <summary>
    /// Destroy the progress window and clear the window pointer.
    /// Corresponds to C: window_destroy(&window.progress) (src/window.c not yet
    /// converted) — inlined for the progress window here.
    /// </summary>
    private static void DestroyProgressWindow()
    {
        if (Variables.Window.Progress == null)
            return;

        (Variables.Window.Progress as Gtk.Window)?.Destroy();
        Variables.Window.Progress = null;
    }

    /// <summary>
    /// Show a window with a progress bar.
    /// Corresponds to C: gui_show_progress(gfloat value, const gchar *text, gint pictype).
    /// </summary>
    /// <param name="value">The value of the progress bar. If set to 1 or &lt; 0 the progress bar window gets destroyed.</param>
    /// <param name="text">The text to show in the progress bar.</param>
    /// <param name="pictype">What kind of picture to show above the progress bar.</param>
    public static void GuiShowProgress(float value, string? text, PicType pictype)
    {
        if (OptionHelper.SettInt("int_opt_goto_mode") != 0)
        {
            DestroyProgressWindow();
            return;
        }

        if (value == 1 || value < 0)
        {
            DestroyProgressWindow();
            return;
        }

        if (Variables.Window.Progress == null)
        {
            // C: window_show_progress(pictype) — creates the progress window and
            // sets the picture above the progress bar according to pictype
            // (src/window.c not yet converted — the stock/pixmap picture logic
            // is omitted here).
            ProgressWindow.NewWithProperties([]);
        }

        Gtk.ProgressBar? progressbar = LookupWidget(Variables.Window.Progress as Gtk.Widget, "progressbar") as Gtk.ProgressBar;

        if (value >= 0 && value < 1)
            progressbar!.Fraction = value;
        else
            progressbar!.Pulse();

        if (text != null)
            progressbar!.Text = text;

        GLib.MainContext context = GLib.MainContext.Default();
        while (context.Pending())
            context.Iteration(false);
    }

    /// <summary>
    /// Get the fraction of the progress bar (0 if the goto mode is on).
    /// Corresponds to C: gui_get_progress_bar_fraction(void).
    /// </summary>
    /// <returns>The fraction of the progress bar.</returns>
    public static double GuiGetProgressBarFraction()
    {
        if (OptionHelper.SettInt("int_opt_goto_mode") != 0)
            return 0;

        return (LookupWidget(Variables.Window.Progress as Gtk.Widget, "progressbar") as Gtk.ProgressBar)!.Fraction;
    }

    /// <summary>
    /// Set either the right pair of arrows atop the right treeview or the left
    /// pair or both to the specified sensitivity state.
    /// Corresponds to C: gui_set_arrow_pair(gint pair, gboolean state).
    /// </summary>
    /// <param name="pair">0 for the left pair, 1 for the right pair, 2 or more for both.</param>
    /// <param name="state">The sensitivity state.</param>
    public static void GuiSetArrowPair(int pair, bool state)
    {
        Gtk.Widget main = (Gtk.Widget)Variables.Window.Main;
        Gtk.Widget?[,] buttons =
        {
            { LookupWidget(main, "button_cl_back"),
              LookupWidget(main, "button_cl_forward") },
            { LookupWidget(main, "button_browse_back"),
              LookupWidget(main, "button_browse_forward") }
        };

        if (pair < 3)
        {
            for (int i = 0; i < 2; i++)
                buttons[pair, i]!.Sensitive = state;
        }
        else
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    buttons[i, j]!.Sensitive = state;
        }
    }

    /// <summary>
    /// Examine the status variable and set the sensitivity of the arrows atop
    /// the right treeview accordingly.
    /// Corresponds to C: gui_set_arrows(void).
    /// </summary>
    public static void GuiSetArrows()
    {
        GuiSetArrowPair(3, false);

        if (Bygfoot.Stat0 == (int)Status0Value.StatusShowFixtures ||
            Bygfoot.Stat0 == (int)Status0Value.StatusBrowseTeams ||
            Bygfoot.Stat0 == (int)Status0Value.StatusShowSeasonHistory)
            GuiSetArrowPair(3, true);
        else if (Bygfoot.Stat0 == (int)Status0Value.StatusShowPlayerInfo ||
                 Bygfoot.Stat0 == (int)Status0Value.StatusShowFixturesWeek)
            GuiSetArrowPair(1, true);
        else if (Bygfoot.Stat0 == (int)Status0Value.StatusShowTables ||
                 Bygfoot.Stat0 == (int)Status0Value.StatusShowPlayerList ||
                 Bygfoot.Stat0 == (int)Status0Value.StatusShowLeagueStats)
            GuiSetArrowPair(0, true);
    }

    /// <summary>
    /// Set the sensitivity of the live game window meters and their
    /// eventboxes.
    /// Corresponds to C: gui_set_sensitive_lg_meters(gboolean state).
    /// </summary>
    /// <param name="state">The sensitivity state.</param>
    public static void GuiSetSensitiveLgMeters(bool state)
    {
        Gtk.Widget live = (Gtk.Widget)Variables.Window.Live;

        // C also sets the inner images image_lg_style and image_lg_boost; in
        // the GTK4 UI the event box wrappers were removed and the images carry
        // the eventbox_* ids, so one widget per meter is enough.
        LookupWidget(live, "eventbox_lg_style")!.Sensitive = state;
        LookupWidget(live, "eventbox_lg_boost")!.Sensitive = state;
    }
}
