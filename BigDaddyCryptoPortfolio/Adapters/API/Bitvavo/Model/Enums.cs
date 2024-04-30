using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Model
{
    public enum BitvavoApiType
    {
        REST,
        WebSockets
    }

    public enum LoggingType
    {
        Info,
        Error,
        Warning
    }

    public enum WebsocketResponseCodes
    {
        Auth = 100,
        Ticker = 110,
        Balance = 120,
    }

    public enum TransactionSide
    {
        Sell,
        Buy
    }
}
