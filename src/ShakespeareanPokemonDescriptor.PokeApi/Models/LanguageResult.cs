using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.PokeApi.Models
{
    public class LanguageResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
