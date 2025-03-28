using CsvHelper.Configuration;

namespace OsuConnectionsGephiGraph.Models;

public class GephiNodeCSVMap : ClassMap<GephiNodeCSV>{
    public GephiNodeCSVMap(){
        Map(m => m.Id).Name("Id");
        Map(m => m.Label).Name("Label");
        Map(m => m.JoinDate).Name("JoinDate");
        Map(m => m.Count300).Name("Count300");
        Map(m => m.Count100).Name("Count100");
        Map(m => m.Count50).Name("Count50");
        Map(m => m.Playcount).Name("Playcount");
        Map(m => m.RankedScore).Name("RankedScore");
        Map(m => m.TotalScore).Name("TotalScore");
        Map(m => m.PpRank).Name("PpRank");
        Map(m => m.Level).Name("Level");
        Map(m => m.PpRaw).Name("PpRaw");
        Map(m => m.Accuracy).Name("Accuracy");
        Map(m => m.CountRankSS).Name("CountRankSS");
        Map(m => m.CountRankSSH).Name("CountRankSSH");
        Map(m => m.CountRankS).Name("CountRankS");
        Map(m => m.CountRankSH).Name("CountRankSH");
        Map(m => m.CountRankA).Name("CountRankA");
        Map(m => m.Country).Name("Country");
        Map(m => m.TotalSecondsPlayed).Name("TotalSecondsPlayed");
        Map(m => m.PpCountryRank).Name("PpCountryRank");
    }
}