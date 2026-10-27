namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of newspaper XML files.
/// Corresponds to the C module src/xml_loadsave_newspaper.h and src/xml_loadsave_newspaper.c
/// (xml_loadsave_newspaper_read, xml_loadsave_newspaper_write and the
/// xml_loadsave_newspaper_start_element, xml_loadsave_newspaper_end_element and
/// xml_loadsave_newspaper_text functions).
/// </summary>
public static class NewspaperXmlLoadSave
{
    private const int TagNewsPaper = 22000;
    private const int TagNewsPaperName = 22001;
    private const int TagNewsPaperArticle = 22002;
    private const int TagNewsPaperArticleId = 22003;
    private const int TagNewsPaperArticleWeek = 22004;
    private const int TagNewsPaperArticleWeekRound = 22005;
    private const int TagNewsPaperArticleTitleId = 22006;
    private const int TagNewsPaperArticleSubtitleId = 22007;
    private const int TagNewsPaperArticleTitleSmall = 22008;
    private const int TagNewsPaperArticleTitle = 22009;
    private const int TagNewsPaperArticleSubtitle = 22010;
    private const int TagNewsPaperArticleUserIdx = 22011;
    private const int TagNewsPaperArticleClid = 22012;
    private const int TagNewsPaperArticleCupRound = 22013;
    private const int TagEnd = 22014;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the newspaper module.
    /// </summary>
    public enum XmlNewspaperLoadSaveStates
    {
        TagNewsPaper = 22000,
        TagNewsPaperName = 22001,
        TagNewsPaperArticle = 22002,
        TagNewsPaperArticleId = 22003,
        TagNewsPaperArticleWeek = 22004,
        TagNewsPaperArticleWeekRound = 22005,
        TagNewsPaperArticleTitleId = 22006,
        TagNewsPaperArticleSubtitleId = 22007,
        TagNewsPaperArticleTitleSmall = 22008,
        TagNewsPaperArticleTitle = 22009,
        TagNewsPaperArticleSubtitle = 22010,
        TagNewsPaperArticleUserIdx = 22011,
        TagNewsPaperArticleClid = 22012,
        TagNewsPaperArticleCupRound = 22013,
        TagEnd = 22014
    }

    private static XmlNewspaperLoadSaveStates State = XmlNewspaperLoadSaveStates.TagNewsPaper;
    private static NewsPaperArticle NewArticle = new NewsPaperArticle();
    private static NewsPaper Newspaper = new NewsPaper();

