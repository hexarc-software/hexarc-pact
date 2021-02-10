using System;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Rpc.Demo.Api.Models;

namespace Hexarc.Rpc.Demo.Api.Controllers
{
    [ApiController, Route("Geometry")]
    public sealed class GeometryController : ControllerBase
    {
        [HttpPost, Route(nameof(Area))]
        public Double Area(Triangle triangle) => triangle.CalculateArea();
    }
}
