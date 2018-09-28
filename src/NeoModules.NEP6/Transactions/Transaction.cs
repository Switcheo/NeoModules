using System;
using System.IO;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.Core.NVM;
using NeoModules.NEP6.Helpers;
using Helper = NeoModules.Core.KeyPair.Helper;
using Utils = NeoModules.NEP6.Helpers.Utils;

namespace NeoModules.NEP6.Transactions
{
    public class Transaction : ISerializable
    {
        public readonly TransactionType Type;
        public byte Version;
        public TransactionAttribute[] Attributes;
        public CoinReference[] Inputs;
        public TransactionOutput[] Outputs;
        public Witness[] Witnesses { get; set; }

        private UInt256 _hash;
        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    _hash = new UInt256(Helper.Hash256(Utils.GetHashData(this)));
                }

                return _hash;
            }
        }

        public virtual Fixed8 SystemFee => Fixed8.Zero;

        private Fixed8 _network_fee = -Fixed8.Satoshi;

        public virtual Fixed8 NetworkFee
        {
            get
            {
                //if (_network_fee == -Fixed8.Satoshi)
                //{
                //    Fixed8 input = References.Values.Where(p => p.AssetId.Equals(Utils.GasToken))
                //        .Sum(p => p.Value);
                //    Fixed8 output = Outputs.Where(p => p.AssetId.Equals(Utils.GasToken)).ToArray()
                //        .Sum(p => p.Value);
                //    _network_fee = input - output - SystemFee;
                //}

                return _network_fee;//todo
            }
        }

        public virtual int Size => sizeof(TransactionType) + sizeof(byte) + Attributes.GetVarSize() + Inputs.GetVarSize() + Outputs.GetVarSize() + Witnesses.GetVarSize();

        protected Transaction(TransactionType type)
        {
            Type = type;
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            SerializeUnsigned(writer);
            writer.Write(Witnesses);
        }

        internal void SerializeUnsigned(BinaryWriter writer)
        {
            writer.Write((byte)Type);
            writer.Write(Version);
            SerializeExclusiveData(writer);
            writer.Write(Attributes);
            writer.Write(Inputs);
            writer.Write(Outputs);
        }

        void ISerializable.Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected virtual void SerializeExclusiveData(BinaryWriter writer)
        {
        }

        public byte[] Sign(KeyPair key)
        {
            byte[] txdata = Utils.GetHashData(this);

            var signature = Utils.Sign(txdata, key.PrivateKey);
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

            var signedTx = this.ToArray();
            return signedTx;
        }

        public byte[] Sign(byte[] privateKey)
        {
            return Sign(new KeyPair(privateKey));
        }
    }
}