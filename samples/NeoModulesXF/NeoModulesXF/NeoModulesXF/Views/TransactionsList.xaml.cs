using NeoModules.RPC.DTOs;
using NeoModulesXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeoModulesXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TransactionsList : ContentPage
	{
		private TransactionsViewModel Vm;

		public TransactionsList()
		{
			InitializeComponent();
			Vm = new TransactionsViewModel();
			BindingContext = Vm;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await Vm.GetTransactions();
		}

		private async void GoToTransactionDetail(object sender, SelectedItemChangedEventArgs e)
		{
			await Navigation.PushAsync(new TransactionDetail(Transactions.SelectedItem as Transaction));
			Transactions.SelectedItem = null;
		}
	}
}