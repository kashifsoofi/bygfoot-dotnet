namespace Bygfoot.Cli;

/// <summary>
/// Static class for starting and ending the game, season, week and week round.
/// Corresponds to the C module src/start_end.h and src/start_end.c
/// (start_new_game, start_new_season, start_write_variables, start_generate_league_teams,
/// start_new_season_reset_ids, end_week_round, end_week_round_results, end_week_round_sort_tables,
/// end_week_round_update_fixtures, end_week_round_generate_news, start_week_round, start_week,
/// end_week, end_week_hide_cups, start_week_add_cups, start_week_update_teams,
/// start_week_round_update_teams, start_week_update_user_finances, start_week_update_users,
/// start_week_update_leagues, query_start_end_season_end, start_new_season_league_changes
/// and end_season).
/// </summary>
public static class StartEnd
{
    /// <summary>Generate the teams etc.</summary>
    public static void StartNewGame()
    {
        StartWriteVariables();

        StartGenerateLeagueTeams();
        StartNewSeason();
    }

    /// <summary>Make new fixtures, nullify things etc.</summary>
    public static void StartNewSeason()
    {
        Variables.Week = 1;
        Variables.WeekRound = 1;
        Free.FreeNames(true);
        Bygfoot.Stat5 = (int)Status0Value.StatusGenerateTeams;

        if (Variables.Season == 1)
        {
            for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            {
                League.AddTable(Bygfoot.GetLeague(i));
                League.CheckNewTables(Bygfoot.GetLeague(i));
            }

            for (int i = 0; i < Variables.Country.Cups.Count; i++)
                if (Bygfoot.GetCup(i).AddWeek <= 0)
                    Variables.Country.AllCups.Add(Bygfoot.GetCup(i));
        }

        /* Remove cups that don't start at the beginning of season. */
        for (int i = Variables.Country.AllCups.Count - 1; i >= 0; i--)
        {
            Cup cup = Bygfoot.GetAllCup(i);
            cup.TeamNames = new List<string>();

            if (cup.AddWeek > 0)
                Variables.Country.AllCups.RemoveAt(i);
        }

        /* Deal with cups that have to take place before promotion/relegation. */
        for (int i = Variables.Country.Cups.Count - 1; i >= 0; i--)
            if (Bygfoot.GetCup(i).AddWeek == -1)
                Fixture.WriteCupFixtures(Bygfoot.GetCup(i));

        if (Variables.Season > 1)
        {
            for (int i = 0; i < Variables.Users.Count; i++)
            {
                User usr = Variables.Users[i];
                string rankBuf = Team.GetLeagueRank(usr.Team, -1).ToString();
                User.HistoryAdd(usr, (int)UserHistoryType.EndSeason,
                                usr.Team.Name,
                                League.LeagueCupGetNameString(usr.Team.Clid),
                                rankBuf, null);

                usr.Counters[(int)CounterType.TrainingCampsLeftSeason] =
                    OptionHelper.ConstInt("int_training_camps_per_season");
            }

            /* Promotion/relegation, mainly. */
            StartNewSeasonLeagueChanges();

            for (int i = 0; i < Variables.Users.Count; i++)
            {
                User usr = Variables.Users[i];
                usr.Team = Team.OfId(usr.TeamId);

                usr.YouthAcademy.Team = usr.Team;
                for (int j = 0; j < usr.YouthAcademy.Players.Count; j++)
                {
                    usr.YouthAcademy.Players[j].Team = usr.Team;
                    Player.SeasonStart(usr.YouthAcademy.Players[j], 0f);
                }

                LiveGame.Reset(usr.LiveGame, null, true);
            }

            StartNewSeasonResetIds();
        }

        /* We have to reset all fixture arrays beforehand because
           of interleague scheduling (see joined_league). */
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            if (League.QueryLeagueActive(Bygfoot.GetLeague(i)))
                Bygfoot.GetLeague(i).Fixtures = new List<Fixture>();

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            if (League.QueryLeagueActive(Bygfoot.GetLeague(i)))
                Fixture.WriteLeagueFixtures(Bygfoot.GetLeague(i));

        /* Loop backwards because the national cup is usually
           the first the results are often still
           needed for the international cups. */
        for (int i = Variables.Country.Cups.Count - 1; i >= 0; i--)
        {
            Cup cup = Bygfoot.GetCup(i);
            if (cup.AddWeek == 0)
                Fixture.WriteCupFixtures(cup);
            /* Reset team pointers using the stored ids
               (pointers might have changed because of prom/rel). */
            else if (cup.AddWeek == -1)
                Fixture.RefreshTeamPointers(cup.Fixtures);
            else if (!Cup.QueryCupSelfReferential(cup))
                Cup.CupReset(cup);
        }

        for (int i = Variables.Country.AllCups.Count - 1; i >= 0; i--)
            if (!Cup.CupCheckFixtures(Bygfoot.GetAllCup(i)))
            {
                Cup.CupReset(Bygfoot.GetAllCup(i));
                Variables.Country.AllCups.RemoveAt(i);
            }

        Bygfoot.Stat5 = -1;

        for (int i = 0; i < Variables.NameLists.Count; i++)
            NameList.ShortenList(Variables.NameLists[i]);

        if (Variables.Season == 1)
            Bet.Update();
    }

