using System;
using System.Collections.Generic;
using Bygfoot.Options;

namespace Bygfoot.Model
{
	/** Indices for the money_in array. */
	public enum MonIn
	{
		MON_IN_PRIZE = 0,
		MON_IN_TICKET,
		MON_IN_SPONSOR,
		MON_IN_BETS,
		MON_IN_TRANSFERS,
		MON_IN_END
	}

	/** Indices for the money_out array. */
	public enum MonOut
	{
		MON_OUT_WAGE = 0,
		MON_OUT_PHYSIO,
		MON_OUT_SCOUT,
		MON_OUT_YC,
		MON_OUT_YA,
		MON_OUT_JOURNEY,
		MON_OUT_COMPENSATIONS,
		MON_OUT_BETS,
		MON_OUT_BOOST,
		MON_OUT_TRANSFERS,
		MON_OUT_STADIUM_IMPROVEMENT,
		MON_OUT_STADIUM_BILLS,
		MON_OUT_TRAINING_CAMP,
		MON_OUT_END
	}

	/** Indices for the counters variable in #User. */
	public enum CounterValue
	{
		COUNT_USER_LOAN = 0, /** How many weeks until user has to pay back his loan. */
		COUNT_USER_OVERDRAWN, /**< How often the user overdrew his bank account. */
		COUNT_USER_POSITIVE, /**< How many weeks until the bank account has to be positive
               or at least not overdrawn). */
		COUNT_USER_SUCCESS, /**< How successful the user is. */
		COUNT_USER_WARNING, /**< Whether there was already a warning about rumours (new coach). */
		COUNT_USER_INC_CAP, /**< How many weeks until the stadium capacity is increased. */
		COUNT_USER_INC_SAF, /**< How often the stadium safety was increased (in a week). */
		COUNT_USER_STADIUM_CAPACITY, /**< Counter for building stadium seats. */
		COUNT_USER_STADIUM_SAFETY, /**< Counter for increasing stadium safety. */
		COUNT_USER_SHOW_RES, /**< Whether the latest result is shown when the main window gets refreshed. */
		COUNT_USER_TOOK_TURN, /**< Whether the user took his turn in a week round. */
		COUNT_USER_NEW_SPONSOR, /**< A new sponsor offer has to be shown. */
		COUNT_USER_TRAININGS_WEEK, /**< Whether the user has already had a training camp this week. */
		COUNT_USER_TRAININGS_LEFT_SEASON, /**< How many training camps left for the season. */
		COUNT_USER_END
	}

	/** User-related things that get recorded. */
	public enum UserHistoryType
	{
		USER_HISTORY_START_GAME = 0,
		USER_HISTORY_FIRE_FINANCE,
		USER_HISTORY_FIRE_FAILURE,
		USER_HISTORY_JOB_OFFER_ACCEPTED,
		USER_HISTORY_END_SEASON,
		USER_HISTORY_PROMOTED,
		USER_HISTORY_RELEGATED,
		USER_HISTORY_WIN_FINAL,
		USER_HISTORY_LOSE_FINAL,
		USER_HISTORY_REACH_CUP_ROUND,
		USER_HISTORY_CHAMPION,
		USER_HISTORY_END
	}

	/** A memorable match (a recording of a live game) of a user. */
	public class MemMatch
	{
		/** Name of the country the user was playing with. */
		public string countryName;
		/** The name of the competition, including the cup round name. */
		public string competitionName;
		/** Whether the match was on neutral ground. Only relevant for MM list display. */
		public bool neutral;
		/** 0 or 1, indicating which team was the user's. */
		public int userTeam;
		/** The recording. */
		public LiveGame lg;
	}

	/** A structure holding an element of a user's history,
     * e.g. the event of being fired. */
	public class UserHistory
	{
		/** When the event happened. */
		public int season, week;

		/** The type (see #UserHistoryType) of the history event. */
		public int type;
		/** The team of the user at the time. */
		public string teamName;
		/** These can hold various information like
         * team or league/cup ids. */
		public string[] str = new string[3];
	}

	/** A user sponsor. */
	public class UserSponsor
	{
		public string Name { get; set; }
		/**< Money per week. */
		public int Benefit { get; set; }
		/**< Contract length in weeks. */
		public int Contract { get; set; }

		public UserSponsor()
		{
			this.Name = string.Empty;
			this.Benefit = -1;
			this.Contract = -1;
		}
	}

