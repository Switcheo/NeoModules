namespace NeoModules.NEP6.Transactions
{
    public class ContractTransaction : Transaction
    {
        public ContractTransaction()
            : base(TransactionType.ContractTransaction)
        {
        }
    }
}