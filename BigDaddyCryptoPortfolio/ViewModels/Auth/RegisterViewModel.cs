using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth
{
    internal class RegisterViewModel(IUserManagement userManagement) : IRegisterViewModel
    {
		public string Email { get; set; } = "enes.hergul215@gmail.com";
		public string Password { get; set; } = "test215X[]";

        private string? _message;
        public string Message
        {
            get => _message;
            private set
            {
                _message = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public async Task Register()
        {
            var result = await userManagement.Register(new Dictionary<string, string>()
            {
                { "username", Email },
                { "password", Password }
            });

            if (result.Okay)
            {
                Message = "Nutzer wurde erfolgreich registriert.";
			}
            else
            {
                Message = $"Nutzer konnte nicht registriert werden: {result.Message}";
			}
        }
    }
}
