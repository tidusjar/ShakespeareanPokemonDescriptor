using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.FunTranslationsApi.Models
{
    public class TranslationResult
    {
        [JsonPropertyName("name")]
        public Success Success { get; set; }

        [JsonPropertyName("contents")]
        public TranslationContent Content { get; set; }
    }
}
