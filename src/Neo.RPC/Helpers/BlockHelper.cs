//using System.IO;
//using Neo.Core;

//namespace Neo.RPC.Helpers
//{
//    public static class BlockHelper
//    {
//        public static Block RpcResponseToBlock(string rpcResponse)
//        {
//            var data = rpcResponse.HexToBytes();
//            var block = new Block();

//            using (MemoryStream ms = new MemoryStream(data, false))
//            using (BinaryReader r = new BinaryReader(ms))
//            {
//                block.Deserialize(r);
//                return block;
//            }
//        }
//    }
//}
