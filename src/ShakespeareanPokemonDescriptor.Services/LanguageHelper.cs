namespace ShakespeareanPokemonDescriptor.Services
{
    public static class LanguageHelper
    {
        public static string TwoCharacterLanguageCode(string languageInput)
        {
            if (string.IsNullOrEmpty(languageInput))
            {
                return "en";
            }
            var lanSplit = languageInput.Split('-');
            if (lanSplit.Length > 1 || lanSplit.Length == 1)
            {
                return lanSplit[0];
            }
            return "en";
        }
    }
}
