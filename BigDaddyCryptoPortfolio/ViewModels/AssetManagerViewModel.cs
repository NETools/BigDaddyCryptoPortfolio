using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using BigDaddyCryptoPortfolio.Adapters.API.Gecko;
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
	internal class AssetManagerViewModel (IPortfolioViewModel portfolioViewModel) : IAssetManagerViewModel
	{
		public IPortfolioViewModel Portfolio { get; private set; } = portfolioViewModel;
		public IDictionary<string, IList<Transaction>> Transactions { get; private set; } = new Dictionary<string, IList<Transaction>>();
		public Coin? SelectedCoin { get; private set; }
        public double SelectedCoinPrice { get; private set; }
        public IList<Transaction> SelectedCoinTransactions =>
			SelectedCoin != null && Transactions.ContainsKey(SelectedCoin.Id) ? Transactions[SelectedCoin.Id] : Enumerable.Empty<Transaction>().ToList();

		public IDictionary<string, TransactionHistory> TransactionHistory { get; private set; } 
			= new Dictionary<string, TransactionHistory>();


		public TransactionHistory SelectedCoinTransactionsHistory =>
			SelectedCoin != null && TransactionHistory.ContainsKey(SelectedCoin.Id) ?
			TransactionHistory[SelectedCoin.Id] : BigDaddyCryptoPortfolio.Models.Ui.TransactionHistory.Empty();


        public event PropertyChangedEventHandler? PropertyChanged;

        private Gecko _gecko = new Gecko("CG-XAPzMYbZ8Q8KoqGdwscqrr6f");
		private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public void AddTransaction(TransactionSide side, DateTime date, double pricePerCoin, double amountInEur, double quantityCoins)
		{
			if (SelectedCoin == null)
				return;

			if (!Transactions.ContainsKey(SelectedCoin.Id))
			{
				Transactions.Add(SelectedCoin.Id, new ObservableCollection<Transaction>());
				TransactionHistory.Add(SelectedCoin.Id, new Models.Ui.TransactionHistory());
			}
			var transaction = new Transaction()
			{
				CoinId = SelectedCoin.Id,
				Side = side,
				PricePerCoin = pricePerCoin,
				AmountEur = amountInEur,
				Date = date,
				QuantityCoins = quantityCoins
			};

            transaction.PropertyChanged += OnTransactionChanged;

            Transactions[SelectedCoin.Id].Add(transaction);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactions)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactionsHistory)));
		}

		private async void OnTransactionChanged(object? sender, PropertyChangedEventArgs e)
		{
			await _semaphore.WaitAsync();
			if (SelectedCoin == null)
				return;

			var history = SelectedCoinTransactionsHistory;
			history.TotalInvestments = 0;
			history.TotalCount = 0;
			history.TotalStocks = 0;
			history.RealizedProfit = 0;
			history.Name = SelectedCoin.Symbol;

			var transactions = Transactions[SelectedCoin.Id];

			var result = await _gecko.Fetch(SelectedCoin.Id);
			if (!result)
				return;

			var currentPrice = _gecko.CurrentPrice(SelectedCoin.Id);

			if (currentPrice == null)
				return;

			history.Price = currentPrice.Value;
			
			double buySideCount = 0;
			double buySideCoinCount = 0;

			double sellSideCount = 0;
            double sellSideCoinCount = 0;

            for (int i = 0; i < transactions.Count; i++)
			{
				var transaction = transactions[i];

				var factor = transaction.Side == TransactionSide.Buy ? 1.0 : -1.0;


				history.TotalInvestments += transaction.AmountEur;

				history.TotalStocks += currentPrice.Value * transaction.QuantityCoins * factor;
				history.TotalCount += transaction.QuantityCoins * factor;

				if (transaction.Side == TransactionSide.Buy)
				{
					buySideCount += transaction.QuantityCoins * transaction.PricePerCoin;
					buySideCoinCount += transaction.QuantityCoins;
				}
				else
				{
					sellSideCount += transaction.QuantityCoins * transaction.PricePerCoin;
					history.RealizedProfit += transaction.AmountEur;
                    sellSideCoinCount += transaction.QuantityCoins;
                }


			}

			history.AveragePurchasePrice = Math.Round(buySideCount / buySideCoinCount, 3);
			history.AverageSellPrice = Math.Round(sellSideCount / sellSideCoinCount, 3);

            history.TotalStocks = Math.Round(history.TotalStocks, 2);
            history.TotalInvestments = Math.Round(history.TotalInvestments, 2);
            history.TotalCount = Math.Round(history.TotalCount, 2);
			history.TotalProfit = Math.Round(history.TotalStocks - history.TotalInvestments + history.RealizedProfit, 2);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactionsHistory)));

			_semaphore.Release();
        }

        public async void SelectCoin(Coin? coin)
		{
			SelectedCoin = coin;

            var result = await _gecko.Fetch(SelectedCoin.Id);
            if (!result)
                return;
            var currentPrice = _gecko.CurrentPrice(SelectedCoin.Id);

            if (currentPrice == null)
                return;


            SelectedCoinPrice = Math.Round(currentPrice.Value, 3);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoin)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactions)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinTransactionsHistory)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoinPrice)));
        }
	}
}
