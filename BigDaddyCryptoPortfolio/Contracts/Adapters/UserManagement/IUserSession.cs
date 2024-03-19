using BigDaddyCryptoPortfolio.Models;
using BigDaddyCryptoPortfolio.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IUserSession
    {
        public void StartSession(User user);
        public User CurrentUser();
        public Subscription CurrentUsersSubscription();
        public string ResolveUsername();
        public Response Get<Request, Response>(string resource, Request request);
    }
}
