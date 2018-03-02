using Newtonsoft.Json.Linq;

namespace Neo.JsonRpc.Client
{
    public class RpcError
    {
        public RpcError(int code, string message, JToken data = null)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public int Code { get; }
        public string Message { get; }
        public JToken Data { get; }
    }
}
