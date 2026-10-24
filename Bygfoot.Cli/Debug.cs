namespace Bygfoot.Cli;

using System;
using System.IO;
using System.Linq;
using Models;

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
        // Implementation depends on simulation logic.
        throw new NotImplementedException();
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
        // Implementation depends on logging mechanism.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Prints a formatted message to the console and/or a log file.
    /// Corresponds to C: debug_print_message(gchar *format, ...).
    /// </summary>
    public static void DebugPrintMessage(string format, params object?[] args)
    {
        string message = string.Format(format, args);

        if (Variables.DebugOutput != (int)DebugOutput.DebugOutLogfile)
        {
            Console.WriteLine(message);
        }

        if (Variables.DebugOutput != (int)DebugOutput.DebugOutStdout)
        {
            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string logFilePath = Path.Combine(home, "bygfoot.log");

            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss.fffK} {message}{Environment.NewLine}";
                File.AppendAllText(logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't open log file: {ex.Message}");
            }
        }
    }
}
