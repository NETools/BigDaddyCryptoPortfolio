using BigDaddyCryptoPortfolio.Adapters.Drawables;
using BigDaddyCryptoPortfolio.Models;
using System.Diagnostics;

namespace BigDaddyCryptoPortfolio.Ui.Graphics.Charts;

public partial class PieChartPercentile : ContentView
{
    private Percentile _percentile;

	public double MaximumPercentage { get; set; }
	public double BeginAtPercentage { get; set; }
	public double CurrentPercenage { get; set; }
	public double RadiusDifferencePercentage { get; set; }
	public Color? Color { get; set; }

    public event Action<PieChartPercentile, Percentile, Point>? TappedInsidePercentile;
    public event Action? TappedOutsidePercentiles;

    public PieChartRenderer Renderer { get; private set; }

    public PieChartPercentile(Percentile percentile)
	{
		InitializeComponent();
		Renderer = (PieChartRenderer)Canvas.Drawable;
        _percentile = percentile;
    }

    public void RedrawPiece()
	{
		if (Color == null)
			return;

		var adapter = (PieChartRenderer)Canvas.Drawable;

        adapter.Color = Color;
        adapter.MaximumPercentage = MaximumPercentage;
        adapter.Percentage = CurrentPercenage;
        adapter.StartPercentage = BeginAtPercentage;
        adapter.RadiusDifferencePercentage = RadiusDifferencePercentage;

        Canvas.Invalidate();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var relativePoint = e.GetPosition((Element)sender);
        if (!relativePoint.HasValue)
            return;

        var Percentiles = FindParent<NsPieChart>().GetPercentiles();
        foreach (var percentile in Percentiles)
        {
            var points = percentile.Renderer.Points;

            if (IsPointInPolygon(points, relativePoint.Value))
            {
                percentile.TappedInsidePercentile?.Invoke(this, percentile._percentile, relativePoint.Value);
                return;
            }
        }

        TappedOutsidePercentiles?.Invoke();
    }

    /// <summary>
    /// Determines if the given point is inside the polygon
    /// </summary>
    /// <param name="polygon">the vertices of polygon</param>
    /// <param name="testPoint">the given point</param>
    /// <returns>true if the point is inside the polygon; otherwise, false</returns>
    public static bool IsPointInPolygon(PointCollection polygon, Point testPoint)
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

    private T FindParent<T>() where T : Element
    {
        var currentParent = Parent;

        while(currentParent is not T)
            currentParent = currentParent.Parent;

        return (T)currentParent;
    }

}