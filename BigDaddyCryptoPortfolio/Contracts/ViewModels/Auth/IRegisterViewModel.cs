using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth
{
    public interface IRegisterViewModel : INotifyPropertyChanged
    {
		public string Email { get; set; }
		public string Password { get; set; }
        public string Message { get; }

        public Task Register();
    }
}
