using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model.Authentication
{
    public class BitvavoApiHeader<T> where T : class
    {
        public string AccessKey { get; set; }
        public long AccessTimestamp { get; set; }
        public Signature<T> AccessSignature { get; set; }
        public int AccessWindow { get; set; }
    }
}
