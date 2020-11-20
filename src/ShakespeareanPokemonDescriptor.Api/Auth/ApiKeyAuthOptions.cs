using Microsoft.AspNetCore.Authentication;

namespace ShakespeareanPokemonDescriptor.Api.Auth
{
    public class ApiKeyAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "API Key";
        public string Scheme => DefaultScheme;
        public string AuthenticationType = DefaultScheme;
    }
}
