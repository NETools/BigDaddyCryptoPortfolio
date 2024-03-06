using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Balance
{
    internal class BalanceResponse
    {
        [JsonPropertyName("root")]
        public List<Balance> Response { get; set; }
    }
}
