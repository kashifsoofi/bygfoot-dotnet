namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of player names XML files.
/// Corresponds to the C module src/xml_name.h and src/xml_name.c
/// (xml_name_read, xml_name_read_start_element, xml_name_read_end_element
/// and xml_name_read_text).
/// </summary>
public static class NameXml
{
    private const string TagNames = "names";
    private const string TagLastName = "last_name";
    private const string TagFirstName = "first_name";

    /// <summary>
    /// Enum with the states used in the XML parser functions.
    /// Corresponds to the C enum XmlNameStates.
    /// </summary>
    public enum XmlNameStates
    {
        StateNames = 0,
        StateLastName,
        StateFirstName,
        StateEnd
    }

    private static XmlNameStates State = XmlNameStates.StateNames;
    private static NameList Nlist = new NameList();

    /// <summary>
    /// Function reading a names file, filling in the first and last names
    /// of the given name list.
    /// @param sid The sid of the names file we read (e.g. 'general').
    /// @param namelist The name list we read the names into.
    /// </summary>
    /// <param name="sid">The sid of the names file to read.</param>
    /// <param name="namelist">The name list to read the names into.</param>
    public static void XmlNameRead(string sid, NameList namelist)
    {
        string fileName = FileHelper.FindSupportFile($"player_names_{sid}.xml", true);
        if (string.IsNullOrEmpty(fileName))
            return;

        State = XmlNameStates.StateNames;
        Nlist = namelist;

        namelist.FirstNames.Clear();
        namelist.LastNames.Clear();
        namelist.Sid = sid;

        using (var reader = XmlReader.Create(fileName))
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
                            XmlNameReadStartElement(reader, reader.LocalName);
                            XmlNameReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlNameReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlNameReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlNameReadText(reader.ReadString());
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
    public static void XmlNameReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlNameReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the names XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagNames:
                State = XmlNameStates.StateNames;
                break;
            case TagFirstName:
                State = XmlNameStates.StateFirstName;
                break;
            case TagLastName:
                State = XmlNameStates.StateLastName;
                break;
            default:
                Debug.DebugPrintMessage($"xml_name_read_start_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void XmlNameReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagFirstName:
            case TagLastName:
                State = XmlNameStates.StateNames;
                break;
            case TagNames:
                break;
            default:
                Debug.DebugPrintMessage($"xml_name_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the name list when a file
    /// gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void XmlNameReadText(string text)
    {
        if (State == XmlNameStates.StateFirstName)
            Nlist.FirstNames.Add(text);
        else if (State == XmlNameStates.StateLastName)
            Nlist.LastNames.Add(text);
    }
}
