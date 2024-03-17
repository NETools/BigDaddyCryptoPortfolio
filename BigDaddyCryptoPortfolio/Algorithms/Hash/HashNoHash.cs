using BigDaddyCryptoPortfolio.Contracts.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Algorithms.Hash
{
    public class HashNoHash : IHash
    {
        public string Hash(string value)
        {
            return value;
        }
    }
}
