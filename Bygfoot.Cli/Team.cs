namespace Bygfoot.Cli;

/// <summary>
/// Values for league/cup metadata related to a team. Corresponds to C enum LeagueCupValue.
/// </summary>
public enum LeagueCupValue
{
    Name = 0,               // LEAGUE_CUP_VALUE_NAME
    ShortName,              // LEAGUE_CUP_VALUE_SHORT_NAME
    Sid,                    // LEAGUE_CUP_VALUE_SID
    Symbol,                 // LEAGUE_CUP_VALUE_SYMBOL
    Id,                     // LEAGUE_CUP_VALUE_ID
    FirstWeek,              // LEAGUE_CUP_VALUE_FIRST_WEEK
    LastWeek,               // LEAGUE_CUP_VALUE_LAST_WEEK
    WeekGap,                // LEAGUE_CUP_VALUE_WEEK_GAP
    YellowRed,              // LEAGUE_CUP_VALUE_YELLOW_RED
    AverageSkill,           // LEAGUE_CUP_VALUE_AVERAGE_SKILL
    AverageCapacity,        // LEAGUE_CUP_VALUE_AVERAGE_CAPACITY
    SkillDiff,              // LEAGUE_CUP_VALUE_SKILL_DIFF
    End                     // LEAGUE_CUP_VALUE_END
}

/// <summary>
/// Team attributes. Corresponds to C enum TeamAttribute.
/// </summary>
public enum TeamAttribute
{
    Style = 0,              // TEAM_ATTRIBUTE_STYLE
    Boost,                  // TEAM_ATTRIBUTE_BOOST
    End                     // TEAM_ATTRIBUTE_END
}

/// <summary>
/// The stadium of a team. Corresponds to C struct Stadium.
/// </summary>
public class Stadium
{
    /// <summary>Name of the stadium. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>How many people fit in. Corresponds to C: gint capacity.</summary>
    public int Capacity { get; set; }

    /// <summary>How many people watched on average. Corresponds to C: gint average_attendance.</summary>
    public int AverageAttendance { get; set; }

    /// <summary>How many people would've watched if every game had been sold out. Corresponds to C: gint possible_attendance.</summary>
    public int PossibleAttendance { get; set; }

    /// <summary>Number of games. Corresponds to C: gint games.</summary>
    public int Games { get; set; }

    /// <summary>Safety percentage between 0 and 100. Corresponds to C: gfloat safety.</summary>
    public float Safety { get; set; }

    /// <summary>Ticket price. Corresponds to C: gfloat ticket_price.</summary>
    public float TicketPrice { get; set; }
}

/// <summary>
/// Structure representing a team. Corresponds to C struct Team.
/// </summary>
public class Team
{
    /// <summary>Name of the team. Corresponds to C: gchar *name.</summary>
    public string Name { get; set; }

    /// <summary>Symbol of the team. Corresponds to C: gchar *symbol.</summary>
    public string Symbol { get; set; }

    /// <summary>File the team takes the player names from. Corresponds to C: gchar *names_file.</summary>
    public string NamesFile { get; set; }

    /// <summary>The def file. Corresponds to C: gchar *def_file.</summary>
    public string DefFile { get; set; }

    /// <summary>The sid of the strategy if it's a CPU team. Corresponds to C: gchar *strategy_sid.</summary>
    public string StrategySid { get; set; }

    /// <summary>Numerical id of the league or cup the team belongs to. Corresponds to C: gint clid.</summary>
    public int Clid { get; set; }

    /// <summary>Id of the team. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>Playing structure. Corresponds to C: gint structure.</summary>
    public int Structure { get; set; }

    /// <summary>Playing style. Corresponds to C: gint style.</summary>
    public int Style { get; set; }

    /// <summary>Whether player boost or anti-boost is switched on. Corresponds to C: gint boost.</summary>
    public int Boost { get; set; }

    /// <summary>Average talent of the players at generation. Corresponds to C: gfloat average_talent.</summary>
    public float AverageTalent { get; set; }

    /// <summary>A value that influences scoring chances etc. If > 1, the team's lucky, if < 1, it's unlucky. Corresponds to C: gfloat luck.</summary>
    public float Luck { get; set; }

    /// <summary>The stadium of the team. Corresponds to C: Stadium stadium.</summary>
    public Stadium Stadium { get; set; }

    /// <summary>Array of players. Corresponds to C: GArray *players.</summary>
    public List<Player> Players { get; set; } = new List<Player>();

    /// <summary>
    /// Create a new team with default values, e.g. random playing structure and an empty string as name.
    /// Corresponds to C: team_new.
    /// </summary>
    /// <param name="newId">Whether a new id should be created.</param>
    /// <returns>A new team.</returns>
    public static Team New(bool newId)
    {
        Team newTeam = new Team();
        newTeam.Name = string.Empty;
        newTeam.Symbol = string.Empty;
        newTeam.NamesFile = string.Empty;
        newTeam.DefFile = string.Empty;
        newTeam.StrategySid = string.Empty;
        newTeam.Clid = -1;
        newTeam.Id = newId ? Bygfoot.TeamIdNew() : -1;
        newTeam.Structure = 442;
        newTeam.Style = 0;
        newTeam.Boost = 0;
        newTeam.AverageTalent = 0;
        newTeam.Luck = 1;
        newTeam.Players = new List<Player>();
        newTeam.Stadium = new Stadium();
        return newTeam;
    }

