using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NeoModules.Rest.Services
{
    public enum NeoScanNet
    {
        MainNet,
        TestNet
    }

    public class NeoScanRestService : INeoRestService
    {
        private static readonly string neoScanTestNetUrl = "https://neoscan-testnet.io/api/test_net/v1/";
        private static readonly string neoScanMainNetUrl = "https://neoscan.io/api/main_net/v1/";
        private static readonly string getBalanceUrl = "get_balance/";
        private static readonly string getClaimedUrl = "get_claimed/";
        private static readonly string getClaimableUrl = "get_claimable/";
        private static readonly string getUnclaimedUrl = "get_unclaimed/";
        private static readonly string getAddressUrl = "get_address/";
        private static readonly string getAllNodes = "get_all_nodes/";
        private static readonly string getTransaction = "get_transaction/";
        private static readonly string getAddressAbstracts = "get_address_abstracts/";
        private static readonly string getAddressNeon = "get_address_neon/";
        private static readonly string getAddressToAddressAbstracts = "get_address_to_address_abstracts/";
        private static readonly string getAsset = "get_asset/";
        private static readonly string getAssets = "get_assets/";
        private static readonly string getBlock = "get_block/";
        private static readonly string getFeesInRange = "get_fees_in_range/";
        private static readonly string getHeight = "get_height/";
        private static readonly string getHighestBlock = "get_highest_block/";
        private static readonly string getLastBlocks = "get_last_blocks/";
        private static readonly string getLastTransactions = "get_last_transactions/";
        private static readonly string getLastTransactionsByAddress = "get_last_transactions_by_address/";
        private static readonly string getNodes = "get_nodes/";


        private readonly HttpClient _restClient;

        public NeoScanRestService(NeoScanNet net)
        {
            _restClient = net == NeoScanNet.MainNet
                ? new HttpClient {BaseAddress = new Uri(neoScanMainNetUrl)}
                : new HttpClient {BaseAddress = new Uri(neoScanTestNetUrl)};
        }

        // TODO: I can refractor this more, move the 3 lines of each call to a function
        public async Task<string> GetBalanceAsync(string address)
        {
            var composedUrl = ComposeAddressUrl(getBalanceUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetClaimableAsync(string address)
        {
            var composedUrl = ComposeAddressUrl(getClaimableUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetClaimedAsync(string address)
        {
            var composedUrl = ComposeAddressUrl(getClaimedUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetUnclaimedAsync(string address)
        {
            var composedUrl = ComposeAddressUrl(getUnclaimedUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetAddressAsync(string address)
        {
            var composedUrl = ComposeAddressUrl(getAddressUrl, address);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetTransactionAsync(string hash)
        {
            var composedUrl = ComposeAddressUrl(getTransaction, hash);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public void ChangeNet(NeoScanNet net)
        {
            if (_restClient == null) return;
            switch (net)
            {
                case NeoScanNet.MainNet:
                    _restClient.BaseAddress = new Uri(neoScanMainNetUrl);
                    return;
                case NeoScanNet.TestNet:
                    _restClient.BaseAddress = new Uri(neoScanTestNetUrl);
                    return;
            }
        }

        public async Task<string> GetAllNodesAsync()
        {
            var result = await _restClient.GetAsync(getAllNodes);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetAssetsAsync()
        {
            var result = await _restClient.GetAsync(getAssets);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetAssetAsync(string assetHash)
        {
            var composedUrl = ComposeAddressUrl(getAsset, assetHash);
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetHeight()
        {
            var result = await _restClient.GetAsync(getHeight);
            var data = await result.Content.ReadAsStringAsync();
            return JObject.Parse(data)["height"].ToString();
        }

        public async Task<string> GetHighestBlock()
        {
            var result = await _restClient.GetAsync(getHighestBlock);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetLastBlocks()
        {
            var result = await _restClient.GetAsync(getLastBlocks);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<string> GetFeesInRange(int range1, int range2)
        {
            var composedUrl = ComposeAddressUrl(getFeesInRange, string.Concat(range1, "-", range2));
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }

        private string ComposeAddressUrl(string url, string address)
        {
            return string.Format("{0}{1}", url, address);
        }
    }
}