    /// <summary>
    /// Function reading a newspaper save file and filling in the given newspaper.
    /// @param dirname The directory the newspaper save file is located in.
    /// @param prefix The prefix of the newspaper save file (e.g. 'user_0').
    /// </summary>
    /// <param name="dirname">The directory the newspaper save file is located in.</param>
    /// <param name="prefix">The prefix of the newspaper save file.</param>
    public static void NewspaperXmlLoadSaveRead(string dirname, string prefix)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(prefix))
            return;

        State = XmlNewspaperLoadSaveStates.TagNewsPaper;
        Newspaper = new NewsPaper();

        string filename = Path.Combine(dirname, prefix + "___newspaper.xml");
        if (!File.Exists(filename))
            return;

        using (var reader = XmlReader.Create(filename))
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
                            NewspaperXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            NewspaperXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            NewspaperXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        NewspaperXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        NewspaperXmlLoadSaveReadText(reader.ReadString());
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
    public static void NewspaperXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void NewspaperXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the newspaper XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);

        switch (tag)
        {
            case TagNewsPaper:
                State = XmlNewspaperLoadSaveStates.TagNewsPaper;
                break;
            case TagNewsPaperName:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperName;
                break;
            case TagNewsPaperArticle:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticle;
                break;
            case TagNewsPaperArticleId:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleId;
                break;
            case TagNewsPaperArticleWeek:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleWeek;
                break;
            case TagNewsPaperArticleWeekRound:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleWeekRound;
                break;
            case TagNewsPaperArticleTitleId:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitleId;
                break;
            case TagNewsPaperArticleSubtitleId:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleSubtitleId;
                break;
            case TagNewsPaperArticleTitleSmall:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitleSmall;
                break;
            case TagNewsPaperArticleTitle:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitle;
                break;
            case TagNewsPaperArticleSubtitle:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleSubtitle;
                break;
            case TagNewsPaperArticleUserIdx:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleUserIdx;
                break;
            case TagNewsPaperArticleClid:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleClid;
                break;
            case TagNewsPaperArticleCupRound:
                State = XmlNewspaperLoadSaveStates.TagNewsPaperArticleCupRound;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_newspaper_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the newspaper module
    /// are the numbers following the underscore in the tag names (e.g. '_22000').
    /// </summary>
    /// <param name="elementName">Name of the element.</param>
    /// <returns>The tag.</returns>
    private static int GetTagFromName(string elementName)
    {
        string tagName = elementName.Substring(1);

        return (int)(double.TryParse(tagName, out double tagValue) ? tagValue : 0);
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
    public static void NewspaperXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagNewsPaperName ||
            tag == TagNewsPaperArticle)
        {
            State = XmlNewspaperLoadSaveStates.TagNewsPaper;
            if (tag == TagNewsPaperArticle)
            {
                Newspaper.Articles.Add(NewArticle);
            }
        }
        else if (tag == TagNewsPaperArticleWeek ||
                 tag == TagNewsPaperArticleWeekRound ||
                 tag == TagNewsPaperArticleId ||
                 tag == TagNewsPaperArticleTitleSmall ||
                 tag == TagNewsPaperArticleTitle ||
                 tag == TagNewsPaperArticleSubtitle ||
                 tag == TagNewsPaperArticleTitleId ||
                 tag == TagNewsPaperArticleSubtitleId ||
                 tag == TagNewsPaperArticleUserIdx ||
                 tag == TagNewsPaperArticleClid ||
                 tag == TagNewsPaperArticleCupRound)
        {
            State = XmlNewspaperLoadSaveStates.TagNewsPaperArticle;
        }
        else if (tag != TagNewsPaper)
        {
            Debug.DebugPrintMessage($"xml_loadsave_newspaper_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the newspaper variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void NewspaperXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleWeek:
                NewArticle.WeekNumber = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleWeekRound:
                NewArticle.WeekRoundNumber = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleId:
                NewArticle.Id = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitleSmall:
                NewArticle.TitleSmall = text;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitle:
                NewArticle.Title = text;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleSubtitle:
                NewArticle.Subtitle = text;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleTitleId:
                NewArticle.TitleId = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleSubtitleId:
                NewArticle.SubtitleId = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleUserIdx:
                NewArticle.UserIdx = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleClid:
                NewArticle.Clid = intValue;
                break;
            case XmlNewspaperLoadSaveStates.TagNewsPaperArticleCupRound:
                NewArticle.CupRound = intValue;
                break;
        }
    }

    /// <summary>
    /// Write the newspaper into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the newspaper save file (e.g. 'user_0').</param>
    public static void NewspaperXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___newspaper.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_22000>");
            foreach (NewsPaperArticle article in Newspaper.Articles)
            {
                fil.WriteLine("<_22002>");
                fil.WriteLine("<_22003>" + article.Id + "</_22003>");
                fil.WriteLine("<_22004>" + article.WeekNumber + "</_22004>");
                fil.WriteLine("<_22005>" + article.WeekRoundNumber + "</_22005>");
                fil.WriteLine("<_22008>" + article.TitleSmall + "</_22008>");
                fil.WriteLine("<_22009>" + article.Title + "</_22009>");
                fil.WriteLine("<_22010>" + article.Subtitle + "</_22010>");
                fil.WriteLine("<_22006>" + article.TitleId + "</_22006>");
                fil.WriteLine("<_22007>" + article.SubtitleId + "</_22007>");
                fil.WriteLine("<_22011>" + article.UserIdx + "</_22011>");
                fil.WriteLine("<_22012>" + article.Clid + "</_22012>");
                fil.WriteLine("<_22013>" + article.CupRound + "</_22013>");
                fil.WriteLine("</_22002>");
            }
            fil.WriteLine("</_22000>");
        }
    }
}
