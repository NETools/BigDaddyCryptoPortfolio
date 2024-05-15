using BigDaddyCryptoPortfolio.Adapters.API.Bitvavo.Networking;
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models.Dtos;
using BigDaddyCryptoPortfolio.Models.Exchange;
using BigDaddyCryptoPortfolio.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth
{
	internal class LoginViewModel (
		IUserManagement userManagement, 
		ISynchronizationManagement<MessageBusNotification, MessageBusRetrievalMessage> synchronizationManagement, 
		IUserSession userSession, 
		ICoinsViewModel coinsViewModel, 
		AuthSucceededAdapter context) : ILoginViewModel
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

				var response = await synchronizationManagement.Retrieve(new MessageBusNotification()
				{
					ChanneId = "PortfolioExchangeService",
					MessageId = "RetrievePortfolio",
					StructData = new RetrievePortfolioMessage()
					{
						Username = Email
					}.ToJsonBytes(Encoding.UTF8),
					GenericMessageType = Models.GenericMessageType.RetrievePortfolio
				});

				if(response.Result.RetrievalType != Models.RetrievalType.Portfolio)
				{
					throw new InvalidOperationException("Something went wrong!");
				}

				var jsonString = Encoding.UTF8.GetString(response.Result.StructBuffer);
				var symbols = JsonSerializer.Deserialize<List<string>>(jsonString);

				foreach (var symbol in symbols)
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
