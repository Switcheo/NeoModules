using NeoModules.RPC.DTOs;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NeoModulesXF.ViewModels
{
	[AddINotifyPropertyChangedInterface]
	public class TransactionsViewModel : BaseViewModel
	{
		public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

		public TransactionsViewModel()
		{
		}

		public async Task GetTransactions()
		{
			if (Transactions.Count > 0) return;
			try
			{
				var block = await NeoService.Blocks.GetBlock.SendRequestAsync(2017000);
				int i = 0;
				foreach (var item in block.Transactions)
				{
					var transaction = await NeoService.Transactions.GetRawTransaction.SendRequestAsync(item.Txid);
					Transactions.Add(transaction);
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
