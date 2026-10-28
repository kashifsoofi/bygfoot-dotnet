---
name: gtk4-migration
description: Workflow for converting a Bygfoot GTK2 Glade widget (from support_files/*.glade) into a GTK4 builder .ui file plus a GirCore C# window class in the Bygfoot.Cli project. Use when the user asks to extract/convert a widget (e.g. window_startup) from a bygfoot_*.glade support file, convert it to GTK4 builder format, and create a GirCore class under the Windows directory.
---

# Bygfoot GTK2 Glade → GTK4 Builder + GirCore Migration Skill

## Overview
The Bygfoot codebase still contains GTK2-era Glade files (`support_files/bygfoot_misc.glade`, etc.) and a C# port (`Bygfoot.Cli`) that is being migrated to GTK4 via GirCore. This skill captures the repeatable workflow for converting one widget at a time.

## Step 1 – Extract the widget
- Start from the widget subtree in the relevant `support_files/bygfoot_*.glade` file (e.g. `support_files/bygfoot_misc.glade`).
- Extract the top-level `<object class="GtkWindow" id="window_x">` … matching `</object>` (e.g. `window_startup` spans lines 5–542 in `bygfoot_misc.glade`).

## Step 2 – Convert to GTK4 builder format
- Root: `<?xml version='1.0' encoding='UTF-8'?>` + `<interface>` with `<requires lib="gtk" version="4.12"/>`.
- Widget: `<object class="..." id="...">`.
- Properties: kebab-case names (`default-width`, `default-height`, `modal`, `can-focus`, `tooltip-text`, `hscrollbar-policy`, `max-length`, `width-chars`, `use-underline`, `draw-indicator`, `group`, `spacing`, `orientation`, `height-request`, `width-request`, `label`, `justify`, …). Booleans are written as `0`/`1` (valid). Enums by name. Floats/ints as numbers.
- Signals: `<signal name="..." handler="..."/>`.
- Packing: no `<packing>` — just ordered `<child>` elements for containers (Box, Paned, ScrolledWindow, …).
- Mapping decisions made for Bygfoot conversions:
  - GTK3 `delete_event` → GTK4 `close-request` (handler returns `bool`).
  - Drop GTK3-only props: `border_width`, `window_position`, `ypad`, packing `expand/fill`, `use_stock`, `<accelerator>` (not supported in GTK4 builder format).
  - `GtkVBox`/`GtkHBox` → `GtkBox` with `orientation` (vertical=1, horizontal=0).
  - `GtkHSeparator`/`GtkVSeparator` → `GtkSeparator` with `orientation`.
  - Buttons: replace `GtkAlignment > child` hierarchy with `label` + `icon-name` on the button.
  - `has_focus` → `focusable`.
- Validate the resulting `.ui` is well-formed XML (e.g. via an XML parser).

## Step 3 – Create the GirCore class
- Location: a folder named `Windows` under the project (e.g. `Bygfoot.Cli/Windows/`), namespace `Bygfoot.Cli.Windows`.
- Class: `public class <X>Window : Gtk.Window` in namespace `Bygfoot.Cli.Windows`.
- Pattern that compiles against **GirCore.Gtk-4.0 0.8.1** (no `[Gtk.Template]` yet — use Builder):
  - `var builder = Gtk.Builder.NewFromFile("<name>.ui");`
  - `var window = (Gtk.Window)builder.GetObject("window_startup") ?? throw new InvalidOperationException(...);`
  - `window.Modal = true; window.DefaultWidth = 550; window.DefaultHeight = 600;`
  - Wire: `window.OnCloseRequest += OnWindowCloseRequest;` (handler `bool OnWindowCloseRequest(Window sender, EventArgs args)`), buttons via `button.OnClicked += ...`, `combo.OnChanged += ...`, `entry.OnActivate += ...`; finally `window.Show();`.
- Events deferred to later: game-logic handlers (`TODO` comments referencing C handlers like `on_team_selection_cancel_clicked`, `misc_callback_*`, `main_exit_program`).

## Step 4 – Project wiring pitfalls (already discovered)
- Add to `Bygfoot.Cli.csproj`: `<PackageReference Include="GirCore.Gtk-4.0" />` (version-less — central package management) and `<Content Include="Windows\*.ui" CopyToOutputDirectory="PreserveNewest" />`.
- Creating a folder named `Windows` creates namespace `Bygfoot.Cli.Windows`, which collides with the existing type `Bygfoot.Cli.Models.Windows` used in `Variables.cs` → fix references to `Models.Windows`.

## Step 5 – Validation
- `dotnet build Bygfoot.Cli/Bygfoot.Cli.csproj` must end with **0 errors**.

## References
- Conversion decision log and open questions: [references/CONVERSION_DECISIONS.md](references/CONVERSION_DECISIONS.md)

## Decisions log (user-approved)
- Builder-based GirCore class is acceptable (GirCore 0.6.3 lacks `[Gtk.Template]`).
- `.ui` file name convention: strip `window_` prefix (e.g. `window_startup` → `startup.ui`).
- GTK `<requires lib="gtk" version="4.12"/>`.
- Close handler returns `true`.
- Game-logic wiring deferred (TODOs).
- Tree interactions to use GTK4 event controllers later.
- Migrate deprecated widgets (ComboBox/TreeView/RadioButton) to DropDown / ListView+ColumnView later.
- Keep file-based `.ui` loading for now.

## Open questions / future work
- Whether/when to switch to `[Gobject.Subclass<Gtk.Window>]` and embedded gresource loading.
- Exact widget-level TODOs and C→C# handler mappings for the startup flow.
