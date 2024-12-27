using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OsuConnectionsGephiGraph.Models;

public class UserPageJSON
{
    [JsonProperty("user_id")]
    public string UserId { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("join_date")]
    public string JoinDate { get; set; }

    [JsonProperty("count300")]
    public string Count300 { get; set; }

    [JsonProperty("count100")]
    public string Count100 { get; set; }

    [JsonProperty("count50")]
    public string Count50 { get; set; }

    [JsonProperty("playcount")]
    public string Playcount { get; set; }

    [JsonProperty("ranked_score")]
    public string RankedScore { get; set; }

    [JsonProperty("total_score")]
    public string TotalScore { get; set; }

    [JsonProperty("pp_rank")]
    public string PpRank { get; set; }

    [JsonProperty("level")]
    public string Level { get; set; }

    [JsonProperty("pp_raw")]
    public string PpRaw { get; set; }

    [JsonProperty("accuracy")]
    public string Accuracy { get; set; }

    [JsonProperty("count_rank_ss")]
    public string CountRankSS { get; set; }

    [JsonProperty("count_rank_ssh")]
    public string CountRankSSH { get; set; }

    [JsonProperty("count_rank_s")]
    public string CountRankS { get; set; }

    [JsonProperty("count_rank_sh")]
    public string CountRankSH { get; set; }

    [JsonProperty("count_rank_a")]
    public string CountRankA { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("total_seconds_played")]
    public string TotalSecondsPlayed { get; set; }

    [JsonProperty("pp_country_rank")]
    public string PpCountryRank { get; set; }

    //[JsonProperty("events")]
    //public List<object> Events { get; set; }
}