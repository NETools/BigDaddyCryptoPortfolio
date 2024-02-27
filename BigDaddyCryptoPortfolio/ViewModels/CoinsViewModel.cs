using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.ViewModels.Commands.CoinsView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigDaddyCryptoPortfolio.ViewModels
{
	internal class CoinsViewModel : ICoinsViewModel
	{
		private Coin? _selectedCoin;
		private CoinCategory _selectedCategory;

		private IPortfolioViewModel _portfolioViewModel;

        private List<Coin> _coins = new List<Coin>();
		public List<Coin> Coins => _coins.FindAll(p => (p.Category & _selectedCategory) == _selectedCategory);

        public Coin? SelectedCoin
		{
			get => _selectedCoin;
			set
			{
				_selectedCoin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoin)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCoinSelected)));
            }
		}

		public bool IsCoinSelected => _selectedCoin != null;

		public ICommand ToolBarSettingsCommand { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		public CoinsViewModel(IPortfolioViewModel portfolioViewModel)
		{
			_portfolioViewModel = portfolioViewModel;
			
			LoadCoins();
			SelectCategory(0);

			ToolBarSettingsCommand = new BasicSettingsShowCommand();


			AddCoin(_coins[0]);
		}

		private void LoadCoins()
		{
            _coins.Add(new Coin()
            {
                Category = CoinCategory.BtcAssociates | CoinCategory.Web3,
                Name = "Bitcoin",
                Symbol = "BTC",
                IconSource = "https://assets.coingecko.com/coins/images/1/standard/bitcoin.png",
                Description = "Bitcoin is a digital currency which operates free of any central control or the oversight of banks or governments. Instead it relies on peer-to-peer software and cryptography"
            });

            _coins.Add(new Coin()
            {
                Category = CoinCategory.ECommerce,
                Name = "Solana",
                Symbol = "SOL",
                IconSource = "https://assets.coingecko.com/coins/images/4128/large/solana.png",
                Description = "Solana is a blockchain whose purpose, use cases, and capabilities rival (and possibly exceed) that of Ethereum. It is one of the more popular blockchains, and its token, SOL, commands a decent share of the cryptocurrency market"
            });
        }

		public void SelectCategory(int index)
		{
			_selectedCategory = (CoinCategory)(1 << index);
			if (SelectedCoin != null)
				SelectedCoin.IsSelected = false;
			SelectedCoin = null;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coins)));
		}

		public void AddCoin(Coin coin)
		{
			coin.IsInPortfolio = true;
			coin.IsNotInPortfolio = false;
			coin.IsSelected = false;

			_portfolioViewModel.AddCoin(coin);

			SelectedCoin = null;
		}

		public void SelectCoin(Coin coin)
		{
			if (SelectedCoin != null && coin != SelectedCoin)
				SelectedCoin.IsSelected = false;
			
			SelectedCoin = coin;
			SelectedCoin.IsSelected = true;

		}

		public void DeleteCoin(Coin coin)
		{
			coin.IsInPortfolio = false;
			coin.IsNotInPortfolio = true;
			coin.IsSelected = false;

			_portfolioViewModel.RemoveCoin(coin);

			SelectedCoin = null;
		}
	}
}
