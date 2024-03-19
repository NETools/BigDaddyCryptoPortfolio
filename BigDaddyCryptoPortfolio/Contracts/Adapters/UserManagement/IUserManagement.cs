using BigDaddyCryptoPortfolio.Models.Api;
using BigDaddyCryptoPortfolio.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IUserManagement
    {
        public ApiResult<User> Login(Dictionary<string, Credential> credentials);
        public ApiResult<User> Logout(User user);
        public ApiResult<User> Register(Dictionary<string, Credential> credentials);
    }
}
