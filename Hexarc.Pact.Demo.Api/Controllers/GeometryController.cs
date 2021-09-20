namespace Hexarc.Pact.Demo.Api.Controllers;

using System;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Pact.Demo.Api.Models;

[ApiController, Route("Geometry")]
public sealed class GeometryController : ControllerBase
{
    [HttpPost, Route(nameof(Area))]
    public Double Area(Triangle triangle) => triangle.CalculateArea();

    [HttpGet, Route(nameof(ComputeCollection))]
    public GeometryCollection? ComputeCollection() => null;
}
