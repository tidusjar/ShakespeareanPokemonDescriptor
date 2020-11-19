using System;
using System.Threading.Tasks;
using System.Threading;
using ShakespeareanPokemonDescriptor.PokeApi;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ShakespeareanPokemonDescriptor.Services
{
    public class PokemonDescriptorService : IDiscriptorService
    {
        private readonly IPokemonApiClient _pokemonApiClient;
        private readonly ILogger<PokemonDescriptorService> _logger;
        private readonly ICacheService _cacheService;

        public PokemonDescriptorService(IPokemonApiClient pokemonApiClient, ILogger<PokemonDescriptorService> logger, ICacheService cacheService)
        {
            _pokemonApiClient = pokemonApiClient;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<string> Describe(string name, string language, CancellationToken cancellationToken)
        {

            var speciesResult = await _cacheService.GetOrAddAsync(CacheKeys.PokemonSearchKey + name, async () => await _pokemonApiClient.SearchPokemon(name, cancellationToken),
                cancellationToken: cancellationToken);

            if (speciesResult == null)
            {
                _logger.LogInformation($"Search for Pokemon {name} returned a 404");
                return string.Empty;
            }
            var speciesText = await _cacheService.GetOrAddAsync(CacheKeys.PokemonSpeciesKey + speciesResult.Id, async () => await _pokemonApiClient.GetSpecies(speciesResult.Id, cancellationToken),
                cancellationToken: cancellationToken);

            if (speciesText == null)
            {
                _logger.LogInformation($"Species search Id {speciesResult.Id} returned a 404");
                return string.Empty;
            }

            var searchLang = LanguageHelper.TwoCharacterLanguageCode(language);
            var allDescriptions = speciesText.Results.Where(x => x.Language.Name.Equals(searchLang, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Text);

        }
    }
}
