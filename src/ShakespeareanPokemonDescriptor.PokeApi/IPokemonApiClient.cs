using ShakespeareanPokemonDescriptor.PokeApi.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.PokeApi
{
    public interface IPokemonApiClient
    {
        Task<PokemonSearchResult> SearchPokemon(string searchTerm, CancellationToken token);
        Task<PokemonSpeciesSearchResult> GetSpecies(int speciesId, CancellationToken token);
    }
}
