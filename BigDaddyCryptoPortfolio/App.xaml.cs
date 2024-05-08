using BigDaddyCryptoPortfolio.Adapters.API.Gecko;
using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Views;
using BigDaddyCryptoPortfolio.Views.SecondOrder;
using System.Collections.Specialized;

namespace BigDaddyCryptoPortfolio
{

    class AboutRouteFactory : RouteFactory
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

            return new AboutView();    
        }
    }

    public partial class App : Application
	{
		public App(IAppUiControl context, IRegisterViewModel registerViewModel, ILoginViewModel loginViewModel, IConfirmViewModel confirmViewModel, ICoinDataProvider coinDataProvider)
		{
            InitializeComponent();
            coinDataProvider.LoadCoins();

            context.AddTabRequested += OnAddTabRequested;
			context.RemoveTabRequested += OnRemoveTabRequested;

            Routing.RegisterRoute("views/coins/settings", new AboutRouteFactory());

            Tabs.Items.Add(new ShellContent()
            {
                Title = "Authentication",
                Route = "AuthenticationView",
                ContentTemplate = new DataTemplate(() => new AuthenticationView(registerViewModel, loginViewModel, confirmViewModel))
            });
		}

		private void OnRemoveTabRequested(string title)
		{
            Tabs.Items.Remove(Tabs.Items.First(p => p.Title == title));
		}

		private void OnAddTabRequested(ShellContent content)
        {
            Tabs.Items.Add(content);
        }
	}
}
