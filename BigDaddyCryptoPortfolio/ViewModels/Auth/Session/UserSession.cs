using Amazon.CognitoIdentityProvider.Model;
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth.Session
{
	internal class UserSession : IUserSession
	{
		private List<AttributeType> _attributes;
		public string Username { get; private set; }

		public AttributeType GetAttribute(string key) => _attributes.Find(p => p.Name == key);

		public void StartSession(string username, List<AttributeType> attributes)
		{
			Username = username;
			_attributes = attributes;
		}
	}
}
