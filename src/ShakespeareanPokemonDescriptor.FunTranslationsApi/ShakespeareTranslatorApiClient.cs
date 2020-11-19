using Microsoft.Extensions.Logging;
using ShakespeareanPokemonDescriptor.FunTranslationsApi.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.PokeApi
{
    public class ShakespeareTranslatorApiClient : ITranslatorApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public ShakespeareTranslatorApiClient(HttpClient httpClient, ILogger<ShakespeareTranslatorApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<TranslationResult> Translate(string searchTerm, CancellationToken token)
        {
            var result = await _httpClient.GetAsync($"shakespeare.json?text={searchTerm}", HttpCompletionOption.ResponseHeadersRead, token);

            if (!result.IsSuccessStatusCode)
            {
                _logger.LogError($"'{_httpClient.BaseAddress}' returned {result.StatusCode} because: {result.ReasonPhrase}");
                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                result.EnsureSuccessStatusCode();
            }

            var content = await JsonSerializer.DeserializeAsync<TranslationResult>(await result.Content.ReadAsStreamAsync());
            return content;
        }
    }
}
