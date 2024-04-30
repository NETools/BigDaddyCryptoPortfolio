using BigDaddyCryptoPortfolio.Adapters.API.Gecko;
using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.Views;
using System.Collections.Specialized;

namespace BigDaddyCryptoPortfolio
{

    class Settings : RouteFactory
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
		public App(IAppUiControl context, IRegisterViewModel registerViewModel, ILoginViewModel loginViewModel, IConfirmViewModel confirmViewModel, ICoinDataProvider coinDataProvider)
		{
            InitializeComponent();
            coinDataProvider.LoadCoins();

            context.AddTabRequested += OnAddTabRequested;
			context.RemoveTabRequested += OnRemoveTabRequested;

            Routing.RegisterRoute("views/coins/settings", new Settings());

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
