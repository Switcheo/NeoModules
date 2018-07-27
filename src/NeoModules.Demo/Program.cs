using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.DTOs.NeoNotifications;
using NeoModules.Rest.DTOs.NeoScan;
using NeoModules.Rest.Services;
using NeoModules.RPC.Services;
using NeoModules.RPC;
using Newtonsoft.Json;
using Asset = NeoModules.Rest.DTOs.NeoScan.Asset;
using Block = NeoModules.Rest.DTOs.NeoScan.Block;
using Node = NeoModules.Rest.DTOs.NeoScan.Node;
using Transaction = NeoModules.Rest.DTOs.NeoScan.Transaction;
using TransactionOutput = NeoModules.NEP6.Transactions.TransactionOutput;

namespace NeoModules.Demo
{
    public class Program
    {
        private static readonly RpcClient RpcClient = new RpcClient(new Uri("http://seed2.aphelion-neo.com:10332"));
        private static readonly RpcClient RpcTestNetClient = new RpcClient(new Uri("http://test5.cityofzion.io:8880"));

        public static void Main(string[] args)
        {
            try
            {
                var neoApiCompleteService = SetupCompleteNeoService();

                var neoApiSimpleContractService = SetupSimpleService();
                var neoApiSimpleAccountService = SetupAnotherSimpleService();
                //You can also create a custom service with only the stuff that you need by creating a class that implements(":") RpcClientWrapper like: public class CustomService : RpcClientWrapper

                var nep5ApiService = SetupNep5Service();

                BlockApiTest(neoApiCompleteService).Wait();

                TestNep5Service(nep5ApiService).Wait();


                //create rest api client
                RestClientTest().Wait();

                //nodes list
                NodesListTestAsync().Wait();

                //http://notifications.neeeo.org/ service
                NotificationsService().Wait();
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
            return new NeoNep5Service(RpcClient); //TNC token script hash
        }

        // Block api demonstration
        private static async Task BlockApiTest(NeoApiService service)
        {
            RPC.DTOs.Block
                genesisBlock =
                    await service.Blocks.GetBlock
                        .SendRequestAsync(
                            0); // Get genesis block by index (can pass a string with block hash as parameter too)
            string bestBlockHash = await service.Blocks.GetBestBlockHash.SendRequestAsync();
            int blockCount = await service.Blocks.GetBlockCount.SendRequestAsync();
            string blockHash = await service.Blocks.GetBlockHash.SendRequestAsync(0);
            string serializedBlock =
                await service.Blocks.GetBlockSerialized
                    .SendRequestAsync(0); // (can pass a string with block hash as parameter too)
            string blockFee = await service.Blocks.GetBlockSysFee.SendRequestAsync(0);
        }

        // Nep5 api demonstration
        private static async Task TestNep5Service(NeoNep5Service nep5Service)
        {
            var name = await nep5Service.GetName("ed07cffad18f1308db51920d99a2af60ac66a7b3", true);
            var decimals = await nep5Service.GetDecimals("ed07cffad18f1308db51920d99a2af60ac66a7b3");
            var totalsupply = await nep5Service.GetTotalSupply("ed07cffad18f1308db51920d99a2af60ac66a7b3", 8);
            var symbol = await nep5Service.GetSymbol("ed07cffad18f1308db51920d99a2af60ac66a7b3", true);
            var balance = await nep5Service.GetBalance("ed07cffad18f1308db51920d99a2af60ac66a7b3",
                "0x3640b023405b4b9c818e8387bd01f67bba04dad2", 8);

            Debug.WriteLine(
                $"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
        }


        private static async Task RestClientTest()
        {
            var testAddress = "ANrL4vPnQCCi5Mro4fqKK1rxrkxEHqmp2E";

            var restService = new NeoScanRestService(NeoScanNet.MainNet); // service creation

            // api calls
            var getBalance = await restService.GetBalanceAsync(testAddress);
            var getClaimed = await restService.GetClaimedAsync(testAddress);
            var getClaimable = await restService.GetClaimableAsync(testAddress);
            var getUnclaimed = await restService.GetUnclaimedAsync(testAddress);
            var getAddress = await restService.GetAddressAsync(testAddress);
            var nodes = await restService.GetAllNodesAsync();
            var transaction =
                await restService.GetTransactionAsync(
                    "599dec5897d416e9a668e7a34c073832fe69ad01d885577ed841eec52c1c52cf");
            var assets = await restService.GetAssetsAsync();
            var asset = await restService.GetAssetAsync(
                "089cd37714d43511e304dc559e05a5a965274685dc21686bdcd05a45e17aab7a");
            var height = await restService.GetHeight();
            var highestBlock = await restService.GetHighestBlock();
            var lastBlocks = await restService.GetLastBlocks();
            var feesInRange = await restService.GetFeesInRange(4, 6);
            var abstractAddress = await restService.GetAddressAbstracts("AGbj6WKPUWHze12zRyEL5sx8nGPVN6NXUn", 0);
            var neonAddress = await restService.GetNeonAddress("AGbj6WKPUWHze12zRyEL5sx8nGPVN6NXUn");
            var addressToAddressAbstract = await restService.GetAddressToAddressAbstract(
                "AJ5UVvBoz3Nc371Zq11UV6C2maMtRBvTJK",
                "AZCcft1uYtmZXxzHPr5tY7L6M85zG7Dsrv", 0);
            var block = await restService.GetBlock("54ffd56d6a052567c5d9abae43cc0504ccb8c1efe817c2843d154590f0b572f7");
            var lastTransactions = await restService.GetLastTransactions();
            var lastTransactionsByAddress =
                await restService.GetLastTransactionsByAddress("AGbj6WKPUWHze12zRyEL5sx8nGPVN6NXUn", 0);

            //Deserialization
            var balanceDto = AddressBalance.FromJson(getBalance);
            var claimedDto = Claimed.FromJson(getClaimed);
            var claimableDto = Claimable.FromJson(getClaimable);
            var unclaimedDto = Unclaimed.FromJson(getUnclaimed);
            var addressDto = AddressHistory.FromJson(getAddress);
            var nodesDto = Node.FromJson(nodes);
            var transactionDto = Transaction.FromJson(transaction);
            var assetsDto = Asset.FromJsonList(assets);
            var assetDto = Asset.FromJson(asset);
            long chainHeight = Convert.ToInt64(height);
            var highestBlockDto = Block.FromJson(highestBlock);
            var lastBlocksDto = Blocks.FromJson(lastBlocks);
            var feesInRangeDto = FeesInRange.FromJson(feesInRange);
            var abstractAddressDto = AbstractAddress.FromJson(abstractAddress);
            var neonAddressDto = NeonAddress.FromJson(neonAddress);
            var addressToAddressAbstractDto = AbstractAddress.FromJson(addressToAddressAbstract);
            var blockDto = Block.FromJson(block);
            var lastTransactionsDto = Transactions.FromJson(lastTransactions);
            var lastTransactionsByAddressDto = Transactions.FromJson(lastTransactionsByAddress);
        }

        // Test getting the nodes list registered on http://monitor.cityofzion.io
        private static async Task<NodeList> NodesListTestAsync()
        {
            var service = new NeoNodesListService();
            var result = await service.GetNodesList(MonitorNet.TestNet);
            var nodes = JsonConvert.DeserializeObject<NodeList>(result);
            return nodes;
        }

        private static async void WalletAndTransactionsTest()
        {
            // Create online wallet and import account
            var walletManager = new WalletManager(new NeoScanRestService(NeoScanNet.MainNet), RpcClient);
            var importedAccount = walletManager.ImportAccount("** INSERT WIF HERE **", "Test");

            // Get account signer for transactions
            if (importedAccount.TransactionManager is AccountSignerTransactionManager accountSignerTransactionManager)
            {
                // Send native assets
                var sendGasTx =
                    await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "GAS", 323.032m);
                var sendNeoTx =
                    await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "NEO", 13m);

                // Call contract
                var scriptHash = "** INSERT CONTRACT SCRIPTHASH **".ToScriptHash().ToArray();
                var operation = "balanceOf";
                var arguments = new object[] { "arg1" };

                var contractCallTx =
                    await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments);

                // Estimate Gas consumed from contract call
                var estimateContractGasCall =
                    await accountSignerTransactionManager.EstimateGasContractCall(scriptHash, operation, arguments);

                // Call contract with attached assets
                var assetToAttach = "GAS";
                var output = new List<TransactionOutput>()
                {
                    new TransactionOutput()
                    {
                        AddressHash = "** INSERT TO ADDRESS HERE**".ToScriptHash().ToArray(),
                        Amount = 2,
                    }
                };
                var contractCallWithAttachedTx =
                    await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments, assetToAttach,
                        output);

                // Claim gas
                var callGasTx = await accountSignerTransactionManager.ClaimGas();

                // Transfer NEP5 tokens
                var transferNepTx =
                    await accountSignerTransactionManager.TransferNep5("** INSERT TO ADDRESS HERE**", 32.3m,
                        scriptHash);
            }
        }
        
        private static async Task NotificationsService()
        {
            var notificationService = new NotificationsService();
            var addressNotifications = await notificationService.GetAddressNotifications("AGfGWQeM6md6RtEsTUivQNXhp8p4ytkDMR", 1, "", 13, 200);
            var blockNotifications = await notificationService.GetBlockNotifications(10);
            var contractNotifications =
                await notificationService.GetContractNotifications("0x67a5086bac196b67d5fd20745b0dc9db4d2930ed");
            var tokenList = await notificationService.GetTokens();
        }

    }
}