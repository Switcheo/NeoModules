using System.Collections.Generic;
using Newtonsoft.Json;

namespace NeoModules.Rest.DTOs
{
    public class TokenList
    {
        [JsonProperty("current_height")]
        public long CurrentHeight { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("page")]
        public long Page { get; set; }

        [JsonProperty("page_len")]
        public long PageLen { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public class Result
    {
        [JsonProperty("block")]
        public long Block { get; set; }

        [JsonProperty("contract")]
        public TokenContract Contract { get; set; }

        [JsonProperty("token")]
        public Token Token { get; set; }

        [JsonProperty("tx")]
        public string Tx { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class TokenContract
    {
        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("code")]
        public Code Code { get; set; }

        [JsonProperty("code_version")]
        public string CodeVersion { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("version")]
        public long Version { get; set; }
    }

    public class Code
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("parameters")]
        public string Parameters { get; set; }

        [JsonProperty("returntype")]
        public long Returntype { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }
    }

    public class Properties
    {
        [JsonProperty("dynamic_invoke")]
        public bool DynamicInvoke { get; set; }

        [JsonProperty("storage")]
        public bool Storage { get; set; }
    }

    public class Token
    {
        [JsonProperty("contract_address")]
        public string ContractAddress { get; set; }

        [JsonProperty("decimals")]
        public long Decimals { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("script_hash")]
        public string ScriptHash { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }
    }
}
