using NeoModules.RPC.DTOs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeoModulesXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionDetail : ContentPage
	{
		public TransactionDetail (Transaction transaction)
		{
			InitializeComponent ();
			BindingContext = transaction;
		}
	}
}