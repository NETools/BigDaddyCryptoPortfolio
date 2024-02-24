using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels
{
	internal class CoinsViewModel : ICoinsViewModel
	{
		private Coin? _selectedCoin;
		private CoinCategory _selectedCategory;


		private List<Coin> _coins = new List<Coin>();
		public List<Coin> Coins => _coins.FindAll(p => p.Category == _selectedCategory);

		public Coin? SelectedCoin
		{
			get => _selectedCoin;
			set
			{
				_selectedCoin = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCoin)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public CoinsViewModel()
		{
			_coins.Add(new Coin()
			{
				Symbol = "btc",
				Name = "Bitcoin",
				IconSource = "https://assets.coingecko.com/coins/images/1/large/bitcoin.png",
				Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.   \r\n\r\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet,",
				Category = CoinCategory.BtcAssociates
			});

			for (int i = 0; i < 20; i++)
			{

				_coins.Add(new Coin()
				{
					Symbol = "sol",
					Name = "Solana",
					IconSource = "https://assets.coingecko.com/coins/images/4128/large/solana.png",
					Description = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.   \r\n\r\nDuis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet,",
					Category = CoinCategory.BtcAssociates
				});

			}

			SelectCategory(0);
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
			SelectedCoin = null;
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coins)));
		}

		public void SelectCoin(Coin coin)
		{
			if (SelectedCoin != null && coin != SelectedCoin)
				SelectedCoin.IsSelected = false;
			
			SelectedCoin = coin;
			SelectedCoin.IsSelected = true;
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coins)));
		}

		public void DeleteCoin(Coin coin)
		{
			coin.IsInPortfolio = false;
			coin.IsNotInPortfolio = true;
			coin.IsSelected = false;
			SelectedCoin = null;
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coins)));
		}
	}
}
