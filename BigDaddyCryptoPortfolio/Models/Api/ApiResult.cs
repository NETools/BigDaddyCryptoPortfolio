using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models.Api
{
    public class ApiResult<T>
    {
        public ApiResult(T? result, HttpStatusCode code, string message)
        {
            Result = result;
            Code = code;
            Message = message;
        }

        public ApiResult() { }
        
        public T? Result { get; set; }
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }

        public bool Okay => Code == HttpStatusCode.OK;
    }
}
