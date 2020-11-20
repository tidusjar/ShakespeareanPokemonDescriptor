namespace ShakespeareanPokemonDescriptor.Services
{
    public static class LanguageHelper
    {
        public const string DefaultLanguage = "en";
        public static string TwoCharacterLanguageCode(string languageInput)
        {
            if (string.IsNullOrEmpty(languageInput))
            {
                return DefaultLanguage;
            }
            var lanSplit = languageInput.Split('-');
            if (lanSplit.Length > 1 || lanSplit.Length == 1)
            {
                return lanSplit[0];
            }
            return DefaultLanguage;
        }
    }
}
