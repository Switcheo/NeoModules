using System.IO;
using NeoModules.Core;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class CoinReference : ISerializable
    {
        public UInt256 PrevHash;
        public uint PrevIndex;
        public int Size => PrevHash.Size + sizeof(ushort);

        void ISerializable.Deserialize(BinaryReader reader)
        {
            PrevHash = reader.ReadSerializable<UInt256>();
            PrevIndex = reader.ReadUInt16();
        }

        void ISerializable.Serialize(BinaryWriter writer)
        {
            writer.Write(PrevHash);
            writer.Write(PrevIndex);
        }
    }
}
