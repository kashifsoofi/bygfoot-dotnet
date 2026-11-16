namespace Bygfoot.Cli;

/// <summary>
/// Enumeration of news article types. 
/// Corresponds to C enum NewsArticleType.
/// </summary>
public enum NewsArticleType
{
    Match = 0,                  // NEWS_ARTICLE_TYPE_MATCH
    Finances,                   // NEWS_ARTICLE_TYPE_FINANCES
    StarPlayerTransfer,         // NEWS_ARTICLE_TYPE_STAR_PLAYER_TRANSFER
    LeagueChampion,             // NEWS_ARTICLE_TYPE_LEAGUE_CHAMPION
    CupQualification,           // NEWS_ARTICLE_TYPE_CUP_QUALIFICATION
    Relegation,                 // NEWS_ARTICLE_TYPE_RELEGATION
    End                         // NEWS_ARTICLE_TYPE_END
}

/// <summary>
/// Structure containing a news title or subtitle with tokens. 
/// Corresponds to C struct NewsText.
/// </summary>
public class NewsText
{
    /// <summary>The text content.</summary>
    public string Text { get; set; }

    /// <summary>Priority of the text. Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>An id to keep track of already used texts. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>A condition (if not fulfilled, the title or subtitle doesn't get considered). Corresponds to C: gchar *condition.</summary>
    public string Condition { get; set; }
}

/// <summary>
/// Structure describing a news paper article with tokens. 
/// Corresponds to C struct NewsArticle.
/// </summary>
public class NewsArticle
{
    /// <summary>Array of possible article titles. Corresponds to C: GArray *titles.</summary>
    public List<NewsText> Titles { get; set; } = new List<NewsText>();

    /// <summary>Array of possible article subtitles. Corresponds to C: GArray *subtitles.</summary>
    public List<NewsText> Subtitles { get; set; } = new List<NewsText>();

    /// <summary>A condition (if not fulfilled, the article doesn't get shown). Corresponds to C: gchar *condition.</summary>
    public string Condition { get; set; }

    /// <summary>Priority of the article. Corresponds to C: gint priority.</summary>
    public int Priority { get; set; }

    /// <summary>An id to avoid repetitions. Corresponds to C: gint id.</summary>
    public int Id { get; set; }
}

/// <summary>
/// Structure holding an article without tokens (ie. the real deal that's displayed). 
/// Corresponds to C struct NewsPaperArticle.
/// </summary>
public class NewsPaperArticle
{
    /// <summary>The week number. Corresponds to C: gint week_number.</summary>
    public int WeekNumber { get; set; }

    /// <summary>The round number within the week. Corresponds to C: gint week_round_number.</summary>
    public int WeekRoundNumber { get; set; }

    /// <summary>The ID of the title. Corresponds to C: gint title_id.</summary>
    public int TitleId { get; set; }

    /// <summary>The ID of the subtitle. Corresponds to C: gint subtitle_id.</summary>
    public int SubtitleId { get; set; }

    /// <summary>The ID of the league or cup. Corresponds to C: gint clid.</summary>
    public int Clid { get; set; }

    /// <summary>The cup round. Corresponds to C: gint cup_round.</summary>
    public int CupRound { get; set; }

    /// <summary>The small title. Corresponds to C: gchar *title_small.</summary>
    public string TitleSmall { get; set; }

    /// <summary>The main title. Corresponds to C: gchar *title.</summary>
    public string Title { get; set; }

    /// <summary>The subtitle. Corresponds to C: gchar *subtitle.</summary>
    public string Subtitle { get; set; }

    /// <summary>The unique ID of the article. Corresponds to C: gint id.</summary>
    public int Id { get; set; }

    /// <summary>The user index. Corresponds to C: gint user_idx.</summary>
    public int UserIdx { get; set; }
}

/// <summary>
/// Structure holding the newspaper for the game. 
/// Corresponds to C struct NewsPaper.
/// </summary>
public class NewsPaper
{
    /// <summary>The array of created articles. Corresponds to C: GArray *articles.</summary>
    public List<NewsPaperArticle> Articles { get; set; } = new List<NewsPaperArticle>();
}

