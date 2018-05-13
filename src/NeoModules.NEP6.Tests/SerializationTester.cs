using NeoModules.NEP6.Models;
using Xunit;

namespace NeoModules.NEP6.Tests
{
    public class SerializationTester
    {
        [Fact]
        public static void Nep6AccountSerializationTest()
        {
            var accountJson =
                "{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"}";
            var accountModel = Account.FromJson(accountJson);

            var deserializeAccount = Account.ToJson(accountModel);
            Assert.Equal(accountJson, deserializeAccount);
        }

        [Fact]
        public static void Nep6ContractSerializationTest()
        {
            var contractJson =
                "{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false}";
            var contractModel = Contract.FromJson(contractJson);

            var deserializeContract = Contract.ToJson(contractModel);
            Assert.Equal(contractJson, deserializeContract);
        }

        [Fact]
        public static void Nep6ParameterSerializationTest()
        {
            var parameterJson = "{\"name\":\"from\",\"type\":\"Hash160\"}";
            var parameterModel = Parameter.FromJson(parameterJson);

            var deserializeParameter = Parameter.ToJson(parameterModel);
            Assert.Equal(parameterJson, deserializeParameter);
        }

        [Fact]
        public static void Nep6WalletFromFileTest()
        {
            var wallet = WalletManager.LoadFromFile("walletFile.json");
            Assert.NotNull(wallet);
        }

        [Fact]
        public static void Nep6WalletSerializationTest()
        {
            var walletJson =
                "{\"name\":\"MyWallet\",\"version\":\"1.0\",\"scrypt\":{\"n\":16384,\"r\":8,\"p\":8},\"accounts\":[{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"},{\"address\":\"AQLASLtT6pWbThcSCYU1biVqhMnzhTgLFq\",\"label\":\"MyAddress\",\"isDefault\":true,\"lock\":false,\"key\":\"6PYWB8m1bCnu5bQkRUKAwbZp2BHNvQ3BQRLbpLdTuizpyLkQPSZbtZfoxx\",\"contract\":{\"script\":\"21036dc4bf8f0405dcf5d12a38487b359cb4bd693357a387d74fc438ffc7757948b0ac\",\"parameters\":[{\"name\":\"from\",\"type\":\"Hash160\"},{\"name\":\"from\",\"type\":\"Hash160\"}],\"deployed\":false},\"extra\":\"test string extra\"}],\"extra\":null}";
            var walletModel = Wallet.FromJson(walletJson);

            var deserializeWallet = Wallet.ToJson(walletModel);
            Assert.Equal(walletJson, deserializeWallet);
        }
    }
}