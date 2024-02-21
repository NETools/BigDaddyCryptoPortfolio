using BigDaddyCryptoPortfolio.DI;
using BigDaddyCryptoPortfolio.Views;
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
				.RegisterViewModels()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});
#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