/// <summary>
/// Static class to handle the news functions. Corresponds to the C module src/news.h and src/news.c
/// (the news_free_tokens, news_set_match_tokens, news_generate_match, news_select, news_get_title,
/// news_check_article_for_repetition, news_check_title_for_repetition, news_load_news_file_from_option,
/// news_load_news_file, news_titles_get_order, news_check_match_relevant, news_set_scorer_tokens,
/// news_set_league_cup_tokens, news_set_fixture_tokens, news_set_rank_tokens, news_articles_get_order and
/// news_set_streak_tokens functions).
/// </summary>
public static class News
{
    /// <summary>
    /// The replacement strings for the news tokens. Corresponds to C: GPtrArray *token_rep_news[2].
    /// </summary>
    private static readonly List<List<string>> TokenRepNews = new List<List<string>>(2);

    /// <summary>
    /// Free the memory occupied by the tokens array and the permanent tokens. Corresponds to C: news_free_tokens.
    /// </summary>
    public static void FreeTokens()
    {
        for (int i = TokenRepNews[0].Count - 1; i >= 0; i--)
        {
            TokenRepNews[0][i] = null;
            TokenRepNews[1][i] = null;
        }

        TokenRepNews[0].Clear();
        TokenRepNews[1].Clear();
    }

    /// <summary>
    /// Set match-related tokens for the news. Corresponds to C: news_set_match_tokens.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    public static void SetMatchTokens(LiveGame liveGame)
    {
        LgCommentary.SetStatsTokens(liveGame.Stats);
        SetFixtureTokens(liveGame.Fix);
        SetLeagueCupTokens(liveGame.Fix);
        SetStreakTokens(liveGame.Fix);
        SetScorerTokens(liveGame.Stats);

        if (liveGame.Fix.Clid < Bygfoot.IdCupStart)
            SetRankTokens(liveGame.Fix);
    }

    /// <summary>
    /// Generate news for a user live game or a CPU fixture. Corresponds to C: news_generate_match.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    public static void GenerateMatch(LiveGame liveGame)
    {
        TokenRepNews.Clear();
        TokenRepNews.Add(new List<string>());
        TokenRepNews.Add(new List<string>());

        SetMatchTokens(liveGame);

        string title = "";
        string subtitle = "";
        int articleId = -1;
        int titleId = -1;
        int subtitleId = -1;

        Select(Variables.News[(int)NewsArticleType.Match], ref title, ref subtitle, ref articleId, ref titleId, ref subtitleId);

        if (titleId != -1 && subtitleId != -1)
        {
            NewsPaperArticle newArticle = new NewsPaperArticle();
            newArticle.Id = articleId;
            newArticle.WeekNumber = Variables.Week;
            newArticle.WeekRoundNumber = Variables.WeekRound;

            string buf = string.Empty;
            Fixture.ResultToBuf(liveGame.Fix, ref buf, false);
            newArticle.TitleSmall = $"{liveGame.Fix.Teams[0].Name} {buf} {liveGame.Fix.Teams[1].Name}";
            newArticle.Title = title;
            newArticle.Subtitle = subtitle;
            newArticle.TitleId = titleId;
            newArticle.SubtitleId = subtitleId;
            newArticle.UserIdx = Fixture.UserTeamInvolved(liveGame.Fix);
            newArticle.Clid = liveGame.Fix.Clid;
            newArticle.CupRound = liveGame.Fix.Round;

            if (Variables.Counters[(int)Counters.CountNewNews] == 0)
                Variables.Counters[(int)Counters.CountNewNews] = 2;

            if (newArticle.UserIdx != -1)
                Variables.Counters[(int)Counters.CountNewNews] = 1;

            Variables.Newspaper.Articles.Add(newArticle);
        }

        FreeTokens();
    }

