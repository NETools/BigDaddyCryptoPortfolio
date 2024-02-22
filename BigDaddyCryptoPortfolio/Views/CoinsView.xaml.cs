using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Models;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace BigDaddyCryptoPortfolio.Views;

public partial class CoinsView : ContentPage
{
	private TappingCountDetection _tappingCountDetection = new TappingCountDetection();

	private ICoinsViewModel? _coinsViewModel;

	public CoinsView(ICoinsViewModel coinsViewModel, IAppUiControl uiControl)
	{
		InitializeComponent();

		_coinsViewModel = coinsViewModel;

		BindingContext = _coinsViewModel;
		
		//_tappingCountDetection.Register(ListView, 2, 500);
	}

	private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
	{
		if (e.SelectedItem == null)
			return;

		_coinsViewModel?.SelectCoin((Coin)e.SelectedItem);
		ListView.ScrollTo(e.SelectedItem, ScrollToPosition.MakeVisible, true);
		CloseSwipeItems();
	}

	private void CloseSwipeItems()
	{
        var field = ListView.GetType().GetRuntimeFields().First(p => p.Name == "_visualChildren");
        if (field != null)
        {
            if (field.GetValue(ListView) is not IReadOnlyList<IVisualTreeElement> viewCells)
                return;

            foreach (ViewCell viewCell in viewCells.Cast<ViewCell>())
            {
                if (viewCell.View != null && viewCell.View is SwipeView swipeView)
                {
                    swipeView.Close(true);
                }
            }
        }
    }

	private void PickerSelectedIndexChanged(object sender, EventArgs e)
	{
		if (picker.SelectedIndex < 0)
			return;

		_coinsViewModel?.SelectCategory(picker.SelectedIndex);
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

	private void ListView_Scrolled(object sender, ScrolledEventArgs e)
	{
		// - 1 -
		// we know per scroll an estimated 7 items are visible
		// this means each item has an height of 400 / 7 ~= 57 units
		// we can divide ScrollY (which gives the very top of the scrollbar) by 57, thus getting the start index.

		//int estimatedStartIndex = (int)(e.ScrollY / (400.0 / 7.0));
		//int estimatedEndIndex = Math.Min(estimatedStartIndex + 7, _coinsViewModel.Coins.Count);

		//if (_lastEndIndex != estimatedEndIndex)
		//{
		//	ListView.SelectedItem = null;
		//}

		//_lastEndIndex = estimatedEndIndex;

		// Debug.WriteLine($"StartIndex: {estimatedStartIndex}.\nEndIndex:{estimatedEndIndex}");
	}

	private void SwipeView_SwipeEnded(object sender, SwipeEndedEventArgs e)
	{
		// InternalChildren
		// Children
	}
}