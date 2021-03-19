using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	/** A statistics element holding some
     * string and integer values. */
	public class Stat
	{
		public string teamName;
		public int value1, value2, value3;
		public string valueString;
	}

	/** A structure holding some stat arrays about a league. */
	public class LeagueStat
	{
		public string leagueSymbol { get; set; }
		public string leagueName { get; set; }

		/** Best offensive and defensive teams. */
		public List<Stat> teamsOff { get; set; }
		public List<Stat> teamsDef { get; set; }
		/** Best goal getters and goalies. */
		public List<Stat> playerScorers { get; set; }
		public List<Stat> playerGoalies { get; set; }

		public LeagueStat(string leagueName, string leagueSymbol)
		{
			this.leagueName = leagueName;
			this.leagueSymbol = leagueSymbol;
			teamsOff = new List<Stat>();
			teamsDef = new List<Stat>();
			playerScorers = new List<Stat>();
			playerGoalies = new List<Stat>();
		}
	}

	/** A team name and a competition name. */
	public class ChampStat
	{
		public string teamName, clName;
	}

	/** A season statistics structure. */
	public class SeasonStat
	{
		/** Which season */
		public int season_number;

		/** League and cup winners. */
		public List<ChampStat> leagueChamps;
		public List<ChampStat> cupChamps;

		/** The league stats at the end of the season. */
		public List<LeagueStat> leagueStats;
	}
}
