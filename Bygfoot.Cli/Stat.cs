namespace Bygfoot.Cli;

/// <summary>
/// A statistics element holding some string and integer values. 
/// Corresponds to C Stat struct.
/// </summary>
public class Stat
{
    /// <summary>The team name.</summary>
    public string TeamName { get; set; }

    /// <summary>Primary integer value.</summary>
    public int Value1 { get; set; }

    /// <summary>Secondary integer value.</summary>
    public int Value2 { get; set; }

    /// <summary>Tertiary integer value.</summary>
    public int Value3 { get; set; }

    /// <summary>A descriptive string for this statistic.</summary>
    public string ValueString { get; set; }

    /// <summary>
    /// Return a newly allocated league stat with specified league name and symbol.
    /// Corresponds to C: stat_league_new.
    /// </summary>
    /// <param name="leagueName">The league name.</param>
    /// <param name="leagueSymbol">The league symbol.</param>
    /// <returns>The league stat.</returns>
    public static LeagueStat LeagueStatNew(string leagueName, string leagueSymbol)
    {
        LeagueStat newStat = new LeagueStat();
        newStat.LeagueName = leagueName;
        newStat.LeagueSymbol = leagueSymbol;
        newStat.TeamsOff = new List<Stat>();
        newStat.TeamsDef = new List<Stat>();
        newStat.PlayerScorers = new List<Stat>();
        newStat.PlayerGoalies = new List<Stat>();

        return newStat;
    }

    /// <summary>
    /// Return a newly allocated season stat with specified season number.
    /// Corresponds to C: stat_season_stat_new.
    /// </summary>
    /// <param name="seasonNumber">The season number.</param>
    /// <returns>The season stat.</returns>
    public static SeasonStat SeasonStatNew(int seasonNumber)
    {
        SeasonStat newStat = new SeasonStat();
        newStat.SeasonNumber = seasonNumber;
        newStat.LeagueChamps = new List<ChampStat>();
        newStat.CupChamps = new List<ChampStat>();
        newStat.LeagueStats = new List<LeagueStat>();

        return newStat;
    }

