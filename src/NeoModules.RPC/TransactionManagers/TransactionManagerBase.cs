using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.Services.Contract;
using NeoModules.RPC.Services.Transactions;

namespace NeoModules.RPC.TransactionManagers
{
    public abstract class TransactionManagerBase : ITransactionManager
    {
        public IClient Client { get; set; }
        public IAccount Account { get; set; }

        public abstract string SignMessage(string messageToSign);

        public abstract Task<Transaction> WaitForTxConfirmation(string tx);

        public virtual async Task<decimal> EstimateGasAsync(string serializedScriptHash)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (serializedScriptHash == null) throw new ArgumentNullException(nameof(serializedScriptHash));
            var neoEstimateGas = new NeoInvokeScript(Client);
            var invokeResult = await neoEstimateGas.SendRequestAsync(serializedScriptHash);
            return invokeResult.GasConsumed;
        }

        public virtual async Task<decimal> EstimateGasAsync(string scriptHash, string operation,
            List<InvokeParameter> parameterList)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (scriptHash == null) throw new ArgumentNullException(nameof(scriptHash));
            var neoEstimateGas = new NeoInvokeFunction(Client);
            var invokeResult = await neoEstimateGas.SendRequestAsync(scriptHash, operation, parameterList);
            return invokeResult.GasConsumed;
        } 

        public async Task<bool> SendTransactionAsync(string signedTx)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (signedTx == null) throw new ArgumentNullException(nameof(signedTx));
            var neoSendRawTransaction = new NeoSendRawTransaction(Client);
            return await neoSendRawTransaction.SendRequestAsync(signedTx);
        }

        public async Task<Transaction> GetTransaction(string tx)
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (string.IsNullOrEmpty(tx)) throw new ArgumentNullException(nameof(tx));
            var neoGetRawTransaction = new NeoGetRawTransaction(Client);
            return await neoGetRawTransaction.SendRequestAsync(tx);
        }
    }
}