namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of transfer XML files.
/// Corresponds to the C module src/xml_loadsave_transfers.h and src/xml_loadsave_transfers.c
/// (xml_loadsave_transfers_read, xml_loadsave_transfers_write and the
/// xml_loadsave_transfers_start_element, xml_loadsave_transfers_end_element and
/// xml_loadsave_transfers_text functions).
/// </summary>
public static class TransfersXmlLoadSave
{
    // Common tags used in the transfer xml files. Corresponds to the tag enum of src/xml.h.
    private const int TagName = 0;
    private const int TagSymbol = 1;
    private const int TagSid = 2;
    private const int TagShortName = 3;
    private const int TagId = 4;
    private const int TagWeekGap = 5;
    private const int TagWeekBreak = 6;
    private const int TagWeekBreakLength = 7;
    private const int TagSkipWeeksWith = 8;
    private const int TagYellowRed = 9;
    private const int TagTeamId = 10;
    private const int TagNamesFile = 11;
    private const int TagProperty = 12;
    private const int TagRound = 13;

    // Transfer tags. Corresponds to the tag enum of the transfers module in src/xml_loadsave_transfers.c.
    private const int TagTransfers = 20000;
    private const int TagTransfer = 20001;
    private const int TagTransferPlayerId = 20002;
    private const int TagTransferTime = 20003;
    private const int TagTransferFee = 20004;
    private const int TagTransferWage = 20005;
    private const int TagTransferOffer = 20006;
    private const int TagTransferOfferTeamId = 20007;
    private const int TagTransferOfferFee = 20008;
    private const int TagTransferOfferWage = 20009;
    private const int TagTransferOfferStatus = 20010;
    private const int TagEnd = 20011;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the transfers module.
    /// </summary>
    public enum XmlTransfersLoadSaveStates
    {
        TagName = 0,
        TagSymbol = 1,
        TagSid = 2,
        TagShortName = 3,
        TagId = 4,
        TagWeekGap = 5,
        TagWeekBreak = 6,
        TagWeekBreakLength = 7,
        TagSkipWeeksWith = 8,
        TagYellowRed = 9,
        TagTeamId = 10,
        TagNamesFile = 11,
        TagProperty = 12,
        TagRound = 13,
        TagTransfers = 20000,
        TagTransfer = 20001,
        TagTransferPlayerId = 20002,
        TagTransferTime = 20003,
        TagTransferFee = 20004,
        TagTransferWage = 20005,
        TagTransferOffer = 20006,
        TagTransferOfferTeamId = 20007,
        TagTransferOfferFee = 20008,
        TagTransferOfferWage = 20009,
        TagTransferOfferStatus = 20010,
        TagEnd = 20011
    }

    private static XmlTransfersLoadSaveStates State = XmlTransfersLoadSaveStates.TagTransfers;
    private static Transfer NewTransfer = new Transfer();
    private static TransferOffer NewOffer = new TransferOffer();
    private static List<Transfer> TransferList = new List<Transfer>();
    private static int FeeIdx = 0;
    private static int WageIdx = 0;

