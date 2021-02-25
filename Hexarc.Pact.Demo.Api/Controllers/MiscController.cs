using System;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Misc")]
    public sealed class MiscController : ControllerBase
    {
        [HttpGet, Route(nameof(Ping))]
        public String Ping([FromQuery] String message) => $"Hello, {message}";

        [HttpGet, Route(nameof(Sum))]
        public Int32 Sum([FromQuery] Int32 a, [FromQuery] Int32 b) => a + b;

        [HttpGet, Route(nameof(Random))]
        public ActionResult<Double> Random() => new Random().NextDouble();
    }
}
