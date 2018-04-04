using Newtonsoft.Json;
using System.Collections.Generic;
using NeoModules.KeyPairs;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs.Cryptography;
using NeoModules.NEP6.Services;

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

        [JsonIgnore]
        public AccountService AccountService { get; set; }

        [JsonConstructor]
        public Wallet(string name, string version, ScryptParameters scryptParameters, List<Account> accounts, object extra = null)
        {
            Name = name ?? "default";
            Version = version ?? "1.0";
            Scrypt = scryptParameters;
            Accounts = accounts;
            Extra = extra;

            //AccountService = new AccountService(this);
        }

        public static Wallet FromJson(string json) => JsonConvert.DeserializeObject<Wallet>(json);

        public static string ToJson(Wallet self) => JsonConvert.SerializeObject(self);

        // TODO move this to somekind of service
        public static Wallet LoadFromFile(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var json = file.ReadToEnd();
                return FromJson(json);
            }
        }

        public void AddAccount(Account account)
        {
            lock (Accounts)
            {
                if (!Accounts.Exists(p => p.Address == account.Address))
                {
                    Accounts.Add(account);
                }
            }
        }

        public bool DeleteAccount(Account account)
        {
            if (account == null) return false;
            lock (Accounts)
            {
                bool deleted;
                deleted = Accounts.Remove(account);
                return deleted;
            }
        }

        public Account GetAccount(UInt160 scriptHash)
        {
            lock (Accounts)
            {
                return Accounts.FirstOrDefault(p => p.Address == scriptHash);
            }
        }

        // todo decouple this
        public static byte[] GetPrivateKeyFromNep2(string nep2, string passphrase, int n = 16384, int r = 8, int p = 8)
        {
            if (nep2 == null) throw new ArgumentNullException(nameof(nep2));
            if (passphrase == null) throw new ArgumentNullException(nameof(passphrase));
            byte[] data = nep2.Base58CheckDecode();
            if (data.Length != 39 || data[0] != 0x01 || data[1] != 0x42 || data[2] != 0xe0)
                throw new FormatException();
            byte[] addresshash = new byte[4];
            Buffer.BlockCopy(data, 3, addresshash, 0, 4);
            byte[] derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(passphrase), addresshash, n, r, p, 64);
            byte[] derivedhalf1 = derivedkey.Take(32).ToArray();
            byte[] derivedhalf2 = derivedkey.Skip(32).ToArray();
            byte[] encryptedkey = new byte[32];
            Buffer.BlockCopy(data, 7, encryptedkey, 0, 32);
            byte[] prikey = XOR(encryptedkey.AES256Decrypt(derivedhalf2), derivedhalf1);
            KeyPairs.Cryptography.ECC.ECPoint pubkey = KeyPairs.Cryptography.ECC.ECCurve.Secp256r1.G * prikey;
            UInt160 script_hash = KeyPairs.Helper.CreateSignatureRedeemScript(pubkey).ToScriptHash();
            string address = KeyPairs.Helper.ToAddress(script_hash);
            if (!Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).SequenceEqual(addresshash))
                throw new FormatException();
            return prikey;
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte)(a ^ b)).ToArray();
        }
    }

    public static class HelperToRemove
    {
        internal static byte[] AES256Decrypt(this byte[] block, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(block, 0, block.Length);
                }
            }
        }

        internal static byte[] AES256Encrypt(this byte[] block, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(block, 0, block.Length);
                }
            }
        }
    }
}
