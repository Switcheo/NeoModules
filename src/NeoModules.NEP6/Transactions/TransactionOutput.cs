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
            throw new NotImplementedException();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(AssetId);
            writer.Write(Value);
            writer.Write(ScriptHash);
        }
    }
}
