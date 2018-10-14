using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NeoModules.Rest.DTOs.NeoScan;
using NeoModules.Rest.DTOs.Switcheo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeoModules.Rest.Services
{

    public enum SwitcheoNet
    {
        MainNet,
        TestNet
    }


    public class SwitcheoRestService // todo auth
    {
        private readonly HttpClient _restClient;

        private static readonly string switcheoTestNetUrl = "https://test-api.switcheo.network/v2/";
        private static readonly string neoScanMainNetUrl = "https://api.switcheo.network/v2/";

        private static readonly string getTimeStamp = "exchange/timestamp/";
        private static readonly string getContracts = "exchange/contracts/";
        private static readonly string getTokens = "exchange/tokens/";
        private static readonly string getPairs = "exchange/pairs";
        private static readonly string getCandleSticks = "tickers/candlesticks/";
        private static readonly string get24HourData = "tickers/last_24_hours/";
        private static readonly string getLastPrice = "tickers/last_price/";

        public SwitcheoRestService(SwitcheoNet net)
        {
            _restClient = net == SwitcheoNet.MainNet
                ? new HttpClient { BaseAddress = new Uri(neoScanMainNetUrl) }
                : new HttpClient { BaseAddress = new Uri(switcheoTestNetUrl) };
        }

        public async Task<long> GetTimeStampAsync()
        {
            var result = await _restClient.GetAsync(getTimeStamp);
            var data = (string)JObject.Parse(await result.Content.ReadAsStringAsync())["timestamp"];
            return long.Parse(data);
        }

        public async Task<JObject> GetContractsAsync() // returns a JObject because the json it does not have a specific format
        {
            var result = await _restClient.GetAsync(getContracts);
            return JObject.Parse(await result.Content.ReadAsStringAsync());
        }

        public async Task<Dictionary<string, TokenData>> GetTokensAsync()
        {
            var result = await _restClient.GetAsync(getTokens);
            return TokenData.FromJson(await result.Content.ReadAsStringAsync());
        }

        public async Task<List<string>> GetPairsAsync()
        {
            var result = await _restClient.GetAsync(getPairs);
            return JsonConvert.DeserializeObject<List<string>>(await result.Content.ReadAsStringAsync());
        }

        public async Task<List<CandleStick>> GetCandleSticksAsync(string pair, int startTime, int endTime, int interval)
        {
            //TODO args validation
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pair"] = pair;
            query["interval"] = interval.ToString();
            query["start_time"] = startTime.ToString();
            query["end_time"] = endTime.ToString();
            string queryString = query.ToString().Insert(0, "?");
            var result = await _restClient.GetAsync(Utils.ComposeUrl(getCandleSticks, queryString));
            return CandleStick.FromJson(await result.Content.ReadAsStringAsync());
        }

        public async Task<List<CandleStick>> Get24HourDataAsync()
        {
            var result = await _restClient.GetAsync(get24HourData);
            return CandleStick.FromJson(await result.Content.ReadAsStringAsync());
        }

    }
}
