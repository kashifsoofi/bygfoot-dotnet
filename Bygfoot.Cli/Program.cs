using Serilog;

namespace Bygfoot.Cli;

/// <summary>
/// Main entry point for the Bygfoot Console application.
/// Delegates to Main.CMain, which maps to c_main() in src/main.c.
/// </summary>
public class Program
{
    /// <summary>
    /// The main execution entry point of the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    public static void Main(string[] args)
    {
        Debug.InitializeLogging();

        try
        {
            Log.Information("--- Bygfoot Application Starting ---");

            global::Bygfoot.Cli.Main.CMain(args);
        }
        catch (Exception ex)
        {
            // C: misc_print_error / main_exit_program error handling.
            Log.Fatal(ex, "An unexpected error occurred.");
        }
        finally
        {
            // Runs on the exception path; on the normal path the process has
            // already exited in Main.MainExitProgram (C: noreturn).
            Log.Information("--- Bygfoot Application Exited Cleanly ---");
            Log.CloseAndFlush();
        }
    }
}