	/** A structure representing a human player. */
	public class User
	{
		/** Username. */
		public string Name { get; set; }
		/** The team the user manages. */
		public Team Team { get; set; }
		/** The team id (needed when the team pointer gets invalid). */
		public int TeamId { get; set; }
		/** User options. */
		public OptionsList Options { get; set; }
		/** Events shown each week. */
		public List<Event> Events { get; set; }
		/** User history. */
		public List<UserHistory> History { get; set; }
		/** User counters (not changeable by the user),
         * like number of weeks until debt has to be paid back. */
		private int[] _counters = new int[(int)CounterValue.COUNT_USER_END];
		public int[] Counters
		{
			get { return _counters; }
			set { _counters = value; }
		}
		/** The user's money, debt, income and expenses.
         * We have double arrays to store information about
         * the current and the past week. */
		public int Money { get; set; }
		public int Debt { get; set; }
		private int[,] _moneyIn = new int[2, (int)MonIn.MON_IN_END];
		public int[,] MoneyIn
		{
			get { return _moneyIn; }
			set { _moneyIn = value; }
		}
		private int[,] _moneyOut = new int[2, (int)MonOut.MON_OUT_END];
		public int[,] MoneyOut
		{
			get { return _moneyOut; }
			set { _moneyOut = value; }
		}
		/** Interest the debt was taken at. */
		public float DebtInterest { get; set; }
		/** Information about the automatic loan repayment. */
		public int AlrStartWeek { get; set; }
		public int AlrWeeklyInstallment { get; set; }
		/** The user's scout and physio qualities.
         * @see #Quality */
		public Quality Scout { get; set; }
		public Quality Physio { get; set; }
		/** The variable for the latest user live game. @see #Game */
		public LiveGame LiveGame { get; set; }
		/** Sponsor of the user. */
		public UserSponsor Sponsor { get; set; }
		/** Youth academy of the user. */
		public YouthAcademy YouthAcademy { get; set; }
		/** The currently used MM file. */
		public string MemMatchesFile { get; set; }
		/** The array of MMs. */
		public List<MemMatch> MemMatches { get; set; }
		/** Array of current and recent bets. */
		private List<BetUser>[] _bets = new List<BetUser>[2];
		public List<BetUser>[] Bets
		{
			get { return _bets; }
			set { _bets = value; }
		}
		// An array of gint that will be used to store the default team of a user
		public List<int> DefaultTeam { get; set; }
		// the default structure of a user team.
		public int DefaultStructure { get; set; }
		// the default playing style of a user team.
		public int DefaultStyle { get; set; }
		// the default boost of a user team.
		public int DefaultBoost { get; set; }

		public User()
		{
#if DEBUG
			Console.WriteLine("User.New");
#endif
			Name = "NONAME";
			Team = null;
			TeamId = -1;

			// TODO
			//live_game_reset(&new.live_game, NULL, FALSE);

			Events = new List<Event>();
			History = new List<UserHistory>();
			Options = new OptionsList();
			//TODO Options.list = null;
			//TODO Options.datalist = null;

			Sponsor = new UserSponsor();

			// TODO
			//new.youth_academy.players = g_array_new(FALSE, FALSE, sizeof(Player));
			//new.youth_academy.pos_pref = PLAYER_POS_ANY;
			//new.youth_academy.coach = QUALITY_AVERAGE;

			MemMatchesFile = null;
			MemMatches = new List<MemMatch>();

			Bets[0] = new List<BetUser>();
			Bets[1] = new List<BetUser>();
			DefaultTeam = new List<int>();
			DefaultStyle = 0;
			DefaultBoost = 0;
		}

		/** Move a user's team to top or bottom league
         * at the beginning of a new game and set up the team.
         * @param user The user we set up the team for. */
		public void SetupTeamNewGame()
		{
#if DEBUG
			Console.WriteLine("User.SetupTeamNewGame");
#endif
			// TODO
		}

		/** Set up finances, remove some players etc. for a new user team.
         * @par am user The user whose team we set up. */
		public void SetupTeam(bool removePlayers)
		{
#if DEBUG
			Console.WriteLine("User.SetupTeam");
#endif
			if (removePlayers)
			{
				for (PlayerPos position = PlayerPos.PLAYER_POS_DEFENDER; position <= PlayerPos.PLAYER_POS_FORWARD; position++)
				{
					for (int j = Team.Players.Count - 1; j > 10; j--)
					{
						if (Team.Players[j].Position == position && Team.Players[j].Recovery != -1)
						{
							// TODO
							//player_remove_from_team(user->tm, j);
							break;
						}
					}
				}
			}

			for (int i = 0; i < Team.Players.Count; i++)
				Team.Players[i].Recovery = 0;

			Scout = Quality.QUALITY_AVERAGE;
			Physio = Quality.QUALITY_AVERAGE;

			//TODO if (Option.SettingInt("int_opt_disable_transfers") != 0)
			Physio = Quality.QUALITY_GOOD;

			Team.Style = 0;

			SetupFinances();
			SetupCounters();

			//TODO if (Option.SettingInt("int_opt_disable_ya") == 0)
			YouthAcademy = new YouthAcademy(this);

			//TODO Counters[(int)CounterValue.COUNT_USER_NEW_SPONSOR] = Option.SettingInt("int_opt_disable_finances") == 1 ? -5 : 1;

			//TODO Option.OptionInt("int_opt_user_penalty_shooter", Options, -1);
		}

