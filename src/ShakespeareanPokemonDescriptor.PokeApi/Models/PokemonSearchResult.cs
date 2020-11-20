using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.PokeApi.Models
{
    public class PokemonSearchResult
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
