using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels
{
    public interface IBitvavoSynchronizationViewModel : INotifyPropertyChanged
    {
        public string ApiKey { get; set; } 
        public string ApiSecret { get; set; }

        public Task<bool> Authorize();
        public IAsyncEnumerable<Coin> SynchronizePortfolio(ICoinDataProvider coinDataProvider, ICoinsViewModel coinsViewModel);

    }

}