    /// <summary>Fill some global variables with default values at the
    /// beginning of a new game.</summary>
    public static void StartWriteVariables()
    {
        Variables.Season = 1;
        Variables.CurUser = 0;

        for (int i = 0; i < Variables.Users.Count; i++)
            FileHelper.LoadUserConfFile(Variables.Users[i]);
    }

    /// <summary>Generate the teams in the leagues.</summary>
    public static void StartGenerateLeagueTeams()
    {
        Bygfoot.Stat5 = (int)Status0Value.StatusGenerateTeams;

        if (Variables.Country.Leagues.Count == 0)
        {
            // main_exit_program(EXIT_NO_LEAGUES, ...)
            Console.WriteLine(
                "start_generate_league_teams: no leagues found. There must be at least one league in the game.");
            Environment.Exit((int)Bygfoot.ExitCode.NoLeagues);
        }

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            for (int j = 0; j < Bygfoot.GetLeague(i).Teams.Count; j++)
                Team.GeneratePlayersStadium(Bygfoot.GetLeague(i).Teams[j], 0f);

        Bygfoot.Stat5 = -1;
    }

    /// <summary>Reset the cup and league ids to the smallest possible
    /// value to avoid an overflow (gotta admit, the id system
    /// isn't perfect).</summary>
    public static void StartNewSeasonResetIds()
    {
        int max = -1;
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            if (Bygfoot.GetLeague(i).Id > max)
                max = Bygfoot.GetLeague(i).Id;

        Variables.Counters[(int)Counters.CountLeagueId] = max + 1;

        max = -1;
        for (int i = 0; i < Variables.Country.Cups.Count; i++)
            if (Bygfoot.GetCup(i).Id > max)
                max = Bygfoot.GetCup(i).Id;

        Variables.Counters[(int)Counters.CountCupId] = max + 1;
    }

    /// <summary>End a week round.</summary>
    public static void EndWeekRound()
    {
        if (Variables.DebugLevel > 100)
            Console.WriteLine($"End w {Variables.Week} r {Variables.WeekRound} ");

        // start_week_round_funcs and end_week_round_funcs arrays.
        EndWeekRoundResults();
        EndWeekRoundSortTables();
        EndWeekRoundGenerateNews();
        EndWeekRoundUpdateFixtures();

        Variables.WeekRound++;

        bool newWeek = true;
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            if (Fixture.QueryFixtureInWeekRound(Bygfoot.GetLeague(i).Id, Variables.Week, Variables.WeekRound))
            {
                newWeek = false;
                break;
            }

        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
            if (Fixture.QueryFixtureInWeekRound(Bygfoot.GetAllCup(i).Id, Variables.Week, Variables.WeekRound))
            {
                newWeek = false;
                break;
            }

        if (newWeek)
        {
            EndWeek();

            if (QueryStartEndSeasonEnd())
            {
                EndSeason();

                Variables.Season++;
                Variables.Week = 1;

                StartNewSeason();
            }
            else
                Variables.Week++;

            Variables.WeekRound = 1;
            StartWeek();
        }

        StartWeekRound();
    }

