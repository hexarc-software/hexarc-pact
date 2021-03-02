using Microsoft.AspNetCore.Mvc;
using Hexarc.Pact.AspNetCore.Attributes;
using Hexarc.Pact.Demo.Api.Models;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Animals"), PactScope("Animals")]
    public sealed class AnimalController : ControllerBase
    {
        [HttpGet, Route(nameof(GetAnimal))]
        public Animal? GetAnimal() => null;
    }
}
