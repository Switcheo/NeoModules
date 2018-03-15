using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NeoModules.NEP6.Models
{
	public class Parameter
	{
		/// <summary>
		/// The name of the parameter, which can be any valid identifier. 
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Indicates the type of the parameter. 
		/// It can be one of the following values: Signature, Boolean, Integer, Hash160, Hash256, ByteArray, PublicKey, String, Array, InteropInterface. 
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("type")]
		public ParameterType Type { get; set; }

		[JsonConstructor]
		public Parameter(string name, ParameterType type)
		{
			Name = name;
			Type = type;
		}

		public static Parameter FromJson(string json) => JsonConvert.DeserializeObject<Parameter>(json);

		public static string ToJson(Parameter self) => JsonConvert.SerializeObject(self);
	}

	public enum ParameterType : byte
	{
		Signature = 0x00,
		Boolean = 0x01,
		Integer = 0x02,
		Hash160 = 0x03,
		Hash256 = 0x04,
		ByteArray = 0x05,
		PublicKey = 0x06,
		String = 0x07,
		Array = 0x10,
		InteropInterface = 0xf0,
		Void = 0xff
	}

	public static class EnumUtil
	{
		public static IEnumerable<T> GetValues<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}
