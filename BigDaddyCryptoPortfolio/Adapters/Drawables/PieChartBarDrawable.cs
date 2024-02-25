using BigDaddyCryptoPortfolio.Ui.Graphics.Charts;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Drawables
{
    internal class PieChartBarDrawable : BindableObject, IDrawable
    {
        public double Percentage { get; set; }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var pc = new PointCollection();
            var minSide = Math.Min(dirtyRect.Width, dirtyRect.Height);
            AddCircularBar(pc, dirtyRect.Width * 0.5f, dirtyRect.Height * .5, minSide * 0.25, Percentage);

            var path = GetPath(pc);
            canvas.FillColor = Color.FromRgb(255, 0, 0);
            canvas.FillPath(path);
        }


        private static void AddCircularBar(PointCollection points, double cx, double cy, double radius, double percentage)
        {
            int samples = 50;
            double dt = Math.PI * 2.0 * percentage / (double)samples;

            for (int sampleIndex = 0; sampleIndex <= samples; sampleIndex++)
            {
                var x = Math.Sin(dt * sampleIndex) * radius + cx;
                var y = Math.Cos(dt * sampleIndex) * radius + cy;

                points.Add(new Point(x, y));
            }

            for (int sampleIndex = samples; sampleIndex >= 0; sampleIndex--)
            {
                var x = Math.Sin(dt * sampleIndex) * radius * 2.0 + cx;
                var y = Math.Cos(dt * sampleIndex) * radius * 2.0 + cy;

                points.Add(new Point(x, y));
            }
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
