using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Ui
{
    public class TransactionHistory : INotifyPropertyChanged
    {
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

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        private double _averagePurchasePrice;
        public double AveragePurchasePrice
        {
            get => _averagePurchasePrice;
            set
            {
                _averagePurchasePrice = value;
                OnPropertyChanged();
            }
        }

        private double _averageSellPrice;
        public double AverageSellPrice
        {
            get => _averageSellPrice;
            set
            {
                _averageSellPrice = value;
                OnPropertyChanged();
            }
        }

        private double _totalCount;
        public double TotalCount
        {
            get => _totalCount;
            set
            {
                _totalCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalCountReadable));
            }
        }

        public string TotalCountReadable => TotalCount + $" {Name}";

        private double _totalStocks;
        public double TotalStocks
        {
            get => _totalStocks;
            set
            {
                _totalStocks = value;
                OnPropertyChanged();
            }
        }

        private double _realizedProfit;
        public double RealizedProfit
        {
            get => _realizedProfit;
            set
            {
                _realizedProfit = value;
                OnPropertyChanged();
            }
        }

        private double _totalInvestments;
        public double TotalInvestments
        {
            get => _totalInvestments;
            set
            {
                _totalInvestments = value;
                OnPropertyChanged();
            }
        }

        private double _totalProfit;
        public double TotalProfit
        {
            get => _totalProfit;
            set
            {
                _totalProfit = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        public static TransactionHistory Empty()
        {
            return new TransactionHistory()
            {
                CoinId = "",
                TotalInvestments = 0
            };
        }

    }
}