using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
	public interface IAssetManagerViewModel : INotifyPropertyChanged
	{
		public IPortfolioViewModel Portfolio { get; }
		public IDictionary<string, IList<Transaction>> Transactions { get; }
		public IList<Transaction> SelectedCoinTransactions { get; }
		public Coin? SelectedCoin { get; }

		public void SelectCoin(Coin? coin);
		public void AddTransaction(TransactionSide action, DateTime date, double pricePerCoin, double amountInEur, double quantityCoins);
	}
}
