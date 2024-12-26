using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace OsuLinkGraphToGephi.Models;

public class GephiNodeCSV{
    [Name("Id")]
    public string Id;
    [Name("Label")]
    public string Label;

    [Name("JoinDate")]
    public string? JoinDate { get; set; }

    [Name("Count300")]
    public string? Count300 { get; set; }

    [Name("Count100")]
    public string? Count100 { get; set; }

    [Name("Count50")]
    public string? Count50 { get; set; }

    [Name("Playcount")]
    public string? Playcount { get; set; }

    [Name("RankedScore")]
    public string? RankedScore { get; set; }

    [Name("TotalScore")]
    public string? TotalScore { get; set; }

    [Name("PpRank")]
    public string? PpRank { get; set; }

    [Name("Level")]
    public string? Level { get; set; }

    [Name("PpRaw")]
    public string? PpRaw { get; set; }

    [Name("Accuracy")]
    public string? Accuracy { get; set; }

    [Name("CountRankSS")]
    public string? CountRankSS { get; set; }

    [Name("CountRankSSH")]
    public string? CountRankSSH { get; set; }

    [Name("CountRankS")]
    public string? CountRankS { get; set; }

    [Name("CountRankSH")]
    public string? CountRankSH { get; set; }

    [Name("CountRankA")]
    public string? CountRankA { get; set; }

    [Name("Country")]
    public string? Country { get; set; }

    [Name("TotalSecondsPlayed")]
    public string? TotalSecondsPlayed { get; set; }

    [Name("PpCountryRank")]
    public string? PpCountryRank { get; set; }
}