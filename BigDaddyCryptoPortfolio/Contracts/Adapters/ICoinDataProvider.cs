using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters
{
    public interface ICoinDataProvider
    {
        public event Action CoinsLoaded;

        public void LoadCoins();

        public Coin? ResolveSymbol(string symbol);
        public Coin? ResolveSymbol(object instance, string symbol);

        public IEnumerable<Coin> SelectByCategory(CoinCategory coinCategory);
        public IEnumerable<Coin> SelectByCategory(object instance, CoinCategory coinCategory);

        public IEnumerable<Coin> SearchByName(string name);
        public IEnumerable<Coin> SearchBySymbol(string symbol);
    }
}
