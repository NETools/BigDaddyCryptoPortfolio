using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Ui;
using BigDaddyCryptoPortfolio.ViewModels.Commands.CoinsView;
using CommunityToolkit.Maui.Core.Extensions;
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
		private ICoinDataProvider _coinDataProvider;

		public IList<Coin> SelectedCategoryCoins => _coinDataProvider.SelectByCategory(this, _selectedCategory).ToList();

		public List<string> Categories { get; private set; } = 
			["AI", 
			"Web3", 
			"Defi", 
			"Grüne Coins", 
			"Gaming/Metaverse", 
			"BTC-Zusammenhang", 
			"CBDC-Netzwerk", 
			"eCommerce", 
			"Tokenization", 
			"Kein Hype"];
        public bool IsCategorySelectorExpanded { get; set; }
		public string SelectedCategory { get; set; }
		public List<Color> CategoryColor { get; } = [Color.FromArgb("#ffd700"), Color.FromArgb("#dc143c"), Color.FromArgb("#15b"), Color.FromArgb("#0a6"), Color.FromArgb("#00bfff"), Color.FromArgb("#e61"), Color.FromArgb("#678"), Color.FromArgb("#72a"), Color.FromArgb("#ff5aac"), Color.FromArgb("#000000")];
		public Color SelectedCategoryColor => CategoryColor[(int)Math.Log2((int)_selectedCategory)];

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

		public ICommand ToolBarAboutCommand { get; set; }
		public ICommand ToolBarLogoutCommand { get; set; }

		private string _uiInfoMessage;
        public string UiInfoMessage
		{
			get => _uiInfoMessage;
			set
			{
				_uiInfoMessage = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UiInfoMessage)));
			}
		}

        public event PropertyChangedEventHandler? PropertyChanged;
		public event Action SelectedCoinChanged;

		public CoinsViewModel(IPortfolioViewModel portfolioViewModel, ICoinDataProvider coinDataProvider)
		{
			_coinDataProvider = coinDataProvider;
            _coinDataProvider.CoinsLoaded += OnCoinsLoaded;

			_portfolioViewModel = portfolioViewModel;

            SelectCategory(0);

            ToolBarAboutCommand = new BasicSettingsShowCommand();
        }

        private void OnCoinsLoaded()
        {
            SelectCategory(0);
        }

        public void SelectCategory(int index)
		{
			_selectedCategory = (CoinCategory)(1 << index);
			if (SelectedCoin != null)
				SelectedCoin.IsSelected = false;
			SelectedCoin = null;
			
			IsCategorySelectorExpanded = false;
			SelectedCategory = Categories[index];

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCategorySelectorExpanded)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategoryCoins)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategoryColor)));
		}

		public async Task AddCoin(string symbol, bool makeApiCall)
		{
			var coin = _coinDataProvider.ResolveSymbol(this, symbol);

			if (!await _portfolioViewModel.AddCoin(coin.Symbol, makeApiCall))
				return;

			coin.IsInPortfolio = true;
			coin.IsNotInPortfolio = false;
			coin.IsSelected = false;

			UiInfoMessage = $"{coin.Name} added to portfolio!";

            SelectedCoin = null;
		}

		public async Task DeleteCoin(string symbol, bool makeApiCall)
		{
			var coin = _coinDataProvider.ResolveSymbol(this, symbol);

			if (!await _portfolioViewModel.RemoveCoin(coin.Symbol, makeApiCall))
				return;

			coin.IsInPortfolio = false;
			coin.IsNotInPortfolio = true;
			coin.IsSelected = false;

            UiInfoMessage = $"{coin.Name} removed from portfolio!";

            SelectedCoin = null;
		}

        public void SelectCoin(Coin coin)
        {
            if (SelectedCoin != null && coin != SelectedCoin)
                SelectedCoin.IsSelected = false;

            SelectedCoin = coin;
            SelectedCoin.IsSelected = true;
			SelectedCoinChanged?.Invoke();

        }

    }
}
