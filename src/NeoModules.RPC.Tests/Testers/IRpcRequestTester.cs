using System;
using System.Threading.Tasks;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Tests.Testers
{
    public interface IRpcRequestTester
    {
        Task<object> ExecuteTestAsync(IClient client);
        Type GetRequestType();
    }
}