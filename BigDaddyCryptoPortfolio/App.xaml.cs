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
            var settingsViewModel = services.GetService<ISettingsViewModel>();
            var portfolioViewModel = services.GetService<IPortfolioViewModel>();
            var coinsViewModel = services.GetService<ICoinsViewModel>();

            return new SettingsView(settingsViewModel, coinsViewModel, portfolioViewModel);    
        }
    }

    public partial class App : Application
	{
		public App(ICoinsViewModel coinsViewModel, IAppUiControl context, IPortfolioViewModel portfolioViewModel, ISettingsViewModel settingsViewModel)
		{
			InitializeComponent();

            context.AddTabRequested += OnAddTabRequested;

            Routing.RegisterRoute("views/coins/settings", new Test());


			Tabs.Items.Add(new ShellContent()
			{
				Title = "Coins",
				Route = "CoinsView",
				ContentTemplate = new DataTemplate(() => new CoinsView(coinsViewModel)),
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
				ContentTemplate = new DataTemplate(() => new PortfolioView(portfolioViewModel))
			});
		}

        private void OnAddTabRequested(string tabName)
        {

        }
	}
}
