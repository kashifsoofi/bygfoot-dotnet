using System;
using System.Collections.Generic;
using System.Xml;
using Bygfoot.Helpers;

namespace Bygfoot.Model
{
	/** Possibilities for team comparison. */
	public enum TeamCompare
	{
		TEAM_COMPARE_LEAGUE_RANK = 0,
		TEAM_COMPARE_LEAGUE_LAYER,
		TEAM_COMPARE_UNSORTED,
		TEAM_COMPARE_AV_SKILL,
		TEAM_COMPARE_OFFENSIVE,
		TEAM_COMPARE_DEFENSE,
		TEAM_COMPARE_END
	}

	/** @see team_return_league_cup_value_int() */
	public enum LeagueCupValue
	{
		LEAGUE_CUP_VALUE_NAME = 0,
		LEAGUE_CUP_VALUE_SHORT_NAME,
		LEAGUE_CUP_VALUE_SID,
		LEAGUE_CUP_VALUE_SYMBOL,
		LEAGUE_CUP_VALUE_ID,
		LEAGUE_CUP_VALUE_FIRST_WEEK,
		LEAGUE_CUP_VALUE_LAST_WEEK,
		LEAGUE_CUP_VALUE_WEEK_GAP,
		LEAGUE_CUP_VALUE_YELLOW_RED,
		LEAGUE_CUP_VALUE_AVERAGE_SKILL,
		LEAGUE_CUP_VALUE_AVERAGE_CAPACITY,
		LEAGUE_CUP_VALUE_SKILL_DIFF,
		LEAGUE_CUP_VALUE_END
	}

	/** Some team attributes. */
	public enum TeamAttribute
	{
		TEAM_ATTRIBUTE_STYLE = 0,
		TEAM_ATTRIBUTE_BOOST,
		TEAM_ATTRIBUTE_END
	}

	/** The stadium of a team. */
	public class Stadium
	{
		public string name;
		public int Capacity { get; set; } /**< How many people fit in. Default: -1 (depends on league). */
		public int average_attendance = 0; /**< How many people watched on average. Default: 0. */
		public int possible_attendance = 0; /**< How many people would've watched if every game had been
                sold out. We need this only to compute the average attendance in percentage
                of the capacity. Default: 0. */
		public int games = 0; /**< Number of games. Default: 0. */
		public float safety; /**< Safety percentage between 0 and 100. Default: randomized. */
		public float ticket_price;
	}

	/** Structure representing a team.
     * @see Player */
	public class Team
	{
		public const string TAG_TEAM = "team";
		public const string TAG_TEAM_NAME = "team_name";
		public const string TAG_STADIUM_NAME = "stadium_name";
		public const string TAG_SYMBOL = "symbol";
		public const string TAG_AVERAGE_TALENT = "average_talent";
		public const string TAG_FORMATION = "formation";
		public const string TAG_NAMES_FILE = "names_file";
		public const string TAG_PLAYER = "player";
		public const string TAG_PLAYER_NAME = "player_name";
		public const string TAG_PLAYER_BIRTH_YEAR = "birth_year";
		public const string TAG_PLAYER_BIRTH_MONTH = "birth_month";
		public const string TAG_PLAYER_SKILL = "skill";
		public const string TAG_PLAYER_TALENT = "talent";
		public const string TAG_PLAYER_POSITION = "position";

		public const string TAG_TEAM_SYMBOL = "team_symbol";
		public const string TAG_TEAM_NAMES_FILE = "team_names_file";
		public const string TAG_TEAM_AVERAGE_TALENT = "team_average_talent";
		public const string TAG_TEAM_DEF_FILE = "def_file";

		public string name;
		public string symbol;
		/** File the team takes the 
         * player names from. */
		public string namesFile;
		public string defFile;
		/** The sid of the strategy if it's a CPU team. */
		public string strategySid;

		public int clid; /**< Numerical id of the league or cup the team belongs to. */
		public int id; /**< Id of the team. */
		public int structure; /**< Playing structure. @see team_assign_playing_structure() */
		public int Style { get; set; } /**< Playing style. @see team_assign_playing_style() */
		public int boost; /**< Whether player boost or anti-boost is switched on. */

		/** Average talent of the players at generation. */
		public double AverageTalent { get; set; }

		/** A value that influences scoring chances etc.
         * If > 1, the team's lucky, if < 1, it's unlucky.
         * Only used for users' teams. */
		public float Luck { get; set; }

		public Stadium Stadium { get; set; }
		/**
         * Array of players.
         * */
		public List<Player> Players { get; set; }

		public Team(bool bNewId)
		{
#if DEBUG
			Console.WriteLine("Team.Team");
#endif
			Stadium = new Stadium();
			name = namesFile = symbol = defFile = Stadium.name = strategySid = string.Empty;
			clid = -1;
			//TODO id = bNewId ? Bygfoot.NewTeamId : -1;
			structure = 442;
			Style = 0;
			boost = 0;
			AverageTalent = 0;
			Luck = 1;
			Players = new List<Player>();
		}

		public void Load(XmlNode xnTeam)
		{
			XmlNode xnTeamName = xnTeam.SelectSingleNode(TAG_TEAM_NAME);
			name = xnTeamName.InnerText;

			// Symbol
			XmlNode xnSymbol = xnTeam.SelectSingleNode(TAG_TEAM_SYMBOL);
			if (xnSymbol == null)
				xnSymbol = xnTeam.SelectSingleNode(TAG_SYMBOL);
			if (xnSymbol != null)
				symbol = xnSymbol.InnerText;

			// Names File
			XmlNode xnNamesFile = xnTeam.SelectSingleNode(TAG_TEAM_NAMES_FILE);
			if (xnNamesFile == null)
				xnNamesFile = xnTeam.SelectSingleNode(TAG_NAMES_FILE);
			if (xnNamesFile != null)
				namesFile = xnNamesFile.InnerText;

			// Average Talent
			XmlNode xnAverageTalent = xnTeam.SelectSingleNode(TAG_TEAM_AVERAGE_TALENT);
			if (xnAverageTalent == null)
				xnAverageTalent = xnTeam.SelectSingleNode(TAG_AVERAGE_TALENT);
			if (xnAverageTalent != null)
				AverageTalent = Convert.ToDouble(xnAverageTalent.InnerText);

			// Def File
			XmlNode xnDefFile = xnTeam.SelectSingleNode(TAG_TEAM_DEF_FILE);
			if (xnDefFile != null)
				defFile = xnDefFile.InnerText;
		}

		public bool IsUserTeam()
		{
#if DEBUG
			Console.WriteLine("Team.IsUserTeam");
#endif
			for (int i = 0; i < Variables.Users.Count; i++)
			{
				if (Variables.Users[i].TeamId == this.id)
					return true;
			}
			return false;
		}
	}
}
