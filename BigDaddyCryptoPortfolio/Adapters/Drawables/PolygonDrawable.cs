using BigDaddyCryptoPortfolio.Ui.Graphics.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Drawables
{
    internal class PolygonDrawable : BindableObject, IDrawable
    {
        public NsPolygon View { get; set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (View == null)
                return;

   
            var path = GetPath(View.Points);
            canvas.FillColor = Color.FromRgb(255, 0, 0);
            canvas.FillPath(path);
        }

        private static PathF GetPath(PointCollection points)
        {
            var path = new PathF();

            if (points?.Count > 0)
            {
                path.MoveTo((float)points[0].X, (float)points[0].Y);

                for (int index = 1; index < points.Count; index++)
                    path.LineTo((float)points[index].X, (float)points[index].Y);

                path.Close();
            }

            return path;
        }
    }
}
