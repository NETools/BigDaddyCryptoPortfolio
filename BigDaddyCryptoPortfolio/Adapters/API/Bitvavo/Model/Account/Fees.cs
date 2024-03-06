using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Account
{
    public class Fees
    {
        [JsonPropertyName("taker")]
        public string Taker { get; set; }

        [JsonPropertyName("maker")]
        public string Maker { get; set; }

        [JsonPropertyName("volume")]
        public string Volume { get; set; }
    }
}
