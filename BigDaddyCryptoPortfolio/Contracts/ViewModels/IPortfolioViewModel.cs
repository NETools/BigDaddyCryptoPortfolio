using BigDaddyCryptoPortfolio.Adapters.Maths;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
    public interface IPortfolioViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public int PortfolioEntryCount { get; }
        public int TotalCointCount { get; }

        public Color[] AllocationFullfillmentsIndicator { get; }
        public IDictionary<CoinCategory, IList<Coin>> Assets { get; }
        public IList<Coin> Coins { get; }
        public IList<CategoryIndicator> CategoryIndicators { get; }
        public double Score { get; }
        public string EvaluationText { get; }

        public event Action<Coin> CoinRemoved;

        public Task<bool> AddCoin(string symbol, bool makeApiCall);
        public Task<bool> RemoveCoin(string symbol, bool makeApiCall);

        public void SelectCategory(CoinCategory category);
        public void UnselectAll();
    }
}
