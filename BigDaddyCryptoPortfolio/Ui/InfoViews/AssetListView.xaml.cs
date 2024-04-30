using BigDaddyCryptoPortfolio.Converters;
using BigDaddyCryptoPortfolio.Models.Ui;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace BigDaddyCryptoPortfolio.Ui.InfoViews;

public partial class AssetListView : ContentView
{
    private CollectionView _listView;

	private IList<Coin> _assets;
	public IList<Coin> Assets
	{
		get => _assets;
		set
		{
			_assets = value;
			OnPropertyChanged(nameof(Assets));
		}
	}

    public Func<SwipeView> SwipeViewGenerator { get; set; }

    public Coin? SelectedCoin { get; private set; }

    public event Action<Coin> CoinSelected;

	public AssetListView()
	{
		InitializeComponent();
		BindingContext = this;
    }

	public void InitView()
	{
		_listView = new CollectionView
		{
			ItemTemplate = new DataTemplate(() =>
			{
				var root = new Grid()
				{
					Padding = new Thickness(5, 5, 5, 0)
				};

                var itemGrid = new Grid()
                {
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto)
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Star)
                    }
                };

                var itemBorder = new Border()
                {
                    Stroke = Color.Parse("Transparent"),
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = 25
                    },
                    Background = new LinearGradientBrush()
                    {
                        GradientStops =
                        {
                            new GradientStop(Color.FromArgb("#202530"), 0.0f),
                            new GradientStop(Color.FromArgb("#111826"), 1.0f)
                        }   
                    }
                };

                itemGrid.AddWithSpan(itemBorder, 0, 0, 2, 2);

                var selectedBorder = new Border()
                {
                    StrokeThickness = 5,
                    Stroke = Color.FromArgb("#2a2f3a"),
                    BackgroundColor = Color.Parse("Transparent"),
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = 25
                    }
                };

                selectedBorder.SetBinding(Border.IsVisibleProperty, "IsSelected");

                itemGrid.AddWithSpan(selectedBorder, 0, 0, 2, 2);

                var imageBorder = new Border()
                {
                    Stroke = Color.Parse("Transparent"),
                    StrokeShape = new RoundRectangle()
                    {
                        CornerRadius = 25
                    }
                };

                var image = new Image()
                {
                    Aspect = Aspect.AspectFill,
                    WidthRequest = 60,
                    HeightRequest = 60
                };

                image.SetBinding(Image.SourceProperty, "IconSource");

                imageBorder.Content = image;

                itemGrid.AddWithSpan(imageBorder, 0, 0, 2, 1);

                var labelingGrid = new Grid()
                {
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto)
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto)
                    },
                    Padding = new Thickness(10, 10, 0, 0)
                };

                var name = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                };
                name.SetBinding(Label.TextProperty, "Name");

                labelingGrid.Add(name);

                var symbol = new Label()
                {
                    FontAttributes = FontAttributes.Bold,
                };
                symbol.SetBinding(Label.TextProperty, "Name");

                labelingGrid.Add(symbol, 0, 1);

                var isInPortfolio = new Label()
                {
                    Margin = 10,
                    VerticalTextAlignment = TextAlignment.Center
                };
                isInPortfolio.SetBinding(Label.TextProperty, "IsInPortfolio");

                labelingGrid.AddWithSpan(isInPortfolio, 0, 1, 2, 1);

                itemGrid.Add(labelingGrid, 1, 0);

                if (SwipeViewGenerator != null && DeviceInfo.Current.Platform != DevicePlatform.WinUI)
                {
                    var swipeView = SwipeViewGenerator();
                    swipeView.Content = itemGrid;
                    root.Add(swipeView);
                }
                else root.Add(itemGrid);

                return root;
			})
        };

        _listView.SetBinding(CollectionView.ItemsSourceProperty, "Assets");
        _listView.SelectionChanged += ListView_SelectionChanged;
        _listView.SelectionMode = SelectionMode.Single;

        Content = _listView;
    }

    public void SetCoinSource(string path, object context)
    {
        _listView.BindingContext = context;
        _listView.SetBinding(CollectionView.ItemsSourceProperty, path);
    }

    public void Unselect()
    {
        _listView.SelectedItem = null;
    }

    private void ListView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;

        var selection = e.CurrentSelection[0] as Coin;
        selection.IsSelected = true;

        if (e.PreviousSelection.Count > 0 && e.PreviousSelection[0] is Coin previous)
            previous.IsSelected = false;

        SelectedCoin = selection;

        CoinSelected?.Invoke(SelectedCoin);

        _listView.ScrollTo(e.CurrentSelection[0], null, ScrollToPosition.MakeVisible, true);
        CloseSwipeItems();
    }

    private void CloseSwipeItems()
    {
        var field = _listView.GetVisualTreeDescendants();
        foreach (IVisualTreeElement element in field)
        {
            if (element is SwipeView swipeView)
            {
                swipeView.Close(true);
            }
        }
    }
}