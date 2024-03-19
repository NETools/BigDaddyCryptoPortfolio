using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Ui;
using BigDaddyCryptoPortfolio.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Data
{
    public class CoinDataProvider : ICoinDataProvider
    {
        private List<Coin> _coins = new List<Coin>();
        private Dictionary<object, List<Coin>> _sharedCoins = new Dictionary<object, List<Coin>>();

        public event Action CoinsLoaded;

        public async void LoadCoins()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("CoinList.json");
            using var streamReader = new StreamReader(stream);
            var json = await streamReader.ReadToEndAsync();
            _coins = new List<Coin>(JsonSerializer.Deserialize<List<Coin>>(json));

            CoinsLoaded?.Invoke();
        }

        public Coin? ResolveSymbol(string symbol)
        {
            var coin = _coins.Find(p => p.Symbol == symbol);
            return Toolkit.Copy(coin);
        }

        public Coin? ResolveSymbol(object instance, string symbol)
        {
            if (!_sharedCoins.ContainsKey(instance))
                _sharedCoins.Add(instance, new List<Coin>());

            var cachedCoin = _sharedCoins[instance].Find(p => p.Symbol == symbol);
            if (cachedCoin == null)
            {
                var coin = ResolveSymbol(symbol);
                _sharedCoins[instance].Add(coin);
                return coin;
            }

            return cachedCoin;
        }

        public IEnumerable<Coin> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Coin> SearchBySymbol(string symbol)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Coin> SelectByCategory(CoinCategory coinCategory)
        {
            return _coins.Where(p => (p.Category & coinCategory) == coinCategory).Select(p => Toolkit.Copy(p)!);
        }

        public IEnumerable<Coin> SelectByCategory(object instance, CoinCategory coinCategory)
        {
            var selectedCategoryCoins = _coins.Where(p => (p.Category & coinCategory) == coinCategory);
            foreach (var coin in selectedCategoryCoins)
            {
                yield return ResolveSymbol(instance, coin.Symbol);
            }
        }
    }
}
