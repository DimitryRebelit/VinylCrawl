using Microsoft.Extensions.Configuration;
using VinylCrawl.Discogs.Agents;
using VinylCrawl.Discogs.Agents.Interfaces;
using VinylCrawl.Discogs.Services;
using VinylCrawl.Discogs.Services.Interfaces;

namespace VinylCrawl.Discogs
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscogsModule(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ICollectionAgent, CollectionAgent>();
            services.AddScoped<IAuthAgent, AuthAgent>();
            services.AddScoped<IIdentityAgent, IdentityAgent>();
            services.AddScoped<IDiscogsService, DiscogsService>();
            return services;
        }
    }
}
