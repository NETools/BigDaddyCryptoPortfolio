using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Dtos
{
    public class SynchronizationTask<T>
    {
        public T Data { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
