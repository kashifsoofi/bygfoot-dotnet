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
}
