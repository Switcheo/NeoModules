using System;
using System.Linq;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.RPC.DTOs;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6.Models
{
    public class TransactionInput
    {
        private UInt256 _hash;

        public decimal Gas;

        public Input[] Inputs;
        public Output[] Outputs;
        public byte[] Script;

        public byte Type;
        public byte Version;
        public Script[] Witnesses;

        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    var rawTx = Serialize(false);
                    var bytes = rawTx.HexToBytes();
                    _hash = new UInt256(Helper.Hash256(bytes));
                }

                return _hash;
            }
        }


        public virtual string Serialize(bool signed = true)
        {
            var tx = this;
            var result = new StringBuilder();
            result.Append(Num2Hexstring(tx.Type));
            result.Append(Num2Hexstring(tx.Version));

            // excluusive data
            if (tx.Type == 0xd1)
            {
                result.Append(Num2VarInt(tx.Script.Length));
                result.Append(tx.Script.ToHexString());
                if (tx.Version >= 1) result.Append(Num2Fixed8(tx.Gas));
            }

            // Don't need any attributes
            result.Append("00");

            result.Append(Num2VarInt(tx.Inputs.Length));
            foreach (var input in tx.Inputs) result.Append(SerializeTransactionInput(input));

            result.Append(Num2VarInt(tx.Outputs.Length));
            foreach (var output in tx.Outputs) result.Append(SerializeTransactionOutput(output));


            if (signed && tx.Witnesses != null && tx.Witnesses.Length > 0)
            {
                result.Append(Num2VarInt(tx.Witnesses.Length));
                foreach (var witnessScript in tx.Witnesses) result.Append(SerializeWitness(witnessScript));
            }

            return result.ToString().ToLowerInvariant();
        }

        public void Sign(KeyPair key)
        {
            var txdata = Serialize(false);
            var txstr = txdata.HexToBytes();

            byte[] signed;
            using (key.Decrypt())
            {
                signed = Utils.Sign(txstr, key.PrivateKey, key.PublicKey.EncodePoint(false).Skip(1).ToArray());
            }

            var invocationScript = "40" + signed.ToHexString();
            var verificationScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToHexString();
            Witnesses = new[]
            {
                new Script {Invocation = invocationScript, Verification = verificationScript}
            };
        }

        public struct Input
        {
            public string PrevHash;
            public uint PrevIndex;
        }

        public struct Output
        {
            public string ScriptHash;
            public string AssetId;
            public decimal Value;
        }

        #region HELPERS

        private static string Num2Hexstring(long num, int size = 2)
        {
            return num.ToString("X" + size);
        }

        private static string Num2VarInt(long num)
        {
            if (num < 0xfd) return Num2Hexstring(num);

            if (num <= 0xffff) return "fd" + Num2Hexstring(num, 4);

            if (num <= 0xffffffff) return "fe" + Num2Hexstring(num, 8);

            return "ff" + Num2Hexstring(num, 8) + Num2Hexstring(num / (int) Math.Pow(2, 32), 8);
        }

        private static string SerializeWitness(Script witness)
        {
            var invoLength = Num2Hexstring(witness.Invocation.Length / 2);
            var veriLength = Num2Hexstring(witness.Verification.Length / 2);
            return invoLength + witness.Invocation + veriLength + witness.Verification;
        }

        private static string SerializeTransactionInput(Input input)
        {
            return Utils.ReverseHex(input.PrevHash) + Utils.ReverseHex(Num2Hexstring(input.PrevIndex, 4));
        }

        private static string SerializeTransactionOutput(Output output)
        {
            var value = Num2Fixed8(output.Value);
            return Utils.ReverseHex(output.AssetId) + value + Utils.ReverseHex(output.ScriptHash);
        }

        private static string Num2Fixed8(decimal num)
        {
            var val = (long) Math.Round(num * 100000000);
            var hexValue = val.ToString("X16");
            return Utils.ReverseHex(("0000000000000000" + hexValue).Substring(hexValue.Length));
        }

        #endregion
    }
}