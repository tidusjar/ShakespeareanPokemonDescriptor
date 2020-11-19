using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.FunTranslationsApi.Models
{
    public class Success
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
