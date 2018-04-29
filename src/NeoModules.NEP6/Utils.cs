using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NeoModules.NVM;

namespace NeoModules.NEP6
{
    public static class Utils
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static byte[] Sign(byte[] message, byte[] prikey, byte[] pubkey)
        {
            using (var ecdsa = ECDsa.Create(new ECParameters
            {
                Curve = ECCurve.NamedCurves.nistP256,
                D = prikey,
                Q = new ECPoint
                {
                    X = pubkey.Take(32).ToArray(),
                    Y = pubkey.Skip(32).ToArray()
                }
            }))
            {
                return ecdsa.SignData(message, HashAlgorithmName.SHA256);
            }
        }

        public static string ReverseHex(string hex)
        {
            var result = "";
            for (var i = hex.Length - 2; i >= 0; i -= 2) result += hex.Substring(i, 2);
            return result;
        }

        public static uint ToTimestamp(this DateTime time)
        {
            return (uint)(time.ToUniversalTime() - UnixEpoch).TotalSeconds;
        }

        public static byte[] GenerateScript(byte[] scriptHash, object[] args)
        {
            using (var sb = new ScriptBuilder())
            {
                var items = new Stack<object>();

                if (args != null)
                {
                    foreach (var item in args)
                    {
                        items.Push(item);
                    }
                }

                while (items.Count > 0)
                {
                    var item = items.Pop();
                    Helper.EmitObject(sb, item);
                }

                sb.EmitAppCall(scriptHash, false);

                var timestamp = DateTime.UtcNow.ToTimestamp();
                var nonce = BitConverter.GetBytes(timestamp);

                sb.Emit(OpCode.RET);
                sb.EmitPush(nonce);

                var bytes = sb.ToArray();
                return bytes;
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