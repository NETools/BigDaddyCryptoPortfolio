using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.ViewModels.Auth
{
	internal class AuthSucceededAdapter(IAppUiControl context, IServiceProvider serviceProvider)
	{
		public void Prepare()
		{
			var coinsViewModel = serviceProvider.GetService<ICoinsViewModel>();
			var portfolioViewModel = serviceProvider.GetService<IPortfolioViewModel>();
			var assetManagerViewModel = serviceProvider.GetService<IAssetManagerViewModel>();

			if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
				context.RemoveTab("Authentication");

			context.AddTab(new ShellContent()
			{
				Icon = "coin_stack.png",
				Title = "Coins",
				Route = "CoinsView",
				ContentTemplate = new DataTemplate(() => new CoinsView(coinsViewModel, serviceProvider)),
			});

			context.AddTab(new ShellContent()
			{
				Icon = "asset_management.png",
				Title = "Asset Manager",
				Route = "AssetManagerView",
				ContentTemplate = new DataTemplate(() => new AssetManagerView(assetManagerViewModel, coinsViewModel, portfolioViewModel))
			});

			context.AddTab(new ShellContent()
			{
				Icon = "portfolio.png",
				Title = "Portfolio",
				Route = "PortfolioView",
				ContentTemplate = new DataTemplate(() => new PortfolioView(coinsViewModel, portfolioViewModel))
			});

			if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
				context.RemoveTab("Authentication");
		}
	}
}
