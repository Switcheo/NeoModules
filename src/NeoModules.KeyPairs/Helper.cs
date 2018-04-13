using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using NeoModules.Core;
using NeoModules.KeyPairs.Cryptography;
using NeoModules.NVM;
using ECPoint = NeoModules.KeyPairs.Cryptography.ECC.ECPoint;

namespace NeoModules.KeyPairs
{
    public static class Helper
    {
        private static readonly ThreadLocal<SHA256> _sha256 = new ThreadLocal<SHA256>(() => SHA256.Create());

        private static readonly ThreadLocal<RIPEMD160Managed> _ripemd160 =
            new ThreadLocal<RIPEMD160Managed>(() => new RIPEMD160Managed());

        public static byte AddressVersion { get; } = byte.Parse("23");

        /// <summary>
        ///     Decodes a string to his correspondent byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] Base58CheckDecode(this string input)
        {
            var buffer = Base58.Decode(input);
            if (buffer.Length < 4) throw new FormatException();
            var checksum = buffer.Sha256(0, buffer.Length - 4).Sha256();
            if (!buffer.Skip(buffer.Length - 4).SequenceEqual(checksum.Take(4)))
                throw new FormatException();
            return buffer.Take(buffer.Length - 4).ToArray();
        }

        /// <summary>
        ///     Encodes a byte array to his correspondent Base58 string
        /// </summary>
        /// <param name="data">Byte array</param>
        /// <returns></returns>
        public static string Base58CheckEncode(this byte[] data)
        {
            var checksum = data.Sha256().Sha256();
            var buffer = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, buffer, 0, data.Length);
            Buffer.BlockCopy(checksum, 0, buffer, data.Length, 4);
            return Base58.Encode(buffer);
        }

        /// <summary>
        ///     Creates the public address of a public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public static byte[] CreateSignatureRedeemScript(ECPoint publicKey)
        {
            using (var sb = new ScriptBuilder())
            {
                sb.EmitPush(publicKey.EncodePoint(true));
                sb.Emit(OpCode.CHECKSIG);
                return sb.ToArray();
            }
        }

        /// <summary>
        ///     Converts the byte array to a ScriptHash of type UInt160
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static UInt160 ToScriptHash(this byte[] script)
        {
            return new UInt160(script.Sha256().RIPEMD160());
        }

        /// <summary>
        ///     Converts the address in string format to a ScriptHash of type UInt160
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static UInt160 ToScriptHash(string address)
        {
            var data = address.Base58CheckDecode();
            if (data.Length != 21)
                throw new FormatException();
            if (data[0] != AddressVersion)
                throw new FormatException();
            return new UInt160(data.Skip(1).ToArray());
        }

        /// <summary>
        ///     Find the sha256 hash value of the byte array
        /// </summary>
        /// <param name="value">Byte array</param>
        /// <returns>Returns this hash value</returns>
        public static byte[] Sha256(this IEnumerable<byte> value)
        {
            return _sha256.Value.ComputeHash(value.ToArray());
        }


        /// <summary>
        ///     Finds the sha256 hash value of the byte array
        /// </summary>
        /// <param name="value">Byte array</param>
        /// <param name="offset">Offset, starting from this offset when hashing</param>
        /// <param name="count">The number of bytes to calculate the hash value</param>
        /// <returns>Return this hash value</returns>
        public static byte[] Sha256(this byte[] value, int offset, int count)
        {
            return _sha256.Value.ComputeHash(value, offset, count);
        }

        /// <summary>
        ///     Ripemd160 hash value for byte array
        /// </summary>
        /// <param name="value">Byte array</param>
        /// <returns>Return this hash value</returns>
        public static byte[] RIPEMD160(this IEnumerable<byte> value)
        {
            return _ripemd160.Value.ComputeHash(value.ToArray());
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
    }
}