using System;
using System.IO;
using System.Linq;
using NeoModules.Core;
using NeoModules.Core.KeyPair;
using NeoModules.NEP6.Helpers;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Serializes a transaction (without witnesses).
        /// Returns a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] SerializeTransaction()
        {
            return Utils.GetHashData(this);
        }

        /// <summary>
        /// Serializes and sign the transaction with the given private key.
        /// Return a byte array.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="signed"></param>
        /// <returns></returns>
        public byte[] Sign(KeyPair key, bool signed = true)
        {
            return Utils.Sign(key, this, signed);
        }

        public byte[] Sign(byte[] privateKey, bool signed = true)
        {
            return Utils.Sign(new KeyPair(privateKey), this, signed);
        }

        /// <summary>
        /// Creates a Transaction object from json string.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Transaction FromJson(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            var tx = new InvocationTransaction();
            if ((TransactionType)Enum.Parse(typeof(TransactionType),
                    jsonObject["type"].ToString()) == TransactionType.InvocationTransaction)
            {
                tx = new InvocationTransaction
                {
                    Attributes = jsonObject["attributes"]
                        .Select(atttribute => new TransactionAttribute
                        {
                            Data = atttribute["data"].ToString().HexToBytes(),
                            Usage = (TransactionAttributeUsage)Enum.Parse(typeof(TransactionAttributeUsage), atttribute["usage"].ToString())
                        }).ToArray(),
                    Version = jsonObject["version"].ToObject<byte>(),
                    Inputs = jsonObject["inputs"]
                       .Select(input => new CoinReference
                       {
                           PrevIndex = input["prevIndex"].ToObject<ushort>(),
                           PrevHash = UInt256.Parse(input["prevHash"].ToString())
                       }).ToArray(),
                    Outputs = jsonObject["outputs"]
                       .Select(output => new TransactionOutput
                       {
                           AssetId = UInt256.Parse(output["assetId"].ToString()),
                           Value = Fixed8.FromDecimal((decimal)output["value"]),
                           ScriptHash = UInt160.Parse(output["scriptHash"].ToString())
                       }).ToArray(),
                    Script = jsonObject["script"].ToString().HexToBytes(),
                    _hash = UInt256.Parse(jsonObject["hash"].ToString()),
                };
            }

            return tx;
        }
    }
}