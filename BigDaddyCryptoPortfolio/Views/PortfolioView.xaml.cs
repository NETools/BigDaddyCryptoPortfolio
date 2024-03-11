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
        { CoinCategory.BtcAssociates, Color.FromArgb("#e61") },
        { CoinCategory.Web3, Color.FromArgb("#dc143c") },
        { CoinCategory.ECommerce, Color.FromArgb("#72a") },
        { CoinCategory.CBDCNetwork,  Color.FromArgb("#678") },
        { CoinCategory.Defi, Color.FromArgb("#15b") },
        { CoinCategory.Gaming, Color.FromArgb("#00bfff") },
        { CoinCategory.Green, Color.FromArgb("#0a6") },
        { CoinCategory.Tokenization, Color.FromArgb("#ff5aac") },
        { CoinCategory.NoHype, Color.FromArgb("#000000") },
        { CoinCategory.Ai, Color.FromArgb("#ffd700") },
    };

    private Dictionary<CoinCategory, string> _labels = new Dictionary<CoinCategory, string>()
    {
        { CoinCategory.BtcAssociates, "BTC-Zusammenhang"},
        { CoinCategory.Web3, "Web3" },
        { CoinCategory.Defi, "DeFi" },
        { CoinCategory.CBDCNetwork, "CBDC-Netzwerk" },
        { CoinCategory.Gaming, "Gaming/Metaverse" },
        { CoinCategory.Tokenization, "Tokenisierung" },
        { CoinCategory.Green, "Gr�ne Coins" },
        { CoinCategory.Ai, "AI" },
        { CoinCategory.NoHype, "Kein Hype" },
        { CoinCategory.ECommerce, "E-Commerce" }
    };

    public PortfolioView(IPortfolioViewModel portfolioViewModel)
	{
		_portfolioViewModel = portfolioViewModel;
        _portfolioViewModel.PropertyChanged += OnPortfolioViewModelPropertyChanged;

		InitializeComponent();

        BindingContext = _portfolioViewModel;

        DrawPieChart();

        _portfolioViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Coins")
            ListView.ItemsSource = _portfolioViewModel.Coins;
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
                Color = _colors[category],
                Tag = category
            });

        }
	}

    private void PieChart_PercentileTapped(Percentile percentile, Point point)
    {
        var percentileCategory = percentile.Tag as CoinCategory?;
        if (!percentileCategory.HasValue)
            return;

        var category = percentileCategory.Value;

        _portfolioViewModel.SelectCategory(category);
    }

    private void PieChart_PercentileNotTapped()
    {
        _portfolioViewModel.UnselectAll();
    }
}