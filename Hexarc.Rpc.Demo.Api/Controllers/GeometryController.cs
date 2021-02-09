using System;
using Hexarc.Rpc.Demo.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Rpc.Demo.Api.Controllers
{
    [ApiController, Route("Geometry")]
    public sealed class GeometryController : ControllerBase
    {
        [HttpPost, Route(nameof(Area))]
        public Double Area(Triangle triangle)
        {
            return triangle.CalculateArea();
        }
    }
}
