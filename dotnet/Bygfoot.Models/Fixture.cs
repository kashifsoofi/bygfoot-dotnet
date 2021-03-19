using System;
namespace Bygfoot.Model
{
	public enum FixtureCompare
	{
		FIXTURE_COMPARE_DATE = 0,
		FIXTURE_COMPARE_END
	}

	/** Structure representing a fixture, or, in other words,
     * a match. */
	public class Fixture
	{
		/** The cup or league the fixture belongs to. */
		public int clid;
		/** The unique id of the fixture. */
		public int id;
		/** The round (in a cup) the fixture belongs to. */
		public int round;
		/** The replay number (ie. how often the match was repeated because of a draw). */
		public int replay_number;
		/** When it takes place. */
		public int week_number, week_round_number;
		/** The teams involved. */
		public Team[] teams = new Team[2];
		/** Ids of the teams. Needed when the team
         * pointers get invalid (e.g. after promotion/relegation). */
		public int[] teamIds = new int[2];
		/** The number of goals for each team in
         * regulation, extra time and penalty shoot-out. */
		public int[,] result = new int[2, 3];
		/** Whether there's home advantage, this is second leg,
         * or the game has to be decided. */
		public bool home_advantage, second_leg, decisive;
		/* How many people attended and whether there were pecial events. */
		public int attendance;
		/** Pointer to the live game used for the fixture calculation. */
		public LiveGame liveGame;
	}
}
