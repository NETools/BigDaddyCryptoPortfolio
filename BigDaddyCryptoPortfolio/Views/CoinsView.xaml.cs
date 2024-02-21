using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BigDaddyCryptoPortfolio.Views;

public partial class CoinsView : ContentPage
{
	private TappingCountDetection _tappingCountDetection = new TappingCountDetection();

	private ICoinsViewModel? _coinsViewModel;

	public CoinsView(ICoinsViewModel coinsViewModel)
	{
		InitializeComponent();

		_coinsViewModel = coinsViewModel;

		BindingContext = _coinsViewModel;

		_tappingCountDetection.Register(ListView, 2, 500);
	}

	private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		_coinsViewModel?.SelectCoin((Coin)e.SelectedItem);
	}

	private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
	{
		_tappingCountDetection.HandleTapping((ListView)sender, e.ItemIndex, async () =>
		{
			await DisplayAlert("Added to portfolio.", $"{_coinsViewModel?.SelectedCoin?.Name} added to portfolio!", "Thanks");
		});
	}
}