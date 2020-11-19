using System.Text.Json.Serialization;

namespace ShakespeareanPokemonDescriptor.FunTranslationsApi.Models
{
    public class TranslationContent
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("translation")]
        public string Type { get; set; }
    }
}
