using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	/**
     * Table element values.
     * @see TableElement
     * @see Table
     * */
	public enum TableElementValues
	{
		TABLE_PLAYED = 0,
		TABLE_WON,
		TABLE_DRAW,
		TABLE_LOST,
		TABLE_GF,
		TABLE_GA,
		TABLE_GD,
		TABLE_PTS,
		TABLE_END
	}

	/**
     * An element representing a team in the tables.
     * @see Table
     * @see #TableElementValues
     * */
	public class TableElement
	{
		public Team team;
		public int teamId;
		/** The rank of the element before the last update. 
         * Used to display an arrow if the rank changed. */
		public int oldRank;
		public int[] values = new int[(int)TableElementValues.TABLE_END];
	}

	/**
     * A table belonging to a league or a cup with round robin.
     * @see TableElement
     * */
	public class Table
	{
		public string name;
		public int clid;
		/** The cup round (or -1 if it's a league). */
		public int round;
		public List<TableElement> elements;
	}
}
