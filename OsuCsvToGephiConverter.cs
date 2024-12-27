using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using OsuConnectionsGephiGraph.Models;

namespace OsuConnectionsGephiGraph;

public class OsuCsvToGephiConverter
{
    private string osuFileName = $"{Directory.GetCurrentDirectory()}/res.csv";
    private string nodeFileName = $"{Directory.GetCurrentDirectory()}/nodes.csv";
    private string edgeFileName = $"{Directory.GetCurrentDirectory()}/edges.csv";

    private CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true
    };


    public void readOsuCsv()
    {
        StreamWriter nodeStreamWriter = new StreamWriter(nodeFileName);
        CsvWriter nodeWriter = new CsvWriter(nodeStreamWriter, CultureInfo.InvariantCulture);
        nodeWriter.Context.RegisterClassMap<GephiNodeCSVMap>();
        nodeWriter.WriteHeader<GephiNodeCSV>();
        nodeWriter.NextRecord();

        StreamWriter edgeStreamWriter = new StreamWriter(edgeFileName);
        CsvWriter edgeWriter = new CsvWriter(edgeStreamWriter, CultureInfo.InvariantCulture);
        edgeWriter.Context.RegisterClassMap<GephiEdgeCSVMap>();
        edgeWriter.WriteHeader<GephiEdgeCSV>();
        edgeWriter.NextRecord();

        using (StreamReader csvStreamReader = new StreamReader(osuFileName))
        {
            using (var csvReader = new CsvReader(csvStreamReader, csvConfig))
            {
                var osuLines = csvReader.GetRecords<BaseCSV>();

                foreach (var osuLine in osuLines)
                {
                    GephiNodeCSV node = new GephiNodeCSV();
                    node.Id = osuLine.Id;
                    node.Label = osuLine.Name;
                    node.Country = osuLine.Country;

                    node.JoinDate = osuLine.JoinDate;
                    node.Count300 = osuLine.Count300;
                    node.Count100 = osuLine.Count100;
                    node.Count50 = osuLine.Count50;
                    node.Playcount = osuLine.Playcount;
                    node.RankedScore = osuLine.RankedScore;
                    node.TotalScore = osuLine.TotalScore;
                    node.PpRank = osuLine.PpRank;
                    node.Level = osuLine.Level;
                    node.PpRaw = osuLine.PpRaw;
                    node.Accuracy = osuLine.Accuracy;
                    node.CountRankSS = osuLine.CountRankSS;
                    node.CountRankSSH = osuLine.CountRankSSH;
                    node.CountRankS = osuLine.CountRankS;
                    node.CountRankSH = osuLine.CountRankSH;
                    node.CountRankA = osuLine.CountRankA;
                    node.TotalSecondsPlayed = osuLine.TotalSecondsPlayed;
                    node.PpCountryRank = osuLine.PpCountryRank;

                    nodeWriter.WriteRecord<GephiNodeCSV>(node);
                    nodeWriter.NextRecord();

                    var nodes = osuLine.Nodes;
                    if (nodes == null || nodes == "")
                    {
                        continue;
                    }

                    List<string> nodesList = new List<string>();
                    if (nodes.Contains(','))
                    {
                        nodesList = nodes.Split(',').ToList<string>();
                    }
                    else
                    {
                        nodesList.Add(nodes);
                    }


                    double weight = 1.0 / nodesList.Count;

                    nodesList.ForEach((string targetId) =>
                    {
                        GephiEdgeCSV gephiEdgeCSV = new GephiEdgeCSV();
                        gephiEdgeCSV.Source = node.Id;
                        gephiEdgeCSV.Target = targetId;
                        gephiEdgeCSV.Weight = weight.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture);

                        edgeWriter.WriteRecord<GephiEdgeCSV>(gephiEdgeCSV);
                        edgeWriter.NextRecord();
                    });
                }
            }
        }

        nodeWriter.Dispose();
        edgeWriter.Dispose();
    }


}