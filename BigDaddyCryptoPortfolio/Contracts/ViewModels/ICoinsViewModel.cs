using BigDaddyCryptoPortfolio.Models;
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
		public IList<Coin> Coins { get; }
		public Coin? SelectedCoin { get; }

		public void SelectCoin(Coin coin);
		public void AddSelectedCoinToPortfolio();
	}
}
