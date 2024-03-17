using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.ViewModels;
using System.Net.WebSockets;

namespace BigDaddyCryptoPortfolio.Views;

public partial class BitvavoSynchronizationView : ContentPage
{
	private IBitvavoSynchronizationViewModel _settingsViewModel;
	private ICoinsViewModel _coinsViewModel;
	private IPortfolioViewModel _portfolioViewModel;

	private ICoinDataProvider _coinDataProvider;

	private IServiceProvider _serviceProvider;

	public BitvavoSynchronizationView(IBitvavoSynchronizationViewModel settingsViewModel, ICoinsViewModel coinsViewModel, IPortfolioViewModel portfolioViewModel, IServiceProvider serviceProvider, ICoinDataProvider coinDataProvider)
	{
		_coinDataProvider = coinDataProvider;
		_portfolioViewModel = portfolioViewModel;
		_settingsViewModel = settingsViewModel;
		_coinsViewModel = coinsViewModel;
		_serviceProvider = serviceProvider;	

		InitializeComponent();

		this.BindingContext = settingsViewModel;
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var authorized = await _settingsViewModel.Authorize();
		if (authorized)
		{
            await foreach(var coin in _settingsViewModel.SynchronizePortfolio(_coinDataProvider, _coinsViewModel))
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

    private async void OnDownloadCodeFromServerClicked(object sender, EventArgs e)
    {
        var websocket = new ClientWebSocket();
        await websocket.ConnectAsync(new Uri("ws://178.25.225.236:8000/"), CancellationToken.None);

        var codeLoader = new RemoteCodeLoader.RemoteCodeLoader(websocket);
        codeLoader.AddLocalAssembly(typeof(ICoinsViewModel));
        codeLoader.AddLocalAssembly(typeof(CoinsViewModel));

        var page = await codeLoader.CreateXamlElement<ContentPage>("rcns://views/contentpages/demo.xaml", _serviceProvider);

        await Navigation.PushAsync(page);
    }
}