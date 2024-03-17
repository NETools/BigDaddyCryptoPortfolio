using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Converters;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Ui.Graphics.Charts;
using BigDaddyCryptoPortfolio.Ui.InfoViews;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;

namespace BigDaddyCryptoPortfolio.Views;

public partial class PortfolioView : ContentPage
{
	private IPortfolioViewModel _portfolioViewModel;
    private ICoinsViewModel _coinsViewModel;
    private TappingCountDetection<AssetListView> _buttonTapDetection;

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
        { CoinCategory.Green, "Grüne Coins" },
        { CoinCategory.Ai, "AI" },
        { CoinCategory.NoHype, "Kein Hype" },
        { CoinCategory.ECommerce, "E-Commerce" }
    };

    public PortfolioView(ICoinsViewModel coinsViewModel, IPortfolioViewModel portfolioViewModel)
	{
        _coinsViewModel = coinsViewModel;
		_portfolioViewModel = portfolioViewModel;
        _portfolioViewModel.PropertyChanged += OnPortfolioViewModelPropertyChanged;

		InitializeComponent();
        AssetListView.Assets = _portfolioViewModel.Coins;
        AssetListView.SwipeViewGenerator = () =>
        {
            var swipeView = new SwipeView();

            var deleteSwipeItem = new SwipeItemView();
            deleteSwipeItem.Invoked += DeleteSwipeItem_Invoked;

            var deleteSwipeItemGrid = new Grid()
            {
                WidthRequest = 100
            };
            var deleteSwipeItemGridBorder = new Border()
            {
                BackgroundColor = Color.FromArgb("#2a2f3a"),
                Stroke = Color.Parse("Transparent"),
                StrokeShape = new RoundRectangle()
                {
                    CornerRadius = 25
                }
            };

            var deleteLabel = new Label()
            {
                BackgroundColor = Color.Parse("Transparent"),
                Text = "Entfernen",
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center
            };

            deleteSwipeItemGrid.Add(deleteSwipeItemGridBorder);
            deleteSwipeItemGrid.Add(deleteLabel);

            deleteSwipeItem.Content = deleteSwipeItemGrid;

            swipeView.Triggers.Add(new DataTrigger(typeof(SwipeView))
            {
                Binding = new MultiBinding()
                {
                    Converter = new MultiBooleanConverter(),
                    Bindings =
                    {
                        new Binding("IsSelected")
                    }
                },
                Value = true,
                Setters =
                {
                    new Setter()
                    {
                        Property = SwipeView.LeftItemsProperty,
                        Value = new SwipeItems
                        {
                            deleteSwipeItem
                        }
                    }
                }
            });

            return swipeView;
        };
        AssetListView.InitView();

        BindingContext = _portfolioViewModel;

        _buttonTapDetection = new TappingCountDetection<AssetListView>();
        _buttonTapDetection.Register(AssetListView, 2, 100);

        DrawPieChart();
    }

    private void DeleteSwipeItem_Invoked(object? sender, EventArgs e)
    {
        _coinsViewModel.DeleteCoin(AssetListView.SelectedCoin.Symbol);
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

    private void OnElementClickedWindows(object sender, TappedEventArgs e)
    {
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        var selectedCoin = AssetListView.SelectedCoin;

        _buttonTapDetection.HandleTapping(AssetListView, AssetListView.SelectedCoin, () =>
        {
            if (selectedCoin == null)
                return;
            _coinsViewModel.DeleteCoin(AssetListView.SelectedCoin.Symbol);
        });
    }
}