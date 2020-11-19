using System;
using System.Threading.Tasks;
using System.Threading;

namespace ShakespeareanPokemonDescriptor.Services
{
    public interface IDiscriptorService
    {
        Task<string> Describe(string name, string language, CancellationToken cancellationToken);
    }
}
