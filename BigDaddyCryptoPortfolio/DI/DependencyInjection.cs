using BigDaddyCryptoPortfolio.Adapters.API.Auth;
using BigDaddyCryptoPortfolio.Adapters.Data;
using BigDaddyCryptoPortfolio.Contracts.Adapters;
using BigDaddyCryptoPortfolio.Contracts.Adapters.UserManagement;
using BigDaddyCryptoPortfolio.Contracts.AppControls;
using BigDaddyCryptoPortfolio.Contracts.ViewModels;
using BigDaddyCryptoPortfolio.Contracts.ViewModels.Auth;
using BigDaddyCryptoPortfolio.ViewModels;
using BigDaddyCryptoPortfolio.ViewModels.Auth;
using BigDaddyCryptoPortfolio.ViewModels.Auth.Session;
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
			appBuilder.Services.AddSingleton<IAssetManagerViewModel, AssetManagerViewModel>();
            appBuilder.Services.AddSingleton<ICoinsViewModel, CoinsViewModel>();
			appBuilder.Services.AddSingleton<IPortfolioViewModel, PortfolioViewModel>();
			appBuilder.Services.AddSingleton<IBitvavoSynchronizationViewModel, BitvavoSynchronizationViewModel>();

			appBuilder.Services.AddSingleton<IAppUiControl, AppUiController>();

			appBuilder.Services.AddSingleton<ICoinDataProvider, CoinDataProvider>();

			appBuilder.Services.AddSingleton<IRegisterViewModel, RegisterViewModel>();
			appBuilder.Services.AddSingleton<ILoginViewModel, LoginViewModel>();
			appBuilder.Services.AddSingleton<IConfirmViewModel, ConfirmViewModel>();

			appBuilder.Services.AddSingleton<AuthSucceededAdapter>();

			appBuilder.Services.AddSingleton<IUserManagement, CognitoUserManagement>();
			appBuilder.Services.AddSingleton<IUserSession, UserSession>();
            return appBuilder;
		}
	}
}
