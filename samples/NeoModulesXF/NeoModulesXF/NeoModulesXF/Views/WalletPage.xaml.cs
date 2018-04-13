using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeoModulesXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeoModulesXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WalletPage : ContentPage
	{

	    private WalletViewModel Vm;

        public WalletPage ()
		{
			InitializeComponent ();
		    Vm = new WalletViewModel();
		    BindingContext = Vm;
        }

	    protected override async void OnAppearing()
	    {
	        base.OnAppearing();
	    }
    }
}