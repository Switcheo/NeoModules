using System;
using System.IO;
using System.Linq;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs.Cryptography;
using NeoModules.KeyPairs.Cryptography.ECC;

namespace NeoModules.KeyPairs
{
    /// <summary>
    /// KeyPair class from https://github.com/neo-project/neo/blob/master/neo/Wallets/KeyPair.cs
    /// </summary>
    public class KeyPair : IEquatable<KeyPair>
    {
        public readonly byte[] PrivateKey;
        public readonly ECPoint PublicKey;

        public KeyPair(byte[] privateKey)
        {
            if (privateKey.Length != 32 && privateKey.Length != 96 && privateKey.Length != 104)
                throw new ArgumentException();
            PrivateKey = new byte[32];
            Buffer.BlockCopy(privateKey, privateKey.Length - 32, PrivateKey, 0, 32);
            if (privateKey.Length == 32)
                PublicKey = ECCurve.Secp256r1.G * privateKey;
            else
                PublicKey = ECPoint.FromBytes(privateKey, ECCurve.Secp256r1);
#if NET47
            ProtectedMemory.Protect(PrivateKey, MemoryProtectionScope.SameProcess);
#endif
        }

        public UInt160 PublicKeyHash => PublicKey.EncodePoint(true).ToScriptHash();

        public bool Equals(KeyPair other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PublicKey.Equals(other.PublicKey);
        }

        public IDisposable Decrypt()
        {
#if NET47
            return new ProtectedMemoryContext(PrivateKey, MemoryProtectionScope.SameProcess);
#else
            return new MemoryStream(0);
#endif
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as KeyPair);
        }

        public string Export()
        {
            using (Decrypt())
            {
                var data = new byte[34];
                data[0] = 0x80;
                Buffer.BlockCopy(PrivateKey, 0, data, 1, 32);
                data[33] = 0x01;
                var wif = data.Base58CheckEncode();
                Array.Clear(data, 0, data.Length);
                return wif;
            }
        }

        public string Export(string passphrase, int n = 16384, int r = 8, int p = 8)
        {
            using (Decrypt())
            {
                var scriptHash = Helper.CreateSignatureRedeemScript(PublicKey).ToScriptHash();
                var address = Helper.ToAddress(scriptHash);
                var addresshash = Encoding.ASCII.GetBytes(address).Sha256().Sha256().Take(4).ToArray();
                var derivedkey = SCrypt.DeriveKey(Encoding.UTF8.GetBytes(passphrase), addresshash, n, r, p, 64);
                var derivedhalf1 = derivedkey.Take(32).ToArray();
                var derivedhalf2 = derivedkey.Skip(32).ToArray();
                var encryptedkey = XOR(PrivateKey, derivedhalf1).AES256Encrypt(derivedhalf2);
                var buffer = new byte[39];
                buffer[0] = 0x01;
                buffer[1] = 0x42;
                buffer[2] = 0xe0;
                Buffer.BlockCopy(addresshash, 0, buffer, 3, addresshash.Length);
                Buffer.BlockCopy(encryptedkey, 0, buffer, 7, encryptedkey.Length);
                return buffer.Base58CheckEncode();
            }
        }

        public override int GetHashCode()
        {
            return PublicKey.GetHashCode();
        }

        public override string ToString()
        {
            return PublicKey.ToString();
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte)(a ^ b)).ToArray();
        }
    }
}