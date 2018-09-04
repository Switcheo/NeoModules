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
        private const string TransactionNotificationsUrl = "transaction/";
        private const string TokensUrl = "tokens";

        private readonly HttpClient _restClient;

        public NotificationsService(string url)
        {
            _restClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        public NotificationsService(int node = 1)
        {
            if (node > 5) throw new ArgumentOutOfRangeException(nameof(node));
            var url = NotificationsMainNetUrl.Replace("X", node.ToString());
            _restClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        public async Task<ContractResult> GetContractNotifications(string scriptHash, int page = -1,
            string eventType = "", int afterBlock = -1, int beforeBlock = -1, int pageSize = -1)
        {
            if (string.IsNullOrEmpty(scriptHash)) throw new ArgumentNullException(nameof(scriptHash));

            var request = BuildRequestUrl(ContractNotificationsUrl, scriptHash, page, eventType, afterBlock,
                beforeBlock,
                pageSize);
            var result = await _restClient.GetAsync(request);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<ContractResult>(data);
            return dto;
        }

        public async Task<TokenResult> GetTokens(int page = -1, string eventType = "", int afterBlock = -1,
            int beforeBlock = -1, int pageSize = -1)
        {
            var request = BuildRequestUrl(TokensUrl, null, page, eventType, afterBlock, beforeBlock,
                pageSize);
            var result = await _restClient.GetAsync(request);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<TokenResult>(data);
            return dto;
        }

        public async Task<BlockResult> GetBlockNotifications(int blockHeight, int page = -1, string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1)
        {
            if (blockHeight < 0) throw new ArgumentOutOfRangeException(nameof(blockHeight));

            var request = BuildRequestUrl(BlockNotificationsUrl, blockHeight.ToString(), page, eventType, afterBlock,
                beforeBlock,
                pageSize);
            var result = await _restClient.GetAsync(request);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<BlockResult>(data);
            return dto;
        }

        public async Task<AddressResult> GetAddressNotifications(string address, int page = -1, string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1)
        {
            if (string.IsNullOrEmpty(address)) throw new ArgumentNullException(nameof(address));

            var request = BuildRequestUrl(AddressNotificationsUrl, address, page, eventType, afterBlock, beforeBlock,
                pageSize);
            var result = await _restClient.GetAsync(request);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<AddressResult>(data);
            return dto;
        }

        public async Task<TransactionResult> GetTransactionNotifications(string txHash, int page = -1,
            string eventType = "",
            int afterBlock = -1, int beforeBlock = -1, int pageSize = -1)
        {
            if (string.IsNullOrEmpty(txHash)) throw new ArgumentNullException(nameof(txHash));

            var request = BuildRequestUrl(TransactionNotificationsUrl, txHash, page, eventType, afterBlock, beforeBlock,
                pageSize);
            var result = await _restClient.GetAsync(request);
            var data = await result.Content.ReadAsStringAsync();
            var dto = JsonConvert.DeserializeObject<TransactionResult>(data);
            return dto;
        }

        private string BuildRequestUrl(string url, string arg, int page, string eventType, int afterBlock,
            int beforeBlock, int pageSize)
        {
            var request = string.IsNullOrEmpty(arg) ? $"{url}" : $"{url}{arg}";
            if (page != -1)
            {
                request += $"?Page ={page}";
            }

            if (!string.IsNullOrEmpty(eventType))
            {
                request += $"&EventType={eventType}";
            }

            if (afterBlock != -1)
            {
                request += $"&AfterBlock={afterBlock}";
            }

            if (beforeBlock != -1)
            {
                request += $"&BeforeBlock={beforeBlock}";
            }

            if (pageSize != -1)
            {
                request += $"&PageSize={pageSize}";
            }
            return request;
        }
    }
}