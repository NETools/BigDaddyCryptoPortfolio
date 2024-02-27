using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Shared
{
    internal static class EnumToolKit
    {
        public static IEnumerable<CoinCategory> GetCoinCategories(CoinCategory category)
        {
            int value = (int)category;
            for (int i = 0; i < 32; i++)
            {
                if (((value >> i) & 1) == 1)
                    yield return (CoinCategory)(1 << i);
            }
        }

       
    }
}
