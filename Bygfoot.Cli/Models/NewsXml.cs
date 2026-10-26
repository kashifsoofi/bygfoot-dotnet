namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of news XML files.
/// Corresponds to the C module src/xml_news.h and src/xml_news.c
/// (xml_news_read, xml_news_read_start_element, xml_news_read_end_element
/// and xml_news_read_text, xml_news_article_type_to_int).
/// </summary>
public static class NewsXml
{
    private const string TagNews = "news";
    private const string TagArticle = "news_article";
    private const string TagArticleType = "type";
    private const string TagArticleCondition = "condition";
    private const string TagArticlePriority = "priority";
    private const string TagArticleTitle = "title";
    private const string TagArticleSubtitle = "subtitle";

    private const string AttNameTextPriority = "priority";
    private const string AttNameTextCondition = "condition";

    private const string ArticleTypeNameMatch = "match";
    private const string ArticleTypeNameFinances = "finances";
    private const string ArticleTypeNameStarPlayerTransfer = "star_player_transfer";
    private const string ArticleTypeNameLeagueChampion = "league_champion";
    private const string ArticleTypeNameCupQualification = "cup_qualification";
    private const string ArticleTypeNameRelegation = "relegation";

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlNewsStates.
    /// </summary>
    public enum XmlNewsStates
    {
        StateNews = 0,
        StateArticle,
        StateArticleType,
        StateArticleCondition,
        StateArticlePriority,
        StateArticleTitle,
        StateArticleSubtitle,
        StateEnd
    }

    private static XmlNewsStates State = XmlNewsStates.StateNews;
    private static int ArticleIdx = 0;
    private static NewsArticle NewArticle = new NewsArticle();
    private static NewsText NewTitle = new NewsText();
    private static NewsText NewSubtitle = new NewsText();

