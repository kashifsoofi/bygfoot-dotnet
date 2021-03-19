using System;
using System.Collections;

namespace Bygfoot.Model
{
	/** Lineup types for a CPU team (ie. which players
     * are preferred when putting together the first 11). */
	public enum StratLineupType
	{
		STRAT_LINEUP_BEST = 1,
		STRAT_LINEUP_WEAKEST,
		STRAT_LINEUP_FITTEST,
		STRAT_LINEUP_UNFITTEST,
		STRAT_LINEUP_END
	}

	public class StrategyMatchAction
	{
		/** A condition describing when the action should be taken. */
		public string condition, subCondition;
		/** New boost and style values. */
		public int boost, style;
		/** Substitution specifiers (position and property).
         * Property is taken from #StratLineupType. */
		public int sub_in_pos, sub_in_prop, sub_out_pos, sub_out_prop;
		/** An id to prevent actions from being applied again and
         * again during a match. */
		public int id;
	}

	/** A CPU strategy. */
	public class Strategy
	{
		/** String id and description of the strategy. */
		public string sid, desc;
		/** How often this strategy gets picked, relative
         * to the other strategies. */
		public int priority;
		/** Array with prematch settings. */
		public ArrayList prematch;
		/** Array with match settings. */
		public ArrayList matchAction;
	}
}
