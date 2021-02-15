using System;
using Hexarc.Pact.Demo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.Demo.Api.Controllers
{
    [ApiController, Route("Geometry")]
    public sealed class GeometryController : ControllerBase
    {
        [HttpPost, Route(nameof(Area))]
        public Double Area(Triangle triangle) => triangle.CalculateArea();
    }
}