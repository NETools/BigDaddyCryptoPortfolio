using BigDaddyCryptoPortfolio.DI;
using BigDaddyCryptoPortfolio.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BigDaddyCryptoPortfolio
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseMauiCommunityToolkit()
				.RegisterViewModels()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("alata-regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
