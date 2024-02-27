using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
    public class Percentile
    {
        public string Label { get; set; }
        public double Percentage { get; set; }
        public double Size { get; set; }
        public Color? Color { get; set; }
    }
}
