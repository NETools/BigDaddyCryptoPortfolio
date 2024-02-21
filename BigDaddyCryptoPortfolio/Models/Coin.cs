using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
	public class Coin
	{
		public string Name { get; set; }
		public string Symbol { get; set; }
		public double Price { get; set; }
		public string IconSource { get; set; }
		public string Description { get; set; }
	}
}
