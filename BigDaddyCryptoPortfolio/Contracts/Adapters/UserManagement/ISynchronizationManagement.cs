using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface ISynchronizationManagement<PushType, RetrieveType>
    {
        public Task<ApiResult<RetrieveType>> Retrieve(PushType data);
        public Task<ApiResult<bool>> Push(PushType data, TransactionType transactionType);
    }
}
