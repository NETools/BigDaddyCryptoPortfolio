using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Authentication
{
    public class Signature<T> where T : class
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public T? Body { get; set; } = null;
    }
}
