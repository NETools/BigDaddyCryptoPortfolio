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

        private Dictionary<CoinCategory, int> _categoryIndex = new Dictionary<CoinCategory, int>()
        {
            { CoinCategory.Ai, 0 },
            { CoinCategory.Web3, 1 },
            { CoinCategory.Defi, 2 },
            { CoinCategory.Green, 3 },
            { CoinCategory.Gaming, 4 },
            { CoinCategory.BtcAssociates, 5 },
            { CoinCategory.CBDCNetwork, 6 },
            { CoinCategory.ECommerce, 7 },
            { CoinCategory.Tokenization, 8 },
        };

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

        public Color[] AllocationFullfillmentsIndicator { get; private set; } = new Color[5];

        private int _totalCoins;
        public int TotalCointCount
        {
            get => _totalCoins;
            set
            {
                _totalCoins = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalCointCount)));
            }
        }

        public IList<CategoryIndicator> CategoryIndicators { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public PortfolioViewModel()
        {
            _scoreCalculation = new ScoreCalculationAdapter(this);

            for (int i = 0; i < AllocationFullfillmentsIndicator.Length; i++)
            {
                AllocationFullfillmentsIndicator[i] = Color.FromRgb(0, 0, 0);
            }

            CategoryIndicators = new List<CategoryIndicator>();
            for (int i = 0; i < 9; i++)
            {
                CategoryIndicators.Add(new CategoryIndicator()
                {
                    StartColor = Color.FromArgb("#1f232e"),
                    EndColor = Color.FromArgb("#1f232e"),
                    Percentage = 0.0
                });
            }
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

            TotalCointCount++;

            UpdateIndicators();
            _scoreCalculation.SetEvalColors(AllocationFullfillmentsIndicator);

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllocationFullfillmentsIndicator)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryIndicators)));
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

            TotalCointCount--;

            UpdateIndicators();
            _scoreCalculation.SetEvalColors(AllocationFullfillmentsIndicator);

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllocationFullfillmentsIndicator)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryIndicators)));
        }

        private void UpdateIndicators()
        {

            foreach (var asset in Assets)
            {
                var category = asset.Key;

                if (category == CoinCategory.NoHype)
                    continue;

                var assets = asset.Value;
                var percentage = assets.Count / (double)PortfolioEntryCount;

                var categoryIndex = _categoryIndex[category];
                var categoryIndicator = CategoryIndicators[categoryIndex];

                categoryIndicator.Percentage = percentage;
                categoryIndicator.StartColor = Color.FromArgb("#073507");

                if (assets.Count > 1)
                    categoryIndicator.EndColor = Color.FromArgb("#073507");


            }
        }
    }
}
