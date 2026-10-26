namespace Bygfoot.Cli.Models;

using System.Collections.Generic;
using System.Xml;

/// <summary>
/// Static class to handle the reading of country XML files.
/// Corresponds to the C module src/xml_country.h and src/xml_country.c
/// (xml_country_read, xml_country_read_start_element, xml_country_read_end_element
/// and xml_country_read_text).
/// </summary>
public static class CountryXml
{
    private const string TagCountry = "country";
    private const string TagDefName = "name";
    private const string TagRating = "rating";
    private const string TagDefSymbol = "symbol";
    private const string TagDefSid = "sid";
    private const string TagSupernational = "supernational";
    private const string TagLeagues = "leagues";
    private const string TagLeague = "league";
    private const string TagCups = "cups";
    private const string TagCup = "cup";

    public enum XmlCountryStates
    {
        StateCountry = 0,
        StateName,
        StateRating,
        StateSymbol,
        StateSid,
        StateSupernational,
        StateLeagues,
        StateLeague,
        StateCups,
        StateCup,
        StateEnd
    }

    private static XmlCountryStates State = XmlCountryStates.StateCountry;
    private static Country Cntry = new Country();

    /// <summary>
    /// Function reading an XML file specifying a country.
    /// @param countryName name of the xml file (e.g. 'country_england.xml')
    /// to be read. Full path is not necessary, if the file is located in
    /// one of the support directories; neither are the prefix 'country_'
    /// or the suffix '.xml'.
    /// @param cntry The country variable to write.
    /// </summary>
    /// <param name="countryName">Name of the country xml file.</param>
    /// <param name="cntry">The country to read the league and cup files for.</param>
    public static void XmlCountryRead(string countryName, Country cntry)
    {
        string fileName = FileHelper.FindSupportFile(countryName, false);
        if (fileName == null)
            fileName = FileHelper.FindSupportFile($"country_{countryName}.xml", true);
        if (string.IsNullOrEmpty(fileName))
            return;

        State = XmlCountryStates.StateCountry;
        Cntry = cntry;

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
                            XmlCountryReadStartElement(reader, reader.LocalName);
                            XmlCountryReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            XmlCountryReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        XmlCountryReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        XmlCountryReadText(reader.ReadString(), Cntry);
                        break;
                }

                if (!reader.Read())
                    break;
            }
        }

        for (int i = 0; i < Cntry.Leagues.Count; i++)
            if (Cntry.Leagues[i].Layer == -1)
                Cntry.Leagues[i].Layer = i + 1;
    }

    /// <summary>
    /// The function called by the reader when an opening tag is read.
    /// The state variable is changed in this function and
    /// sometimes memory allocated for the information that's going to be read.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="reader">The XML reader.</param>
    /// <param name="elementName">Name of the opening tag.</param>
    public static void XmlCountryReadStartElement(XmlReader reader, string elementName)
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
    public static void XmlCountryReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the country XML parser, dispatching
    /// on the name of the opening tag and updating the parser state
    /// accordingly. Attributes are fetched via the given function.
    /// </summary>
    /// <param name="elementName">Name of the opening tag.</param>
    /// <param name="getAttribute">Function to get the value of an attribute by its name.</param>
    private static void StartElement(string elementName, Func<string, string> getAttribute)
    {
        switch (elementName)
        {
            case TagCountry:
                State = XmlCountryStates.StateCountry;
                break;
            case TagDefName:
                State = XmlCountryStates.StateName;
                break;
            case TagRating:
                State = XmlCountryStates.StateRating;
                break;
            case TagDefSymbol:
                State = XmlCountryStates.StateSymbol;
                break;
            case TagDefSid:
                State = XmlCountryStates.StateSid;
                break;
            case TagSupernational:
                State = XmlCountryStates.StateSupernational;
                break;
            case TagLeagues:
                State = XmlCountryStates.StateLeagues;
                break;
            case TagLeague:
                State = XmlCountryStates.StateLeague;
                break;
            case TagCups:
                State = XmlCountryStates.StateCups;
                break;
            case TagCup:
                State = XmlCountryStates.StateCup;
                break;
            default:
                Debug.DebugPrintMessage($"xml_country_read_start_element: unknown tag: {elementName}; I'm in state {State}");
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
    public static void XmlCountryReadEndElement(string elementName)
    {
        switch (elementName)
        {
            case TagDefName:
            case TagRating:
            case TagDefSymbol:
            case TagDefSid:
            case TagSupernational:
            case TagLeagues:
            case TagCups:
                State = XmlCountryStates.StateCountry;
                break;
            case TagLeague:
                State = XmlCountryStates.StateLeagues;
                break;
            case TagCup:
                State = XmlCountryStates.StateCups;
                break;
            case TagCountry:
                break;
            default:
                Debug.DebugPrintMessage($"xml_country_read_end_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the variables (e.g. team names)
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    /// <param name="cntry">The country to read the league and cup files for.</param>
    public static void XmlCountryReadText(string text, Country cntry)
    {
        float floatValue = 0;
        if (!float.TryParse(text, out floatValue))
            floatValue = 0;
        int intValue = (int)floatValue;

        switch (State)
        {
            case XmlCountryStates.StateName:
                cntry.Name = text;
                break;
            case XmlCountryStates.StateRating:
                cntry.Rating = intValue;
                break;
            case XmlCountryStates.StateSymbol:
                cntry.Symbol = text;
                break;
            case XmlCountryStates.StateSid:
                cntry.SId = text;
                break;
            case XmlCountryStates.StateSupernational:
                break;
            case XmlCountryStates.StateLeague:
                LeagueXml.XmlLeagueRead(text, cntry.Leagues);
                break;
            case XmlCountryStates.StateCup:
                CupXml.XmlCupRead(text, cntry.Cups);
                break;
        }
    }
}
