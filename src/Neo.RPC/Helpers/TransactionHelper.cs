//using System;
//using Neo.Core;

//namespace Neo.RPC.Helpers
//{
//    public static class TransactionHelper
//    {
//        public static Transaction RpcResponseToTransaction(string rpcResponse)
//        {
//            if (string.IsNullOrEmpty(rpcResponse)) throw new ArgumentNullException(nameof(rpcResponse));

//            var data = rpcResponse.HexToBytes();

//            return Transaction.DeserializeFrom(data);
//        }
//    }
//}
