namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;

/// <summary>
/// Information about what cup another cup has to wait for
/// before scheduling matches. Corresponds to C struct CupRoundWait.
/// </summary>
public class CupRoundWait
{
    /// <summary>The cup we wait for. Corresponds to C: gchar *cup_sid.</summary>
    public string CupSid { get; set; }

    /// <summary>The cup round of the cup we wait for. Corresponds to C: gint cup_round.</summary>
    public int CupRound { get; set; }
}

/// <summary>
/// Rules for a round of a cup.
/// Cups consist of rounds, e.g. the final counts as a round or the round robin games.
/// Corresponds to C struct CupRound.
/// </summary>
public class CupRound
{
    /// <summary>Name of the cup round. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Whether there are home and away games or only one leg. Corresponds to C: gboolean home_away.</summary>
    public bool HomeAway { get; set; }

    /// <summary>How many times a match gets repeated if there's a draw. Corresponds to C: gint replay.</summary>
    public int Replay { get; set; }

    /// <summary>Whether the matches are on neutral ground. Corresponds to C: gboolean neutral.</summary>
    public bool Neutral { get; set; }

    /// <summary>Whether the teams array gets randomised before writing the fixtures. Corresponds to C: gboolean randomise_teams.</summary>
    public bool RandomiseTeams { get; set; }

    /// <summary>How many round robin groups there are. Corresponds to C: gint round_robin_number_of_groups.</summary>
    public int RoundRobinNumberOfGroups { get; set; }

    /// <summary>How many teams advance from each group. Corresponds to C: gint round_robin_number_of_advance.</summary>
    public int RoundRobinNumberOfAdvance { get; set; }

    /// <summary>How many teams advance apart from those that are specified already. Corresponds to C: gint round_robin_number_of_best_advance.</summary>
    public int RoundRobinNumberOfBestAdvance { get; set; }

    /// <summary>How many matchdays there are in the round robin phase. Corresponds to C: gint round_robins.</summary>
    public int RoundRobins { get; set; }

    /// <summary>Number of weeks between the parts of a round robin. Corresponds to C: GArray *rr_breaks.</summary>
    public List<int> RrBreaks { get; set; } = new List<int>();

    /// <summary>Number of new teams participating in the cup round. Corresponds to C: gint new_teams.</summary>
    public int NewTeams { get; set; }

    /// <summary>The number of byes to be awarded for this cup round. Corresponds to C: gint byes.</summary>
    public int Byes { get; set; }

    /// <summary>Number of weeks the cup round is delayed (or scheduled sooner). Corresponds to C: gint delay.</summary>
    public int Delay { get; set; }

    /// <summary>Intervals of fixtures during which there should be two matches in a week. Corresponds to C: GArray *two_match_weeks[2].</summary>
    public List<int>[] TwoMatchWeeks { get; set; } = new List<int>[2];

    /// <summary>Whether the two matches of a home/away round are played in one week. Corresponds to C: gboolean two_match_week.</summary>
    public bool TwoMatchWeek { get; set; }

    /// <summary>The teams that got loaded for this cup round. Corresponds to C: GArray *teams.</summary>
    public List<int> Teams { get; set; } = new List<int>();

    /// <summary>Pointers to all teams loaded in the cup round. Corresponds to C: GPtrArray *team_ptrs.</summary>
    public List<int> TeamPtrs { get; set; } = new List<int>();

    /// <summary>Which new teams come into the cup. Corresponds to C: GArray *choose_teams.</summary>
    public List<CupChooseTeam> ChooseTeams { get; set; } = new List<CupChooseTeam>();

    /// <summary>The round robin tables. Corresponds to C: GArray *tables.</summary>
    public List<Table> Tables { get; set; } = new List<Table>();

    /// <summary>Array with CupRoundWaits. Corresponds to C: GArray *waits.</summary>
    public List<CupRoundWait> Waits { get; set; } = new List<CupRoundWait>();
}

/// <summary>
/// Rules for choosing teams from the league files. Corresponds to C struct CupChooseTeam.
/// </summary>
public class CupChooseTeam
{
    /// <summary>The string id of the league we choose from. Corresponds to C: gchar *sid.</summary>
    public string Sid { get; set; }

    /// <summary>The number of teams chosen. Corresponds to C: gint number_of_teams.</summary>
    public int NumberOfTeams { get; set; }

