<p align="center">
  <img
    src="http://res.cloudinary.com/vidsy/image/upload/v1503160820/CoZ_Icon_DARKBLUE_200x178px_oq0gxm.png"
    width="125px;">
</p>

<h1 align="center">Neo-RPC-SharpClient</h1>

<p align="center">
  Modular RPC client for C# <b>NEO</b> blockchain.
</p>

<p align="center">
  <b>Still on testing phase, do not use it on the MainNet!</b>
</p>
<p align="center">
<a href="https://www.nuget.org/packages/NeoModules.RPC/1.0.5">
    <img src="https://img.shields.io/badge/nuget-1.0.5-green.svg"></a>
</p>


## Update (11-03-2018)
Added a new REST client package to fill some missing properties needed to create transactions. This client uses the https://neoscan.io/ public API. You can see a simple usage on the Demo project.
It's also available on nuget [now](https://www.nuget.org/packages/NeoModules.Rest/).


## Update (05-03-2018)
Because of the project name changes, there is a new nuget package. To use you need to install [this](https://www.nuget.org/packages/NeoModules.RPC) for the main implementation. If during the package installation it does not install the NeoModules.JsonRpc.Client, [add it to your project manually](https://www.nuget.org/packages/NeoModules.JsonRpc.Client/).


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

* Client base - Neo.JsonRpc.Client project
* RPC client implementation - Neo.JsonRpc.RpcClient project
* DTO'S, Services, Helpers - Neo.RPC (main project)
* Tests - Neo.RPC.Tests
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
* Some base code comes from [Nethereum](https://github.com/Nethereum/Nethereum)

## License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/BrunoFreitasgit/Neo-RPC-SharpClient/blob/master/LICENSE) file for details
