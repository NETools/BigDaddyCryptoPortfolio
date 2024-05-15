using BigDaddyCryptoPortfolio.Models.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace BigDaddyCryptoPortfolio.Models.Dtos
{
	public struct TransactionDto
	{
		[JsonInclude]
		public string Username;
		[JsonInclude]
		public Guid TransactionId;
		[JsonInclude]
		public string CoinId;
		[JsonInclude]
		public TransactionSide Side;
		[JsonInclude]
		public DateTime Date;
		[JsonInclude]
		public double PricePerCoin;
		[JsonInclude]
		public double AmountEur;
		[JsonInclude]
		public double QuantityCoins;

		[JsonInclude]
		public TransactionType Action;

		public void CopyFrom(Transaction transaction)
		{
			TransactionId = transaction.TransactionId;
			CoinId = transaction.CoinId;
			Side = transaction.Side;
			Date = transaction.Date;
			PricePerCoin = transaction.PricePerCoin;
			AmountEur = transaction.AmountEur;
			QuantityCoins = transaction.QuantityCoins;
		}
	}
}
