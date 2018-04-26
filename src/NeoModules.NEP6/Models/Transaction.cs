using System;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6.Models
{
    public class Transaction
    {
        public decimal Gas;

        public Input[] Inputs;
        public Output[] Outputs;
        public byte[] Script;

        public byte Type;
        public byte Version;
        public Witness[] Witnesses;

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
                if (tx.Version >= 1) result.Append(Fixed8.FromDecimal(tx.Gas));
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

            var privkey = key.PrivateKey;
            var pubkey = key.PublicKey.EncodePoint(true); //todo validate this encodingpoint
            var signature = Utils.Sign(txstr, privkey, pubkey);

            var invocationScript = "40" + signature.ToHexString();
            var verificationScript = Helper.CreateSignatureRedeemScript(key.PublicKey).ToHexString();
            Witnesses = new[]
            {
                new Witness {InvocationScript = invocationScript, VerificationScript = verificationScript}
            };
        }

        public struct Witness
        {
            public string InvocationScript;
            public string VerificationScript;
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

        protected static string Num2Hexstring(long num, int size = 2)
        {
            return num.ToString("X" + size);
        }

        protected static string Num2VarInt(long num)
        {
            if (num < 0xfd) return Num2Hexstring(num);

            if (num <= 0xffff) return "fd" + Num2Hexstring(num, 4);

            if (num <= 0xffffffff) return "fe" + Num2Hexstring(num, 8);

            return "ff" + Num2Hexstring(num, 8) + Num2Hexstring(num / (int) Math.Pow(2, 32), 8);
        }

        protected static string SerializeWitness(Witness witness)
        {
            var invoLength = Num2Hexstring(witness.InvocationScript.Length / 2);
            var veriLength = Num2Hexstring(witness.VerificationScript.Length / 2);
            return invoLength + witness.InvocationScript + veriLength + witness.VerificationScript;
        }

        protected static string SerializeTransactionInput(Input input)
        {
            return Utils.ReverseHex(input.PrevHash) + Utils.ReverseHex(Num2Hexstring(input.PrevIndex, 4));
        }

        protected static string SerializeTransactionOutput(Output output)
        {
            var value = Fixed8.FromDecimal(output.Value);
            return Utils.ReverseHex(output.AssetId) + value + Utils.ReverseHex(output.ScriptHash);
        }

        #endregion
    }
}