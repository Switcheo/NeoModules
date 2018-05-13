using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.JsonRpc.Client;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using NeoModules.RPC.TransactionManagers;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6
{
    public class AccountSignerTransactionManager : TransactionManagerBase
    {
        private static Dictionary<string, string> _systemAssets;
        private readonly INeoRestService _restService;

        public AccountSignerTransactionManager(IClient rpcClient, INeoRestService restService, Account account)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            _restService = restService;
            Client = rpcClient;
        }

        public async Task<SignerTransaction> CallContract(KeyPair key, byte[] scriptHash, string operation,
            object[] args, string attachSymbol = null, IEnumerable<TransactionOutput> attachTargets = null)
        {
            var bytes = Utils.GenerateScript(scriptHash, operation, args);
            return await CallContract(key, scriptHash, bytes, attachSymbol, attachTargets);
        }

        public async Task<SignerTransaction> CallContract(KeyPair key, byte[] scriptHash, byte[] bytes,
            string attachSymbol = null, IEnumerable<TransactionOutput> attachTargets = null)
        {
            if (string.IsNullOrEmpty(attachSymbol)) attachSymbol = "GAS";

            if (attachTargets == null) attachTargets = new List<TransactionOutput>();

            var (inputs, outputs) = await GenerateInputsOutputs(key, attachSymbol, attachTargets);

            if (inputs.Count == 0) throw new WalletException($"Not enough inputs for transaction");

            var tx = new SignerTransaction
            {
                Type = TransactionType.InvocationTransaction,
                Version = 0,
                Script = bytes,
                Gas = 0,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(key, tx);
            return result ? tx : null;
        }

        public async Task<SignerTransaction> SendAsset(KeyPair fromKey, string toAddress, string symbol, decimal amount)
        {
            var toScriptHash = toAddress.GetScriptHashFromAddress();
            var target = new TransactionOutput { AddressHash = toScriptHash, Amount = amount };
            var targets = new List<TransactionOutput> { target };
            return await SendAsset(fromKey, symbol, targets);
        }

        public async Task<SignerTransaction> SendAsset(KeyPair fromKey, string symbol,
            IEnumerable<TransactionOutput> targets)
        {
            var (inputs, outputs) = await GenerateInputsOutputs(fromKey, symbol, targets);

            var tx = new SignerTransaction
            {
                Type = TransactionType.ContractTransaction,
                Version = 0,
                Script = null,
                Gas = -1,
                Inputs = inputs.ToArray(),
                Outputs = outputs.ToArray()
            };

            var result = await SignAndSendTransaction(fromKey, tx);
            return result ? tx : null;
        }

        private async Task<bool> SignAndSendTransaction(KeyPair key, SignerTransaction txInput)
        {
            if (txInput == null) return false;
            txInput.Sign(key);
            var serializedTx = txInput.Serialize();
            return await SendTransactionAsync(serializedTx.ToHexString());
        }

        private async Task<(List<SignerTransaction.Input> inputs, List<SignerTransaction.Output> outputs)>
            GenerateInputsOutputs(KeyPair key, string symbol, IEnumerable<TransactionOutput> targets)
        {
            if (targets == null) throw new WalletException("Invalid amount list");

            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = await GetUnspent(Wallet.ToAddress(address.ToScriptHash()));

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            var inputs = new List<SignerTransaction.Input>();
            var outputs = new List<SignerTransaction.Output>();

            string assetId;

            var info = GetAssetsInfo();
            if (info.ContainsKey(symbol))
                assetId = info[symbol];
            else
                throw new WalletException($"{symbol} is not a valid blockchain asset.");

            if (!unspent.ContainsKey(symbol)) throw new WalletException($"Not enough {symbol} in address {address}");

            decimal cost = 0;

            var fromHash = key.PublicKeyHash.ToArray();
            foreach (var target in targets)
            {
                if (target.AddressHash.SequenceEqual(fromHash))
                    throw new WalletException("Target can't be same as input");

                cost += target.Amount;
            }

            var sources = unspent[symbol];
            decimal selected = 0;

            foreach (var src in sources)
            {
                selected += src.Value;

                var input = new SignerTransaction.Input
                {
                    PrevHash = src.TxId.HexToBytes().Reverse().ToArray(),
                    PrevIndex = src.N
                };

                inputs.Add(input);

                if (selected >= cost) break;
            }

            if (selected < cost) throw new WalletException($"Not enough {symbol}");


            if (cost > 0)
                foreach (var target in targets)
                {
                    var output = new SignerTransaction.Output
                    {

                        AssetId = assetId.HexToBytes().Reverse().ToArray(),
                        ScriptHash = target.AddressHash.ToArray(),
                        Value = target.Amount
                    };
                    outputs.Add(output);
                }


            if (selected > cost || cost == 0)
            {
                var left = selected - cost;
                var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
                var change = new SignerTransaction.Output
                {
                    AssetId = assetId.HexToBytes().Reverse().ToArray(),
                    ScriptHash = signatureScript.ToArray(),
                    Value = left
                };
                outputs.Add(change);
            }

            return (inputs, outputs);
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

        internal static Dictionary<string, string> GetAssetsInfo()
        {
            if (_systemAssets == null)
            {
                _systemAssets = new Dictionary<string, string>();
                AddAsset("NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
                AddAsset("GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");
            }

            return _systemAssets;
        }

        private static void AddAsset(string symbol, string hash)
        {
            _systemAssets[symbol] = hash;
        }

        public static string SymbolFromAssetID(string assetID)
        {
            if (assetID == null) return null;

            if (assetID.StartsWith("0x")) assetID = assetID.Substring(2);

            var info = GetAssetsInfo();
            foreach (var entry in info)
                if (entry.Value == assetID)
                    return entry.Key;

            return null;
        }
    }
}