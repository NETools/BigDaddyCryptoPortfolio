
using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement
{
    public interface IUserSession
    {
        public string Username { get; }
        public void StartSession(string username);
    }
}
