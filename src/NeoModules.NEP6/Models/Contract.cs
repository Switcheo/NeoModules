using NeoModules.Core;
using NeoModules.Hex.HexConverters.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace NeoModules.NEP6.Models
{
	public class Contract
	{
		/// <summary>
		/// The script code of the contract. 
		/// This field can be null if the contract has been deployed to the blockchain. 
		/// </summary>
		[JsonProperty("script")]
		public byte[] Script { get; set; }

		/// <summary>
		/// An array of Parameter objects which describe the details of each parameter in the contract function.
		/// </summary>
		[JsonProperty("parameters")]
		public List<Parameter> Parameters { get; set; }

		/// <summary>
		/// An array of Parameter objects which describe the details of each parameter in the contract function.
		/// For more information about Parameter object, see the descriptions in NEP-3: <see href="https://github.com/neo-project/proposals/pull/12">NeoContract ABI</see>
		/// </summary>
		[JsonProperty("deployed")]
		public bool Deployed { get; set; }

		//[JsonConstructor]
		//public Contract(Parameter[] parameters, bool deployed, string script = null)
		//{
		//	Parameters = parameters;
		//	Deployed = false;
		//	Script = script;
		//}

		public static Contract FromJson(JObject json)
		{
			return new Contract
			{
				Script = json["script"].ToString().HexToBytes(),
				Parameters = (JArray)json["parameters"]).
				ParameterList = ((JArray)json["parameters"]).Select(p => p["type"].AsEnum<ParameterType>()).ToArray(),
				ParameterNames = ((JArray)json["parameters"]).Select(p => p["name"].AsString()).ToArray(),
				Deployed = json["deployed"].AsBoolean()
			};
			JsonConvert.DeserializeObject<Contract>(json);
		}

		public static string ToJson(Contract self) => JsonConvert.SerializeObject(self);
	}
}
