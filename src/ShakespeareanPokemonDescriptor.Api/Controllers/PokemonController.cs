using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShakespeareanPokemonDescriptor.Services;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Api.Controllers
{
    public class PokemonController : V1Controller
    {
        private readonly ILogger _logger;
        private readonly IDiscriptorService _service;

        public PokemonController(ILogger<PokemonController> logger, IDiscriptorService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(nameof(name));
            }
            _logger.LogDebug($"Request incoming with Pokemon name {name}");
            var descriptorResult = await _service.Describe(name, RequestLanguage, HttpContext.RequestAborted);
            return Ok(descriptorResult);
        }
    }
}
