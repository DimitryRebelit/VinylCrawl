using VinylCrawl.Discogs;
using VinylCrawl.ViewModels;
using VinylCrawl.Views;

namespace VinylCrawl;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoginView>();
        builder.Services.AddDiscogsModule(builder.Configuration);
		return builder.Build();
	}
}
