using BigDaddyCryptoPortfolio.Adapters.Maths;
using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
    public interface IPortfolioViewModel : INotifyPropertyChanged
    {
        public int PortfolioEntryCount { get; }
        public int TotalCointCount { get; }

        public Color[] AllocationFullfillmentsIndicator { get; }
        public IDictionary<CoinCategory, IList<Coin>> Assets { get; }
        public double Score { get; }
        public string EvaluationText { get; }

        public void AddCoin(Coin coin);
        public void RemoveCoin(Coin coin);
    }
}
