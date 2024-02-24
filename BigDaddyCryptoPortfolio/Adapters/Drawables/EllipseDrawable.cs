using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Drawables
{
    public class EllipseDrawable : BindableObject, IDrawable
    {
        public GraphicsView? GraphicsView { get; set; }

        private Color _color = Color.FromRgba(255, 0, 0, 255);

        public void SetTap(double x, double y)
        {
            var rectF = new RectF(0, 0, 50, 50);
            if (rectF.Contains(new PointF((float)x, (float)y)))
            {
                _color = Color.FromRgba(0, 255, 0, 255);
            }
            else
            {
                _color = Color.FromRgba(255, 0, 0, 255);
            }

            GraphicsView?.Invalidate();
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = _color;
            //float minSide = MathF.Min(dirtyRect.Width, dirtyRect.Height);
            //canvas.FillEllipse((dirtyRect.Width  - minSide) * 0.5f, (dirtyRect.Height - minSide) * 0.5f, minSide, minSide);

            PathF path = new PathF();
  
            path.MoveTo(0, 0);
            path.LineTo(100, 0);
            path.LineTo(0, 100);
            path.Close();

            canvas.FillPath(path);

        }
    }
}
