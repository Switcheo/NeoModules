using System.Threading.Tasks;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using Xunit;

namespace NeoModules.NEP6.Tests
{
    public class WalletManagerTester
    {
        private static string walletJson =
            "{\"name\":\"MyWallet\",\"version\":\"1.0\",\"scrypt\":{\"n\":16384,\"r\":8,\"p\":8},\"accounts\":[{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"},{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"}],\"extra\":null}";

        [Fact]
        public static void CreateAndAddAccountTest()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var createdAccount = walletManager.CreateAccount("Test Account");
            Assert.NotNull(createdAccount);
            Assert.Contains(createdAccount, wallet.Accounts);
        }

        [Fact]
        public static void DeleteAccountByAddressTest()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var scriptHash = "AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq".ToScriptHash();
            var accountToDelete = new Account(scriptHash);
            walletManager.AddAccount(accountToDelete);
            walletManager.DeleteAccount("AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq");

            Assert.DoesNotContain(accountToDelete, wallet.Accounts);
        }

        [Fact]
        public static void DeleteAccountTest()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var createdAccount = walletManager.CreateAccount("Test Account");
            walletManager.AddAccount(createdAccount);
            walletManager.DeleteAccount(createdAccount);

            Assert.DoesNotContain(createdAccount, wallet.Accounts);
        }

        [Fact]
        public static void GetAccountTest()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var account = walletManager.GetAccount("AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq");
            Assert.NotNull(account);
        }

        [Fact]
        public static void ImportAccountNep2Test()
        {
            Wallet wallet = new Wallet();
            WalletManager walletManager = new WalletManager(wallet);

            var account = walletManager.ImportAccount("6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL",
                "TestingOneTwoThree", "testAccount").Result;

            Assert.NotNull(account);
            Assert.Contains(account, wallet.Accounts);
        }

        [Fact]
        public static void ImportAccountWifTest()
        {
            Wallet wallet = new Wallet();
            WalletManager walletManager = new WalletManager(wallet);

            var account =
                walletManager.ImportAccount("L44B5gGEpqEDRS9vVPz7QT35jcBG2r3CZwSwQ4fCewXAhAhqGVpP", "accoun1"); //wif
            var account2 = walletManager.ImportAccount("6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL",
                "TestingOneTwoThree", "account2").Result;

            Assert.Equal(account2.Address, account.Address); // make sure the importing from wif and importing from nep2 returns the same account
            var address = "AStZHy8E6StCqYQbzMqi4poH7YNDHQKxvt"; // account address
            var addressScript = address.ToScriptHash(); // address to UInt160

            Assert.NotNull(account);
            Assert.Equal(account.Address, addressScript);
            Assert.Contains(account, wallet.Accounts);
        }

        [Fact]
        public static async Task GetKeyTestAsync()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var password = "TestingOneTwoThree";
            var wif = "L44B5gGEpqEDRS9vVPz7QT35jcBG2r3CZwSwQ4fCewXAhAhqGVpP";
            var wifBytes = Wallet.GetPrivateKeyFromWif(wif);
            KeyPair expectedKeyPair = new KeyPair(wifBytes);

            KeyPair key =
                await walletManager.GetKey("6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL", password);

            Assert.Equal(key.PrivateKey, expectedKeyPair.PrivateKey);
        }

        [Fact]
        public static async Task VerifyPassword()
        {
            Wallet wallet = Wallet.FromJson(walletJson);
            WalletManager walletManager = new WalletManager(wallet);

            var nep2Key = "6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL";

            var password = "TestingOneTwoThree";
            var wrongPassword = "Testing";

            bool correct = await walletManager.VerifyPassword(nep2Key, password);
            bool incorrect = await walletManager.VerifyPassword(nep2Key, wrongPassword);
            Assert.True(correct);
            Assert.False(incorrect);
        }
    }
}