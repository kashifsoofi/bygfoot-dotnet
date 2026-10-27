namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of table XML files.
/// Corresponds to the C module src/xml_loadsave_table.h and src/xml_loadsave_table.c
/// (xml_loadsave_table_read, xml_loadsave_table_write and the
/// xml_loadsave_table_start_element, xml_loadsave_table_end_element and
/// xml_loadsave_table_text functions).
/// </summary>
public static class TableXmlLoadSave
{
    // Common tags used in the table xml files. Corresponds to the tag enum of src/xml.h.
    private const int TagName = 0;
    private const int TagId = 4;
    private const int TagRound = 13;
    private const int TagTeamId = 10;

    // Tags. Corresponds to the tag enum of the table module in src/xml_loadsave_table.c.
    private const int TagTable = 6000;
    private const int TagTableElement = 6001;
    private const int TagTableElementValue = 6002;
    private const int TagTableElementOldRank = 6003;
    private const int TagEnd = 6004;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the table module.
    /// </summary>
    public enum XmlTableStates
    {
        TagName = 0,
        TagId = 4,
        TagRound = 13,
        TagTeamId = 10,
        TagTable = 6000,
        TagTableElement = 6001,
        TagTableElementValue = 6002,
        TagTableElementOldRank = 6003,
        TagEnd = 6004
    }

    private static XmlTableStates State = XmlTableStates.TagTable;
    private static Table NewTable = new Table();
    private static TableElement NewElement = new TableElement();
    private static int ValueIdx = 0;

    /// <summary>
    /// Function reading a table xml file and filling in the given table.
    /// @param filename The name of the table xml file to be read.
    /// @param table The table to be read into.
    /// </summary>
    /// <param name="filename">The name of the table xml file to read.</param>
    /// <param name="table">The table to be read into.</param>
    public static void TableXmlLoadSaveRead(string filename, Table table)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlTableStates.TagTable;
        NewTable = table;
        NewElement = new TableElement();
        ValueIdx = 0;

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
                            TableXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            TableXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            TableXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        TableXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        TableXmlLoadSaveReadText(reader.ReadString());
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
    public static void TableXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void TableXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the table XML parser, dispatching
    /// on the tag of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        int tag = GetTagFromName(elementName);
        bool validTag = false;

        switch (tag)
        {
            case TagTable:
                State = XmlTableStates.TagTable;
                validTag = true;
                break;
            case TagTableElement:
                State = XmlTableStates.TagTableElement;
                ValueIdx = 0;
                validTag = true;
                break;
            case TagTableElementValue:
                State = XmlTableStates.TagTableElementValue;
                validTag = true;
                break;
            case TagTableElementOldRank:
                State = XmlTableStates.TagTableElementOldRank;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_table_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_table_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the table module
    /// are the numbers following the underscore in the tag names (e.g. '_6000').
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
    /// The state variable is changed in this function and the
    /// new element gets added to the table.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="elementName">Name of the closing tag.</param>
    public static void TableXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagName ||
            tag == TagId ||
            tag == TagRound ||
            tag == TagTableElement)
        {
            State = XmlTableStates.TagTable;

            if (tag == TagTableElement)
                NewTable.Elements.Add(NewElement);
        }
        else if (tag == TagTableElementValue ||
                 tag == TagTableElementOldRank ||
                 tag == TagTeamId)
        {
            State = XmlTableStates.TagTableElement;

            if (tag == TagTableElementValue)
                ValueIdx++;
        }
        else if (tag != TagTable)
        {
            Debug.DebugPrintMessage($"xml_loadsave_table_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the table variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void TableXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlTableStates.TagName:
                NewTable.Name = text;
                break;
            case XmlTableStates.TagId:
                NewTable.Clid = intValue;
                break;
            case XmlTableStates.TagRound:
                NewTable.Round = intValue;
                break;
            case XmlTableStates.TagTeamId:
                NewElement.TeamId = intValue;
                break;
            case XmlTableStates.TagTableElementValue:
                NewElement.Values[ValueIdx] = intValue;
                break;
            case XmlTableStates.TagTableElementOldRank:
                NewElement.OldRank = intValue;
                break;
        }
    }

    /// <summary>
    /// Write the table into the given file.
    /// </summary>
    /// <param name="filename">The table xml file to write.</param>
    /// <param name="table">The table to write.</param>
    public static void TableXmlLoadSaveWrite(string filename, Table table)
    {
        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_6000>");
            if (table.Name != null)
                fil.WriteLine("<_0>" + table.Name + "</_0>");
            fil.WriteLine("<_4>" + table.Clid + "</_4>");
            fil.WriteLine("<_13>" + table.Round + "</_13>");
            foreach (TableElement element in table.Elements)
            {
                fil.WriteLine("<_6001>");
                fil.WriteLine("<_10>" + element.TeamId + "</_10>");
                fil.WriteLine("<_6003>" + element.OldRank + "</_6003>");
                for (int j = 0; j < 9; j++)
                {
                    fil.WriteLine("<_6002>" + element.Values[j] + "</_6002>");
                }
                fil.WriteLine("</_6001>");
            }
            fil.WriteLine("</_6000>");
        }
    }
}
