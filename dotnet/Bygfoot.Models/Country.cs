using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Bygfoot.Helpers;

namespace Bygfoot.Model
{
    public class Country
    {
        public const string TAG_COUNTRY = "country";
        public const string TAG_RATING = "rating";
        public const string TAG_SUPERNATURAL = "supernational";
        public const string TAG_LEAGUES = "leagues";
        public const string TAG_LEAGUE = "league";
        public const string TAG_CUPS = "cups";
        public const string TAG_CUP = "cup";

        public string Name { get; set; }
        public string Symbol { get; set; }
        public string SId { get; set; }
        public int Rating { get; set; }

        public List<League> Leagues = new List<League>();
        public List<Cup> Cups = new List<Cup>();

        public List<Cup> AllCups;

        public void Load(string countryName)
        {
            string filename = Path.GetFileName(countryName);
            if (!filename.StartsWith("country_"))
                filename = "country_" + filename;
            if (!filename.EndsWith(".xml"))
                filename += ".xml";

            string path = FileHelper.FindSupportFile(filename, false);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(path));
            Load(doc.DocumentElement);
        }

        public void Load(XmlNode xnCountry)
        {
            XmlNode xnName = xnCountry.SelectSingleNode(XmlHelper.TAG_DEF_NAME);
            Name = xnName.InnerText;
            XmlNode xnSymbol = xnCountry.SelectSingleNode(XmlHelper.TAG_DEF_SYMBOL);
            Symbol = xnSymbol.InnerText;
            XmlNode xnSid = xnCountry.SelectSingleNode(XmlHelper.TAG_DEF_SID);
            SId = xnSid.InnerText;
            XmlNode xnRating = xnCountry.SelectSingleNode(TAG_RATING);
            Rating = Convert.ToInt32(xnRating.InnerText);
            XmlNode xnLeagues = xnCountry.SelectSingleNode(TAG_LEAGUES);
            foreach (XmlNode xnLeague in xnLeagues.ChildNodes)
            {
                League league = new League(true);
                league.Load(xnLeague.InnerText);
                Leagues.Add(league);
            }

            XmlNode xnCups = xnCountry.SelectSingleNode(TAG_CUPS);
            foreach (XmlNode xnCup in xnCups.ChildNodes)
            {
                Cup cup = new Cup();
                cup.Load(xnCup.InnerText);
                Cups.Add(cup);
            }
        }
    }
}