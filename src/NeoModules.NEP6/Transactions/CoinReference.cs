using System;
using System.IO;
using NeoModules.Core;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class CoinReference : ISerializable
    {
        public UInt256 PrevHash;
        public ushort PrevIndex;
        public int Size => PrevHash.Size + sizeof(ushort);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            return PrevHash.GetHashCode() + PrevIndex.GetHashCode();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(PrevIndex);
        }
    }
}
