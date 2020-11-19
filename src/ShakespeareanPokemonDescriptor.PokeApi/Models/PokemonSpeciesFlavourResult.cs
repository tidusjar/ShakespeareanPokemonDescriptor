using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.PokeApi.Models
{
    public class PokemonSpeciesFlavourResult
    {
        [JsonPropertyName("flavor_text")]
        public string Text { get; set; }
        [JsonPropertyName("language")]
        public LanguageResult Language { get; set; }
    }
}
