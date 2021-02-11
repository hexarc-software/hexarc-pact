using System;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Misc")]
    public sealed class MiscController : ControllerBase
    {
        [HttpGet, Route(nameof(Ping))]
        public String Ping([FromQuery]String message) => $"Hello, {message}";
    }
}
