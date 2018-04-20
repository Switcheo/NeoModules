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
| [NeoModules.RPC](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.RPC)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.8-green.svg)](https://www.nuget.org/packages/NeoModules.RPC/)| RPC Class Library to interact with NEO RPC nodes |
| [NeoModules.JsonRpc.Client](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.JsonRpc.Client) | [![NuGet version](https://img.shields.io/badge/nuget-1.0.2-green.svg)](https://www.nuget.org/packages/NeoModules.JsonRpc.Client/)| Base RPC client definition, used in NeoModules.RPC|
| [NeoModules.Rest](https://github.com/CityOfZion/NeoModules/tree/master/src/NeoModules.Rest)    | [![NuGet version](https://img.shields.io/badge/nuget-1.0.0-green.svg)](https://www.nuget.org/packages/NeoModules.Rest/)| Simple Rest client for https://neoscan.io public API |

## Libraries (in dev)
|  Project Source | Nuget Package |  Description |
| ------------- |--------------------------|-----------|
| [NeoModules.Core](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.Core)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.1-yellow.svg)](https://www.nuget.org/packages/NeoModules.Core/)| Core data types and methods used in NeoModules |
| [NeoModules.NVM](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.NVM)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.1-yellow.svg)](https://www.nuget.org/packages/NeoModules.NVM/)| Neo VM with only the necessary functions to support script construction and KeyPair/NEP6 |
| [NeoModules.KeyPairs](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.KeyPairs)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.1-yellow.svg)](https://www.nuget.org/packages/NeoModules.KeyPairs/)| KeyPair project, has the crypto methods needed for KeyPair creation and KeyPair definition |
| [NeoModules.NEP6](https://github.com/CityOfZion/NeoModules/tree/feature-core/src/NeoModules.NEP6)    | [![NuGet version](https://img.shields.io/badge/nuget-0.0.2-yellow.svg)](https://www.nuget.org/packages/NeoModules.NEP6/)| NEP6 light wallet implementation |

## Update (25-02-2018)
This project will be rename to **neo-modules** and it will host small different packages to help C# devs to start working on Neo. It follows a similar struture as [Nethereum](https://github.com/Nethereum/Nethereum) to ease the migration of Ethereum devs to Neo. This will include: 
  * base types used on neo-project;
  * hex and string operations;
  * KeyPair generation encryption /decryption, NEP6 implementation
  * Signing of transactions and other more top level operations (to be decided)
  * ABI

Each of these will get is own nuget package.  
  


## RPC client - Intro and Quick Start (Status - finished)

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

You can also create a service to query NEP5 tokens. 
**Note** For the results to be human readable, these methods do not return the original result from the rpc node.

```C#
var client = new RpcClient(new Uri("http://seed5.neo.org:10332"));
var scriptHash = "08e8c4400f1af2c20c28e0018f29535eb85d15b6"; //TNC token
var nep5Service = new NeoNep5Service(client, scriptHash);
var name = await nep5Service.GetName();
var decimals = await nep5Service.GetDecimals();
var totalsupply = await nep5Service.GetTotalSupply(decimals);
var symbol = await nep5Service.GetSymbol();
var balance = await nep5Service.GetBalance("0x0ff9070d64d19076d08947ba4a82b72709f30baf", decimals);

Token info: 
Name: Trinity Network Credit 
Symbol: TNC 
Decimals: 8 
TotalSupply: 1000000000 
Balance: 1457.82
```

## Contributing


## Authors

* **Bruno Freitas** - [BrunoFreitasgit](https://github.com/BrunoFreitasgit)
* Multiple base code is taken/inspired from [Nethereum](https://github.com/Nethereum/Nethereum) project

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/BrunoFreitasgit/Neo-RPC-SharpClient/blob/master/LICENSE) file for details
