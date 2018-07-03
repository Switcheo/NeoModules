using Newtonsoft.Json;
using System.Collections.Generic;
using NeoModules.KeyPairs;
using System;
using System.IO;

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
        public Wallet(string name = "DefaultWallet", string version = "1.0", ScryptParameters scryptParameters = null, List<Account> accounts = null,
            object extra = null)
        {
            Name = name;
            Version = version;
            Scrypt = scryptParameters ?? ScryptParameters.Default;
            Accounts = accounts ?? new List<Account>();
            Extra = extra;
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter file = File.CreateText(filePath))
            {
                file.Write(ToJson(this));
            }
        }

        public static Wallet FromJson(string json) => JsonConvert.DeserializeObject<Wallet>(json);

        public static string ToJson(Wallet self) => JsonConvert.SerializeObject(self);

        public static byte[] GetPrivateKeyFromWif(string wif)
        {
            if (wif == null) throw new ArgumentNullException();
            byte[] data = wif.Base58CheckDecode();
            if (data.Length != 34 || data[0] != 0x80 || data[33] != 0x01)
                throw new FormatException();
            byte[] privateKey = new byte[32];
            Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
            Array.Clear(data, 0, data.Length);
            return privateKey;
        }
    }
}
