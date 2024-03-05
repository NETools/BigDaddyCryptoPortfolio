using BigDaddyCryptoPortfolio.Adapters.Maths;
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
        private bool _isDirty = true;
        private double _lastScore;

        private ScoreCalculationAdapter _scoreCalculation;

        public IDictionary<CoinCategory, IList<Coin>> Assets { get; set; } = new Dictionary<CoinCategory, IList<Coin>>();
        public int PortfolioEntryCount { get; private set; }

        public double Score
        {
            get
            {
                if (_isDirty)
                {
                    _lastScore = _scoreCalculation.CalculateScore();
                    _isDirty = false;
                }

                return _lastScore;
            }
        }
        public string EvaluationText
        {
            get
            {
                var score = Score;
                if (score >= 91)
                    return "Sehr gut";
                else if (score >= 80)
                    return "Gut";
                else if (score >= 67)
                    return "Befriedigend";
                else if (score >= 50)
                    return "Ausreichend";
                else if (score >= 30)
                    return "Mangelhaft";
                else return "Schlecht";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PortfolioViewModel()
        {
            _scoreCalculation = new ScoreCalculationAdapter(this);
        }

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

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
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

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
        }
    }
}
