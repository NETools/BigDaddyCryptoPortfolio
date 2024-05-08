using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
    public interface ICoinsViewModel : INotifyPropertyChanged
	{
        public List<string> Categories { get; }
		public List<Color> CategoryColor { get; }
		public bool IsCategorySelectorExpanded { get; set; }
		public string SelectedCategory { get; set; }	

        public IList<Coin> SelectedCategoryCoins { get; }
		public Coin? SelectedCoin { get; }
		public string UiInfoMessage { get; }
		public bool IsCoinSelected { get; }

		public event Action SelectedCoinChanged;

        public void SelectCategory(int index);
		public void SelectCoin(Coin coin);

		public Task AddCoin(string symbol, bool makeApiCall);
		public Task DeleteCoin(string symbol, bool makeApiCall);
	}
}
