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
            Console.WriteLine("Usage: AnimateExporter.exe [InputFla] [ExportName]");
            return;
        }
        else
        {
            Exporter.Export(Arguments[0], Arguments[1]);
        }
    }
}