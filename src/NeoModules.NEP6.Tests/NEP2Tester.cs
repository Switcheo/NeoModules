using NeoModules.Core;
using NeoModules.NEP6.Models;
using Xunit;

namespace NeoModules.NEP6.Tests
{
    public class Nep2Tester
    {
        [Fact]
        public static void EncryptTest1()
        {
            string password = "TestingOneTwoThree";
            var wif = "L44B5gGEpqEDRS9vVPz7QT35jcBG2r3CZwSwQ4fCewXAhAhqGVpP";

            var wifBytes = Wallet.GetPrivateKeyFromWif(wif);
            var wifHexString = wifBytes.ToHexString().ToUpper();

            var encrypted = Nep2.Encrypt(wif, password, ScryptParameters.Default).Result;

            Assert.Equal("CBF4B9F70470856BB4F40F80B87EDB90865997FFEE6DF315AB166D713AF433A5", wifHexString);
            Assert.Equal("6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL", encrypted);
        }

        [Fact]
        public static void EncryptTest2()
        {
            string password = "Satoshi";
            var wif = "KwYgW8gcxj1JWJXhPSu4Fqwzfhp5Yfi42mdYmMa4XqK7NJxXUSK7";

            var wifBytes = Wallet.GetPrivateKeyFromWif(wif);
            var wifHexString = wifBytes.ToHexString().ToUpper();

            var encrypted = Nep2.Encrypt(wif, password, ScryptParameters.Default).Result;

            Assert.Equal("09C2686880095B1A4C249EE3AC4EEA8A014F11E6F986D0B5025AC1F39AFBD9AE", wifHexString);
            Assert.Equal("6PYN6mjwYfjPUuYT3Exajvx25UddFVLpCw4bMsmtLdnKwZ9t1Mi3CfKe8S", encrypted);
        }

        [Fact]
        public static void DecryptTest1()
        {
            var decryptedWif = Nep2.Decrypt("6PYVPVe1fQznphjbUxXP9KZJqPMVnVwCx5s5pr5axRJ8uHkMtZg97eT5kL", "TestingOneTwoThree")
                .Result;

            var wifBytes = Wallet.GetPrivateKeyFromWif("L44B5gGEpqEDRS9vVPz7QT35jcBG2r3CZwSwQ4fCewXAhAhqGVpP");

            Assert.Equal(decryptedWif, wifBytes);
        }

        [Fact]
        public static void DecryptTest2()
        {
            var decryptedWif = Nep2.Decrypt("6PYN6mjwYfjPUuYT3Exajvx25UddFVLpCw4bMsmtLdnKwZ9t1Mi3CfKe8S", "Satoshi")
                .Result;

            var wifBytes = Wallet.GetPrivateKeyFromWif("KwYgW8gcxj1JWJXhPSu4Fqwzfhp5Yfi42mdYmMa4XqK7NJxXUSK7");

            Assert.Equal(decryptedWif, wifBytes);
        }
    }
}
