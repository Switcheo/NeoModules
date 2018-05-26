using System;
using System.IO;
using System.Linq;

namespace NeoModules.NEP6.Transactions
{
    public struct TransactionAttribute
    {
        public TransactionAttributeUsage Usage;
        public byte[] Data;

        public static TransactionAttribute Unserialize(BinaryReader reader)
        {
            var usage = (TransactionAttributeUsage) reader.ReadByte();

            byte[] data;

            if (usage == TransactionAttributeUsage.ContractHash || usage == TransactionAttributeUsage.Vote ||
                usage >= TransactionAttributeUsage.Hash1 && usage <= TransactionAttributeUsage.Hash15)
                data = reader.ReadBytes(32);
            else switch (usage)
            {
                case TransactionAttributeUsage.ECDH02:
                case TransactionAttributeUsage.ECDH03:
                    data = new[] {(byte) usage}.Concat(reader.ReadBytes(32)).ToArray();
                    break;
                case TransactionAttributeUsage.Script:
                    data = reader.ReadBytes(20);
                    break;
                case TransactionAttributeUsage.DescriptionUrl:
                    data = reader.ReadBytes(reader.ReadByte());
                    break;
                default:
                    if (usage == TransactionAttributeUsage.Description || usage >= TransactionAttributeUsage.Remark)
                        data = reader.ReadVarBytes(ushort.MaxValue);
                    else
                        throw new NotImplementedException();
                    break;
            }

            return new TransactionAttribute {Usage = usage, Data = data};
        }

        internal void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Usage);
            if (Usage == TransactionAttributeUsage.DescriptionUrl)
                writer.Write((byte)Data.Length);
            else if (Usage == TransactionAttributeUsage.Description || Usage >= TransactionAttributeUsage.Remark)
                writer.WriteVarInt(Data.Length);
            if (Usage == TransactionAttributeUsage.ECDH02 || Usage == TransactionAttributeUsage.ECDH03)
                writer.Write(Data, 1, 32);
            else
                writer.Write(Data);
        }
    }
}