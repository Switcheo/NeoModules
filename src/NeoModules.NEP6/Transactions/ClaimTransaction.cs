using System;
using System.Collections.Generic;
using System.Text;
using NeoModules.Core;

namespace NeoModules.NEP6.Transactions
{
    public class ClaimTransaction : Transaction
    {
        public ClaimTransaction() : base(TransactionType.ClaimTransaction)
        {
        }
        public CoinReference[] Claims;

        public override Fixed8 NetworkFee => Fixed8.Zero;
    }
}
