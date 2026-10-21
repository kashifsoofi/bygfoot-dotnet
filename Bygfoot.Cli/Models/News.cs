namespace Bygfoot.Cli.Models;

using System.Collections.Generic;

/// <summary>
/// Enumeration of news article types. 
/// Corresponds to C enum NewsArticleTypes.
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
    /// <summary>Possible article titles (possibly containing tokens). Corresponds to C: GArray *titles.</summary>
    public List<string> Titles { get; set; } = new List<string>();

    /// <summary>Possible article subtitles (possibly containing tokens). Corresponds to C: GArray *subtitles.</summary>
    public List<string> Subtitles { get; set; } = new List<string>();

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
