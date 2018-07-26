<p align="center">
  <img
    src="http://res.cloudinary.com/vidsy/image/upload/v1503160820/CoZ_Icon_DARKBLUE_200x178px_oq0gxm.png"
    width="125px;">
</p>

<h1 align="center">NeoModules</h1>

<p align="center">
  Modular packages for C# devs to use on your <b>NEO</b> blockchain project.
</p>

<p align="center">
  <b>Waiting for peer review. Use on main net at your own risk</b>
</p>


## Libraries (ready for use)

|  Project Source | Nuget Package |  Description |
| ------------- |--------------------------|-----------|
| [NeoModules.RPC](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.RPC)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.10-green.svg)](https://www.nuget.org/packages/NeoModules.RPC/)| RPC Class Library to interact with NEO RPC nodes |
| [NeoModules.JsonRpc.Client](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.JsonRpc.Client) | [![NuGet version](https://img.shields.io/badge/nuget-1.0.2-green.svg)](https://www.nuget.org/packages/NeoModules.JsonRpc.Client/)| Base RPC client definition, used in NeoModules.RPC|
| [NeoModules.Rest](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.Rest)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.7-green.svg)](https://www.nuget.org/packages/NeoModules.Rest/)| Simple Rest client for https://neoscan.io public API |

## Libraries (in dev)
|  Project Source | Nuget Package |  Description |
| ------------- |--------------------------|-----------|
| [NeoModules.Core](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.Core)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.3-yellow.svg)](https://www.nuget.org/packages/NeoModules.Core/)| Core data types and methods used in NeoModules |
| [NeoModules.NVM](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.NVM)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.3-yellow.svg)](https://www.nuget.org/packages/NeoModules.NVM/)| Neo VM with only the necessary functions to support script construction and KeyPair/NEP6 |
| [NeoModules.KeyPairs](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.KeyPairs)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.5-yellow.svg)](https://www.nuget.org/packages/NeoModules.KeyPairs/)| KeyPair project, has the crypto methods needed for KeyPair creation and KeyPair definition |
| [NeoModules.NEP6](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.NEP6)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.16-yellow.svg)](https://www.nuget.org/packages/NeoModules.NEP6/)| NEP6 light wallet implementation |


## RPC client

Develop with decoupling in mind to make maintenance and new RPC methods implemented more quickly:

* Client base and RPC client implementation - NeoModules.JsonRpc.Client project
* DTO'S, Services, Helpers - NeoModules.RPC (main project)
* Tests - NeoModules.RPC.Tests
* Demo - Simple demonstration project

Setup the rpc client node

```C#
var rpcClient = new RpcClient(new Uri("http://seed5.neo.org:10332"));
var NeoApiService = new NeoApiService(rpcClient);
```

With **NeoApiService** you have all the methods available, organized by:
Accounts,
Assets,
Block,
Contract,
Node,
Transaction

Then you just need to choose the wanted service, call ```SendRequestAsync()``` and pass the necessary parameters if needed.
e.g.

```C#
var accountsService = NeoApiService.Accounts;
var state = accountsService.GetAccountState.SendRequestAsync("ADDRESS HERE");
```

If you don't need all the services, you can simply create an instance of the desired service.

```C#
var blockService = new NeoApiBlockService(new RpcClient(new Uri("http://seed5.neo.org:10332")));
var bestBlockHash  = await blockService.GetBestBlockHash.SendRequestAsync();
```

All rpc calls return a DTO or a simple type like string or int.

### NEP 5 service
You can also create a service to query NEP5 tokens. 

```C#
var scriptHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";
var nep5Service = NeoNep5Service(new RpcClient(new Uri("http://seed2.aphelion-neo.com:10332")));
var name = await nep5Service.GetName(scriptHash, true);
var decimals = await nep5Service.GetDecimals(scriptHash);
var totalsupply = await nep5Service.GetTotalSupply(scriptHash, 8);
var symbol = await nep5Service.GetSymbol(scriptHash, true);
var balance = await nep5Service.GetBalance(scriptHash, "0x3640b023405b4b9c818e8387bd01f67bba04dad2", 8);
Debug.WriteLine($"Token info: \nName: {name} \nSymbol: {symbol} \nDecimals: {decimals} \nTotalSupply: {totalsupply} \nBalance: {balance}");
                
Token info: 
Name: Trinity Network Credit 
Symbol: TNC 
Decimals: 8 
TotalSupply: 1000000000 
Balance: 1457.82
```

## Rest services
### Create Rest service (only neoscan available right now)

```C# 
var restService = new NeoScanRestService(NeoScanNet.MainNet);
```
or use your own local NeoScan

```C# 
var restService = new NeoScanRestService("https://url.here/api/main_net[or test_net]/v1/");
```

### Using the API

```C# 
var transaction_json = await restService.GetTransactionAsync("599dec5897d416e9a668e7a34c073832fe69ad01d885577ed841eec52c1c52cf");
```
This returns the json in string format. But you can also transform the result to a defined DTO for easier use:

```C# 
var transaction_json = await restService.GetTransactionAsync("599dec5897d416e9a668e7a34c073832fe69ad01d885577ed841eec52c1c52cf");
var transactionDto = Transaction.FromJson(transaction_json);
```
You can see all the available calls in Demo console project.

## Nodes list
Besides the "get_all_nodes" call on NeoScan API, there also an option to use http://monitor.cityofzion.io/ to get all the nodes with some extra info.

```C# 
var service = new NeoNodesListService();
var result = await service.GetNodesList(MonitorNet.TestNet);
var nodes = JsonConvert.DeserializeObject<NodeList>(result);
```

## NEP6 Compatible Wallet
The wallet creation is of WalletManager.cs responsability. You can use this online or offline, it only depends on the initialization.
The online wallet needs a rest and a rpc client:

```C# 
public WalletManager(INeoRestService restService, IClient client, Wallet wallet = null)
```

### Create an empty wallet and import and account 
You can use wif in string or byte format
```C# 
var walletManager = new WalletManager(new NeoScanRestService(NeoScanNet.MainNet), RpcClient);
var importedAccount = walletManager.ImportAccount("** INSERT WIF HERE **", "Custom account label");
```
Or use NEP6 (this one uses async/await because it can be a heavy operation, especially on mobile hardware)
```C# 
var importedAccount = await walletManager.ImportAccount("** INSERT NEP6 PASSPHRASE HERE **", "** PASSWORD **", "Custom account label");
```

## Transactions
For now, you need to use this check before using the TransactionManager, responsable for the making and signing the transactions. This is needed because NeoModules will use different signing strategies, but for now, only the AccountSignerTransactionManager is available.

### Sending native assets
```C# 
if (importedAccount.TransactionManager is AccountSignerTransactionManager accountSignerTransactionManager)
{
    var sendGasTx = await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "GAS", 323.032m);
    var sendNeoT = await accountSignerTransactionManager.SendAsset("** INSERT TO ADDRESS HERE **", "NEO", 13m) 
}
```
### Making a contract call
```C# 
var scriptHash = "** INSERT CONTRACT SCRIPTHASH **".ToScriptHash().ToArray();
var operation = "balanceOf";
var arguments = new object[] { "arg1" };        

var contractCallTx = await accountSignerTransactionManager.CallContract(scriptHash, operation, arguments);
```

### Estimate contract call gas consumption
```C#
var estimateContractGasCall = await accountSignerTransactionManager.EstimateGasContractCall(scriptHash, operation, arguments);
```

### Making a call contract with attached assets
```C#
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
```

### Claiming Gas
```C#
var callGasTx = await accountSignerTransactionManager.ClaimGas();
```

### Transfer NEP5 tokens
```C#
var transferNepTx = await accountSignerTransactionManager.TransferNep5("** INSERT TO ADDRESS HERE**", 32.3m, scriptHash);
```


## Contributing


## Authors

* **Bruno Freitas** - [BrunoFreitasgit](https://github.com/BrunoFreitasgit)
* [Neo](https://github.com/neo-project/)
* [Neo-lux](https://github.com/CityOfZion/neo-lux)
* Multiple base code is taken/inspired from [Nethereum](https://github.com/Nethereum/Nethereum) project

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/BrunoFreitasgit/Neo-RPC-SharpClient/blob/master/LICENSE) file for details
