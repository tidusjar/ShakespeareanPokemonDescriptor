using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ShakespeareanPokemonDescriptor.Api.Auth;
using ShakespeareanPokemonDescriptor.PokeApi;
using ShakespeareanPokemonDescriptor.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ShakespeareanPokemonDescriptor.Api.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection ConfigureApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IPokemonApiClient, PokeApiClient>(x =>
            {
                x.BaseAddress = new Uri(configuration.GetValue<string>("Endpoints:Pokemon"));
            });

            services.AddHttpClient<IPokemonApiClient, PokeApiClient>(x =>
            {
                x.BaseAddress = new Uri(configuration.GetValue<string>("Endpoints:Pokemon"));
            });
            services.AddHttpClient<ITranslatorApiClient, ShakespeareTranslatorApiClient>(x =>
            {
                x.BaseAddress = new Uri(configuration.GetValue<string>("Endpoints:Translator"));
            });
            services.AddTransient<IDiscriptorService, PokemonDescriptorService>();

            services.AddMemoryCache();
            services.AddScoped<ICacheService, CacheService>();
            AddSwagger(services);

            return services;
        }

        public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder)
        {
            return authenticationBuilder.AddScheme<ApiKeyAuthOptions, ApiKeyAuthHandler>(ApiKeyAuthOptions.DefaultScheme, options => { });
        }

        private static void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ShakespeareanPokemonDescriptor Api",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Jamie Rees",
                        Email = "tidusjar@gmail.com",
                        Url = new Uri("https://github.com/tidusjar/")
                    }
                });
                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "X-Api-Key",
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization by X-Api-Key inside request's header",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                };

                c.AddSecurityRequirement(requirement);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }
    }
}
