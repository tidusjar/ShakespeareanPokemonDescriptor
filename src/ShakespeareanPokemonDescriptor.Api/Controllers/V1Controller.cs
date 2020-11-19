using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Api.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class V1Controller : ControllerBase
    {
        public string RequestLanguage => Request.GetTypedHeaders()
                        .AcceptLanguage?
                        .OrderByDescending(x => x.Quality ?? 1)
                        .Select(x => x.Value.ToString())
                        .FirstOrDefault();
    }
}
