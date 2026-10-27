namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;

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
        TokenRepNews.Clear();
        TokenRepNews.Add(new List<string>());
        TokenRepNews.Add(new List<string>());

        SetScorerTokens(liveGame.Stats);
        SetFixtureTokens(liveGame.Fix);
        SetLeagueCupTokens(liveGame.Fix);
        SetStreakTokens(liveGame.Fix);
        SetRankTokens(liveGame.Fix);
        SetScorerTokens(liveGame.Stats);
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

            newArticle.TitleSmall = liveGame.Fix.Teams[0].Name + " " + liveGame.Fix.Teams[1].Name;
            newArticle.Title = title;
            newArticle.Subtitle = subtitle;
            newArticle.TitleId = titleId;
            newArticle.SubtitleId = subtitleId;
            newArticle.UserIdx = -1;
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
        // Implementation depends on the not yet converted misc, news and token functions.
        throw new NotImplementedException();
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
    public static int GetTitle(List<NewsText> titles, ref string title, ref int order, bool isTitle, bool ignoreRepetition, int toCheck)
    {
        // Implementation depends on the not yet converted misc and news functions.
        throw new NotImplementedException();
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
        // Implementation depends on the not yet converted newspaper and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Check recent news for article repetition. Corresponds to C: news_check_article_for_repetition.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>Whether it repeated.</returns>
    public static bool CheckArticleForRepetition(int id)
    {
        // Implementation depends on the not yet converted newspaper and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load the appropriate news file by evaluating the language option. Corresponds to C: news_load_news_file_from_option.
    /// </summary>
    public static void LoadNewsFileFromOption()
    {
        // Implementation depends on the not yet converted language and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Load the news file given. Abort if not found and abort=TRUE, load default english file otherwise. Corresponds to C: news_load_news_file.
    /// </summary>
    /// <param name="newsFile">The news file.</param>
    /// <param name="abort">Whether to abort.</param>
    public static void LoadNewsFile(string newsFile, bool abort)
    {
        // Implementation depends on the not yet converted file and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write a random order of indices into the integer array (only depending on the priority values of the news titles). Corresponds to C: news_titles_get_order.
    /// </summary>
    /// <param name="titles">The titles.</param>
    /// <param name="order">The order.</param>
    public static void TitlesGetOrder(List<NewsText> titles, int[] order)
    {
        // Implementation depends on the not yet converted maths and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Find out if the match is interesting from a newspaper article generation point of view. Corresponds to C: news_check_match_relevant.
    /// </summary>
    /// <param name="liveGame">The live game.</param>
    /// <returns>Whether the match is relevant.</returns>
    public static bool CheckMatchRelevant(LiveGame liveGame)
    {
        // Implementation depends on the not yet converted user, option and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set scorer tokens for the news. Corresponds to C: news_set_scorer_tokens.
    /// </summary>
    /// <param name="stats">The live game stats.</param>
    public static void SetScorerTokens(LiveGameStats stats)
    {
        // Implementation depends on the not yet converted live game, player and token functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set league/cup tokens for the news. Corresponds to C: news_set_league_cup_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetLeagueCupTokens(Fixture fix)
    {
        // Implementation depends on the not yet converted league, cup and token functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the news tokens related to the fixture. Corresponds to C: news_set_fixture_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetFixtureTokens(Fixture fix)
    {
        // Implementation depends on the not yet converted fixture, team and token functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the rank tokens of the involved teams. Corresponds to C: news_set_rank_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetRankTokens(Fixture fix)
    {
        // Implementation depends on the not yet converted league, table and token functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Write a random order of indices into the integer array (only depending on the priority values of the news articles). Corresponds to C: news_articles_get_order.
    /// </summary>
    /// <param name="articles">The articles.</param>
    /// <param name="order">The order.</param>
    public static void ArticlesGetOrder(List<NewsArticle> articles, int[] order)
    {
        // Implementation depends on the not yet converted maths and news functions.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Set the streak tokens of the involved teams. Corresponds to C: news_set_streak_tokens.
    /// </summary>
    /// <param name="fix">The fixture.</param>
    public static void SetStreakTokens(Fixture fix)
    {
        // Implementation depends on the not yet converted fixture, team, user and token functions.
        throw new NotImplementedException();
    }
}
