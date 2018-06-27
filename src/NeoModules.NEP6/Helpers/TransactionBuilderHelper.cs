using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.DTOs;
using NeoModules.Rest.Services;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6.Helpers
{
    public static class TransactionBuilderHelper
    {
        private static Dictionary<string, string> _systemAssets;

        public static async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address,
            INeoRestService restService)
        {
            if (restService == null) throw new NullReferenceException("REST client not configured");
            var response = await restService.GetBalanceAsync(address);
            var addressBalance = AddressBalance.FromJson(response);

            var result = new Dictionary<string, List<Unspent>>();
            if (addressBalance.Balance != null)
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


        public static async Task<(List<SignedTransaction.Input> inputs, List<SignedTransaction.Output> outputs)>
            GenerateInputsOutputs(KeyPair key, string symbol, IEnumerable<TransactionOutput> targets,
                INeoRestService restService)
        {
            var address = Helper.CreateSignatureRedeemScript(key.PublicKey);
            var unspent = await GetUnspent(Wallet.ToAddress(address.ToScriptHash()), restService);

            // filter any asset lists with zero unspent inputs
            unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

            var inputs = new List<SignedTransaction.Input>();
            var outputs = new List<SignedTransaction.Output>();

            string assetId;

            var info = GetAssetsInfo();
            if (info.ContainsKey(symbol))
                assetId = info[symbol];
            else
                throw new WalletException($"{symbol} is not a valid blockchain asset.");

            decimal cost = 0;

            var fromHash = key.PublicKeyHash.ToArray();
            List<TransactionOutput> transactionOutputs = null;
            if (targets != null)
            {
                transactionOutputs = targets.ToList();
                foreach (var target in transactionOutputs)
                {
                    if (target.AddressHash.SequenceEqual(fromHash))
                        throw new WalletException("Target can't be same as input");

                    cost += target.Amount;
                }
            }

            if (unspent.Count > 0)
            {
                var sources = unspent[symbol];
                decimal selected = 0;

                foreach (var src in sources)
                {
                    selected += (decimal) src.Value;

                    var input = new SignedTransaction.Input
                    {
                        PrevHash = src.TxId.HexToBytes().Reverse().ToArray(),
                        PrevIndex = src.N
                    };

                    inputs.Add(input);

                    if (selected >= cost) break;
                }

                if (selected < cost) throw new WalletException($"Not enough {symbol}");

                if (cost > 0 && targets != null)
                    foreach (var target in transactionOutputs)
                    {
                        var output = new SignedTransaction.Output
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
                    var change = new SignedTransaction.Output
                    {
                        AssetId = assetId.HexToBytes().Reverse().ToArray(),
                        ScriptHash = signatureScript.ToArray(),
                        Value = left
                    };
                    outputs.Add(change);
                }
            }

            return (inputs, outputs);
        }

        public static async Task<(List<ClaimableElement>, decimal amount)> GetClaimable(string address, INeoRestService restService)
        {
            var claimableJson = await restService.GetClaimableAsync(address);
            var claimable = Claimable.FromJson(claimableJson);
            var amount = claimable.Unclaimed;

            return (claimable.ClaimableList.ToList(), (decimal)amount);
        }

        internal static Dictionary<string, string> GetAssetsInfo()
        {
            if (_systemAssets == null)
            {
                _systemAssets = new Dictionary<string, string>();
                AddAsset("NEO", Utils.NeoToken);
                AddAsset("GAS", Utils.GasToken);
            }

            return _systemAssets;
        }

        private static void AddAsset(string symbol, string hash)
        {
            _systemAssets[symbol] = hash;
        }
    }
}