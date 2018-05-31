using System;
using System.IO;
using NeoModules.NEP6.Transactions;

namespace NeoModules.NEP6.Helpers
{
    public static class SerializationHelper
    {
        public static void SerializeTransactionInput(BinaryWriter writer, SignedTransaction.Input input)
        {
            writer.Write(input.PrevHash);
            writer.Write((ushort) input.PrevIndex);
        }

        public static void SerializeTransactionOutput(BinaryWriter writer, SignedTransaction.Output output)
        {
            writer.Write(output.AssetId);
            writer.WriteFixed(output.Value);
            writer.Write(output.ScriptHash);
        }

        public static SignedTransaction.Input UnserializeTransactionInput(BinaryReader reader)
        {
            var prevHash = reader.ReadBytes(32);
            var prevIndex = reader.ReadUInt16();
            return new SignedTransaction.Input {PrevHash = prevHash, PrevIndex = prevIndex};
        }

        public static SignedTransaction.Output UnserializeTransactionOutput(BinaryReader reader)
        {
            var assetId = reader.ReadBytes(32);
            var value = reader.ReadFixed();
            var scriptHash = reader.ReadBytes(20);
            return new SignedTransaction.Output {AssetId = assetId, Value = value, ScriptHash = scriptHash};
        }

        public static SignedTransaction Unserialize(BinaryReader reader)
        {
            var tx = new SignedTransaction
            {
                Type = (TransactionType) reader.ReadByte(),
                Version = reader.ReadByte()
            };


            switch (tx.Type)
            {
                case TransactionType.InvocationTransaction:
                {
                    //todo
                    break;
                }

                case TransactionType.MinerTransaction:
                {
                    //todo
                    break;
                }

                case TransactionType.ClaimTransaction:
                {
                    var len = (int) reader.ReadVarInt(0x10000000);
                    tx.References = new SignedTransaction.Input[len];
                    for (var i = 0; i < len; i++) tx.References[i] = UnserializeTransactionInput(reader);

                    break;
                }

                case TransactionType.ContractTransaction:
                {
                    break;
                }

                case TransactionType.PublishTransaction:
                {
                    //todo
                    break;
                }

                case TransactionType.EnrollmentTransaction:
                {
                    //todo
                    break;
                }

                case TransactionType.RegisterTransaction:
                {
                    //todo
                    break;
                }

                case TransactionType.IssueTransaction:
                {
                    break;
                }

                case TransactionType.StateTransaction:
                    break;
                default:
                {
                    throw new NotImplementedException();
                }
            }

            var attrCount = (int) reader.ReadVarInt(16);
            if (attrCount != 0)
            {
                tx.Attributes = new TransactionAttribute[attrCount];
                for (var i = 0; i < attrCount; i++) tx.Attributes[i] = TransactionAttribute.Unserialize(reader);
            }

            var inputCount = (int) reader.ReadVarInt();
            tx.Inputs = new SignedTransaction.Input[inputCount];
            for (var i = 0; i < inputCount; i++) tx.Inputs[i] = UnserializeTransactionInput(reader);

            var outputCount = (int) reader.ReadVarInt();
            tx.Outputs = new SignedTransaction.Output[outputCount];
            for (var i = 0; i < outputCount; i++) tx.Outputs[i] = UnserializeTransactionOutput(reader);

            var witnessCount = (int) reader.ReadVarInt();
            tx.Witnesses = new Witness[witnessCount];
            for (var i = 0; i < witnessCount; i++) tx.Witnesses[i] = Witness.Unserialize(reader);

            return tx;
        }

        public static SignedTransaction Unserialize(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(stream))
                {
                    return Unserialize(reader);
                }
            }
        }
    }
}