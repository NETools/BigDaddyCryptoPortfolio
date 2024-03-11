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
                var categoryPercentage = ((double)assetCount / portfolioViewModel.TotalCointCount) * 100.0;

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

            score += Math.Min(MaxAssetsCount, portfolioViewModel.TotalCointCount) * (10.0 / MaxAssetsCount);
    
            if(TotalCategories > portfolioViewModel.Assets.Count)
            {
                score *= 0.9;
            }


            return Math.Round(Math.Min(score, 100.0), 1);
        }

 

        public void SetEvalColors(Color[] colors)
        {
            int categoryScore = (int)CalculateCategoryScore();
            colors[0] = PortfolioEvalColorIndicators[Math.Min(categoryScore, 10)];

            int categoryTwiceScore = (int)CalculateCategoryTwiceScore();
            colors[1] = PortfolioEvalColorIndicators[Math.Min(categoryTwiceScore, 10)];

            int categoryMissingScore = (int)CalculateCategoryMissingScore();
            colors[2] = PortfolioEvalColorIndicators[Math.Min(categoryMissingScore, 10)];

            int allocationScore = (int)CalculateAllocationScore();
            colors[3] = PortfolioEvalColorIndicators[Math.Min(allocationScore, 10)];

            int coinCountScore = (int)CalculateCoinCountScore();
            colors[4] = PortfolioEvalColorIndicators[Math.Min(coinCountScore, 10)];
        }

        private double CalculateCategoryScore()
        {
            var categoryScore = 0.0;
            foreach (var asset in portfolioViewModel.Assets)
            {
                var category = asset.Key;
                var assets = asset.Value;

                var assetCount = assets.Count;
                var categoryPercentage = ((double)assetCount / portfolioViewModel.TotalCointCount) * 100.0;
                if (assetCount >= 2)
                    categoryScore += (100.0 / TotalCategories) * 0.1;
                else
                    categoryScore += (100.0 / TotalCategories) * 0.1;
            }

            return categoryScore;
        }

        private double CalculateCategoryTwiceScore()
        {
            var categoryTwiceScore = 0.0;
            foreach (var asset in portfolioViewModel.Assets)
            {
                var category = asset.Key;
                var assets = asset.Value;

                var assetCount = assets.Count;

                if (assetCount >= 2)
                    categoryTwiceScore += (100.0 / TotalCategories) * 0.1;
            }

            return categoryTwiceScore;
        }

        private double CalculateCategoryMissingScore()
        {
            return TotalCategories > portfolioViewModel.Assets.Count ? 0 : 10;
        }

        private double CalculateAllocationScore()
        {
            var averageCategoryAllocation = 100.0 / portfolioViewModel.Assets.Count;
            var maxCategoryAllocation = 0.0;
            var minCategoryAllocation = 100.0;

            foreach (var asset in portfolioViewModel.Assets)
            {
                var category = asset.Key;
                var assets = asset.Value;
                var percentage = assets.Count / (double)portfolioViewModel.PortfolioEntryCount;

                maxCategoryAllocation = Math.Max(percentage, maxCategoryAllocation);
                minCategoryAllocation = Math.Min(percentage, minCategoryAllocation);
            }

            var maxCategoryAllocationDiff = 100.0 / averageCategoryAllocation * maxCategoryAllocation;
            var maxOk = 200.0;
            var maxBad = 600.0;

            var maxSF = Math.Min(1.0, Math.Max(0.8945, 1.0 - (maxCategoryAllocationDiff - maxOk) / (maxBad - maxOk) * 0.1));

            var minCategoryAllocationDiff = 100 / averageCategoryAllocation * (averageCategoryAllocation - minCategoryAllocation);

            var minOk = 50.0;
            var minBad = 75.0;
            var minSF = Math.Min(1.0, Math.Max(0.8945, 1.0 - (minCategoryAllocationDiff - minOk) / (minBad - minOk) * 0.1));

            return (minSF * maxSF) * 50.0 - 40.0;
        }

        private double CalculateCoinCountScore()
        {
            return Math.Min(MaxAssetsCount, portfolioViewModel.TotalCointCount) * 0.5;
        }

        private static Color[] PortfolioEvalColorIndicators = [
            Color.FromArgb("#DC143C"), 
            Color.FromArgb("#E32612"), 
            Color.FromArgb("#E84612"), 
            Color.FromArgb("#ef6600"), 
            Color.FromArgb("#ff9600"), 
            Color.FromArgb("#ffa600"), 
            Color.FromArgb("#ffc400"), 
            Color.FromArgb("#ffe400"), 
            Color.FromArgb("#dfe800"),
            Color.FromArgb("#9fdf20"),
            Color.FromArgb("#41b431")];


    }
}