    /// <summary>
    /// Select a news from the array depending on the tokens available and write the texts and ids into the variables.
    /// Corresponds to C: news_select.
    /// </summary>
    /// <param name="newsArray">The news articles.</param>
    /// <param name="title">The title.</param>
    /// <param name="subtitle">The subtitle.</param>
    /// <param name="articleId">The article id.</param>
    /// <param name="titleId">The title id.</param>
    /// <param name="subtitleId">The subtitle id.</param>
    public static void Select(List<NewsArticle> newsArray, ref string title, ref string subtitle, ref int articleId, ref int titleId, ref int subtitleId)
    {
        int i;
        int[] orderArticles = new int[newsArray.Count];

        ArticlesGetOrder(newsArray, orderArticles);

        titleId = -1;
        subtitleId = -1;

        for (i = 0; i < newsArray.Count; i++)
        {
            if ((!CheckArticleForRepetition(newsArray[orderArticles[i]].Id) ||
                 newsArray[orderArticles[i]].Priority > 20) &&
                Misc.MiscParseCondition(newsArray[orderArticles[i]].Condition, TokenRepNews))
                break;
        }

        if (i == newsArray.Count)
            for (i = 0; i < newsArray.Count; i++)
                if (Misc.MiscParseCondition(newsArray[orderArticles[i]].Condition, TokenRepNews))
                    break;

        if (i == newsArray.Count)
            return;

        NewsArticle article = newsArray[orderArticles[i]];

        articleId = article.Id;

        int[] orderTitles = new int[article.Titles.Count];
        int[] orderSubtitles = new int[article.Subtitles.Count];
        TitlesGetOrder(article.Titles, orderTitles);
        TitlesGetOrder(article.Subtitles, orderSubtitles);

        int toCheck;
        titleId = GetTitle(article.Titles, ref title, orderTitles, true, false,
            OptionHelper.ConstInt("int_news_repetition_max_check_number"));
        if (titleId == -1)
            for (toCheck = OptionHelper.ConstInt("int_news_repetition_max_check_number") - 1;
                toCheck >= OptionHelper.ConstInt("int_news_repetition_min_check_number");
                toCheck--)
            {
                titleId = GetTitle(article.Titles, ref title, orderTitles, true,
                    toCheck == OptionHelper.ConstInt("int_news_repetition_min_check_number"), toCheck);
                if (titleId == -1)
                    break;
            }

        subtitleId = GetTitle(article.Subtitles, ref subtitle, orderSubtitles, false, false,
            OptionHelper.ConstInt("int_news_repetition_max_check_number"));
        if (subtitleId == -1)
            for (toCheck = OptionHelper.ConstInt("int_news_repetition_max_check_number") - 1;
                toCheck >= OptionHelper.ConstInt("int_news_repetition_min_check_number");
                toCheck--)
            {
                subtitleId = GetTitle(article.Subtitles, ref subtitle, orderSubtitles, false,
                    toCheck == OptionHelper.ConstInt("int_news_repetition_min_check_number"), toCheck);
                if (subtitleId == -1)
                    break;
            }
    }

    /// <summary>
    /// Try to find a news article title with valid tokens. Corresponds to C: news_get_title.
    /// </summary>
    /// <param name="titles">The titles.</param>
    /// <param name="title">The title.</param>
    /// <param name="order">The order.</param>
    /// <param name="isTitle">Whether we look for a title.</param>
    /// <param name="ignoreRepetition">Whether to ignore repetitions.</param>
    /// <param name="toCheck">The number to check.</param>
    /// <returns>The title id.</returns>
    public static int GetTitle(List<NewsText> titles, ref string title, int[] order, bool isTitle, bool ignoreRepetition, int toCheck)
    {
        for (int i = 0; i < titles.Count; i++)
        {
            if (Misc.MiscParseCondition(titles[order[i]].Condition, TokenRepNews) &&
                Misc.MiscStringReplaceAllTokens(TokenRepNews, titles[order[i]].Text, out title))
            {
                int result = titles[order[i]].Id;
                if (ignoreRepetition || !CheckTitleForRepetition(result, isTitle, toCheck))
                    return result;
            }
        }

        return -1;
    }

