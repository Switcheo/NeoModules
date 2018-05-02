using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.TransactionManagers;
using NeoModules.Core;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase
    {
        private readonly INeoRestService _restService;

        public AccountSignerTransactionManager(IClient rpcClient, INeoRestService restService, Account account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            _restService = restService;
            Client = rpcClient;
        }


        public override Task<bool> SendTransactionAsync(CallInput transactionInput)
        {
           throw new NotImplementedException();
        }

        //// OLD
        //public async Task<TransactionInput> CallContract(KeyPair key, byte[] scriptHash, object[] args)
        //{
        //    var bytes = Utils.GenerateScript(scriptHash, args);
        //    return await CallContract(key, scriptHash, bytes);
        //}

        ////public async Task<TransactionInput> CallContract(KeyPair key, byte[] scriptHash, string operation, object[] args)
        ////{
        ////    return await CallContract(key, scriptHash, new object[] { operation, args });
        ////}

        //// OLD FINISH

        //TODO: move this to separate class/service because of SRP
        public async Task<TransactionInput> CallContract(KeyPair key, byte[] scriptHash, string operation, object[] args = null)
        {
            var scriptBuilderResult = Utils.GenerateScript(scriptHash, operation, args);
            return await CallContract(key, scriptHash, scriptBuilderResult);
        }

        public async Task<TransactionInput> CallContract(KeyPair key, byte[] scriptHash, byte[] bytes)
        {
            var gasCost = 0;

            GenerateInputsOutputs(key, scriptHash, new Dictionary<string, decimal> { { "GAS", gasCost } }, out var inputs,
                out var outputs);

            var tx = new TransactionInput
            {
                Type = 0xd1,
                Version = 0,
                Script = bytes,
                Gas = gasCost,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(key, tx);
            return result ? tx : null;
        }

        public async Task<TransactionInput> SendAsset(KeyPair fromKey, string toAddress, string symbol, decimal amount)
        {
            return await SendAsset(fromKey, toAddress, new Dictionary<string, decimal>() { { symbol, amount } });
        }

        public async Task<TransactionInput> SendAsset(KeyPair fromKey, string toAddress, Dictionary<string, decimal> amounts)
        {
            if (String.Equals(fromKey.PublicKeyHash.ToString(), toAddress, StringComparison.OrdinalIgnoreCase))
            {
                throw new WalletException("Source and dest addresses are the same");
            }

            var toScriptHash = toAddress.ToScriptHash().ToArray();
            return await SendAsset(fromKey, toScriptHash, amounts);
        }

        public async Task<TransactionInput> SendAsset(KeyPair fromKey, byte[] scriptHash,
            Dictionary<string, decimal> amounts)
        {
            GenerateInputsOutputs(fromKey, scriptHash, amounts, out var inputs, out var outputs);

            var tx = new TransactionInput
            {
                Type = 128,
                Version = 0,
                Script = null,
                Gas = -1,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(fromKey, tx);
            return result ? tx : null;
        }

        private async Task<bool> SignAndSendTransaction(KeyPair key, TransactionInput txInput)
        {
            if (txInput == null) return false;
            txInput.Sign(key);
            var serializedTx = txInput.Serialize();
            return await SendTransactionAsync(serializedTx);
        }

        private void GenerateInputsOutputs(KeyPair key, byte[] outputHash, Dictionary<string, decimal> ammounts,
            out List<TransactionInput.Input> inputs, out List<TransactionInput.Output> outputs)
        {
            if (ammounts == null || ammounts.Count == 0) throw new WalletException("Invalid amounts");

            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = GetUnspent(Wallet.ToAddress(address.ToScriptHash())).Result; // todo remove result

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            inputs = new List<TransactionInput.Input>();
            outputs = new List<TransactionInput.Output>();

            foreach (var entry in ammounts)
            {
                var symbol = entry.Key;
                if (!unspent.ContainsKey(symbol))
                    throw new WalletException($"Not enough {symbol} in address {Wallet.ToAddress(key.PublicKeyHash)}");

                var cost = entry.Value;

                string assetId;

                var info = Utils.GetAssetsInfo();
                if (info.ContainsKey(symbol))
                    assetId = info[symbol];
                else
                    throw new WalletException($"{symbol} is not a valid blockchain asset.");

                var sources = unspent[symbol];

                decimal selected = 0;
                foreach (var src in sources)
                {
                    selected += src.Value;

                    var input = new TransactionInput.Input
                    {
                        PrevHash = src.TxId,
                        PrevIndex = src.N
                    };

                    inputs.Add(input);

                    if (selected >= cost) break;
                }

                if (selected < cost) throw new ArgumentOutOfRangeException($"Not enough {symbol}");

                if (cost > 0)
                {
                    var output = new TransactionInput.Output
                    {
                        AssetId = assetId,
                        ScriptHash = outputHash.Reverse().ToHexString(),
                        Value = cost
                    };
                    outputs.Add(output);
                }

                if (selected > cost || cost == 0)
                {
                    var left = selected - cost;

                    var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
                    var change = new TransactionInput.Output
                    {
                        AssetId = assetId,
                        ScriptHash = signatureScript.ToArray().Reverse().ToHexString(),
                        Value = left
                    };
                    outputs.Add(change);
                }
            }
        }

        public async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address)
        {
            if (_restService == null) throw new NullReferenceException("REST client not configured");
            var response = await _restService.GetBalanceAsync(address);
            var addressBalance = AddressBalance.FromJson(response);

            var result = new Dictionary<string, List<Unspent>>();
            foreach (var balanceEntry in addressBalance.Balance)
            {
                var child = balanceEntry.Unspent;
                if (!(child?.Count > 0)) continue;
                List<Unspent> list;
                if (result.ContainsKey(balanceEntry.Asset))
                {
                    list = result[balanceEntry.Asset];
                }
                else
                {
                    list = new List<Unspent>();
                    result[balanceEntry.Asset] = list;
                }

                list.AddRange(balanceEntry.Unspent.Select(data => new Unspent
                {
                    TxId = data.TxId,
                    N = data.N,
                    Value = data.Value
                }));
            }

            return result;
        }
    }
}
