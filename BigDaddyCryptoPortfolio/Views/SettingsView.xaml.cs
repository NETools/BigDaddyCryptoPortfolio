using BigDaddyCryptoPortfolio.Contracts.ViewModels;

namespace BigDaddyCryptoPortfolio.Views;

public partial class SettingsView : ContentPage
{
	private ISettingsViewModel _settingsViewModel;
	private ICoinsViewModel _coinsViewModel;
	private IPortfolioViewModel _portfolioViewModel;

	public SettingsView(ISettingsViewModel settingsViewModel, ICoinsViewModel coinsViewModel, IPortfolioViewModel portfolioViewModel)
	{
		_portfolioViewModel = portfolioViewModel;
		_settingsViewModel = settingsViewModel;
		_coinsViewModel = coinsViewModel;

		InitializeComponent();

		this.BindingContext = settingsViewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var authorized = await _settingsViewModel.Authorize();
		if (authorized)
		{
            await foreach(var coin in _settingsViewModel.SynchronizePortfolio(_coinsViewModel, _portfolioViewModel))
			{
				await ShowInfoMessage($"{coin.Name} added to your portfolio!");
			}
		}
    }
	
	private async Task ShowInfoMessage(string infoMessage)
	{
        InfoLabel.Text = infoMessage;

        StatusBorder.IsVisible = true;
        StatusBorder.FadeTo(1, 1000);
        await StatusBorder.TranslateTo(0, 0, 1200);
        StatusBorder.FadeTo(0);
        await StatusBorder.TranslateTo(-20, 0);
        StatusBorder.IsVisible = false;
        await StatusBorder.TranslateTo(0, -20);
    }
}