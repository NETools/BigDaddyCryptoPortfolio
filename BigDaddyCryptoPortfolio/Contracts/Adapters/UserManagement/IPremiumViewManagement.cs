using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IPremiumViewManagement
    {
        public ApiResult<T> LoadView<T>(User user) where T : Element;
    }
}
