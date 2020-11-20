using System;
using System.Threading.Tasks;
using System.Threading;
using ShakespeareanPokemonDescriptor.Services.Models;

namespace ShakespeareanPokemonDescriptor.Services
{
    public interface IDiscriptorService
    {
        Task<DescriptorResult> Describe(string name, string language, CancellationToken cancellationToken);
    }
}
