using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface ISynchronizationManagement
    {
        public ApiResult<SynchronizationResponse> BeginSynchronization(User user, string address);
        public void Add<T>(T action) where T : ISerializable;
        public ApiResult<SynchronizationResponse> EndSynchronization();
    }
}
