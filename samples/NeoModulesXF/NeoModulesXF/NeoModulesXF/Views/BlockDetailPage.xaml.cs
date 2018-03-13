using NeoModules.RPC.DTOs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NeoModulesXF.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BlockDetailPage : ContentPage
	{
		public BlockDetailPage(Block block)
		{
			InitializeComponent ();
			BindingContext = block;
		}
	}
}