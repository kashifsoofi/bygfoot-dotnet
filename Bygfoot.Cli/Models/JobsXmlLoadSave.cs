namespace Bygfoot.Cli.Models;

using System.Xml;

/// <summary>
/// Static class to handle the loading and saving of job XML files.
/// Corresponds to the C module src/xml_loadsave_jobs.h and src/xml_loadsave_jobs.c
/// (xml_loadsave_jobs_read, xml_loadsave_jobs_write and the
/// xml_loadsave_jobs_start_element, xml_loadsave_jobs_end_element and
/// xml_loadsave_jobs_text functions).
/// </summary>
public static class JobsXmlLoadSave
{
    // Tags. Corresponds to the tag enum of the jobs module in src/xml_loadsave_jobs.c.
    private const int TagJobs = 21000;
    private const int TagJob = 21001;
    private const int TagJobType = 21002;
    private const int TagJobTime = 21003;
    private const int TagJobCountryFile = 21004;
    private const int TagJobCountryName = 21005;
    private const int TagJobCountryRating = 21006;
    private const int TagJobLeagueName = 21007;
    private const int TagJobLeagueLayer = 21008;
    private const int TagJobTalentPercent = 21009;
    private const int TagJobTeamId = 21010;
    private const int TagEnd = 21011;

    /// <summary>
    /// Enum with the states used in the XML parsing functions.
    /// Corresponds to the tags of the jobs module.
    /// </summary>
    public enum XmlJobsLoadSaveStates
    {
        TagJobs = 21000,
        TagJob = 21001,
        TagJobType = 21002,
        TagJobTime = 21003,
        TagJobCountryFile = 21004,
        TagJobCountryName = 21005,
        TagJobCountryRating = 21006,
        TagJobLeagueName = 21007,
        TagJobLeagueLayer = 21008,
        TagJobTalentPercent = 21009,
        TagJobTeamId = 21010,
        TagEnd = 21011
    }

    private static XmlJobsLoadSaveStates State = XmlJobsLoadSaveStates.TagJobs;
    private static Job NewJob = new Job();

    /// <summary>
    /// Function reading a job xml file and adding the jobs
    /// to the internal job list.
    /// @param dirname The directory the job xml file is located in.
    /// @param basename The basename of the job xml file (e.g. 'user_0').
    /// </summary>
    /// <param name="dirname">The directory the job xml file is located in.</param>
    /// <param name="basename">The basename of the job xml file.</param>
    public static void JobsXmlLoadSaveRead(string dirname, string basename)
    {
        if (string.IsNullOrEmpty(dirname) || string.IsNullOrEmpty(basename))
            return;

        State = XmlJobsLoadSaveStates.TagJobs;
        NewJob = new Job();

        Variables.Jobs.Clear();
        Variables.JobTeams.Clear();

        string jobTeamsFileName = Path.Combine(dirname, basename + "___job_teams.xml");
        TeamsXmlLoadSave.TeamsXmlLoadSaveRead(jobTeamsFileName, Variables.JobTeams);

        string filename = Path.Combine(dirname, basename + "___jobs.xml");
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
                            JobsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            JobsXmlLoadSaveReadEndElement(reader.LocalName);
                        }
                        else
                        {
                            JobsXmlLoadSaveReadStartElement(reader, reader.LocalName);
                            depth++;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        JobsXmlLoadSaveReadEndElement(reader.LocalName);
                        depth--;
                        break;

                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                    case XmlNodeType.Whitespace:
                        JobsXmlLoadSaveReadText(reader.ReadString());
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
    public static void JobsXmlLoadSaveReadStartElement(XmlReader reader, string elementName)
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
    public static void JobsXmlLoadSaveReadStartElement(string elementName, IDictionary<string, string> attributeNames)
    {
        StartElement(elementName, attributeName => GetAttribute(attributeNames, attributeName));
    }

    /// <summary>
    /// Start element handler of the job XML parser, dispatching
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
            case TagJobs:
                State = XmlJobsLoadSaveStates.TagJobs;
                validTag = true;
                break;
            case TagJob:
                State = XmlJobsLoadSaveStates.TagJob;
                validTag = true;
                break;
            case TagJobType:
                State = XmlJobsLoadSaveStates.TagJobType;
                validTag = true;
                break;
            case TagJobTime:
                State = XmlJobsLoadSaveStates.TagJobTime;
                validTag = true;
                break;
            case TagJobCountryFile:
                State = XmlJobsLoadSaveStates.TagJobCountryFile;
                validTag = true;
                break;
            case TagJobCountryName:
                State = XmlJobsLoadSaveStates.TagJobCountryName;
                validTag = true;
                break;
            case TagJobCountryRating:
                State = XmlJobsLoadSaveStates.TagJobCountryRating;
                validTag = true;
                break;
            case TagJobLeagueName:
                State = XmlJobsLoadSaveStates.TagJobLeagueName;
                validTag = true;
                break;
            case TagJobLeagueLayer:
                State = XmlJobsLoadSaveStates.TagJobLeagueLayer;
                validTag = true;
                break;
            case TagJobTalentPercent:
                State = XmlJobsLoadSaveStates.TagJobTalentPercent;
                validTag = true;
                break;
            case TagJobTeamId:
                State = XmlJobsLoadSaveStates.TagJobTeamId;
                validTag = true;
                break;
            default:
                Debug.DebugPrintMessage($"xml_loadsave_jobs_start_element: unknown tag: {elementName}; I'm in state {State}");
                break;
        }

