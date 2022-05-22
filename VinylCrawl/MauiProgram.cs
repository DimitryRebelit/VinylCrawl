using Microsoft.Maui.LifecycleEvents;
using VinylCrawl.Discogs;
using VinylCrawl.Discogs.Agents;
using VinylCrawl.Discogs.Agents.Interfaces;

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

		
        builder.Services                   
            .AddDiscogsModule(builder.Configuration);

		builder.Services.AddScoped<ICollectionAgent, CollectionAgent>();
		builder.Services.AddScoped<IAuthAgent, AuthAgent>();
		builder.Services.AddScoped<IIdentityAgent, IdentityAgent>();


		return builder.Build();
	}
}