    /// <summary>
    /// Function reading a transfer list file and adding the transfers
    /// to the internal transfer list.
    /// @param filename Name of the transfer xml file (e.g. '___transfer_list.xml') to be read.
    /// </summary>
    /// <param name="filename">The transfer xml file to read.</param>
    public static void TransfersXmlLoadSaveRead(string filename)
    {
        if (string.IsNullOrEmpty(filename) || !File.Exists(filename))
            return;

        State = XmlTransfersLoadSaveStates.TagTransfers;
        NewTransfer = new Transfer();
        NewTransfer.Offers = new List<TransferOffer>();

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
                            TransfersXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            TransfersXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            TransfersXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        TransfersXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        TransfersXmlLoadSaveReadText(reader.ReadString());
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
    public static void TransfersXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void TransfersXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the transfer XML parser, dispatching
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
            case TagName:
                State = XmlTransfersLoadSaveStates.TagName;
                break;
            case TagSymbol:
                State = XmlTransfersLoadSaveStates.TagSymbol;
                break;
            case TagSid:
                State = XmlTransfersLoadSaveStates.TagSid;
                break;
            case TagShortName:
                State = XmlTransfersLoadSaveStates.TagShortName;
                break;
            case TagId:
                State = XmlTransfersLoadSaveStates.TagId;
                break;
            case TagWeekGap:
                State = XmlTransfersLoadSaveStates.TagWeekGap;
                break;
            case TagWeekBreak:
                State = XmlTransfersLoadSaveStates.TagWeekBreak;
                break;
            case TagWeekBreakLength:
                State = XmlTransfersLoadSaveStates.TagWeekBreakLength;
                break;
            case TagSkipWeeksWith:
                State = XmlTransfersLoadSaveStates.TagSkipWeeksWith;
                break;
            case TagYellowRed:
                State = XmlTransfersLoadSaveStates.TagYellowRed;
                break;
            case TagTeamId:
                State = XmlTransfersLoadSaveStates.TagTeamId;
                break;
            case TagNamesFile:
                State = XmlTransfersLoadSaveStates.TagNamesFile;
                break;
            case TagProperty:
                State = XmlTransfersLoadSaveStates.TagProperty;
                break;
            case TagRound:
                State = XmlTransfersLoadSaveStates.TagRound;
                break;
            case TagTransfers:
                State = XmlTransfersLoadSaveStates.TagTransfers;
                break;
            case TagTransfer:
                State = XmlTransfersLoadSaveStates.TagTransfer;
                NewTransfer = new Transfer();
                NewTransfer.Offers = new List<TransferOffer>();
                FeeIdx = 0;
                WageIdx = 0;
                break;
            case TagTransferPlayerId:
                State = XmlTransfersLoadSaveStates.TagTransferPlayerId;
                break;
            case TagTransferTime:
                State = XmlTransfersLoadSaveStates.TagTransferTime;
                break;
            case TagTransferFee:
                State = XmlTransfersLoadSaveStates.TagTransferFee;
                break;
            case TagTransferWage:
                State = XmlTransfersLoadSaveStates.TagTransferWage;
                break;
            case TagTransferOffer:
                State = XmlTransfersLoadSaveStates.TagTransferOffer;
                break;
            case TagTransferOfferTeamId:
                State = XmlTransfersLoadSaveStates.TagTransferOfferTeamId;
                break;
            case TagTransferOfferFee:
                State = XmlTransfersLoadSaveStates.TagTransferOfferFee;
                break;
            case TagTransferOfferWage:
                State = XmlTransfersLoadSaveStates.TagTransferOfferWage;
                break;
            case TagTransferOfferStatus:
                State = XmlTransfersLoadSaveStates.TagTransferOfferStatus;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_transfers_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the transfers module
    /// are the numbers following the underscore in the tag names (e.g. '_20000').
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
    public static void TransfersXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagTransfer)
        {
            State = XmlTransfersLoadSaveStates.TagTransfers;
            TransferList.Add(NewTransfer);
        }
        else if (tag == TagTeamId ||
                 tag == TagTransferPlayerId ||
                 tag == TagTransferTime ||
                 tag == TagTransferFee ||
                 tag == TagTransferWage)
        {
            State = XmlTransfersLoadSaveStates.TagTransfer;
            if (tag == TagTransferFee)
                FeeIdx++;
            else if (tag == TagTransferWage)
                WageIdx++;
            else if (tag == TagTransferOffer)
                NewTransfer.Offers.Add(NewOffer);
        }
        else if (tag == TagTransferOfferTeamId ||
                 tag == TagTransferOfferFee ||
                 tag == TagTransferOfferWage ||
                 tag == TagTransferOfferStatus)
        {
            State = XmlTransfersLoadSaveStates.TagTransferOffer;
        }
        else if (tag != TagTransfers)
        {
            Debug.DebugPrintMessage($"xml_loadsave_transfers_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the transfer variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void TransfersXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlTransfersLoadSaveStates.TagTeamId:
                NewTransfer.Tm = null;
                break;
            case XmlTransfersLoadSaveStates.TagTransferPlayerId:
                NewTransfer.Id = intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferTime:
                NewTransfer.Time = intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferFee:
                NewTransfer.Fee[FeeIdx] = intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferWage:
                NewTransfer.Wage[WageIdx] = intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferOfferTeamId:
                NewOffer.Tm = null;
                break;
            case XmlTransfersLoadSaveStates.TagTransferOfferWage:
                NewOffer.Wage = intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferOfferStatus:
                NewOffer.Status = (TransferOfferStatus)intValue;
                break;
            case XmlTransfersLoadSaveStates.TagTransferOfferFee:
                NewOffer.Fee = intValue;
                break;
        }
    }

    /// <summary>
    /// Write the transfers into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the transfer list save file (e.g. 'user_0').</param>
    public static void TransfersXmlLoadSaveWrite(string prefix)
    {
        string filename = prefix + "___transfer_list.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_20000>");
            foreach (Transfer transfer in TransferList)
            {
                fil.WriteLine("<_20001>");
                fil.WriteLine("<_10>" + (transfer.Tm?.Id ?? 0) + "</_10>");
                fil.WriteLine("<_20002>" + transfer.Id + "</_20002>");
                fil.WriteLine("<_20003>" + transfer.Time + "</_20003>");
                for (int j = 0; j < 4; j++)
                {
                    fil.WriteLine("<_20004>" + transfer.Fee[j] + "</_20004>");
                }
                for (int j = 0; j < 4; j++)
                {
                    fil.WriteLine("<_20005>" + transfer.Wage[j] + "</_20005>");
                }
                foreach (TransferOffer offer in transfer.Offers)
                {
                    fil.WriteLine("<_20006>");
                    fil.WriteLine("<_20007>" + (offer.Tm?.Id ?? 0) + "</_20007>");
                    fil.WriteLine("<_20009>" + offer.Wage + "</_20009>");
                    fil.WriteLine("<_20010>" + (int)offer.Status + "</_20010>");
                    fil.WriteLine("<_20008>" + offer.Fee + "</_20008>");
                    fil.WriteLine("</_20006>");
                }
                fil.WriteLine("</_20001>");
            }
            fil.WriteLine("</_20000>");
        }
    }
}
