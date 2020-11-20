using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShakespeareanPokemonDescriptor.Api.ViewModels;
using ShakespeareanPokemonDescriptor.Services;
using System.Net;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Api.Controllers
{
    [Authorize]
    public class PokemonController : V1Controller
    {
        private readonly ILogger _logger;
        private readonly IDiscriptorService _service;

        public PokemonController(ILogger<PokemonController> logger, IDiscriptorService service)
        {
            _logger = logger;
            _service = service;
        }

        /// <summary>
        /// Return the description of a pokemon in a shakespearean format.
        /// </summary>
        /// <remarks>
        /// There are multiple descriptions of a single Pokemon, you will currently recieve a random description each time you call this API
        ///
        /// Sample:
        ///
        ///     GET /Pokemon/Charizard
        ///     {
        ///         "name": "charizard",
        ///         "description": "Spits fire yond is hot enow to melt boulders. Known to cause forest fires unintentionally."
        ///     }
        /// </remarks>
        /// <param name="name">Pokemon name or PokeApi ID</param>
        /// <response code="400">Missing the name from the route</response>
        /// <response code="404">Could not find pokemon, please check spelling</response>
        /// <response code="401">Could not authenticate, validate API Key</response>
        /// <response code="200">Successful Response</response>
        [HttpGet("{name}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(nameof(name));
            }
            _logger.LogDebug($"Request incoming with Pokemon name {name}");
            var descriptorResult = await _service.Describe(name, RequestLanguage, HttpContext.RequestAborted);

            if (descriptorResult == null)
            {
                return NotFound("Invalid pokemon");
            }

            return Ok(new PokemonDescriptionViewModel
            {
                Name = descriptorResult.Name,
                Description = descriptorResult.Description
            });
        }
    }
}
