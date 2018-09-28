using System.IO;
using NeoModules.Core;
using NeoModules.Core.NVM;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class InvocationTransaction : Transaction
    {
        public Fixed8 Gas;
        public byte[] Script;

        public InvocationTransaction() : base(TransactionType.InvocationTransaction)
        {
        }

        public override int Size => base.Size + Script.GetVarSize();

        public override Fixed8 SystemFee => Gas;

        public static Fixed8 GetGas(Fixed8 consumed)
        {
            var gas = consumed - Fixed8.FromDecimal(10);
            if (gas <= Fixed8.Zero) return Fixed8.Zero;
            return gas.Ceiling();
        }

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.WriteVarBytes(Script);
            if (Version >= 1) writer.Write(Gas.value);
        }
    }
}