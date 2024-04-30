using BigDaddyCryptoPortfolio.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IPremiumViewManagement
    {

        /// <summary>
        /// Will load a view specified in <paramref name="viewAddress"/> when user is assigned a corresponding subscription
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user">The user for which the view is to be loaded.</param>
        /// <param name="viewAddress">A view address of format rcns://remote/./views/</param>
        /// <returns></returns>
        public ApiResult<T> LoadView<T>(string username, string viewAddress) where T : Element;
    }
}
