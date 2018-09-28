using System.IO;
using NeoModules.Core;
using NeoModules.NEP6.Helpers;

namespace NeoModules.NEP6.Transactions
{
    public class ClaimTransaction : Transaction
    {
        public ClaimTransaction() : base(TransactionType.ClaimTransaction)
        {
        }

        public CoinReference[] Claims;

        public override Fixed8 NetworkFee => Fixed8.Zero;

        protected override void SerializeExclusiveData(BinaryWriter writer)
        {
            writer.Write(Claims);
        }
    }
}
