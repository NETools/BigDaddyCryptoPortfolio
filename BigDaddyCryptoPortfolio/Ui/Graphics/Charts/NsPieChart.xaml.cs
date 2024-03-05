using BigDaddyCryptoPortfolio.Models;
using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using Timer = System.Timers.Timer;

namespace BigDaddyCryptoPortfolio.Ui.Graphics.Charts;

public partial class NsPieChart : ContentView
{
    private IDispatcherTimer _animationTimer;
    private int _currentIndex;
    private double _lastPercentile;

    private PieChartPercentile _shadowPercentile;

    private ObservableCollection<PieChartPercentile> _percentiles = [];

    private Grid _percentileInfoGrid;
    private Label _percentileInfoText;
    private Border _percentileColor;

    public ObservableCollection<Grid> Descriptions { get; private set; } = [];

    private int _interval = 20;
    public int Interval
    {
        get => _interval;
        set
        {
            _interval = value;
            
            _animationTimer.Stop();
            _animationTimer.Interval = TimeSpan.FromTicks(_interval);

            RestartAnimation();
        }
    }

    private double _increment = 0.025;
    public double Increment
    {
        get => _increment;
        set => _increment = value;
    }

    public NsPieChartShadow PieChartShadow { get; set; }

    public event Action<Percentile, Point> PercentileTapped;

    public NsPieChart()
	{
		InitializeComponent();

        _percentiles.CollectionChanged += OnPercentilesChanged;
        Descriptions.CollectionChanged += OnDescriptionsChanged;

        _animationTimer = Dispatcher.CreateTimer();
        _animationTimer.Tick += OnElapsed;
        _animationTimer.Interval = TimeSpan.FromTicks(20);
        _animationTimer.Start();

        Shell.Current.Navigated += Current_Navigated;

        _percentileInfoGrid = new Grid();
        _percentileInfoGrid.ZIndex = int.MaxValue;
        _percentileInfoGrid.WidthRequest = 100;
        _percentileInfoGrid.HeightRequest = 30;

        _percentileInfoGrid.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        _percentileInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(20));
        _percentileInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

        var border = new Border();
        border.BackgroundColor = Color.FromRgb(10, 10, 10);
        border.Opacity = 0.8;

        _percentileInfoGrid.AddWithSpan(border, 0, 0, 1, 2);


        _percentileColor = new Border();
        _percentileColor.WidthRequest = 10;
        _percentileColor.HeightRequest = 10;
        _percentileColor.VerticalOptions = LayoutOptions.Center;
        _percentileColor.HorizontalOptions = LayoutOptions.End;

        _percentileInfoGrid.Add(_percentileColor);

        _percentileInfoText = new Label();
        _percentileInfoText.TextColor = Color.FromRgb(255, 255, 255);
        _percentileInfoText.HorizontalTextAlignment = TextAlignment.Start;
        _percentileInfoText.VerticalTextAlignment = TextAlignment.Center;
        _percentileInfoText.HorizontalOptions = LayoutOptions.Start;
        _percentileInfoText.Margin = new Thickness(10, 0, 10, 0);

        _percentileInfoText.Text = "Balken drücken für Details.";

        _percentileInfoGrid.Add(_percentileInfoText, 1, 0);

        _percentileInfoGrid.IsVisible = true;

        _percentileInfoGrid.TranslateTo(0, 0);
        Canvas.Add(_percentileInfoGrid);

        _shadowPercentile = new PieChartPercentile(new Percentile(), false);
        _shadowPercentile.BeginAtPercentage = 0;
        _shadowPercentile.MaximumPercentage = 1;
        _shadowPercentile.Color = Color.FromRgb(0, 0, 0);
        _shadowPercentile.RadiusDifferencePercentage = 0.25;

