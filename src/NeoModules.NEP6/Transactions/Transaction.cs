using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Helpers;
using Helper = NeoModules.KeyPairs.Helper;

namespace NeoModules.NEP6.Transactions
{
    public class Transaction
    {
        /// <summary>
        /// Maximum number of attributes that can be contained within a transaction
        /// </summary>
        private const int MaxTransactionAttributes = 16;

        public readonly TransactionType Type;
        public byte Version;
        public TransactionAttribute[] Attributes;
        public CoinReference[] CoinReferences;
        public TransactionOutput[] Outputs;
        public Witness[] Witnesses { get; set; }

        private UInt256 _hash;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    var rawTx = Serialize(false);
                    _hash = new UInt256(Helper.Hash256(rawTx));
                }

                return _hash;
            }
        }

        public Dictionary<CoinReference, TransactionOutput> References { get; set; }

        public virtual Fixed8 SystemFee => Fixed8.Zero;

        private Fixed8 _network_fee = -Fixed8.Satoshi;
        public virtual Fixed8 NetworkFee
        {
            get
            {
                if (_network_fee == -Fixed8.Satoshi)
                {
                    Fixed8 input = References.Values.Where(p => p.AssetId.Equals(UInt256.Parse(Utils.GasToken))).Sum(p => p.Value);
                    Fixed8 output = Outputs.Where(p => p.AssetId.Equals(UInt256.Parse(Utils.GasToken).ToArray())).Sum(p => p.Value);
                    _network_fee = input - output - SystemFee;
                }
                return _network_fee;
            }
        }

        protected Transaction(TransactionType type)
        {
            Type = type;
        }

        void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Version);
            SerializeExclusiveData(writer);
            writer.Write(Attributes);
            writer.Write(Inputs);
            writer.Write(Outputs);
        }


        //TODO remove
        public byte[] Serialize(bool signed = true)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)Type);
                    writer.Write(Version);

                    //// exclusive data
                    //switch (Type)
                    //{
                    //    case TransactionType.InvocationTransaction:
                    //        {
                    //            writer.WriteVarInt(Script.Length);
                    //            writer.Write(Script);
                    //            if (Version >= 1) writer.WriteFixed(Gas);

                    //            break;
                    //        }
                    //    case TransactionType.ClaimTransaction:
                    //        {
                    //            writer.WriteVarInt(References.Length);
                    //            foreach (var entry in References)
                    //            {
                    //                SerializationHelper.SerializeTransactionInput(writer, entry);
                    //            }

                    //            break;
                    //        }
                    //}
                    // Don't need any attributes
                    if (Attributes != null)
                    {
                        writer.WriteVarInt(Attributes.Length);
                        foreach (var attr in Attributes)
                        {
                            attr.Serialize(writer);
                        }
                    }
                    else
                    {
                        writer.Write((byte)0);
                    }

                    writer.WriteVarInt(CoinReferences.Length);
                    foreach (var input in CoinReferences) SerializationHelper.SerializeTransactionInput(writer, input);

                    writer.WriteVarInt(Outputs.Length);
                    foreach (var output in Outputs) SerializationHelper.SerializeTransactionOutput(writer, output);

                    if (signed && Witnesses != null)
                    {
                        writer.WriteVarInt(Witnesses.Length);
                        foreach (var witness in Witnesses) witness.Serialize(writer);
                    }
                }
                return stream.ToArray();
            }
        }

        protected virtual void SerializeExclusiveData(BinaryWriter writer)
        {
        }

        public void Sign(KeyPair key)
        {
            var txdata = Serialize(false);

            var privkey = key.PrivateKey;
            var signature = Utils.Sign(txdata, privkey);

            var invocationScript = ("40" + signature.ToHexString()).HexToBytes();
            var verificationScript = Helper.CreateSignatureRedeemScript(key.PublicKey);
            Witnesses = new[]
            {
                new Witness
                {
                    InvocationScript = invocationScript,
                    VerificationScript = verificationScript
                }
            };
        }

        public void Sign(byte[] privateKey)
        {
            var txdata = Serialize(false);

            var signature = Utils.Sign(txdata, privateKey);

            var invocationScript = ("40" + signature.ToHexString()).HexToBytes();
            var verificationScript = Helper.CreateSignatureRedeemScript(new KeyPair(privateKey).PublicKey);
            Witnesses = new[]
            {
                new Witness
                {
                    InvocationScript = invocationScript,
                    VerificationScript = verificationScript
                }
            };
        }
    }
}