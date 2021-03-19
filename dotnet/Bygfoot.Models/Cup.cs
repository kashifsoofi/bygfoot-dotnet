using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Bygfoot.Helpers;

namespace Bygfoot.Model
{
	/** Information about what cup another cup has to wait for
     * before scheduling matches. */
	public class CupRoundWait
	{
		/** The cup we wait for. */
		public string cupSid;
		/** The cup round of the cup we wait for. */
		public int cupRound;
	}

	/** Rules for a round of a cup.
     * Cups consist of rounds, e.g. the final counts as
     * a round or the round robin games. */
	public class CupRound
	{
		/** Name of the cup round. By default filled with "Last 32", "Final" etc. */
		public string name;
		/** Whether there are home and away games or only one leg. 
         * Default: TRUE. */
		public bool homeAway = true;
		/** How many times a match gets repeated if there's a draw.
         * Default: 0. */
		public int replay = 0;
		/** Whether the matches are on neutral ground.
         * Default: FALSE. */
		public bool neutral = false;
		/** Whether the teams array gets randomised before writing the fixtures.
         * Default: TRUE. FALSE makes only sense in the first cup round. */
		public bool randomiseTeams = true;
		/** How many round robin groups there are. 
         * Default: 0 (ie. no round robin). */
		public int roundRobinNumberOfGroups = 0;
		/** How many teams advance from each group.
         * Default: -1. */
		public int roundRobinNumberOfAdvance = -1;
		/** How many teams advance apart from those that
         * are specified already (e.g. the first two advance
         * and additionally the best 3 from all the groups.
         * Default: 0. */
		public int roundRobinNumberOfBestAdvance = 0;
		/** How many matchdays there are in the round robin phase. */
		public int roundRobins;
		/** Number of weeks between the parts of a round robin. */
		public ArrayList rr_breaks;
		/** Number of new teams participating in the cup round 
         * (ie. teams that get loaded and are not advancing from a previous
         * round). */
		public int newTeams;
		/** The number of byes to be awarded for this cup round.
         * The default is enough to bring the next round to a power of two. */
		public int byes;
		/** Number of weeks the cup round is delayed (or scheduled sooner if 
         * the value is negative) with respect to the previous cup round and
         * the week gap. Default: 0. */
		public int delay = 0;
		/** Here we store intervals of fixtures during which
         * there should be two matches in a week instead of one.
         * This is only relevant in round robin rounds. */
		public ArrayList[] twoMatchWeeks = new ArrayList[2];
		/** Whether the two matches of a home/away round are
         * played in one week. */
		public bool twoMatchWeek;
		/** The teams that got loaded for this cup round.
         * Mostly this only happens in the first round. */
		public ArrayList teams;
		/** Pointers to all teams loaded in the cup round; these
         * teams get passed to the fixture generation function
         * together with the teams advancing from the previous round. */
		public ArrayList teamPtrs;
		/** Which new teams come into the cup (@see #CupChooseTeam) */
		public List<CupChooseTeam> chooseTeams;
		/** The round robin tables (in case there is a round robin). */
		public ArrayList tables;
		/** Array with CupRoundWaits. */
		public List<CupRoundWait> waits;
	}

	/**
     * Rules for choosing teams from the league files.
     * This could tell us to select the first three teams
     * from the league 'Italy 1' to participate in the cup.
     * */
	public class CupChooseTeam
	{
		/** The string id of the league we choose from.
         * Default: "". */
		public string sid = string.Empty;
		/** The number of teams chosen.
         * Default: -1 (ie. all teams are chosen). */
		public int numberOfTeams = -1;
		/** Which league table to use. Only relevant
         * for leagues which use more than one table during
         * the season. Default is 0, ie. the cumulative table. */
		public int fromTable = 0;
		/** The range from which the teams are chosen,
         * e.g. 2 and 5 means the teams from 2 to 5 are chosen
         * (given that 'number_of_teams' is 4). 
         * Defaults: -1 (ie. the teams are chosen from the whole
         * range of teams in the league). */
		public int start_idx = -1, end_idx = -1;
		/** Whether the teams are chosen randomly,
         * ie. 3 random teams from the range 1-20.
         * Default: FALSE. */
		public bool randomly = false;
		/** Whether the team is generated and loaded from
         * a league file or taken from one of the country's leagues
         * or cups. Default: FALSE. */
		public bool generate = false;
		/** Whether to skip the checking if a team participates in other
         * of the same cup groupcups. */
		public bool skip_group_check;
		/** Whether to load the choose_team when the cup fixtures for the
         * first cup round are written or only when the cup round the choose_team
         * belongs to is scheduled. Default: TRUE. */
		public bool preload = true;
	}

	/** Structure representing a cup. */
	public class Cup
	{
		/** Name and short name of the cup, a pixmap path,
         * and the string id (e.g. england_fa or so).
         * Default: "". */
		public string name = string.Empty;
		public string shortName = string.Empty;
		public string symbol = string.Empty;
		public string sid = string.Empty;
		/** Numerical id. */
		public int id;
		/** An integer specifying which cups are mutually exclusive for
         * league teams, e.g. the same team can't participate in the UEFA Cup and
         * the Champions' League. */
		public int group;
		/** Last week (typically the week the final
         * takes place) and weeks between matchdays.
         * Default: -1 and 1. */
		public int lastWeek = -1, weekGap = 1;
		/** This determines when the cup gets added to the acps
         * pointer array (and becomes visible for the user). Also determines
         * when the cup fixtures for the first round get written.
         * Default: 0 (ie. the cup is visible from the first week). */
		public int add_week = 0;
		/** Number of yellow cards that lead to a missed match.
         * Default: 1000 (off). */
		public int yellow_red = 0;
		/** Difference of the average talent for the cup teams compared to
         * the league with highest average talent.
         * Default: 0. */
		public float talentDiff = 0;
		/** The week and week_round at the beginning of which the fixtures
         * have to be updated. */
		public int nextFixtureUpdateWeek;
		public int nextFixtureUpdateWeekRound;

		/** A gchar pointer array of properties (like "national"). */
		public ArrayList properties;
		/** The rounds of the cup.
         * @see #CupRound*/
		public List<CupRound> rounds;
		/** Pointer array containing teams that got a bye for a round of the cup. */
		public ArrayList bye;
		/** The teams belonging to the cup (stored in the cup rounds,
         * these are only pointers).
         * Relevant only if it's an international one. */
		public ArrayList teams;
		/** Pointer array with the names of all the teams in the cup.
         * Also the teams from the country's leagues. */
		public List<string> team_names;
		/** The fixtures of a season for the cup. */
		public ArrayList fixtures;
		/** Array of custom breaks in schedule. */
		public List<WeekBreak> weekBreaks;
		/** Pointer array with the sids of competitions that
         * the fixtures of which should be avoided when scheduling
         * the cup fixtures. */
		public ArrayList skipWeeksWith;

		public Cup()
		{
		}

		public void Load(string cupName)
		{
			string filename = Path.GetFileName(cupName);
			if (!filename.StartsWith("cup_"))
				filename = "cup_" + filename;
			if (!filename.EndsWith(".xml"))
				filename += ".xml";

			string path = FileHelper.FindSupportFile(filename, false);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(File.ReadAllText(path));
			Load(doc.DocumentElement);
		}

		public void Load(XmlNode xnCup)
		{
		}
	}
}
