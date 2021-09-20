namespace Hexarc.Pact.Demo.Api.Controllers;

using System;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("Tuples")]
public sealed class TuplesController : ControllerBase
{
    [HttpGet, Route(nameof(GetTupleOfTwo))]
    public (Int32 x, Int32) GetTupleOfTwo() => default;

    [HttpGet, Route(nameof(GetTupleOfTen))]
    public (Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32, Int32) GetTupleOfTen() => default;

    [HttpGet, Route(nameof(GetTupleWithNesting))]
    public (Int32, (Int32, (Int32 X, Int32)), Int32) GetTupleWithNesting() => default;

    [HttpGet, Route(nameof(GetPoint))]
    public (Int32 X, Int32 Y) GetPoint() => default;

    [HttpPost, Route(nameof(SetPoint))]
    public (Int32? X, Int32? Y) SetPoint((Int32? NewX, Int32? NewY) point) => default;

    [HttpPost, Route(nameof(SetTriangle))]
    public Boolean SetTriangle(((Int32 X1, Int32 Y1) P1, (Int32 X2, Int32 Y2) P2, (Int32 X3, Int32 Y3) P3) triangle) => default;
}
