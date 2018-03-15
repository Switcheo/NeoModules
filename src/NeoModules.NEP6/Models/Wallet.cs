using Newtonsoft.Json;
using System.Collections.Generic;

namespace NeoModules.NEP6.Models
{
	public class Wallet
	{
		/// <summary>
		/// A label that the user has made to the wallet file
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Is currently fixed at 1.0 and will be used for functional upgrades in the future. 
		/// </summary>
		[JsonProperty("version")]
		public string Version { get; }

		/// <summary>
		/// Object which describe the parameters of SCrypt algorithm used for encrypting and decrypting the private keys in the wallet. 
		/// </summary>
		[JsonProperty("scrypt")]
		public ScryptParameters Scrypt { get; set; }

		/// <summary>
		/// An array of Account objects which describe the details of each account in the wallet.
		/// </summary>
		[JsonProperty("accounts")]
		public List<Account> Accounts { get; set; }

		/// <summary>
		/// An object that is defined by the implementor of the client for storing extra data.This field can be null.
		/// </summary>
		[JsonProperty("extra")]
		public object Extra { get; set; }

		[JsonConstructor]
		public Wallet(string name, string version, ScryptParameters scryptParameters, List<Account> accounts, object extra = null)
		{
			Name = name ?? "default";
			Version = Version ?? "1.0";
			Scrypt = scryptParameters;
			Accounts = accounts;
			Extra = extra;
		}

		public static Wallet FromJson(string json) => JsonConvert.DeserializeObject<Wallet>(json);

		public static string ToJson(Wallet self) => JsonConvert.SerializeObject(self);
	}
}
