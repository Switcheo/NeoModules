using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NVM;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Helper = NeoModules.NVM.Helper;

namespace NeoModules.NEP6
{
    public static class Utils
    {
        private static readonly string NeoToken = "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b";
        private static readonly string GasToken = "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7";
        public static Dictionary<string, string> _systemAssets;
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

        public static string ReverseHex(string hex)
        {
            var result = "";
            for (var i = hex.Length - 2; i >= 0; i -= 2) result += hex.Substring(i, 2);
            return result;
        }

        public static uint ToTimestamp(this DateTime time)
        {
            return (uint) (time.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        public static byte[]
            GenerateScript(byte[] scriptHash, string operation, object[] args) // TODO: this does not work correctly
        {
            var script = new UInt160(scriptHash);
            using (var sb = new ScriptBuilder())
            {
                if (args != null)
                    sb.EmitAppCall(script, operation, args);
                else
                    sb.EmitAppCall(script, operation);

                var timestamp = DateTime.UtcNow.ToTimestamp();
                var nonce = BitConverter.GetBytes(timestamp);

                sb.Emit(OpCode.RET);
                sb.EmitPush(nonce);

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
            byte fb = reader.ReadByte();
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

        public static string ReadVarString(this BinaryReader reader)
        {
            return Encoding.UTF8.GetString(reader.ReadVarBytes());
        }

        public static void WriteVarBytes(this BinaryWriter writer, byte[] value)
        {
            writer.WriteVarInt(value.Length);
            writer.Write(value);
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

        public static void WriteVarString(this BinaryWriter writer, string value)
        {
            writer.WriteVarBytes(Encoding.UTF8.GetBytes(value));
        }

        public static void WriteFixed(this BinaryWriter writer, decimal value)
        {
            long D = 100_000_000;
            value *= D;
            writer.Write((long)value);
        }

        public static decimal ReadFixed(this BinaryReader reader)
        {
            var val = reader.ReadInt64();
            long D = 100_000_000;
            decimal r = val;
            r /= D;
            return r;
        }
    }

    public class WalletException : Exception
    {
        public WalletException(string msg) : base(msg)
        {
        }
    }
}