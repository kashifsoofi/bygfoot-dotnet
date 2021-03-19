using System;
using System.Collections.Generic;

namespace Bygfoot.Model
{
	public class YouthAcademy
	{
		/** Pointer to the team the academy belongs to. */
		public Team tm;
		/** Position preference of the user. */
		public int pos_pref;
		/** Coach quality, like scout or physio. */
		public int coach;
		/** Percentage of income the user devotes to the youth academy. */
		public int percentage;
		/** Average coach and percentage values; these are used
         * when a new youth comes into the academy to determine
         * the quality of the youth. */
		public float av_coach, av_percentage;
		/** When this counter is <= 0, a new youth appears. */
		public float counter_youth;
		/** The youths are simply young players. */
		public List<Player> players;

		/** Set up a youth academy taking the average skill and talent in
         * the team into account. */
		public YouthAcademy(User user)
		{
#if DEBUG
			Console.WriteLine("YouthAcademy constrctor");
#endif
			// TODO
		}

		/** Add a new player to the academy based on the
         * average skill value of the user team, the average percentage
         * the user paid for the academy for a period of time and the
         * average youth coach quality. */
		public void AddNewPlayer()
		{
#if DEBUG
			Console.WriteLine("YouthAcademy.AddNewPlayer");
#endif
			// TODO
		}

		/** Update the users' youth academies. */
		public void UpdateWeekly()
		{
#if DEBUG
			Console.WriteLine("YouthAcademy.UpdateWeekly");
#endif
			// TODO
		}
	}
}
