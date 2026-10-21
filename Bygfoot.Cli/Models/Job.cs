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
}
