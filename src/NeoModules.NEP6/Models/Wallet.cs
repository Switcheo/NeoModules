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
			Version = version ?? "1.0";
			Scrypt = scryptParameters;
			Accounts = accounts;
			Extra = extra;
		}

		//private void AddAccount(Account account, bool is_import)
		//{
		//	lock (Accounts)
		//	{
		//		if (Accounts.TryGetValue(account.ScriptHash, out NEP6Account account_old))
		//		{
		//			account.Label = account_old.Label;
		//			account.IsDefault = account_old.IsDefault;
		//			account.Lock = account_old.Lock;
		//			if (account.Contract == null)
		//			{
		//				account.Contract = account_old.Contract;
		//			}
		//			else
		//			{
		//				NEP6Contract contract_old = (NEP6Contract)account_old.Contract;
		//				if (contract_old != null)
		//				{
		//					NEP6Contract contract = (NEP6Contract)account.Contract;
		//					contract.ParameterNames = contract_old.ParameterNames;
		//					contract.Deployed = contract_old.Deployed;
		//				}
		//			}
		//			account.Extra = account_old.Extra;
		//		}
		//		else
		//		{
		//			WalletIndexer.RegisterAccounts(new[] { account.ScriptHash }, is_import ? 0 : Blockchain.Default?.Height ?? 0);
		//		}
		//		accounts[account.ScriptHash] = account;
		//	}
		//}

		public static Wallet FromJson(string json) => JsonConvert.DeserializeObject<Wallet>(json);

		public static string ToJson(Wallet self) => JsonConvert.SerializeObject(self);
	}
}
