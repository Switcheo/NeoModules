using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs.HappyNodes
{
    public class FlatNode
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("protocol")]
        public Protocol Protocol { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("p2p_tcp_status")]
        public bool P2PTcpStatus { get; set; }

        [JsonProperty("p2p_ws_status")]
        public bool P2PWsStatus { get; set; }

        [JsonProperty("address")]
        public Uri Address { get; set; }

        [JsonProperty("validated_peers_counts")]
        public long ValidatedPeersCounts { get; set; }

        [JsonProperty("stability")]
        public long Stability { get; set; }

        [JsonProperty("blockheight_score")]
        public long BlockheightScore { get; set; }

        [JsonProperty("normalised_latency_score")]
        public double NormalisedLatencyScore { get; set; }

        [JsonProperty("validated_peers_counts_score")]
        public double ValidatedPeersCountsScore { get; set; }

        [JsonProperty("health_score")]
        public double HealthScore { get; set; }

        [JsonProperty("latency")]
        public double Latency { get; set; }

        [JsonProperty("rcp_https_status")]
        public bool? RcpHttpsStatus { get; set; }

        [JsonProperty("rcp_http_status")]
        public bool? RcpHttpStatus { get; set; }

        [JsonProperty("mempool_size")]
        public long MempoolSize { get; set; }

        [JsonProperty("connection_counts")]
        public long ConnectionCounts { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("blockheight")]
        public long Blockheight { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("long")]
        public double Long { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("max_blockheight")]
        public long MaxBlockheight { get; set; }

        public static IList<FlatNode> FromJson(string json) => JsonConvert.DeserializeObject<List<FlatNode>>(json, Utils.Settings);

        public static FlatNode FromSingleNodeJson(string json) => JsonConvert.DeserializeObject<FlatNode>(json, Utils.Settings);
    }
}