    /// <summary>Which league table to use. Corresponds to C: gint from_table.</summary>
    public int FromTable { get; set; }

    /// <summary>The range from which the teams are chosen. Corresponds to C: gint start_idx, end_idx.</summary>
    public int StartIdx { get; set; }
    public int EndIdx { get; set; }

    /// <summary>Whether the teams are chosen randomly. Corresponds to C: gboolean randomly.</summary>
    public bool Randomly { get; set; }

    /// <summary>Whether the team is generated and loaded from a league file. Corresponds to C: gboolean generate.</summary>
    public bool Generate { get; set; }

    /// <summary>Whether to skip the checking if a team participates in other of the same cup groupcups. Corresponds to C: gboolean skip_group_check.</summary>
    public bool SkipGroupCheck { get; set; }

    /// <summary>Whether to load the choose_team when the cup fixtures for the first cup round are written. Corresponds to C: gboolean preload.</summary>
    public bool Preload { get; set; }
}

/// <summary>
/// Structure representing a cup. Corresponds to C struct Cup.
/// </summary>
public class Cup
{
    public string Sid { get; set; }

    /// <summary>Name of the cup. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Short name of the cup. Corresponds to C: gchar *short_name.</summary>
    public string ShortName { get; set; }

    /// <summary>Symbol of the cup. Corresponds to C: gchar *symbol.</summary>
    public string Symbol { get; set; }

    /// <summary>The id of the cup. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>The group of the cup. Corresponds to C: gint group.</summary>
    public int Group { get; set; }

    /// <summary>The last week of the cup. Corresponds to C: gint last_week.</summary>
    public int LastWeek { get; set; }

    /// <summary>Weeks between matchdays. Corresponds to C: gint week_gap.</summary>
    public int WeekGap { get; set; }

    /// <summary>The week when the cup is added to the acps pointer array. Corresponds to C: gint add_week.</summary>
    public int AddWeek { get; set; }

    /// <summary>The number of yellow cards that lead to a missed match. Corresponds to C: gint yellow_red.</summary>
    public int YellowRed { get; set; }

    /// <summary>Difference of the average talent for the cup teams. Corresponds to C: gfloat talent_diff.</summary>
    public float TalentDiff { get; set; }

    /// <summary>The week and week_round at the beginning of which the fixtures have to be updated. Corresponds to C: gint next_fixture_update_week, next_fixture_update_week_round.</summary>
    public int NextFixtureUpdateWeek { get; set; }
    public int NextFixtureUpdateWeekRound { get; set; }

    /// <summary>A pointer array of properties. Corresponds to C: GPtrArray *properties.</summary>
    public List<string> Properties { get; set; } = new List<string>();

    /// <summary>The rounds of the cup. Corresponds to C: GArray *rounds.</summary>
    public List<CupRound> Rounds { get; set; } = new List<CupRound>();

    /// <summary>A list of byes for the cup rounds. Corresponds to C: GArray *bye.</summary>
    public List<int> Bye { get; set; } = new List<int>();

    /// <summary>The teams belonging to the cup. Corresponds to C: GPtrArray *teams.</summary>
    public List<int> Teams { get; set; } = new List<int>();

    /// <summary>Pointer array containing the names of all the teams in the cup. Corresponds to C: GPtrArray *team_names.</summary>
    public List<string> TeamNames { get; set; } = new List<string>();

    /// <summary>The fixtures of a season for the cup. Corresponds to C: GArray *fixtures.</summary>
    public List<Fixture> Fixtures { get; set; } = new List<Fixture>();

    /// <summary>Array of custom breaks in schedule. Corresponds to C: GArray *week_breaks.</summary>
    public List<int> WeekBreaks { get; set; } = new List<int>();

    /// <summary>Pointer array with the sids of competitions. Corresponds to C: GPtrArray *skip_weeks_with.</summary>
    public List<string> SkipWeeksWith { get; set; } = new List<string>();

