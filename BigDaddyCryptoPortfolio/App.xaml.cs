using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Views;
using System.Collections.Specialized;

namespace BigDaddyCryptoPortfolio
{
	public partial class App : Application
	{
		public App(ICoinsViewModel coinsViewModel, IAppUiControl context, IPortfolioViewModel portfolioViewModel)
		{
			InitializeComponent();

            context.AddTabRequested += OnAddTabRequested;

            Routing.RegisterRoute("views/coins/settings", typeof(SettingsView));


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
