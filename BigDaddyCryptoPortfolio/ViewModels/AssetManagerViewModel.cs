using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels
{
	internal class AssetManagerViewModel : IAssetManagerViewModel
	{
		public IDictionary<string, IList<Transaction>> Transactions { get; private set; } = new Dictionary<string, IList<Transaction>>();
		public Coin? SelectedCoin { get; private set; }
		public IList<Transaction> SelectedCoinTransactions =>
			SelectedCoin != null && Transactions.ContainsKey(SelectedCoin.Id) ? Transactions[SelectedCoin.Id] : Enumerable.Empty<Transaction>().ToList();

		public event PropertyChangedEventHandler? PropertyChanged;

		public void AddTransaction(TransactionSide side, DateTime date, double pricePerCoin, double amountInEur, double quantityCoins)
		{
			if (SelectedCoin == null)
				return;

			if (!Transactions.ContainsKey(SelectedCoin.Id))
				Transactions.Add(SelectedCoin.Id, new ObservableCollection<Transaction>());

			Transactions[SelectedCoin.Id].Add(new Transaction()
			{
				TransactionId = Guid.NewGuid(),
				CoinId = SelectedCoin.Id,
				Side = side,
				PricePerCoin = pricePerCoin,
				AmountEur = amountInEur,
				Date = date,
				QuantityCoins = quantityCoins
			});
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactions)));
		}

		public void SelectCoin(Coin coin)
		{
			SelectedCoin = coin;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoin)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactions)));
		}
	}
}
