namespace AnimateExporter
{
    class Fla
    {
        public static void CreateBaseFla(string exportName)
        {
            Directory.CreateDirectory(exportName);
            string path = exportName + @"\";
            Directory.CreateDirectory(path + @"LIBRARY\");
            Directory.CreateDirectory(path + @"bin\");
            Directory.CreateDirectory(path + @"LIBRARY\resources\");
            Directory.CreateDirectory(path + @"LIBRARY\shapes\");
            Directory.CreateDirectory(path + @"LIBRARY\movieclips\");
            Directory.CreateDirectory(path + @"LIBRARY\exports\");
            File.WriteAllText(path + $"{exportName.Split(@"\").Last()}.xfl", "PROXY-CS5");
            File.OpenWrite(path + "DOMDocument.xml").Close();
        }
    }
}