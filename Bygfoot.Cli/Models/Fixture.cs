namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Structure representing a fixture, or, in other words, a match. 
/// Corresponds to C struct Fixture.
/// </summary>
public class Fixture
{
    /// <summary>The cup or league the fixture belongs to. Corresponds to C: gint clid.</summary>
    public int Clid { get; set; }

    /// <summary>The unique id of the fixture. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>The round (in a cup) the fixture belongs to. Corresponds to C: gint round.</summary>
    public int Round { get; set; }

    /// <summary>The replay number (ie. how often the match was repeated because of a draw). Corresponds to C: gint replay_number.</summary>
    public int ReplayNumber { get; set; }

    /// <summary>When it takes place. Corresponds to C: gint week_number, week_round_number.</summary>
    public int WeekNumber { get; set; }
    public int WeekRoundNumber { get; set; }

    /// <summary>The teams involved. Corresponds to C: Team *teams[2].</summary>
    public List<Team> Teams { get; set; } = new List<Team>();

    /// <summary>Ids of the teams. Needed when the team pointers get invalid (e.g. after promotion/relegation). Corresponds to C: gint team_ids[2].</summary>
    public int[] TeamIds { get; set; } = new int[2];

    /// <summary>
    /// The number of goals for each team in regulation, extra time and penalty shoot-out. 
    /// Corresponds to C: gint result[2][3].
    /// </summary>
    public int[,] Result { get; set; } = new int[2, 3];

    /// <summary>Whether there's home advantage. Corresponds to C: gboolean home_advantage.</summary>
    public bool HomeAdvantage { get; set; }

    /// <summary>Whether this is the second leg. Corresponds to C: gboolean second_leg.</summary>
    public bool SecondLeg { get; set; }

    /// <summary>Whether the game has to be decided. Corresponds to C: gboolean decisive.</summary>
    public bool Decisive { get; set; }

    /// <summary>How many people attended. Corresponds to C: gint attendance.</summary>
    public int Attendance { get; set; }

    /// <summary>The live game used for the fixture calculation. Corresponds to C: gpointer live_game.</summary>
    public LiveGame LiveGame { get; set; }

    /// <summary>
    /// Write the fixtures for the given league at the beginning of a new season.
    /// Corresponds to C: fixture_write_league_fixtures.
    /// </summary>
    /// <param name="league">The league we write the fixtures for.</param>
    public static void WriteLeagueFixtures(League league)
    {
        int maxRr = league.RoundRobins;
        List<Team> teams = Team.GetPointersFromArray(league.Teams, null);

        foreach (JoinedLeague joined in league.JoinedLeagues)
        {
            League joinedLeague = FindLeagueBySid(joined.Sid);
            if (joinedLeague == null)
                continue;

            if (joined.Rr > maxRr)
                maxRr = joined.Rr;

            teams = Team.GetPointersFromArray(joinedLeague.Teams, teams);
        }

        int i = 0;
        while (i < maxRr)
        {
            bool oneRound = i == maxRr - 1;
            WriteRoundRobin(league, -1, teams, oneRound, league.FirstWeek, league.RrBreaks, i - 1);
            i += (i < maxRr - 1) ? 2 : 1;
        }

        teams.Clear();

        league.Fixtures.Sort((fix1, fix2) =>
        {
            if (fix1.WeekNumber != fix2.WeekNumber)
                return fix1.WeekNumber < fix2.WeekNumber ? -1 : 1;
            if (fix1.WeekRoundNumber != fix2.WeekRoundNumber)
                return fix1.WeekRoundNumber < fix2.WeekRoundNumber ? -1 : 1;
            return 0;
        });

        if (league.JoinedLeagues.Count > 0)
        {
            for (int j = 0; j < league.JoinedLeagues.Count; j++)
            {
                League joinedLeague = FindLeagueBySid(league.JoinedLeagues[j].Sid);
                if (joinedLeague == null)
                    continue;

                if (league.JoinedLeagues[j].Rr < maxRr)
                    RemoveRrs(league.Fixtures, league.Id, joinedLeague.Id, maxRr - league.JoinedLeagues[j].Rr);

                for (int k = j; k < league.JoinedLeagues.Count; k++)
                {
                    League joinedLeagueTwo = FindLeagueBySid(league.JoinedLeagues[k].Sid);
                    if (joinedLeagueTwo == null)
                        continue;

                    RemoveRrs(league.Fixtures, joinedLeague.Id, joinedLeagueTwo.Id, maxRr);
                }
            }
        }

        if (league.RoundRobins < maxRr)
            RemoveRrs(league.Fixtures, league.Id, league.Id, maxRr - league.RoundRobins);

        FixturesCondense(league.Fixtures);
    }

