using System;
namespace Bygfoot.Model
{
	/** Enumeration describing the type of a job. */
	public enum JobType
	{
		/** Job offer by a team from the country
         * the user's playing. */
		JOB_TYPE_NATIONAL = 0,
		/** Job offer by a team from a different country
         * than user's playing. */
		JOB_TYPE_INTERNATIONAL,
		/** Job offer by a team from a different country
         * than user's playing; the team participates
         * in an international cup. */
		JOB_TYPE_CUP,
		JOB_TYPE_END
	}

	/** A structure representing a job in the job exchange. */
	public class Job
	{
		/** Whether it's an international job or a national one. */
		public JobType type;
		/** How many weeks remaining the offer is on the list. */
		public int time;
		/** Only relevant for international jobs. **/
		public string countryFile, countryName, leagueName;
		/** Only relevant for international jobs. **/
		public int leagueLayer;
		/** Only relevant for international jobs. **/
		public int countryRating;
		/** Average talent compared to the league average in percent. */
		public int talentPercent;
		/** The id of the team the job describes. */
		public int teamId;
	}
}
