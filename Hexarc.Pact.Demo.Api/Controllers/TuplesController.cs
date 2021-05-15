using System;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Tuples")]
    public sealed class TuplesController : ControllerBase
    {
        [HttpGet, Route(nameof(GetTupleOfTwo))]
        public (Int32, Int32) GetTupleOfTwo() => default;

        [HttpGet, Route(nameof(GetTupleOfTen))]
        public (Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32) GetTupleOfTen() => default;

        [HttpGet, Route(nameof(GetTupleWithNesting))]
        public (Int32, (Int32, (Int32, Int32)), Int32) GetTupleWithNesting() => default;
    }
}