    /// <summary>
    /// Create a new cup with default values.
    /// Corresponds to C: cup_new.
    /// </summary>
    /// <param name="newId">Whether a new id should be created.</param>
    /// <returns>A new cup.</returns>
    public static Cup New(bool newId)
    {
        Cup newCup = new Cup();
        newCup.Sid = null;
        newCup.Name = null;
        newCup.ShortName = null;
        newCup.Symbol = null;
        newCup.Id = newId ? Bygfoot.CupIdNew() : -1;
        newCup.Group = -1;
        newCup.AddWeek = 0;
        newCup.LastWeek = -1;
        newCup.WeekGap = 1;
        newCup.YellowRed = 1000;
        newCup.TalentDiff = 0;
        newCup.NextFixtureUpdateWeek = -1;
        newCup.NextFixtureUpdateWeekRound = -1;
        newCup.Properties = new List<string>();
        newCup.Rounds = new List<CupRound>();
        newCup.Bye = new List<int>();
        newCup.Teams = new List<int>();
        newCup.TeamNames = new List<string>();
        newCup.Fixtures = new List<Fixture>();
        newCup.WeekBreaks = new List<int>();
        newCup.SkipWeeksWith = new List<string>();

        return newCup;
    }

    /// <summary>
    /// Return a CupChooseTeam with default values.
    /// Corresponds to C: cup_choose_team_new.
    /// </summary>
    /// <returns>A new CupChooseTeam.</returns>
    public static CupChooseTeam CupChooseTeamNew()
    {
        CupChooseTeam newChooseTeam = new CupChooseTeam();
        newChooseTeam.Sid = null;
        newChooseTeam.NumberOfTeams = -1;
        newChooseTeam.StartIdx = -1;
        newChooseTeam.EndIdx = -1;
        newChooseTeam.Randomly = false;
        newChooseTeam.Generate = false;
        newChooseTeam.SkipGroupCheck = false;
        newChooseTeam.FromTable = 0;
        newChooseTeam.Preload = true;

        return newChooseTeam;
    }

    /// <summary>
    /// Return a CupRound with default values.
    /// Corresponds to C: cup_round_new.
    /// </summary>
    /// <returns>A new CupRound.</returns>
    public static CupRound CupRoundNew()
    {
        CupRound newRound = new CupRound();
        newRound.Name = null;
        newRound.NewTeams = 0;
        newRound.Byes = -1;
        newRound.HomeAway = true;
        newRound.Replay = 0;
        newRound.Neutral = false;
        newRound.RandomiseTeams = true;
        newRound.Delay = 0;
        newRound.RoundRobinNumberOfGroups = 0;
        newRound.RoundRobinNumberOfAdvance = 0;
        newRound.RoundRobinNumberOfBestAdvance = 0;
        newRound.RoundRobins = 2;
        newRound.RrBreaks = new List<int>();
        newRound.TwoMatchWeeks = new List<int>[2];
        newRound.TwoMatchWeeks[0] = new List<int>();
        newRound.TwoMatchWeeks[1] = new List<int>();
        newRound.TwoMatchWeek = false;
        newRound.Tables = new List<Table>();
        newRound.ChooseTeams = new List<CupChooseTeam>();
        newRound.Teams = new List<int>();
        newRound.Waits = new List<CupRoundWait>();
        newRound.TeamPtrs = new List<int>();

        return newRound;
    }

    /// <summary>
    /// Reset some arrays at the beginning of a new season.
    /// Corresponds to C: cup_reset.
    /// </summary>
    /// <param name="cup">The cup.</param>
    public static void CupReset(Cup cup)
    {
        cup.TeamNames = new List<string>();
        cup.Teams = new List<int>();
        cup.Fixtures = new List<Fixture>();
        cup.Bye = new List<int>();

        foreach (CupRound cupRound in cup.Rounds)
        {
            cupRound.Tables = new List<Table>();
            cupRound.Teams = new List<int>();
            cupRound.TeamPtrs = new List<int>();
        }
    }

