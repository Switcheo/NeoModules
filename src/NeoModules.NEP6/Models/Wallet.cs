using Newtonsoft.Json;
using System.Collections.Generic;
using NeoModules.KeyPairs;
using System;
using System.IO;
using NeoModules.Core;

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

        [JsonIgnore] public WalletManager WalletManager { get; set; }

        [JsonConstructor]
        public Wallet(string name, string version, ScryptParameters scryptParameters, List<Account> accounts,
            object extra = null)
        {
            Name = name ?? "default";
            Version = version ?? "1.0";
            Scrypt = scryptParameters;
            Accounts = accounts;
            Extra = extra;

            // TODO: this needs to change to enable custom interface implementation
            WalletManager = new WalletManager(this);
        }

        public static Wallet FromJson(string json) => JsonConvert.DeserializeObject<Wallet>(json);

        public static string ToJson(Wallet self) => JsonConvert.SerializeObject(self);

        public static string ToAddress(UInt160 scriptHash)
        {
            byte[] data = new byte[21];
            data[0] = byte.Parse("23");
            Buffer.BlockCopy(scriptHash.ToArray(), 0, data, 1, 20);
            return data.Base58CheckEncode();
        }

        public static Wallet LoadFromFile(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var json = file.ReadToEnd();
                return FromJson(json);
            }
        }
    }
}