    /// <summary>
    /// Update the league stats.
    /// Corresponds to C: stat_update_leagues.
    /// </summary>
    public static void StatUpdateLeagues()
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (League.QueryLeagueActive(league) &&
                league.Fixtures.Count > 0 &&
                league.Fixtures[league.Fixtures.Count - 1].WeekNumber >= Variables.Week)
            {
                league.Stats = LeagueStatNew(league.Name, league.Symbol);
                league.Stats.TeamsOff = StatUpdateLeagueTeams(league.Teams, 4);
                league.Stats.TeamsDef = StatUpdateLeagueTeams(league.Teams, 5);
                StatUpdateLeaguePlayers(league);
            }
        }
    }

    /// <summary>
    /// Update the stats of the league.
    /// Corresponds to C: stat_update_league_players.
    /// </summary>
    /// <param name="league">The league.</param>
    public static void StatUpdateLeaguePlayers(League league)
    {
        int i, j;
        List<Player> playersSorted = new List<Player>();
        List<Player> playersGoalies = new List<Player>();
        int maxlen = Variables.Constants.GetInt("int_stat_players_len");

        for (i = 0; i < league.Teams.Count; i++)
            for (j = 0; j < league.Teams[i].Players.Count; j++)
            {
                Player pl = league.Teams[i].Players[j];
                if (pl.Pos != PlayerPos.Goalie)
                    playersSorted.Add(pl);
                else if (Player.GamesGoalsGet(pl, pl.Team.Clid, (int)PlayerValue.Games) >=
                    Variables.Constants.GetFloat("float_stat_goalie_percentage") *
                    (float)Team.GetTableValue(pl.Team, (int)TableElementValues.TablePlayed))
                    playersGoalies.Add(pl);
            }

        playersSorted.Sort((pl1, pl2) => Player.CompareFunc(pl1, pl2, (int)PlayerCompareAttrib.LeagueGoals));
        playersGoalies.Sort((pl1, pl2) => Player.CompareFunc(pl1, pl2, -(int)PlayerCompareAttrib.LeagueGoals));

        maxlen = Math.Min(maxlen, playersSorted.Count);
        maxlen = Math.Min(maxlen, playersGoalies.Count);

        List<Stat> players = new List<Stat>();
        List<Stat> playersGoaliesStats = new List<Stat>();
        for (i = 0; i < 2; i++)
            for (j = 0; j < maxlen; j++)
            {
                Player pl = i == 0 ? playersSorted[j] : playersGoalies[j];
                Stat newStat = new Stat();
                newStat.TeamName = pl.Team.Name;
                newStat.ValueString = pl.Name;
                newStat.Value1 =
                    Player.GamesGoalsGet(pl, pl.Team.Clid, (int)PlayerValue.Goals);
                newStat.Value2 =
                    Player.GamesGoalsGet(pl, pl.Team.Clid, (int)PlayerValue.Games);
                newStat.Value3 =
                    Player.GamesGoalsGet(pl, pl.Team.Clid, (int)PlayerValue.Shots);

                if (i == 0)
                    players.Add(newStat);
                else
                    playersGoaliesStats.Add(newStat);
            }

        league.Stats.PlayerScorers = players;
        league.Stats.PlayerGoalies = playersGoaliesStats;
    }

    /// <summary>
    /// Update the league team stats.
    /// Corresponds to C: stat_update_league_teams.
    /// </summary>
    /// <param name="teams">The teams.</param>
    /// <param name="compareType">The compare type.</param>
    /// <returns>The list of stats.</returns>
    public static List<Stat> StatUpdateLeagueTeams(List<Team> teams, int compareType)
    {
        List<Stat> stats = new List<Stat>();
        List<Team> sortedTeams = new List<Team>(teams);
        sortedTeams.Sort((tm1, tm2) => Team.CompareFunc(tm1, tm2, compareType));
        int count = Math.Min(teams.Count, sortedTeams.Count);

        for (int i = 0; i < count; i++)
        {
            Stat newStat = new Stat();
            newStat.TeamName = sortedTeams[i].Name;
            newStat.Value1 = Team.GetTableValue(sortedTeams[i], (int)TableElementValues.TableGf);
            newStat.Value2 = Team.GetTableValue(sortedTeams[i], (int)TableElementValues.TableGa);
            newStat.Value3 = -1;
            newStat.ValueString = null;
            stats.Add(newStat);
        }

        return stats;
    }

    /// <summary>
    /// Create a seasonstat struct at the end of a season.
    /// Corresponds to C: stat_create_season_stat.
    /// </summary>
    public static void StatCreateSeasonStat()
    {
        SeasonStat newStat = SeasonStatNew(Variables.Season);
        ChampStat newChamp;

        foreach (League league in Variables.Country.Leagues)
        {
            if (!League.QueryLeagueCupHasProperty(league.Id, "omit_from_history") &&
                !League.QueryLeagueCupHasProperty(league.Id, "inactive"))
            {
                foreach (Table table in league.Tables)
                {
                    newChamp = new ChampStat();
                    newChamp.ClName = table.Name;
                    newChamp.TeamName =
                        table.Elements[0].Team.Name;
                    newStat.LeagueChamps.Add(newChamp);
                }

                newStat.LeagueStats.Add(league.Stats);
                league.Stats = LeagueStatNew(league.Name, league.Symbol);
            }
        }

        foreach (Cup cup in Variables.Country.AllCups)
        {
            if (!League.QueryLeagueCupHasProperty(cup.Id, "omit_from_history"))
            {
                newChamp = new ChampStat();
                newChamp.ClName = cup.Name;
                newChamp.TeamName =
                    Cup.CupGetWinner(cup).Name;
                newStat.CupChamps.Add(newChamp);
            }
        }

        Variables.SeasonStats.Add(newStat);
    }

    /// <summary>
    /// Function printing goals stats from the fixture array.
    /// Used for fine-tuning the game.
    /// Corresponds to C: stat_show_av_goals.
    /// </summary>
    /// <param name="fixtures">The fixtures.</param>
    public static void StatShowAvGoals(List<Fixture> fixtures)
    {
        float games = 0,
            allgoals = 0,
            homegoals = 0,
            awaygoals = 0,
            goaldiff = 0,
            homewon = 0,
            homeloss = 0,
            draw = 0;

        foreach (Fixture fix in fixtures)
            if (fix.Attendance != -1)
            {
                games++;
                allgoals += fix.Result[0, 0] + fix.Result[1, 0];
                homegoals += fix.Result[0, 0];
                awaygoals += fix.Result[1, 0];
                goaldiff += Math.Abs(fix.Result[0, 0] - fix.Result[1, 0]);

                if (fix.Result[0, 0] > fix.Result[1, 0])
                    homewon++;
                else if (fix.Result[0, 0] < fix.Result[1, 0])
                    homeloss++;
                else
                    draw++;
            }

        Console.WriteLine("------------------------------------ ");
        Console.WriteLine(League.LeagueCupGetNameString(fixtures[0].Clid) +
            "\nGoals/G HomeG/G AwayG/G Away % GD/G \n");
        Console.WriteLine($"{allgoals / games:0.00} \t {homegoals / games:0.00} \t {awaygoals / games:0.00} \t {awaygoals / allgoals:0.00} \t {goaldiff / games:0.00} \n");
        Console.WriteLine($"Home win %: {homewon / games:0.00} Loss %: {homeloss / games:0.00} Draw %: {draw / games:0.00} \n");
    }

    /// <summary>
    /// Show league goals stats (debug).
    /// Corresponds to C: stat_show_av_league_goals.
    /// </summary>
    public static void StatShowAvLeagueGoals()
    {
        Console.WriteLine("\n\n");
        foreach (League league in Variables.Country.Leagues)
            StatShowAvGoals(league.Fixtures);
    }
}