    /// <summary>Calculate the match results of a week round.</summary>
    public static void EndWeekRoundResults()
    {
        int done = 0;
        float numMatches = Fixture.GetNumberOfMatches(Variables.Week, Variables.WeekRound);

        /** Free the matches from last week. */
        for (int i = 0; i < Variables.LiveGames.Count; i++)
            Free.FreeLiveGame(Variables.LiveGames[i]);
        Variables.LiveGames.Clear();

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            League league = Bygfoot.GetLeague(i);
            for (int j = 0; j < league.Fixtures.Count; j++)
            {
                Fixture fix = league.Fixtures[j];
                if (fix.WeekNumber == Variables.Week &&
                   fix.WeekRoundNumber == Variables.WeekRound &&
                   fix.Attendance == -1)
                {
                    int usrIdx = Fixture.UserTeamInvolved(fix);

                    if (usrIdx == -1)
                    {
                        Variables.LiveGames.Add(new LiveGame());
                        LiveGame.CalculateFixture(fix, Variables.LiveGames[Variables.LiveGames.Count - 1]);
                    }
                    else
                        LiveGame.CalculateFixture(fix, Variables.Users[usrIdx].LiveGame);

                    done++;
                    string buf = string.Empty;
                    Fixture.ResultToBuf(fix, ref buf, false);
                    string buf2 = $"{fix.Teams[0].Name} {buf} {fix.Teams[1].Name}";

                    Gui.GuiShowProgress((float)done / numMatches, buf2, PicType.PicTypeMatchpic);

                    if (Variables.DebugLevel > 120)
                        Console.WriteLine($"{buf2} ");
                }
            }
        }

        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
        {
            Cup cup = Bygfoot.GetAllCup(i);
            for (int j = 0; j < cup.Fixtures.Count; j++)
            {
                Fixture fix = cup.Fixtures[j];
                if (fix.WeekNumber == Variables.Week &&
                   fix.WeekRoundNumber == Variables.WeekRound &&
                   fix.Attendance == -1)
                {
                    int usrIdx = Fixture.UserTeamInvolved(fix);

                    if (usrIdx == -1)
                    {
                        Variables.LiveGames.Add(new LiveGame());
                        LiveGame.CalculateFixture(fix, Variables.LiveGames[Variables.LiveGames.Count - 1]);
                    }
                    else
                        LiveGame.CalculateFixture(fix, Variables.Users[usrIdx].LiveGame);

                    done++;
                    string buf = string.Empty;
                    Fixture.ResultToBuf(fix, ref buf, false);
                    string buf2 = $"{fix.Teams[0].Name} {buf} {fix.Teams[1].Name}";

                    Gui.GuiShowProgress((float)done / numMatches, buf2, PicType.PicTypeMatchpic);

                    if (Variables.DebugLevel > 120)
                        Console.WriteLine($"{buf2} ");
                }
            }
        }