    /// <summary>
    /// Find out whether a choose_team definition refers to a league in the country.
    /// If so, no teams get loaded for that choose_team.
    /// Corresponds to C: query_cup_choose_team_is_league.
    /// </summary>
    /// <param name="sid">The sid.</param>
    /// <returns>Whether the choose team is a league.</returns>
    public static bool QueryCupChooseTeamIsLeague(string sid)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Sid == sid)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Write the cup or league of the chooseteam into the appropriate pointer
    /// and return TRUE; return FALSE if no cup/league is found.
    /// Corresponds to C: cup_get_choose_team_league_cup.
    /// </summary>
    /// <param name="ct">The choose team.</param>
    /// <param name="league">The league.</param>
    /// <param name="cup">The cup.</param>
    public static void CupGetChooseTeamLeagueCup(CupChooseTeam ct, out League league, out Cup cup)
    {
        league = null;
        cup = null;

        string sid = ct.Sid;
        string prefix = sid.EndsWith("*") ? sid.Substring(0, sid.Length - 1) : "NONAME";
        int idx = -1;
        int numStart = 0;
        while (numStart < sid.Length && !char.IsDigit(sid[numStart]))
            numStart++;

        if (numStart < sid.Length)
        {
            int numEnd = numStart;
            while (numEnd < sid.Length && char.IsDigit(sid[numEnd]))
                numEnd++;

            int.TryParse(sid.Substring(numStart, numEnd - numStart), out idx);
        }

        if (sid.StartsWith("LEAGUE") && idx >= 0 && idx <= Variables.Country.Leagues.Count)
        {
            league = Variables.Country.Leagues[idx - 1];
            cup = null;
        }
        else if (sid.StartsWith("CUP") && idx >= 0 && idx <= Variables.Country.Cups.Count)
        {
            cup = Variables.Country.Cups[idx - 1];
            league = null;
        }
        else
        {
            foreach (League l in Variables.Country.Leagues)
            {
                if (l.Sid == sid || l.Sid.StartsWith(prefix, StringComparison.Ordinal))
                {
                    league = l;
                    cup = null;
                    break;
                }
            }

            foreach (Cup c in Variables.Country.Cups)
            {
                if (c.Sid == sid || c.Sid.StartsWith(prefix, StringComparison.Ordinal))
                {
                    cup = c;
                    league = null;
                    break;
                }
            }
        }

        if (league == null && cup == null)
            throw new System.Exception("CupGetChooseTeamLeagueCup: no league nor cup found for chooseteam " + ct.Sid);
    }

    /// <summary>
    /// Load the pointers to the teams participating in the cup round.
    /// If necessary, teams are generated and stored in the teams array of the cup round.
    /// Corresponds to C: cup_get_team_pointers.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The cup round.</param>
    /// <param name="teamsSorted">The sorted teams.</param>
    /// <param name="preload">Whether to preload.</param>
    public static void CupGetTeamPointers(Cup cup, int round, List<Team> teamsSorted, bool preload)
    {
        // Implementation depends on the not yet converted league and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the pointers to the teams (already generated, in one of the leagues or cups)
    /// specified in the chooseteam. Add them to the 'teams' pointer array.
    /// Corresponds to C: cup_load_choose_team.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="teams">The teams.</param>
    /// <param name="teamsSorted">The sorted teams.</param>
    /// <param name="ct">The choose team.</param>
    public static void CupLoadChooseTeam(Cup cup, List<Team> teams, List<Team> teamsSorted, CupChooseTeam ct)
    {
        // Implementation depends on the not yet converted league and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load the teams specified in the chooseteam from a cup.
    /// Corresponds to C: cup_load_choose_team_from_cup.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="cupTemp">The cup we choose the teams from.</param>
    /// <param name="teams">The teams.</param>
    /// <param name="teamsSorted">The sorted teams.</param>
    /// <param name="ct">The choose team.</param>
    public static void CupLoadChooseTeamFromCup(Cup cup, Cup cupTemp, List<Team> teams, List<Team> teamsSorted, CupChooseTeam ct)
    {
        // Implementation depends on the not yet converted league and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load the teams specified in the chooseteam from a league.
    /// Corresponds to C: cup_load_choose_team_from_league.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="league">The league.</param>
    /// <param name="teams">The teams.</param>
    /// <param name="ct">The choose team.</param>
    public static void CupLoadChooseTeamFromLeague(Cup cup, League league, List<Team> teams, CupChooseTeam ct)
    {
        // Implementation depends on the not yet converted league and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load the teams specified in the chooseteam from a league and generate new teams if necessary.
    /// Corresponds to C: cup_load_choose_team_generate.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="cupRound">The cup round.</param>
    /// <param name="ct">The choose team.</param>
    public static void CupLoadChooseTeamGenerate(Cup cup, CupRound cupRound, CupChooseTeam ct)
    {
        // Implementation depends on the not yet converted league, xml and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return a pointer array of teams ordered corresponding to their success in the cup.
    /// A bit tricky because we have to fetch the team pointers corresponding to their name because
    /// the team pointers in the fixtures are partially invalid because of promotion relegation.
    /// Corresponds to C: cup_get_teams_sorted.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>The sorted teams.</returns>
    public static List<Team> CupGetTeamsSorted(Cup cup)
    {
        List<Team> teams = new List<Team>();
        List<int> teamIds = new List<int>();

        foreach (Fixture fixture in cup.Fixtures)
        {
            foreach (Team team in fixture.Teams)
            {
                if (!teamIds.Contains(team.Id))
                {
                    teamIds.Add(team.Id);
                    teams.Add(team);
                }
            }
        }

        teams.Sort((tm1, tm2) => CupCompareSuccess(tm1, tm2, cup));

        return teams;
    }

    /// <summary>
    /// Compare two teams in a pointer array taking into account their success in a cup.
    /// Corresponds to C: cup_compare_success.
    /// </summary>
    /// <param name="tm1">The first team.</param>
    /// <param name="tm2">The second team.</param>
    /// <param name="cup">The cup.</param>
    /// <returns>The comparison result.</returns>
    public static int CupCompareSuccess(Team tm1, Team tm2, Cup cup)
    {
        int roundReached1 = CupGetRoundReached(tm1, cup.Fixtures);
        int roundReached2 = CupGetRoundReached(tm2, cup.Fixtures);
        int returnValue = 0;

        if (ReferenceEquals(tm1, tm2))
            returnValue = 0;
        else if (roundReached1 < roundReached2)
            returnValue = 1;
        else if (roundReached1 > roundReached2)
            returnValue = -1;
        else
        {
            CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[roundReached1] : new CupRound();

            if (cupRound.Tables.Count > 0)
                returnValue = CupCompareSuccessTables(tm1, tm2, cup, roundReached1);
            else
                returnValue = CupCompareSuccessKnockout(tm1, tm2, cup.Fixtures);
        }

        return returnValue;
    }

    /// <summary>
    /// Compare two teams that reached the same knockout round.
    /// Corresponds to C: cup_compare_success_knockout.
    /// </summary>
    /// <param name="tm1">The first team.</param>
    /// <param name="tm2">The second team.</param>
    /// <param name="fixtures">The fixtures of the cup.</param>
    /// <returns>The comparison result.</returns>
    public static int CupCompareSuccessKnockout(Team tm1, Team tm2, List<Fixture> fixtures)
    {
        int winner1 = -1;
        int winner2 = -1;
        int returnValue = 0;

        for (int i = fixtures.Count - 1; i >= 0; i--)
        {
            if (winner1 == -1 && Fixture.FixtureTeamInvolved(fixtures[i], tm1))
            {
                Team winner = Fixture.WinnerOf(fixtures[i], true);
                winner1 = winner != null ? winner.Id : -1;
            }

            if (winner2 == -1 && Fixture.FixtureTeamInvolved(fixtures[i], tm2))
            {
                Team winner = Fixture.WinnerOf(fixtures[i], true);
                winner2 = winner != null ? winner.Id : -1;
            }

            if (winner1 != -1 && winner2 != -1)
                break;
        }

        if (winner1 == tm1.Id && winner2 != tm2.Id)
            returnValue = -1;
        else if (winner1 != tm1.Id && winner2 == tm2.Id)
            returnValue = 1;

        return returnValue;
    }

    /// <summary>
    /// Compare two teams in cup tables.
    /// Corresponds to C: cup_compare_success_tables.
    /// </summary>
    /// <param name="tm1">The first team.</param>
    /// <param name="tm2">The second team.</param>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The cup round.</param>
    /// <returns>The comparison result.</returns>
    public static int CupCompareSuccessTables(Team tm1, Team tm2, Cup cup, int round)
    {
        int returnValue = 0;
        CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[round] : new CupRound();

        if (Team.GetCupRank(tm1, cupRound, true) > Team.GetCupRank(tm2, cupRound, true))
            returnValue = 1;
        else if (Team.GetCupRank(tm1, cupRound, true) < Team.GetCupRank(tm2, cupRound, true))
            returnValue = -1;
        else
        {
            TableElement element1 = null;
            TableElement element2 = null;

            foreach (Table table in cupRound.Tables)
            {
                foreach (TableElement element in table.Elements)
                {
                    if (element.TeamId == tm1.Id)
                        element1 = element;
                    else if (element.TeamId == tm2.Id)
                        element2 = element;
                }
            }

            returnValue = Table.TableElementCompareFunc(element1, element2, cup.Id);
        }

        return returnValue;
    }

    /// <summary>
    /// Return the cup round that the team reached in the cup.
    /// @param tm The team.
    /// @param fixtures The fixtures array of the cup.
    /// @return The cup round.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="fixtures">The fixtures.</param>
    /// <returns>The round reached.</returns>
    public static int CupGetRoundReached(Team tm, List<Fixture> fixtures)
    {
        int round = -1;

        foreach (Fixture fixture in fixtures)
        {
            if (fixture.TeamIds[0] == tm.Id || fixture.TeamIds[1] == tm.Id)
                round = System.Math.Max(round, fixture.Round);
        }

        return round;
    }

    /// <summary>
    /// Calculate the week number the first matchday of the given cup round takes place.
    /// @param cup The cup we examine.
    /// @param cupRound The index of the cup round in the cup.rounds array.
    /// @param withDelay Whether to take the delay into account.
    /// @return A week number.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="cupRound">The cup round.</param>
    /// <param name="withDelay">Whether to take the delay into account.</param>
    /// <returns>The first week.</returns>
    public static int CupGetFirstWeekOfCupRound(Cup cup, int cupRound, bool withDelay)
    {
        // Implementation depends on the not yet converted cup and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Calculate the last week of a cup if we only know the first week.
    /// Corresponds to C: cup_get_last_week_from_first.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="firstWeek">The first week.</param>
    /// <returns>The last week.</returns>
    public static int CupGetLastWeekFromFirst(Cup cup, int firstWeek)
    {
        int matchdays = 0;

        for (int i = 0; i < cup.Rounds.Count; i++)
        {
            matchdays += CupGetMatchdaysInCupRound(cup, i);
        }

        return firstWeek + (matchdays - 1) * cup.WeekGap;
    }

    /// <summary>
    /// Return the number of matchdays for a given cup round.
    /// @param cup The cup we examine.
    /// @param round The cup round.
    /// @return The number of matchdays, mostly 1 or 2.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The cup round.</param>
    /// <returns>The number of matchdays.</returns>
    public static int CupGetMatchdaysInCupRound(Cup cup, int round)
    {
        // Implementation depends on the not yet converted cup and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the number of teams playing in the given cup round.
    /// @param cup The cup we examine.
    /// @param round The cup round.
    /// @return The number of teams in the round.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The cup round.</param>
    /// <returns>The number of teams.</returns>
    public static int CupRoundGetNumberOfTeams(Cup cup, int round)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Get the number of byes in the given cup round.
    /// Corresponds to C: cup_round_get_byes.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The cup round.</param>
    /// <returns>The number of byes.</returns>
    public static int CupRoundGetByes(Cup cup, int round)
    {
        // Implementation depends on the not yet converted maths and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the number of new teams that come into the cup in the given cup round.
    /// Corresponds to C: cup_round_get_new_teams.
    /// </summary>
    /// <param name="cupRound">The cup round.</param>
    /// <returns>The number of new teams.</returns>
    public static int CupRoundGetNewTeams(CupRound cupRound)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the cup pointer belonging to the id.
    /// @param clid The id we look for.
    /// @return The cup pointer or NULL if failed.
    /// </summary>
    /// <param name="clid">The id.</param>
    /// <returns>The cup.</returns>
    public static Cup CupFromClid(int clid)
    {
        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id == clid)
                return cup;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Id == clid)
                return cup;
        }

        throw new System.Exception("CupFromClid: didn't find cup with id " + clid);
    }

    /// <summary>
    /// Find the cup with the given sid.
    /// Corresponds to C: cup_from_sid.
    /// </summary>
    /// <param name="sid">The sid.</param>
    /// <returns>The cup.</returns>
    public static Cup CupFromSid(string sid)
    {
        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Sid == sid)
                return cup;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Sid == sid)
                return cup;
        }

        throw new System.Exception("CupFromSid: didn't find cup with sid " + sid);
    }

    /// <summary>
    /// Return a standard cup round name.
    /// Corresponds to C: cup_get_round_name.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="round">The round.</param>
    /// <param name="buf">The buffer we write the name into.</param>
    public static void CupGetRoundName(Cup cup, int round, ref string buf)
    {
        CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[round] : new CupRound();

        if (cupRound.RoundRobinNumberOfGroups > 0)
        {
            buf = "Round robin";
            return;
        }

        int stages = cup.Rounds.Count - round;
        if (stages == 1)
            buf = "Final";
        else if (stages == 2)
            buf = "Semi-final";
        else if (stages == 3)
            buf = "Quarter-final";
        else
            buf = "Last " + System.Math.Pow(2, stages);
    }

    /// <summary>
    /// Return the round name of a fixture.
    /// Corresponds to C: cup_round_get_name.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="buf">The buffer we write the name into.</param>
    public static void CupRoundGetName(Fixture fix, ref string buf)
    {
        Cup cup = CupFromClid(fix.Clid);
        int round = fix.Round;
        CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[round] : new CupRound();

        if (cupRound.RoundRobinNumberOfGroups > 0)
        {
            buf = "Round robin";
            return;
        }

        int stages = cup.Rounds.Count - round;
        if (stages == 1)
            buf = "Final";
        else if (stages == 2)
            buf = "Semi-final";
        else if (stages == 3)
            buf = "Quarter-final";
        else
            buf = "Last " + System.Math.Pow(2, stages);
    }

    /// <summary>
    /// Find out whether the cup contains tables that can be displayed.
    /// Returns -1 if false and the number of the cup round with tables otherwise.
    /// Corresponds to C: cup_has_tables.
    /// </summary>
    /// <param name="clid">The id.</param>
    /// <returns>The cup round with tables or -1.</returns>
    public static int CupHasTables(int clid)
    {
        Cup cup = CupFromClid(clid);
        for (int i = cup.Rounds.Count - 1; i >= 0; i--)
        {
            if (cup.Rounds[i].Tables.Count > 0)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Return the team that won the cup.
    /// Corresponds to C: cup_get_winner.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>The winning team.</returns>
    public static Team CupGetWinner(Cup cup)
    {
        List<Team> teamsSorted = CupGetTeamsSorted(cup);
        Team tm = teamsSorted.Count > 0 ? teamsSorted[0] : null;
        return tm;
    }

    /// <summary>
    /// Find out whether a cup with add_week 1000 should begin now.
    /// Corresponds to C: query_cup_begins.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the cup begins.</returns>
    public static bool QueryCupBegins(Cup cup)
    {
        // Implementation depends on the not yet converted league, fixture and cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the number of international cups in the country.
    /// Corresponds to C: query_cup_transfer.
    /// </summary>
    /// <returns>Whether there are international cups.</returns>
    public static bool QueryCupTransfer()
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the cup has a highlight property and return the highlight colour.
    /// Corresponds to C: cup_get_highlight_colour.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>The highlight colour.</returns>
    public static string CupGetHighlightColour(Cup cup)
    {
        // Implementation depends on the not yet converted app functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check the cup fixtures for suspicious entries.
    /// Corresponds to C: cup_check_fixtures.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the fixtures are OK.</returns>
    public static bool CupCheckFixtures(Cup cup)
    {
        foreach (Fixture fixture in cup.Fixtures)
        {
            if (fixture.Teams[0] == fixture.Teams[1])
            {
                // debug_print_message(...);
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Check whether we have to wait for other cups before we can write fixtures for the cup round.
    /// Corresponds to C: cup_round_check_waits.
    /// </summary>
    /// <param name="cupRound">The cup round.</param>
    /// <returns>Whether we have to wait.</returns>
    public static bool CupRoundCheckWaits(CupRound cupRound)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the cup chooses teams from itself (e.g. the defending champion from last season).
    /// Corresponds to C: query_cup_self_referential.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the cup is self-referential.</returns>
    public static bool QueryCupSelfReferential(Cup cup)
    {
        foreach (CupRound cupRound in cup.Rounds)
        {
            foreach (CupChooseTeam chooseTeam in cupRound.ChooseTeams)
            {
                if (chooseTeam.Sid == cup.Sid)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Find out if the cup is part of the array of cups that are displayed in the game.
    /// Corresponds to C: query_cup_hidden.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the cup is hidden.</returns>
    public static bool QueryCupHidden(Cup cup)
    {
        foreach (Cup c in Variables.Country.Cups)
        {
            if (c == cup)
                return false;
        }

        foreach (Cup c in Variables.Country.AllCups)
        {
            if (c == cup)
                return false;
        }

        return true;
    }
}
