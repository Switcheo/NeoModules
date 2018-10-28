using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NeoModules.Core;
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

        private const string getTimeStamp = "exchange/timestamp";
        private const string getContracts = "exchange/contracts";
        private const string getTokens = "exchange/tokens";
        private const string getPairs = "exchange/pairs";
        private const string getCandleSticks = "tickers/candlesticks";
        private const string get24HourData = "tickers/last_24_hours";
        private const string getLastPrice = "tickers/last_price";
        private const string getOffers = "offers";
        private const string getTrades = "trades";
        private const string getRecentTrades = "trades/recent";
        private const string getBalances = "balances"; //todo
        private const string createDeposit = "deposits";
        private const string executeDeposit = "deposits/:id/broadcast";
        private const string createWithdrawl = "withdrawals";
        private const string executeWithdrawl = "withdrawals/:id/broadcast";
        private const string orders = "orders";
        private const string executeOrder = "orders/:id/broadcast";

        public string ContractHash { get; set; }
        public string Blockchain { get; set; }

        public SwitcheoRestService(SwitcheoNet net)
        {
            _restClient = net == SwitcheoNet.MainNet
                ? new HttpClient { BaseAddress = new Uri(neoScanMainNetUrl) }
                : new HttpClient { BaseAddress = new Uri(switcheoTestNetUrl) };
        }

        /// <summary>
        /// Gets the last contract hash for the specified blockchain
        /// Sets the Blockchain and ContractHash fields for future calls
        /// <param name="blockchain"></param>
        /// </summary>
        public async Task InitService(string blockchain)
        {
            var contracts = await GetContractsAsync();
            var lastContractHash = contracts[blockchain.ToUpperInvariant()].Last.ToObject<string>();
            if (!string.IsNullOrEmpty(lastContractHash))
            {
                ContractHash = lastContractHash;
                Blockchain = blockchain.ToLowerInvariant();
            }
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

        /// <summary>
        /// Returns 20 most recent trades on all pairs sorted in descending order by executed time.
        /// </summary>
        /// <param name="pair"></param>
        /// <returns></returns>
        public async Task<List<RecentTrade>> GetRecentTradesAsync(string pair)
        {
            if (string.IsNullOrEmpty(pair)) throw new ArgumentNullException(nameof(pair));
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["pair"] = pair;
            string queryString = query.ToString().Insert(0, "?");
            var result = await _restClient.GetAsync(Utils.ComposeUrl(getRecentTrades, queryString));
            return RecentTrade.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="blockchain"></param>
        /// <param name="contractHash"></param>
        /// <param name="assetId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<Transact> PrepareCreateDeposit(string assetId, string amount, string blockchain = "", string contractHash = "")
        {
            if (string.IsNullOrEmpty(blockchain)) throw new ArgumentNullException(nameof(blockchain));
            if (string.IsNullOrEmpty(assetId)) throw new ArgumentNullException(nameof(assetId));
            if (string.IsNullOrEmpty(amount)) throw new ArgumentNullException(nameof(amount));
            if (string.IsNullOrEmpty(contractHash)) contractHash = ContractHash;
            if (string.IsNullOrEmpty(blockchain)) blockchain = Blockchain;

            var timestamp = await GetTimeStampAsync();
            var tokens = await GetTokensAsync();
            int decimals;
            if (tokens.ContainsKey(assetId))
            {
                decimals = tokens[assetId].Decimals;
            }
            else
            {
                throw new Exception($"{assetId} is not available for deposit in Switcheo");
            }
            var transact = new Transact
            {
                Blockchain = blockchain,
                AssetId = assetId,
                Timestamp = timestamp,
                ContractHash = contractHash,
                Amount = (long)BigDecimal.Parse(amount, byte.Parse(decimals.ToString())).Value
            };

            return transact;
        }

        /// <summary>
        /// This endpoint creates a deposit which can be executed through Execute Deposit.
        /// To be able to make a deposit, sufficient funds are required in the depositing wallet.
        /// IMPORTANT: After calling this endpoint, the Execute Deposit endpoint has to be called for the deposit to be executed. 
        /// </summary>
        /// <param name="apiParams"></param>
        /// <returns></returns>
        public async Task<string> CreateDeposit(string apiParams)
        {
            var httpContent = new StringContent(apiParams, Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(createDeposit, httpContent);
            return await result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// This is the second endpoint required to execute a deposit.
        /// After using the Create Deposit endpoint, you will receive a response which requires additional signing.
        /// The signature should then be attached as the signature parameter in the request payload.
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> ExecuteDeposit(string signature, string id)
        {
            var json = new JObject { ["signature"] = signature };
            var httpContent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(executeDeposit.Replace(":id", id), httpContent);
            return await result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="blockchain"></param>
        /// <param name="contractHash"></param>
        /// <param name="assetId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<Transact> PrepareCreateWithdrawal(string assetId, string amount, string blockchain = "", string contractHash = "")
        {
            if (string.IsNullOrEmpty(assetId)) throw new ArgumentNullException(nameof(assetId));
            if (string.IsNullOrEmpty(amount)) throw new ArgumentNullException(nameof(amount));
            if (string.IsNullOrEmpty(contractHash)) contractHash = ContractHash;
            if (string.IsNullOrEmpty(blockchain)) blockchain = Blockchain;

            var timestamp = await GetTimeStampAsync();
            var tokens = await GetTokensAsync();
            int decimals;
            if (tokens.ContainsKey(assetId))
            {
                decimals = tokens[assetId].Decimals;
            }
            else
            {
                throw new Exception($"{assetId} is not available for withdrawal in Switcheo");
            }
            var transact = new Transact
            {
                Blockchain = blockchain,
                AssetId = assetId,
                Timestamp = timestamp,
                ContractHash = contractHash,
                Amount = (long)BigDecimal.Parse(amount, byte.Parse(decimals.ToString())).Value
            };

            return transact;
        }

        /// <summary>
        /// This endpoint creates a withdrawal which can be executed through Execute Withdrawal.
        /// To be able to make a withdrawal, sufficient funds are required in the contract balance.
        /// A signature of the request payload has to be provided for this API call.
        /// </summary>
        /// <param name="apiParams"></param>
        /// <returns></returns>
        public async Task<string> CreateWithdrawl(string apiParams)
        {
            var httpContent = new StringContent(apiParams, Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(createWithdrawl, httpContent);
            return await result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// This is the second endpoint required to execute a withdrawal.
        /// After using the Create Withdrawal endpoint, you will receive a response which requires additional signing.
        /// </summary>
        /// <param name="withdrawal"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public async Task<WithdrawlResponse> ExecuteWithdraw(ExecuteWithdrawl withdrawal, string signature)
        {
            var apiParams = new JObject
            {
                ["id"] = withdrawal.Id,
                ["timestamp"] = withdrawal.Timestamp,
                ["signature"] = signature
            };

            var httpContent = new StringContent(apiParams.ToString(), Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(executeWithdrawl.Replace(":id", withdrawal.Id.ToString()), httpContent);
            return WithdrawlResponse.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Retrieves orders from a specific address filtered by the given parameters.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="contractHash"></param>
        /// <param name="pair"></param>
        /// <param name="from"></param>
        /// <param name="orderStatus"></param>
        /// <param name="beforeId"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<OrderResponse> GetOrders(string address, string contractHash, string pair = "",
            int from = -1, string orderStatus = "", string beforeId = "", int limit = -1)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["address"] = address;
            query["contract_hash"] = contractHash;

            //optional args
            if (!string.IsNullOrEmpty(pair)) query["pair"] = pair;
            if (!string.IsNullOrEmpty(orderStatus)) query["orderStatus"] = orderStatus;
            if (!string.IsNullOrEmpty(beforeId)) query["beforeId"] = beforeId;
            if (from >= 0) query["from"] = from.ToString();
            if (limit >= 0) query["limit"] = limit.ToString();

            string queryString = query.ToString().Insert(0, "?");

            var result = await _restClient.GetAsync(Utils.ComposeUrl(orders, queryString));
            return OrderResponse.FromJson(await result.Content.ReadAsStringAsync());
        }


        public async Task<OrderRequest> PrepareCreateOrder(string pair, string side, string price, string wantAmount, bool useNativeTokens,
            string orderType, string blockchain = "", string contractHash = "") //todo checkDecimals, otcaddress, and fixed8 stuff
        {
            if (string.IsNullOrEmpty(pair)) throw new ArgumentNullException(nameof(pair));
            if (string.IsNullOrEmpty(side)) throw new ArgumentNullException(nameof(side));
            if (string.IsNullOrEmpty(price)) throw new ArgumentNullException(nameof(price));
            if (string.IsNullOrEmpty(wantAmount)) throw new ArgumentNullException(nameof(wantAmount));
            if (string.IsNullOrEmpty(orderType)) throw new ArgumentNullException(nameof(orderType));
            //if (string.IsNullOrEmpty(otcAddress)) throw new ArgumentNullException(nameof(otcAddress));
            if (string.IsNullOrEmpty(contractHash)) contractHash = ContractHash;
            if (string.IsNullOrEmpty(blockchain)) blockchain = Blockchain;

            var timestamp = await GetTimeStampAsync();
            var order = new OrderRequest
            {
                Blockchain = blockchain,
                ContractHash = contractHash,
                OrderType = orderType,
                Pair = pair,
                Price = price,
                Side = side,
                Timestamp = timestamp,
                UseNativeTokens = useNativeTokens,
                WantAmount = wantAmount
            };
            return order;
        }

        /// <summary>
        /// This endpoint creates an order which can be executed through Broadcast Order.
        /// Orders can only be created after sufficient funds have been deposited into the user's contract balance.
        /// A successful order will have zero or one make and/or zero or more fills.
        /// </summary>
        /// <param name="apiParams"></param>
        /// <returns></returns>
        public async Task<OrderResponse> CreateOrder(string apiParams)
        {
            var httpContent = new StringContent(apiParams, Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(orders, httpContent);
            return OrderResponse.FromJson(await result.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// This is the second endpoint required to execute an order.
        /// After using the Create Order endpoint, you will receive a response which needs to be signed.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> ExecuteOrder(ExecuteOrder order, string id) //todo ExecuteOrderResponse
        {
            var json = JsonConvert.SerializeObject(order);
            var signaturesJson = $"{{\"signatures\":{json}}}";
            var httpContent = new StringContent(signaturesJson, Encoding.UTF8, "application/json");
            var result = await _restClient.PostAsync(executeOrder.Replace(":id", id), httpContent);
            //return ExecuteOrderResponse.FromJson(await result.Content.ReadAsStringAsync()); 
            return await result.Content.ReadAsStringAsync(); 
        }
    }
}
