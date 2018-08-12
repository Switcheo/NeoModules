using System;
using System.Net.Http;
using System.Threading.Tasks;
using NeoModules.Rest.DTOs.NeoNotifications;
using Newtonsoft.Json;

namespace NeoModules.Rest.Services
{
    public class NotificationsService : INotificationsService
    {
        private static readonly string notificationsMainNetUrl = "http://notifications.neeeo.org/v1/";
        private static readonly string addressNotificationsUrl = "notifications/addr/";
        private static readonly string blockNotificationsUrl = "notifications/block/";
        private static readonly string contractNotificationsUrl = "notifications/contract/";
        private static readonly string tokensUrl = "tokens";

        private readonly HttpClient _restClient;
        
        public NotificationsService()
        {
            _restClient = new HttpClient {BaseAddress = new Uri(notificationsMainNetUrl)};
        }

        public async Task<ContractResult> GetContractNotifications(string scriptHash, int page = 1,
            string eventType = "", int afterBlock = 0, int pageSize = 100)
        {
            if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));
            var composedUrl =
                $"{contractNotificationsUrl}{scriptHash}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<ContractResult>(data);
            return dto;
        }

        public async Task<TokenResult> GetTokens(int page = 1, string eventType = "", int afterBlock = 0,
            int pageSize = 100)
        {
            var result = await _restClient.GetAsync(tokensUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<TokenResult>(data);
            return dto;
        }

        public async Task<BlockResult> GetBlockNotifications(int blockHeight, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100)
        {
            if (blockHeight < 0) throw new ArgumentOutOfRangeException(nameof(blockHeight));
            var composedUrl =
                $"{blockNotificationsUrl}{blockHeight}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<BlockResult>(data);
            return dto;
        }

        public async Task<AddressResult> GetAddressNotifications(string address, int page = 1, string eventType = "",
            int afterBlock = 0, int pageSize = 100)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));
            var composedUrl = eventType == "" ? $"{addressNotificationsUrl}{address}?Page={page}&AfterBlock={afterBlock}&PageSize={pageSize}" : $"{addressNotificationsUrl}{address}?Page={page}&EventType={eventType}&AfterBlock={afterBlock}&PageSize={pageSize}";
          
            var result = await _restClient.GetAsync(composedUrl);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<AddressResult>(data);
            return dto;
        }
    }
}