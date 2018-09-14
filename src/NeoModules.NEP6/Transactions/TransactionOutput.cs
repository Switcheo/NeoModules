using NeoModules.Core;

namespace NeoModules.NEP6.Transactions
{
    public class TransactionOutput
    {
        public UInt256 AssetId;
        public Fixed8 Value;
        public UInt160 ScriptHash;
    }
}
