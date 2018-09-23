using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.DTOs.NeoScan;
using NeoModules.Rest.Interfaces;


namespace NeoModules.NEP6.Helpers
{
    public static class TransactionBuilderHelper
    {
        private static readonly Dictionary<string, UInt256> SystemAssets = new Dictionary<string, UInt256>
        {
            {"NEO", Utils.NeoToken},
            {"GAS", Utils.GasToken},
        };

        private static Fixed8 SystemFee => Fixed8.Zero;

        public static async Task<IEnumerable<Coin>> GetUnspent(string address,
            INeoscanService restService)
        {
            if (restService == null) throw new NullReferenceException("REST client not configured");
            var addressBalance = await restService.GetBalanceAsync(address);

            var coinList = new List<Coin>();
            if (addressBalance.Balance != null)
                foreach (var balanceEntry in addressBalance.Balance)
                {
                    var child = balanceEntry.Unspent;
                    if (!(child?.Count > 0)) continue;

                    foreach (var unspent in balanceEntry.Unspent)
                    {
                        coinList.Add(new Coin
                        {
                            Output = new TransactionOutput
                            {
                                AssetId = SystemAssets[balanceEntry.Asset],
                                ScriptHash = address.ToScriptHash(),
                                Value = Fixed8.FromDecimal((decimal) unspent.Value),
                            },
                            Reference = new CoinReference
                            {
                                PrevHash = UInt256.Parse(unspent.TxId),
                                PrevIndex = unspent.N,
                            }
                        });
                    }
                }
            return coinList;
        }

        //public static async Task<Dictionary<string, List<Unspent>>> GetUnspent(string address,
        //    INeoscanService restService)
        //{
        //    if (restService == null) throw new NullReferenceException("REST client not configured");
        //    var addressBalance = await restService.GetBalanceAsync(address);

        //    var result = new Dictionary<string, List<Unspent>>();
        //    if (addressBalance.Balance != null)
        //        foreach (var balanceEntry in addressBalance.Balance)
        //        {
        //            var child = balanceEntry.Unspent;
        //            if (!(child?.Count > 0)) continue;
        //            List<Unspent> list;
        //            if (result.ContainsKey(balanceEntry.Asset))
        //            {
        //                list = result[balanceEntry.Asset];
        //            }
        //            else
        //            {
        //                list = new List<Unspent>();
        //                result[balanceEntry.Asset] = list;
        //            }

        //            list.AddRange(balanceEntry.Unspent.Select(data => new Unspent
        //            {
        //                TxId = data.TxId,
        //                N = data.N,
        //                Value = data.Value
        //            }));
        //        }

        //    return result;
        //}


        //// Old method
        //public static async Task<(List<CoinReference> inputs, List<TransactionOutput> outputs)>
        //   GenerateInputsOutputs(string address, string symbol, IEnumerable<TransferOutput> targets, decimal fee,
        //       INeoscanService restService)
        //{
        //    var unspent = await GetUnspent(address, restService);

        //    // filter any asset lists with zero unspent inputs
        //    unspent = unspent.Where(pair => pair.Value.Count > 0).ToDictionary(pair => pair.Key, pair => pair.Value);


        //    var inputs = new List<CoinReference>();
        //    var outputs = new List<TransactionOutput>();

        //    string assetId;

        //    var info = GetAssetsInfo();
        //    if (info.ContainsKey(symbol))
        //        assetId = info[symbol];
        //    else
        //        throw new WalletException($"{symbol} is not a valid blockchain asset.");

        //    decimal cost = 0;

        //    var fromHash = key.PublicKeyHash.ToArray();
        //    List<TransferOutput> transactionOutputs = null;
        //    if (targets != null)
        //    {
        //        transactionOutputs = targets.ToList();
        //        foreach (var target in transactionOutputs)
        //        {
        //            if (target.AddressHash.SequenceEqual(fromHash))
        //                throw new WalletException("Target can't be same as coinReference");

        //            cost += target.Amount;
        //        }
        //    }

        //    if (unspent.Count > 0)
        //    {
        //        var sources = unspent[symbol];
        //        decimal selected = 0;

        //        foreach (var src in sources)
        //        {
        //            selected += (decimal)src.Value;

        //            var input = new Transaction.CoinReference
        //            {
        //                PrevHash = src.TxId.HexToBytes().Reverse().ToArray(),
        //                PrevIndex = src.N
        //            };

        //            inputs.Add(input);

        //            if (selected >= cost) break;
        //        }

        //        if (selected < cost) throw new WalletException($"Not enough {symbol}");

        //        if (cost > 0 && targets != null)
        //            foreach (var target in transactionOutputs)
        //            {
        //                var output = new Transaction.TransactionOutput
        //                {
        //                    AssetId = assetId.HexToBytes().Reverse().ToArray(),
        //                    ScriptHash = target.AddressHash.ToArray(),
        //                    Value = target.Amount
        //                };
        //                outputs.Add(output);
        //            }

        //        if (selected > cost || cost == 0)
        //        {
        //            var left = selected - cost;
        //            var signatureScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToScriptHash();
        //            var change = new Transaction.TransactionOutput
        //            {
        //                AssetId = assetId.HexToBytes().Reverse().ToArray(),
        //                ScriptHash = signatureScript.ToArray(),
        //                Value = left
        //            };
        //            outputs.Add(change);
        //        }
        //    }

        //    return (inputs, outputs);
        //}

        public static async Task<(List<ClaimableElement>, decimal amount)> GetClaimable(string address, INeoscanService restService)
        {
            var claimable = await restService.GetClaimableAsync(address);
            var amount = claimable.Unclaimed;

            return (claimable.ClaimableList.ToList(), (decimal)amount);
        }

        //public static List<TransferOutput> BuildTransferOutputs(string address, Dictionary<string, decimal> symbolsAndAmount)
        //{
        //    var targets = new List<TransferOutput>();
        //    var toScriptHash = address.ToScriptHash().ToArray();
        //    foreach (var symbol in symbolsAndAmount.Keys)
        //    {
        //        var target = new TransferOutput
        //        {
        //            AssetId = SystemAssets[symbol].HexToBytes().Reverse().ToArray(),
        //            AddressHash = toScriptHash,
        //            Amount = symbolsAndAmount[symbol],
        //            Symbol = symbol
        //        };
        //        targets.Add(target);
        //    }
        //    return targets;
        //}
    }
}