    /// <summary>
    /// Generate a team with default values, e.g. random playing structure and an empty string as name.
    /// Corresponds to C: team_generate_players_stadium.
    /// </summary>
    /// <param name="tm">The team that gets filled.</param>
    /// <param name="avTalent">The average talent.</param>
    public static void GeneratePlayersStadium(Team tm, float avTalent)
    {
        int i;
        float skillFactor = (float)MathHelper.Rnd(1 - Variables.Constants.GetFloat("float_team_skill_variance"),
                                                  1 + Variables.Constants.GetFloat("float_team_skill_variance"));
        float wages = 0, averageTalent, leagueAvTalent;
        string defFile = TeamHasDefFile(tm);

        tm.StrategySid = Strategy.GetRandom();

        tm.Stadium.AverageAttendance = tm.Stadium.PossibleAttendance =
            tm.Stadium.Games = 0;
        tm.Stadium.Safety =
            (float)MathHelper.Rnd(Variables.Constants.GetFloat("float_team_stadium_safety_lower"),
                                  Variables.Constants.GetFloat("float_team_stadium_safety_upper"));
        tm.Stadium.TicketPrice = Variables.Constants.GetInt("int_team_stadium_ticket_price");

        if (tm.Clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClidStatic(tm.Clid);
            leagueAvTalent = (avTalent > 0) ?
                avTalent : (league != null ? league.AverageTalent : 0f);
            averageTalent = (tm.AverageTalent == 0) ?
                skillFactor * leagueAvTalent :
                tm.AverageTalent;
        }
        else
        {
            Cup cup = Cup.CupFromClid(tm.Clid);
            averageTalent = (tm.AverageTalent == 0) ?
                GetAverageTalents(Variables.Country.Leagues[0].Teams) *
                (1 + cup.TalentDiff) :
                tm.AverageTalent;
            averageTalent *= skillFactor;
        }

        averageTalent = Math.Clamp((float)averageTalent, 0, Variables.Constants.GetFloat("float_player_max_skill"));
        tm.AverageTalent = averageTalent;

        if (defFile == null)
        {
            for (i = 0; i < Variables.Constants.GetInt("int_team_cpu_players"); i++)
                tm.Players.Add(Player.New(tm, averageTalent, true));
        }
        else
        {
            // xml_team_read(tm, def_file);  // not yet converted
        }

        for (i = 0; i < Variables.Constants.GetInt("int_team_cpu_players") - 2; i++)
            wages += tm.Players[i].Wage;

        tm.Stadium.Capacity =
            MathHelper.RoundInteger((int)Math.Round((wages / (float)Variables.Constants.GetInt("int_team_stadium_ticket_price")) *
                            Variables.Constants.GetFloat("float_team_stadium_size_wage_factor")), 2);
    }

    /// <summary>
    /// Check whether the team is already part of an international cup. We'd like to avoid having Real Madrid
    /// both in the Champions' League and in the CWC.
    /// Corresponds to C: query_team_is_in_cups.
    /// </summary>
    /// <param name="tm">The team we check.</param>
    /// <param name="group">The cup group the team shouldn't be in.</param>
    /// <returns>Whether the team is already participating in a cup.</returns>
    public static bool QueryTeamIsInCups(Team tm, int group)
    {
        if (group == -1)
            return false;

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Group != group)
                continue;