    /// <summary>
    /// Write the fixtures for the given cup at the beginning of a new season.
    /// Corresponds to C: fixture_write_cup_fixtures.
    /// </summary>
    /// <param name="cup">The cup we write the fixtures for.</param>
    /// <returns>Whether the fixtures were written successfully.</returns>
    public static bool WriteCupFixtures(Cup cup)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Update the fixtures for the given cup.
    /// Corresponds to C: fixture_update.
    /// </summary>
    /// <param name="cup">The cup we update.</param>
    public static void Update(Cup cup)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write replay matches for the cup.
    /// Corresponds to C: fixture_update_write_replays.
    /// </summary>
    /// <param name="cup">The cup we update.</param>
    /// <returns>Whether fixtures were written, FALSE otherwise.</returns>
    public static bool UpdateWriteReplays(Cup cup)
    {
        // Implementation depends on the not yet converted cup functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the teams that advance to the next cup round.
    /// Corresponds to C: fixture_get_cup_round_winners.
    /// </summary>
    /// <param name="cup">The cup the fixtures belong to.</param>
    /// <returns>The teams that advance.</returns>
    public static List<Team> GetCupRoundWinners(Cup cup)
    {
        List<Team> array = new List<Team>();
        List<Fixture> fixtures = cup.Fixtures;
        int round = fixtures.Count > 0 ? fixtures[fixtures.Count - 1].Round : 0;
        CupRound cupround = cup.Rounds.Count > 0 ? cup.Rounds[round] : new CupRound();

        if (cupround.RoundRobinNumberOfGroups > 0)
            return GetRoundRobinAdvance(cup, round);

        if (cupround.Replay != 0)
        {
            for (int i = 0; i < fixtures.Count; i++)
            {
                Fixture fixture = fixtures[i];
                if (fixture.Round == round &&
                    (fixture.Result[0, 0] != fixture.Result[1, 0] ||
                     Sum(fixture.Result[0, 0], fixture.Result[0, 1], fixture.Result[0, 2]) <
                     Sum(fixture.Result[1, 0], fixture.Result[1, 1], fixture.Result[1, 2])))
                    array.Add(WinnerOf(fixture, false));
            }

            return array;
        }

        if (!cupround.HomeAway)
        {
            for (int i = 0; i < fixtures.Count; i++)
            {
                Fixture fixture = fixtures[i];
                if (fixture.Round == round)
                    array.Add(WinnerOf(fixture, false));
            }

            return array;
        }

        for (int i = 0; i < fixtures.Count; i++)
        {
            Fixture fixture = fixtures[i];
            if (fixture.Round == round && fixture.SecondLeg)
                array.Add(WinnerOf(fixture, false));
        }

        return array;
    }

    /// <summary>
    /// Return an array of teams advancing from a round robin stage of a cup.
    /// Corresponds to C: fixture_get_round_robin_advance.
    /// </summary>
    /// <param name="cup">The cup the fixtures belong to.</param>
    /// <param name="round">The cup round.</param>
    /// <returns>The teams that advance.</returns>
    public static List<Team> GetRoundRobinAdvance(Cup cup, int round)
    {
        CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[round] : new CupRound();

        List<Team> teams = Cup.CupGetTeamsSorted(cup);
        int remove = teams.Count - (cupRound.RoundRobinNumberOfGroups * cupRound.RoundRobinNumberOfAdvance) - cupRound.RoundRobinNumberOfBestAdvance;

        for (int i = 0; i < remove; i++)
            teams.RemoveAt(teams.Count - 1);

        return teams;
    }

    /// <summary>
    /// Return the pointer of the team that won the encounter.
    /// Corresponds to C: fixture_winner_of.
    /// </summary>
    /// <param name="fix">The fixture we examine.</param>
    /// <param name="teamId">Whether to return a team pointer or the team id of the winner.</param>
    /// <returns>A team pointer or a coded integer.</returns>
    public static Team WinnerOf(Fixture fix, bool teamId)
    {
        int winnerIdx = -1;
        Fixture firstLeg = null;
        CupRound cupround = null;

        if (fix.Clid < Bygfoot.IdCupStart)
        {
            winnerIdx = fix.Result[0, 0] < fix.Result[1, 0] ? 1 : 0;
            if (teamId)
                return Team.OfId(fix.TeamIds[winnerIdx]);
            return fix.Teams[winnerIdx];
        }

        Cup cup = FindCupById(fix.Clid);
        if (cup == null)
            return null;

        cupround = cup.Rounds.Count > 0 ? cup.Rounds[fix.Round] : new CupRound();

        if (cupround.Replay != 0 ||
            !cupround.HomeAway ||
            cupround.RoundRobinNumberOfGroups > 0)
        {
            winnerIdx = Sum(fix.Result[0, 0], fix.Result[0, 1], fix.Result[0, 2]) <
                        Sum(fix.Result[1, 0], fix.Result[1, 1], fix.Result[1, 2]) ? 1 : 0;
        }
        else
        {
            firstLeg = GetFirstLeg(fix, false);

            if (fix.Result[0, 2] + fix.Result[1, 2] != 0)
                winnerIdx = fix.Result[0, 2] < fix.Result[1, 2] ? 1 : 0;
            else if (fix.Result[0, 0] + fix.Result[0, 1] +
                     firstLeg.Result[1, 0] + firstLeg.Result[1, 1] !=
                     fix.Result[1, 0] + fix.Result[1, 1] +
                     firstLeg.Result[0, 0] + firstLeg.Result[0, 1])
            {
                winnerIdx = fix.Result[0, 0] + fix.Result[0, 1] +
                            firstLeg.Result[1, 0] + firstLeg.Result[1, 1] <
                            fix.Result[1, 0] + fix.Result[1, 1] +
                            firstLeg.Result[0, 0] + firstLeg.Result[0, 1] ? 1 : 0;
            }
            else
            {
                winnerIdx = fix.Result[1, 0] + fix.Result[1, 1] >
                            firstLeg.Result[1, 0] + firstLeg.Result[1, 1] ? 1 : 0;
            }
        }

        if (teamId)
            return Team.OfId(fix.TeamIds[winnerIdx]);
        return fix.Teams[winnerIdx];
    }

    /// <summary>
    /// Write round robins for the teams in the array.
    /// Corresponds to C: fixture_write_round_robin.
    /// </summary>
    /// <param name="leagueCup">The league or cup the fixtures belong to.</param>
    /// <param name="cupRound">The round of the cup the fixtures belong to or -1 if it's a league.</param>
    /// <param name="teams">The pointer array containing pointers to the teams.</param>
    /// <param name="oneRound">Whether a team plays each other team twice or only once.</param>
    /// <param name="firstWeek">The first week the fixtures should be written at.</param>
    /// <param name="rrBreaks">The number of weeks between the parts of a round robin.</param>
    /// <param name="rrBreakIdx">The index of the break in the breaks array.</param>
    public static void WriteRoundRobin(object leagueCup, int cupRound, List<Team> teams, bool oneRound, int firstWeek, List<int> rrBreaks, int rrBreakIdx)
    {
        League league = null;
        Cup cup = null;
        List<Fixture> fixtures = null;
        List<int>[] twoMatchWeeks = null;
        int weekGap;
        int weekNumber;
        int weekRoundNumber;
        int clid;
        int firstFixture;
        bool homeAdvantage;
        bool oddFixtures = false;
        Team teamTemp = null;
        int len = teams.Count;

        teams = Misc.MiscRandomiseGPointerArray(teams);

        if (cupRound == -1)
        {
            league = (League)leagueCup;
            fixtures = league.Fixtures;
            twoMatchWeeks = league.TwoMatchWeeks;
            clid = league.Id;
            firstWeek = fixtures.Count == 0 ? league.FirstWeek : fixtures[fixtures.Count - 1].WeekNumber + rrBreaks[rrBreakIdx];
            weekGap = league.WeekGap;
            homeAdvantage = true;
        }
        else
        {
            cup = (Cup)leagueCup;
            if (firstWeek == -1)
                firstWeek = GetFirstWeekOfCupRound(cup, cupRound);
            weekGap = cup.WeekGap;
            fixtures = cup.Fixtures;
            twoMatchWeeks = cup.Rounds[cupRound].TwoMatchWeeks;
            clid = cup.Id;
            homeAdvantage = !cup.Rounds[cupRound].Neutral;
        }

        firstFixture = fixtures.Count;

        if (firstWeek < 1)
            throw new System.Exception("Fixture write error: first week of " + clid + " is not positive (" + firstWeek + ").");

        if (len % 2 != 0)
        {
            teamTemp = Team.New(false);
            teams.Add(teamTemp);
            oddFixtures = true;
            len++;
        }

        weekNumber = League.LeagueCupGetWeekWithBreak(clid, firstWeek);

        for (int i = 0; i < len - 1; i++)
        {
            if (i > 0 && !League.QueryLeagueCupMatchdayInTwoMatchWeek(twoMatchWeeks, CountMatchdays(fixtures) + 1))
                weekNumber = League.LeagueCupGetWeekWithBreak(clid, weekNumber + weekGap);

            WriteRoundRobinMatchday(fixtures, cupRound, teams, i, weekNumber, clid, homeAdvantage);
        }

        if (!oneRound)
        {
            weekNumber = League.LeagueCupGetWeekWithBreak(clid, weekNumber + rrBreaks[rrBreakIdx + 1]);

            for (int i = 0; i < len - 1; i++)
            {
                if (i > 0 && !League.QueryLeagueCupMatchdayInTwoMatchWeek(twoMatchWeeks, CountMatchdays(fixtures) + 1))
                    weekNumber = League.LeagueCupGetWeekWithBreak(clid, weekNumber + weekGap);

                weekRoundNumber = GetFreeRound(weekNumber, teams, -1, -1);

                for (int j = 0; j < len / 2; j++)
                {
                    Fixture fixture = fixtures[firstFixture + i * (len / 2) + j];
                    WriteFixture(fixtures, fixture.Teams[1], fixture.Teams[0], weekNumber, weekRoundNumber, clid, cupRound, 0, homeAdvantage, false, false);
                }
            }
        }

        teams.Clear();

        if (oddFixtures)
        {
            for (int j = fixtures.Count - 1; j >= 0; j--)
            {
                Fixture fixture = fixtures[j];
                if (fixture.TeamIds[0] == -1 || fixture.TeamIds[1] == -1)
                    fixtures.RemoveAt(j);
            }
        }
    }

    /// <summary>
    /// Write one matchday of round robin games.
    /// Corresponds to C: fixture_write_round_robin_matchday.
    /// </summary>
    /// <param name="fixtures">The fixture array we add the fixtures to.</param>
    /// <param name="cupRound">The round of the cup the fixtures belong to or -1 if it's a league.</param>
    /// <param name="teams">The array containing the pointers to the teams participating.</param>
    /// <param name="special">The special team.</param>
    /// <param name="weekNumber">The week the matchday takes place.</param>
    /// <param name="clid">The id of the league or cup.</param>
    /// <param name="homeAdvantage">Whether there's home advantage.</param>
    public static void WriteRoundRobinMatchday(List<Fixture> fixtures, int cupRound, List<Team> teams, int special, int weekNumber, int clid, bool homeAdvantage)
    {
        int len = teams.Count / 2;
        Team[] home = new Team[len];
        Team[] away = new Team[len];
        int weekRoundNumber = GetFreeRound(weekNumber, teams, -1, -1);

        home[0] = teams[len * 2 - 1];
        away[0] = teams[special];

        for (int i = 1; i < len; i++)
        {
            home[i] = teams[(special + i) % (len * 2 - 1)];
            away[i] = teams[(special + i + (len - 1 - i) * 2 + 1) % (len * 2 - 1)];
        }

        if (special % 2 != 0)
        {
            for (int i = 0; i < len; i++)
            {
                Team temp = home[i];
                home[i] = away[i];
                away[i] = temp;
            }
        }

        for (int i = 0; i < len; i++)
        {
            WriteFixture(fixtures, home[i], away[i], weekNumber, weekRoundNumber, clid, cupRound, 0, homeAdvantage, false, false);
        }
    }

    /// <summary>
    /// Write fixtures for a knockout round, e.g. home/away games.
    /// Corresponds to C: fixture_write_knockout_round.
    /// </summary>
    /// <param name="cup">The cup the fixtures belong to.</param>
    /// <param name="cupRound">The index of the cup round.</param>
    /// <param name="teams">The teams participating.</param>
    public static void WriteKnockoutRound(Cup cup, int cupRound, List<Team> teams)
    {
        // Implementation depends on the not yet converted league and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write a fixture and append it to a fixture array.
    /// Corresponds to C: fixture_write.
    /// </summary>
    /// <param name="fixtures">The fixture array we add the fixtures to.</param>
    /// <param name="homeTeam">The pointer of the home team.</param>
    /// <param name="awayTeam">The pointer of the away team.</param>
    /// <param name="weekNumber">The week it takes place.</param>
    /// <param name="weekRoundNumber">The week round it takes place.</param>
    /// <param name="clid">The id of the league or cup.</param>
    /// <param name="cupRound">The index of the cup round or -1 if it's a league.</param>
    /// <param name="replayNumber">Whether this is a replay match and how 'manyth' it is.</param>
    /// <param name="homeAdvantage">Whether there's home advantage.</param>
    /// <param name="secondLeg">Whether this is the second leg of a round.</param>
    /// <param name="decisive">Whether the match has to have a winner.</param>
    public static void WriteFixture(List<Fixture> fixtures, Team homeTeam, Team awayTeam, int weekNumber, int weekRoundNumber, int clid, int cupRound, int replayNumber, bool homeAdvantage, bool secondLeg, bool decisive)
    {
        Fixture newFixture = new Fixture();
        newFixture.Id = Bygfoot.FixtureIdNew();
        newFixture.Clid = clid;
        newFixture.Round = cupRound;
        newFixture.ReplayNumber = replayNumber;
        newFixture.WeekNumber = weekNumber;
        newFixture.WeekRoundNumber = weekRoundNumber;
        newFixture.Teams = new List<Team> { homeTeam, awayTeam };
        newFixture.TeamIds = new int[] { homeTeam.Id, awayTeam.Id };
        newFixture.Result = new int[2, 3];
        newFixture.HomeAdvantage = homeAdvantage;
        newFixture.SecondLeg = secondLeg;
        newFixture.Decisive = decisive;
        newFixture.Attendance = -1;
        newFixture.LiveGame = null;
        fixtures.Add(newFixture);
    }

    /// <summary>
    /// Return the round of the week which doesn't contain any matches yet featuring teams from the team list or with the given ids.
    /// Corresponds to C: fixture_get_free_round.
    /// </summary>
    /// <param name="weekNumber">The week we examine.</param>
    /// <param name="teams">The participating in the round or NULL.</param>
    /// <param name="teamId1">Id of the team1.</param>
    /// <param name="teamId2">Id of the team2.</param>
    /// <returns>A round number. If the clid belongs to a league this is always 1.</returns>
    public static int GetFreeRound(int weekNumber, List<Team> teams, int teamId1, int teamId2)
    {
        int maxRound = 0;

        foreach (League league in Variables.Country.Leagues)
        {
            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber &&
                    (Variables.Status[(int)Status0Value.StatusGenerateTeams] != 0 || fixture.Attendance == -1) &&
                    ((teams == null && (fixture.TeamIds[0] == teamId1 || fixture.TeamIds[1] == teamId1 || fixture.TeamIds[0] == teamId2 || fixture.TeamIds[1] == teamId2)) ||
                     (teams != null && (Team.QueryTeamIsInTeamsArray(fixture.Teams[0], teams) || Team.QueryTeamIsInTeamsArray(fixture.Teams[1], teams)))) &&
                    fixture.WeekRoundNumber > maxRound)
                    maxRound = fixture.WeekRoundNumber;
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber &&
                    (Variables.Status[(int)Status0Value.StatusGenerateTeams] != 0 || fixture.Attendance == -1) &&
                    ((teams == null && (fixture.TeamIds[0] == teamId1 || fixture.TeamIds[1] == teamId1 || fixture.TeamIds[0] == teamId2 || fixture.TeamIds[1] == teamId2)) ||
                     (teams != null && (Team.QueryTeamIsInTeamsArray(fixture.Teams[0], teams) || Team.QueryTeamIsInTeamsArray(fixture.Teams[1], teams)))) &&
                    fixture.WeekRoundNumber > maxRound)
                    maxRound = fixture.WeekRoundNumber;
            }
        }

