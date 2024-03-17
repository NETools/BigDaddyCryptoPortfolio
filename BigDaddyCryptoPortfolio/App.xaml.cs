using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Views;
using System.Collections.Specialized;

namespace BigDaddyCryptoPortfolio
{

    class Test : RouteFactory
    {
        public override Element GetOrCreate()
        {
            throw new NotImplementedException();
        }

        public override Element GetOrCreate(IServiceProvider services)
        {
            var settingsViewModel = services.GetService<IBitvavoSynchronizationViewModel>();
            var portfolioViewModel = services.GetService<IPortfolioViewModel>();
            var coinsViewModel = services.GetService<ICoinsViewModel>();

            var coinDataProvider = services.GetService<ICoinDataProvider>();

            return new BitvavoSynchronizationView(settingsViewModel, coinsViewModel, portfolioViewModel, services, coinDataProvider);    
        }
    }

    public partial class App : Application
	{
		public App(ICoinsViewModel coinsViewModel, IAppUiControl context, IPortfolioViewModel portfolioViewModel, IBitvavoSynchronizationViewModel settingsViewModel, IServiceProvider serviceProvider, ICoinDataProvider coinDataProvider)
		{
			InitializeComponent();
            coinDataProvider.LoadCoins();

            context.AddTabRequested += OnAddTabRequested;

            Routing.RegisterRoute("views/coins/settings", new Test());


			Tabs.Items.Add(new ShellContent()
			{
				Title = "Coins",
				Route = "CoinsView",
				ContentTemplate = new DataTemplate(() => new CoinsView(coinsViewModel, serviceProvider)),
			});

            Tabs.Items.Add(new ShellContent()
            {
                Title = "Asset Manager",
                Route = "AssetManagerView",
                ContentTemplate = new DataTemplate(() => new AssetManagerView())
            });

            Tabs.Items.Add(new ShellContent()
			{
				Title = "Portfolio",
				Route = "PortfolioView",
				ContentTemplate = new DataTemplate(() => new PortfolioView(coinsViewModel, portfolioViewModel))
			});
		}

        private void OnAddTabRequested(string tabName)
        {

        }
	}
}
