using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Api.Auth
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        private const string ProblemDetailsContentType = "application/problem+json";
        private const string ApiKeyHeaderName = "X-Api-Key";

        private readonly string _apiKey;

        public ApiKeyAuthHandler(
            IOptionsMonitor<ApiKeyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _apiKey = configuration.GetValue<string>("ApiKey");
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var apiKeyHeader = apiKeyHeaderValues.FirstOrDefault();

            if (apiKeyHeaderValues.Count == 0 || string.IsNullOrWhiteSpace(apiKeyHeader))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            if (apiKeyHeader.Equals(_apiKey, StringComparison.InvariantCultureIgnoreCase))
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "ApiKey") // TODO if using Key to lookup from DB, we should store who this belongs to
            };

                var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, Options.Scheme);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Response.ContentType = ProblemDetailsContentType;
            await Response.WriteAsync(JsonSerializer.Serialize("Invalid or Missing API Key"));
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Response.ContentType = ProblemDetailsContentType;
            await Response.WriteAsync(JsonSerializer.Serialize("Invalid or Missing API Key"));
        }
    }
}
