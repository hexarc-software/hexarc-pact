using Microsoft.AspNetCore.Mvc;
using Hexarc.Annotations;
using Hexarc.Pact.Demo.Api.Models;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Animals")]
    public sealed class AnimalController : ControllerBase
    {
        [HttpGet, Route(nameof(GetAnimal))]
        [return: NullableReference]
        public Animal? GetAnimal() => null;
    }
}
