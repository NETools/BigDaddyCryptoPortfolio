using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Ui.MiniViews;
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
    private TappingCountDetection<CollectionView> _buttonTapDetection;

    private View[] _miniSelectedCoinViews = [new SelectedCoinInfoView(), new SelectedCoinCourseView()];

    private int _selectedMiniCoinViewIndex = 0;

	public CoinsView(ICoinsViewModel coinsViewModel)
	{
		InitializeComponent();

		_coinsViewModel = coinsViewModel;
		BindingContext = _coinsViewModel;

        _coinsViewModel.PropertyChanged += OnViewModelPropertyChanged;

        _buttonTapDetection = new TappingCountDetection<CollectionView>();
        _buttonTapDetection.Register(ListView, 2, 100);

        InfoContainer.Content = _miniSelectedCoinViews[_selectedMiniCoinViewIndex];
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

		StatusBorder.IsVisible = false;
        StatusBorder.FadeTo(0);
        await StatusBorder.TranslateTo(0, -20);
    }

    private async void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
		if (e.PropertyName == "UiInfoMessage")
		{
			StatusBorder.IsVisible = true;
			StatusBorder.FadeTo(1, 1000);
			await StatusBorder.TranslateTo(0, 0, 1200);
            StatusBorder.FadeTo(0);
            await StatusBorder.TranslateTo(-20, 0);
            StatusBorder.IsVisible = false;
            await StatusBorder.TranslateTo(0, -20);
        }
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

	private void DeleteInvoked(object sender, EventArgs e)
	{
		var deletedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		RemoveCoin(deletedCoin);
	}

	private void AddInvoked(object sender, EventArgs e)
	{
        var addedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		AddCoin(addedCoin);
	}

	private void AddCoin(Coin coin)
	{
        _coinsViewModel?.AddCoin(coin);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        ListView.SelectedItem = null;
    }

	private void RemoveCoin(Coin coin)
	{
        _coinsViewModel?.DeleteCoin(coin);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        ListView.SelectedItem = null;
    }


    private async void OnCourseDataButtonClicked(object sender, EventArgs e)
    {
        await CourseDataButton.ScaleTo(0.9, 100);
        await CourseDataButton.ScaleTo(1.0, 100);

        int index = (++_selectedMiniCoinViewIndex) % 2;
        InfoContainer.Content = _miniSelectedCoinViews[index];
        

    }

    private void OnElementClickedWindows(object sender, TappedEventArgs e)
    {
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        var selectedCoin = ListView.SelectedItem as Coin;

        _buttonTapDetection.HandleTapping(ListView, ListView.SelectedItem, () =>
        {
            if (selectedCoin == null)
                return;
            if (selectedCoin.IsInPortfolio)
            {
                RemoveCoin(selectedCoin);
            }
            else AddCoin(selectedCoin);
        });
    }

    private async void OnPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_coinsViewModel == null)
            return;


        await CoinListContainer.FadeTo(0);

        var index = _coinsViewModel.Categories.IndexOf((string)e.CurrentSelection[0]);
        _coinsViewModel?.SelectCategory(index);


        await CoinListContainer.FadeTo(1, 2000);
    }
}