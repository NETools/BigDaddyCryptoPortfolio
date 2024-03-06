using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Account
{
    public class AccountResponse
    {
        [JsonPropertyName("fees")]
        public Fees Fees { get; set; }
    }
}
