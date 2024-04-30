using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth
{
	public interface IConfirmViewModel : INotifyPropertyChanged
	{
		public string Email { get; }
		public string ConfirmationCode { get; }
		public string Message { get; }

		public event Action Confirmed;

		public Task Confirm();
		public Task Resend();
	}
}
