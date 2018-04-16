using System;
using System.Collections.ObjectModel;
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
    }
}