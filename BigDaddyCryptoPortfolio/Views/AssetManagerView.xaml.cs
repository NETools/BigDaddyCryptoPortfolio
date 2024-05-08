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

		portfolioViewModel.CoinRemoved += OnCoinRemoved;

		AssetsView.Assets = portfolioViewModel.Coins;
		AssetsView.CoinSelected += OnCoinSelected;
		AssetsView.InitView();
	}

	private void OnCoinRemoved(Coin coin)
	{
		if (_assetManagerViewModel.SelectedCoin?.Id == coin.Id)
			_assetManagerViewModel.SelectCoin(null);
	}

	private void OnCoinSelected(Coin coin)
	{
		_assetManagerViewModel.SelectCoin(coin);
	}

	private async void OnAddTransaction(object sender, EventArgs e)
	{
		await AddTransactionButton.ScaleTo(0.9, 100);
		await AddTransactionButton.ScaleTo(1.0, 100);

		_assetManagerViewModel.AddTransaction(TransactionSide.Buy, DateTime.Now, 0, 0, 0);
	}

	private void OnPickerSelectedIndexChanged(object sender, EventArgs e)
	{
		if (sender is not Picker picker)
			return;
		if (picker.BindingContext is not Transaction transaction)
			return;

		transaction.Side = (TransactionSide)picker.SelectedIndex;
	}

	private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
	{
		if (sender is not DatePicker datePicker)
			return;
		if (datePicker.BindingContext is not Transaction transaction)
			return;

		transaction.Date = e.NewDate;
	}
}