            foreach (string teamName in cup.TeamNames)
            {
                if (tm.Name == teamName)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check whether a team participates in a cup.
    /// Corresponds to C: query_team_is_in_cup.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="cup">The cup.</param>
    /// <returns>Whether the team participates in the cup.</returns>
    public static bool QueryTeamIsInCup(Team tm, Cup cup)
    {
        foreach (string teamName in cup.TeamNames)
        {
            if (tm.Name == teamName)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Return a list containing the pointers to the teams from the teams array.
    /// Corresponds to C: team_get_pointers_from_array.
    /// </summary>
    /// <param name="teams">The teams array we use.</param>
    /// <param name="teamPtrs">The team pointers we append to.</param>
    /// <returns>The team pointers.</returns>
    public static List<Team> GetPointersFromArray(List<Team> teams, List<Team> teamPtrs)
    {
        List<Team> teamPointers = new List<Team>();

        foreach (Team team in teams)
            teamPointers.Add(team);

        if (teamPtrs != null)
        {
            foreach (Team team in teamPtrs)
                teamPointers.Add(team);

            teamPtrs.Clear();
        }

        return teamPointers;
    }

    /// <summary>
    /// Return the pointer to the team belonging to the id.
    /// Corresponds to C: team_of_id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>The team or null if not found.</returns>
    public static Team OfId(int id)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            foreach (Team team in league.Teams)
            {
                if (team.Id == id)
                    return team;
            }
        }

        return null;
    }

    /// <summary>
    /// Return a pointer to the next or last fixture the team participates in.
    /// Corresponds to C: team_get_fixture.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <param name="lastFixture">Whether we return the last fixture.</param>
    /// <returns>The fixture or null.</returns>
    public static Fixture GetFixture(Team tm, bool lastFixture)
    {
        int i, j;
        Fixture fix = null;

        LiveGame liveGame = Variables.Statp as LiveGame;
        if (!lastFixture &&
            (Bygfoot.Stat0 == (int)Status0Value.StatusLiveGamePause ||
             Bygfoot.Stat0 == (int)Status0Value.StatusShowLiveGame) &&
            liveGame != null && liveGame.Fix != null &&
            (liveGame.Fix.Teams[0] == tm || liveGame.Fix.Teams[1] == tm))
            return liveGame.Fix;

        if (!lastFixture)
        {
            for (i = 0; i < Variables.Country.Leagues.Count; i++)
            {
                League league = Variables.Country.Leagues[i];
                if (QueryLeagueActiveStatic(league))
                {
                    for (j = 0; j < league.Fixtures.Count; j++)
                    {
                        Fixture fixture = league.Fixtures[j];
                        if (fixture.Attendance == -1 &&
                            Fixture.FixtureTeamInvolved(fixture, tm) &&
                            (fix == null || Fixture.QueryFixtureIsEarlier(fixture, fix)))
                        {
                            fix = fixture;
                            break;
                        }
                    }
                }
            }

            for (i = 0; i < Variables.Country.AllCups.Count; i++)
            {
                Cup cup = Variables.Country.AllCups[i];
                if (fix == null || fix.WeekNumber != Variables.Week || fix.WeekRoundNumber != Variables.WeekRound)
                {
                    if (QueryLeagueCupHasPropertyStatic(cup.Id, "national") || QueryTeamIsInCup(tm, cup))
                    {
                        for (j = 0; j < cup.Fixtures.Count; j++)
                        {
                            Fixture fixture = cup.Fixtures[j];
                            if (fixture.Attendance == -1 &&
                                Fixture.FixtureTeamInvolved(fixture, tm) &&
                                (fix == null || Fixture.QueryFixtureIsEarlier(fixture, fix)))
                            {
                                fix = fixture;
                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (i = 0; i < Variables.Country.Leagues.Count; i++)
            {
                League league = Variables.Country.Leagues[i];
                if (QueryLeagueActiveStatic(league))
                {
                    for (j = league.Fixtures.Count - 1; j >= 0; j--)
                    {
                        Fixture fixture = league.Fixtures[j];
                        if (fixture.Attendance != -1 &&
                            Fixture.FixtureTeamInvolved(fixture, tm) &&
                            (fix == null || Fixture.QueryFixtureIsLater(fixture, fix)))
                        {
                            fix = fixture;
                            break;
                        }
                    }
                }
            }

            for (i = 0; i < Variables.Country.AllCups.Count; i++)
            {
                Cup cup = Variables.Country.AllCups[i];
                if (fix == null || fix.WeekNumber != Variables.Week || fix.WeekRoundNumber != Variables.WeekRound - 1)
                {
                    if (QueryLeagueCupHasPropertyStatic(cup.Id, "national") || QueryTeamIsInCup(tm, cup))
                    {
                        for (j = cup.Fixtures.Count - 1; j >= 0; j--)
                        {
                            Fixture fixture = cup.Fixtures[j];
                            if (fixture.Attendance != -1 &&
                                Fixture.FixtureTeamInvolved(fixture, tm) &&
                                (fix == null || Fixture.QueryFixtureIsLater(fixture, fix)))
                            {
                                fix = fixture;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return fix;
    }

    /// <summary>
    /// Check whether the team is a user-managed team.
    /// Corresponds to C: team_is_user.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <returns>The user's index in the users array or -1.</returns>
    public static int IsUser(Team tm)
    {
        for (int i = 0; i < Variables.Users.Count; i++)
        {
            if (Variables.Users[i].TeamId == tm.Id)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Check whether the team with given name is a user-managed team.
    /// Corresponds to C: team_name_is_user.
    /// </summary>
    /// <param name="teamName">The team name we examine.</param>
    /// <returns>The user's index in the users array or -1.</returns>
    public static int NameIsUser(string teamName)
    {
        for (int i = 0; i < Variables.Users.Count; i++)
        {
            if (Variables.Users[i].Team != null && Variables.Users[i].Team.Name == teamName)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Return the overall average skill or the cskill of the first 11 players.
    /// Corresponds to C: team_get_average_skill.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <param name="cskill">Whether to take into account all players.</param>
    /// <returns>The average skill.</returns>
    public static float GetAverageSkill(Team tm, bool cskill)
    {
        int counter = 0;
        float sum = 0;

        if (!cskill)
        {
            foreach (Player player in tm.Players)
            {
                if (player.Cskill != 0)
                {
                    sum += player.Skill;
                    counter++;
                }
            }
        }
        else
        {
            for (int i = 0; i < 11 && i < tm.Players.Count; i++)
            {
                sum += Player.GetGameSkill(tm.Players[i], false, false);
                counter++;
            }
        }

        return counter > 0 ? sum / counter : 0;
    }

    /// <summary>
    /// Return the overall average talent of the team's players.
    /// Corresponds to C: team_get_average_talent.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <returns>The average talent.</returns>
    public static float GetAverageTalent(Team tm)
    {
        float sum = 0;

        foreach (Player player in tm.Players)
            sum += player.Talent;

        return tm.Players.Count > 0 ? sum / tm.Players.Count : 0;
    }

    /// <summary>
    /// Return the rank of the team in the league tables.
    /// Corresponds to C: team_get_league_rank.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="clid">The league or cup id.</param>
    /// <returns>The rank.</returns>
    public static int GetLeagueRank(Team tm, int clid)
    {
        int i;
        int clidLocal = (clid == -1) ? GetTableClid(tm) : clid;

        if (clidLocal < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClidStatic(clidLocal);
            if (league == null || !QueryLeagueActiveStatic(league))
                return 0;

            List<TableElement> elements = league.Tables[league.Tables.Count - 1].Elements;
            for (i = 0; i < elements.Count; i++)
                if (elements[i].TeamId == tm.Id)
                    return i + 1;

            Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                "team_get_league_rank: no rank found for team {0} in league {1}. \n",
                tm.Name, League.LeagueCupGetNameString(tm.Clid));

            return -1;
        }

        if (Cup.CupHasTables(clidLocal) == -1)
            return 0;

        int rank = GetCupRank(tm, Cup.CupFromClid(clidLocal).Rounds[Cup.CupHasTables(clidLocal)], false);
        return (rank == -1) ? 0 : rank;
    }

    /// <summary>
    /// Return the rank of the team in the round robin stage.
    /// Corresponds to C: team_get_cup_rank.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="cupRound">The cup round.</param>
    /// <param name="abort">Whether to exit if no corresponding entry can be found.</param>
    /// <returns>The rank.</returns>
    public static int GetCupRank(Team tm, CupRound cupRound, bool abort)
    {
        for (int t = 0; t < cupRound.Tables.Count; t++)
        {
            for (int e = 0; e < cupRound.Tables[t].Elements.Count; e++)
            {
                if (cupRound.Tables[t].Elements[e].TeamId == tm.Id)
                    return e + 1;
            }
        }

        if (abort)
            Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                "team_get_cup_rank: no rank found for team {0}. \n ", tm.Name);

        return -1;
    }

    /// <summary>
    /// Return the structure that fits the positions of the first 11 players.
    /// Corresponds to C: team_find_appropriate_structure.
    /// </summary>
    /// <param name="tm">The team we examine.</param>
    /// <returns>A new structure.</returns>
    public static int FindAppropriateStructure(Team tm)
    {
        int structure = 0;

        for (int i = 0; i < 11; i++)
        {
            Player player = tm.Players[i];
            if (player.Cskill > 0 && player.Cpos != PlayerPos.Goalie)
            {
                if (player.Pos != PlayerPos.Goalie)
                    structure += (int)Math.Round(Math.Pow(10, (int)PlayerPos.Forward - (int)player.Pos));
                else
                    structure += (int)Math.Round(Math.Pow(10, (int)PlayerPos.Forward - (int)player.Cpos));
            }
        }

        return structure;
    }

    /// <summary>
    /// Change the structure of a team and the appropriate cpos and cskill values.
    /// Corresponds to C: team_change_structure.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="newStructure">The new structure value, e.g. 442.</param>
    public static void ChangeStructure(Team tm, int newStructure)
    {
        tm.Structure = newStructure;

        for (int i = 1; i < 11; i++)
        {
            Player player = tm.Players.Count > i ? tm.Players[i] : null;
            if (player == null)
                break;

            player.Cpos = Player.GetPositionFromStructure(newStructure, i);
            player.Cskill = Player.GetCskill(player, (int)player.Cpos, true);
        }
    }

    /// <summary>
    /// Try to set each of the first 11 players on his favoured position and sort the substitutes by position.
    /// Corresponds to C: team_rearrange.
    /// </summary>
    /// <param name="tm">The team we rearrange.</param>
    public static void Rearrange(Team tm)
    {
        int i;
        int compareData = 100 + (int)PlayerCompareAttrib.Pos;

        tm.Players.Sort((a, b) => Player.CompareFunc(a, b, compareData));

        for (i = 0; i < tm.Players.Count; i++)
        {
            Player player = tm.Players[i];
            player.Cpos = (i < 11) ?
                Player.GetPositionFromStructure(tm.Structure, i) : player.Pos;
            if (player.Cskill > 0)
                player.Cskill = (i < 11) ?
                    Player.GetCskill(player, (int)player.Cpos, true) : player.Skill;
        }
    }

    /// <summary>
    /// Return the name of the current setting of a team attribute, e.g. style.
    /// Corresponds to C: team_attribute_to_char.
    /// </summary>
    /// <param name="attribute">The attribute.</param>
    /// <param name="value">The value.</param>
    /// <returns>The name of the attribute value.</returns>
    public static string AttributeToChar(int attribute, int value)
    {
        switch (attribute)
        {
            case (int)TeamAttribute.Style:
                switch (value)
                {
                    case -2:
                        return "ALL OUT DEFEND";
                    case -1:
                        return "DEFEND";
                    case 0:
                        return "BALANCED";
                    case 1:
                        return "ATTACK";
                    case 2:
                        return "ALL OUT ATTACK";
                }
                break;
            case (int)TeamAttribute.Boost:
                switch (value)
                {
                    case -1:
                        return "ANTI";
                    case 0:
                        return "OFF";
                    case 1:
                        return "ON";
                }
                break;
            default:
                Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                    "team_attribute_to_char: unknown attribute {0}\n", attribute);
                break;
        }

        return string.Empty;
    }

    /// <summary>
    /// Change a team attribute of the current user and print a message.
    /// Corresponds to C: team_change_attribute_with_message.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="attribute">The attribute.</param>
    /// <param name="newValue">The new value.</param>
    public static void ChangeAttributeWithMessage(Team tm, int attribute, int newValue)
    {
        Team userTeam = Variables.Users[Variables.CurUser].Team;

        switch (attribute)
        {
            case (int)TeamAttribute.Style:
                userTeam.Style = newValue;
                // game_gui_print_message("Team style changed to %s.", AttributeToChar(attribute, newValue));
                break;
            case (int)TeamAttribute.Boost:
                userTeam.Boost = newValue;
                if (newValue == 1)
                {
                    // game_gui_print_message("Boost changed to %s (costs %d per minute).",
                    //     AttributeToChar(attribute, newValue),
                    //     (int)Math.Round(Finance.WageUnit(userTeam) * Variables.Constants.GetFloat("float_boost_cost_factor")));
                }
                else
                {
                    // game_gui_print_message("Boost changed to %s.", AttributeToChar(attribute, newValue));
                }
                break;
            default:
                Debug.DebugPrintMessage($"team_attribute_to_char: unknown attribute {attribute}\n");
                break;
        }
    }

    /// <summary>
    /// Replace some players by new ones in a team.
    /// Corresponds to C: team_update_cpu_new_players.
    /// </summary>
    /// <param name="tm">The team.</param>
    public static void UpdateCpuNewPlayers(Team tm)
    {
        int i;
        int numberOfNew = Misc.MathRndi(Variables.Constants.GetInt("int_team_new_players_lower"),
                                        Variables.Constants.GetInt("int_team_new_players_upper"));
        int[] playerNumbers = new int[tm.Players.Count];

        MathHelper.GeneratePermutation(playerNumbers, 0, tm.Players.Count - 1);

        for (i = 0; i < numberOfNew; i++)
        {
            Player player = tm.Players[playerNumbers[i]];
            if (!Transfer.QueryTransferPlayerIsOnList(player))
                Player.ReplaceByNew(player, true);
        }
    }

    /// <summary>
    /// Increase player ages etc.
    /// Corresponds to C: team_update_team_weekly.
    /// </summary>
    /// <param name="tm">The user team we examine.</param>
    public static void UpdateTeamWeekly(Team tm)
    {
        foreach (Player player in tm.Players)
        {
            Player.UpdateWeekly(player);
        }

        if (IsUser(tm) == -1 && Misc.MathRndi(0, 1) < Variables.Constants.GetFloat("float_team_new_player_probability"))
        {
            // team_update_cpu_new_players(tm);
        }
    }

    /// <summary>
    /// Regenerate player fitness etc. after a match.
    /// Corresponds to C: team_update_post_match.
    /// </summary>
    /// <param name="tm">The user team we examine.</param>
    /// <param name="fix">The fixture.</param>
    public static void UpdatePostMatch(Team tm, Fixture fix)
    {
        foreach (Player player in tm.Players)
        {
            Player.UpdatePostMatch(player, fix);
        }
    }

    /// <summary>
    /// Some updates each round.
    /// Corresponds to C: team_update_team_week_roundly.
    /// </summary>
    /// <param name="tm">The user team we examine.</param>
    public static void UpdateTeamWeekRoundly(Team tm)
    {
        // Implementation of player_update_week_roundly(tm, i) for the players is not yet converted.

        if (IsUser(tm) == -1)
        {
            // strategy_update_team_pre_match(tm);
        }
    }

    /// <summary>
    /// Find a new team for a user, depending on whether he's been fired or very successful.
    /// Corresponds to C: team_get_new.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="fire">Whether the user was fired.</param>
    /// <returns>The new team.</returns>
    public static Team GetNew(Team tm, bool fire)
    {
        int i;
        int lower = 0, upper = 0;
        int bound1 = fire ? Variables.Constants.GetInt("int_team_new_bound_upper") :
                            Variables.Constants.GetInt("int_team_new_bound_lower");
        int bound2 = fire ? Variables.Constants.GetInt("int_team_new_bound_lower") :
                            Variables.Constants.GetInt("int_team_new_bound_upper");
        int idx = -1;
        List<Team> teams = GetSorted((a, b) => CompareFunc(a, b, 0), 0, false);
        Team returnValue;

        for (i = 0; i < teams.Count; i++)
        {
            if (teams[i] != tm)
                upper++;
            else
            {
                idx = i;
                break;
            }
        }

        for (i = teams.Count - 1; i >= 0; i--)
        {
            if (teams[i] != tm)
                lower++;
            else
                break;
        }

        bound1 = Math.Min(bound1, upper);
        bound2 = Math.Min(bound2, lower);

        returnValue = teams[Misc.MathRndi(i - bound1, i + bound2)];
        while (returnValue == tm)
            returnValue = teams[Misc.MathRndi(i - bound1, i + bound2)];

        return returnValue;
    }

    /// <summary>
    /// Return the teams from all leagues sorted by the specified function.
    /// Corresponds to C: team_get_sorted.
    /// </summary>
    /// <param name="compareFunction">The compare function.</param>
    /// <param name="type">The integer to pass to the compare function.</param>
    /// <param name="cup">Whether we return the international cup teams or league teams.</param>
    /// <returns>The sorted teams.</returns>
    public static List<Team> GetSorted(Func<Team, Team, int> compareFunction, int type, bool cup)
    {
        int i, j;
        List<Team> teams = new List<Team>();

        if (!cup)
        {
            for (i = 0; i < Variables.Country.Leagues.Count; i++)
                for (j = 0; j < Variables.Country.Leagues[i].Teams.Count; j++)
                    teams.Add(Variables.Country.Leagues[i].Teams[j]);
        }
        else
        {
            for (i = 0; i < Variables.Country.AllCups.Count; i++)
                for (j = 0; j < Variables.Country.AllCups[i].Teams.Count; j++)
                {
                    Team cupTeam = Team.OfId(Variables.Country.AllCups[i].Teams[j]);
                    if (cupTeam != null)
                        teams.Add(cupTeam);
                }
        }

        teams.Sort((a, b) => compareFunction(a, b));

        return teams;
    }

    /// <summary>
    /// Compare function for team arrays or pointer arrays.
    /// Corresponds to C: team_compare_func.
    /// </summary>
    /// <param name="tm1">The first team.</param>
    /// <param name="tm2">The second team.</param>
    /// <param name="type">The type to compare.</param>
    /// <returns>The comparison result.</returns>
    public static int CompareFunc(Team tm1, Team tm2, int type)
    {
        int returnValue = 0;

        if (type == 0)
        {
            /** TEAM_COMPARE_LEAGUE_RANK */
            if (tm1.Clid == tm2.Clid)
                returnValue = Misc.MiscIntCompare(GetLeagueRank(tm2, -1), GetLeagueRank(tm1, -1));
            else
                returnValue = Misc.MiscIntCompare(LeagueFromClidStatic(tm2.Clid).Layer, LeagueFromClidStatic(tm1.Clid).Layer);
        }
        else if (type == 1)
        {
            /** TEAM_COMPARE_LEAGUE_LAYER */
            returnValue = (tm1.Clid >= Bygfoot.IdCupStart || tm2.Clid >= Bygfoot.IdCupStart) ?
                0 : Misc.MiscIntCompare(LeagueFromClidStatic(tm2.Clid).Layer, LeagueFromClidStatic(tm1.Clid).Layer);
        }
        else if (type == 4)
        {
            /** TEAM_COMPARE_OFFENSIVE */
            int gf1 = GetTableValue(tm1, (int)TableElementValues.TableGf);
            int gf2 = GetTableValue(tm2, (int)TableElementValues.TableGf);
            int ga1 = GetTableValue(tm1, (int)TableElementValues.TableGa);
            int ga2 = GetTableValue(tm2, (int)TableElementValues.TableGa);

            if (gf1 > gf2)
                returnValue = -1;
            else if (gf1 < gf2)
                returnValue = 1;
            else if (ga1 < ga2)
                returnValue = -1;
            else if (ga1 > ga2)
                returnValue = 1;
            else
                returnValue = 0;
        }
        else if (type == 5)
        {
            /** TEAM_COMPARE_DEFENSE */
            int gf1 = GetTableValue(tm1, (int)TableElementValues.TableGf);
            int gf2 = GetTableValue(tm2, (int)TableElementValues.TableGf);
            int ga1 = GetTableValue(tm1, (int)TableElementValues.TableGa);
            int ga2 = GetTableValue(tm2, (int)TableElementValues.TableGa);

            if (ga1 > ga2)
                returnValue = 1;
            else if (ga1 < ga2)
                returnValue = -1;
            else if (gf1 > gf2)
                returnValue = -1;
            else if (gf1 < gf2)
                returnValue = 1;
            else
                returnValue = 0;
        }
        else if (type == 2)
        {
            /** TEAM_COMPARE_UNSORTED */
            returnValue = 0;
        }
        else if (type == 3)
        {
            /** TEAM_COMPARE_AV_SKILL */
            returnValue = Misc.MiscFloatCompare(GetAverageSkill(tm1, false), GetAverageSkill(tm2, false));
        }

        return returnValue;
    }

    /// <summary>
    /// Return the index of the team in the teams array.
    /// Corresponds to C: team_get_index.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The index.</returns>
    public static int GetIndex(Team tm)
    {
        int i;
        List<Team> teams = League.LeagueCupGetTeams(tm.Clid);

        for (i = 0; i < teams.Count; i++)
            if (ReferenceEquals(teams[i], tm))
                return i;

        Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
            "team_get_index: team {0} not found.\n", tm.Name);

        return -1;
    }

    /// <summary>
    /// Return the average of the average talents of the teams in the array.
    /// Corresponds to C: team_get_average_talents.
    /// </summary>
    /// <param name="teams">The teams.</param>
    /// <returns>The average talent.</returns>
    public static float GetAverageTalents(List<Team> teams)
    {
        if (teams == null || teams.Count == 0)
            return 0;

        float sum = 0;
        int cnt = 0;

        foreach (Team team in teams)
        {
            foreach (Player player in team.Players)
            {
                sum += player.Talent;
                cnt++;
            }
        }

        return cnt > 0 ? sum / cnt : 0;
    }

    /// <summary>
    /// Find out whether a team plays at a given date.
    /// Corresponds to C: query_team_plays.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="weekNumber">The week number.</param>
    /// <param name="weekRoundNumber">The week round number.</param>
    /// <returns>Whether the team plays.</returns>
    public static bool QueryTeamPlays(Team tm, int weekNumber, int weekRoundNumber)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id != tm.Clid)
                continue;

            foreach (Fixture fixture in league.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber &&
                    fixture.WeekRoundNumber == weekRoundNumber &&
                    (fixture.Teams[0] == tm || fixture.Teams[1] == tm))
                    return true;
            }
        }

        foreach (Cup cup in Variables.Country.Cups)
        {
            if (!QueryLeagueCupHasPropertyStatic(cup.Id, "national") && !QueryTeamIsInCup(tm, cup))
                continue;

            foreach (Fixture fixture in cup.Fixtures)
            {
                if (fixture.WeekNumber == weekNumber &&
                    fixture.WeekRoundNumber == weekRoundNumber &&
                    (fixture.Teams[0] == tm || fixture.Teams[1] == tm))
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Return a value from the league table element going with the team.
    /// Corresponds to C: team_get_table_value.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="type">The type of the value.</param>
    /// <returns>The value.</returns>
    public static int GetTableValue(Team tm, int type)
    {
        int i;

        if (tm.Clid >= Bygfoot.IdCupStart)
            Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                "team_get_table_value: team is not a league team: {0} \n", tm.Name);

        League league = LeagueFromClidStatic(tm.Clid);
        List<TableElement> elements = league.Tables[league.Tables.Count - 1].Elements;

        for (i = 0; i < elements.Count; i++)
            if (elements[i].TeamId == tm.Id)
                break;

        if (i == elements.Count)
            Main.MainExitProgram(Bygfoot.ExitCode.IntNotFound,
                "team_get_table_value: table entry not found for team {0} \n", tm.Name);

        return elements[i].Values[type];
    }

    /// <summary>
    /// Show the results of the user team against the specified team.
    /// Corresponds to C: team_write_results.
    /// </summary>
    /// <param name="tm">The team we find the results for.</param>
    /// <param name="resultBuf">The buffer we print the results into.</param>
    /// <param name="goalsBuf">The buffer we print the goals into.</param>
    public static void WriteResults(Team tm, ref string resultBuf, ref string goalsBuf)
    {
        int i;
        List<Fixture> latestFixtures = Fixture.GetLatest(tm, true);
        int[] res = new int[2];
        int[] goals = new int[2];
        int endIdx = latestFixtures.Count - Variables.Constants.GetInt("int_treeview_latest_results");

        resultBuf = string.Empty;
        endIdx = Math.Max(0, endIdx);
        for (i = latestFixtures.Count - 1; i >= endIdx; i--)
        {
            Fixture fixture = latestFixtures[i];
            res[0] = fixture.Result[0, 0] + fixture.Result[0, 1] + fixture.Result[0, 2];
            res[1] = fixture.Result[1, 0] + fixture.Result[1, 1] + fixture.Result[1, 2];

            bool userIsHome = fixture.Teams[0] == tm;
            int myIdx = userIsHome ? 0 : 1;
            int oppIdx = userIsHome ? 1 : 0;

            goals[0] += fixture.Result[myIdx, 0] + fixture.Result[myIdx, 1];
            goals[1] += fixture.Result[oppIdx, 0] + fixture.Result[oppIdx, 1];

            if (res[0] == res[1])
                resultBuf += "Dw ";
            else if (res[oppIdx] > res[myIdx])
                resultBuf += "L  ";
            else
                resultBuf += "W  ";
        }

        goalsBuf = $"{goals[0]} : {goals[1]}";
    }

    /// <summary>
    /// Write a won/lost/draw, gf:ga stat about the team's results in the specified competition into the string.
    /// Corresponds to C: team_write_overall_results.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="clid">The competition id.</param>
    /// <param name="results">The buffer.</param>
    public static void WriteOverallResults(Team tm, int clid, ref string results)
    {
        int i, idx;
        int won = 0, lost = 0, drawn = 0, gf = 0, ga = 0;
        List<Fixture> fixtures;

        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClidStatic(clid);
            fixtures = league != null ? league.Fixtures : new List<Fixture>();
        }
        else
        {
            Cup cup = GetCupStatic(clid);
            fixtures = cup != null ? cup.Fixtures : new List<Fixture>();
        }

        for (i = 0; i < fixtures.Count; i++)
        {
            Fixture fix = fixtures[i];
            if (fix.Attendance == -1)
                break;

            if (fix.Teams[0] != tm && fix.Teams[1] != tm)
                continue;

            idx = fix.Teams[0] != tm ? 1 : 0;

            if (fix.Result[0, 0] == fix.Result[1, 0])
                drawn++;
            else if (fix.Result[idx, 0] > fix.Result[1 - idx, 0])
                won++;
            else
                lost++;

            gf += fix.Result[idx, 0];
            ga += fix.Result[1 - idx, 0];
        }

        results = $"{won}-{lost}-{drawn}, {gf}:{ga}";
    }

    /// <summary>
    /// Show a row of WDWWLL type results and the goals for and against.
    /// Corresponds to C: team_write_own_results.
    /// </summary>
    /// <param name="tm">The team we find the results for.</param>
    /// <param name="buf">The buffer to fill with the results.</param>
    /// <param name="sort">Whether to sort the results according to home/away.</param>
    /// <param name="cupMatches">Whether to take cup matches into account as well.</param>
    public static void WriteOwnResults(Team tm, ref string buf, bool sort, bool cupMatches)
    {
        int i;
        int[] res = new int[2];
        Team userTeam = Variables.Users[Variables.CurUser].Team;
        List<Fixture> matches = Fixture.GetMatches(userTeam, tm);

        string buf4 = string.Empty;
        string neutral = string.Empty;

        for (i = 0; i < matches.Count; i++)
        {
            Fixture fixture = matches[i];
            if (!cupMatches && fixture.Clid >= Bygfoot.IdCupStart)
                continue;

            res[0] = fixture.Result[0, 0] + fixture.Result[0, 1];
            res[1] = fixture.Result[1, 0] + fixture.Result[1, 1];

            bool userIsHome = fixture.Teams[0] == userTeam;
            int myIdx = userIsHome ? 0 : 1;
            int oppIdx = userIsHome ? 1 : 0;

            string buf2;
            if (res[myIdx] > res[oppIdx])
                buf2 = "W  " + res[myIdx] + " : " + res[oppIdx];
            else if (res[myIdx] < res[oppIdx])
                buf2 = "L  " + res[myIdx] + " : " + res[oppIdx];
            else
                buf2 = "Dw " + res[myIdx] + " : " + res[oppIdx];

            string buf3;
            int place;
            if (fixture.HomeAdvantage)
            {
                if (fixture.Teams[0] == userTeam)
                {
                    buf3 = buf2 + " (H) ";
                    place = 0;
                }
                else
                {
                    string awayBg = Variables.ConstantsApp.GetString("string_treeview_live_game_commentary_away_bg");
                    string awayFg = Variables.ConstantsApp.GetString("string_treeview_live_game_commentary_away_fg");
                    buf3 = "<span background='" + awayBg + "' foreground='" + awayFg + "'>" + buf2 + " (A) </span> ";
                    place = 1;
                }
            }
            else
            {
                buf3 = buf2 + " (N) ";
                place = -1;
            }

            if (!sort || place == 1)
                buf4 += buf3;
            else
            {
                if (place == 0)
                {
                    string oldBuf4 = buf4;
                    buf4 = buf3 + oldBuf4;
                }
                else if (place == -1)
                    neutral += buf3;
            }
        }

        buf = buf4 + neutral;
    }

    /// <summary>
    /// Find out whether the team is in the given pointer array.
    /// Corresponds to C: query_team_is_in_teams_array.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <param name="teams">The teams.</param>
    /// <returns>Whether the team is in the array.</returns>
    public static bool QueryTeamIsInTeamsArray(Team tm, List<Team> teams)
    {
        foreach (Team team in teams)
        {
            if (ReferenceEquals(team, tm))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Check whether we find a definition file for the given team.
    /// Corresponds to C: team_has_def_file.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The def file or null.</returns>
    public static string TeamHasDefFile(Team tm)
    {
        string returnValue = null;
        string buf = "";

        if (tm.DefFile != null && Variables.Options.GetInt("int_opt_load_defs") != 0)
        {
            buf = "team_" + tm.DefFile + ".xml";
            // file_find_support_file is not yet converted.
        }

        return returnValue;
    }

    /// <summary>
    /// Complete the definition of the team (add players, calculate wages etc. Called after reading a team def file).
    /// Corresponds to C: team_complete_def.
    /// </summary>
    /// <param name="tm">The team.</param>
    public static void CompleteDef(Team tm)
    {
        int i, newPos, posSum;
        int[] positions = { 0, 0, 0, 0 };
        int add = Variables.Constants.GetInt("int_team_cpu_players") - tm.Players.Count;
        bool isUser = IsUser(tm) != -1;

        for (i = 0; i < tm.Players.Count; i++)
        {
            Player.CompleteDef(tm.Players[i], tm.AverageTalent);
            positions[(int)tm.Players[i].Pos]++;

            if (isUser)
                tm.Players[i].Recovery = 1;
        }

        for (i = 0; i < add; i++)
        {
            posSum = MathHelper.SumIntArray(positions, 4);
            Player newPlayer = Player.New(tm, tm.AverageTalent, true);

            if (positions[0] < 2)
                newPos = 0;
            else if ((float)positions[1] / posSum < Variables.Constants.GetFloat("float_player_pos_bound1"))
                newPos = 1;
            else if ((float)positions[2] / posSum < Variables.Constants.GetFloat("float_player_pos_bound1"))
                newPos = 2;
            else
                newPos = 3;

            newPlayer.Pos = newPlayer.Cpos = (PlayerPos)newPos;
            positions[newPos]++;

            tm.Players.Add(newPlayer);
        }

        CompleteDefSort(tm);
    }

    /// <summary>
    /// Sort the players in the team according to the team structure and the player positions.
    /// Corresponds to C: team_complete_def_sort.
    /// </summary>
    /// <param name="tm">The team.</param>
    public static void CompleteDefSort(Team tm)
    {
        int i, j;
        int[] positions = { 0, 0, 0, 0 };
        int[] structure = { 1, Misc.MathGetPlace(tm.Structure, 3), Misc.MathGetPlace(tm.Structure, 2), Misc.MathGetPlace(tm.Structure, 1) };
        Player playerTmp = new Player();
        Player playerTmp2 = new Player();

        for (i = 0; i < 11; i++)
            positions[(int)tm.Players[i].Pos]++;

        for (i = 0; i < 4; i++)
        {
            while (positions[i] > structure[i])
            {
                for (j = 0; j < 11; j++)
                {
                    if ((int)tm.Players[j].Pos == i)
                    {
                        playerTmp = tm.Players[j];
                        tm.Players.RemoveAt(j);
                        break;
                    }
                }

                for (j = 10; j < tm.Players.Count; j++)
                {
                    if ((int)tm.Players[j].Pos != i)
                    {
                        playerTmp2 = tm.Players[j];
                        tm.Players.RemoveAt(j);
                        break;
                    }
                }

                if (j == tm.Players.Count)
                    Main.MainExitProgram(Bygfoot.ExitCode.DefSort,
                        "team_complete_def_sort (1): cannot sort according to structure {0} (team {1}).",
                        tm.Structure, tm.Name);

                positions[i]--;
                positions[(int)playerTmp2.Pos]++;

                tm.Players.Add(playerTmp);
                tm.Players.Insert(0, playerTmp2);
            }

            while (positions[i] < structure[i])
            {
                for (j = 0; j < 11; j++)
                {
                    if ((int)tm.Players[j].Pos > i)
                    {
                        playerTmp = tm.Players[j];
                        tm.Players.RemoveAt(j);
                        break;
                    }
                }

                if (j == 11)
                    Main.MainExitProgram(Bygfoot.ExitCode.DefSort,
                        "team_complete_def_sort (2): cannot sort according to structure {0} (team {1}).",
                        tm.Structure, tm.Name);

                for (j = 10; j < tm.Players.Count; j++)
                {
                    if ((int)tm.Players[j].Pos == i)
                    {
                        playerTmp2 = tm.Players[j];
                        tm.Players.RemoveAt(j);
                        break;
                    }
                }

                if (j == tm.Players.Count)
                    Main.MainExitProgram(Bygfoot.ExitCode.DefSort,
                        "team_complete_def_sort (3): cannot sort according to structure {0} (team {1}).",
                        tm.Structure, tm.Name);

                positions[i]++;
                positions[(int)playerTmp.Pos]--;

                tm.Players.Add(playerTmp);
                tm.Players.Insert(0, playerTmp2);
            }
        }

        Rearrange(tm);
    }

    /// <summary>
    /// Find out which cup or league table the team belongs to.
    /// Corresponds to C: team_get_table_clid.
    /// </summary>
    /// <param name="tm">The team.</param>
    /// <returns>The clid.</returns>
    public static int GetTableClid(Team tm)
    {
        if (tm.Clid >= Bygfoot.IdCupStart ||
            (tm.Clid < Bygfoot.IdCupStart && !QueryLeagueActiveStatic(LeagueFromClidStatic(tm.Clid))))
        {
            for (int i = Variables.Country.AllCups.Count - 1; i >= 0; i--)
            {
                Cup cup = Variables.Country.AllCups[i];
                if (cup.Id != -1 && QueryTeamIsInCup(tm, cup))
                    return cup.Id;
            }
        }

        return tm.Clid;
    }

    /// <summary>
    /// Whether a league is active. Corresponds to the C macro query_league_active.
    /// </summary>
    /// <param name="league">The league.</param>
    /// <returns>Whether the league is active.</returns>
    private static bool QueryLeagueActiveStatic(League league)
    {
        return !QueryLeagueCupHasPropertyStatic(league.Id, "inactive");
    }

    /// <summary>
    /// Whether a league or cup has the specified property. Corresponds to the C macro query_league_cup_has_property.
    /// </summary>
    /// <param name="clid">The id of the league or cup.</param>
    /// <param name="property">The property.</param>
    /// <returns>Whether the property is set.</returns>
    private static bool QueryLeagueCupHasPropertyStatic(int clid, string property)
    {
        List<string> properties = new List<string>();

        if (clid < Bygfoot.IdCupStart)
        {
            League league = LeagueFromClidStatic(clid);
            if (league != null)
                properties = league.Properties;
        }

        Cup cup = GetCupStatic(clid);
        if (cup != null)
            properties = cup.Properties;

        foreach (string value in properties)
        {
            if (value == property)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Return the league with the given id. Corresponds to C: league_from_clid.
    /// </summary>
    /// <param name="clid">The id.</param>
    /// <returns>The league or null.</returns>
    private static League LeagueFromClidStatic(int clid)
    {
        foreach (League league in Variables.Country.Leagues)
        {
            if (league.Id == clid)
                return league;
        }

        return null;
    }

    /// <summary>
    /// Return the cup with the given id. Corresponds to C: cup_from_clid.
    /// </summary>
    /// <param name="clid">The id.</param>
    /// <returns>The cup or null.</returns>
    private static Cup GetCupStatic(int clid)
    {
        foreach (Cup cup in Variables.Country.Cups)
        {
            if (cup.Id == clid)
                return cup;
        }

        return null;
    }
}
