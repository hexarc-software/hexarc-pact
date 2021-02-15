using System;
using Hexarc.Annotations;
using Hexarc.Pact.Demo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Geometry")]
    public sealed class GeometryController : ControllerBase
    {
        [HttpPost, Route(nameof(Area))]
        public Double Area(Triangle triangle) => triangle.CalculateArea();

        [HttpGet, Route(nameof(ComputeCollection))]
        [return: NullableReference]
        public GeometryCollection? ComputeCollection() => null;
    }
}
