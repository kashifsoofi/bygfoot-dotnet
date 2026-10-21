namespace Bygfoot.Cli
{
    using System;
    
    /// <summary>
    /// Global variables and constants, initialized upon application startup.
    /// Maps to the global structures initialized in main_init_variables in main.c.
    /// </summary>
    public static class Globals
    {
        // --- Application/Game State Flags & Statuses ---
        public static int CurrentDebugLevel { get; set; } = -1;
        public static int DebugOutputMode { get; set; } = -1;
        public static string DebugPrintMessage { get; private set; }
        public static bool IsDebuggingModeActive { get; set; } = false;

        // --- Data Collections (Simulating large game databases) ---
        public static readonly List<League> Leagues = new List<League>();
        public static readonly List<Cup> Cups = new List<Cup>();
        public static readonly List<User> Users = new List<User>();
        public static readonly List<Strategy> Strategies = new List<Strategy>();
        public static readonly List<LiveGame> LiveGames = new List<LiveGame>();
        public static readonly List<Transfer> TransferList = new List<Transfer>();

        // --- Match Simulation/Betting Data (Simulating `bets[]` array) ---
        public static readonly List<MatchBet> BetMatches = new List<MatchBet>();

        // --- Simulation Control Variables ---
        public static int SeasonNumber { get; set; } = 1;
        public static int WeekOfYear { get; set; } = 1;
        private const int CountEnd = 30; // Arbitrary limit matching array sizes

        // --- Application Options and Constants ---
        public static readonly List<string> AppConstantsList = new List<string>();
        public static readonly List<string> AppSettingsList = new List<string>();

        // --- Transient Data during a session ---
        public static League CurrentLeague { get; set; } // If context requires current active league

        // --- Initialization Methods (Conceptualization of main_init_variables) ---
        public static void InitializeApplication()
        {
            // This method conceptually runs main_init_variables from main.c, 
            // setting up pointers and allocating structures.
            Console.WriteLine("Globals initialized successfully.");
            // In a full conversion, this is where complex object graph initialization happens.
        }

        public static void ClearGlobals()
        {
            // Equivalent to free_memory() when exiting the game.
            Console.WriteLine("Global resources freed.");
        }
    }

    // --- Placeholder Structures (These would replace the included *.h files) ---
    public class League { /* ... properties based on league_struct.h */ }
    public class Cup { /* ... properties based on cup_struct.h */ }
    public class Strategy { /* ... properties based on strategy_struct.h */ }
    public class LiveGame { /* ... properties based on live_game_struct.h */ }
    public class Transfer { /* ... properties based on transfer_struct.h */ }
    public class MatchBet { /* ... properties based on bet_match.h */ }

}
