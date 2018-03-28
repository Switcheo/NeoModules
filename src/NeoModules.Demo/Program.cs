using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.NVM;
using NeoModules.Rest.Models;
using NeoModules.Rest.Services;
using NeoModules.RPC.Services;
using NeoModules.RPC.Services.Contract;
using NeoModules.RPC;

namespace NeoModules.Demo
{
    public class Program
    {
        private static readonly RpcClient RpcClient = new RpcClient(new Uri("http://seed1.cityofzion.io:8080"));

        public static void Main(string[] args)
        {
            try
            {
                CreateScriptTest().Wait();
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
            return new NeoApiService(RpcClient);
        }

        // Returns an instance of ContractService with only the api calls that concern contracts
        private static NeoApiContractService SetupSimpleService()
        {
            return new NeoApiContractService(RpcClient);
        }

        // Another example of a simple Service - Account api calls
        private static NeoApiAccountService SetupAnotherSimpleService()
        {
            return new NeoApiAccountService(RpcClient);
        }

        // Returns an instance of NeoNep5Service with the api calls that concern nep5 tokens.
        private static NeoNep5Service SetupNep5Service()
        {
            return new NeoNep5Service(RpcClient, "08e8c4400f1af2c20c28e0018f29535eb85d15b6"); //TNC token script hash
        }

        // Block api demonstration
        private static async Task BlockApiTest(NeoApiService service)
        {
            var genesisBlock =
                await service.Blocks.GetBlock
                    .SendRequestAsync(
                        0); // Get genesis block by index (can pass a string with block hash as parameter too)
            var bestBlockHash = await service.Blocks.GetBestBlockHash.SendRequestAsync();
            var blockCount = await service.Blocks.GetBlockCount.SendRequestAsync();
            var blockHash = await service.Blocks.GetBlockHash.SendRequestAsync(0);
            var serializedBlock =
                await service.Blocks.GetBlockSerialized
                    .SendRequestAsync(0); // (can pass a string with block hash as parameter too)
            var blockFee = await service.Blocks.GetBlockSysFee.SendRequestAsync(0);
        }

        // Nep5 api demonstration
        private static async Task TestNep5Service(NeoNep5Service nep5Service)
        {
            var name = await nep5Service.GetName();
            var decimals = await nep5Service.GetDecimals();
            var totalsupply = await nep5Service.GetTotalSupply(decimals);
            var symbol = await nep5Service.GetSymbol();
            var balance = await nep5Service.GetBalance("0x0ff9070d64d19076d08947ba4a82b72709f30baf", decimals);

            Debug.WriteLine(
                $"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
        }

        // Rest demo
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

            var balanceModel = AddressBalance.FromJson(getBalance);
            var claimedModel = Claimed.FromJson(getClaimed);
            var claimableModel = Claimable.FromJson(getClaimable);
            var unclaimedModel = Unclaimed.FromJson(getUnclaimed);
            var addressModel = AddressHistory.FromJson(getAddress);
        }

        // ScriptBuilder test, nep5 
        private static async Task CreateScriptTest()
        {
            var scripthashString = "08e8c4400f1af2c20c28e0018f29535eb85d15b6";
            UInt160 scriptHash = UInt160.Parse(scripthashString);
            byte[] script;
            using (var sb = new ScriptBuilder())
            {
                sb.EmitAppCall(scriptHash, "name");
                sb.EmitAppCall(scriptHash, "decimals");
                sb.EmitAppCall(scriptHash, "symbol");
                sb.EmitAppCall(scriptHash, "totalSupply");
                script = sb.ToArray();
            }
            Debug.WriteLine(script.ToHexString());
            var x = new NeoInvokeScript(RpcClient);
            var test = await x.SendRequestAsync(script.ToHexString());
            Debug.WriteLine(test.Stack[0].Value);
        }
    }
}