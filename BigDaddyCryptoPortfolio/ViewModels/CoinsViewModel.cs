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

		public ICommand ToolBarSettingsCommand { get; set; }

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

		public CoinsViewModel(IPortfolioViewModel portfolioViewModel)
		{
			_portfolioViewModel = portfolioViewModel;
			
			LoadCoins();
			SelectCategory(0);

			ToolBarSettingsCommand = new BasicSettingsShowCommand();
        }

		private async void LoadCoins()
		{
			using var stream = await FileSystem.OpenAppPackageFileAsync("CoinList.json");
			using var streamReader = new StreamReader(stream);
			var json = await streamReader.ReadToEndAsync();
			_coins = new List<Coin>(JsonSerializer.Deserialize<List<Coin>>(json));
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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coins)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategoryColor)));
		}

		public void AddCoin(Coin coin)
		{
			coin.IsInPortfolio = true;
			coin.IsNotInPortfolio = false;
			coin.IsSelected = false;

			_portfolioViewModel.AddCoin(coin);

			UiInfoMessage = $"{coin.Name} added to portfolio!";

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

            UiInfoMessage = $"{coin.Name} removed from portfolio!";

            SelectedCoin = null;
		}
	}
}
