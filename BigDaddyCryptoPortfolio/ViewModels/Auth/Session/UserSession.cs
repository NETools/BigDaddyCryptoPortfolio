
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using NSQM.Core.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth.Session
{
	internal class UserSession : IUserSession
	{
		public string Username { get; private set; }
		public void StartSession(string username)
		{
			Username = username;
		}
	}
}
