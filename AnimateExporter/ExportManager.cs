using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AnimateExporter
{
    class ExportManager
    {
        public static string exportPath { get; set; } = "";
        public static string baseDir { get; set; } = "";
        public static List<string> allRequierments { get; set; } = [];
        public static void Export()
        {
            string LIBRARYPATH = Path.Combine(exportPath.Split(@"\")[0], "LIBRARY");
            HashSet<string> processed = new HashSet<string>();
            Queue<string> toProcess = new Queue<string>(AnalizeRequierments(exportPath));
            Console.WriteLine("Analyzing requierments");
            while (toProcess.Count > 0)
            {
                string req = toProcess.Dequeue();
                if (processed.Contains(req)) continue;
                processed.Add(req);

                string item = Path.Combine(LIBRARYPATH, req).Replace("/", @"\") + ".xml";

                if (File.Exists(item))
                {
                    List<string> newReqs = AnalizeRequierments(item);
                    foreach (var newReq in newReqs)
                    {
                        if (!processed.Contains(newReq))
                            toProcess.Enqueue(newReq);
                    }
                }
            }
            Console.WriteLine("Copying requierments");
            allRequierments = processed.ToList(); // Store the final list
            CopyRequierments();
            List<string> bitmaps = CopyAssets();
            allRequierments.Add(exportPath.Replace(@"ui\LIBRARY\", "").Replace(".xml", "").Replace(@"\", "/"));
            Console.WriteLine("Creating DOMDocument");
            DOMDocument.CreateDOMDocument(baseDir + @"\DOMDocument.xml", allRequierments, Directory.GetFiles(Path.Combine(exportPath.Split(@"\")[0], "bin")).Count(), bitmaps);
            Console.WriteLine("Exporting to fla... (TODO)");
        }
        public static List<string> AnalizeRequierments(string item)
        {
            XDocument doc = XDocument.Load(item);

            if (doc.Root == null)
                return [];

            XNamespace ns = doc.Root.GetDefaultNamespace();

            var libraryItemNames = doc.Descendants(ns + "DOMSymbolInstance")
                                    .Attributes("libraryItemName")
                                    .Select(attr => attr.Value)
                                    .ToList();

            return libraryItemNames;
        }
        public static void CopyRequierments()
        {
            foreach (var req in allRequierments)
            {
                File.Copy(Path.Combine(exportPath.Split(@"\")[0], "LIBRARY", req + ".xml"), Path.Combine(baseDir + @"\", "LIBRARY", req.Replace("/", @"\") + ".xml"));
            }
        }
        public static List<string> CopyAssets()
        {
            List<string> shapes = [];
            foreach (string asset in allRequierments)
            {
                if (asset.Contains("shape"))
                {
                    shapes.Add(asset);
                }
            }
            List<string> resources = [];
            List<string> neededResources = [];
            foreach (string shape in shapes)
            {
                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.Load(Path.Combine(baseDir + @"\", "LIBRARY", shape.Replace("/", @"\") + ".xml"));
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
                    nsmgr.AddNamespace("x", "http://ns.adobe.com/xfl/2008/");
                    XmlNode? bitmapNode = xml.SelectSingleNode("//x:DOMBitmapInstance", nsmgr);
                    string libraryItemName = bitmapNode.Attributes["libraryItemName"].Value;
                    if (neededResources.Contains(libraryItemName) == false) neededResources.Add(libraryItemName);
                }
                catch (Exception ex)
                {
                    ;
                }

            }
            foreach (string resource in neededResources)
            {
                resources.Add(Path.GetFileName(resource));
            }
            foreach (var resource in resources)
            {
                File.Copy(Path.Combine(exportPath.Split(@"\")[0], "LIBRARY", "resources", Regex.Match(resource, @"\d+").Value + ".png"), Path.Combine(baseDir + @"\", "LIBRARY", "resources", Regex.Match(resource, @"\d+").Value + ".png"));
            }
            foreach (var met in neededResources)
            {
                File.Copy(Path.Combine(exportPath.Split(@"\")[0], "bin", $"M {Regex.Match(met, @"\d+").Value}.dat"), Path.Combine(baseDir + @"\", "bin", $"M {Regex.Match(met, @"\d+").Value}.dat"));
            }

            File.Copy(exportPath, Path.Combine(baseDir, "LIBRARY", "exports", exportPath.Split(@"\").Last()));
            return neededResources;
        }
    }
}