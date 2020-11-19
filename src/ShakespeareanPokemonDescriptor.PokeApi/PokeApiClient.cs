using Microsoft.Extensions.Logging;
using ShakespeareanPokemonDescriptor.PokeApi.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.PokeApi
{
    public class PokeApiClient : IPokemonApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public PokeApiClient(HttpClient httpClient, ILogger<PokeApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PokemonSearchResult> SearchPokemon(string searchTerm, CancellationToken token)
        {
            var result = await _httpClient.GetAsync($"pokemon/{searchTerm.ToLowerInvariant()}", HttpCompletionOption.ResponseHeadersRead, token);

            if (!result.IsSuccessStatusCode)
            {
                _logger.LogError($"'{_httpClient.BaseAddress}' returned {result.StatusCode} because: {result.ReasonPhrase}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                result.EnsureSuccessStatusCode();
            }

            var content = await JsonSerializer.DeserializeAsync<PokemonSearchResult>(await result.Content.ReadAsStreamAsync());
            return content;
        }

        public async Task<PokemonSpeciesSearchResult> GetSpecies(int speciesId, CancellationToken token)
        {
            var result = await _httpClient.GetAsync($"pokemon-species/{speciesId}", HttpCompletionOption.ResponseHeadersRead, token);

            if (!result.IsSuccessStatusCode)
            {
                _logger.LogError($"'{_httpClient.BaseAddress}' returned {result.StatusCode} because: {result.ReasonPhrase}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                result.EnsureSuccessStatusCode();
            }

            var content = await JsonSerializer.DeserializeAsync<PokemonSpeciesSearchResult>(await result.Content.ReadAsStreamAsync());
            return content;
        }
    }
}
