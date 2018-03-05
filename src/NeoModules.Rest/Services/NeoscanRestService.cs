using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NeoModules.Rest.Services
{
	public enum NeoscanNet
	{
		MainNet,
		TestNet
	}

	public class NeoscanRestService : INeoRestService
	{
		private static readonly string neoScanTestNetUrl = "https://neoscan-testnet.io/api/test_net/v1/";
		private static readonly string neoScanMainNetUrl = "https://neoscan.io/api/test_net/v1/";
		private static readonly string getBalanceUrl = "get_balance/";
		private static readonly string getClaimedUrl = "get_claimed/";
		private static readonly string getClaimableUrl = "get_claimable/";
		private static readonly string getUnclaimedUrl = "get_unclaimed/";
		private static readonly string getAddressUrl = "get_address/";

		private readonly HttpClient _restClient;

		public NeoscanRestService(NeoscanNet net)
		{
			if (net == NeoscanNet.MainNet)
			{
				_restClient = new HttpClient { BaseAddress = new Uri(neoScanMainNetUrl) };
			}
			else
			{
				_restClient = new HttpClient { BaseAddress = new Uri(neoScanTestNetUrl) };
			}
		}


		// TODO: I can refractor this more
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

		private string ComposeAddressUrl(string url, string address)
		{
			return string.Format("{0}{1}", url, address);
		}
	}
}