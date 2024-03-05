using BigDaddyCryptoPortfolio.Ui.Graphics.Charts;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.Drawables
{
    public class PieChartRenderer : BindableObject, IDrawable
    {
        public Color Color { get; set; }

        public double RadiusDifferencePercentage { get; set; }
        public double MaximumPercentage { get; set; }
        public double StartPercentage { get; set; }
        public double Percentage { get; set; }

        public double OffsetX { get; set; }
        public double OffsetY { get; set; } 

        public PointCollection Points { get; private set; }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Points = new PointCollection();
            var minSide = System.Math.Min(dirtyRect.Width, dirtyRect.Height);

            AddCircularBar(
                Points,
                dirtyRect.Width * 0.5f + OffsetX,
                dirtyRect.Height * .5 + OffsetY,
                minSide * 0.5 / (1.0 + RadiusDifferencePercentage),
                RadiusDifferencePercentage,
                StartPercentage,
                System.Math.Min(Percentage, MaximumPercentage));

            var path = GetPath(Points);
            canvas.FillColor = Color;
            canvas.FillPath(path);
        }


        private static void AddCircularBar(PointCollection points, double cx, double cy, double radius, double diffPercentage, double startPercentage, double percentage)
        {
            int samples = 25;
            double dt = (System.Math.PI * 2.0 * percentage) / samples;

            for (int sampleIndex = 0; sampleIndex <= samples; sampleIndex++)
            {
                var x = System.Math.Sin(sampleIndex * dt + System.Math.PI * 2.0 * startPercentage) * radius + cx;
                var y = System.Math.Cos(sampleIndex * dt + System.Math.PI * 2.0 * startPercentage) * radius + cy;

                points.Add(new Point(x, y));
            }

            for (int sampleIndex = samples; sampleIndex >= 0; sampleIndex--)
            {
                var x = System.Math.Sin(sampleIndex * dt + System.Math.PI * 2.0 * startPercentage) * radius * (1.0 + diffPercentage) + cx;
                var y = System.Math.Cos(sampleIndex * dt + System.Math.PI * 2.0 * startPercentage) * radius * (1.0 + diffPercentage) + cy;

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