        Gui.GuiShowProgress(-1, "", PicType.PicTypeMatchpic);
    }

    /// <summary>Sort league and cup tables.</summary>
    public static void EndWeekRoundSortTables()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            if (Fixture.QueryFixtureInWeekRound(Bygfoot.GetLeague(i).Id, Variables.Week, Variables.WeekRound))
            {
                League league = Bygfoot.GetLeague(i);
                for (int k = 0; k < league.Tables.Count; k++)
                {
                    Table table = league.Tables[k];
                    for (int j = 0; j < table.Elements.Count; j++)
                        table.Elements[j].OldRank = j;

                    table.Elements.Sort(
                        (element1, element2) => Table.TableElementCompareFunc(element1, element2, league.Id));
                }
            }

        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
        {
            Cup cup = Bygfoot.GetAllCup(i);
            if (Fixture.QueryFixtureInWeekRound(cup.Id, Variables.Week, Variables.WeekRound) &&
               cup.Fixtures[cup.Fixtures.Count - 1].Round == Cup.CupHasTables(cup.Id))
            {
                List<Table> tables = cup.Rounds[Cup.CupHasTables(cup.Id)].Tables;
                for (int j = 0; j < tables.Count; j++)
                {
                    for (int k = 0; k < tables[j].Elements.Count; k++)
                        tables[j].Elements[k].OldRank = k;

                    tables[j].Elements.Sort(
                        (element1, element2) => Table.TableElementCompareFunc(element1, element2, cup.Id));
                }
            }
        }
    }

    /// <summary>Update cup fixtures.</summary>
    public static void EndWeekRoundUpdateFixtures()
    {
        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
            if (Bygfoot.GetAllCup(i).NextFixtureUpdateWeek <= Variables.Week &&
               Bygfoot.GetAllCup(i).NextFixtureUpdateWeekRound <= Variables.WeekRound)
                Fixture.Update(Bygfoot.GetAllCup(i));

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            League league = Bygfoot.GetLeague(i);
            if (League.QueryLeagueActive(league) && Variables.Week ==
               league.Fixtures[league.Fixtures.Count - 1].WeekNumber &&
               Variables.WeekRound == 1 &&
               Team.IsUser(league.Tables[league.Tables.Count - 1].Elements[0].Team) != -1)
                User.HistoryAdd(
                    Variables.Users[Team.IsUser(league.Tables[league.Tables.Count - 1].Elements[0].Team)],
                    (int)UserHistoryType.Champion,
                    league.Tables[league.Tables.Count - 1].Elements[0].Team.Name,
                    League.LeagueCupGetNameString(league.Id),
                    null, null);
        }

        for (int i = 0; i < Variables.Country.Cups.Count; i++)
        {
            Cup cup = Bygfoot.GetCup(i);
            if (cup.AddWeek == 1000 &&
               Cup.QueryCupHidden(cup) &&
               Cup.QueryCupBegins(cup))
            {
                cup.LastWeek = Cup.CupGetLastWeekFromFirst(cup, Variables.Week + 1);

                if (Fixture.WriteCupFixtures(cup))
                    Variables.Country.AllCups.Add(cup);
            }
        }
    }

    /// <summary>Write newspaper articles after week round.</summary>
    public static void EndWeekRoundGenerateNews()
    {
        /** News for user matches. */
        for (int i = 0; i < Variables.Users.Count; i++)
            if (Variables.Users[i].LiveGame.Fix != null &&
               Variables.Users[i].LiveGame.Fix.WeekNumber == Variables.Week &&
               Variables.Users[i].LiveGame.Fix.WeekRoundNumber == Variables.WeekRound &&
               News.CheckMatchRelevant(Variables.Users[i].LiveGame))
                News.GenerateMatch(Variables.Users[i].LiveGame);

        /** News for other matches. */
        for (int i = 0; i < Variables.LiveGames.Count; i++)
            if (News.CheckMatchRelevant(Variables.LiveGames[i]))
                News.GenerateMatch(Variables.LiveGames[i]);

        /* Get rid of older news. */
        while (Variables.Newspaper.Articles.Count > OptionHelper.ConstInt("int_news_history_length"))
        {
            Free.FreeNewspaperArticle(Variables.Newspaper.Articles[0]);
            Variables.Newspaper.Articles.RemoveAt(0);
        }
    }

    /// <summary>Start a new week round.</summary>
    public static void StartWeekRound()
    {
        if (Variables.DebugLevel > 100)
            Console.WriteLine($"Start w {Variables.Week} r {Variables.WeekRound} ");

        // start_week_round_funcs array.
        StartWeekRoundUpdateTeams();
        Bet.Update();

        if (!QueryStartEndSeasonEnd() &&
           OptionHelper.OptInt("int_opt_skip") != 0 &&
           !User.QueryUserGamesInWeekRound(Variables.Week, Variables.WeekRound) &&
           !User.QueryUserEvents() &&
           ((Variables.WeekRound == 1 &&
             !User.QueryUserGamesInWeekRound(Variables.Week - 1, Fixture.GetLastWeekRound(Variables.Week - 1))) ||
            (Variables.WeekRound > 1 &&
             !User.QueryUserGamesInWeekRound(Variables.Week, Variables.WeekRound - 1))))
        {
            User.EventShowNext();
            EndWeekRound();
        }
        else
        {
            Variables.CurUser = 0;

            // game_gui_show_main();
            // TODO: Call the C function game_gui_show_main (not yet converted).

            User.EventShowNext();
        }
    }

    /// <summary>Start a new week.</summary>
    public static void StartWeek()
    {
        // start_week_funcs array.
        StartWeekUpdateLeagues();
        StartWeekAddCups();
        StartWeekUpdateUsers();
        StartWeekUpdateTeams();
        StartWeekUpdateUserFinances();
        YouthAcademy.UpdateWeekly();
        Transfer.TransferUpdate();
        Job.Update();
        Finance.UpdateCurrentInterest();
    }

    /// <summary>Finish a week.</summary>
    public static void EndWeek()
    {
        // end_week_funcs array.
        Stat.StatUpdateLeagues();
        EndWeekHideCups();

        if (Variables.DebugLevel > 150)
            Stat.StatShowAvLeagueGoals();
    }

    /// <summary>Hide some not-so-important cups that
    /// are already finished.</summary>
    public static void EndWeekHideCups()
    {
        for (int i = Variables.Country.AllCups.Count - 1; i >= 0; i--)
            if (League.QueryLeagueCupHasProperty(Bygfoot.GetAllCup(i).Id, "hide") &&
               Bygfoot.GetAllCup(i).Fixtures[Bygfoot.GetAllCup(i).Fixtures.Count - 1].Attendance > 0)
                Variables.Country.AllCups.RemoveAt(i);
    }

    /// <summary>Add the cups that begin later in the season to the acps array.</summary>
    public static void StartWeekAddCups()
    {
        for (int i = 0; i < Variables.Country.Cups.Count; i++)
            if (Bygfoot.GetCup(i).AddWeek == Variables.Week)
            {
                if (Fixture.WriteCupFixtures(Bygfoot.GetCup(i)))
                    Variables.Country.AllCups.Add(Bygfoot.GetCup(i));
            }
    }

    /// <summary>Age increase etc. of players.
    /// CPU teams get updated at the end of their matches
    /// (to avoid cup teams getting updated too often).</summary>
    public static void StartWeekUpdateTeams()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            for (int j = 0; j < Bygfoot.GetLeague(i).Teams.Count; j++)
                Team.UpdateTeamWeekly(Bygfoot.GetLeague(i).Teams[j]);

        for (int i = 0; i < Variables.Country.Cups.Count; i++)
            for (int j = 0; j < Bygfoot.GetCup(i).Teams.Count; j++)
                Team.UpdateTeamWeekly(Team.OfId(Bygfoot.GetCup(i).Teams[j]));
    }

    /// <summary>Do some things at the beginning of each new round for
    /// the user teams.</summary>
    public static void StartWeekRoundUpdateTeams()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            for (int j = 0; j < Bygfoot.GetLeague(i).Teams.Count; j++)
                Team.UpdateTeamWeekRoundly(Bygfoot.GetLeague(i).Teams[j]);

        for (int i = 0; i < Variables.Country.Cups.Count; i++)
            for (int j = 0; j < Bygfoot.GetCup(i).Teams.Count; j++)
                Team.UpdateTeamWeekRoundly(Team.OfId(Bygfoot.GetCup(i).Teams[j]));
    }

    /// <summary>Deduce wages etc.</summary>
    public static void StartWeekUpdateUserFinances()
    {
        if (OptionHelper.SettInt("int_opt_disable_finances") != 0)
            return;

        for (int i = 0; i < Variables.Users.Count; i++)
            Finance.UpdateUserWeekly(Variables.Users[i]);
    }

    /// <summary>Some general user update.</summary>
    public static void StartWeekUpdateUsers()
    {
        for (int i = 0; i < Variables.Users.Count; i++)
        {
            User usr = Variables.Users[i];
            if (usr.Scout >= 100)
                usr.Scout = Misc.MathGetPlace(usr.Scout, 2);

            if (usr.Physio >= 100)
                usr.Physio = Misc.MathGetPlace(usr.Physio, 2);

            if (usr.YouthAcademy.Coach >= 100)
                usr.YouthAcademy.Coach = Misc.MathGetPlace(usr.YouthAcademy.Coach, 2);

            User.WeeklyUpdateCounters(usr);
        }
    }

    /// <summary>Create new tables if necessary etc.</summary>
    public static void StartWeekUpdateLeagues()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            League.CheckNewTables(Bygfoot.GetLeague(i));
    }

    /// <summary>Check whether the season has ended.</summary>
    /// <returns>Whether the season has ended.</returns>
    public static bool QueryStartEndSeasonEnd()
    {
        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            for (int j = 0; j < Bygfoot.GetLeague(i).Fixtures.Count; j++)
                if (Bygfoot.GetLeague(i).Fixtures[j].WeekNumber > Variables.Week - 1)
                    return false;

        for (int i = 0; i < Variables.Country.AllCups.Count; i++)
            for (int j = 0; j < Bygfoot.GetAllCup(i).Fixtures.Count; j++)
                if (Bygfoot.GetAllCup(i).Fixtures[j].WeekNumber > Variables.Week - 1)
                    return false;

        return true;
    }

    /// <summary>Manage promotions and relegations at the beginning of a new season.</summary>
    public static void StartNewSeasonLeagueChanges()
    {
        int[] leagueSize = new int[Variables.Country.Leagues.Count];
        List<League.TeamMove> teamMovements = new List<League.TeamMove>();

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            League.GetTeamMovements(Bygfoot.GetLeague(i), teamMovements);

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
            leagueSize[i] = Bygfoot.GetLeague(i).Teams.Count;

        for (int i = 0; i < teamMovements.Count; i++)
            League.RemoveTeamWithId(
                League.LeagueFromClid(teamMovements[i].Tm.Clid),
                teamMovements[i].Tm.Id);

        League.TeamMovementsDestinations(teamMovements, leagueSize);

        for (int i = teamMovements.Count - 1; i >= 0; i--)
            if (teamMovements[i].PromRelType == PromRelType.Relegation)
                Bygfoot.GetLeague(teamMovements[i].DestIdcs[0]).Teams.Insert(0, teamMovements[i].Tm);

        for (int i = 1; i < teamMovements.Count; i++)
            if (teamMovements[i].PromRelType != PromRelType.Relegation)
                Bygfoot.GetLeague(teamMovements[i].DestIdcs[0]).Teams.Add(teamMovements[i].Tm);

        // Freeing the dest_idcs arrays and team_movements is a no-op in C# (no Free method in free.c for TeamMove).

        for (int i = 0; i < Variables.Country.Leagues.Count; i++)
        {
            League league = Bygfoot.GetLeague(i);
            for (int j = 0; j < league.Teams.Count; j++)
            {
                league.Teams[j].Clid = league.Id;
                for (int k = 0; k < league.Teams[j].Players.Count; k++)
                    league.Teams[j].Players[k].Team = league.Teams[j];
            }

            League.SeasonStart(league);
        }
    }

    /// <summary>End a season (store stats etc.)</summary>
    public static void EndSeason()
    {
        Stat.StatCreateSeasonStat();
    }
}
