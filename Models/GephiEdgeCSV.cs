using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace OsuConnectionsGephiGraph.Models;

public class GephiEdgeCSV{
    [Name("Source")]
    public string Source;

    [Name("Target")]
    public string Target;

    [Name("Label")]
    public string? Label;

    [Name("Weight")]
    public string? Weight;    
}