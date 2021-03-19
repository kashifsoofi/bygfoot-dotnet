using System;

namespace Bygfoot.Model
{
	/** A struct representing a betting possibility: a fixture plus odds. */
	public class BetMatch
	{
		/** The match the bet is about. */
        public int FixId { get; set; }
		/** The odds for a win, draw or loss of the first team. */
        public float[] Odds { get; set; } = new float[3];
	}

	/** A struct representing a bet by a user. */
	public class BetUser
	{
		/** Match the user betted on. */
        public int FixId { get; set; }
		/** Which outcome he picked. */
        public int Outcome { get; set; }
		/** How much he wagered. */
        public int Wager { get; set; }
	}
}
