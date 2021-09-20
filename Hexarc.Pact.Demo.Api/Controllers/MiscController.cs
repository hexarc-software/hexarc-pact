namespace Hexarc.Pact.Demo.Api.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[ApiController, Route("Misc")]
public sealed class MiscController : ControllerBase
{
    [HttpGet, Route(nameof(Ping))]
    public String Ping([FromQuery] String message) => $"Hello, {message}";

    [HttpGet, Route(nameof(Sum))]
    public Int32 Sum([FromQuery] Int32 a, [FromQuery] Int32 b) => a + b;

    [HttpGet, Route(nameof(Random))]
    public ActionResult<Double> Random() => new Random().NextDouble();

    [HttpGet, Route(nameof(GetVoid))]
    public void GetVoid() { }

    [HttpGet, Route(nameof(GetVoidTask))]
    public async Task GetVoidTask() => await Task.Delay(0);

    [HttpPost, Route(nameof(PostVoid))]
    public void PostVoid(Object payload) { }

    [HttpPost, Route(nameof(PostVoidTask))]
    public async Task PostVoidTask(Object payload) => await Task.Delay(0);

    [HttpPost(nameof(PostVoidTaskWithVoidRequest))]
    public async Task PostVoidTaskWithVoidRequest() => await Task.Delay(0);
}