        _shadowPercentile.Renderer.OffsetX += 5;
        _shadowPercentile.Renderer.OffsetY += 5;
    }

    public void AddPercentile(Percentile percentile)
    {
        _percentiles.Add(new PieChartPercentile(percentile, true)
        {
            BeginAtPercentage = _lastPercentile,
            MaximumPercentage = percentile.Percentage,
            RadiusDifferencePercentage = percentile.Size,
            Color = percentile.Color
        });

        _lastPercentile += percentile.Percentage;
    }

    public void ClearPercentiles()
    {
        _lastPercentile = 0;
        _percentiles.Clear();
    }

    public ICollection<PieChartPercentile> GetPercentiles() => _percentiles;

    private void OnDescriptionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        DeleteChild<Grid>([_percentileInfoGrid]);

        foreach (var label in Descriptions)
        {
            Canvas.Add(label);
        }
    }

    private void OnPercentilesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        var collection = sender as ObservableCollection<PieChartPercentile>;
        if (collection == null)
            return;

        DeleteChild<PieChartPercentile>();

        Canvas.Add(_shadowPercentile);

        foreach (var piece in collection)
        {
            piece.TappedInsidePercentile -= OnPercentileTapped;
            piece.TappedInsidePercentile += OnPercentileTapped;
            piece.TappedOutsidePercentiles += OnPercnetileNotTapped;
            Canvas.Add(piece);
        }

        RestartAnimation();
    }

    private void OnPercnetileNotTapped()
    {
        _percentileInfoGrid.IsVisible = false;
    }

    private bool _firstRender = true;
    private async void Current_Navigated(object? sender, ShellNavigatedEventArgs e)
    {
        var shell = sender as Shell;
        if (shell == null)
            return;

        _percentileInfoGrid.IsVisible = _firstRender;
        _firstRender = false;

        var descendents = shell.CurrentPage.GetVisualTreeDescendants();
        foreach (var descendent in descendents)
        {
            if (descendent is NsPieChart)
            {
                RestartAnimation();
                break;
            }
        }
    }

    private void RestartAnimation()
    {
        _shadowPercentile.CurrentPercenage = 0;
        _shadowPercentile.Color = PieChartShadow == null ? Color.FromRgb(0, 0, 0) : PieChartShadow.Color;
        _shadowPercentile.RedrawPiece();

        foreach (var piece in _percentiles)
        {
            piece.CurrentPercenage = 0;
            piece.RedrawPiece();
        }


        _currentIndex = 0;
        _animationTimer.Start();
    }

    private void OnElapsed(object? sender, EventArgs e)
    {
        if(_percentiles.Count == 0)
        {
            _animationTimer.Stop();
            return;
        }

        var currentPiece = _percentiles[_currentIndex];

        if (currentPiece.CurrentPercenage >= currentPiece.MaximumPercentage)
        {
            _currentIndex++;
            if (_currentIndex >= _percentiles.Count)
            {
                _animationTimer.Stop();
                return;

            }
        }


        _shadowPercentile.CurrentPercenage += _increment;
        _shadowPercentile.RedrawPiece();

        currentPiece.CurrentPercenage += _increment;
        currentPiece.RedrawPiece();
    }

    private void DeleteChild<T>(List<T> excludes = null)
    {
        for (int i = Canvas.Children.Count - 1; i >= 0; i--)
        {
            var child = Canvas.Children[i];
            if (child is T)
            {
                if (excludes != null && excludes.Exists(p => p.Equals(child)))
                    continue;

                Canvas.Children.RemoveAt(i);
                if (child is Element element)
                {
                    Canvas.RemoveLogicalChild(element);
                }
            }
        }
    }

    private async void OnPercentileTapped(PieChartPercentile renderer, Percentile percentile, Point point)
    {
        _percentileColor.BackgroundColor = percentile.Color;

        _percentileInfoGrid.IsVisible = true;
        _percentileInfoText.Text = percentile.Label;

        var x = point.X - _percentileInfoGrid.X;
        var y = point.Y - _percentileInfoGrid.Y;

        if (x + _percentileInfoGrid.Width >= Canvas.Width * 0.5)
        {
            x -= _percentileInfoGrid.Width;
        }
        if (y + _percentileInfoGrid.Height >= Canvas.Height * 0.5)
        {
            y -= _percentileInfoGrid.Height;
        }

        HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);

        await _percentileInfoGrid.TranslateTo(x, y);

        // Delegate event further up
        PercentileTapped?.Invoke(percentile, point);
    }

}