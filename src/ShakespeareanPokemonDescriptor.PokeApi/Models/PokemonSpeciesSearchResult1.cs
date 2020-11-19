using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.PokeApi.Models
{
    public class PokemonSpeciesSearchResult
    {
        [JsonPropertyName("flavor_text_entries")]
        public List<PokemonSpeciesFlavourResult> Results { get; set; }
    }
}
