using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NVM;
using Helper = NeoModules.NVM.Helper;

namespace NeoModules.NEP6
{
    public static class Utils
    {
        private static readonly string NeoToken = "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b";
        private static readonly string GasToken = "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7";
        public static Dictionary<string, string> _systemAssets;
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal static byte[] Sign(byte[] message, byte[] prikey, byte[] pubkey)
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

        internal static Dictionary<string, string> GetAssetsInfo() //TODO redo this
        {
            if (_systemAssets != null) return _systemAssets;
            _systemAssets = new Dictionary<string, string>();
            AddAsset("NEO", "c56f33fc6ecfcd0c225c4ab356fee59390af8560be0e930faebe74a6daff7c9b");
            AddAsset("GAS", "602c79718b16e442de58778e148d0b1084e3b2dffd5de6b7b16cee7969282de7");

            return _systemAssets;
        }

        private static void AddAsset(string symbol, string hash)
        {
            _systemAssets[symbol] = hash;
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

        public static byte[] GenerateScript(byte[] scriptHash, object[] args) // todo redo generateScript
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

        public static byte[] GenerateScript(byte[] scriptHash, string operation, object[] args)
        {
            var script = scriptHash.ToScriptHash();
            using (var sb = new ScriptBuilder())
            {
                if (args != null)
                {
                    sb.EmitAppCall(script, operation, args);
                }
                else
                {
                    sb.EmitAppCall(script, operation);
                }
                

                //TODO: not sure about this
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