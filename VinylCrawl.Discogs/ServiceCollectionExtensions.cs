using Microsoft.Extensions.Configuration;
using VinylCrawl.Discogs.Agents;
using VinylCrawl.Discogs.Agents.Interfaces;

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
            return services;
        }
    }
}
