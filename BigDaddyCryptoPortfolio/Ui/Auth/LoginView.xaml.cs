using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models;

namespace BigDaddyCryptoPortfolio.Ui.Auth;

public partial class LoginView : ContentView, IViewChange<AuthViews>
{
	private ILoginViewModel _loginViewModel;

	public event Action<AuthViews> ViewChange;

	public LoginView(ILoginViewModel loginViewModel)
	{
		InitializeComponent();
		
		_loginViewModel = loginViewModel;
		
		_loginViewModel.PropertyChanged += OnViewModelPropertyChanged;
		_loginViewModel.NotConfirmed += OnNotConfirmed;

		BindingContext = _loginViewModel;
	}

	private void OnNotConfirmed()
	{
		ViewChange?.Invoke(AuthViews.Confirmation);
	}

	private async void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == "Message")
		{
			StatusBorder.IsVisible = true;
			StatusBorder.FadeTo(1, 1000);
			await StatusBorder.TranslateTo(0, 0, 1200);
			await Task.Delay(1200);
			StatusBorder.FadeTo(0);
			await StatusBorder.TranslateTo(-20, 0);
			StatusBorder.IsVisible = false;
			await StatusBorder.TranslateTo(0, -20);
		}
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
		ViewChange?.Invoke(AuthViews.Register);
    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
		LoginButton.IsEnabled = false;
		
		await LoginButton.ScaleTo(0.9, 100);
		await LoginButton.ScaleTo(1.0, 100);

		await _loginViewModel.LogIn();

		LoginButton.IsEnabled = true;
	}
}