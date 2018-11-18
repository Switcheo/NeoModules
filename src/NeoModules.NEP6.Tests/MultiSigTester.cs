using NeoModules.Core;
using NeoModules.Core.KeyPair;
using Xunit;

namespace NeoModules.NEP6.Tests
{
    public class MultiSigTester
    {
        [Fact]
        public void CreateMultiSigTest()
        {
            string[] pubKeyString = new[] {
                "036245f426b4522e8a2901be6ccc1f71e37dc376726cc6665d80c5997e240568fb",
                "0303897394935bb5418b1c1c4cf35513e276c6bd313ddd1330f113ec3dc34fbd0d",
                "02e2baf21e36df2007189d05b9e682f4192a101dcdf07eed7d6313625a930874b4"
            };

            var walletManager = new WalletManager("testwallet");

            var multiSigAccount = walletManager.CreateMultiSigAccount(2, pubKeyString, "testMultiSig");
            var verificationScript = multiSigAccount.Contract.Script.ToHexString();
            var address = "AJetuB7TxUkSmRNjot1G7FL5dDpNHE6QLZ";

            Assert.Equal(verificationScript,
                $"5221036245f426b4522e8a2901be6ccc1f71e37dc376726cc6665d80c5997e240568fb210303897394935bb5418b1c1c4cf35513e276c6bd313ddd1330f113ec3dc34fbd0d2102e2baf21e36df2007189d05b9e682f4192a101dcdf07eed7d6313625a930874b453ae");
            Assert.Equal(address, multiSigAccount.Address.ToAddress());
        }
    }
}