/// <summary>
/// A structure holding some stat arrays about a league. 
/// Corresponds to C LeagueStat struct.
/// </summary>
public class LeagueStat
{
    /// <summary>The league symbol.</summary>
    public string LeagueSymbol { get; set; }

    /// <summary>The league name.</summary>
    public string LeagueName { get; set; }

    /// <summary>Best offensive and defensive teams. Corresponds to C: GArray *teams_off, *teams_def.</summary>
    public List<Stat> TeamsOff { get; set; } = new List<Stat>();

    /// <summary>Best defensive and offensive teams. Corresponds to C: GArray *teams_def, *teams_off.</summary>
    public List<Stat> TeamsDef { get; set; } = new List<Stat>();

    /// <summary>Best goal getters and goalies. Corresponds to C: GArray *player_scorers, *player_goalies.</summary>
    public List<Stat> PlayerScorers { get; set; } = new List<Stat>();

    /// <summary>Best goalies and goal scorers. Corresponds to C: GArray *player_goalies, *player_scorers.</summary>
    public List<Stat> PlayerGoalies { get; set; } = new List<Stat>();
}

/// <summary>
/// A team name and a competition name. 
/// Corresponds to C ChampStat struct.
/// </summary>
public class ChampStat
{
    /// <summary>The team name.</summary>
    public string TeamName { get; set; }

    /// <summary>The competition name.</summary>
    public string ClName { get; set; }
}

/// <summary>
/// A season statistics structure. 
/// Corresponds to C SeasonStat struct.
/// </summary>
public class SeasonStat
{
    /// <summary>Which season.</summary>
    public int SeasonNumber { get; set; }

    /// <summary>League and cup winners. Corresponds to C: GArray *league_champs, *cup_champs.</summary>
    public List<ChampStat> LeagueChamps { get; set; } = new List<ChampStat>();

    /// <summary>League and cup winners. Corresponds to C: GArray *cup_champs, *league_champs.</summary>
    public List<ChampStat> CupChamps { get; set; } = new List<ChampStat>();

    /// <summary>The league stats at the end of the season. Corresponds to C: GArray *league_stats.</summary>
    public List<LeagueStat> LeagueStats { get; set; } = new List<LeagueStat>();
}

/// <summary>
/// A rank entry used within a larger statistical body.
/// </summary>
public class TeamRankEntry
{
    public string TeamName { get; set; }
}

/// <summary>
/// A rank entry for a player.
/// </summary>
public class PlayerRankEntry
{
    public string PlayerName { get; set; }
}
