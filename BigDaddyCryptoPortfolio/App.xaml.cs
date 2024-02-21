using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Views;
using System.Collections.Specialized;

namespace BigDaddyCryptoPortfolio
{
	public partial class App : Application
	{
		public App(ICoinsViewModel coinsViewModel)
		{
			InitializeComponent();
			
			Tabs.Items.Add(new ShellContent()
			{
				Title = "Coins",
				Route = "CoinsView",
				ContentTemplate = new DataTemplate(() => new CoinsView(coinsViewModel))
			});


			Tabs.Items.Add(new ShellContent()
			{
				Title = "Portfolio",
				Route = "PortfolioView",
				ContentTemplate = new DataTemplate(() => new PortfolioView())
			});


			Tabs.Items.Add(new ShellContent()
			{
				Title = "Evaluation",
				Route = "EvaluationView",
				ContentTemplate = new DataTemplate(() => new EvaluationView())
			});

		}

		private async void OnCollectionChanged(object? sender, NotifyCollectionChangedAction e)
		{

		}
	}
}
