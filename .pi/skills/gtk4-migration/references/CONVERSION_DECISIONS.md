# Bygfoot GTK2 → GTK4 Conversion Decision Log

## Session context
Converting the `window_startup` widget from `support_files/bygfoot_misc.glade` (GTK2/GtkBuilder format) into a GTK4 builder `.ui` file and a GirCore C# class in the `Bygfoot.Cli` project, following the ongoing C# port of the GTK2-based Bygfoot Football Manager.

## Deliverables produced in the session
- `Bygfoot.Cli/Windows/startup.ui` — GTK4 builder file with the `window_startup` widget (widget ids preserved: `window_startup`, `vbox2`, `vbox34`, `label69`, `combo_country`, `label121`, `hpaned1`, `scrolledwindow10`, `treeview_users`, `scrolledwindow1`, `treeview_startup`, `hseparator11`, `label4`, `combobox_start_league`, `hseparator12`, `label75`, `hbox54`, `entry_player_name`, `button_add_player`, `hseparator13`, `hbox74`, `radiobutton_team_def_load/names/dont`, `vseparator2`, `checkbutton_randomise_teams`, `hseparator16`, `hbox3`, `team_selection_ok`, `button_team_selection_back`, `team_selection_cancel`).
- `Bygfoot.Cli/Windows/StartupWindow.cs` — GirCore class wiring the builder, setting `Modal`, `DefaultWidth`, `DefaultHeight`, wiring `close-request`, button/ combo/ entry signals, calling `Show()`.
- `Bygfoot.Cli/Bygfoot.Cli.csproj` — added `GirCore.Gtk-4.0` package reference (CPM requires version-less) and `Content` copy of `Windows\*.ui`.
- `Bygfoot.Cli/Variables.cs` — `Windows` → `Models.Windows` (namespace collision with new `Bygfoot.Cli.Windows`).

## Decisions made during conversion (user-approved)
1. **Class approach**: builder-based class is fine (GirCore 0.6.3 has no `[Gtk.Template]` yet; Builder-based pattern compiled successfully).
2. **File name**: `startup.ui` (strip `window_` prefix, matching the earlier GladeToUi tool convention).
3. **GTK version**: `<requires lib="gtk" version="4.12"/>`.
4. **Close semantics**: GTK3 `delete_event` handler (`on_team_selection_cancel_clicked`) → GTK4 `close-request`; handler returns `true`.
5. **TODOs**: game-logic wiring deferred (call `main_exit_program`, `misc_callback_*`, etc. later).
6. **Tree interactions**: use GTK4 event controllers later (no TreeView `button-press-event` signal in GirCore 0.6.3).
7. **Deprecated widgets**: migrate `GtkComboBox`, `GtkTreeView`, `GtkRadioButton`, etc. to modern equivalents (DropDown, ListView+ColumnView) later.
8. **`.ui` loading**: file-based (`Gtk.Builder.NewFromFile`) is fine for now.

## Open questions / future work
- Switch to `[Gobject.Subclass<Gtk.Window>]` and/or embedded gresource loading (`glib-compile-resources`) later.
- Implement exact widget-level TODOs and full C→C# handler mappings for the startup flow.
- Wire the class into `Variables.Window` / the `Models.Windows` struct and the game logic.
