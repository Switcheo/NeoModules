namespace NeoModules.NEP6.Transactions
{
    public class TransferOutput
    {
        public byte[] AddressHash { get; set; }
        public byte[] AssetId { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
    }
}
