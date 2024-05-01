using BigDaddyCryptoPortfolio.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IUserManagement
    {
        public event Action<string> Notification; 
        public Task<ApiResult<bool>> Login(Dictionary<string, string> credentials);
        public Task<ApiResult<bool>> Logout(string user);
        public Task<ApiResult<bool>> Register(Dictionary<string, string> credentials);
        public Task<ApiResult<bool>> ConfirmUser(Dictionary<string, string> credentials);
        public Task<ApiResult<bool>> ResendCode(Dictionary<string, string> credentials);
    }
}
