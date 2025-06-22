namespace AnimateExporter;

/*
Usage:
AnimateExporter.exe inputFla exportName
*/
internal class Program
{
    public static List<string>? Arguments { get; set; }
    static void Main(string[] args)
    {
        Arguments = args.ToList();
        if (Arguments.Count < 2)
        {
            Console.WriteLine("Usage: AnimateExporter.exe [InputFla] [ExportName] <bool AllAssets?>");
            return;
        }
        else if (Arguments.Count == 3)
        {
            Exporter.Export(Arguments[0], [Arguments[1], Arguments[2]]);
        }
        else if (Arguments.Count == 2)
        {
            Exporter.Export(Arguments[0], [Arguments[1], "true"]); // true by default atm
        }
    }
}