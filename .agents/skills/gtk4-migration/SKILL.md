---
name: gtk4-migration
description: Convert GTK3 Glade widget/window files and their corresponding C# classes to GTK4 composite templates and GTK4 widgets. Use when the user asks to migrate or convert a GTK3 UI file and its C# code to GTK4.
---

# GTK4 Migration Skill

Use this skill when the user wants to migrate a project from GTK3 to GTK4, specifically regarding UI files and their C# bindings.

## Workflow

1.  **Analyze the `.ui` file (formerly `.glade`)**:
    -   Look for `<packing>` elements and remove them.
    -   Look for `shadow-type` properties in `GtkScrolledWindow` and remove them.
    -   Replace `GtkTreeView` with `GtkListView`. Note: This is a structural change that requires a `GtkSelectionModel` and a `GtkListItemFactory`. If you are only doing a surface-level conversion, replace `GtkTreeView` with `GtkListView` and don't change the ID.
    -   Update `GtkSeparator` and other widgets to be GTK4 compliant.

2.  **Analyze the C# Class**:
    -   Convert the class to a composite template using `[GObject.Subclass<T>(qualifiedName: nameof(ClassName))]` and `[Gtk.Template<Gtk.AssemblyResource>("filename.ui")]`.
    -   The class should be `public partial class ClassName`.
    -   The constructor should be removed or refactored to use `partial void Initialize()`.
    -   Update widget bindings from `_builder.GetObject("id")` to `[Gtk.Connect("id")]` or `[Gtk.Child("id")]` depending on the pattern used in the project.
    -   Ensure signals and properties are correctly mapped to GTK4.
    -   Add private property for all fields with `[Gtk.Connect]` attribute
    -   Connect to handlers in Initialize method

3.  **Apply Changes**:
    -   Perform the edits in the `.ui` file.
    -   Perform the edits in the `.cs` file.
