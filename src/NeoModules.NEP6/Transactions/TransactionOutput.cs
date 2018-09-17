using System;
using System.IO;
using NeoModules.Core;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class TransactionOutput : ISerializable
    {
        public UInt256 AssetId;
        public Fixed8 Value;
        public UInt160 ScriptHash;
        public int Size => AssetId.Size + Value.Size + ScriptHash.Size;

        void ISerializable.Deserialize(BinaryReader reader)
        {
            AssetId = reader.ReadSerializable<UInt256>();
            Value = reader.ReadSerializable<Fixed8>();
            if (Value <= Fixed8.Zero) throw new FormatException();
            ScriptHash = reader.ReadSerializable<UInt160>();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
        }
    }
}
