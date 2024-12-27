using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.CommandLineUtils;

namespace OsuConnectionsGephiGraph;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;

class Program
{
    static int Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var appsettings = configuration.GetSection("AppSettings");

        Option<string> userIdsOption = new(
            "-u",
            "List of userIds used as starting points for recursive scraping\nFormat: -u \"12345,67891,13456\"");

        Option<int> depthOption = new(
            "-d",
            "Depth of recursive scraping per provided userId");
        depthOption.IsRequired = true;
        
        RootCommand rootCommand = new(description: "Scrapes OSU for userIds and creates files ready for import into gephi"){
            userIdsOption,
            depthOption
        };

        rootCommand.SetHandler((userIds, depth) =>
            {
                string apiKey = appsettings["OSU_API_KEY"];
                int timeOutMs = int.Parse(appsettings["TIMEOUT_MS"]);

                if (string.IsNullOrEmpty(apiKey) || timeOutMs < 0)
                {
                    throw new ArgumentException("appsettings.json has incorrect configuration.");
                }

                List<string> userIdsList = userIds.Split(',').ToList<string>();

                OsuCsvGenerator gen = new OsuCsvGenerator();
                gen.init(depth, apiKey);

                foreach (var scrapeUid in userIdsList)
                {
                    try
                    {
                        gen.execute(scrapeUid);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                gen.deinit();

                OsuCsvToGephiConverter conv = new OsuCsvToGephiConverter();
                conv.readOsuCsv();

                return Task.CompletedTask;
            },
            userIdsOption,
            depthOption
        );

        rootCommand.AddGlobalOption(new Option<bool>(
            "--help",
            "Display help information"));

        return rootCommand.InvokeAsync(args).Result;
    }
}