		/** Set the counters of the user to their initial values. */
		public void SetupCounters()
		{
#if DEBUG
			Console.WriteLine("User.SetupCounters");
#endif
			for (int i = 0; i < (int)CounterValue.COUNT_USER_END; i++)
				Counters[i] = 0;

			Counters[(int)CounterValue.COUNT_USER_LOAN] = -1;
			Counters[(int)CounterValue.COUNT_USER_POSITIVE] = -1;
			//TODO Counters[(int)CounterValue.COUNT_USER_TRAININGS_LEFT_SEASON] = Option.ConstInt("int_training_camps_per_season");
		}

		/** Set up the user's finances when he's got a new team.
         * @param user The user we set up the finances for. */
		public void SetupFinances()
		{
#if DEBUG
			Console.WriteLine("User.SetupFinances");
#endif
			for (int i = 0; i < (int)MonOut.MON_OUT_END; i++)
			{
				MoneyOut[0, i] = 0;
				MoneyOut[1, i] = 0;
			}

			for (int i = 0; i < (int)MonIn.MON_IN_END; i++)
			{
				MoneyIn[0, i] = 0;
				MoneyIn[1, i] = 0;
			}

			Debt = 0;
			DebtInterest = 0;
			AlrStartWeek = 0;
			AlrWeeklyInstallment = 0;
			// TODO
			//math_round_integer(user->tm->stadium.capacity * 
			//                   math_rndi(const_int("int_initial_money_lower"),
			//          const_int("int_initial_money_upper")), 2);
		}

		/** Remove a user from the game.
         * @param idx The index of the user in the #users array.
         * @param regenerate_team Whether the user's team has to be
         * regenerated. */
		public static void Remove(int index, bool regenerateTeam)
		{
#if DEBUG
			Console.WriteLine("User.Remove");
#endif
			/*
						gint i;

						if(regenerate_team)
						{
							for(i=0;i<usr(idx).tm->players->len;i++)
								free_player(&g_array_index(usr(idx).tm->players, Player, i));

							g_array_free(usr(idx).tm->players, TRUE);
							usr(idx).tm->players = g_array_new(FALSE, FALSE, sizeof(Player));

							usr(idx).tm->luck = 1;

							team_generate_players_stadium(usr(idx).tm, 0);
							for(i=0;i<usr(idx).tm->players->len;i++)
								g_array_index(usr(idx).tm->players, Player, i).team = usr(idx).tm;
						}

						free_user(&usr(idx));
						g_array_remove_index(users, idx);

						cur_user = 0;

						if(window.main != NULL)
							game_gui_show_main();*/
		}
	}

	public enum EventType
	{
		EVENT_TYPE_WARNING = 0,
		EVENT_TYPE_PLAYER_LEFT,
		EVENT_TYPE_PAYBACK,
		EVENT_TYPE_OVERDRAW,
		EVENT_TYPE_JOB_OFFER,
		EVENT_TYPE_FIRE_FINANCE,
		EVENT_TYPE_FIRE_FAILURE,
		EVENT_TYPE_TRANSFER_OFFER_USER,
		EVENT_TYPE_TRANSFER_OFFER_CPU,
		EVENT_TYPE_TRANSFER_OFFER_REJECTED_BETTER_OFFER,
		EVENT_TYPE_TRANSFER_OFFER_REJECTED_FEE_WAGE,
		EVENT_TYPE_TRANSFER_OFFER_REJECTED_FEE,
		EVENT_TYPE_TRANSFER_OFFER_REJECTED_WAGE,
		EVENT_TYPE_TRANSFER_OFFER_REJECTED_STARS,
		EVENT_TYPE_TRANSFER_OFFER_MONEY,
		EVENT_TYPE_TRANSFER_OFFER_ROSTER,
		EVENT_TYPE_PLAYER_CAREER_STOP,
		EVENT_TYPE_CHARITY,
		EVENT_TYPE_END
	}

	/** A structure representing an event for a user. This is used
     * to show information like a successful transfer or a job offer. */
	public class Event
	{
		/** Pointer to the user the event belongs to. */
		public User user;
		/** Type of the event. See #EventType. */
		public EventType type;
		/** Some values that are used for different purposes. */
		public int value1, value2;
		/** A pointer for different purposes. */
		public object Value;
		/** A string for different purposes. */
		public string valueString;
	}
}
