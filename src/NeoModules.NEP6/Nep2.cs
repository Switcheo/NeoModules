using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NeoModules.KeyPairs;
using NeoModules.KeyPairs.Cryptography;
using NeoModules.NEP6.Models;
using ECCurve = NeoModules.KeyPairs.Cryptography.ECC.ECCurve;

namespace NeoModules.NEP6
{
    public static class Nep2
    {
        public static IDisposable Decrypt()
        {
            return new MemoryStream(0);
        }

        /// <summary>
        ///     Encrypts a WIF key using a given keyphrase under NEP-2 Standard.
        /// </summary>
        /// <param name="wif">WIF key to encrypt (52 chars long).</param>
        /// <param name="password">The password will be encoded as UTF-8.</param>
        /// <param name="scryptParameters">Parameters for Scrypt. Defaults to NEP2 specified parameters.</param>
        /// <returns></returns>
        public static async Task<string> Encrypt(string wif, string password, ScryptParameters scryptParameters = null)
        {
            if (scryptParameters == null) scryptParameters = ScryptParameters.Default;

            using (Decrypt())
            {
                var keyPair = new KeyPair(Wallet.GetPrivateKeyFromWif(wif));
                var scriptHash =
                    Helper.CreateSignatureRedeemScript(keyPair.PublicKey).ToScriptHash(); // todo move this method
                var address = Wallet.ToAddress(scriptHash);

                var addresshash = Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).ToArray();
                return await Task.Run(() =>
                {
                    var derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(password), addresshash,
                        scryptParameters.N, scryptParameters.R, scryptParameters.P, 64);
                    var derivedhalf1 = derivedkey.Take(32).ToArray();
                    var derivedhalf2 = derivedkey.Skip(32).ToArray();
                    var encryptedkey = XOR(keyPair.PrivateKey, derivedhalf1).AES256Encrypt(derivedhalf2);
                    var buffer = new byte[39];
                    buffer[0] = 0x01;
                    buffer[1] = 0x42;
                    buffer[2] = 0xe0;
                    Buffer.BlockCopy(addresshash, 0, buffer, 3, addresshash.Length);
                    Buffer.BlockCopy(encryptedkey, 0, buffer, 7, encryptedkey.Length);

                    return buffer.Base58CheckEncode();
                });
            }
        }

        /// <summary>
        ///     Decrypts an encrypted key using a given keyphrase under NEP-2 Standard.
        /// </summary>
        /// <param name="encryptedKey"></param>
        /// <param name="password"></param>
        /// <param name="scryptParameters"></param>
        /// <returns></returns>
        public static async Task<byte[]> Decrypt(string encryptedKey, string password,
            ScryptParameters scryptParameters = null)
        {
            if (encryptedKey == null) throw new ArgumentNullException(nameof(encryptedKey));
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (scryptParameters == null) scryptParameters = ScryptParameters.Default;

            var data = encryptedKey.Base58CheckDecode();
            if (data.Length != 39 || data[0] != 0x01 || data[1] != 0x42 || data[2] != 0xe0)
                throw new FormatException();
            var addresshash = new byte[4];
            Buffer.BlockCopy(data, 3, addresshash, 0, 4);

            return await Task.Run(() =>
            {
                var derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(password), addresshash, scryptParameters.N,
                    scryptParameters.R, scryptParameters.P, 64);
                var derivedhalf1 = derivedkey.Take(32).ToArray();
                var derivedhalf2 = derivedkey.Skip(32).ToArray();
                var encryptedkey = new byte[32];
                Buffer.BlockCopy(data, 7, encryptedkey, 0, 32);
                var prikey = XOR(encryptedkey.AES256Decrypt(derivedhalf2), derivedhalf1);
                var pubkey = ECCurve.Secp256r1.G * prikey;
                var scriptHash = Helper.CreateSignatureRedeemScript(pubkey).ToScriptHash();
                var address = Wallet.ToAddress(scriptHash);
                if (!Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).SequenceEqual(addresshash))
                    throw new FormatException();

                return prikey;
            });
        }

        // Encryption methods
        internal static byte[] AES256Decrypt(this byte[] block, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                using (var decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(block, 0, block.Length);
                }
            }
        }

        internal static byte[] AES256Encrypt(this byte[] block, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                using (var encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(block, 0, block.Length);
                }
            }
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte) (a ^ b)).ToArray();
        }
    }
}