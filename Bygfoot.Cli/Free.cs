namespace Bygfoot.Cli;

/// <summary>
/// Static class for freeing memory. Memory is garbage collected in C#,
/// so all of these functions do nothing.
/// Corresponds to the C module src/free.h and src/free.c
/// (free_memory, free_transfer_list, free_season_stats, free_users, free_user,
/// free_mmatches, free_event, free_option_list, free_live_game, free_country,
/// free_leagues_array, free_league, free_prom_rel, free_joined_leagues,
/// free_league_stats, free_tables, free_new_tables, free_teams_array, free_team,
/// free_player_array, free_player, free_cups_array, free_cup, free_cup_round,
/// free_cup_choose_team, free_variables, free_lg_commentary, free_news,
/// free_newspaper, free_newspaper_article, free_gchar_array, free_g_string,
/// free_g_array, free_g_ptr_array, free_support_dirs, free_name_list, free_names,
/// free_strategies, free_bets, free_job, free_jobs).
/// </summary>
public static class Free
{
    /// <summary>Free the memory we have allocated. Does nothing in C# (memory is garbage collected).</summary>
    public static void FreeMemory()
    {
    }

    /// <summary>Free the transfer list. Does nothing in C# (memory is garbage collected).</summary>
    public static void FreeTransferList()
    {
    }

    /// <summary>Free the season stats. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeSeasonStats(bool reset)
    {
    }

    /// <summary>Free the user array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeUsers(bool reset)
    {
    }

    /// <summary>Free a user. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="user">The user.</param>
    public static void FreeUser(User user)
    {
    }

    /// <summary>Free the memorized matches. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="mmatches">The memorized matches.</param>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeMmatches(List<MemMatch> mmatches, bool reset)
    {
    }

    /// <summary>Free an event. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="event">The event.</param>
    public static void FreeEvent(Event @event)
    {
    }

    /// <summary>Free an option list. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="optionList">The option list.</param>
    /// <param name="reset">Whether to reset the list.</param>
    public static void FreeOptionList(OptionList optionList, bool reset)
    {
    }

    /// <summary>Free a live game. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="match">The live game.</param>
    public static void FreeLiveGame(LiveGame match)
    {
    }

    /// <summary>Free the country. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="country">The country.</param>
    /// <param name="reset">Whether to reset the country.</param>
    public static void FreeCountry(Country country, bool reset)
    {
    }

    /// <summary>Free the leagues array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="leagues">The leagues.</param>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeLeaguesArray(List<League> leagues, bool reset)
    {
    }

    /// <summary>Free a league. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="league">The league.</param>
    public static void FreeLeague(League league)
    {
    }

    /// <summary>Free the promotion/relegation variable. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="promRel">The promotion/relegation variable.</param>
    public static void FreePromRel(PromRel promRel)
    {
    }

    /// <summary>Free the joined leagues. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="joinedLeagues">The joined leagues.</param>
    public static void FreeJoinedLeagues(List<JoinedLeague> joinedLeagues)
    {
    }

    /// <summary>Free the league stats. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="stats">The league stats.</param>
    public static void FreeLeagueStats(LeagueStat stats)
    {
    }

    /// <summary>Free the tables. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="tables">The tables.</param>
    public static void FreeTables(List<Table> tables)
    {
    }

    /// <summary>Free the new tables. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="newTables">The new tables.</param>
    public static void FreeNewTables(List<NewTable> newTables)
    {
    }

    /// <summary>Free the teams array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="teams">The teams.</param>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeTeamsArray(List<Team> teams, bool reset)
    {
    }

    /// <summary>Free a team. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="team">The team.</param>
    public static void FreeTeam(Team team)
    {
    }

    /// <summary>Free the players array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="players">The players.</param>
    public static void FreePlayerArray(List<Player> players)
    {
    }

    /// <summary>Free a player. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="pl">The player.</param>
    public static void FreePlayer(Player pl)
    {
    }

    /// <summary>Free the cups array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="cups">The cups.</param>
    /// <param name="reset">Whether to reset the array.</param>
    public static void FreeCupsArray(List<Cup> cups, bool reset)
    {
    }

    /// <summary>Free a cup. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="cup">The cup.</param>
    public static void FreeCup(Cup cup)
    {
    }

    /// <summary>Free a cup round. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="cupRound">The cup round.</param>
    public static void FreeCupRound(CupRound cupRound)
    {
    }

    /// <summary>Free a choose team. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="cupChooseTeam">The choose team.</param>
    public static void FreeCupChooseTeam(CupChooseTeam cupChooseTeam)
    {
    }

    /// <summary>Free the global variables. Does nothing in C# (memory is garbage collected).</summary>
    public static void FreeVariables()
    {
    }

    /// <summary>Free the live game commentary. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the commentary.</param>
    public static void FreeLgCommentary(bool reset)
    {
    }

    /// <summary>Free the news. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the news.</param>
    public static void FreeNews(bool reset)
    {
    }

    /// <summary>Free the newspaper. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the newspaper.</param>
    public static void FreeNewspaper(bool reset)
    {
    }

    /// <summary>Free a newspaper article. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="article">The newspaper article.</param>
    public static void FreeNewspaperArticle(NewsPaperArticle article)
    {
    }

    /// <summary>Free an array of strings. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="array">The array of strings.</param>
    public static void FreeGcharArray(object array)
    {
    }

    /// <summary>Free a string. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="str">The string.</param>
    public static void FreeGString(object str)
    {
    }

    /// <summary>Free an array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="array">The array.</param>
    public static void FreeGArray(object array)
    {
    }

    /// <summary>Free a pointer array. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="array">The pointer array.</param>
    public static void FreeGPtrArray(object array)
    {
    }

    /// <summary>Free the support directories. Does nothing in C# (memory is garbage collected).</summary>
    public static void FreeSupportDirs()
    {
    }

    /// <summary>Free a name list. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="nameList">The name list.</param>
    /// <param name="reset">Whether to reset the name list.</param>
    public static void FreeNameList(NameList nameList, bool reset)
    {
    }

    /// <summary>Free the name lists. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the name lists.</param>
    public static void FreeNames(bool reset)
    {
    }

    /// <summary>Free the strategies. Does nothing in C# (memory is garbage collected).</summary>
    public static void FreeStrategies()
    {
    }

    /// <summary>Free the bets. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the bets.</param>
    public static void FreeBets(bool reset)
    {
    }

    /// <summary>Free a job. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="job">The job.</param>
    /// <param name="freeTm">Whether to free the job team too.</param>
    public static void FreeJob(Job job, bool freeTm)
    {
    }

    /// <summary>Free the jobs. Does nothing in C# (memory is garbage collected).</summary>
    /// <param name="reset">Whether to reset the jobs.</param>
    public static void FreeJobs(bool reset)
    {
    }
}
