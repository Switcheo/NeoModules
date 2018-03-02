using System;
using System.Net.Http;
using System.Threading.Tasks;
using Neo.RestClient.Interfaces;
using Newtonsoft.Json.Linq;

namespace Neo.RestClient.Services
{
    public class NeoRestService : INeoRestService
    {
        public async Task<string> GetBalanceAsync(string address)
        {
            //todo
            var url = "https://neoscan-testnet.io/api/test_net/v1/get_balance";
            var _restClient =  new HttpClient();
            _restClient.BaseAddress = new Uri(url + "/" + address);
            var result = await _restClient.GetAsync(_restClient.BaseAddress);
            var data = await result.Content.ReadAsStringAsync();
            return data;
        }
    }
}
