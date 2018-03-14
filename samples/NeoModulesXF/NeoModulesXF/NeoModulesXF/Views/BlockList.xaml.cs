using NeoModules.RPC.DTOs;
using NeoModulesXF.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeoModulesXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlockList : ContentPage
	{
		private BlocksViewModel Vm;
		public BlockList()
		{
			InitializeComponent();
			Vm =  new BlocksViewModel();
			BindingContext = Vm;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await Vm.GetBlocks();
		}

		private async void GoToBlockDetail(object sender, SelectedItemChangedEventArgs e)
		{
			await Navigation.PushAsync(new BlockDetailPage(Blocks.SelectedItem as Block));
			Blocks.SelectedItem = null;
		}
	}
}