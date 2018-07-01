using System;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.NEP6;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using UnityEngine;

public class NeoModulesDemo : MonoBehaviour
{

    // Use this for initialization
    async void Start()
    {
        // Create rpc client and service
        var neoRpcClient = new RpcClient(new Uri("http://seed1.cityofzion.io:8080"));
        var neoRpcService = new NeoApiService(neoRpcClient);

        // Invoke script
        var contractScriptHash = UInt160.Parse("ed07cffad18f1308db51920d99a2af60ac66a7b3").ToArray();
        var script = NeoModules.NEP6.Utils.GenerateScript(contractScriptHash, "getAddressFromMailbox",
            new object[] { "teste" });
        var response = await neoRpcService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());


        // Wallet manager and contract calls
        var walletManager = new WalletManager(new NeoScanRestService(NeoScanNet.MainNet), neoRpcClient);

        var importedAccount = walletManager.CreateAccount("unity account");
        var accountSigner = importedAccount.TransactionManager as AccountSignerTransactionManager;
        var response2 = await accountSigner.CallContract(contractScriptHash, "registerMailbox",
            new object[] { importedAccount.Address.ToArray(), "unitytest" });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
