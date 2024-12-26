using CsvHelper.Configuration;

namespace OsuLinkGraphToGephi.Models;

public class GephiEdgeCSVMap : ClassMap<GephiEdgeCSV>{
    public GephiEdgeCSVMap(){
        Map(m => m.Source).Name("Source");
        Map(m => m.Target).Name("Target");
        Map(m => m.Label).Name("Label");
        Map(m => m.Weight).Name("Weight");
    }
}