using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
	public class Coin : INotifyPropertyChanged
	{
		public string Name { get; set; }
		public string Symbol { get; set; }
		public double Price { get; set; }
		public string IconSource { get; set; }
		public string Description { get; set; }
		public CoinCategory Category { get; set; }

		private bool _isInPortFolio;
		public bool IsInPortfolio
		{
			get => _isInPortFolio;
			set
			{
				_isInPortFolio = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInPortfolio)));
			}
		}

		private bool _isNotInPortfolio = true;
		public bool IsNotInPortfolio
		{
			get => _isNotInPortfolio;
			set
			{
				_isNotInPortfolio = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotInPortfolio)));
			}
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
