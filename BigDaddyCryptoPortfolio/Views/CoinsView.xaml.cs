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
using BigDaddyCryptoPortfolio.ViewModels;
using System.Net.WebSockets;
using BigDaddyCryptoPortfolio.Converters;
using Microsoft.Maui.Controls.Shapes;
using BigDaddyCryptoPortfolio.Ui.InfoViews;

namespace BigDaddyCryptoPortfolio.Views;

public partial class CoinsView : ContentPage
{
	private ICoinsViewModel? _coinsViewModel;
    private TappingCountDetection<AssetListView> _buttonTapDetection;

    private View[] _miniSelectedCoinViews = [new SelectedCoinInfoView(), new SelectedCoinCourseView()];

    private int _selectedMiniCoinViewIndex = 0;

    private IServiceProvider _serviceProvider;

    public CoinsView(ICoinsViewModel coinsViewModel, IServiceProvider serviceProvider)
	{
		InitializeComponent();

        _serviceProvider = serviceProvider;
		_coinsViewModel = coinsViewModel;
		BindingContext = _coinsViewModel;

        AssetsView.SwipeViewGenerator = () =>
        {
            var swipeView = new SwipeView();

            var deleteSwipeItem = new SwipeItemView();
            deleteSwipeItem.Invoked += DeleteInvoked;

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
                        new Binding("IsInPortfolio"),
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

            var addSwipeItem = new SwipeItemView();
            addSwipeItem.Invoked += AddInvoked;

            var addSwipeItemGrid = new Grid()
            {
                WidthRequest = 100
            };
            var addSwipeItemGridBorder = new Border()
            {
                BackgroundColor = Color.FromArgb("#2a2f3a"),
                Stroke = Color.Parse("Transparent"),
                StrokeShape = new RoundRectangle()
                {
                    CornerRadius = 25
                }
            };

            var addLabel = new Label()
            {
                BackgroundColor = Color.Parse("Transparent"),
                Text = "Hinzufügen",
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center
            };

            addSwipeItemGrid.Add(addSwipeItemGridBorder);
            addSwipeItemGrid.Add(addLabel);

            addSwipeItem.Content = addSwipeItemGrid;

            swipeView.Triggers.Add(new DataTrigger(typeof(SwipeView))
            {
                Binding = new MultiBinding()
                {
                    Converter = new MultiBooleanConverter(),
                    Bindings =
                    {
                        new Binding("IsNotInPortfolio"),
                        new Binding("IsSelected")
                    }
                },
                Value = true,
                Setters =
                {
                    new Setter()
                    {
                        Property = SwipeView.RightItemsProperty,
                        Value = new SwipeItems
                        {
                            addSwipeItem
                        }
                    }
                }
            });

            return swipeView;
        };
        AssetsView.CoinSelected += (coin) => _coinsViewModel.SelectCoin(coin);
        AssetsView.InitView();
        AssetsView.SetCoinSource("SelectedCategoryCoins", _coinsViewModel);

        _coinsViewModel.PropertyChanged += OnViewModelPropertyChanged;

        _buttonTapDetection = new TappingCountDetection<AssetListView>();
        _buttonTapDetection.Register(AssetsView, 2, 100);

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


	private void DeleteInvoked(object? sender, EventArgs e)
	{
		var deletedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		RemoveCoin(deletedCoin);
	}

	private void AddInvoked(object? sender, EventArgs e)
	{
        var addedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		AddCoin(addedCoin);
	}

	private void AddCoin(Coin coin)
	{
        _coinsViewModel?.AddCoin(coin.Symbol);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        AssetsView.Unselect();
    }

	private void RemoveCoin(Coin coin)
	{
        _coinsViewModel?.DeleteCoin(coin.Symbol);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        AssetsView.Unselect();
    }


    private async void OnCourseDataButtonClicked(object sender, EventArgs e)
    {
        await CourseDataButton.ScaleTo(0.9, 100);
        await CourseDataButton.ScaleTo(1.0, 100);

        int index = (++_selectedMiniCoinViewIndex) % 2;
        InfoContainer.Content = _miniSelectedCoinViews[index];

        //var websocket = new ClientWebSocket();
        //await websocket.ConnectAsync(new Uri("ws://178.25.225.236:8000/"), CancellationToken.None);

        //var codeLoader = new RemoteCodeLoader.RemoteCodeLoader(websocket);
        //codeLoader.AddLocalAssembly(typeof(ICoinsViewModel));
        //codeLoader.AddLocalAssembly(typeof(CoinsViewModel));

        //var view = await codeLoader.CreateXamlElement<ContentView>("rcns://remote/./views/contentviews/courseview.xaml", _serviceProvider);
        //InfoContainer.Content = view;

    }

    private void OnElementClickedWindows(object sender, TappedEventArgs e)
    {
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        var selectedCoin = AssetsView.SelectedCoin;

        _buttonTapDetection.HandleTapping(AssetsView, AssetsView.SelectedCoin, () =>
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