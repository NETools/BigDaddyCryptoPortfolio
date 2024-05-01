using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Exchange
{
	public struct CredentialsMessage
	{
		[JsonInclude]
		public string Username;
		[JsonInclude]
		public string Password;
	}
}
