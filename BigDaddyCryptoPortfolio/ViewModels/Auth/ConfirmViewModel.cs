using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth
{
	internal class ConfirmViewModel (IUserManagement userManagement, AuthSucceededAdapter context) : IConfirmViewModel
	{
		public string Email { get; set; }
		public string ConfirmationCode { get; set; }

		private string _message;
		public string Message
		{
			get => _message;
			set
			{
				_message = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
			}
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public event Action Confirmed;

		public async Task Confirm()
		{
			var result = await userManagement.ConfirmUser(new Dictionary<string, string>()
			{
				{ "username", Email },
				{ "code", ConfirmationCode }
			});

			if (result.Okay)
			{
				Confirmed?.Invoke();
			}
			else
			{
				Message = $"Der angegebene Code stimmt nicht: {result.Message}";
			}
		}

		public async Task Resend()
		{
			var result = await userManagement.ResendCode(new Dictionary<string, string>()
			{
				{ "username", Email },
			});

			if (result.Okay)
			{
				Message = $"Code wurde zugestellt -- schauen Sie in Ihrem Postfach nach!";
			}
			else
			{
				Message = $"Code konnte nicht zugestellt werden: {result.Message}";
			}
		}
	}
}
