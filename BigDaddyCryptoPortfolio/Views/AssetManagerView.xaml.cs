using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Models.Ui;

namespace BigDaddyCryptoPortfolio.Views;

public partial class AssetManagerView : ContentPage
{
	private IAssetManagerViewModel _assetManagerViewModel;
	public AssetManagerView(IAssetManagerViewModel assetManagerViewModel, ICoinsViewModel coinsViewModel, IPortfolioViewModel portfolioViewModel)
	{
		InitializeComponent();

		_assetManagerViewModel = assetManagerViewModel;
		BindingContext = _assetManagerViewModel;

		AssetsView.Assets = portfolioViewModel.Coins;
		AssetsView.CoinSelected += OnCoinSelected;
		AssetsView.InitView();
	}

	private void OnCoinSelected(Coin coin)
	{
		_assetManagerViewModel.SelectCoin(coin);
	}

	private void OnAddTransaction(object sender, EventArgs e)
	{
		_assetManagerViewModel.AddTransaction(TransactionSide.Buy, DateTime.Now, 0, 0, 0);
	}
}