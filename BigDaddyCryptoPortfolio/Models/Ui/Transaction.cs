using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using Humanizer;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Ui
{
	public class Transaction : INotifyPropertyChanged
	{
		public Guid TransactionId { get; } = Guid.NewGuid();

		private string _coinId;
		public string CoinId
		{
			get => _coinId;
			set
			{
				_coinId = value;
				OnPropertyChanged();
			}
		}

		private TransactionSide _side;
		public TransactionSide Side
		{
			get => _side;
			set
			{
				_side = value;
				OnPropertyChanged();
			}
		}

		private DateTime _dateTime;
		public DateTime Date
		{
			get => _dateTime;
			set
			{
				_dateTime = value;
				OnPropertyChanged();
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HoldingPeriod)));
			}
		}

		private double _pricePerCoin;
		public double PricePerCoin
		{
			get => _pricePerCoin;
			set
			{
				_pricePerCoin = value;
				OnPropertyChanged();
			}
		}

		private double _amountEur;
		public double AmountEur
		{
			get => _amountEur;
			set
			{
				_amountEur = value;
				OnPropertyChanged();
			}
		}

		private double _quantityCoins;
		public double QuantityCoins
		{
			get => _quantityCoins;
			set
			{
				_quantityCoins = value;
				OnPropertyChanged();
			}
		}

		public string HoldingPeriod => (DateTime.Now - Date).Humanize();

		public event PropertyChangedEventHandler? PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
