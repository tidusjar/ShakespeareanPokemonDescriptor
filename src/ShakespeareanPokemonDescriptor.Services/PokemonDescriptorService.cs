using System;
using System.Threading.Tasks;
using System.Threading;
using ShakespeareanPokemonDescriptor.PokeApi;
using Microsoft.Extensions.Logging;
using System.Linq;
using ShakespeareanPokemonDescriptor.Services.Models;

namespace ShakespeareanPokemonDescriptor.Services
{
    public class PokemonDescriptorService : IDiscriptorService
    {
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly ITranslatorApiClient _translatorApiClient;
        private readonly ILogger<PokemonDescriptorService> _logger;
        private readonly ICacheService _cacheService;

        private readonly Random _random = new Random();

        public PokemonDescriptorService(IPokemonApiClient pokemonApiClient, ITranslatorApiClient translatorApiClient,
                                        ILogger<PokemonDescriptorService> logger, ICacheService cacheService)
        {
            _pokemonApiClient = pokemonApiClient;
            _translatorApiClient = translatorApiClient;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<DescriptorResult> Describe(string name, string language, CancellationToken cancellationToken)
        {
            var searchResult = await _cacheService.GetOrAddAsync(CacheKeys.PokemonSearchKey + name, async () => await _pokemonApiClient.SearchPokemon(name, cancellationToken),
                cancellationToken: cancellationToken);

            if (searchResult == null)
            {
                _logger.LogInformation($"Search for Pokemon {name} returned a 404");
                return null;
            }
            var species = await _cacheService.GetOrAddAsync(CacheKeys.PokemonSpeciesKey + searchResult.Id, async () => await _pokemonApiClient.GetSpecies(searchResult.Id, cancellationToken),
                cancellationToken: cancellationToken);

            if (species == null)
            {
                _logger.LogInformation($"Species search Id {searchResult.Id} returned a 404");
                return null;
            }

            var searchLang = LanguageHelper.TwoCharacterLanguageCode(language);
            var descriptionByLang = species.Results.Where(x => x.Language.Name.Equals(searchLang, StringComparison.InvariantCultureIgnoreCase));
            if (!descriptionByLang.Any() && !searchLang.Equals(LanguageHelper.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase))
            {
                // Possible we do not have the specified language, fall back to the default
                descriptionByLang = species.Results.Where(x => x.Language.Name.Equals(LanguageHelper.DefaultLanguage, StringComparison.InvariantCultureIgnoreCase));
            }
            var allDescriptions = descriptionByLang.Select(x => x.Text).ToArray();

            var itemToTake = _random.Next(allDescriptions.Length - 1);
            var descriptionToTranslate = allDescriptions[itemToTake];

            var translationResult = await _translatorApiClient.Translate(descriptionToTranslate, cancellationToken);
            if (translationResult == null)
            {
                return null;
            }

            return new DescriptorResult
            {
                Description = translationResult.Content?.Translated ?? string.Empty,
                Name = searchResult.Name
            };
        }
    }
}
