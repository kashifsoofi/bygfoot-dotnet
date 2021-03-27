using System;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Xml2Json
{
    class Program
    {
        static void Main(string[] args)
        {
            var srcPath = args[0];
            var dstPath = args[1];

            var xmlFilePaths = Directory.EnumerateFiles(srcPath, "*.xml", SearchOption.AllDirectories);
            foreach (var xmlFilePath in xmlFilePaths)
            {
                var jsonFilePath = xmlFilePath.Replace(srcPath, dstPath);
                jsonFilePath = jsonFilePath.Replace(".xml", ".json");

                var dirPath = Path.GetDirectoryName(jsonFilePath);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                ConvertXmlToJson(xmlFilePath, jsonFilePath);
            }

            Console.WriteLine("All xml files converted to json.");
        }

        private static void ConvertXmlToJson(string xmlFilePath, string jsonFilePath)
        {
            var xmlContent = File.ReadAllText(xmlFilePath);
            var doc = XDocument.Parse(xmlContent);
            string jsonContent = JsonConvert.SerializeXNode(doc.Root, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonContent);
        }
    }
}
