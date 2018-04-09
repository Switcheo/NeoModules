using System;
using System.Collections.Generic;
using System.Text;
using NeoModules.Core;
using NeoModules.KeyPairs;
using NeoModules.NEP6.Models;
using Xunit;

namespace NeoModules.NEP6.Tests
{
    public class WalletManagerTester
    {
        [Fact]
        public static void CreateAndAddAccountTest()
        {
            var wallet = new Wallet("testwallet");
            var createdAccount = wallet.WalletManager.CreateAccount("Test Account");
            Assert.NotNull(createdAccount);
            Assert.Contains(createdAccount, wallet.Accounts);
        }

        [Fact]
        public static void DeleteAccountTest()
        {
            var walletJson =
                "{\"name\":\"MyWallet\",\"version\":\"1.0\",\"scrypt\":{\"n\":16384,\"r\":8,\"p\":8},\"accounts\":[{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"},{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"}],\"extra\":null}";
            var walletModel = Wallet.FromJson(walletJson);

            var scriptHash = KeyPairs.Helper.ToScriptHash("AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq");
            var accountToDelete = new Account(scriptHash);
            walletModel.WalletManager.DeleteAccount("AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq");
            Assert.DoesNotContain(accountToDelete, walletModel.Accounts);
        }

        [Fact]
        public static void ImportAccountTest()
        {
            var wallet = new Wallet("testwallet");
        }
    }
}
