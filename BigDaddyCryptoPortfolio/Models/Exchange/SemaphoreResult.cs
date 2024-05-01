using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Exchange
{
    public class SemaphoreResult
    {
        public SemaphoreSlim Semaphore { get; private set; } = new SemaphoreSlim(0, 1);
        public object Result { get; set; }
    }
}
