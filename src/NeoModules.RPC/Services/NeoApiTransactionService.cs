using NeoModules.JsonRpc.Client;
using NeoModules.RPC.Services.Transactions;

namespace NeoModules.RPC.Services
{
	public class NeoApiTransactionService : RpcClientWrapper
	{
		public NeoApiTransactionService(IClient client) : base(client)
		{
			GetApplicationLog = new NeoGetApplicationLog(client);
			GetRawTransaction = new NeoGetRawTransaction(client);
			GetRawTransactionSerialized = new NeoGetRawTransactionSerialized(client);
			SendRawTransaction = new NeoSendRawTransaction(client);
			GetTransactionOutput = new NeoGetTransactionOutput(client);
			SendToAddress = new NeoSendToAddress(client);
			SendMany = new NeoSendMany(client);
			SendFrom = new NeoSendFrom(client);
		}

		public NeoGetApplicationLog GetApplicationLog { get; private set; }
		public NeoGetRawTransaction GetRawTransaction { get; private set; }
		public NeoGetRawTransactionSerialized GetRawTransactionSerialized { get; private set; }
		public NeoSendRawTransaction SendRawTransaction { get; private set; }
		public NeoGetTransactionOutput GetTransactionOutput { get; private set; }
		public NeoSendToAddress SendToAddress { get; private set; }
		public NeoSendMany SendMany { get; private set; }
		public NeoSendFrom SendFrom { get; private set; }
	}
}
