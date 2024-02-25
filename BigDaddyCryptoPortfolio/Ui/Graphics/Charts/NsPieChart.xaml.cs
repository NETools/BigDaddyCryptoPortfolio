using BigDaddyCryptoPortfolio.Adapters.Drawables;
using BigDaddyCryptoPortfolio.Ui.Graphics.Primitives;
using Microsoft.Maui.Controls.Shapes;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Path = Microsoft.Maui.Controls.Shapes.Path;
using Timer = System.Timers.Timer;

namespace BigDaddyCryptoPortfolio.Ui.Graphics.Charts;

public partial class NsPieChart : ContentView
{
    private Timer _timer = new Timer();

	public NsPieChart()
	{
		InitializeComponent();

        _timer.Elapsed += OnElapsed;
        _timer.Interval = 20;
        _timer.Start();

        Shell.Current.Navigated += Current_Navigated;
   
        SizeChanged += NsPieChart_SizeChanged;
    }

    private void Current_Navigated(object? sender, ShellNavigatedEventArgs e)
    {
        if (e.Current.Location.ToString().Contains("Portfolio"))
        {
            ((PieChartBarDrawable)Renderer.Drawable).Percentage = 0;
            _timer.Start();
        }
    }

    private void OnElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        ((PieChartBarDrawable)Renderer.Drawable).Percentage += 0.025;
        Renderer.Invalidate();

        if (((PieChartBarDrawable)Renderer.Drawable).Percentage >= 0.92)
            _timer.Stop();
    }

    private void NsPieChart_SizeChanged(object? sender, EventArgs e)
    {
        ((PieChartBarDrawable)Renderer.Drawable).Percentage = 0;
        _timer.Start();
    }

    private void PolygonTapped(object? sender, TappedEventArgs e)
	{
        var polygon = sender as Polygon;
        if (polygon == null)
            return;

		var points = polygon.Points;
		var relativePoint = e.GetPosition((Element)sender);
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var result = IsPointInPolygon4(points, relativePoint.Value);
        sw.Stop();

        Debug.WriteLine($"Calculation took: {sw.Elapsed.TotalMilliseconds} ms!");
        if (result)
        {
            Debug.WriteLine("Polygon clicked!");
        }
        else
        {
         
        }
	}


    /// <summary>
    /// Determines if the given point is inside the polygon
    /// </summary>
    /// <param name="polygon">the vertices of polygon</param>
    /// <param name="testPoint">the given point</param>
    /// <returns>true if the point is inside the polygon; otherwise, false</returns>
    public static bool IsPointInPolygon4(PointCollection polygon, Point testPoint)
    {
        bool result = false;
        int j = polygon.Count - 1;
        for (int i = 0; i < polygon.Count; i++)
        {
            if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y ||
                polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
            {
                if (polygon[i].X + (testPoint.Y - polygon[i].Y) /
                   (polygon[j].Y - polygon[i].Y) *
                   (polygon[j].X - polygon[i].X) < testPoint.X)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }
}