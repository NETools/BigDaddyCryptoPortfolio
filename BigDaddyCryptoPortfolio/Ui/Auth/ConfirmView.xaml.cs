using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Models;

namespace BigDaddyCryptoPortfolio.Ui.Auth;

public partial class ConfirmView : ContentView, IViewChange<AuthViews>
{
	public event Action<AuthViews> ViewChange;

	private IConfirmViewModel _confirmViewModel;

	public ConfirmView(IConfirmViewModel confirmViewModel)
	{
		InitializeComponent();

		this.BindingContext = confirmViewModel;
		
		_confirmViewModel = confirmViewModel;
		_confirmViewModel.Confirmed += OnConfirmed;
		_confirmViewModel.PropertyChanged += OnPropertyChanged;
	}

	private async void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
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

	private void OnConfirmed()
	{
		ViewChange?.Invoke(AuthViews.Login);
	}

	private async void OnButtonClicked(object sender, EventArgs e)
	{

		EmailEntry.IsEnabled = false;
		CodeEntry.IsEnabled = false;
		ConfirmButton.IsEnabled = false;

		await ConfirmButton.ScaleTo(0.9, 100);
		await ConfirmButton.ScaleTo(1.0, 100);

		await _confirmViewModel.Confirm();

		EmailEntry.IsEnabled = true;
		CodeEntry.IsEnabled = true;
		ConfirmButton.IsEnabled = true;
	}

	private async void OnSendCodeAgainTapped(object sender, TappedEventArgs e)
	{
		await _confirmViewModel.Resend();
	}

	private void OnGoBackTapped(object sender, TappedEventArgs e)
	{
		ViewChange?.Invoke(AuthViews.Login);
	}
}