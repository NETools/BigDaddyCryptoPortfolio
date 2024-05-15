using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Exchange
{
	internal struct RetrievePortfolioMessage
	{
		[JsonInclude]
		public string Username;
	}
}
