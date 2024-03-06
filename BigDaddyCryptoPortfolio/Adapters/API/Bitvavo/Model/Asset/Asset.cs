using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Asset
{
    public class Asset
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("depositFee")]
        public string DepositFee { get; set; }

        [JsonPropertyName("depositConfirmations")]
        public int DepositConfirmations { get; set; }

        [JsonPropertyName("depositStatus")]
        public string DepositStatus { get; set; }

        [JsonPropertyName("withdrawalFee")]
        public string WithdrawalFee { get; set; }

        [JsonPropertyName("withdrawalMinAmount")]
        public string WithdrawalMinAmount { get; set; }

        [JsonPropertyName("withdrawalStatus")]
        public string WithdrawalStatus { get; set; }

        [JsonPropertyName("networks")]
        public List<string> Networks { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
