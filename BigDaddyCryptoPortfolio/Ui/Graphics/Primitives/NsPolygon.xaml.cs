using BigDaddyCryptoPortfolio.Adapters.Drawables;

namespace BigDaddyCryptoPortfolio.Ui.Graphics.Primitives;

public partial class NsPolygon : ContentView
{
	public PointCollection Points { get; set; }

	public NsPolygon(PointCollection points)
	{
		Points = points;

		InitializeComponent();
		((PolygonDrawable)Canvas.Drawable).View = this;

        this.SizeChanged += OnSizeChanged;

	}

    private void OnSizeChanged(object? sender, EventArgs e)
    {
		Canvas.Invalidate();
    }
}