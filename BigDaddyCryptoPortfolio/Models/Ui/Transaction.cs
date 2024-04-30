using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Ui
{
	public class Transaction
	{
		public Guid TransactionId { get; set; }
		public string CoinId { get; set; }
		public TransactionSide Side { get; set; }
		public DateTime Date { get; set; }
		public double PricePerCoin { get; set; }
		public double AmountEur { get; set; }
		public double QuantityCoins { get; set; }

		public TimeSpan HoldingPeriod => DateTime.Now - Date;
	}
}
