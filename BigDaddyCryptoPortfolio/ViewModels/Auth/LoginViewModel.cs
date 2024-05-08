using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth
{
	internal class LoginViewModel (IUserManagement userManagement, ISynchronizationManagement<string, List<string>> synchronizationManagement, IUserSession userSession, ICoinsViewModel coinsViewModel, AuthSucceededAdapter context) : ILoginViewModel
	{
		public string Email { get; set; } = "enes.hergul215@gmail.com";
		public string Password { get; set; } = "test215X[]";

		private string? _message;
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
		
		public event Action NotConfirmed;

		public async Task LogIn()
		{
			var result = await userManagement.Login(new Dictionary<string, string>()
			{
				{ "username", Email },
				{ "password", Password }
			});

			if (result.Okay)
			{
				Message = "Nutzer wurde erfolgreich eingeloggt.";
				userSession.StartSession(Email);

				var response = await synchronizationManagement.Retrieve();
				foreach (var symbol in response.Result)
				{
					await coinsViewModel.AddCoin(symbol, false);
				}


				context.Prepare();
			}
			else
			{
				if (result.Code == System.Net.HttpStatusCode.NotAcceptable)
				{
					NotConfirmed?.Invoke();
				}
				else
					Message = $"Nutzer konnte nicht eingeloggt werden: {result.Message}";
			}
		}
	}
}
