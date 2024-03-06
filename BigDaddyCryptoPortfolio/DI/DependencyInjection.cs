using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.ViewModels;
using BigDaddyCryptoPortfolio.ViewModels.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.DI
{
    internal static class DependencyInjection
	{
		public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder appBuilder)
		{
            appBuilder.Services.AddSingleton<ICoinsViewModel, CoinsViewModel>();
			appBuilder.Services.AddSingleton<IPortfolioViewModel, PortfolioViewModel>();
			appBuilder.Services.AddSingleton<ISettingsViewModel, SettingsViewModel>();

			appBuilder.Services.AddSingleton<IAppUiControl, AppUiController>();
		
            return appBuilder;
		}
	}
}
