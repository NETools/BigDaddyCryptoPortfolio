using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Gestures;
using BigDaddyCryptoPortfolio.Ui.MiniViews;
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
using BigDaddyCryptoPortfolio.Models.Ui;

namespace BigDaddyCryptoPortfolio.Views;

public partial class CoinsView : ContentPage
{
	private ICoinsViewModel? _coinsViewModel;
    private TappingCountDetection<AssetListView> _buttonTapDetection;

    private View[] _miniSelectedCoinViews;
    private string[] _miniSelectedButtonText = ["Zeige Kursdaten", "Zeige Beschreibung"];
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
		AssetsView.CoinSelected += OnCoinSelected;
		AssetsView.InitView();
        AssetsView.SetCoinSource("SelectedCategoryCoins", _coinsViewModel);

        _coinsViewModel.PropertyChanged += OnViewModelPropertyChanged;

        _buttonTapDetection = new TappingCountDetection<AssetListView>();
        _buttonTapDetection.Register(AssetsView, 2, 100);

        _miniSelectedCoinViews = [new SelectedCoinInfoView(), new SelectedCoinCourseView(coinsViewModel)];

        InfoContainer.Content = _miniSelectedCoinViews[_selectedMiniCoinViewIndex];
	}

	private void OnCoinSelected(Coin coin)
	{
        //AssetsView.MaximumHeightRequest = 400;
		_coinsViewModel.SelectCoin(coin);
	}

	protected override async void OnAppearing()
    {
        base.OnAppearing();
		StatusBorder.IsVisible = false;
        StatusBorder.FadeTo(0);


        InfoGrid.IsVisible = false;
        InfoGrid.FadeTo(0);

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
        else if (e.PropertyName == "IsCoinSelected")
        {
            if (_coinsViewModel.IsCoinSelected)
            {
				await InfoGrid.FadeTo(0, 150);

				AnimateHeightRequest(Window.Height - 100, 400, 1000);
				InfoGrid.IsVisible = true;
				await InfoGrid.FadeTo(1, 350);

			}
            else
            {
				await InfoGrid.FadeTo(0, 350);
                InfoGrid.IsVisible = false;
				AnimateHeightRequest(400, Window.Height - 100, 250);
			}

        }
    }

	async void AnimateHeightRequest(double fromValue, double toValue, uint length)
	{
		// Create animation
		Animation animation = new Animation(v => AssetsView.MaximumHeightRequest = v, fromValue, toValue);

		// Start animation
		 animation.Commit(this, "HeightAnimation", length: length, easing: Easing.Linear);
	}

	private async void DeleteInvoked(object? sender, EventArgs e)
	{
		var deletedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		await RemoveCoin(deletedCoin);
		//AssetsView.MaximumHeightRequest = Window.Height - 100;
        //AnimateHeightRequest(400, Window.Height - 100, 1000);
	}

	private async void AddInvoked(object? sender, EventArgs e)
	{
        var addedCoin = (Coin)((SwipeItemView)sender).BindingContext;
		await AddCoin(addedCoin);
		//AssetsView.MaximumHeightRequest = Window.Height - 100;
		//AnimateHeightRequest(400, Window.Height - 100, 1000);
	}

	private async Task AddCoin(Coin coin)
	{
        await _coinsViewModel?.AddCoin(coin.Symbol, true);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        AssetsView.Unselect();
    }

	private async Task RemoveCoin(Coin coin)
	{
        await _coinsViewModel?.DeleteCoin(coin.Symbol, true);
        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        AssetsView.Unselect();
    }


    private async void OnCourseDataButtonClicked(object sender, EventArgs e)
    {
        await CourseDataButton.ScaleTo(0.9, 100);
        await CourseDataButton.ScaleTo(1.0, 100);

        int index = (++_selectedMiniCoinViewIndex) % 2;
        InfoContainer.Content = _miniSelectedCoinViews[index];
        CourseDataButton.Text = _miniSelectedButtonText[index];

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
        //AssetsView.MaximumHeightRequest = 400;
        if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
        {
            return;
        }
        var selectedCoin = AssetsView.SelectedCoin;

        _buttonTapDetection.HandleTapping(AssetsView, AssetsView.SelectedCoin, async () =>
        {
            if (selectedCoin == null)
                return;
            if (selectedCoin.IsInPortfolio)
            {
                await RemoveCoin(selectedCoin);
				//AssetsView.MaximumHeightRequest = Window.Height - 100;
				//AnimateHeightRequest(400, Window.Height - 100, 1000);
			}
            else
            {
                await AddCoin(selectedCoin);
				//AssetsView.MaximumHeightRequest = Window.Height - 100;
				//AnimateHeightRequest(400, Window.Height - 100, 1000);
			}
        });
    }

    private async void OnPickerSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_coinsViewModel == null)
            return;

		//AssetsView.MaximumHeightRequest = Window.Height - 100;
		
		await CoinListContainer.FadeTo(0);

        var index = _coinsViewModel.Categories.IndexOf((string)e.CurrentSelection[0]);
        _coinsViewModel?.SelectCategory(index);


        await CoinListContainer.FadeTo(1, 2000);
    }
}