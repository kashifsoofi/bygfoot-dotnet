using System;
using System.Xml;
using System.IO;

namespace GladeToUi
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: GladeToUi <path_to_glade_file>");
                return;
            }

            string inputPath = args[0];

            if (!File.Exists(inputPath))
            {
                Console.WriteLine($"Error: File not found: {inputPath}");
                return;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(inputPath);

                // Find all top-level GtkWindow objects
                XmlNodeList windows = doc.SelectNodes("//object[@class='GtkWindow']");
                if (windows == null || windows.Count == 0)
                {
                    Console.WriteLine("No GtkWindow objects found in the file.");
                    return;
                }

                // Determine base output directory (Bygfoot.Gtk\Windows relative to project root)
                // The tool is in Tools/GladeToUi, so we go up two levels to reach the project root.
                string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", ".."));
                string outputDir = Path.Combine(baseDir, "Bygfoot.Gtk", "Windows");
                Directory.CreateDirectory(outputDir);

                foreach (XmlElement win in windows)
                {
                    string id = win.GetAttribute("id");
                    if (string.IsNullOrEmpty(id)) continue;

                    // Remove prefix "window_" if present for the filename
                    string fileName = id.StartsWith("window_") ? id.Substring(7) : id;
                    string outputPath = Path.Combine(outputDir, fileName + ".ui");

                    XmlDocument outputDoc = new XmlDocument();
                    XmlElement root = outputDoc.CreateElement("interface");
                    outputDoc.AppendChild(root);

                    MapElement(win, outputDoc, root);

                    string formatted = FormatXml(outputDoc);
                    File.WriteAllText(outputPath, formatted);
                    Console.WriteLine($"Converted {id} to {outputPath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
        }

        static void MapElement(XmlElement source, XmlDocument outputDoc, XmlElement parent)
        {
            // 1. Create the new object element
            XmlElement obj = outputDoc.CreateElement("object");
            obj.SetAttribute("class", source.GetAttribute("class"));

            string id = source.GetAttribute("id");
            if (!string.IsNullOrEmpty(id))
                obj.SetAttribute("id", id);

            // 2. Copy properties from source to new element
            foreach (XmlAttribute attr in source.Attributes)
            {
                if (attr.Name != "class" && attr.Name != "id")
                    obj.SetAttribute(attr.Name, attr.Value);
            }

            // 3. Handle placement in the hierarchy
            if (parent.Name == "interface")
            {
                // Top level objects go directly into the interface
                parent.AppendChild(obj);
            }
            else
            {
                // Nested objects go into a child wrapper
                XmlElement childWrapper = outputDoc.CreateElement("child");
                parent.AppendChild(childWrapper);
                childWrapper.AppendChild(obj);
            }

            // 4. Process children of the source element recursively
            XmlNodeList children = source.SelectNodes("child");
            if (children != null)
            {
                foreach (XmlNode child in children)
                {
                    if (child is XmlElement el && el.Name == "child")
                    {
                        // In Glade, a <child> can contain an <object>.
                        XmlElement inner = el.SelectSingleNode("object") as XmlElement;
                        if (inner != null)
                        {
                            // We pass 'obj' as the parent because this is a child of the current object.
                            MapElement(inner, outputDoc, obj);
                        }
                    }
                }
            }
        }

        static string FormatXml(XmlDocument doc)
        {
            using (var sw = new StringWriter())
            {
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = Environment.NewLine,
                    NewLineHandling = NewLineHandling.Replace
                };
                using (var xw = XmlWriter.Create(sw, settings))
                    doc.Save(xw);
                return sw.ToString();
            }
        }
    }
}
