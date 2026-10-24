namespace Bygfoot.Cli.Models;

using System;
using System.Collections.Generic;
using System.Xml;

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
/// Static class to handle news loading and storage.
/// </summary>
public static class News
{
    private static int _nextTitleId = 1;
    private static int _nextSubtitleId = 1;
    private static int _nextArticleId = 1;

    public static List<NewsArticle> Articles { get; set; } = new List<NewsArticle>();

    /// <summary>
    /// Loads news from an XML file.
    /// </summary>
    /// <param name="filePath">Path to the XML file.</param>
    public static void LoadFromXml(string filePath)
    {
        Articles.Clear();

        var doc = new XmlDocument();
        doc.Load(filePath);
        var root = doc.DocumentElement;
        if (root == null || root.Name != "news")
            throw new Exception("Invalid news XML format.");
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node.Name == "news_article")
            {
                Articles.Add(ParseArticle(node));
            }
        }
    }

    private static NewsArticle ParseArticle(XmlNode node)
    {
        var article = new NewsArticle { Id = _nextArticleId++ };

        foreach (XmlNode child in node.ChildNodes)
        {
            switch (child.Name.ToLower())
            {
                case "type":
                    // In C, this was used to set article_idx.
                    // For simplicity, we assume the XML structure is consistent.
                    break;
                case "condition":
                    article.Condition = child.InnerText;
                    break;
                case "priority":
                    article.Priority = int.Parse(child.InnerText);
                    break;
                case "title":
                    article.Titles.Add(ParseText(child, ref _nextTitleId));
                    break;
                case "subtitle":
                    article.Subtitles.Add(ParseText(child, ref _nextSubtitleId));
                    break;
            }
        }
        return article;
    }

    private static NewsText ParseText(XmlNode node, ref int idCounter)
    {
        var text = new NewsText
        {
            Text = node.InnerText,
            Id = idCounter++,
            Priority = 1,
            Condition = "1"
        };

        if (node.Attributes != null)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name.ToLower() == "priority")
                    text.Priority = int.Parse(attr.Value);
                else if (attr.Name.ToLower() == "condition")
                    text.Condition = attr.Value;
            }
        }

        return text;
    }
}
