using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BigDaddyCryptoPortfolio.Views;

public partial class CoinsView : ContentPage
{
	private ICoinsViewModel? _coinsViewModel;

	public CoinsView(ICoinsViewModel coinsViewModel)
	{
		InitializeComponent();

		_coinsViewModel = coinsViewModel;
		BindingContext = _coinsViewModel;
		
	}

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
		if (e.CurrentSelection.Count == 0)
			return;

		_coinsViewModel?.SelectCoin((Coin)e.CurrentSelection[0]);
		ListView.ScrollTo(e.CurrentSelection[0], null, ScrollToPosition.MakeVisible, true);
		CloseSwipeItems();
    }

    private void CloseSwipeItems()
	{
        var field = ListView.GetVisualTreeDescendants();
        foreach (IVisualTreeElement element in field)
        {
            if (element is SwipeView swipeView)
            {
                swipeView.Close(true);
            }
        }
    }

	private void PickerSelectedIndexChanged(object sender, EventArgs e)
	{
		if (Picker.SelectedIndex < 0)
			return;

		_coinsViewModel?.SelectCategory(Picker.SelectedIndex);
	}

	private async void DeleteInvoked(object sender, EventArgs e)
	{
		var deletedCoin = (Coin)((SwipeItem)sender).BindingContext;
		_coinsViewModel?.DeleteCoin(deletedCoin);
		await DisplayAlert("Deleted from portfolio.", $"{deletedCoin.Name} deleted from portfolio!", "Okay");
		ListView.SelectedItem = null;
	}

	private async void AddInvoked(object sender, EventArgs e)
	{
		var addedCoin = (Coin)((SwipeItem)sender).BindingContext;
		_coinsViewModel?.AddCoin(addedCoin);
		await DisplayAlert("Added to portfolio.", $"{addedCoin.Name} added to portfolio!", "Okay");
		ListView.SelectedItem = null;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var addedCoin = ListView.SelectedItem as Coin;
        _coinsViewModel?.AddCoin(addedCoin);
        await DisplayAlert("Added to portfolio.", $"{addedCoin.Name} added to portfolio!", "Okay");
        ListView.SelectedItem = null;
    }
}