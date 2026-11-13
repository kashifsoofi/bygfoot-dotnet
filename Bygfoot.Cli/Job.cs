namespace Bygfoot.Cli;

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
        int newOffers;
        int intOffers;

        for (int i = Variables.Jobs.Count - 1; i >= 0; i--)
        {
            Variables.Jobs[i].Time--;

            if (Variables.Jobs[i].Time <= 0)
                Remove(Variables.Jobs[i], true);
        }

        if (Variables.Week % OptionHelper.ConstInt("int_job_update_interval") != 2)
            return;

        newOffers = MathHelper.Rndi(OptionHelper.ConstInt("int_job_new_offers_lower"),
            OptionHelper.ConstInt("int_job_new_offers_upper"));
        intOffers = (Variables.Users.Count == 1)
            ? (int)Math.Round((double)newOffers * OptionHelper.ConstFloat("float_job_international_perc"))
            : 0;

        for (int i = 0; i < newOffers - intOffers; i++)
            AddNewNational();

        AddNewInternational(intOffers);
    }

    /// <summary>
    /// Add some new international job offers to the job exchange. Corresponds to C: job_add_new_international.
    /// </summary>
    /// <param name="numOfNew">The number of new offers.</param>
    public static void AddNewInternational(int numOfNew)
    {
        List<string> countryFiles = FileHelper.GetCountryFiles();
        List<Country> countries = new List<Country>();
        Team tm;
        League league;

        for (int i = 0; i < numOfNew; i++)
        {
            int rndom;
            int idx;
            Job newJob = new Job();

            do
                rndom = MathHelper.Rndi(0, countryFiles.Count - 1);
            while (countryFiles[rndom].Contains(Variables.Country.SId));

            idx = CountryIsInList(countryFiles[rndom], countries, countries.Count);

            if (idx == -1)
            {
                idx = countries.Count;
                countries.Add(new Country());
                CountryXml.XmlCountryRead(countryFiles[rndom], countries[idx]);
                Variables.Counters[(int)Counters.CountLeagueId] -= countries[idx].Leagues.Count;
            }

            PickTeamFromCountry(countries[idx], out tm, out league);

            newJob.CountryFile = $"country_{countries[idx].SId}.xml";
            newJob.Time = MathHelper.Rndi(OptionHelper.ConstInt("int_job_update_interval") - 1,
                OptionHelper.ConstInt("int_job_update_interval") + 1);
            newJob.CountryName = countries[idx].Name;
            newJob.CountryRating = countries[idx].Rating;
            newJob.LeagueName = league.Name;
            newJob.LeagueLayer = league.Layer;

            int teamId = TeamIsInCup(tm.Name);

            if (teamId == -1)
            {
                Team.GeneratePlayersStadium(tm, league.AverageTalent);
                Variables.JobTeams.Add(tm);

                newJob.TeamId = tm.Id;
                newJob.Type = JobType.International;
            }
            else
            {
                newJob.TeamId = teamId;
                newJob.Type = JobType.Cup;
            }

            newJob.TalentPercent = (int)Math.Round((double)(Team.GetAverageTalent(tm) /
                league.AverageTalent) * 100);

            Variables.Jobs.Add(newJob);
        }

        Free.FreeGcharArray(countryFiles);
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
        for (int i = 0; i < len; i++)
        {
            if (string.IsNullOrEmpty(countries[i].SId))
                return -1;
            else if (countryFile.Contains(countries[i].SId))
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Add a new national job offer to the job exchange. Corresponds to C: job_add_new_national.
    /// </summary>
    public static void AddNewNational()
    {
        Job newJob = new Job();
        League league;
        Team tm;

        PickTeamFromCountry(Variables.Country, out tm, out league);

        newJob.Type = JobType.National;
        newJob.Time = MathHelper.Rndi(OptionHelper.ConstInt("int_job_update_interval") - 1,
            OptionHelper.ConstInt("int_job_update_interval") + 1);
        newJob.CountryFile = null;
        newJob.CountryName = Variables.Country.Name;
        newJob.CountryRating = -1;
        newJob.LeagueName = league.Name;
        newJob.LeagueLayer = league.Layer;

        newJob.TalentPercent = (int)Math.Round((double)(Team.GetAverageTalent(tm) /
            league.AverageTalent) * 100);
        newJob.TeamId = tm.Id;

        Variables.Jobs.Add(newJob);
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
        if (job.Type == JobType.National)
            return Team.OfId(job.TeamId);
        else if (job.Type == JobType.International)
        {
            for (int i = 0; i < Variables.JobTeams.Count; i++)
                if (Variables.JobTeams[i].Id == job.TeamId)
                    return Variables.JobTeams[i];
        }
        else
        {
            for (int i = 0; i < Variables.Country.AllCups.Count; i++)
                for (int j = 0; j < Variables.Country.AllCups[i].Teams.Count; j++)
                    if (Variables.Country.AllCups[i].Teams[j] == job.TeamId)
                        return Team.OfId(job.TeamId);
        }

        Main.MainExitProgram(Bygfoot.ExitCode.PointerNotFound,
            "job_get_team: team not found (league %s, country %s.", job.LeagueName, job.CountryName);

        return null;
    }

    /// <summary>
    /// Find out whether the user's application for the job gets accepted. Corresponds to C: query_job_application_successful.
    /// </summary>
    /// <param name="job">The job.</param>
    /// <param name="user">The user.</param>
    /// <returns>Whether the application is successful.</returns>
    public static bool QueryApplicationSuccessful(Job job, User user)
    {
        float successNeeded;
        Team tm = GetTeam(job);
        float userAvSkill = Team.GetAverageSkill(user.Team, false);
        float jobAvSkill = Team.GetAverageSkill(tm, false);

        successNeeded = (jobAvSkill - userAvSkill) *
            OptionHelper.ConstInt("int_job_application_points_per_av_skill");
        successNeeded += (job.LeagueLayer - League.LeagueFromClid(user.Team.Clid)!.Layer) *
            OptionHelper.ConstInt("int_job_application_points_per_layer");

        if (job.Type != JobType.National)
        {
            successNeeded += OptionHelper.ConstInt("int_job_application_points_international");
            successNeeded += (job.CountryRating - Variables.Country.Rating) *
                OptionHelper.ConstInt("int_job_application_points_per_rating");
        }

        return user.Counters[(int)CounterType.Success] >= successNeeded;
    }

    /// <summary>
    /// Remove a job from the jobs array. Corresponds to C: job_remove.
    /// </summary>
    /// <param name="job">The job.</param>
    /// <param name="freeTm">Whether to free memory occupied by the job and its team.</param>
    public static void Remove(Job job, bool freeTm)
    {
        for (int i = 0; i < Variables.Jobs.Count; i++)
            if (Variables.Jobs[i] == job)
            {
                Free.FreeJob(job, freeTm);
                Variables.Jobs.RemoveAt(i);
                break;
            }
    }

    /// <summary>
    /// Change the game so that the country is used that's specified in the job. Corresponds to C: job_change_country.
    /// </summary>
    /// <param name="job">The job.</param>
    public static void ChangeCountry(Job job)
    {
        int seasonTemp = Variables.Season + 1;
        Team tm = GetTeam(job);

        for (int i = Variables.TransferList.Count - 1; i >= 0; i--)
            Transfer.TransferRemovePlayer(i);

        Free.FreeBets(true);

        // There's only one user (otherwise
        // international job offers are disabled).
        for (int i = 0; i < 2; i++)
            Variables.Users[0].Bets[i] = new List<BetUser>();

        LiveGame.Reset(Variables.Users[0].LiveGame, null, false);

        Free.FreeCountry(Variables.Country, true);

        CountryXml.XmlCountryRead(job.CountryFile, Variables.Country);

        Bygfoot.Stat5 = (int)Status0Value.StatusGenerateTeams;
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            for (int j = 0; j < Variables.Country.Leagues[i].Teams.Count; j++)
                if (Variables.Country.Leagues[i].Teams[j].Name != tm.Name)
                    Team.GeneratePlayersStadium(Variables.Country.Leagues[i].Teams[j], 0);
                else
                {
                    tm.Id = Variables.Country.Leagues[i].Teams[j].Id;
                    tm.Clid = Variables.Country.Leagues[i].Teams[j].Clid;
                    job.TeamId = tm.Id;
                    Free.FreeTeam(Variables.Country.Leagues[i].Teams[j]);
                    Variables.Country.Leagues[i].Teams[j] = tm;

                    for (int k = 0; k < Variables.Country.Leagues[i].Teams[j].Players.Count; k++)
                        Variables.Country.Leagues[i].Teams[j].Players[k].Team =
                            Variables.Country.Leagues[i].Teams[j];
                }
        Bygfoot.Stat5 = -1;

        // Set season to 1 so that some special things
        // in the start_new_season function don't get applied.
        Variables.Season = 1;
        StartEnd.StartNewSeason();
        Variables.Season = seasonTemp;
    }

    /// <summary>
    /// Pick a random team and its league from the given country.
    /// Corresponds to C: job_pick_team_from_country.
    /// </summary>
    /// <param name="cntry">The country.</param>
    /// <param name="tm">The picked team.</param>
    /// <param name="league">The league the team belongs to.</param>
    private static void PickTeamFromCountry(Country cntry, out Team tm, out League league)
    {
        int rndom;
        int i;
        List<int> teamLens = new List<int>(cntry.Leagues.Count);
        Team selectedTm = null;
        League selectedLeague = null;

        teamLens.Add(cntry.Leagues[0].Teams.Count);
        for (i = 1; i < cntry.Leagues.Count; i++)
            teamLens.Add(teamLens[i - 1] + cntry.Leagues[i].Teams.Count);

        do
        {
            rndom = MathHelper.Rndi(0, teamLens[cntry.Leagues.Count - 1] - 1);
            for (i = 0; i < cntry.Leagues.Count; i++)
                if (rndom < teamLens[i])
                {
                    selectedTm = (i > 0)
                        ? cntry.Leagues[i].Teams[rndom - teamLens[i - 1]]
                        : cntry.Leagues[i].Teams[rndom];
                    selectedLeague = cntry.Leagues[i];
                    break;
                }
        }
        while (Team.IsUser(selectedTm!) != -1 || TeamIsOnList(selectedTm!.Id) != -1);

        tm = selectedTm;
        league = selectedLeague;
    }

    /// <summary>
    /// Find out whether the team with given name is participating in an
    /// international cup (and thus doesn't have to be generated).
    /// Corresponds to C: job_team_is_in_cup.
    /// </summary>
    /// <param name="teamName">The team name.</param>
    /// <returns>The id of the team if found or -1.</returns>
    private static int TeamIsInCup(string teamName)
    {
        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
            for (int j = 0; j < Variables.Country.AllCups[i].Teams.Count; j++)
            {
                Team cupTeam = Team.OfId(Variables.Country.AllCups[i].Teams[j]);
                if (cupTeam != null && cupTeam.Name == teamName)
                    return cupTeam.Id;
            }

        return -1;
    }
}
