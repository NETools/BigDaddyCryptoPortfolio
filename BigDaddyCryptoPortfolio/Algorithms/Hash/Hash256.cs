using BigDaddyCryptoPortfolio.Contracts.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Algorithms.Hash
{
    public class Hash256 : IHash
    {
        public string Hash(string value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                var hashedBytes = sha256.ComputeHash(bytes);
                foreach (byte b in hashedBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
