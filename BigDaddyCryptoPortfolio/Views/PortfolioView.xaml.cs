using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Ui.Graphics.Charts;
using System.ComponentModel;

namespace BigDaddyCryptoPortfolio.Views;

public partial class PortfolioView : ContentPage
{
	private IPortfolioViewModel _portfolioViewModel;
    private Dictionary<CoinCategory, Color> _colors = new Dictionary<CoinCategory, Color>()
    {
        { CoinCategory.BtcAssociates, Color.FromRgb(214, 130, 45) },
        { CoinCategory.Web3, Color.FromRgb(191, 54, 38) },
        { CoinCategory.ECommerce, Color.FromRgb(15, 148, 163) },
    };

    private Dictionary<CoinCategory, string> _labels = new Dictionary<CoinCategory, string>()
    {
        { CoinCategory.BtcAssociates, "BTC-Zusammenhang"},
        { CoinCategory.Web3, "Web3" },
        { CoinCategory.ECommerce, "E-Commerce" },
    };

    public PortfolioView(IPortfolioViewModel portfolioViewModel)
	{
		_portfolioViewModel = portfolioViewModel;
        _portfolioViewModel.PropertyChanged += OnPortfolioViewModelPropertyChanged;

		InitializeComponent();
        DrawPieChart();

    }

    private void OnPortfolioViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
		if (e.PropertyName == nameof(_portfolioViewModel.Assets))
		{
			DrawPieChart();
		}
    }

	private void DrawPieChart()
	{
		var pieChartRenderer = PieChart;
		pieChartRenderer?.ClearPercentiles();

		foreach (var asset in _portfolioViewModel.Assets)
		{
			var category = asset.Key;
			var assets = asset.Value;

			pieChartRenderer?.AddPercentile(new Percentile()
            {
				Label = _labels[category],
				Percentage = assets.Count / (double)_portfolioViewModel.PortfolioEntryCount,
                Size = .25,
                Color = _colors[category]

            });

        }
	}

    private void PercentileTapped(Percentile percentile, Point point)
    {

    }
}