        return maxRound + 1;
    }

    /// <summary>
    /// Check whether the date of a fixture is earlier than the date of a second one.
    /// Corresponds to C: query_fixture_is_earlier.
    /// </summary>
    /// <param name="fix1">The first fixture.</param>
    /// <param name="fix2">The second fixture.</param>
    /// <returns>Whether fix1 is earlier than fix2, FALSE otherwise.</returns>
    public static bool QueryFixtureIsEarlier(Fixture fix1, Fixture fix2)
    {
        return fix1.WeekNumber < fix2.WeekNumber ||
               (fix1.WeekNumber == fix2.WeekNumber && fix1.WeekRoundNumber < fix2.WeekRoundNumber);
    }

    /// <summary>
    /// Check whether the date of a fixture is later than the date of a second one.
    /// Corresponds to C: query_fixture_is_later.
    /// </summary>
    /// <param name="fix1">The first fixture.</param>
    /// <param name="fix2">The second fixture.</param>
    /// <returns>Whether fix1 is later than fix2, FALSE otherwise.</returns>
    public static bool QueryFixtureIsLater(Fixture fix1, Fixture fix2)
    {
        return fix1.WeekNumber > fix2.WeekNumber ||
               (fix1.WeekNumber == fix2.WeekNumber && fix1.WeekRoundNumber > fix2.WeekRoundNumber);
    }

    /// <summary>
    /// Find out whether the current result of the fixture is a draw.
    /// Corresponds to C: query_fixture_is_draw.
    /// </summary>
    /// <param name="fix">The pointer to the fixture.</param>
    /// <returns>TRUE or FALSE.</returns>
    public static bool QueryFixtureIsDraw(Fixture fix)
    {
        if (!fix.Decisive)
            return false;

        if (!fix.SecondLeg)
            return fix.Result[0, 0] + fix.Result[0, 1] == fix.Result[1, 0] + fix.Result[1, 1];

        Fixture firstLeg = GetFirstLeg(fix, false);

        return fix.Result[0, 1] + fix.Result[1, 1] == 0 &&
               fix.Result[0, 0] == firstLeg.Result[0, 0] &&
               fix.Result[1, 0] == firstLeg.Result[1, 0];
    }

    /// <summary>
    /// Check whether a user's team participates in the fixture.
    /// Corresponds to C: fixture_user_team_involved.
    /// </summary>
    /// <param name="fix">The fixture we check.</param>
    /// <returns>The index of the user or -1.</returns>
    public static int UserTeamInvolved(Fixture fix)
    {
        int userIndex = Team.IsUser(fix.Teams[0]);
        if (userIndex != -1)
            return userIndex;

        userIndex = Team.IsUser(fix.Teams[1]);
        if (userIndex != -1)
            return userIndex;

        return -1;
    }

    /// <summary>
    /// Check whether we have to update a table after calculating the fixture.
    /// Corresponds to C: query_fixture_has_tables.
    /// </summary>
    /// <param name="fix">The fixture that got calculated.</param>
    /// <returns>TRUE or FALSE.</returns>
    public static bool QueryFixtureHasTables(Fixture fix)
    {
        return fix.Clid < Bygfoot.IdCupStart || Cup.CupHasTables(fix.Clid) == fix.Round;
    }

    /// <summary>
    /// Find out whether there were games in the specified league or cup at the specified time.
    /// Corresponds to C: query_fixture_in_week_round.
    /// </summary>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round.</param>
    /// <returns>TRUE or FALSE.</returns>
    public static bool QueryFixtureInWeekRound(int clid, int weekNumber, int weekRoundNumber)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != clid)
                continue;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    return true;
            }

            return false;
        }

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id != clid)
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    return true;
            }

            return false;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Id != clid)
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    return true;
            }

            return false;
        }

        return false;
    }

    /// <summary>
    /// Return a pointer to the first leg going with the fixture.
    /// Corresponds to C: fixture_get_first_leg.
    /// </summary>
    /// <param name="fix">The second leg fixture.</param>
    /// <param name="silent">Whether to complain about a missing first leg.</param>
    /// <returns>A fixture pointer or NULL if failed.</returns>
    public static Fixture GetFirstLeg(Fixture fix, bool silent)
    {
        if (fix.Clid < Bygfoot.IdCupStart)
            return null;

        Cup cup = FindCupById(fix.Clid);
        if (cup == null)
            return null;

        CupRound cupRound = cup.Rounds.Count > 0 ? cup.Rounds[fix.Round] : new CupRound();
        if (cupRound.RoundRobinNumberOfGroups > 0)
            return null;

        Fixture firstLeg = null;

        foreach (Fixture fixture in cup.Fixtures)
        {
            if (fixture.Round == fix.Round &&
                fixture.TeamIds[0] == fix.TeamIds[1] &&
                fixture.TeamIds[1] == fix.TeamIds[0])
                firstLeg = fixture;
        }

        if (firstLeg == null && !silent)
            Debug.DebugPrintMessage("fixture_get_first_leg: didn't find first leg match; cup " + cup.Name + " round " + fix.Round);

        return firstLeg;
    }

    /// <summary>
    /// Return a list of fixture pointers.
    /// Corresponds to C: fixture_get_week_list_clid.
    /// </summary>
    /// <param name="clid">The cup or league id.</param>
    /// <param name="weekNumber">The week number of the fixtures.</param>
    /// <param name="weekRoundNumber">The week round of the fixtures.</param>
    /// <returns>The list of fixtures.</returns>
    public static List<Fixture> GetWeekListClid(int clid, int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixturesArray = new List<Fixture>();

        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != clid)
                continue;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    fixturesArray.Add(fixture);
            }

            return fixturesArray;
        }

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id != clid)
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    fixturesArray.Add(fixture);
            }

            return fixturesArray;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Id != clid)
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    fixturesArray.Add(fixture);
            }

            return fixturesArray;
        }

        return fixturesArray;
    }

    /// <summary>
    /// Return an array with all fixtures in a given week.
    /// Corresponds to C: fixture_get_week_list.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round.</param>
    /// <returns>The list of fixtures.</returns>
    public static List<Fixture> GetWeekList(int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixtures = new List<Fixture>();

        foreach (League league in Variables.Country.Leagues)
        {
            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    fixtures.Add(fixture);
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    fixtures.Add(fixture);
            }
        }

        return fixtures;
    }

    /// <summary>
    /// Print the result of the fixture into a buffer.
    /// Corresponds to C: fixture_result_to_buf.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="buf">The buffer.</param>
    /// <param name="swap">Whether to swap the two teams' goals.</param>
    public static void ResultToBuf(Fixture fix, ref string buf, bool swap)
    {
        int idx0 = 0;
        int idx1 = 1;

        if (swap)
        {
            idx0 = 1;
            idx1 = 0;
        }

        if (fix.Attendance < 0)
        {
            buf = "-- : --";
            return;
        }

        string localBuf = Sum(fix.Result[idx0, 0], fix.Result[idx0, 1], fix.Result[idx0, 2]) + " - " +
                           Sum(fix.Result[idx1, 0], fix.Result[idx1, 1], fix.Result[idx1, 2]);

        if (fix.Result[idx0, 2] + fix.Result[idx1, 2] != 0)
            localBuf += " p.";
        else if (fix.Result[idx0, 1] + fix.Result[idx1, 1] != 0)
            localBuf += " e.t.";

        if (fix.SecondLeg)
        {
            Fixture firstLeg = GetFirstLeg(fix, false);
            buf = localBuf + " (" + firstLeg.Result[idx1, 0] + " - " + firstLeg.Result[idx0, 0] + ")";
        }
        else
        {
            buf = localBuf;
        }
    }

    /// <summary>
    /// Return the number of fixtures in a given week round.
    /// Corresponds to C: fixture_get_number_of_matches.
    /// </summary>
    /// <param name="weekNumber">The week number the fixtures should have.</param>
    /// <param name="weekRoundNumber">The week round number the fixtures should have.</param>
    /// <returns>The number of matches.</returns>
    public static int GetNumberOfMatches(int weekNumber, int weekRoundNumber)
    {
        int sum = 0;

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                    sum++;
            }
        }

        if (weekRoundNumber == 1)
        {
            foreach (League league in Variables.Country.Leagues)
            {
                foreach (Fixture fixture in league.Fixtures)
                {
                    if (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber == weekRoundNumber)
                        sum++;
                }
            }
        }

        return sum;
    }

    /// <summary>
    /// Return a fixture from the following match-day.
    /// Corresponds to C: fixture_get_next.
    /// </summary>
    /// <param name="clid">The cup/league id.</param>
    /// <param name="weekNumber">The current week.</param>
    /// <param name="weekRoundNumber">The current round.</param>
    /// <returns>A fixture pointer.</returns>
    public static Fixture GetNext(int clid, int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixtures = GetFixturesForClid(clid);

        foreach (Fixture fixture in fixtures)
        {
            if (fixture.WeekNumber > weekNumber ||
                (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber > weekRoundNumber))
                return fixture;
        }

        if (fixtures.Count > 0)
            return fixtures[0];

        return null;
    }

    /// <summary>
    /// Return a fixture from the previous match-day.
    /// Corresponds to C: fixture_get_previous.
    /// </summary>
    /// <param name="clid">The cup/league id.</param>
    /// <param name="weekNumber">The current week.</param>
    /// <param name="weekRoundNumber">The current round.</param>
    /// <returns>A fixture pointer.</returns>
    public static Fixture GetPrevious(int clid, int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixtures = GetFixturesForClid(clid);

        for (int i = fixtures.Count - 1; i >= 0; i--)
        {
            Fixture fixture = fixtures[i];
            if (fixture.WeekNumber < weekNumber ||
                (fixture.WeekNumber == weekNumber && fixture.WeekRoundNumber < weekRoundNumber))
                return fixture;
        }

        if (fixtures.Count > 0)
            return fixtures[fixtures.Count - 1];

        return null;
    }

    /// <summary>
    /// Find a certain fixture for displaying in the right treeview.
    /// Corresponds to C: fixture_get.
    /// </summary>
    /// <param name="type">Type telling us how to look for the fixture (@see #ShowType)</param>
    /// <param name="clid">The id of the current league/cup we're showing.</param>
    /// <param name="weekNumber">The week we're showing.</param>
    /// <param name="weekRoundNumber">The round we're showing.</param>
    /// <param name="tm">A team pointer (for the case SHOW_TEAM).</param>
    /// <returns>A fixture pointer or NULL.</returns>
    public static Fixture Get(int type, int clid, int weekNumber, int weekRoundNumber, Team tm)
    {
        Fixture fix = null;

        if (type == (int)ShowType.ShowTeam)
        {
            fix = Team.GetFixture(tm, true);
            if (fix == null)
                fix = Team.GetFixture(tm, false);
        }
        else if (type == (int)ShowType.ShowCurrent)
        {
            fix = League.LeagueCupGetPreviousFixture(clid, weekNumber, weekRoundNumber);
            if (fix == null)
                fix = League.LeagueCupGetNextFixture(clid, weekNumber, weekRoundNumber);
        }
        else if (type == (int)ShowType.ShowNext)
            fix = GetNext(clid, weekNumber, weekRoundNumber);
        else if (type == (int)ShowType.ShowPrevious)
            fix = GetPrevious(clid, weekNumber, weekRoundNumber);
        else if (type == (int)ShowType.ShowNextLeague || type == (int)ShowType.ShowPreviousLeague)
        {
            int newClid = type == (int)ShowType.ShowNextLeague
                ? League.LeagueCupGetNextClid(clid, false)
                : League.LeagueCupGetPreviousClid(clid, false);

            fix = Get((int)ShowType.ShowCurrent, newClid, weekNumber, weekRoundNumber, null);
        }

        if (fix == null)
            Debug.DebugPrintMessage("fixture_get: no fixture found for type " + type + " clid " + clid + " week " + weekNumber + " round " + weekRoundNumber);

        return fix;
    }

    /// <summary>
    /// Compare two fixtures.
    /// Corresponds to C: fixture_compare_func.
    /// </summary>
    /// <param name="fix1">The first fixture.</param>
    /// <param name="fix2">The second fixture.</param>
    /// <param name="data">The data to compare.</param>
    /// <returns>The comparison result.</returns>
    public static int CompareFunc(Fixture fix1, Fixture fix2, object data)
    {
        int localData = data is int intData ? intData : 0;
        int type = localData % 100;
        int returnValue = 0;
        int compareType = (int)FixtureCompare.Date;

        if (type == compareType)
        {
            if (QueryFixtureIsEarlier(fix1, fix2))
                returnValue = -1;
            else
                returnValue = 1;
        }

        return returnValue;
    }

    /// <summary>
    /// Return an array with the last fixtures of the team.
    /// Corresponds to C: fixture_get_latest.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="withCups">Whether to include the international cup fixtures.</param>
    /// <returns>The list of latest fixtures.</returns>
    public static List<Fixture> GetLatest(Team tm, bool withCups)
    {
        List<Fixture> latest = new List<Fixture>();

        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != tm.Clid)
                continue;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.Attendance == -1)
                    break;
                if (fixture.Teams[0] == tm || fixture.Teams[1] == tm)
                    latest.Add(fixture);
            }
        }

        if (withCups)
        {
            foreach (Cup cup in Variables.Country.AllCups)
            {
                foreach (Fixture fixture in cup.Fixtures)
                {
                    if (fixture.Attendance == -1)
                        break;
                    if (fixture.Teams[0] == tm || fixture.Teams[1] == tm)
                        latest.Add(fixture);
                }
            }
        }

        latest.Sort((fix1, fix2) => CompareFunc(fix1, fix2, 0));

        return latest;
    }

    /// <summary>
    /// Return an array with the next fixtures of the team.
    /// Corresponds to C: fixture_get_coming.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The list of coming fixtures.</returns>
    public static List<Fixture> GetComing(Team tm)
    {
        List<Fixture> coming = new List<Fixture>();

        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != tm.Clid)
                continue;

            for (int j = league.Fixtures.Count - 1; j >= 0; j--)
            {
                Fixture fixture = league.Fixtures[j];
                if (fixture.Attendance != -1)
                    break;
                if (fixture.Teams[0] == tm || fixture.Teams[1] == tm)
                    coming.Add(fixture);
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            for (int j = cup.Fixtures.Count - 1; j >= 0; j--)
            {
                Fixture fixture = cup.Fixtures[j];
                if (fixture.Attendance != -1)
                    break;
                if (fixture.Teams[0] == tm || fixture.Teams[1] == tm)
                    coming.Add(fixture);
            }
        }

        coming.Sort((fix1, fix2) => CompareFunc(fix1, fix2, 0));

        return coming;
    }

    /// <summary>
    /// Return a pointer array with all the matches featuring the two teams.
    /// Corresponds to C: fixture_get_matches.
    /// </summary>
    /// <param name="tm1">The first team.</param>
    /// <param name="tm2">The second team.</param>
    /// <returns>The list of matches.</returns>
    public static List<Fixture> GetMatches(Team tm1, Team tm2)
    {
        List<Fixture> matches = new List<Fixture>();

        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != tm1.Clid)
                continue;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.Attendance == -1)
                    break;
                if ((fixture.Teams[0] == tm1 && fixture.Teams[1] == tm2) ||
                    (fixture.Teams[0] == tm2 && fixture.Teams[1] == tm1))
                    matches.Add(fixture);
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.Attendance == -1)
                    break;
                if ((fixture.Teams[0] == tm1 && fixture.Teams[1] == tm2) ||
                    (fixture.Teams[0] == tm2 && fixture.Teams[1] == tm1))
                    matches.Add(fixture);
            }
        }

        return matches;
    }

    /// <summary>
    /// Return the fixture going with the id.
    /// Corresponds to C: fixture_from_id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="abortProgram">Whether to abort the program.</param>
    /// <returns>The fixture.</returns>
    public static Fixture FromId(int id, bool abortProgram)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.Id == id)
                    return fixture;
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.Id == id)
                    return fixture;
            }
        }

        if (abortProgram)
            throw new System.Exception("Fixture not found: " + id);

        return null;
    }

    /// <summary>
    /// Return the last week round number of the specified week.
    /// Corresponds to C: fixture_get_last_week_round.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <returns>The week round number.</returns>
    public static int GetLastWeekRound(int weekNumber)
    {
        int weekRoundNumber = 1;

        foreach (Cup cup in Variables.Country.AllCups)
        {
            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber)
                    weekRoundNumber = System.Math.Max(weekRoundNumber, fixture.WeekRoundNumber);
            }
        }

        return weekRoundNumber;
    }

    /// <summary>
    /// Get the next week and round there are fixtures from the specified ones onwards and write it into the variables.
    /// Corresponds to C: fixture_get_next_week.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round.</param>
    public static void GetNextWeek(ref int weekNumber, ref int weekRoundNumber)
    {
        int localWeek = weekNumber;
        int localRound = weekRoundNumber;
        Fixture fix = null;

        weekNumber = 1000;
        weekRoundNumber = 1000;

        foreach (League league in Variables.Country.Leagues)
        {
            if (!League.QueryLeagueActive(league))
                continue;

            fix = GetNext(league.Id, localWeek, localRound);
            if (fix != null &&
                (fix.WeekNumber > localWeek || (fix.WeekNumber == localWeek && fix.WeekRoundNumber > localRound)) &&
                (fix.WeekNumber < weekNumber || (fix.WeekNumber == weekNumber && fix.WeekRoundNumber < weekRoundNumber)) &&
                (fix.Clid == Variables.Users[Variables.CurUser].Team.Clid || Variables.Options.GetInt("int_opt_user_show_all_leagues") != 0))
            {
                weekNumber = fix.WeekNumber;
                weekRoundNumber = fix.WeekRoundNumber;
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            fix = GetNext(cup.Id, localWeek, localRound);
            if (fix != null &&
                (fix.WeekNumber > localWeek || (fix.WeekNumber == localWeek && fix.WeekRoundNumber > localRound)) &&
                (fix.WeekNumber < weekNumber || (fix.WeekNumber == weekNumber && fix.WeekRoundNumber < weekRoundNumber)))
            {
                weekNumber = fix.WeekNumber;
                weekRoundNumber = fix.WeekRoundNumber;
            }
        }

        if (weekNumber == 1000)
        {
            weekNumber = 1;
            weekRoundNumber = 1;
        }
    }

    /// <summary>
    /// Get the previous week and round there are fixtures and write it into the variables.
    /// Corresponds to C: fixture_get_previous_week.
    /// </summary>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round.</param>
    public static void GetPreviousWeek(ref int weekNumber, ref int weekRoundNumber)
    {
        int localWeek = weekNumber;
        int localRound = weekRoundNumber;
        Fixture fix = null;

        weekNumber = -1;
        weekRoundNumber = -1;

        foreach (League league in Variables.Country.Leagues)
        {
            if (!League.QueryLeagueActive(league))
                continue;

            fix = GetPrevious(league.Id, localWeek, localRound);
            if (fix != null &&
                (fix.WeekNumber < localWeek || (fix.WeekNumber == localWeek && fix.WeekRoundNumber < localRound)) &&
                (fix.WeekNumber > weekNumber || (fix.WeekNumber == weekNumber && fix.WeekRoundNumber > weekRoundNumber)) &&
                (fix.Clid == Variables.Users[Variables.CurUser].Team.Clid || Variables.Options.GetInt("int_opt_user_show_all_leagues") != 0))
            {
                weekNumber = fix.WeekNumber;
                weekRoundNumber = 1;
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            fix = GetPrevious(cup.Id, localWeek, localRound);
            if (fix != null &&
                (fix.WeekNumber < localWeek || (fix.WeekNumber == localWeek && fix.WeekRoundNumber < localRound)) &&
                (fix.WeekNumber > weekNumber || (fix.WeekNumber == weekNumber && fix.WeekRoundNumber > weekRoundNumber)))
            {
                weekNumber = fix.WeekNumber;
                weekRoundNumber = fix.WeekRoundNumber;
            }
        }

        if (weekNumber == -1)
        {
            weekNumber = localWeek;
            weekRoundNumber = localRound;
        }
    }

    /// <summary>
    /// Return an array with the user's fixtures in this season.
    /// Corresponds to C: fixture_get_season_results.
    /// </summary>
    /// <returns>The list of season results.</returns>
    public static List<Fixture> GetSeasonResults()
    {
        List<Fixture> results = new List<Fixture>();
        Team userTeam = Variables.Users[Variables.CurUser].Team;

        if (userTeam != null && userTeam.Clid >= Bygfoot.IdCupStart)
        {
            League league = League.LeagueFromClid(userTeam.Clid);
            if (league != null && League.QueryLeagueActive(league))
            {
                foreach (Fixture fixture in league.Fixtures)
                {
                    if (FixtureTeamInvolved(fixture, userTeam))
                        results.Add(fixture);
                }
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (userTeam == null || !Team.QueryTeamIsInCup(userTeam, cup))
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (FixtureTeamInvolved(fixture, userTeam))
                    results.Add(fixture);
            }
        }

        results.Sort((fix1, fix2) => CompareFunc(fix1, fix2, 0));

        return results;
    }

    /// <summary>
    /// Calculate how many goals the team has to score to win the match (also taking into account second leg stuff).
    /// Corresponds to C: fixture_get_goals_to_win.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="tm">The team.</param>
    /// <returns>The number of goals to win.</returns>
    public static int GetGoalsToWin(Fixture fix, Team tm)
    {
        Fixture firstLeg = null;
        int idx = tm == fix.Teams[1] ? 1 : 0;
        int returnValue = -100;
        int res0 = fix.Result[0, 0] + fix.Result[0, 1];
        int res1 = fix.Result[1, 0] + fix.Result[1, 1];
        int tmp;

        if (!FixtureTeamInvolved(fix, tm))
        {
            // debug_print_message("fixture_get_goals_to_win: team %s doesn't participate in fixture given (%s)\n",
            //   tm.Name, league_cup_get_name_string(fix.Clid));
            return -100;
        }

        if (!fix.SecondLeg)
            return idx == 0 ? res1 - res0 + 1 : res0 - res1 + 1;

        firstLeg = GetFirstLeg(fix, false);

        if (res0 + firstLeg.Result[1, 0] == res1 + firstLeg.Result[0, 0])
        {
            if (idx == 0)
                returnValue = res1 >= firstLeg.Result[1, 0] ? 1 : 0;
            else
                returnValue = res1 > firstLeg.Result[1, 0] ? 0 : 1;
        }
        else if (res0 + firstLeg.Result[1, 0] > res1 + firstLeg.Result[0, 0])
        {
            tmp = res1;
            while (res0 + firstLeg.Result[1, 0] > res1 + firstLeg.Result[0, 0])
                res1++;

            if (idx == 0)
                returnValue = res1 >= firstLeg.Result[1, 0] ? tmp - res1 + 1 : tmp - res1;
            else
                returnValue = res1 > firstLeg.Result[1, 0] ? res1 - tmp : res1 - tmp + 1;
        }
        else
        {
            tmp = res0;
            while (res0 + firstLeg.Result[1, 0] < res1 + firstLeg.Result[0, 0])
                res0++;

            if (idx == 0)
                returnValue = res1 >= firstLeg.Result[1, 0] ? res0 - tmp + 1 : res0 - tmp;
            else
                returnValue = res1 > firstLeg.Result[1, 0] ? tmp - res0 : tmp - res0 + 1;
        }

        return returnValue;
    }

    /// <summary>
    /// Count how many matchdays there are in a fixture array.
    /// Corresponds to C: fixture_count_matchdays.
    /// </summary>
    /// <param name="fixtures">The fixture array.</param>
    /// <returns>The number of matchdays.</returns>
    public static int CountMatchdays(List<Fixture> fixtures)
    {
        int count = 0;

        for (int i = 0; i < fixtures.Count - 1; i++)
        {
            if (fixtures[i].WeekNumber != fixtures[i + 1].WeekNumber ||
                fixtures[i].WeekRoundNumber != fixtures[i + 1].WeekRoundNumber)
                count++;
        }

        return count + (fixtures.Count != 0 ? 1 : 0);
    }

    /// <summary>
    /// Return the last week there are fixtures written for.
    /// Corresponds to C: fixture_get_last_scheduled_week.
    /// </summary>
    /// <returns>The last scheduled week.</returns>
    public static int GetLastScheduledWeek()
    {
        int weekNr = 0;

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Fixtures.Count > 0)
                weekNr = System.Math.Max(weekNr, cup.Fixtures[cup.Fixtures.Count - 1].WeekNumber);
        }

        return weekNr;
    }

    /// <summary>
    /// Remove a number of round robin fixtures from a fixture array.
    /// Corresponds to C: fixture_remove_rrs.
    /// </summary>
    /// <param name="fixtures">The fixture array.</param>
    /// <param name="clid1">The league id for one team.</param>
    /// <param name="clid2">The league id for the second team.</param>
    /// <param name="toRemove">How many matches to remove per pairing.</param>
    public static void RemoveRrs(List<Fixture> fixtures, int clid1, int clid2, int toRemove)
    {
        League league1 = League.LeagueFromClid(clid1);
        League league2 = League.LeagueFromClid(clid2);

        if (league1 == null || league2 == null)
            return;

        for (int i = 0; i < league1.Teams.Count; i++)
        {
            int start = clid1 == clid2 ? i + 1 : 0;
            for (int j = start; j < league2.Teams.Count; j++)
            {
                int removed = 0;
                for (int k = fixtures.Count - 1; k >= 0; k--)
                {
                    Fixture fixture = fixtures[k];
                    if ((fixture.Teams[0].Id == league1.Teams[i].Id && fixture.Teams[1].Id == league2.Teams[j].Id) ||
                        (fixture.Teams[1].Id == league1.Teams[i].Id && fixture.Teams[0].Id == league2.Teams[j].Id))
                    {
                        fixtures.RemoveAt(k);
                        removed++;

                        if (removed == toRemove)
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Move matches in the fixture array to earlier dates if possible.
    /// Corresponds to C: fixtures_condense.
    /// </summary>
    /// <param name="fixtures">The fixture array.</param>
    public static void FixturesCondense(List<Fixture> fixtures)
    {
        bool fixMoved = true;

        while (fixMoved)
        {
            fixMoved = false;

            Dictionary<(int Week, int Round), List<int>> matchdayTeams = new Dictionary<(int, int), List<int>>();

            foreach (League league in Variables.Country.Leagues)
            {
                foreach (Fixture fixture in league.Fixtures)
                {
                    KeyValuePair<(int, int), List<int>> key = new KeyValuePair<(int, int), List<int>>((fixture.WeekNumber - 1, fixture.WeekRoundNumber - 1), new List<int>());
                    if (!matchdayTeams.ContainsKey(key.Key))
                        matchdayTeams[key.Key] = new List<int>();

                    matchdayTeams[key.Key].Add(fixture.Teams[0].Id);
                    matchdayTeams[key.Key].Add(fixture.Teams[1].Id);
                }
            }

            for (int i = fixtures.Count - 1; i >= 0; i--)
            {
                Fixture fix = fixtures[i];
                for (int j = 0; j <= fix.WeekNumber - 2; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        if (matchdayTeams.TryGetValue((j, k), out List<int> teamIds))
                        {
                            if (!teamIds.Contains(fix.Teams[0].Id) && !teamIds.Contains(fix.Teams[1].Id))
                            {
                                fixMoved = true;
                                fix.WeekNumber = j + 1;
                                fix.WeekRoundNumber = k + 1;
                                teamIds.Add(fix.Teams[0].Id);
                                teamIds.Add(fix.Teams[1].Id);
                            }
                        }
                    }
                }
            }

            foreach (KeyValuePair<(int, int), List<int>> entry in matchdayTeams)
            {
                // Nothing to clean up, the dictionary just holds the last state.
            }
        }
    }

    /// <summary>
    /// Return the name of a cup round, e.g. 'round robin' or 'final' or so.
    /// Corresponds to C: fixture_get_cup_round_name.
    /// </summary>
    /// <param name="fix">A fixture belonging to the round.</param>
    /// <param name="buf">The buffer we write the name into.</param>
    public static void GetCupRoundName(Fixture fix, ref string buf)
    {
        Cup cup = FindCupById(fix.Clid);
        CupRound cupRound = cup != null && cup.Rounds.Count > 0 ? cup.Rounds[fix.Round] : new CupRound();

        buf = cupRound.Name;

        if (cupRound.HomeAway && cupRound.RoundRobinNumberOfGroups == 0)
        {
            if (fix.SecondLeg)
                buf += " -- Second leg";
            else
                buf += " -- First leg";
        }
        else if (fix.ReplayNumber > 0)
        {
            buf += " -- Replay matches";
        }
    }

    /// <summary>
    /// Reset the team pointers from the team ids in the fixtures.
    /// Corresponds to C: fixture_refresh_team_pointers.
    /// </summary>
    /// <param name="fixtures">The fixture array.</param>
    public static void RefreshTeamPointers(List<Fixture> fixtures)
    {
        for (int i = 0; i < fixtures.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                Fixture fixture = fixtures[i];
                Team team = Team.OfId(fixture.TeamIds[j]);
                if (j == 0)
                    fixture.Teams[0] = team;
                else
                    fixture.Teams[1] = team;
            }
        }
    }

    /// <summary>
    /// Find a league by its sid.
    /// </summary>
    /// <param name="sid">The sid.</param>
    /// <returns>The league or null.</returns>
    private static League FindLeagueBySid(string sid)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Sid == sid)
                return league;
        }

        return null;
    }

    /// <summary>
    /// Find a cup by its id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>The cup or null.</returns>
    private static Cup FindCupById(int id)
    {
        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id == id)
                return cup;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Id == id)
                return cup;
        }

        return null;
    }

    /// <summary>
    /// Get the first week of a cup round.
    /// </summary>
    /// <param name="cup">The cup.</param>
    /// <param name="cupRound">The cup round.</param>
    /// <returns>The first week.</returns>
    private static int GetFirstWeekOfCupRound(Cup cup, int cupRound)
    {
        // Implementation depends on the not yet converted league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sum the given values of a result row.
    /// </summary>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <param name="c">The third value.</param>
    /// <returns>The sum.</returns>
    private static int Sum(int a, int b, int c)
    {
        return a + b + c;
    }

    /// <summary>
    /// Get all fixtures for a league or cup id.
    /// </summary>
    /// <param name="clid">The cup or league id.</param>
    /// <returns>The fixtures.</returns>
    private static List<Fixture> GetFixturesForClid(int clid)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id == clid)
                return league.Fixtures;
        }

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id == clid)
                return cup.Fixtures;
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (cup.Id == clid)
                return cup.Fixtures;
        }

        return new List<Fixture>();
    }

    /// <summary>
    /// Check whether a fixture involves the given team.
    /// Corresponds to C: query_fixture_team_involved.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    /// <param name="tm">The team.</param>
    /// <returns>Whether the team is involved.</returns>
    public static bool FixtureTeamInvolved(Fixture fix, Team tm)
    {
        return fix.TeamIds[0] == tm.Id || fix.TeamIds[1] == tm.Id;
    }
}

/// <summary>
/// Types of ways to compare fixtures, e.g. when sorting them.
/// Corresponds to C enum FixtureCompare.
/// </summary>
public enum FixtureCompare
{
    Date = 0, // FIXTURE_COMPARE_DATE
    End = 1   // FIXTURE_COMPARE_END
}
