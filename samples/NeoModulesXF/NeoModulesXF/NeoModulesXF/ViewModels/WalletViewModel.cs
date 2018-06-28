using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using NeoModules.Rest.DTOs;
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
        public ICommand GetBalanceCommand => new Command(async () => await GetBalanceExecute());

        public string Wif { get; set; } = "L4qi4yPsNU2m6RtrWPeponmXm1fD12vThQefursdFkSVFASBudUV";//do not use this account on main net!

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
            var account = TestWalletManager.ImportAccount(Wif, "demo");
            count++;
            Accounts.Add(account);
        }

        private async Task GetBalanceExecute()
        {
            foreach (var account in Accounts)
            {
                var address = Wallet.ToAddress(account.Address);

                //use this for native assets if you don't want NEP5 balance
                var naviteAssetsBalance = await NeoService.Accounts.GetAccountState.SendRequestAsync(address);

                //use this for all assets
                var json = await NeoScanService.GetBalanceAsync(address);
                var jsonBalance = AddressBalance.FromJson(json);

                if (jsonBalance.Balance.Count != 0) //just on value for DEMO
                {
                    account.Extra = new[]
                    {
                        new
                        {
                            jsonBalance.Balance[0].Asset,
                            jsonBalance.Balance[0].Amount,
                        }
                    };
                }
            }
        }
    }
}