    /// <summary>
    /// Check whether a news article text has occurred in the paper recently. Corresponds to C: news_check_title_for_repetition.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <param name="isTitle">Whether we check a title.</param>
    /// <param name="toCheck">The number to check.</param>
    /// <returns>Whether it repeated.</returns>
    public static bool CheckTitleForRepetition(int id, bool isTitle, int toCheck)
    {
        int end = (Variables.Newspaper.Articles.Count < toCheck) ?
            0 : Variables.Newspaper.Articles.Count - toCheck;

        for (int i = Variables.Newspaper.Articles.Count - 1; i >= end; i--)
        {
            NewsPaperArticle article = Variables.Newspaper.Articles[i];
            if ((isTitle && article.TitleId == id) ||
                (!isTitle && article.SubtitleId == id))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Check recent news for article repetition. Corresponds to C: news_check_article_for_repetition.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>Whether it repeated.</returns>
    public static bool CheckArticleForRepetition(int id)
    {
        int maxCheck = OptionHelper.ConstInt("int_news_repetition_max_check_number");
        int end = (Variables.Newspaper.Articles.Count < maxCheck) ?
            0 : Variables.Newspaper.Articles.Count - maxCheck;

        for (int i = Variables.Newspaper.Articles.Count - 1; i >= end; i--)
            if (Variables.Newspaper.Articles[i].Id == id)
                return true;

        return false;
    }

    /// <summary>
    /// Load the appropriate news file by evaluating the language option. Corresponds to C: news_load_news_file_from_option.
    /// </summary>
    public static void LoadNewsFileFromOption()
    {
        string buf = string.Empty;
        Language.LanguageGetCode(ref buf);

        LoadNewsFile($"news_{buf}.xml", false);
    }

    /// <summary>
    /// Load the news file given. Abort if not found and abort=TRUE, load default english file otherwise. Corresponds to C: news_load_news_file.
    /// </summary>
    /// <param name="newsFile">The news file.</param>
    /// <param name="abort">Whether to abort.</param>
    public static void LoadNewsFile(string newsFile, bool abort)
    {
        if (File.Exists(newsFile))
        {
            NewsXml.XmlNewsRead(newsFile);
            return;
        }

        string fileName = FileHelper.FindSupportFile(newsFile, false);

        if (fileName != null)
        {
            NewsXml.XmlNewsRead(fileName);
        }
        else
        {
            if (abort)
                Main.MainExitProgram(Bygfoot.ExitCode.FileNotFound,
                    "news_load_news_file: file {0} not found ", newsFile);
            else
                LoadNewsFile("news_en.xml", true);
        }
    }

    /// <summary>
    /// Write a random order of indices into the integer array (only depending on the priority values of the news titles). Corresponds to C: news_titles_get_order.
    /// </summary>
    /// <param name="titles">The titles.</param>
    /// <param name="order">The order.</param>
    public static void TitlesGetOrder(List<NewsText> titles, int[] order)
    {
        int i, j, orderIdx = 0;
        int prioritySum = 0;
        int[] bounds = new int[titles.Count + 1];

        bounds[0] = 0;

        for (i = 0; i < titles.Count; i++)
        {
            prioritySum += titles[i].Priority;
            bounds[i + 1] = prioritySum;
            order[i] = -1;
        }

        int[] permutation = new int[prioritySum];
        MathHelper.GeneratePermutation(permutation, 1, prioritySum);

        for (i = 0; i < prioritySum; i++)
        {
            if (orderIdx == titles.Count)
                break;

            for (j = 0; j < titles.Count; j++)
                if (bounds[j] < permutation[i] && permutation[i] <= bounds[j + 1])
                {
                    if (!Misc.QueryIntegerIsInArray(j, order, titles.Count))
                    {
                        order[orderIdx] = j;
                        orderIdx++;
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// Find out if the match is interesting from a newspaper article generation point of view. Corresponds to C: news_check_match_relevant.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>Whether the match is relevant.</returns>
    public static bool CheckMatchRelevant(LiveGame liveGame)
    {
        if (Fixture.UserTeamInvolved(liveGame.Fix) != -1 &&
            OptionHelper.OptInt("int_opt_news_create_user") != 0)
            return true;

        if (liveGame.Fix.Clid >= Bygfoot.IdCupStart &&
            OptionHelper.OptInt("int_opt_news_create_cup") != 0)
            return true;

        List<int> userLeagues = new List<int>();

        for (int i = 0; i < Variables.Users.Count; i++)
            if (!Misc.QueryIntegerIsInArray(Variables.Users[i].Team.Clid, userLeagues.ToArray(), userLeagues.Count))
                userLeagues.Add(Variables.Users[i].Team.Clid);

        if (Misc.QueryIntegerIsInArray(liveGame.Fix.Clid, userLeagues.ToArray(), userLeagues.Count) &&
            OptionHelper.OptInt("int_opt_news_create_league") != 0)
            return true;

        return false;
    }

    /// <summary>
    /// Set scorer tokens for the news. Corresponds to C: news_set_scorer_tokens.
    /// </summary>
    /// <param name="stats">The live game stats.</param>
    public static void SetScorerTokens(LiveGameStats stats)
    {
        for (int i = 0; i < 2; i++)
        {
            List<string> scorers = new List<string>();
            List<int> goals = new List<int>();

            foreach (object obj in stats.Players[i, (int)LiveGameStatArray.Scorers])
            {
                string scorer = (string)obj;

                int existingIdx = scorers.IndexOf(scorer);
                if (existingIdx != -1)
                    goals[existingIdx]++;
                else
                {
                    scorers.Add(scorer);
                    goals.Add(1);
                }
            }

            int maxGoals = 0;
            string buf = "";
            string scorerStr = "";
            string highScorer = "";

            for (int j = 0; j < scorers.Count; j++)
            {
                int scorerGoals = goals[j];
                string scorer = scorers[j];

                if (scorerGoals > maxGoals)
                {
                    maxGoals = scorerGoals;
                    highScorer = scorer;
                }

                if (j == 0)
                {
                    scorerStr = (scorerGoals > 1) ?
                        $"{scorer} ({scorerGoals})" : scorer;
                }
                else if (j == scorers.Count - 1)
                {
                    scorerStr = (scorerGoals > 1) ?
                        $"{buf} and {scorer} ({scorerGoals})" : $"{buf} and {scorer}";
                }
                else
                {
                    scorerStr = (scorerGoals > 1) ?
                        $"{buf}, {scorer} ({scorerGoals})" : $"{buf}, {scorer}";
                }

                buf = scorerStr;
            }

            if (scorerStr != "")
            {
                Misc.MiscTokenAddBool(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_bool_multiple_scorers{i}"),
                    scorers.Count > 1);

                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_scorers{i}"),
                    scorerStr);
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_highscorer{i}"),
                    highScorer);
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_highscorer_goals{i}"),
                    Misc.MiscIntToChar(maxGoals));
            }
        }
    }

    /// <summary>
    /// Set league/cup tokens for the news. Corresponds to C: news_set_league_cup_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetLeagueCupTokens(Fixture fix)
    {
        Team tm = null;

        if (fix.Teams[0].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_layer0"),
                Misc.MiscIntToChar(League.LeagueFromClid(fix.Teams[0].Clid).Layer));
        if (fix.Teams[1].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_layer1"),
                Misc.MiscIntToChar(League.LeagueFromClid(fix.Teams[1].Clid).Layer));

        if (fix.Teams[0].Clid < Bygfoot.IdCupStart &&
            fix.Teams[1].Clid < Bygfoot.IdCupStart)
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_layerdiff"),
                Misc.MiscIntToChar(Math.Abs(League.LeagueFromClid(fix.Teams[0].Clid).Layer -
                    League.LeagueFromClid(fix.Teams[1].Clid).Layer)));

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_league_cup_name"),
            League.LeagueCupGetNameString(fix.Clid));

        Misc.MiscTokenAddBool(TokenRepNews,
            Variables.Tokens.GetInt("string_token_bool_cup"),
            fix.Clid >= Bygfoot.IdCupStart);

        if (fix.Clid >= Bygfoot.IdCupStart)
        {
            Cup cup = Cup.CupFromClid(fix.Clid);
            CupRound cupRound = cup.Rounds[fix.Round];

            if (League.QueryLeagueCupHasProperty(fix.Clid, "international"))
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt("string_token_team_layerdiff"),
                    Misc.MiscIntToChar(0));

            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_cup_stage"),
                Misc.MiscIntToChar(cup.Rounds.Count - fix.Round));

            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_neutral"),
                Misc.MiscIntToChar(cupRound.Neutral ? 1 : 0));

