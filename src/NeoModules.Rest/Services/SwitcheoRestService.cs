using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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

        private static readonly string getTimeStamp = "exchange/timestamp";
        private static readonly string getContracts = "exchange/contracts";
        private static readonly string getTokens = "exchange/tokens";
        private static readonly string getPairs = "exchange/pairs";
        private static readonly string getCandleSticks = "tickers/candlesticks";
        private static readonly string get24HourData = "tickers/last_24_hours";
        private static readonly string getLastPrice = "tickers/last_price";
        private static readonly string getOffers = "offers";
        private static readonly string getTrades = "trades";

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

        /// <summary>
        /// Returns candlestick chart data filtered by url parameters.
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="startTime">Start of time range for data in epoch seconds</param>
        /// <param name="endTime">End of time range for data in epoch seconds</param>
        /// <param name="interval">Candlestick period in minutes Possible values are: 1, 5, 30, 60, 360, 1440</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns 24-hour data for all pairs and markets.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CandleStick>> Get24HourDataAsync()
        {
            var result = await _restClient.GetAsync(get24HourData);
            return CandleStick.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Returns last price of given symbol(s). Defaults to all symbols.
        /// </summary>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, LastPrice>> GetLastPriceAsync(List<string> symbols) 
            //todo ask for how to create query of array, did not work with "?symbols=GAS&symbols=NEO"
        {
            // ignore param for now
            //    var query = HttpUtility.ParseQueryString(string.Empty);
            //    foreach (var symbol in symbols)
            //    {
            //        query["symbols"] = symbol;
            //    }

            var result = await _restClient.GetAsync(getLastPrice);
            return LastPrice.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Retrieves the best 70 offers (per side) on the offer book.
        /// </summary>
        /// <param name="blockchain"></param>
        /// <param name="pair"></param>
        /// <param name="contractHash"></param>
        /// <returns></returns>
        public async Task<List<Offers>> GetOffers(string blockchain, string pair, string contractHash)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["blockchain"] = blockchain;
            query["pair"] = pair;
            query["contract_hash"] = contractHash;
            string queryString = query.ToString().Insert(0, "?");
            var result = await _restClient.GetAsync(Utils.ComposeUrl(getOffers, queryString));
            return Offers.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Retrieves trades that have already occurred on Switcheo Exchange filtered by the request parameters.
        /// </summary>
        /// <param name="contractHash"></param>
        /// <param name="pair"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<Trade>> GetTradesAsync(string contractHash, string pair, int from = 0, int to = 0, int limit = 0)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pair"] = pair;
            query["contract_hash"] = contractHash;
            if (from != 0) query["from"] = from.ToString();
            if (to != 0) query["to"] = to.ToString();
            if (limit != 0) query["limit"] = limit.ToString();
            string queryString = query.ToString().Insert(0, "?");
            var result = await _restClient.GetAsync(Utils.ComposeUrl(getTrades, queryString));
            return Trade.FromJson(await result.Content.ReadAsStringAsync());
        }
    }
}
