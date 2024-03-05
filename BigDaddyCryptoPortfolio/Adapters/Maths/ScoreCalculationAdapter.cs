using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Maths
{
    public class ScoreCalculationAdapter (IPortfolioViewModel portfolioViewModel)
    {
        private const int TotalCategories = 10 - 1;
        private const int MaxAssetsCount = 20;

        public double CalculateScore()
        {
            double score = .0;
            double totalCategoryPercentage = .0;
            double maxCategoryPercentage = .0;

            double scoreFactor_Category = 0;
            double scoreFactor_CategoryTwice = 0;

            foreach (var asset in portfolioViewModel.Assets)
            {
                var category = asset.Key;
                var assets = asset.Value;

                var assetCount = assets.Count;
                var categoryPercentage = ((double)assetCount / portfolioViewModel.Assets.Count) * 100.0;

                totalCategoryPercentage += categoryPercentage;

                maxCategoryPercentage = Math.Max(categoryPercentage, maxCategoryPercentage);

                if (assetCount >= 2)
                {
                    score += (100.0 / TotalCategories) * 0.9;
                    scoreFactor_CategoryTwice += (100 / TotalCategories) * 0.1;
                    scoreFactor_Category += (100 / TotalCategories) * 0.1;
                }
                else
                {
                    score += (100.0 / TotalCategories) * 0.8;
                    scoreFactor_Category += (100.0 / TotalCategories) * 0.1;
                }
            }

            score += Math.Min(MaxAssetsCount, portfolioViewModel.Assets.Count) * (10.0 / MaxAssetsCount);
    
            if(TotalCategories > portfolioViewModel.Assets.Count)
            {
                score *= 0.9;
            }


            return System.Math.Round(score, 1);
        }
    }
}
