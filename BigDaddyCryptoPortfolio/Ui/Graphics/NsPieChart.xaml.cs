using BigDaddyCryptoPortfolio.Adapters.Drawables;
using System.Diagnostics;

namespace BigDaddyCryptoPortfolio.Ui.Graphics;

public partial class NsPieChart : ContentView
{
	private EllipseDrawable _ellipseDrawable;
	public NsPieChart()
	{
		InitializeComponent();
		OnStartUp();
	}

	private void OnStartUp()
	{
		_ellipseDrawable = ((EllipseDrawable)Canvas.Drawable);
		_ellipseDrawable.GraphicsView = Canvas;
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		var pt = e.GetPosition((Element)sender);

		if (!pt.HasValue)
			return;

		_ellipseDrawable.SetTap(pt.Value.X, pt.Value.Y);
    }

    private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        Debug.WriteLine(e.StatusType);
    }
}