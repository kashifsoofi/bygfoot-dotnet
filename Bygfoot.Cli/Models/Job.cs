namespace Bygfoot.Cli.Models;

/// <summary>
/// Enumeration describing the type of a job. Corresponds to C enum JobType.
/// </summary>
public enum JobType
{
    National = 0,      // JOB_TYPE_NATIONAL
    International,     // JOB_TYPE_INTERNATIONAL
    Cup,               // JOB_TYPE_CUP
    End                // JOB_TYPE_END
}

/// <summary>
/// A structure representing a job in the job exchange. Corresponds to C struct Job.
/// </summary>
public class Job
{
    /// <summary>Whether it's an international job or a national one. Corresponds to C: gint type.</summary>
    public JobType Type { get; set; }

    /// <summary>How many weeks remaining the offer is on the list. Corresponds to C: gint time.</summary>
    public int Time { get; set; }

    /// <summary>Only relevant for international jobs. Corresponds to C: gchar *country_file, *country_name, *league_name.</summary>
    public string CountryFile { get; set; }

    /// <summary>Only relevant for international jobs. Corresponds to C: gchar *country_name, *league_name.</summary>
    public string CountryName { get; set; }

    /// <summary>Only relevant for international jobs. Corresponds to C: gchar *league_name.</summary>
    public string LeagueName { get; set; }

    /// <summary>Only relevant for international jobs. Corresponds to C: gint league_layer.</summary>
    public int LeagueLayer { get; set; }

    /// <summary>Only relevant for international jobs. Corresponds to C: gint country_rating.</summary>
    public int CountryRating { get; set; }

    /// <summary>Average talent compared to the league average in percent. Corresponds to C: gint talent_percent.</summary>
    public int TalentPercent { get; set; }

    /// <summary>The id of the team the job describes. Corresponds to C: gint team_id.</summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Update the job exchange: remove expired offers and add new ones. Corresponds to C: job_update.
    /// </summary>
    public static void Update()
    {
        // Implementation depends on the not yet converted jobs, user and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add some new international job offers to the job exchange. Corresponds to C: job_add_new_international.
    /// </summary>
    /// <param name="numOfNew">The number of new offers.</param>
    public static void AddNewInternational(int numOfNew)
    {
        // Implementation depends on the not yet converted country, team and job functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the country file is already loaded and part of the country array. Corresponds to C: job_country_is_in_list.
    /// </summary>
    /// <param name="countryFile">The country file.</param>
    /// <param name="countries">The countries.</param>
    /// <param name="len">The length of the array.</param>
    /// <returns>The index in the countries array or -1.</returns>
    public static int CountryIsInList(string countryFile, List<Country> countries, int len)
    {
        // Implementation depends on the not yet converted country and job functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add a new national job offer to the job exchange. Corresponds to C: job_add_new_national.
    /// </summary>
    public static void AddNewNational()
    {
        // Implementation depends on the not yet converted job and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the team with given id is already on the job exchange list. Corresponds to C: job_team_is_on_list.
    /// </summary>
    /// <param name="teamId">The team id.</param>
    /// <returns>The index in the jobs array or -1.</returns>
    public static int TeamIsOnList(int teamId)
    {
        for (int i = 0; i < Variables.Jobs.Count; i++)
        {
            if (Variables.Jobs[i].TeamId == teamId)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Find the team going with the job offer. Corresponds to C: job_get_team.
    /// </summary>
    /// <param name="job">The job.</param>
    /// <returns>The team.</returns>
    public static Team GetTeam(Job job)
    {
        // Implementation depends on the not yet converted team and job functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the user's application for the job gets accepted. Corresponds to C: query_job_application_successful.
    /// </summary>
    /// <param name="job">The job.</param>
    /// <param name="user">The user.</param>
    /// <returns>Whether the application is successful.</returns>
    public static bool QueryApplicationSuccessful(Job job, User user)
    {
        // Implementation depends on the not yet converted team, league and finance functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove a job from the jobs array. Corresponds to C: job_remove.
    /// </summary>
    /// <param name="job">The job.</param>
    /// <param name="freeTm">Whether to free memory occupied by the job and its team.</param>
    public static void Remove(Job job, bool freeTm)
    {
        // Implementation depends on the not yet converted job and finance functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Change the game so that the country is used that's specified in the job. Corresponds to C: job_change_country.
    /// </summary>
    /// <param name="job">The job.</param>
    public static void ChangeCountry(Job job)
    {
        // Implementation depends on the not yet converted job, user, team and season functions.
        throw new NotImplementedException();
    }
}
