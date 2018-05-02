using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.DTOs;
using NeoModules.RPC.Infrastructure;
using NeoModules.RPC.Services.Transactions;

namespace NeoModules.RPC.TransactionManagers
{
    public class TransactionManager : TransactionManagerBase
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }

        public override Task<bool> SendTransactionAsync(CallInput transactionInput)
        {
            throw new NotImplementedException();
        }
    }
}