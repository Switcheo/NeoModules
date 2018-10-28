using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NeoModules.Rest.Helpers
{
    public static class SwitcheoHelper
    {
        public static string PrepareParametersRequest(string json)
        {
            var parameterHexString = Utils.ConvertStringToHex(json);
            var lengthHex = (parameterHexString.Length / 2).ToString("X2").PadLeft(2, '0');
            var concatenatedString = lengthHex + parameterHexString;
            var serializedTransaction = "010001f0" + concatenatedString + "0000";
            return serializedTransaction;
        }

        public static string AddTransactFields(string signableParams, string signature, string addressHash)
        {
            // adds the 'address' and 'signature' fields to the json
            JObject apiParams = JsonConvert.DeserializeObject<JObject>(signableParams);
            apiParams["address"] = addressHash;
            apiParams["signature"] = signature;
            return apiParams.ToString();
        }
    }
}
