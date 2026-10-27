// League.cs (Located in Bygfoot.Cli/Models folder)
// Converted from src/league_struct.h
//
// Bygfoot Football Manager -- a small and simple GTK2-based football management game.
// Copyright (C) 2005 Gyözö Both (gyboth@bygfoot.com)
// GPL v2 or later

using System;
using System.Collections.Generic;

namespace Bygfoot.Cli.Models;

/// <summary>
/// Type of promotion/relegation rule.
/// Corresponds to C enum PromRelType.
/// </summary>
public enum PromRelType
{
    Promotion,   // PROM_REL_PROMOTION = 0
    Relegation,  // PROM_REL_RELEGATION
    None         // PROM_REL_NONE
}

/// <summary>
/// An element representing a promotion or relegation rule.
/// This means, a PromRelElement specifies a range of teams
/// that get promoted or relegated to a given league.
/// Corresponds to C struct PromRelElement.
/// @see PromRel
/// </summary>
public class PromRelElement
{
    /// <summary>The range of teams; default 0 and 0. Corresponds to C: gint ranks[2].</summary>
    public int[] Ranks { get; set; } = new int[2];

    /// <summary>From which table to pick the teams in case there are several. Default: 0.</summary>
    public int FromTable { get; set; }

    /// <summary>The id of the destination league. Default: "".</summary>
    public string DestSid { get; set; }

    /// <summary>Type. Promotion or relegation or none. @see PromRelType</summary>
    public PromRelType Type { get; set; }
}

/// <summary>
/// A struct representing promotion/relegation games.
/// Corresponds to C struct PromGames.
/// </summary>
public class PromGames
{
    /// <summary>The id of the league the promotion games winner gets promoted to. Default: "".</summary>
    public string DestSid { get; set; }

    /// <summary>The id of the league the promotion games losers get moved to. Default: "".</summary>
    public string LoserSid { get; set; }

    /// <summary>Number of teams that advance from the promotion games. Default: 1.</summary>
    public int NumberOfAdvance { get; set; }

    /// <summary>The cup determining how the promotion games are handled.</summary>
    public string CupSid { get; set; }
}

/// <summary>
/// This structure specifies how promotion and relegation is handled in a league.
/// It contains promotion and relegation rules in an array and possibly also
/// a rule about promotion games to be played.
/// Corresponds to C struct PromRel.
/// @see PromRelElement
/// </summary>
public class PromRel
{
    /// <summary>Array with promotion/relegation rules. @see PromRelElement. Corresponds to C: GArray *elements.</summary>
    public List<PromRelElement> Elements { get; set; } = new List<PromRelElement>();

    /// <summary>Array with promotion/relegation games. @see PromGames. Corresponds to C: GArray *prom_games.</summary>
    public List<PromGames> PromGames { get; set; } = new List<PromGames>();
}

/// <summary>
/// A structure describing a different league joined to the current one
/// in the sense that there are matches played between teams from both leagues
/// like in the US conference system.
/// Corresponds to C struct JoinedLeague.
/// </summary>
public class JoinedLeague
{
    /// <summary>Sid of the joined league.</summary>
    public string Sid { get; set; }

    /// <summary>How many round robins to schedule.</summary>
    public int Rr { get; set; }
}

/// <summary>
/// A structure containing a week when a new table
/// gets created with nullified values for the league;
/// older tables get stored.
/// Corresponds to C struct NewTable.
/// </summary>
public class NewTable
{
    /// <summary>The week when the new table is added.</summary>
    public int AddWeek { get; set; }

    /// <summary>Name of the new table.</summary>
    public string Name { get; set; }
}

/// <summary>
/// A structure describing a custom break in the fixtures
/// schedule occuring at a particular week.
/// Corresponds to C struct WeekBreak.
/// </summary>
public class WeekBreak
{
    /// <summary>In which week the break occurs.</summary>
    public int WeekNumber { get; set; }

    /// <summary>Length of break in weeks.</summary>
    public int Length { get; set; }
}

