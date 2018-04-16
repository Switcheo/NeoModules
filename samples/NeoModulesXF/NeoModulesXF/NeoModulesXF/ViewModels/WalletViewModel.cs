using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace NeoModulesXF.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WalletViewModel : BaseViewModel
    {
        public Wallet TestWallet { get; set; }
        public WalletManager TestWalletManager { get; set; }

        [DependsOn("TestWalletManager.Accounts")]
        public ObservableCollection<Account> Accounts { get; set; }

        public ICommand CreateNewAccountCommand => new Command(CreateNewAccountExecute);
        public ICommand DeleteAccountCommand => new Command<Account>(DeleteAccountExecute);
        public ICommand ImportEncryptedAccountCommand => new Command(async () => await ImportEncryptedAccountExecute());

        public ICommand ImportWifAccountCommand => new Command(ImportWifAccountExecute);

        public string Wif { get; set; } = "KwYgW8gcxj1JWJXhPSu4Fqwzfhp5Yfi42mdYmMa4XqK7NJxXUSK7";

        public string EncryptedKey { get; set; } =
            "6PYN6mjwYfjPUuYT3Exajvx25UddFVLpCw4bMsmtLdnKwZ9t1Mi3CfKe8S"; //do not use this account on main net!

        public string Password { get; set; } = "Satoshi";

        private int count = 0;

        public WalletViewModel()
        {
            TestWallet = new Wallet("test wallet");
            TestWalletManager = new WalletManager(TestWallet);
            Accounts = new ObservableCollection<Account>(TestWallet.Accounts);
        }

        private void CreateNewAccountExecute()
        {
            var account = TestWalletManager.CreateAccount("TestAccount" + count);
            Accounts.Add(account);
            count++;
        }

        private void DeleteAccountExecute(Account a)
        {
            TestWalletManager.DeleteAccount(a);
            Accounts.Remove(a);
        }

        private async Task ImportEncryptedAccountExecute()
        {
            if (!string.IsNullOrEmpty(EncryptedKey) && !string.IsNullOrEmpty(Password))
            {
                var account = await TestWalletManager.ImportAccount(EncryptedKey, Password, "ImportedAccount" + count);
                count++;
                Accounts.Add(account);
            }
        }

        private void ImportWifAccountExecute()
        {
            if (string.IsNullOrEmpty(Wif)) return;
            var account = TestWalletManager.ImportAccount(Wif);
            count++;
            Accounts.Add(account);
        }
    }
}