using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NeoModules.Rest.Interfaces;

namespace NeoModules.Rest.Services
{
    public class NelApiRestService:INelApiRestService
    {
        private static readonly string nelScanMainNetUrl = "https://api.nel.group/api/testnet";
        private static readonly string nelScaTestNetUrl = "https://api.nel.group/api/mainnet";

        private static readonly string getBalance = "get_balance/";
        private static readonly string getClaimed = "get_claimed/";
        private static readonly string getClaimable = "get_claimable/";
        private static readonly string getUnclaimed = "get_unclaimed/";
        private static readonly string getAllNodes = "get_all_nodes/";

        public Task<long> GetBlockCount()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlock(int height)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRawTransaction(string txHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetAsset(string assetHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFullLog(string txHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNotify(string txHash)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUtxo(string address)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUtxoCount(string address)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUtxosToPay(string address, string txHash, decimal firstValue, decimal secondValue)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBalance(string address)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlocks(int maxHeigth, int minHeight)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRawTransactions(int maxHeight, int minHeight, string type)
        {
            throw new NotImplementedException();
        }
    }
}
