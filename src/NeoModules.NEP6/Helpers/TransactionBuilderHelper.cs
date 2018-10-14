using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.NEP6.Transactions;
using NeoModules.Rest.DTOs.NeoScan;
using NeoModules.Rest.Interfaces;

namespace NeoModules.NEP6.Helpers
{
    public static class TransactionBuilderHelper
    {
        public static async Task<IEnumerable<Coin>> GetUnspent(string address,
            INeoscanService restService)
        {
            if (restService == null) throw new NullReferenceException("REST client not configured");
            var addressBalance = await restService.GetBalanceAsync(address);

            var coinList = new List<Coin>();
            if (addressBalance.Balance != null)
            {
                coinList.AddRange(from balanceEntry in addressBalance.Balance
                    let child = balanceEntry.Unspent
                    where child?.Count > 0
                    from unspent in balanceEntry.Unspent
                    select new Coin
                    {
                        Output = new TransactionOutput
                        {
                            AssetId = UInt256.Parse(balanceEntry.AssetHash),
                            ScriptHash = address.ToScriptHash(),
                            Value = Fixed8.FromDecimal((decimal)unspent.Value),
                        },
                        Reference = new CoinReference
                        {
                            PrevHash = UInt256.Parse(unspent.TxId),
                            PrevIndex = (ushort)unspent.N,
                        }
                    });
            }
            return coinList;
        }

        //TODO change this to become neoscan independent
        public static async Task<List<Balance>> GetNep5Balances(string address,
            INeoscanService restService)
        {
            if (restService == null) throw new NullReferenceException("REST client not configured");
            var addressBalance = await restService.GetBalanceAsync(address);
            var nep5Balances = new List<Balance>();
            if (addressBalance.Balance != null)
            {
                foreach (var balanceEntry in addressBalance.Balance)
                {
                    if (balanceEntry.Amount > 0)
                    {
                        nep5Balances.Add(balanceEntry);
                    }
                }
            }

            return nep5Balances;
        }

        public static async Task<(List<ClaimableElement>, decimal amount)> GetClaimable(string address, INeoscanService restService)
        {
            var claimable = await restService.GetClaimableAsync(address);
            var amount = claimable.Unclaimed;

            return (claimable.ClaimableList.ToList(), (decimal)amount);
        }

        public static async Task<Coin[]> FindUnspentCoins(UInt256 assetId, Fixed8 amount, UInt160 from, INeoscanService restService)
        {
            var address = from.ToAddress(); //todo
            var unspents = await GetUnspent(address, restService);

            Coin[] unspentsAsset = unspents.Where(p => p.Output.AssetId == assetId).ToArray();
            Fixed8 sum = unspentsAsset.Sum(p => p.Output.Value);
            if (sum < amount) return null;
            if (sum == amount) return unspentsAsset;
            Coin[] unspentsOrdered = unspentsAsset.OrderByDescending(p => p.Output.Value).ToArray();
            int i = 0;
            while (unspentsOrdered[i].Output.Value <= amount)
                amount -= unspentsOrdered[i++].Output.Value;
            if (amount == Fixed8.Zero)
                return unspentsOrdered.Take(i).ToArray();
            else
                return unspentsOrdered.Take(i).Concat(new[] { unspentsOrdered.Last(p => p.Output.Value >= amount) }).ToArray();
        }
    }
}