            string buf = string.Empty;
            Cup.CupGetRoundName(cup, fix.Round, ref buf);
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_cup_round_name"),
                buf);

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_knockout"),
                cupRound.Tables.Count == 0);

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_home_away"),
                cupRound.HomeAway);

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_first_leg"),
                cupRound.HomeAway && !fix.SecondLeg);
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_second_leg"),
                cupRound.HomeAway && fix.SecondLeg);

            if (fix.Decisive)
            {
                tm = Fixture.WinnerOf(fix, false);
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt("string_token_cup_match_winner"),
                    tm.Name);
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt("string_token_cup_match_loser"),
                    fix.Teams[fix.Teams[1] != tm ? 1 : 0].Name);
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt("string_token_cup_match_winnern"),
                    Misc.MiscIntToChar(fix.Teams[1] == tm ? 1 : 0));
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt("string_token_cup_match_losern"),
                    Misc.MiscIntToChar(fix.Teams[1] != tm ? 1 : 0));
            }

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_national"),
                League.QueryLeagueCupHasProperty(fix.Clid, "national"));
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_international"),
                League.QueryLeagueCupHasProperty(fix.Clid, "international"));
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_promrel"),
                League.QueryLeagueCupHasProperty(fix.Clid, "promotion"));
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_aux"),
                League.QueryLeagueCupHasProperty(fix.Clid, "hide") ||
                League.QueryLeagueCupHasProperty(fix.Clid, "omit_from_history"));
        }
        else
        {
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_knockout"),
                false);

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_home_away"),
                false);

            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_first_leg"),
                false);
            Misc.MiscTokenAddBool(TokenRepNews,
                Variables.Tokens.GetInt("string_token_bool_cup_second_leg"),
                false);
        }
    }

    /// <summary>
    /// Set the news tokens related to the fixture. Corresponds to C: news_set_fixture_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetFixtureTokens(Fixture fix)
    {
        int avskill0 = (int)Math.Round(Team.GetAverageSkill(fix.Teams[0], true));
        int avskill1 = (int)Math.Round(Team.GetAverageSkill(fix.Teams[1], true));
        int res0 = fix.Result[0, 0] + fix.Result[0, 1] + fix.Result[0, 2];
        int res1 = fix.Result[1, 0] + fix.Result[1, 1] + fix.Result[1, 2];
        int goaldiffaggr = Math.Abs(res0 - res1);

        string buf = string.Empty;
        Fixture.ResultToBuf(fix, ref buf, false);
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_result"),
            buf);

        Fixture.ResultToBuf(fix, ref buf, res0 < res1);
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_result_rew"),
            buf);

        Fixture.ResultToBuf(fix, ref buf, res0 > res1);
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_result_rel"),
            buf);

        Misc.MiscTokenAddBool(TokenRepNews,
            Variables.Tokens.GetInt("string_token_bool_cup_penalties"),
            fix.Result[0, 2] + fix.Result[1, 2] != 0);

        Misc.MiscTokenAddBool(TokenRepNews,
            Variables.Tokens.GetInt("string_token_bool_cup_extra"),
            fix.Result[0, 2] + fix.Result[1, 2] == 0 &&
            fix.Result[0, 1] + fix.Result[1, 1] != 0);

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_attendance"),
            Misc.MiscPrintGroupedInt(MathHelper.RoundInteger(fix.Attendance, 2)));

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_team_avskill0"),
            Misc.MiscIntToChar(avskill0));
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_team_avskill1"),
            Misc.MiscIntToChar(avskill1));
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_team_avskilldiff"),
            Misc.MiscIntToChar(Math.Abs(avskill0 - avskill1)));

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_goals0"),
            Misc.MiscIntToChar(fix.Result[0, 0]));
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_goals1"),
            Misc.MiscIntToChar(fix.Result[1, 0]));
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_goal_diff"),
            Misc.MiscIntToChar(goaldiffaggr));

        if (fix.Clid >= Bygfoot.IdCupStart)
        {
            Fixture firstLeg = Fixture.GetFirstLeg(fix, true);

            if (firstLeg != null)
                goaldiffaggr = Math.Abs(fix.Result[0, 0] + fix.Result[0, 1] + firstLeg.Result[1, 0] -
                    fix.Result[1, 0] - fix.Result[1, 1] - firstLeg.Result[0, 0]);
        }

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_goal_diff_aggregate"),
            Misc.MiscIntToChar(goaldiffaggr));

        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_team_home"),
            fix.Teams[0].Name);
        Misc.MiscTokenAdd(TokenRepNews,
            Variables.Tokens.GetInt("string_token_team_away"),
            fix.Teams[1].Name);

        if (fix.Result[0, 0] != fix.Result[1, 0])
        {
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_losing"),
                fix.Teams[fix.Result[0, 0] > fix.Result[1, 0] ? 1 : 0].Name);
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_winning"),
                fix.Teams[fix.Result[0, 0] < fix.Result[1, 0] ? 1 : 0].Name);
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_losingn"),
                Misc.MiscIntToChar(fix.Result[0, 0] > fix.Result[1, 0] ? 1 : 0));
            Misc.MiscTokenAdd(TokenRepNews,
                Variables.Tokens.GetInt("string_token_team_winningn"),
                Misc.MiscIntToChar(fix.Result[0, 0] < fix.Result[1, 0] ? 1 : 0));
        }
    }

    /// <summary>
    /// Set the rank tokens of the involved teams. Corresponds to C: news_set_rank_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetRankTokens(Fixture fix)
    {
        for (int i = 0; i < 2; i++)
        {
            League league = League.LeagueFromClid(fix.Teams[i].Clid);
            if (league.Tables.Count == 0)
                continue;

            Table table = league.Tables[league.Tables.Count - 1];
            for (int j = 0; j < table.Elements.Count; j++)
                if (table.Elements[j].Team == fix.Teams[i])
                {
                    Misc.MiscTokenAdd(TokenRepNews, Variables.Tokens.GetInt($"string_token_rank{i}"), Misc.MiscIntToChar(j + 1));
                    Misc.MiscTokenAdd(TokenRepNews, Variables.Tokens.GetInt($"string_token_oldrank{i}"),
                        Misc.MiscIntToChar(table.Elements[j].OldRank));
                    break;
                }
        }
    }

    /// <summary>
    /// Write a random order of indices into the integer array (only depending on the priority values of the news articles). Corresponds to C: news_articles_get_order.
    /// </summary>
    /// <param name="articles">The articles.</param>
    /// <param name="order">The order.</param>
    public static void ArticlesGetOrder(List<NewsArticle> articles, int[] order)
    {
        int i, j, orderIdx = 0;
        int prioritySum = 0;
        int[] bounds = new int[articles.Count + 1];

        bounds[0] = 0;

        for (i = 0; i < articles.Count; i++)
        {
            prioritySum += articles[i].Priority;
            bounds[i + 1] = prioritySum;
            order[i] = -1;
        }

        int[] permutation = new int[prioritySum];
        MathHelper.GeneratePermutation(permutation, 1, prioritySum);

        for (i = 0; i < prioritySum; i++)
        {
            if (orderIdx == articles.Count)
                break;

            for (j = 0; j < articles.Count; j++)
                if (bounds[j] < permutation[i] && permutation[i] <= bounds[j + 1])
                {
                    if (!Misc.QueryIntegerIsInArray(j, order, articles.Count))
                    {
                        order[orderIdx] = j;
                        orderIdx++;
                    }

                    break;
                }
        }
    }

    /// <summary>
    /// Set the streak tokens of the involved teams. Corresponds to C: news_set_streak_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetStreakTokens(Fixture fix)
    {
        for (int i = 0; i < 2; i++)
        {
            for (int k = 0; k < 2; k++)
            {
                List<Fixture> latestFixtures = Fixture.GetLatest(fix.Teams[i], k == 0);
                int streakWon, streakLost, streakUnbeaten;
                bool draw;
                streakWon = streakLost = streakUnbeaten = 0;
                draw = false;

                for (int j = latestFixtures.Count - 1; j >= 0; j--)
                {
                    Fixture f = latestFixtures[j];
                    int res0 = f.Result[0, 0] + f.Result[0, 1] + f.Result[0, 2];
                    int res1 = f.Result[1, 0] + f.Result[1, 1] + f.Result[1, 2];

                    if (res0 == res1)
                    {
                        if (streakLost == 0)
                        {
                            draw = true;
                            streakUnbeaten++;
                        }
                        else
                            break;
                    }
                    else if ((f.Teams[0] == fix.Teams[i] ? res0 : res1) >
                        (f.Teams[0] == fix.Teams[i] ? res1 : res0))
                    {
                        if (streakWon == 0)
                            streakLost++;
                        else
                            break;
                    }
                    else
                    {
                        if (streakLost == 0)
                        {
                            streakUnbeaten++;

                            if (!draw)
                                streakWon++;
                        }
                        else
                            break;
                    }
                }

                string prefix = k == 0 ? "" : "league_";

                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_streak_{prefix}won{i}"),
                    Misc.MiscIntToChar(streakWon));
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_streak_{prefix}lost{i}"),
                    Misc.MiscIntToChar(streakLost));
                Misc.MiscTokenAdd(TokenRepNews,
                    Variables.Tokens.GetInt($"string_token_streak_{prefix}unbeaten{i}"),
                    Misc.MiscIntToChar(streakUnbeaten));
            }
        }
    }
}
