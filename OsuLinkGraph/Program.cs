using System.Dynamic;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using OsuLinkGraph.Models;
using System.Threading.Channels;
using System.Text.RegularExpressions;
using System.Globalization;


namespace OsuLinkGraph;

class Program
{
    static void Main(string[] args)
    {
        csvGenerator gen = new csvGenerator();
        List<string> scrapeList = new List<string>(){};
        gen.init(depth: 5, apiKey: "");

        foreach(string scrapeUid in scrapeList)
        {
            try
            {
                gen.execute(scrapeUid);
            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }   
        }
        
        gen.deinit();

    }
}

class csvGenerator {
    private const string resFileName = "../../../res.csv";
    private static HttpClient client = new HttpClient();

    private string apiKey;

    private HashSet<string> searchedIds = new HashSet<string>();
    private HashSet<string> searchedIdsThisSession = new HashSet<string>();

    private int origDepth;

    private CsvConfiguration csvConfig;

    private StreamWriter csvStreamWriter;

    private CsvWriter csvWriter;

    private void initSearchedIdsSet(CsvReader csvReader){
        
        var lines = csvReader.GetRecords<BaseCSV>();
        foreach(var line in lines){
            searchedIds.Add(line.Id);
        }
        
    }

    public void execute(string uId){
        searchRef(uId,origDepth);
    }

    public void init(int depth, string apiKey) {
        if(apiKey == null || apiKey == ""){
            throw new ArgumentNullException("Provide an APIKey");
        }else{
            this.apiKey = apiKey
        }

        if(depth>0){
            origDepth = depth;
        }
        else{
            throw new ArgumentOutOfRangeException($"Depth must be a positive Integer above 0! Depth was {depth}");
        }
        bool fileExists = File.Exists(resFileName);

        if(fileExists){
            csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){
                HasHeaderRecord = false
            };

            csvConfig.MissingFieldFound = null;

            using(StreamReader csvStreamReader = new StreamReader(resFileName))
            {
                using(var csvReader = new CsvReader(csvStreamReader,csvConfig))
                {
                    initSearchedIdsSet(csvReader);
                }
            }
            
            csvStreamWriter = new StreamWriter(resFileName,append:true );
            csvWriter = new CsvWriter(csvStreamWriter,CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<BaseCSVMap>();
        }
        else
        {
            csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture){
                HasHeaderRecord = true
            };

            csvStreamWriter = new StreamWriter(resFileName);
            csvWriter = new CsvWriter(csvStreamWriter,CultureInfo.InvariantCulture);
            csvWriter.Context.RegisterClassMap<BaseCSVMap>();
            csvWriter.WriteHeader<BaseCSV>();
        }

        

    }

    public void deinit(){
        csvWriter.Dispose();
    }

    private void searchRef(string userId, int depth = 1){
        if(searchedIdsThisSession.Contains(userId)){
            return;
        }

        var connectedUserIds = callUserpage(userId);
        var userPage = getUserPageJSON(userId);

        if(! searchedIds.Contains(userId)){
            writeUserToCSV(connectedUserIds,userPage);
        }

        searchedIds.Add(userId);
        searchedIdsThisSession.Add(userId);

        if(depth == 0){
            return;
        }
      
        double percBase = connectedUserIds.Count;

        int iter = 1;
        connectedUserIds.ForEach((string conUserId)=>{
            if(depth == origDepth){
                Console.WriteLine($"{(100/percBase*iter).ToString("0.00")}% of id:{userId} name:{userPage.Username} scraped");
            }
            searchRef(conUserId, depth - 1);
            iter++;
        });
    }

    private void writeUserToCSV(List<string> nodes, UserPageJSON userPage){
        BaseCSV csvLine = new BaseCSV();
        csvLine.Id = userPage.UserId;
        csvLine.Name = userPage.Username;
        csvLine.Nodes = String.Join(",",nodes);
        
        csvLine.JoinDate = userPage.JoinDate;
        csvLine.Count300 = userPage.Count300;
        csvLine.Count100 = userPage.Count100;
        csvLine.Count50 = userPage.Count50;
        csvLine.Playcount = userPage.Playcount;
        csvLine.RankedScore = userPage.RankedScore;
        csvLine.TotalScore = userPage.TotalScore;
        csvLine.PpRank = userPage.PpRank;
        csvLine.Level = userPage.Level;
        csvLine.PpRaw = userPage.PpRaw;
        csvLine.Accuracy = userPage.Accuracy;
        csvLine.CountRankSS = userPage.CountRankSS;
        csvLine.CountRankSSH = userPage.CountRankSSH;
        csvLine.CountRankS = userPage.CountRankS;
        csvLine.CountRankSH = userPage.CountRankSH;
        csvLine.Country = userPage.Country;
        csvLine.CountRankA = userPage.CountRankA;
        csvLine.TotalSecondsPlayed = userPage.TotalSecondsPlayed;
        csvLine.PpCountryRank = userPage.PpCountryRank;

        csvWriter.NextRecord();
        csvWriter.WriteRecord(csvLine);       
    }

    
    private UserPageJSON getUserPageJSON(string uID)
    {
        int retries = 5;
        string exceptionMessage = "";

        string URL = $"https://osu.ppy.sh/api/get_user?k={apiKey}&u={uID}";
        
        do
        {
            try
            {
                //let's not abuse api pls; "If you are doing more than 60 requests a minute, you should probably give peppy a yell"
                System.Threading.Thread.Sleep(1000);

                retries--;
        
                HttpResponseMessage response = client.GetAsync(URL).Result;

                string jsonResponse = response.Content.ReadAsStringAsync().Result;

                JArray jsonArray = JArray.Parse(jsonResponse);
                List<UserPageJSON> users = JsonConvert.DeserializeObject<List<UserPageJSON>>(jsonResponse);

                if (users.Count > 0)
                {
                    return users.First();
                }
                else 
                {
                    Console.WriteLine("error! Is user " + uID + " restricted?");
                    
                    UserPageJSON userPage = new UserPageJSON();
                    userPage.UserId = uID;
                    userPage.Username = uID + "_RESTRICTED";
                    return userPage;
                }
            }
            catch(Exception e){
                exceptionMessage = e.Message;
            }
        }
        while(retries > 0);

        throw new Exception($"Failed at id: {uID}\n{exceptionMessage}");
    }

    private List<string> callUserpage(string uID)
    {
        string URL = "https://osu.ppy.sh/pages/include/profile-userpage.php?u=" + uID;
        HttpResponseMessage response = client.GetAsync(URL).Result;
        string pageContent = response.Content.ReadAsStringAsync().Result;
        
        List<string> userids = new List<string>();

        //declare our regex
        Regex userIDRegex = new Regex(@"(?:users\/(?<userId>\w+)|/u\/(?<userIdAlt>\w+)|\[profile=(?<profileId>\d+))");

        MatchCollection userIDMatches = userIDRegex.Matches(pageContent);

        foreach (Match userIDmatch in userIDMatches)
        {
            string id = "";

            if (userIDmatch.Groups["userId"].Success)
                id = userIDmatch.Groups["userId"].Value;
            else if (userIDmatch.Groups["userIdAlt"].Success)
                id = userIDmatch.Groups["userIdAlt"].Value;
            else if (userIDmatch.Groups["profileId"].Success)
                id = userIDmatch.Groups["profileId"].Value;

            if (!string.IsNullOrEmpty(id))
            {
                userids.Add(id);
            }
        }

        return userids;
    }

}
