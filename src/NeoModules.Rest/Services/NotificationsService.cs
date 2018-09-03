using System;
using System.Net.Http;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.NeoNotifications;
using NeoModules.Rest.Interfaces;
using Newtonsoft.Json;

namespace NeoModules.Rest.Services
{
    public class NotificationsService : INotificationsService
    {
        private const string NotificationsMainNetUrl = "https://nX.cityofzion.io/v1/";
        private const string AddressNotificationsUrl = "notifications/addr/";
        private const string BlockNotificationsUrl = "notifications/block/";
        private const string ContractNotificationsUrl = "notifications/contract/";
        private const string TokensUrl = "tokens";

        private readonly HttpClient _restClient;

        public NotificationsService(string url)
        {
            _restClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        public NotificationsService(int node = 1)
        {
            if (node > 5) throw new ArgumentOutOfRangeException(nameof(node));
            string url = NotificationsMainNetUrl.Replace("X", node.ToString());
            _restClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        public async Task<ContractResult> GetContractNotifications(string scriptHash, int page = 1,
            string eventType = "", int afterBlock = 0, int pageSize = 100)
        {
            if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
            var composedUrl =
                $"{ContractNotificationsUrl}{scriptHash}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<ContractResult>(data);
            return dto;
        }

        public async Task<TokenResult> GetTokens(int page = 1, string eventType = "", int afterBlock = 0,
            int pageSize = 100)
        {
            var result = await _restClient.GetAsync(TokensUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<TokenResult>(data);
            return dto;
        }

        public async Task<BlockResult> GetBlockNotifications(int blockHeight, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100)
        {
            if (blockHeight < 0) throw new ArgumentOutOfRangeException(nameof(blockHeight));
            var composedUrl =
                $"{BlockNotificationsUrl}{blockHeight}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<BlockResult>(data);
            return dto;
        }

        public async Task<AddressResult> GetAddressNotifications(string address, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
            var composedUrl = eventType == "" ? $"{AddressNotificationsUrl}{address}?Page={page}&AfterBlock={afterBlock}&PageSize={pageSize}" : $"{AddressNotificationsUrl}{address}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";

            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<AddressResult>(data);
            return dto;
        }
    }
}