using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Api
{
    public abstract class ApiResult<T>(T? result, int code, string message)
    {
        public T? Result { get; private set; } = result;
        public int Code { get; private set; } = code;
        public string Message { get; private set; } = message;
        public abstract bool HasSucceeded();
    }
}