/// <summary>
/// Representation of a league.
/// Corresponds to C struct League.
/// @see PromRel
/// @see Table
/// </summary>
public class League
{
    /// <summary>Name of the league. Default: "".</summary>
    public string Name { get; set; }

    /// <summary>Short name of the league. Default: "".</summary>
    public string ShortName { get; set; }

    /// <summary>String id of the league. Default: "".</summary>
    public string Sid { get; set; }

    /// <summary>Symbol of the league. Default: "".</summary>
    public string Symbol { get; set; }

    /// <summary>
    /// The sid of the player names file the teams in the league take their names from.
    /// Default: 'general', meaning the 'player_names_general.xml' file.
    /// </summary>
    public string NamesFile { get; set; }

    /// <summary>Promotion/relegation rules. @see PromRel. Corresponds to C: PromRel prom_rel.</summary>
    public PromRel PromRel { get; set; } = new PromRel();

    /// <summary>Numerical id, as opposed to the string id 'sid'.</summary>
    public int Id { get; set; }

    /// <summary>Layer of the league; this specifies which leagues are parallel.</summary>
    public int Layer { get; set; }

    /// <summary>The first week games are played. Default: 1.</summary>
    public int FirstWeek { get; set; }

    /// <summary>Weeks between two matchdays. Default: 1.</summary>
    public int WeekGap { get; set; }

    /// <summary>
    /// Here we store intervals of fixtures during which there should be two matches
    /// in a week instead of one.
    /// Corresponds to C: GArray *two_match_weeks[2].
    /// </summary>
    public List<int>[] TwoMatchWeeks { get; set; } = new List<int>[2];

    /// <summary>
    /// How many round robins are played. Important for small leagues with 10 teams or so.
    /// Default: 2.
    /// </summary>
    public int RoundRobins { get; set; }

    /// <summary>Number of weeks between the parts of a round robin. Corresponds to C: GArray *rr_breaks.</summary>
    public List<int> RrBreaks { get; set; } = new List<int>();

    /// <summary>
    /// Number of yellow cards until a player gets banned.
    /// Default: 1000 (which means 'off', basically).
    /// </summary>
    public int YellowRed { get; set; }

    /// <summary>Average talent for the first season. Default: -1.</summary>
    public float AverageTalent { get; set; }

    /// <summary>Array of teams in the league. @see Team. Corresponds to C: GArray *teams.</summary>
    public List<Team> Teams { get; set; } = new List<Team>();

    /// <summary>List of leagues joined fixture-wise to this one. @see JoinedLeague. Corresponds to C: GArray *joined_leagues.</summary>
    public List<JoinedLeague> JoinedLeagues { get; set; } = new List<JoinedLeague>();

    /// <summary>
    /// League tables. Usually only one, sometimes more than one is created.
    /// @see Table. Corresponds to C: GArray *tables.
    /// </summary>
    public List<Table> Tables { get; set; } = new List<Table>();

    /// <summary>
    /// Array holding NewTable elements specifying when to add a new table to the tables array.
    /// Corresponds to C: GArray *new_tables.
    /// </summary>
    public List<NewTable> NewTables { get; set; } = new List<NewTable>();

    /// <summary>The fixtures of a season for the league. Corresponds to C: GArray *fixtures.</summary>
    public List<Fixture> Fixtures { get; set; } = new List<Fixture>();

    /// <summary>A pointer array of properties (like "inactive"). Corresponds to C: GPtrArray *properties.</summary>
    public List<string> Properties { get; set; } = new List<string>();

    /// <summary>Array of custom breaks in schedule. Corresponds to C: GArray *week_breaks.</summary>
    public List<WeekBreak> WeekBreaks { get; set; } = new List<WeekBreak>();

    /// <summary>The current league statistics. Corresponds to C: LeagueStat stats.</summary>
    public LeagueStat Stats { get; set; } = new LeagueStat();

    /// <summary>
    /// Pointer array with the sids of competitions that the fixtures of which should be
    /// avoided when scheduling the league fixtures.
    /// Corresponds to C: GPtrArray *skip_weeks_with.
    /// </summary>
    public List<string> SkipWeeksWith { get; set; } = new List<string>();

