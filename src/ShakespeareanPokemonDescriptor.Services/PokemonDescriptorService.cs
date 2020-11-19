using System;
using System.Threading.Tasks;
using System.Threading;
using ShakespeareanPokemonDescriptor.PokeApi;

namespace ShakespeareanPokemonDescriptor.Services
{
    public class PokemonDescriptorService : IDiscriptorService
    {
        private readonly IPokemonApiClient _pokemonApiClient;

        public PokemonDescriptorService(IPokemonApiClient pokemonApiClient)
        {
            _pokemonApiClient = pokemonApiClient;
        }

        public async Task<string> Describe(string name, string language, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
