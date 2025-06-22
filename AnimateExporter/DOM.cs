using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace AnimateExporter
{
    class DOMDocument
    {
        public static void CreateDOMDocument(string DOMPath, List<string> symbols, int mediaCount, List<string> bitmaps)
        {
            XNamespace ns = "http://ns.adobe.com/xfl/2008/";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            
            int symbolCount = 1;

            List<XElement> mediaItems = new List<XElement>();
            foreach (string bitmap in bitmaps)
            {
                mediaItems.Add(
                    new XElement(ns + "DOMBitmapItem",
                        new XAttribute("name", $"resources/{Regex.Match(bitmap, @"\d+").Value}"),
                        new XAttribute("bitmapDataHRef", $"M {Regex.Match(bitmap, @"\d+").Value}.dat"),
                        new XAttribute("sourceExternalFilepath", $"LIBRARY/resources/{Regex.Match(bitmap, @"\d+").Value}.png"),
                        new XAttribute("quality", "100"),
                        new XAttribute("useImportedJPEGData", "false"),
                        new XAttribute("allowSmoothing", "true"),
                        new XAttribute("lastModified", "1746624851")
                    )
                );
            }
            List<XElement> symbolItems = new List<XElement>();
            for (int i = 0; i < symbols.Count; i++)
            {
                symbolItems.Add(
                    new XElement(ns + "Include",
                        new XAttribute("loadImmediate", "false"),
                        new XAttribute("href", $"{symbols[i]}.xml"),
                        new XAttribute("itemID", $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds():X}-{symbolCount:X8}"),
                        new XAttribute("lastModified", $"{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}")
                    )
                );
                symbolCount++;
            }

            XDocument doc = new XDocument(
                new XElement(ns + "DOMDocument",
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute("xflVersion", "2.971"),
                    new XAttribute("exporterInf", "SupercellAnimateExporter made by kubune"),
                    new XAttribute("creatorInfo", "File generated with SC tool by SCW Make! (VK: vk.com/scwmake, GITHUB: github.com/scwmake/SC)"),
                    new XAttribute("width", "1280"),
                    new XAttribute("height", "720"),
                    new XAttribute("frameRate", "30"),
                    new XAttribute("currentTimeline", "1"),
                    new XAttribute("backgroundColor", "#666666"),

                    new XAttribute("filetypeGUID", "A03733F0-0799-11EE-BE56-0242AC120002"),
                    new XAttribute("filetypeName", "org.scWorkshop.SupercellSWF.Publisher"),

                    new XElement(ns + "folders",
                        new XElement(ns + "DOMFolderItem",
                            new XAttribute("name", "shapes"),
                            new XAttribute("isExpanded", "false")
                        ),
                        new XElement(ns + "DOMFolderItem",
                            new XAttribute("name", "movieclips"),
                            new XAttribute("isExpanded", "false")
                        ),
                        new XElement(ns + "DOMFolderItem",
                            new XAttribute("name", "exports"),
                            new XAttribute("isExpanded", "false")
                        ),
                        new XElement(ns + "DOMFolderItem",
                            new XAttribute("name", "resources"),
                            new XAttribute("isExpanded", "false")
                        )
                    ),

                    new XElement(ns + "media", mediaItems),
                    new XElement(ns + "symbols", symbolItems)
                )
            );
            
            doc.Save(DOMPath);
        }
    }
}
