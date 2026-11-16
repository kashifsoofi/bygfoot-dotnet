namespace Bygfoot.Cli;

using System;
using System.IO;
using System.Linq;
using Serilog;
using Serilog.Core;
using Serilog.Events;

/// <summary>
/// Debug output options. Corresponds to C enum DebugOutput.
/// </summary>
public enum DebugOutput
{
    DebugOutStdout = 0,
    DebugOutLogfile,
    DebugOutStdoutLogfile
}

/// <summary>
/// Debugging utilities. Corresponds to C functions in src/debug.h and src/debug.c.
/// </summary>
public static class Debug
{
    /// <summary>
    /// The level switch controlling the global Serilog logger's minimum level.
    /// Kept in sync with <see cref="Variables.DebugLevel"/>.
    /// </summary>
    private static readonly LoggingLevelSwitch LevelSwitch = new(LogEventLevel.Information);

    /// <summary>
    /// Configures the global Serilog logger with a console sink (stdout) and/or a
    /// file sink appending to $HOME/.bygfoot/bygfoot.log, depending on the current
    /// <see cref="Variables.DebugOutput"/> mode. Corresponds to the output behavior
    /// of C's debug_print_message (src/debug.c): stdout via g_warning and an
    /// ISO8601-timestamped append to the log file.
    /// </summary>
    public static void InitializeLogging()
    {
        var configuration = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(LevelSwitch);

        if (Variables.DebugOutput != (int)DebugOutput.DebugOutLogfile)
            configuration = configuration.WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

        if (Variables.DebugOutput != (int)DebugOutput.DebugOutStdout)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string logDir = Path.Combine(home, Bygfoot.HomeDirName);
            Directory.CreateDirectory(logDir);
            configuration = configuration.WriteTo.File(
                Path.Combine(logDir, "bygfoot.log"),
                outputTemplate: "{Timestamp:o} {Message:lj}{NewLine}{Exception}");
        }

