using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShakespeareanPokemonDescriptor.PokeApi;
using ShakespeareanPokemonDescriptor.Services;
using System;

namespace ShakespeareanPokemonDescriptor.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureApplicationDependancies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPokemonApiClient, PokeApiClient>(x =>
            {
                x.BaseAddress = new Uri(configuration.GetValue<string>("Endpoints:Pokemon"));
            });
            services.AddTransient<IDiscriptorService, PokemonDescriptorService>();

            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();

            return services;
        }
    }
}
