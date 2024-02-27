using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels
{
    internal class PortfolioViewModel : IPortfolioViewModel
    {
        public IDictionary<CoinCategory, IList<Coin>> Assets { get; set; } = new Dictionary<CoinCategory, IList<Coin>>();
        public int PortfolioEntryCount { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void AddCoin(Coin coin)
        {
            var categories = EnumToolKit.GetCoinCategories(coin.Category);
            foreach (var category in categories)
            {
                if (!Assets.ContainsKey(category))
                    Assets.Add(category, new List<Coin>());

                Assets[category].Add(coin);

                PortfolioEntryCount++;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
        }

        public void RemoveCoin(Coin coin)
        {
            var categories = EnumToolKit.GetCoinCategories(coin.Category);
            foreach (var category in categories)
            {
                Assets[category].Remove(coin);
                if (Assets[category].Count == 0)
                    Assets.Remove(category);
                PortfolioEntryCount--;
            }

     
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
        }
    }
}
