using System.IO;
using NeoModules.Core;

namespace NeoModules.NEP6.Transactions
{
    public class InvocationTransaction : Transaction
    {
        public Fixed8 Gas;
        public byte[] Script;

        protected InvocationTransaction() : base(TransactionType.InvocationTransaction)
        {
        }

        public override Fixed8 SystemFee => Gas;

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.WriteVarBytes(Script);
            if (Version >= 1) writer.Write(Gas.value);
        }
    }
}