using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models;

namespace BigDaddyCryptoPortfolio.Ui.Auth;

public partial class RegisterView : ContentView, IViewChange<AuthViews>
{
    private IRegisterViewModel _registerViewModel;

	public event Action<AuthViews> ViewChange;

	public RegisterView(IRegisterViewModel registerViewModel)
	{
        InitializeComponent();
        _registerViewModel = registerViewModel;

        this.BindingContext = _registerViewModel;

		_registerViewModel.PropertyChanged += OnViewModelPropertyChanged;
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
        ViewChange?.Invoke(AuthViews.Login);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		RegisterButton.IsEnabled = false;
		await RegisterButton.ScaleTo(0.9, 100);
		await RegisterButton.ScaleTo(1.0, 100);

		await _registerViewModel.Register();

		RegisterButton.IsEnabled = true;
	}
}