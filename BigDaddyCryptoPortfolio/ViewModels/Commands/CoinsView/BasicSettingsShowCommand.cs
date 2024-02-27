using BigDaddyCryptoPortfolio.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BigDaddyCryptoPortfolio.ViewModels.Commands.CoinsView
{
    internal class BasicSettingsShowCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            await Shell.Current.GoToAsync("views/coins/settings", true);
        }
    }
}
