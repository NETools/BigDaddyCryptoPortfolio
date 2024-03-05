using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
	[Flags]
	public enum CoinCategory
	{
		Ai = 1 << 0,
		Web3 = 1 << 1,
		Defi = 1 << 2,
		Green = 1 << 3,
		Gaming = 1 << 4,
		BtcAssociates = 1 << 5,
		CBDCNetwork = 1 << 6,
		ECommerce = 1 << 7,
		Tokenization = 1 << 8,
		NoHype = 1 << 9,
	}
}
