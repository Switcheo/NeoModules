using System;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.NEP6;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.Services;
using NeoModules.RPC;
using NeoModules.RPC.DTOs;
using UnityEngine;

public class NeoModulesDemo : MonoBehaviour
{

    readonly string URI = "https://seed1.spotcoin.com:10332";
    private readonly string contractHash = "ed07cffad18f1308db51920d99a2af60ac66a7b3";

    // Use this for initialization
    async void Start()
    {
        try
        {
            var invoke = await InvokeContract();
            Debug.Log(invoke.Stack[0].Value.ToString());
            var contract = await CallContract();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }

    async Task<Invoke> InvokeContract()
    {
        var neoRpcClient = new RpcClient(new Uri(URI));
        var neoRpcService = new NeoApiService(neoRpcClient);

        // Invoke script
        var contractScriptHash = UInt160.Parse(contractHash).ToArray();
        var script = Utils.GenerateScript(contractScriptHash, "getAddressFromMailbox",
            new object[] { "teste" });
        return await neoRpcService.Contracts.InvokeScript.SendRequestAsync(script.ToHexString());
    }

    async Task<SignedTransaction> CallContract()
    {
        // Wallet manager and contract calls
        var neoRpcClient = new RpcClient(new Uri(URI));
        var walletManager = new WalletManager(new NeoScanRestService(NeoScanNet.MainNet), neoRpcClient);
        var contractScriptHash = UInt160.Parse(contractHash).ToArray();
        var importedAccount = walletManager.CreateAccount("unity account");
        var accountSigner = importedAccount.TransactionManager as AccountSignerTransactionManager;
        return await accountSigner.CallContract(contractScriptHash, "registerMailbox",
            new object[] { importedAccount.Address.ToArray(), "unitytest" });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
