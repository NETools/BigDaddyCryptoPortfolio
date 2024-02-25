using BigDaddyCryptoPortfolio.Adapters.Drawables;

namespace BigDaddyCryptoPortfolio.Ui.Graphics.Charts;

public partial class PieChartPiece : ContentView
{
	public double BeginAtPercentage { get; set; }
	public double Percentage { get; set; }
	public Color Color { get; set; }

	public PieChartPiece()
	{
		InitializeComponent();
		Color = Color.FromRgb(255, 0, 0);
	}

	public void RedrawPiece()
	{
		((PieChartBarDrawable)Canvas.Drawable).Percentage = Percentage;
		Canvas.Invalidate();
	}
}