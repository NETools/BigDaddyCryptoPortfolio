using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Balance
{
    public class Balance
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("available")]
        public string Available { get; set; }

        [JsonPropertyName("inOrder")]
        public string InOrder { get; set; }
    }
}
