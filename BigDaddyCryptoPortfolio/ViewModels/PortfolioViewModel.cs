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

        public event PropertyChangedEventHandler? PropertyChanged;

        public PortfolioViewModel()
        {
            _scoreCalculation = new ScoreCalculationAdapter(this);

            for (int i = 0; i < AllocationFullfillmentsIndicator.Length; i++)
            {
                AllocationFullfillmentsIndicator[i] = Color.FromRgb(0, 0, 0);
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

            _scoreCalculation.SetEvalColors(AllocationFullfillmentsIndicator);

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllocationFullfillmentsIndicator)));
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

            _scoreCalculation.SetEvalColors(AllocationFullfillmentsIndicator);

            _isDirty = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assets)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EvaluationText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllocationFullfillmentsIndicator)));
        }
    }
}
