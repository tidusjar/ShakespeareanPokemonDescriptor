using Microsoft.Extensions.Logging;
using ShakespeareanPokemonDescriptor.FunTranslationsApi.Models;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.PokeApi
{
    public interface ITranslatorApiClient
    {
        Task<TranslationResult> Translate(string searchTerm, CancellationToken token);
    }
}
