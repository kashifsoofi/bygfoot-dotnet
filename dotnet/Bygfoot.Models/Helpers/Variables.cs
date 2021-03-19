using System;
using System.Collections;
using System.Collections.Generic;
using Bygfoot.Model;
using Bygfoot.Options;

namespace Bygfoot.Helpers
{
	public static class Variables
	{
		/**
         * The main variable of the game.
         * @see Country
         */
		public static Country Country = new Country();
		/** The array of human players. @see #User */
		public static List<User> Users = new List<User>();
		/** The season, week and week round numbers.
         * We keep track of the time in the game with these variables. */
		public static int Season;
		public static int Week;
		public static int WeekRound;
		/** Array of options that get read from
         * bygfoot.conf. */
		public static OptionsList Options = new OptionsList();
		/** Array of constants that get read from the constants
         * file specified in bygfoot.conf. */
		public static OptionsList Constants = new OptionsList();
		/** Array of constants affecting game appearance rather than
         * behaviour. */
		public static OptionsList ConstantsApp = new OptionsList();
		/** Array with internal settings. */
		public static OptionsList Settings = new OptionsList();
		/* Array holding string replacement tokens. */
		public static OptionsList Tokens = new OptionsList();

		/** The array containing the live game commentary strings. */
		public static ArrayList Commentary = new ArrayList(Convert.ToInt32(LiveGameEventType.LIVE_GAME_EVENT_END));
		/** The array containing the news article strings. */
		public static ArrayList News = new ArrayList(Convert.ToInt32(NewsArticleTypes.NEWS_ARTICLE_TYPE_END));
		/** Newspaper containing the news articles. */
		public static NewsPaper Newspaper;

		/** The array containing players to be transfered.
         * @see TransferPlayer */
		public static List<Transfer> TransferList;

		/** Array with season statistics (updated at the
         * end of each season. */
		public static List<SeasonStat> SeasonStats;

		/** Array of available CPU strategies. */
		public static List<Strategy> Strategies;

		/** Array of current and recent bets. */
		public static List<BetMatch>[] Bets = new List<BetMatch>[2];

		/** Loan interest for the current week. */
		public static double current_interest;

		/** Array of jobs in the job exchange and
         * teams going with the international jobs. */
		public static List<Job> Jobs;
		public static List<Team> JobTeams;

		/** Some counters we use. */
		public static int[] Counters = new int[Convert.ToInt32(CounterType.COUNT_END)];

		/** These help us keep track of what's happening. */
		//public static StatusValue[] status = new StatusValue[6];

		/** A pointer we store temporary stuff in. */
		//TODO: gpointer statp;

		/** The currently selected row in the treeview. */
		public static int selected_row;

		/** An array of name lists. */
		public static List<NameList> NameLists;

		/** The struct containing the window pointers. */
		//public static Windows Window = new Windows();

		/** The variables for non-user live games (which aren't shown). */
		public static List<LiveGame> LiveGames;

		/** The index of the current user in the #users array. */
		public static int cur_user;

		public static int timeout_id;

		public static Random RandGenerator;

		/** Debug information. */
		public static int debug_level;
		//public static DebugOutput debugOutput;

		/**
         * The list of directories the file_find_support_file() function
         * searches for support files (e.g. pixmaps or text files).
         * @see file_find_support_file()
         * @see file_add_support_directory_recursive()
         **/
		public static List<string> SupportDirectories;

		/**
         * The list of root defintions directories found (ending in definitions)
         **/
		public static List<string> root_definitions_directories = new List<string>();

		/**
         * The list of defintions directories found
         **/
		public static List<string> definitions_directories = new List<string>();

		/** The name of the current save file (gets updated when a game is
         * saved or loaded).  */
		public static string SaveFile;

		/** Whether we are using a Unix system or Windows. */
		public static bool os_is_unix;

		/** The hints displayed in the splash screen. */
		public static OptionsList hints = new OptionsList();


		public static string LeagueCupGetName(int id)
		{
			//return id < Bygfoot.ID_CUP_START ? LeagueFromId (id).name : CupFromId (id).name;
			return "";
		}

		/** Return the league pointer belonging to the id.
         * @param clid The id we look for.
         * @return The league pointer or NULL if failed. */
		public static League LeagueFromId(int id)
		{
#if DEBUG
			Console.WriteLine("Variables.LeagueFromId");
#endif
			foreach (League league in Variables.Country.Leagues)
			{
				if (league.id == id)
					return league;
			}

			//Program.ExitProgram(ExitCodes.EXIT_POINTER_NOT_FOUND, "Variables.LeagueFromId: didn't find league with id {0}\n", id);
			return null;
		}

		/** Return the cup pointer belonging to the id.
         * @param clid The id we look for.
         * @return The cup pointer or NULL if failed. */
		public static Cup CupFromId(int id)
		{
#if DEBUG
			Console.WriteLine("Variables.CupFromId");
#endif
			foreach (Cup cup in Variables.Country.Cups)
			{
				if (cup.id == id)
					return cup;
			}

			//Program.ExitProgram(ExitCodes.EXIT_POINTER_NOT_FOUND, "Variables.CupFromId: didn't find cup with id {0}\n", id);
			return null;
		}
	}
}