        Log.Logger = configuration.CreateLogger();
        SyncMinimumLevel();
    }

    /// <summary>
    /// Synchronizes the global logger's minimum level with <see cref="Variables.DebugLevel"/>:
    /// 0-1 = Information, 2 = Debug, 3 or more = Verbose.
    /// </summary>
    public static void SyncMinimumLevel()
    {
        LevelSwitch.MinimumLevel = Variables.DebugLevel switch
        {
            <= 1 => LogEventLevel.Information,
            2 => LogEventLevel.Debug,
            _ => LogEventLevel.Verbose
        };
    }
    /// <summary>
    /// Take some debug action depending on the text. Text is a prefix and a number.
    /// Corresponds to C: debug_action(const gchar *text).
    /// </summary>
    /// <param name="text">The debug action text.</param>
    public static void DebugAction(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        // In C: sscanf(text, "%[^-0-9]%d", buf, &value);
        // We'll look for the first digit or minus sign.
        int firstDigitIndex = -1;
        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsDigit(text[i]) || text[i] == '-')
            {
                firstDigitIndex = i;
                break;
            }
        }

        if (firstDigitIndex == -1)
            return;

        string prefix = text.Substring(0, firstDigitIndex);
        string valueStr = text.Substring(firstDigitIndex);

        if (!int.TryParse(valueStr, out int value))
            return;

        if (prefix == "deb")
        {
            Variables.DebugLevel = value;
            SyncMinimumLevel();
            DebugPrintMessage($"Debug value set to {value}.");
        }
        // Other actions require more implementation/dependencies.
        else
        {
            DebugPrintMessage($"Debug action '{prefix}' with value {value} is not yet fully implemented.");
        }
    }

    /// <summary>
    /// Resets a counter. Corresponds to C: debug_reset_counter(gpointer data).
    /// </summary>
    /// <param name="data">The index of the counter to reset.</param>
    public static bool DebugResetCounter(object? data)
    {
        if (data is int index && index >= 0 && index < Variables.Counters.Length)
        {
            Variables.Counters[index] = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Calibrates the betting odds. Corresponds to C: debug_calibrate_betting_odds(gint skilldiffmax, gint matches_per_skilldiff).
    /// </summary>
    public static void DebugCalibrateBettingOdds(int skilldiffmax, int matches_per_skilldiff)
    {
        int i, skilldiff, matches;
        Fixture fix = Bygfoot.GetLeague(0).Fixtures[0];
        LiveGame liveGame = new LiveGame();

        fix.HomeAdvantage = false;

        for (skilldiff = 0; skilldiff <= skilldiffmax; skilldiff++)
        {
            int[] res = new int[3];

            for (matches = 0; matches < matches_per_skilldiff; matches++)
            {
                fix.Attendance = -1;
                fix.Result[0, 0] = fix.Result[1, 0] = 0;
                for (i = 0; i < fix.Teams[0].Players.Count; i++)
                {
                    Strategy.RepairPlayer(fix.Teams[0].Players[i]);
                    Strategy.RepairPlayer(fix.Teams[1].Players[i]);
                    fix.Teams[0].Players[i].Skill = 90;
                    fix.Teams[1].Players[i].Skill = 90 - skilldiff;
                    fix.Teams[0].Players[i].Fitness = 0.9f;
                    fix.Teams[1].Players[i].Fitness = 0.9f;
                }

                LiveGame.CalculateFixture(fix, liveGame);
                if (fix.Result[0, 0] < fix.Result[1, 0])
                    res[2]++;
                else
                    res[fix.Result[0, 0] == fix.Result[1, 0] ? 1 : 0]++;
            }

            // C: g_print(...) — routed through the file's Serilog logger.
            DebugPrintMessage("sd {0,3} res {1,3} {2,3} {3,3} prob {4:0.00} {5:0.00} {6:0.00}",
                skilldiff, res[0], res[1], res[2],
                (double)res[0] / matches, (double)res[1] / matches, (double)res[2] / matches);
        }
    }

    /// <summary>
    /// Check whether the 4 forwards, boost on, style all-out-attack
    /// easter egg should be activated. Corresponds to C: debug_egg_forwards_boost_style(void).
    /// </summary>
    public static bool DebugEggForwardsBoostStyle()
    {
        var user = Variables.Users.ElementAtOrDefault(Variables.CurUser);
        if (user == null || user.Team == null || user.Team.Boost != 1 || user.Team.Style != 2 || user.Team.Players == null || user.Team.Players.Count < 11)
            return false;

        int fwds = 0;
        for (int i = 0; i < 11; i++)
        {
            if (user.Team.Players[i].Cpos == PlayerPos.Forward && user.Team.Players[i].Cskill > 0)
                fwds++;
        }

        return fwds > 3;
    }

    /// <summary>
    /// Writes debug information to a file. Corresponds to C: debug_writer_out(const gchar *file_name, const gchar *method_name, const gchar *text, gint debuglevel).
    /// </summary>
    public static void DebugWriterOut(string fileName, string methodName, string text, int debugLevel)
    {
        // C only declares debug_writer_out (src/debug.h); it is never defined or
        // called in the C source, so there is no C body to port. Provide the
        // documented behavior: append the text to the file when a debug level is set.
        if (debugLevel <= 0)
            return;

        try
        {
            File.AppendAllText(fileName, $"{methodName}: {text}{Environment.NewLine}");
        }
        catch
        {
            // Ignore I/O errors: this is best-effort debug output.
        }
    }

    /// <summary>
    /// Prints a formatted message to the console and/or a log file, depending on the
    /// current <see cref="Variables.DebugOutput"/> mode (see <see cref="InitializeLogging"/>).
    /// Corresponds to C: debug_print_message(gchar *format, ...), which writes to stdout
    /// via g_warning and appends to the log file; the C# equivalent routes the message
    /// through the global Serilog logger at Warning level.
    /// </summary>
    public static void DebugPrintMessage(string format, params object?[] args)
    {
        Log.Warning(string.Format(format, args));
    }
}
