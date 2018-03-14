using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using NeoModules.RPC.DTOs;
using PropertyChanged;

namespace NeoModulesXF.ViewModels
{
	[AddINotifyPropertyChangedInterface]
	public class BlocksViewModel : BaseViewModel
	{
		private int blockCount = 2017000;

		public ObservableCollection<Block> Blocks { get; set; } = new ObservableCollection<Block>();

		public async Task GetBlocks()
		{
			if (Blocks.Count > 0) return;
			try
			{
				int i = 0;
				while (i < 10)
				{
					var block = await NeoService.Blocks.GetBlock.SendRequestAsync(blockCount);
					Blocks.Add(block);
					blockCount--;
					i++;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}
	}
}
