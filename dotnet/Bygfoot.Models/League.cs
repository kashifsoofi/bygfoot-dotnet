using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Bygfoot.Helpers;

namespace Bygfoot.Model
{
	public enum PromRelType
	{
		PROM_REL_PROMOTION = 0,
		PROM_REL_RELEGATION,
		PROM_REL_NONE
	}

	/**
     * An element representing a promotion or relegation rule.
     * This means, a PromRelElement specifies a range of teams
     * that get promoted or relegated to a given league.
     * @see PromRel
     * */
	public class PromRelElement
	{
		public int[] ranks = new int[] { 0, 0 };
		public int fromTable = 0;
		public string destSid = string.Empty;
		public PromRelType type = PromRelType.PROM_REL_NONE;
	}

	/**
     * An struct representing promotion/relegation games.
     * */
	public class PromGames
	{
		/** The id of the league the promotion games winner gets promoted to. Default "" */
		public string destSid = string.Empty;
		/** The id of the league the promotion games losers get moved to. Default "" */
		public string loserSid = string.Empty;
		/** Number of teams that advance from the promotion games. Default: 1. */
		public int numberOfAdvance;
		/** The cup determining how the promotion games are handled. */
		public string cupSid;
	}

	/**
     * This structure specifies how promotion and relegation is handled in a league.
     * It contains promotion and relegation rules in an array and possibly also
     * a rule about promotion games to be played.
     * @see PromRelElement
     * */
	public class PromRel
	{
		/** Array with promotion/relegation rules.
         * @see PromRelElement
         * */
		public List<PromRelElement> elements;
		/** Array with promotion/relegation games.
         * @see PromGames
         * */
		public List<PromGames> promGames;

		public PromRel()
		{
			elements = new List<PromRelElement>();
			promGames = new List<PromGames>();
		}
	}

	/**
     * A structure describing a different league joined to the current one
     * in the sense that there are matches played between teams from both leagues
     * like in the US conference system.
     * */
	public class JoinedLeague
	{
		/** Sid of the joined league. */
		public string sid;
		/** How many round robins to schedule. */
		public int rr;
	}

	/**
     * A structure containing a week when a new table
     * gets created with nullified values for the league;
     * older tables get stored.
     * */
	public class NewTable
	{
		public int addWeek;
		public string name;
	}

	/**
     * A structure describing a custom break in the fixtures
     * schedule occuring at a particular week.
     * */
	public class WeekBreak
	{
		/** In which week the break occurs. */
		public int weekNumber;
		/** Length of break in weeks. */
		public int length;
	}

	public class League
	{
		public const string TAG_TEAMS = "teams";

		/** Default value "" */
		public string name { get; set; }
		public string shortName { get; set; }
		public string sid { get; set; }
		public string symbol { get; set; }
		/** The sid of the player names file the 
         * teams in the league take their names from.
         * Default: 'general', meaning the 'player_names_general.xml' file. */
		public string names_file { get; set; }
		/** @see PromRel */
		public PromRel promRel { get; set; }
		/** Numerical id, as opposed to the string id 'sid'. */
		public int id;
		/** Layer of the league; this specifies which leagues are
         * parallel. */
		public int layer { get; set; }
		/** The first week games are played. Default 1. */
		public int firstWeek { get; set; }
		/** Weeks between two matchdays. Default 1. */
		public int weekGap { get; set; }
		/** Here we store intervals of fixtures during which
         * there should be two matches in a week instead of one. */
		public List<int>[] twoMatchWeeks { get; set; }
		/** How many round robins are played. Important for
         * small leagues with 10 teams or so. Default: 2. */
		public int roundRobins { get; set; }
		/** Number of weeks between the parts of a round robin. */
		public List<int> rrBreaks { get; set; }
		/** Number of yellow cards until a player gets banned. 
         * Default 1000 (which means 'off', basically). */
		public int yellowCard { get; set; }
		/** Average talent for the first season. Default: -1. */
		public float averageTalent { get; set; }
		/** Array of teams in the league.
         * @see Team */
		public List<Team> teams { get; set; }
		/** List of leagues joined fixture-wise to this one.
         * @see JoinedLeague */
		public List<JoinedLeague> joinedLeagues { get; set; }
		/** League tables. Usually only one, sometimes more than one is created.
         * @see Table */
		public List<Table> tables { get; set; }
		/** Array holding NewTable elements specifying when to add
         * a new table to the tables array. */
		public List<NewTable> newTables { get; set; }
		/** The fixtures of a season for the league. */
		public List<Fixture> fixtures = new List<Fixture>();
		/** A gchar pointer array of properties (like "inactive"). */
		public ArrayList properties;
		/** Array of custom breaks in schedule. */
		public List<WeekBreak> weekBreaks { get; set; }
		/** The current league statistics. */
		public LeagueStat stats;
		/** Pointer array with the sids of competitions that
        the fixtures of which should be avoided when scheduling
        the league fixtures. */
		public List<string> skipWeeksWith { get; set; }

		public League(bool bNewId)
		{
#if DEBUG
			Console.WriteLine("League.League");
#endif
			name = string.Empty;
			//TODO names_file = Option.OptStr ("string_opt_player_names_file");
			sid = string.Empty;
			shortName = string.Empty;
			symbol = string.Empty;

			//TODO id = bNewId ? Bygfoot.NewLeagueId : -1;
			layer = -1;

			averageTalent = 0;
			promRel = new PromRel();

			teams = new List<Team>();
			fixtures = new List<Fixture>();
			joinedLeagues = new List<JoinedLeague>();
			newTables = new List<NewTable>();
			tables = new List<Table>();
			//TODO: new.properties = g_ptr_array_new();
			skipWeeksWith = new List<string>();
			rrBreaks = new List<int>();
			weekBreaks = new List<WeekBreak>();

			firstWeek = weekGap = 1;
			twoMatchWeeks = new List<int>[2];
			twoMatchWeeks[0] = new List<int>();
			twoMatchWeeks[1] = new List<int>();
			roundRobins = 2;
			yellowCard = 1000;

			stats = new LeagueStat(string.Empty, string.Empty);
		}

		public void Load(string leagueName)
		{
			string filename = Path.GetFileName(leagueName);
			if (!filename.StartsWith("league_"))
				filename = "league_" + filename;
			if (!filename.EndsWith(".xml"))
				filename += ".xml";

			string path = FileHelper.FindSupportFile(filename, false);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(File.ReadAllText(path));
			Load(doc.DocumentElement);
		}

		public void Load(XmlNode xnLeague)
		{
			XmlNode xnName = xnLeague.SelectSingleNode(XmlHelper.TAG_DEF_NAME);
			name = xnName.InnerText;
			XmlNode xnShortName = xnLeague.SelectSingleNode(XmlHelper.TAG_DEF_SHORT_NAME);
			shortName = xnShortName.InnerText;
			XmlNode xnSid = xnLeague.SelectSingleNode(XmlHelper.TAG_DEF_SID);
			sid = xnSid.InnerText;
			XmlNode xnSymbol = xnLeague.SelectSingleNode(XmlHelper.TAG_DEF_SYMBOL);
			symbol = xnSymbol.InnerText;

			XmlNode xnTeams = xnLeague.SelectSingleNode(TAG_TEAMS);
			foreach (XmlNode xnTeam in xnTeams.ChildNodes)
			{
				Team team = new Team(true);
				if (string.IsNullOrEmpty(team.symbol))
					team.symbol = symbol;
				if (string.IsNullOrEmpty(team.namesFile))
					team.namesFile = names_file;
				team.clid = id;

				team.Load(xnTeam);
				teams.Add(team);
			}
		}
	}
}
