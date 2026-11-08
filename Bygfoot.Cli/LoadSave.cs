namespace Bygfoot.Cli;

/// <summary>
/// Saving and loading of the game.
/// Corresponds to the C functions in src/load_save.h and src/load_save.c.
/// </summary>
public static class LoadSave
{
    /// <summary>Number of progress steps when saving/loading. Corresponds to C: PROGRESS_MAX.</summary>
    private const int ProgressMax = 9;

    /// <summary>
    /// Save the game to the specified file.
    /// Corresponds to C: load_save_save_game(const gchar *filename).
    /// </summary>
    /// <param name="filename">The save file name (the save suffix is appended if it is missing).</param>
    public static void LoadSaveSaveGame(string filename)
    {
        string suffix = OptionHelper.ConstStr("string_fs_save_suffix");
        string prefix = filename.EndsWith(suffix) ? filename.Substring(0, filename.Length - suffix.Length) : filename;
        string fullname = filename.EndsWith(suffix) ? filename : filename + suffix;

        Variables.SaveFile = fullname;

        if (File.Exists(fullname))
            File.Delete(fullname);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save options\n");

        // TODO: Call the C function gui_show_progress(0, "Saving options...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        FileHelper.SaveOptFile(prefix + "___options", Variables.Options);
        FileHelper.SaveOptFile(prefix + "___settings", Variables.Settings);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save leagues/cups \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving leagues and cups...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        LeaguesCupsXmlLoadSave.LeaguesCupsXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save users \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving users...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        UsersXmlLoadSave.UsersXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save transfers \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving transfer list...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        TransfersXmlLoadSave.TransfersXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save stats \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving season stats...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        SeasonStatsXmlLoadSave.SeasonStatsXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save jobs \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving job exchange...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        JobsXmlLoadSave.JobsXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save newspaper \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving newspaper...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        NewspaperXmlLoadSave.NewspaperXmlLoadSaveWrite(prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save misc \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Saving miscellaneous...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        MiscXmlLoadSave.MiscXmlLoadSaveWrite(prefix);

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Compressing savegame...", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        FileHelper.CompressFiles(fullname, prefix + "___");

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_save done \n");

        // TODO: Call the C function gui_show_progress(1, "Done.", PIC_TYPE_SAVE) (src/gui.c is not yet converted).

        FileHelper.StoreTextInSaves("last_save", fullname);

        // TODO: Call the C function gui_show_progress(-1, "", PIC_TYPE_SAVE) (src/gui.c is not yet converted).
        // TODO: Apply the setsav1 macro (disables the save button and save menu of the main window; src/game_gui.h is not yet converted).
    }

    /// <summary>
    /// Load the game from the specified file.
    /// Corresponds to C: load_save_load_game(const gchar* filename, gboolean create_main_window).
    /// </summary>
    /// <param name="filename">The save file name (or "last_save" for the last save).</param>
    /// <param name="createMainWindow">Whether to create and show the main window.</param>
    /// <returns>Whether the game could be loaded.</returns>
    public static bool LoadSaveLoadGame(string filename, bool createMainWindow)
    {
        string suffix = OptionHelper.ConstStr("string_fs_save_suffix");
        string fullname = filename.EndsWith(suffix) ? filename : filename + suffix;
        string basename = Path.GetFileName(fullname);
        string dirname = Path.GetDirectoryName(fullname) ?? string.Empty;
        string prefix = basename.EndsWith(suffix) ? basename.Substring(0, basename.Length - suffix.Length) : basename;
        string pwd = Directory.GetCurrentDirectory();

        if (filename.EndsWith("last_save"))
        {
            basename = FileHelper.LoadTextFromSaves("last_save");

            if (basename != null)
            {
                LoadSaveLoadGame(basename, createMainWindow);
                return true;
            }
            else
            {
                // TODO: Call the C function game_gui_show_warning("Last save file not found.") (src/game_gui.c is not yet converted).
                return false;
            }
        }

        // TODO: Hide the main window if it is shown (window.main != NULL -> gtk_widget_hide(window.main)); (src/window.c is not yet converted).

        // TODO: Call the C function gui_show_progress(0, "Uncompressing savegame...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        FileHelper.Decompress(fullname);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load options\n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading options...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        FileHelper.LoadOptFile(Path.Combine(dirname, prefix + "___options"), Variables.Options, false);
        FileHelper.LoadOptFile(Path.Combine(dirname, prefix + "___settings"), Variables.Settings, false);
        Language.LanguageSet(Language.LanguageGetCodeIndex(OptionHelper.OptStr("string_opt_language_code")) + 1);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load leagues \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading leagues and cups...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        LeaguesCupsXmlLoadSave.LeaguesCupsXmlLoadSaveRead(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load users \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading users...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        Xml.XmlLoadUsers(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load transfers \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading transfer list...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        Xml.XmlLoadTransfers(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load stats \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading season stats...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        SeasonStatsXmlLoadSave.SeasonStatsXmlLoadSaveRead(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load jobs \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading job exchange...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        JobsXmlLoadSave.JobsXmlLoadSaveRead(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load newspaper \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading newspaper...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        NewspaperXmlLoadSave.NewspaperXmlLoadSaveRead(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load misc \n");

        // TODO: Call the C function gui_show_progress(((PROGRESS_MAX * gui_get_progress_bar_fraction()) + 1) / PROGRESS_MAX, "Loading miscellaneous...", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        MiscXmlLoadSave.MiscXmlLoadSaveRead(dirname, prefix);

        if (Variables.DebugLevel > 60)
            Debug.DebugPrintMessage("load_save_load done \n");

        // TODO: Call the C function gui_show_progress(1, "Done.", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        Directory.SetCurrentDirectory(dirname);
        List<string> files = FileHelper.GetDirectoryContents(dirname, prefix, "");
        // Remove the zipfile from the list
        for (int i = files.Count - 1; i >= 0; i--)
        {
            if (Path.GetFileName(files[i]) == basename)
            {
                files.RemoveAt(i);
            }
        }
        FileHelper.RemoveFiles(files);
        Directory.SetCurrentDirectory(pwd);

        Variables.SaveFile = fullname;

        FileHelper.StoreTextInSaves("last_save", fullname);

        // TODO: Call the C function gui_show_progress(-1, "", PIC_TYPE_LOAD) (src/gui.c is not yet converted).

        if (createMainWindow)
        {
            // TODO: Call the C functions window_create(WINDOW_MAIN) and on_button_back_to_main_clicked(NULL, NULL) (src/window.c and src/callbacks.c are not yet converted).
        }
        else if (Variables.Window.Main != null)
        {
            // TODO: Show the main window (gtk_widget_show(window.main)) and call the C function window_main_load_geometry (src/window.c is not yet converted).
        }

        return true;
    }

    /// <summary>
    /// Write an autosave.
    /// Corresponds to C: load_save_autosave(void).
    /// </summary>
    public static void LoadSaveAutosave()
    {
        if (OptionHelper.OptInt("int_opt_autosave") == 0)
            return;

        Variables.Counters[(int)Counters.CountAutosave] =
            (Variables.Counters[(int)Counters.CountAutosave] + 1) % OptionHelper.OptInt("int_opt_autosave_interval");

        if (Variables.Counters[(int)Counters.CountAutosave] != 0)
            return;

        string name = LoadSaveWriteAutosaveName();

        string directory;
        if (Variables.OsIsUnix)
            directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".bygfoot", "saves");
        else
            directory = Path.Combine(Directory.GetCurrentDirectory(), "saves");

        name += ".zip";
        string buf = Path.Combine(directory, name);

        if (!FileHelper.MyFopen(buf, "w", out Stream fil, false))
            return;
        fil.Dispose();

        string prefix = string.Format("autosave_{0:00}_", Variables.Counters[(int)Counters.CountAutosaveFile]);
        LoadSaveSaveGame(buf);

        string pwd = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(directory);
        List<string> files = FileHelper.GetDirectoryContents(directory, prefix, ".zip");
        // Remove the zipfile from the list
        for (int i = files.Count - 1; i >= 0; i--)
        {
            if (Path.GetFileName(files[i]) == name)
            {
                files.RemoveAt(i);
            }
        }
        FileHelper.RemoveFiles(files);
        Directory.SetCurrentDirectory(pwd);

        Variables.Counters[(int)Counters.CountAutosaveFile] =
            (Variables.Counters[(int)Counters.CountAutosaveFile] + 1) % OptionHelper.OptInt("int_opt_autosave_files");
    }

    /// <summary>
    /// Write the autosave file name which is put together from
    /// the user name, team name etc.
    /// Corresponds to C: load_save_write_autosave_name(gchar *filename).
    /// </summary>
    /// <returns>The autosave file name (without extension).</returns>
    public static string LoadSaveWriteAutosaveName()
    {
        User user = Variables.Users[0];

        string teamname = "";
        foreach (char c in user.Team.Name)
            teamname += !char.IsWhiteSpace(c) ? c : '_';

        return string.Format("autosave_{0:00}_{1}_{2}_{3}_S{4:00}_W{5:00}",
                             Variables.Counters[(int)Counters.CountAutosaveFile],
                             user.Name,
                             Variables.Country.Name,
                             teamname,
                             Variables.Season,
                             Variables.Week);
    }

    /// <summary>
    /// Try to load a savegame given on the command line.
    /// Corresponds to C: load_game_from_command_line(const gchar *filename).
    /// </summary>
    /// <param name="filename">The save file name (or "last_save" for the last save).</param>
    /// <returns>Whether the savegame could be loaded.</returns>
    public static bool LoadGameFromCommandLine(string filename)
    {
        if (filename == "last_save")
            return LoadSaveLoadGame(filename, true);

        string suffix = OptionHelper.ConstStr("string_fs_save_suffix");
        string fullname = filename.EndsWith(suffix) ? filename : filename + suffix;

        if (File.Exists(fullname))
        {
            return LoadSaveLoadGame(fullname, true);
        }

        string supportFileName = FileHelper.FindSupportFile(fullname, false);

        if (supportFileName != null && File.Exists(supportFileName))
        {
            return LoadSaveLoadGame(supportFileName, true);
        }

        Debug.DebugPrintMessage($"Could not find file {fullname}.");

        return false;
    }
}