    /// <summary>
    /// A structure needed when managing promotions and relegations.
    /// Corresponds to C struct TeamMove in src/league.h.
    /// </summary>
    public class TeamMove
    {
    /// <summary>The team we move. Corresponds to C: Team tm.</summary>
    public Team Tm { get; set; }

    /// <summary>Array of possible destination indices. Corresponds to C: GArray *dest_idcs.</summary>
    public List<int> DestIdcs { get; set; } = new List<int>();

    /// <summary>Promotion or relegation. Corresponds to C: gint prom_rel_type.</summary>
    public PromRelType PromRelType { get; set; }

    /// <summary>Whether a destination league for the team is already assigned. Corresponds to C: gboolean dest_assigned.</summary>
    public bool DestAssigned { get; set; }
}

/// <summary>
    /// <summary>
    /// Create a new league with some default values. Corresponds to C: league_new.
    /// </summary>
    /// <param name="newId">Whether a new id should be created.</param>
    /// <returns>The league we created.</returns>
    public static League New(bool newId)
    {
        League newLeague = new League();
        newLeague.Name = string.Empty;
        newLeague.ShortName = string.Empty;
        newLeague.Sid = string.Empty;
        newLeague.Symbol = string.Empty;
        newLeague.NamesFile = Variables.Options.GetString("string_opt_player_names_file");
        newLeague.Id = newId ? Bygfoot.LeagueIdNew() : -1;
        newLeague.Layer = -1;
        newLeague.AverageTalent = 0;
        newLeague.PromRel = new PromRel();
        newLeague.FirstWeek = 1;
        newLeague.WeekGap = 1;
        newLeague.TwoMatchWeeks = new List<int>[2];
        newLeague.TwoMatchWeeks[0] = new List<int>();
        newLeague.TwoMatchWeeks[1] = new List<int>();
        newLeague.RoundRobins = 2;
        newLeague.RrBreaks = new List<int>();
        newLeague.YellowRed = 1000;
        newLeague.Teams = new List<Team>();
        newLeague.JoinedLeagues = new List<JoinedLeague>();
        newLeague.NewTables = new List<NewTable>();
        newLeague.Tables = new List<Table>();
        newLeague.Properties = new List<string>();
        newLeague.SkipWeeksWith = new List<string>();
        newLeague.WeekBreaks = new List<WeekBreak>();
        newLeague.Stats = new LeagueStat();

        return newLeague;
    }

    /// <summary>
    /// Create a new PromRelElement with default values. Corresponds to C: prom_rel_element_new.
    /// </summary>
    /// <returns>The PromRelElement we created.</returns>
    public static PromRelElement New()
    {
        PromRelElement newElement = new PromRelElement();
        newElement.Ranks = new int[2];
        newElement.FromTable = 0;
        newElement.DestSid = string.Empty;
        newElement.Type = PromRelType.None;

        return newElement;
    }

    /// <summary>
    /// Create a new PromGames with default values. Corresponds to C: prom_games_new.
    /// </summary>
    /// <returns>The PromGames we created.</returns>
    public static PromGames PromGamesNew()
    {
        PromGames newGames = new PromGames();
        newGames.DestSid = string.Empty;
        newGames.LoserSid = string.Empty;
        newGames.NumberOfAdvance = 1;
        newGames.CupSid = string.Empty;

        return newGames;
    }

