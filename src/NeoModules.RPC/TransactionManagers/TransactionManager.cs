using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;

namespace NeoModules.RPC.TransactionManagers
{
    public class TransactionManager : ITransactionManager
    {
        public TransactionManager(IClient client)
        {
            Client = client;
        }

        public IClient Client { get; set; }

        public async Task<bool> SendTransactionAsync(string hexTx) //another method with TransactionInput parameter
        {
            if (Client == null) throw new NullReferenceException("Client not configured");
            if (string.IsNullOrEmpty(hexTx)) throw new ArgumentNullException(nameof(hexTx));

            var neoSendTransaction = new NeoSendRawTransaction(Client);
            return await neoSendTransaction.SendRequestAsync(hexTx).ConfigureAwait(false); 
        }


    }
}