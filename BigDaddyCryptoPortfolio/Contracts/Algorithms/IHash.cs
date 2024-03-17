using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Algorithms
{
    public interface IHash
    {
        public string Hash(string value);
    }
}