    /// <summary>
    /// Get the array index of the given league or cup id. Corresponds to C: league_cup_get_index_from_clid.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The index in the leagues or cups array.</returns>
    public static int LeagueCupGetIndexFromClid(int clid)
    {
        int index = -1;

        if (clid < Bygfoot.IdCupStart)
        {
            for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            {
                if (Variables.Country.Leagues[i].Id == clid)
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < Variables.Country.Cups.Count; i++)
            {
                if (Variables.Country.Cups[i].Id == clid)
                {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }

    /// <summary>
    /// Return the league pointer belonging to the id. Corresponds to C: league_from_clid.
    /// </summary>
    /// <param name="clid">The id we look for.</param>
    /// <returns>The league or null if failed.</returns>
    public static League LeagueFromClid(int clid)
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            if (Variables.Country.Leagues[i].Id == clid)
                return Variables.Country.Leagues[i];
        }

        return null;
    }

    /// <summary>
    /// Return the id of the next league (or cup if we are at the end of the leagues array).
    /// Corresponds to C: league_cup_get_next_clid.
    /// </summary>
    /// <param name="clid">The id of the current league or cup.</param>
    /// <param name="countInactive">Whether inactive leagues count.</param>
    /// <returns>The id of the next league or cup.</returns>
    public static int LeagueCupGetNextClid(int clid, bool countInactive)
    {
        int returnValue = -1;

        if (clid < Bygfoot.IdCupStart)
        {
            int i = 0;
            for (i = 0; i < Variables.Country.Leagues.Count; i++)
            {
                if (Variables.Country.Leagues[i].Id == clid)
                    break;
            }

            if (i != Variables.Country.Leagues.Count - 1)
            {
                if (QueryLeagueActive(Variables.Country.Leagues[i + 1]) || countInactive)
                    returnValue = Variables.Country.Leagues[i + 1].Id;
                else
                    returnValue = LeagueCupGetNextClid(Variables.Country.Leagues[i + 1].Id, countInactive);
            }
            else if (Variables.Country.AllCups.Count > 0)
                returnValue = Variables.Country.AllCups[0].Id;
            else
            {
                if (QueryLeagueActive(Variables.Country.Leagues[0]) || countInactive)
                    returnValue = Variables.Country.Leagues[0].Id;
                else
                    returnValue = LeagueCupGetNextClid(Variables.Country.Leagues[0].Id, countInactive);
            }
        }
        else
        {
            int iCup = 0;
            for (iCup = 0; iCup < Variables.Country.Cups.Count; iCup++)
            {
                if (Variables.Country.Cups[iCup].Id == clid)
                    break;
            }

            if (iCup != Variables.Country.Cups.Count - 1)
                returnValue = Variables.Country.Cups[iCup + 1].Id;
            else
            {
                if (QueryLeagueActive(Variables.Country.Leagues[0]) || countInactive)
                    returnValue = Variables.Country.Leagues[0].Id;
                else
                    returnValue = LeagueCupGetNextClid(Variables.Country.Leagues[0].Id, countInactive);
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Return the id of the previous league or cup.
    /// Corresponds to C: league_cup_get_previous_clid.
    /// </summary>
    /// <param name="clid">The id of the current league or cup.</param>
    /// <param name="countInactive">Whether inactive leagues count.</param>
    /// <returns>The id of the previous league or cup.</returns>
    public static int LeagueCupGetPreviousClid(int clid, bool countInactive)
    {
        int returnValue = -1;

        if (clid < Bygfoot.IdCupStart)
        {
            int i = 0;
            for (int j = Variables.Country.Leagues.Count - 1; j >= 0; j--)
            {
                if (Variables.Country.Leagues[j].Id == clid)
                {
                    i = j;
                    break;
                }
            }

            if (i != 0)
            {
                if (QueryLeagueActive(Variables.Country.Leagues[i - 1]) || countInactive)
                    returnValue = Variables.Country.Leagues[i - 1].Id;
                else
                    returnValue = LeagueCupGetPreviousClid(Variables.Country.Leagues[i - 1].Id, countInactive);
            }
            else if (Variables.Country.AllCups.Count > 0)
                returnValue = Variables.Country.AllCups[Variables.Country.AllCups.Count - 1].Id;
            else
            {
                if (QueryLeagueActive(Variables.Country.Leagues[Variables.Country.Leagues.Count - 1]) || countInactive)
                    returnValue = Variables.Country.Leagues[Variables.Country.Leagues.Count - 1].Id;
                else
                    returnValue = LeagueCupGetPreviousClid(Variables.Country.Leagues[Variables.Country.Leagues.Count - 1].Id, countInactive);
            }
        }
        else
        {
            int iCup = 0;
            for (int j = Variables.Country.Cups.Count - 1; j >= 0; j--)
            {
                if (Variables.Country.Cups[j].Id == clid)
                {
                    iCup = j;
                    break;
                }
            }

            if (iCup != 0)
                returnValue = Variables.Country.Cups[iCup - 1].Id;
            else
            {
                if (QueryLeagueActive(Variables.Country.Leagues[Variables.Country.Leagues.Count - 1]) || countInactive)
                    returnValue = Variables.Country.Leagues[Variables.Country.Leagues.Count - 1].Id;
                else
                    returnValue = LeagueCupGetPreviousClid(Variables.Country.Leagues[Variables.Country.Leagues.Count - 1].Id, countInactive);
            }
        }

        return returnValue;
    }

    /// <summary>
    /// Return the fixture coming after the specified week and round.
    /// Corresponds to C: league_cup_get_next_fixture.
    /// </summary>
    /// <param name="clid">The id of the league or cup the fixtures of which we examine.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round number.</param>
    /// <returns>A fixture or null.</returns>
    public static Fixture LeagueCupGetNextFixture(int clid, int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixtures = GetFixtures(clid);

        for (int i = 0; i < fixtures.Count; i++)
        {
            if (fixtures[i].WeekNumber > weekNumber ||
                (fixtures[i].WeekNumber == weekNumber && fixtures[i].WeekRoundNumber >= weekRoundNumber))
                return fixtures[i];
        }

        return null;
    }

    /// <summary>
    /// Return the fixture coming just before the specified week and round.
    /// Corresponds to C: league_cup_get_previous_fixture.
    /// </summary>
    /// <param name="clid">The id of the league or cup the fixtures of which we examine.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round number.</param>
    /// <returns>A fixture or null.</returns>
    public static Fixture LeagueCupGetPreviousFixture(int clid, int weekNumber, int weekRoundNumber)
    {
        List<Fixture> fixtures = GetFixtures(clid);

        for (int i = fixtures.Count - 1; i >= 0; i--)
        {
            if (fixtures[i].WeekNumber < weekNumber ||
                (fixtures[i].WeekNumber == weekNumber && fixtures[i].WeekRoundNumber < weekRoundNumber))
                return fixtures[i];
        }

        return null;
    }

    /// <summary>
    /// Return the average stadium capacity of cpu teams in the specified league or cup.
    /// Corresponds to C: league_cup_average_capacity.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The average capacity.</returns>
    public static int LeagueCupAverageCapacity(int clid)
    {
        int cnt = 0;
        float sum = 0;
        List<Team> teams = new List<Team>();

        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                teams = league.Teams;
        }

        foreach (Team team in teams)
        {
            sum += team.Stadium.Capacity;
            cnt++;
        }

        if (cnt == 0)
            return 0;

        return (int)(sum / cnt);
    }

    /// <summary>
    /// Get the index of the league with the specified string id. Corresponds to C: league_index_from_sid.
    /// </summary>
    /// <param name="sid">The sid.</param>
    /// <returns>The index in the leagues array.</returns>
    public static int LeagueIndexFromSid(string sid)
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            if (Variables.Country.Leagues[i].Sid == sid)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Remove the team with the specified id from the teams array without freeing the memory
    /// (used in promotion/relegation). Corresponds to C: league_remove_team_with_id.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="id">The id.</param>
    public static void RemoveTeamWithId(League league, int id)
    {
        league.Teams.RemoveAll(t => t.Id == id);
    }

    /// <summary>
    /// Nullify league stuff at the beginning of a season. Corresponds to C: league_season_start.
    /// </summary>
    /// <param name="league">The league.</param>
    public static void SeasonStart(League league)
    {
        // Implementation depends on the not yet converted team, table and player functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether the team with specified rank in the league would participate in promotion
    /// games were the season to end. Corresponds to C: query_league_rank_in_prom_games.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="rank">The rank.</param>
    /// <returns>Whether the rank is in the promotion games.</returns>
    public static bool QueryLeagueRankInPromGames(League league, int rank)
    {
        // Implementation depends on the not yet converted cup and league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there are/were league matches. Corresponds to C: query_league_matches_in_week.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <returns>Whether there are matches in the week.</returns>
    public static bool QueryLeagueMatchesInWeek(League league, int weekNumber)
    {
        foreach (Fixture fixture in league.Fixtures)
        {
            if (fixture.WeekNumber == weekNumber)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Add the teams to promote/relegate (from the prom_rel elements) from the league to the array.
    /// Corresponds to C: league_get_team_movements_prom_rel.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="teamMovements">The team movements.</param>
    public static void GetTeamMovementsPromRel(League league, List<TeamMove> teamMovements)
    {
        // Implementation depends on the not yet converted misc and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add the team movements from the promotion games to the array. Corresponds to C: league_get_team_movements_prom_games.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="promGames">The promotion games.</param>
    /// <param name="teamMovements">The team movements.</param>
    /// <param name="promGamesTeams">The teams of the promotion games.</param>
    /// <param name="up">Whether the movement is an upgrade.</param>
    public static void GetTeamMovementsPromGames(League league, PromGames promGames, List<TeamMove> teamMovements, List<Team> promGamesTeams, bool up)
    {
        // Implementation depends on the not yet converted cup and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add the team movements from the promotion/relegation to the array and remove them from the leagues.
    /// Corresponds to C: league_get_team_movements.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <param name="teamMovements">The team movements.</param>
    public static void GetTeamMovements(League league, List<TeamMove> teamMovements)
    {
        // Implementation depends on the not yet converted cup and team functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there are unassigned TeamMoves in the array. Corresponds to C: query_league_team_movements_unassigned.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <returns>Whether there are unassigned moves.</returns>
    public static bool QueryLeagueTeamMovementsUnassigned(List<TeamMove> teamMovements)
    {
        foreach (TeamMove teamMove in teamMovements)
        {
            if (!teamMove.DestAssigned)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Find out whether there are unassigned team moves with a single destination value. Corresponds to C: query_league_team_movements_unassigned_single.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <returns>Whether there are unassigned moves with a single destination.</returns>
    public static bool QueryLeagueTeamMovementsUnassignedSingle(List<TeamMove> teamMovements)
    {
        foreach (TeamMove teamMove in teamMovements)
        {
            if (!teamMove.DestAssigned && teamMove.DestIdcs.Count == 1)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Print out the movements to the console (debug function). Corresponds to C: league_team_movements_print.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <param name="leagueSize">The size of all leagues.</param>
    /// <param name="leagueCurSize">The current size of all leagues.</param>
    public static void TeamMovementsPrint(List<TeamMove> teamMovements, int[] leagueSize, int[] leagueCurSize)
    {
        // Implementation depends on the not yet converted team, league and debug functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compare two leagues when doing promotion/relegation. The league that has fewer teams is preferred.
    /// Corresponds to C: league_team_movements_compare_dest_idcs.
    /// </summary>
    /// <param name="a">The first league index.</param>
    /// <param name="b">The second league index.</param>
    /// <param name="data">The current league sizes.</param>
    /// <returns>The comparison result.</returns>
    public static int TeamMovementsCompareDestIdcs(object a, object b, object data)
    {
        // Implementation depends on the not yet converted league and maths functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign a random destination for the team move with given index and remove the destination
    /// from all other unassigned moves if the dest league is full. Corresponds to C: league_team_movements_assign_dest.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <param name="idx">The index of the move.</param>
    /// <param name="leagueSize">The size of all leagues.</param>
    /// <param name="leagueCurSize">The current size of all leagues.</param>
    public static void TeamMovementsAssignDest(List<TeamMove> teamMovements, int idx, int[] leagueSize, int[] leagueCurSize)
    {
        // Implementation depends on the not yet converted team and league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assign all unassigned TeamMoves in the array that only have one destination value.
    /// Corresponds to C: league_team_movements_prune.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <param name="leagueSize">The size of all leagues.</param>
    /// <param name="leagueCurSize">The current size of all leagues.</param>
    public static void TeamMovementsPrune(List<TeamMove> teamMovements, int[] leagueSize, int[] leagueCurSize)
    {
        // Implementation depends on the not yet converted team and league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to assign destinations for the team movements based on the array of possible destinations.
    /// Corresponds to C: league_team_movements_destinations.
    /// </summary>
    /// <param name="teamMovements">The team movements.</param>
    /// <param name="leagueSize">The size of all leagues.</param>
    public static void TeamMovementsDestinations(List<TeamMove> teamMovements, int[] leagueSize)
    {
        // Implementation depends on the not yet converted team and league functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out whether there are any active leagues in the country. Corresponds to C: query_leagues_active_in_country.
    /// </summary>
    /// <returns>Whether there are active leagues.</returns>
    public static bool QueryLeaguesActiveInCountry()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            if (QueryLeagueActive(Variables.Country.Leagues[i]))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Find out whether a given matchday should occur in the same week as the one before or a full week later.
    /// Corresponds to C: query_league_cup_matchday_in_two_match_week.
    /// </summary>
    /// <param name="twoMatchWeeks">The two match weeks.</param>
    /// <param name="matchday">The matchday.</param>
    /// <returns>Whether the matchday is in a two-match week.</returns>
    public static bool QueryLeagueCupMatchdayInTwoMatchWeek(List<int>[] twoMatchWeeks, int matchday)
    {
        for (int i = 0; i < twoMatchWeeks[0].Count; i++)
        {
            if (twoMatchWeeks[0][i] < matchday &&
                matchday <= twoMatchWeeks[1][i] &&
                (matchday - twoMatchWeeks[0][i]) % 2 == 1)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Add a new table to the league tables if specified in the new_tables array. Corresponds to C: league_check_new_tables.
    /// </summary>
    /// <param name="league">The league.</param>
    public static void CheckNewTables(League league)
    {
        // Implementation depends on the not yet converted table and misc functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add an initialized table to the league. Corresponds to C: league_add_table.
    /// </summary>
    /// <param name="league">The league.</param>
    public static void AddTable(League league)
    {
        Table newTable = new Table();
        newTable.Clid = league.Id;
        newTable.Name = league.Name;

        for (int i = 0; i < league.Teams.Count; i++)
        {
            TableElement newTableElement = new TableElement();
            newTableElement.Team = league.Teams[i];
            newTableElement.TeamId = league.Teams[i].Id;
            newTableElement.Values = new int[(int)TableElementValues.TableEnd];
            newTable.Elements.Add(newTableElement);
        }

        league.Tables.Add(newTable);
    }

    /// <summary>
    /// Find out whether the league or cup has the specified property. Corresponds to C: query_league_cup_has_property.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <param name="property">The property.</param>
    /// <returns>Whether the property is set.</returns>
    public static bool QueryLeagueCupHasProperty(int clid, string property)
    {
        return Misc.QueryMiscStringInArray(property, GetProperties(clid));
    }

    /// <summary>
    /// Get the teams of the league or cup. Corresponds to C: league_cup_get_teams.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The teams.</returns>
    public static List<Team> LeagueCupGetTeams(int clid)
    {
        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                return league.Teams;
        }

        Cup cup = GetCup(clid);
        List<Team> teams = new List<Team>();
        foreach (int teamId in cup.Teams)
        {
            // Team ids of cups cannot be resolved to team structures here
            // without the league structures they are part of.
        }

        return teams;
    }

    /// <summary>
    /// Get the properties of the league or cup. Corresponds to C: league_cup_get_properties.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The properties.</returns>
    public static List<string> LeagueCupGetProperties(int clid)
    {
        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                return league.Properties;
        }

        Cup cup = GetCup(clid);
        if (cup != null)
            return cup.Properties;

        return new List<string>();
    }

    /// <summary>
    /// Synchronise the number of league breaks with the number of round robins in the league.
    /// Corresponds to C: league_cup_adjust_rr_breaks.
    /// </summary>
    /// <param name="rrBreaks">The breaks.</param>
    /// <param name="roundRobins">The number of round robins.</param>
    /// <param name="weekGap">The week gap.</param>
    public static void LeagueCupAdjustRrBreaks(List<int> rrBreaks, int roundRobins, int weekGap)
    {
        for (int i = rrBreaks.Count - 1; i >= roundRobins - 1; i--)
        {
            rrBreaks.RemoveAt(i);
        }

        int defaultBreak = weekGap;
        if (rrBreaks.Count != 0)
            defaultBreak = rrBreaks[rrBreaks.Count - 1];

        for (int i = rrBreaks.Count; i < roundRobins - 1; i++)
        {
            rrBreaks.Add(defaultBreak);
        }
    }

    /// <summary>
    /// Fill the breaks array from a comma separated string of integers. Corresponds to C: league_cup_fill_rr_breaks.
    /// </summary>
    /// <param name="rrBreaks">The breaks.</param>
    /// <param name="breaks">The comma separated string of integers.</param>
    public static void LeagueCupFillRrBreaks(List<int> rrBreaks, string breaks)
    {
        string[] breaksArr = breaks.Split(',');
        for (int i = 0; i < breaksArr.Length; i++)
        {
            if (int.TryParse(breaksArr[i], out int newBreak))
                rrBreaks.Add(newBreak);
        }
    }

    /// <summary>
    /// Set the values of the week breaks to the week gap of the league or cup if necessary.
    /// Corresponds to C: league_cup_adjust_week_breaks.
    /// </summary>
    /// <param name="weekBreaks">The week breaks.</param>
    /// <param name="weekGap">The week gap.</param>
    public static void LeagueCupAdjustWeekBreaks(List<WeekBreak> weekBreaks, int weekGap)
    {
        for (int i = 0; i < weekBreaks.Count; i++)
        {
            if (weekBreaks[i].Length == -1000)
                weekBreaks[i].Length = weekGap;
        }
    }

    /// <summary>
    /// Return the week number with a possible schedule break adjustment. Corresponds to C: league_cup_get_week_with_break.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <returns>The week number.</returns>
    public static int LeagueCupGetWeekWithBreak(int clid, int weekNumber)
    {
        List<WeekBreak> weekBreaks = new List<WeekBreak>();

        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                weekBreaks = league.WeekBreaks;
        }

        for (int i = 0; i < weekBreaks.Count; i++)
        {
            if (weekBreaks[i].WeekNumber == weekNumber)
                return weekNumber + weekBreaks[i].Length;
        }

        return weekNumber;
    }

    /// <summary>
    /// Whether a league is active. Corresponds to the C macro query_league_active.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <returns>Whether the league is active.</returns>
    public static bool QueryLeagueActive(League league)
    {
        return !QueryLeagueCupHasProperty(league.Id, "inactive");
    }

    /// <summary>
    /// Get the fixtures of a league or cup. Corresponds to the C macro league_cup_get_fixtures.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The fixtures.</returns>
    private static List<Fixture> GetFixtures(int clid)
    {
        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                return league.Fixtures;
        }

        Cup cup = GetCup(clid);
        if (cup != null)
            return cup.Fixtures;

        return new List<Fixture>();
    }

    /// <summary>
    /// Get a cup by its id. Corresponds to the C function cup_from_clid.
    /// </summary>
    /// <param name="clid">The id of the cup.</param>
    /// <returns>The cup or null.</returns>
    private static Cup GetCup(int clid)
    {
        for (int i = 0; i < Variables.Country.Cups.Count; i++)
        {
            if (Variables.Country.Cups[i].Id == clid)
                return Variables.Country.Cups[i];
        }

        return null;
    }

    /// <summary>
    /// Get the properties of a league or cup. Corresponds to the C macro league_cup_get_properties.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <returns>The properties.</returns>
    private static List<string> GetProperties(int clid)
    {
        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClid(clid);
            if (league != null)
                return league.Properties;
        }

        Cup cup = GetCup(clid);
        if (cup != null)
            return cup.Properties;

        return new List<string>();
    }
}