        if (tag == TagJob)
        {
            NewJob = new Job();
            NewJob.CountryFile = null;
            NewJob.CountryName = null;
            NewJob.LeagueName = null;
        }

        if (!validTag)
        {
            Debug.DebugPrintMessage($"xml_loadsave_jobs_start_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// Get the tag of an element name. In C, the tags of the jobs module
    /// are the numbers following the underscore in the tag names (e.g. '_21000').
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
    public static void JobsXmlLoadSaveReadEndElement(string elementName)
    {
        int tag = GetTagFromName(elementName);

        if (tag == TagJob)
        {
            State = XmlJobsLoadSaveStates.TagJobs;
            Variables.Jobs.Add(NewJob);
        }
        else if (tag == TagJobType ||
                 tag == TagJobTime ||
                 tag == TagJobCountryFile ||
                 tag == TagJobCountryName ||
                 tag == TagJobCountryRating ||
                 tag == TagJobLeagueName ||
                 tag == TagJobLeagueLayer ||
                 tag == TagJobTalentPercent ||
                 tag == TagJobTeamId)
        {
            State = XmlJobsLoadSaveStates.TagJob;
        }
        else if (tag != TagJobs)
        {
            Debug.DebugPrintMessage($"xml_loadsave_jobs_end_element: unknown tag: {elementName}; I'm in state {State}");
        }
    }

    /// <summary>
    /// The function called by the reader when the text between tags is read.
    /// This function is responsible for filling in the job variables
    /// when a file gets loaded.
    /// @see The GLib manual (Simple XML parser).
    /// </summary>
    /// <param name="text">The text between the tags.</param>
    public static void JobsXmlLoadSaveReadText(string text)
    {
        int intValue = (int)(double.TryParse(text, out double tagValue) ? tagValue : 0);

        switch (State)
        {
            case XmlJobsLoadSaveStates.TagJobType:
                NewJob.Type = (JobType)intValue;
                break;
            case XmlJobsLoadSaveStates.TagJobTime:
                NewJob.Time = intValue;
                break;
            case XmlJobsLoadSaveStates.TagJobCountryFile:
                NewJob.CountryFile = text;
                break;
            case XmlJobsLoadSaveStates.TagJobCountryName:
                NewJob.CountryName = text;
                break;
            case XmlJobsLoadSaveStates.TagJobCountryRating:
                NewJob.CountryRating = intValue;
                break;
            case XmlJobsLoadSaveStates.TagJobLeagueName:
                NewJob.LeagueName = text;
                break;
            case XmlJobsLoadSaveStates.TagJobLeagueLayer:
                NewJob.LeagueLayer = intValue;
                break;
            case XmlJobsLoadSaveStates.TagJobTalentPercent:
                NewJob.TalentPercent = intValue;
                break;
            case XmlJobsLoadSaveStates.TagJobTeamId:
                NewJob.TeamId = intValue;
                break;
        }
    }

    /// <summary>
    /// Write the jobs into the given file.
    /// </summary>
    /// <param name="prefix">The prefix of the job xml file (e.g. 'user_0').</param>
    public static void JobsXmlLoadSaveWrite(string prefix)
    {
        string jobTeamsFileName = prefix + "___job_teams.xml";
        TeamsXmlLoadSave.TeamsXmlLoadSaveWrite(jobTeamsFileName, Variables.JobTeams);

        string filename = prefix + "___jobs.xml";

        using (var fil = new StreamWriter(filename))
        {
            fil.WriteLine("<_21000>");
            foreach (Job job in Variables.Jobs)
            {
                fil.WriteLine("<_21001>");
                fil.WriteLine("<_21002>" + (int)job.Type + "</_21002>");
                fil.WriteLine("<_21003>" + job.Time + "</_21003>");
                if (job.CountryFile != null)
                    fil.WriteLine("<_21004>" + job.CountryFile + "</_21004>");
                if (job.CountryName != null)
                    fil.WriteLine("<_21005>" + job.CountryName + "</_21005>");
                fil.WriteLine("<_21006>" + job.CountryRating + "</_21006>");
                if (job.LeagueName != null)
                    fil.WriteLine("<_21007>" + job.LeagueName + "</_21007>");
                fil.WriteLine("<_21008>" + job.LeagueLayer + "</_21008>");
                fil.WriteLine("<_21009>" + job.TalentPercent + "</_21009>");
                fil.WriteLine("<_21010>" + job.TeamId + "</_21010>");
                fil.WriteLine("</_21001>");
            }
            fil.WriteLine("</_21000>");
        }
    }
}
