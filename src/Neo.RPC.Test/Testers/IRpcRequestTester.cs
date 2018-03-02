using System;
using System.Threading.Tasks;
using Neo.JsonRpc.Client;

namespace Neo.RPC.Tests.Testers
{
    public interface IRpcRequestTester
    {
        Task<object> ExecuteTestAsync(IClient client);
        Type GetRequestType();
    }
}