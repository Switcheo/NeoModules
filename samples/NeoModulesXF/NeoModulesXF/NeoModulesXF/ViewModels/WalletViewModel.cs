using System;
using System.Collections.Generic;
using System.Text;
using NeoModules.NEP6;
using NeoModules.NEP6.Models;
using PropertyChanged;

namespace NeoModulesXF.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WalletViewModel : BaseViewModel
    {
        public Wallet TestWallet { get; set; }
        public WalletManager TestWalletManager { get; set; }

        public WalletViewModel()
        {
            TestWallet = new Wallet("test wallet");
            TestWalletManager = new WalletManager(TestWallet);
        }
    }
}