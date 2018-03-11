using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.Rest.Services;
using NeoModules.Rest.Models;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Services;
using NeoModules.RPC;

namespace NeoModules.Demo
{
	public class Program
	{
		private static readonly RpcClient _rpcClient = new RpcClient(new Uri("http://seed5.neo.org:10332"));

		public static void Main(string[] args)
		{
			try
			{
				var neoApiCompleteService = SetupCompleteNeoService();

				var neoApiSimpleContractService = SetupSimpleService();
				var neoApiSimpleAccountService = SetupAnotherSimpleService();
				// You can also create a custom service with only the stuff that you need by creating a class that implements (":") RpcClientWrapper like: public class CustomService : RpcClientWrapper

				var nep5ApiService = SetupNep5Service();

				BlockApiTest(neoApiCompleteService).Wait();

				TestNep5Service(nep5ApiService).Wait();


				// create rest api client
				RestClientTest().Wait();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"There was an exception: {ex.Message}");
			}
		}

		// Returns a NeoApiService that has all the api calls (except the nep5)
		private static NeoApiService SetupCompleteNeoService()
		{
			return new NeoApiService(_rpcClient);
		}

		// Returns an instance of ContractService with only the api calls that concern contracts
		private static NeoApiContractService SetupSimpleService()
		{
			return new NeoApiContractService(_rpcClient);
		}

		// Another example of a simple Service - Account api calls
		private static NeoApiAccountService SetupAnotherSimpleService()
		{
			return new NeoApiAccountService(_rpcClient);
		}

		// Returns an instance of NeoNep5Service with the api calls that concern nep5 tokens.
		private static NeoNep5Service SetupNep5Service()
		{
			return new NeoNep5Service(_rpcClient, "08e8c4400f1af2c20c28e0018f29535eb85d15b6"); //TNC token script hash
		}

		// Block api demonstration
		private static async Task BlockApiTest(NeoApiService service)
		{
			Block genesisBlock = await service.Blocks.GetBlock.SendRequestAsync(0); // Get genesis block by index (can pass a string with block hash as parameter too)
			string bestBlockHash = await service.Blocks.GetBestBlockHash.SendRequestAsync();
			int blockCount = await service.Blocks.GetBlockCount.SendRequestAsync();
			string blockHash = await service.Blocks.GetBlockHash.SendRequestAsync(0);
			string serializedBlock = await service.Blocks.GetBlockSerialized.SendRequestAsync(0); // (can pass a string with block hash as parameter too)
			string blockFee = await service.Blocks.GetBlockSysFee.SendRequestAsync(0);
		}

		// Nep5 api demonstration
		private static async Task TestNep5Service(NeoNep5Service nep5Service)
		{
			var name = await nep5Service.GetName();
			var decimals = await nep5Service.GetDecimals();
			var totalsupply = await nep5Service.GetTotalSupply(decimals);
			var symbol = await nep5Service.GetSymbol();
			var balance = await nep5Service.GetBalance("0x0ff9070d64d19076d08947ba4a82b72709f30baf", decimals);

			Debug.WriteLine($"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
		}


		private static async Task RestClientTest()
		{
			var testAddress = "ANrL4vPnQCCi5Mro4fqKK1rxrkxEHqmp2E";

			var restService = new NeoScanRestService(NeoScanNet.MainNet); // service creation

			// api calls
			var getBalance = await restService.GetBalanceAsync(testAddress);
			var getClaimed = await restService.GetClaimedAsync(testAddress); // returns internal server error
			var getClaimable = await restService.GetClaimableAsync(testAddress);
			var getUnclaimed = await restService.GetUnclaimedAsync(testAddress);
			var getAddress = await restService.GetAddressAsync(testAddress);

			//Deserialization

			var balance_model = AddressBalance.FromJson(getBalance);
			var claimed_model = Claimed.FromJson(getClaimed);
			var claimable_model = Claimable.FromJson(getClaimable);
			var unclaimed_model = Unclaimed.FromJson(getUnclaimed);
			var address_model = AddressHistory.FromJson(getAddress);

		}
	}
}
