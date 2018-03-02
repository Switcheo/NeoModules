//using System;
//using Neo.Core;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Neo.RPC.Helpers
//{
//    public static class AccountHelper
//    {
//        public static AccountState RpcResponseToAccountState(string rpcResponse)
//        {
//            if (string.IsNullOrEmpty(rpcResponse)) throw new ArgumentNullException(nameof(rpcResponse));


//            JObject obj = JObject.Parse(rpcResponse);
//            //return event array
//            var token = (JArray)obj.SelectToken("result");
//            AccountState x = JsonConvert.DeserializeObject<AccountState>(token.ToString());
//            return x;
//        }
//    }
//}
