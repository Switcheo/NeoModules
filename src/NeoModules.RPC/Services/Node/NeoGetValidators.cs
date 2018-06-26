using NeoModules.RPC.Infrastructure;
using System.Collections.Generic;
using NeoModules.JsonRpc.Client;

namespace NeoModules.RPC.Services.Node
{
    public class NeoGetValidators : GenericRpcRequestResponseHandlerNoParam<List<DTOs.Validator>>
    {
        public NeoGetValidators(IClient client) : base(client, ApiMethods.getvalidators.ToString())
        {
        }
    }
}