    /// <summary>
    /// Function reading an XML file specifying the news articles.
    /// The variable holding the news of the current article type gets
    /// freed and overwritten afterwards.
    /// @param newsFile Name of the xml file (e.g. 'news_en.xml')
    /// to be read. Full path is not necessary, if the file is located in
    /// one of the support directories; neither are the prefix 'news_'
    /// or the suffix '.xml'.
    /// </summary>
    /// <param name="newsFile">Name of the news xml file.</param>
    public static void XmlNewsRead(string newsFile)
    {
        if (!File.Exists(newsFile))
            return;

        State = XmlNewsStates.StateNews;

        for (int i = 0; i < Variables.News.Length; i++)
            Variables.News[i].Clear();

        using (var reader = XmlReader.Create(newsFile))
        {
            reader.MoveToContent();
            int depth = 0;

            while (true)
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.IsEmptyElement)
                        {
                            XmlNewsReadStartElement(reader, reader.LocalName);
                            XmlNewsReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlNewsReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlNewsReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlNewsReadText(reader.ReadString());
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void XmlNewsReadStartElement(XmlReader reader, string elementName)
    {
        StartElement(elementName, attributeName => reader.GetAttribute(attributeName));
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="attributeNames">The attributes of the opening tag.</param>
    public static void XmlNewsReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the news XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagNews:
                State = XmlNewsStates.StateNews;
                break;
            case TagArticle:
                State = XmlNewsStates.StateArticle;
                NewArticle = new NewsArticle
                {
                    Condition = "0",
                    Priority = 1,
                    Id = Bygfoot.NewsArticleIdNew()
                };
                NewArticle.Titles = new List<NewsText>();
                NewArticle.Subtitles = new List<NewsText>();
                break;
            case TagArticleType:
                State = XmlNewsStates.StateArticleType;
                break;
            case TagArticleCondition:
                State = XmlNewsStates.StateArticleCondition;
                break;
            case TagArticlePriority:
                State = XmlNewsStates.StateArticlePriority;
                break;
            case TagArticleTitle:
                State = XmlNewsStates.StateArticleTitle;
                NewTitle = new NewsText
                {
                    Id = Bygfoot.NewsTitleIdNew(),
                    Priority = 1,
                    Condition = "1"
                };
                string titlePriority = getAttribute(AttNameTextPriority);
                if (titlePriority != null && int.TryParse(titlePriority, out int titlePriorityValue))
                    NewTitle.Priority = titlePriorityValue;
                string titleCondition = getAttribute(AttNameTextCondition);
                if (titleCondition != null)
                    NewTitle.Condition = titleCondition;
                break;
            case TagArticleSubtitle:
                State = XmlNewsStates.StateArticleSubtitle;
                NewSubtitle = new NewsText
                {
                    Id = Bygfoot.NewsSubtitleIdNew(),
                    Priority = 1,
                    Condition = "1"
                };
                string subtitlePriority = getAttribute(AttNameTextPriority);
                if (subtitlePriority != null && int.TryParse(subtitlePriority, out int subtitlePriorityValue))
                    NewSubtitle.Priority = subtitlePriorityValue;
                string subtitleCondition = getAttribute(AttNameTextCondition);
                if (subtitleCondition != null)
                    NewSubtitle.Condition = subtitleCondition;
                break;
            default:
                Debug.DebugPrintMessage($"xml_news_read_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the value of an attribute from a dictionary of attributes.
    /// </summary>
    /// <param name="attributeNames">The attributes.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The value of the attribute, or null if it is not set.</returns>
    private static string GetAttribute(IDictionary<string, string> attributeNames, string attributeName)
    {
        return attributeNames.TryGetValue(attributeName, out string value) ? value : null;
    }

    /// <summary>
    /// The function called by the reader when a closing tag is read.
    /// The state variable is changed in this function.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void XmlNewsReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagArticle:
                State = XmlNewsStates.StateNews;
                Variables.News[ArticleIdx].Add(NewArticle);
                break;
            case TagArticleType:
            case TagArticleCondition:
            case TagArticlePriority:
            case TagArticleTitle:
            case TagArticleSubtitle:
                State = XmlNewsStates.StateArticle;
                break;
            case TagNews:
                break;
            default:
                Debug.DebugPrintMessage($"xml_news_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the news article texts
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void XmlNewsReadText(string text)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlNewsStates.StateArticleType:
                ArticleIdx = XmlNewsArticleTypeToInt(text);
                break;
            case XmlNewsStates.StateArticleCondition:
                NewArticle.Condition = text;
                break;
            case XmlNewsStates.StateArticlePriority:
                NewArticle.Priority = intValue;
                break;
            case XmlNewsStates.StateArticleTitle:
                NewTitle.Text = text;
                NewArticle.Titles.Add(NewTitle);
                break;
            case XmlNewsStates.StateArticleSubtitle:
                NewSubtitle.Text = text;
                NewArticle.Subtitles.Add(NewSubtitle);
                break;
        }
    }

    /// <summary>
    /// Return the appropriate enum integer going with the type string.
    /// </summary>
    /// <param name="typeString">The type string.</param>
    /// <returns>The integer value of the news article type.</returns>
    public static int XmlNewsArticleTypeToInt(string typeString)
    {
        int returnValue = -1;

        if (typeString == ArticleTypeNameMatch)
            returnValue = (int)NewsArticleType.Match;
        else if (typeString == ArticleTypeNameFinances)
            returnValue = (int)NewsArticleType.Finances;
        else if (typeString == ArticleTypeNameStarPlayerTransfer)
            returnValue = (int)NewsArticleType.StarPlayerTransfer;
        else if (typeString == ArticleTypeNameLeagueChampion)
            returnValue = (int)NewsArticleType.LeagueChampion;
        else if (typeString == ArticleTypeNameCupQualification)
            returnValue = (int)NewsArticleType.CupQualification;
        else if (typeString == ArticleTypeNameRelegation)
            returnValue = (int)NewsArticleType.Relegation;
        else
            Debug.DebugPrintMessage($"xml_news_article_type_to_int: unknown type name {typeString}");

        return returnValue;
    }
}
