using BigDaddyCryptoPortfolio.Adapters.API.Gecko;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;

namespace BigDaddyCryptoPortfolio.Ui.MiniViews;

public partial class SelectedCoinCourseView : ContentView
{
	private ICoinsViewModel _coinsViewModel;
	private Gecko _gecko = new Gecko("CG-XAPzMYbZ8Q8KoqGdwscqrr6f");

	public double? CurrentPrice { get; set; } = null;
	public double? TotalVolume { get; set; } = null;
	public double? High24H { get; set; } = null;
	public double? Low24H { get; set; } = null;
	public double? MarketCap { get; set; } = null;
	public double? PriceChange24H { get; set; } = null;
	public double? InCirculation { get; set; } = null;
	public double? MarketCapRank { get; set; } = null;
	public SelectedCoinCourseView(ICoinsViewModel coinsViewModel)
	{
		InitializeComponent();
		BindingContext = this;

		_coinsViewModel = coinsViewModel;
		_coinsViewModel.SelectedCoinChanged += OnSelectedCoinChanged;
	}

	private async void OnSelectedCoinChanged()
	{
		Reset();

		if (_coinsViewModel.SelectedCoin == null)
		{
			return;
		}

		var result  = await _gecko.Fetch(_coinsViewModel.SelectedCoin.Id);
		if (!result)
			return;

		CurrentPrice = _gecko.CurrentPrice(_coinsViewModel.SelectedCoin.Id);
		
		TotalVolume = _gecko.TotalVolume(_coinsViewModel.SelectedCoin.Id);
		
		High24H = _gecko.High24H(_coinsViewModel.SelectedCoin.Id);
		Low24H = _gecko.Low24H(_coinsViewModel.SelectedCoin.Id);

		MarketCap = _gecko.MarketCap(_coinsViewModel.SelectedCoin.Id);
		MarketCapRank = _gecko.MarketCapRank(_coinsViewModel.SelectedCoin.Id);
		
		PriceChange24H = _gecko.PriceChange24H(_coinsViewModel.SelectedCoin.Id);
		
		InCirculation = _gecko.InCirculation(_coinsViewModel.SelectedCoin.Id);

		NotifyChanges();

	}

	private void Reset()
	{
		CurrentPrice = null;
		
		TotalVolume = null;
		
		High24H = null;
		Low24H = null;

		MarketCap = null;
		MarketCapRank = null;

		PriceChange24H = null;
		InCirculation = null;
		
		NotifyChanges();
	}

	private void NotifyChanges()
	{
		OnPropertyChanged(nameof(CurrentPrice));
		OnPropertyChanged(nameof(TotalVolume));
		OnPropertyChanged(nameof(High24H));
		OnPropertyChanged(nameof(Low24H));
		OnPropertyChanged(nameof(MarketCap));
		OnPropertyChanged(nameof(MarketCapRank));
		OnPropertyChanged(nameof(PriceChange24H));
		OnPropertyChanged(nameof(InCirculation));
	}
}