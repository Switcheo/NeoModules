using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;

namespace NeoModules.RPC.Services
{
    public class NeoApiTransactionService : RpcClientWrapper
    {
        public NeoApiTransactionService(IClient client) : base(client)
        {
            GetRawTransaction = new NeoGetRawTransaction(client);
            GetRawTransactionSerialized = new NeoGetRawTransactionSerialized(client);
            SendRawTransaction = new NeoSendRawTransaction(client);
            GetTransactionOutput = new NeoGetTransactionOutput(client);
            GetApplicationLog = new NeoGetApplicationLog(client);
        }

        public NeoGetRawTransaction GetRawTransaction { get; private set; }
        public NeoGetRawTransactionSerialized GetRawTransactionSerialized { get; private set; }
        public NeoSendRawTransaction SendRawTransaction { get; private set; }
        public NeoGetTransactionOutput GetTransactionOutput { get; private set; }
        public NeoGetApplicationLog GetApplicationLog { get; private set; }

    }
}
