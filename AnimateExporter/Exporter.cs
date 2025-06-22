using System.IO.Compression;

namespace AnimateExporter
{
    public static class Exporter
    {
        public static bool foundExport { get; set; } = false;
        public static int Export(string inputFla, List<string> args)
        {
            string exportName = args[0];
            bool allAssets = args[1] == "true";
            Console.WriteLine($"UnZipping {inputFla}");
            string outputZip = UnZIP(inputFla);
            Console.WriteLine($"Creating base {exportName} fla");
            Fla.CreateBaseFla(Environment.CurrentDirectory + @"\" + exportName);
            Console.WriteLine("Finding Export and its childs");
            foreach (string export in Directory.GetFiles(outputZip + @"LIBRARY\exports"))
            {
                if (Path.GetFileNameWithoutExtension(export).Equals(exportName, StringComparison.OrdinalIgnoreCase))
                {
                    foundExport = true;
                    Console.WriteLine($"Found export at {export}");
                    ExportManager.exportPath = export;
                    ExportManager.baseDir = Environment.CurrentDirectory + @"\" + exportName;
                    ExportManager.allAssets = allAssets;
                    ExportManager.flaName = inputFla;
                    ExportManager.Export();
                }
            }
            if (!foundExport)
            {
                Console.WriteLine($"Export not found: {exportName}");
                return -1;   
            }
            return 0;
        }
        public static string UnZIP(string inputFile)
        {
            string outputPath = $"{inputFile.Replace(".fla", @"\")}";
            if (Directory.Exists(outputPath)) return outputPath;
            File.Copy(inputFile, inputFile.Replace(".fla", ".zip"));
            Directory.CreateDirectory(outputPath);
            ZipFile.ExtractToDirectory(inputFile.Replace(".fla", ".zip"), outputPath);
            File.Delete(inputFile.Replace(".fla", ".zip"));
            return outputPath;
        }
    }
}