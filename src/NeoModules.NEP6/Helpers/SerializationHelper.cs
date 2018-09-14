using System.IO;
using NeoModules.NEP6.Transactions;

namespace NeoModules.NEP6.Helpers
{
    public static class SerializationHelper
    {
        public static void SerializeTransactionInput(BinaryWriter writer, CoinReference coinReference)
        {
            writer.Write(coinReference.PrevHash);
            writer.Write((ushort) coinReference.PrevIndex);
        }

        public static void SerializeTransactionOutput(BinaryWriter writer,TransactionOutput transactionOutput)
        {
            writer.Write(transactionOutput.AssetId);
            writer.WriteFixed(transactionOutput.Value);
            writer.Write(transactionOutput.ScriptHash);
        }
    }
}