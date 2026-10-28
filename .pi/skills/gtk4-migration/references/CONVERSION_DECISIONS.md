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

## Session status (2026-10-28)
- Validation run: `dotnet build Bygfoot.Cli/Bygfoot.Cli.csproj` ends with **0 errors** (295 pre-existing nullable warnings, unrelated).
- `Bygfoot.Cli/Windows/startup.ui` validated as well-formed XML (via Python `xml.dom.minidom`).
- Conversion notes: GTK3-only properties (`border_width`, `window_position`, `ypad`, packing `expand/fill`, `use_stock`, `<accelerator>`) were dropped as per decisions; booleans written as `0`/`1`; enums by name; `<GtkVBox>`/`<GtkHBox` → `GtkBox` with `orientation`; separators got `orientation`; buttons use `label` + `icon-name`.
- Known remaining conversion gaps in `startup.ui`: the `close-request` handler name in the builder (`on_team_selection_cancel_clicked`) is mirrored by the C# handler `OnWindowCloseRequest`; `GtkComboBox`/`GtkTreeView`/`GtkRadioButton` remain deprecated (to be migrated later per decisions); the `button-press-event` signal on the `treeview_users` TreeView has no GTK4 equivalent yet (event controllers to be used later).

## Session status (2026-10-29)
- Converted next widget: `window_font_sel` (`GtkFontSelectionDialog`) → `Bygfoot.Cli/Windows/font_sel.ui` (GTK4 builder file, `GtkFontChooserDialog`, GTK4 version 4.12) + `Bygfoot.Cli/Windows/FontSelWindow.cs` (GirCore class deriving from `Gtk.Window`, Builder-based, wires `close-request`, buttons and shows the window; registers itself via `Variables.Window.FontSel = this;`).
- GTK3-only properties dropped for the font window: `border_width`, `window_position`, `type_hint`, and the inner font-selection `border_width`; the inner font chooser uses the GTK4 class `GtkFontChooserWidget` (the GTK4 replacement of the removed `GtkFontSelection`).
- Font window dialog layout uses the GTK4 `GtkDialog` buildable internal children `content_area` (font chooser) and `action_area` (button box), mirroring the GirCore `Builder` sample dialog pattern; buttons got translatable labels (`OK`, `Apply`, `Cancel`) since the source relied on action widgets without labels.
- Validation run: `dotnet build Bygfoot.Cli/Bygfoot.Cli.csproj` ends with **0 errors**; `font_sel.ui` validated as well-formed XML. No warnings/errors from the new files.

## Session status (2026-10-29, part 2)
- Converted next widget: `window_file_chooser` (`GtkFileChooserDialog`) → `Bygfoot.Cli/Windows/file_chooser.ui` (GTK4 builder file, `GtkFileChooserDialog`, GTK4 version 4.12) + `Bygfoot.Cli/Windows/FileChooserWindow.cs` (GirCore class deriving from `Gtk.Window`, Builder-based, wires the `button1` ("Cancel") and `button2` ("Open") buttons, registers via `Variables.Window.FileChooser = this;`, shows the window; no close-request signal since the source had no `delete_event`).
- GTK3-only properties dropped for the file chooser window: `border_width`, `window_position`, `type_hint`, `show_hidden`; the button box uses a plain `GtkBox` (horizontal) since the deprecated `GtkHButtonBox` no longer exists in GTK4; buttons use translatable labels (`Cancel`, `Open`) instead of `use_stock` labels (`gtk-cancel`, `gtk-open`).
- Validation run: `dotnet build Bygfoot.Cli/Bygfoot.Cli.csproj` ends with **0 errors**; `file_chooser.ui` validated as well-formed XML. No warnings/errors from the new files.

## Open questions / future work
- Switch to `[Gobject.Subclass<Gtk.Window>]` and/or embedded gresource loading (`glib-compile-resources`) later.
- Implement exact widget-level TODOs and full C→C# handler mappings for the startup flow.
- Wire the class into `Variables.Window` / the `Models.Windows` struct and the game logic. (Partially done: `StartupWindow` registers itself via `Variables.Window.Startup = this;` in its constructor.)
