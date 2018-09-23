using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NeoModules.Core;
using NeoModules.NEP6.Transactions;
using NeoModules.NVM;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace NeoModules.NEP6.Helpers
{
    public static class Utils
    {
        public static readonly UInt256 NeoToken = UInt256.Parse("c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
        public static readonly UInt256 GasToken = UInt256.Parse("602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static byte[] TranscodeSignatureToConcat(byte[] derSignature, int outputLength)
        {
            if (derSignature.Length < 8 || derSignature[0] != 48) throw new Exception("Invalid ECDSA signature format");

            int offset;
            if (derSignature[1] > 0)
                offset = 2;
            else if (derSignature[1] == 0x81)
                offset = 3;
            else
                throw new Exception("Invalid ECDSA signature format");

            var rLength = derSignature[offset + 1];

            int i = rLength;
            while (i > 0
                   && derSignature[offset + 2 + rLength - i] == 0)
                i--;

            var sLength = derSignature[offset + 2 + rLength + 1];

            int j = sLength;
            while (j > 0
                   && derSignature[offset + 2 + rLength + 2 + sLength - j] == 0)
                j--;

            var rawLen = Math.Max(i, j);
            rawLen = Math.Max(rawLen, outputLength / 2);

            if ((derSignature[offset - 1] & 0xff) != derSignature.Length - offset
                || (derSignature[offset - 1] & 0xff) != 2 + rLength + 2 + sLength
                || derSignature[offset] != 2
                || derSignature[offset + 2 + rLength] != 2)
                throw new Exception("Invalid ECDSA signature format");

            var concatSignature = new byte[2 * rawLen];

            Array.Copy(derSignature, offset + 2 + rLength - i, concatSignature, rawLen - i, i);
            Array.Copy(derSignature, offset + 2 + rLength + 2 + sLength - j, concatSignature, 2 * rawLen - j, j);

            return concatSignature;
        }

        internal static byte[] Sign(byte[] message, byte[] prikey)
        {
            var signer = SignerUtilities.GetSigner("SHA256withECDSA");
            var curve = NistNamedCurves.GetByName("P-256");
            var dom = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            ECKeyParameters privateKeyParameters = new ECPrivateKeyParameters(new BigInteger(1, prikey), dom);

            signer.Init(true, privateKeyParameters);
            signer.BlockUpdate(message, 0, message.Length);
            var sig = signer.GenerateSignature();

            return TranscodeSignatureToConcat(sig, 64);
        }

        public static System.Numerics.BigInteger ConvertToBigInt(decimal value, int decimals)
        {
            while (decimals > 0)
            {
                value *= 10;
                decimals--;
            }

            return new System.Numerics.BigInteger((ulong)value);
        }

        public static uint ToTimestamp(this DateTime time)
        {
            return (uint)(time.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        public static byte[]
            GenerateScript(byte[] scriptHash, string operation, object[] args, bool addNonce = true) //todo maintain nonce for sending the same txhash in case of tx failure/mempool
        {
            var script = new UInt160(scriptHash);
            using (var sb = new ScriptBuilder())
            {
                if (args != null)
                    sb.EmitAppCall(script, operation, args);
                else
                    sb.EmitAppCall(script, operation);

                if (addNonce)
                {
                    var timestamp = DateTime.UtcNow.ToTimestamp();
                    var nonce = BitConverter.GetBytes(timestamp);

                    sb.Emit(OpCode.RET);
                    sb.EmitPush(nonce);
                }

                var bytes = sb.ToArray();
                return bytes;
            }
        }

        public static byte[] ReadVarBytes(this BinaryReader reader, int max = 0X7fffffc7)
        {
            return reader.ReadBytes((int)reader.ReadVarInt((ulong)max));
        }

        public static ulong ReadVarInt(this BinaryReader reader, ulong max = ulong.MaxValue)
        {
            var fb = reader.ReadByte();
            ulong value;
            if (fb == 0xFD)
                value = reader.ReadUInt16();
            else if (fb == 0xFE)
                value = reader.ReadUInt32();
            else if (fb == 0xFF)
                value = reader.ReadUInt64();
            else
                value = fb;
            if (value > max) throw new FormatException();
            return value;
        }

        public static void WriteVarBytes(this BinaryWriter writer, byte[] value)
        {
            writer.WriteVarInt(value.Length);
            writer.Write(value);
        }

        public static T ReadSerializable<T>(this BinaryReader reader) where T : ISerializable, new()
        {
            T obj = new T();
            obj.Deserialize(reader);
            return obj;
        }

        public static void WriteVarInt(this BinaryWriter writer, long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException();
            if (value < 0xFD)
            {
                writer.Write((byte)value);
            }
            else if (value <= 0xFFFF)
            {
                writer.Write((byte)0xFD);
                writer.Write((ushort)value);
            }
            else if (value <= 0xFFFFFFFF)
            {
                writer.Write((byte)0xFE);
                writer.Write((uint)value);
            }
            else
            {
                writer.Write((byte)0xFF);
                writer.Write(value);
            }
        }

        public static void WriteFixed(this BinaryWriter writer, decimal value)
        {
            long D = 100_000_000;
            value *= D;
            writer.Write((long)value);
        }

        public static void Write<T>(this BinaryWriter writer, T[] value) where T : ISerializable
        {
            writer.WriteVarInt(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                value[i].Serialize(writer);
            }
        }

        public static void Write(this BinaryWriter writer, ISerializable value)
        {
            value.Serialize(writer);
        }

        public static Fixed8 Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, Fixed8> selector)
        {
            return source.Select(selector).Sum();
        }

        public static Fixed8 Sum(this IEnumerable<Fixed8> source)
        {
            long sum = 0;
            checked
            {
                foreach (Fixed8 item in source)
                {
                    sum += item.value;
                }
            }
            return new Fixed8(sum);
        }

        internal static int GetVarSize(int value)
        {
            if (value < 0xFD)
                return sizeof(byte);
            if (value <= 0xFFFF)
                return sizeof(byte) + sizeof(ushort);
            return sizeof(byte) + sizeof(uint);
        }

        internal static int GetVarSize<T>(this T[] value)
        {
            int valueSize;
            Type t = typeof(T);
            if (typeof(ISerializable).IsAssignableFrom(t))
            {
                valueSize = value.OfType<ISerializable>().Sum(p => p.Size);
            }
            else if (t.GetTypeInfo().IsEnum)
            {
                int elementSize;
                Type u = t.GetTypeInfo().GetEnumUnderlyingType();
                if (u == typeof(sbyte) || u == typeof(byte))
                    elementSize = 1;
                else if (u == typeof(short) || u == typeof(ushort))
                    elementSize = 2;
                else if (u == typeof(int) || u == typeof(uint))
                    elementSize = 4;
                else //if (u == typeof(long) || u == typeof(ulong))
                    elementSize = 8;
                valueSize = value.Length * elementSize;
            }
            else
            {
                valueSize = value.Length * Marshal.SizeOf<T>();
            }
            return GetVarSize(value.Length) + valueSize;
        }

        public static byte[] GetHashData(Transaction tx)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                tx.SerializeUnsigned(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        public static byte[] ToArray(this ISerializable value)
        {
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
            {
                value.Serialize(writer);
                writer.Flush();
                return ms.ToArray();
            }
        }
    }

    public class WalletException : Exception
    {
        public WalletException(string msg) : base(msg)
        {
        }
    }
}