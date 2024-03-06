using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Price
{
    public class Prices
    {
        [JsonPropertyName("market")]
        public string Market { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }
    }
}
