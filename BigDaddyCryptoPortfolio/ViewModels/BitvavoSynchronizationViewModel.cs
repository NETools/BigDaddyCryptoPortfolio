using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels
{
    internal class BitvavoSynchronizationViewModel : IBitvavoSynchronizationViewModel
    {
        private Bitvavo _bitvavo;

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Task<bool> Authorize()
        {
            _bitvavo = new Bitvavo(ApiKey, ApiSecret);
            return _bitvavo.Authenticate();
        }

        public async IAsyncEnumerable<Coin> SynchronizePortfolio(ICoinsViewModel coinsViewModel, IPortfolioViewModel portfolioViewModel)
        {
            var balance = await _bitvavo.Balance();
            foreach (var asset in balance)
            {
                double avaible = double.Parse(asset.Available.Replace(".", ","));
                if (avaible == 0)
                    continue;

                var coin = coinsViewModel.FindCoin(asset.Symbol);
                if (coin == null)
                    continue;

                coinsViewModel.AddCoin(coin);
                yield return coin;
            }

        }
    }
}
