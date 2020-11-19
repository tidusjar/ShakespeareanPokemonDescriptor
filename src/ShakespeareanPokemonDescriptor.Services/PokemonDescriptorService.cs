using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ShakespeareanPokemonDescriptor.Services
{
    public class PokemonDescriptorService : IDiscriptorService
    {
        public async Task<string> Describe(string name, string language, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
