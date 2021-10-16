namespace Hexarc.Pact.AspNetCore.Models;

using Microsoft.AspNetCore.Mvc;

public sealed class ControllerCandidate
{
    public System.Type Type { get; }

    public PactIgnoreAttribute? IgnoreAttribute { get; }

    public ApiControllerAttribute? ApiControllerAttribute { get; }

    public RouteAttribute? RouteAttribute { get; }

    public Boolean IsPactCompatible =>
        this.Type.IsSubclassOf(typeof(ControllerBase)) &&
        this.IgnoreAttribute is null &&
        this.ApiControllerAttribute is not null &&
        this.RouteAttribute is not null;

    public ControllerCandidate(
        System.Type type,
        PactIgnoreAttribute? ignoreAttribute,
        ApiControllerAttribute? apiControllerAttribute,
        RouteAttribute? routeAttribute)
    {
        this.Type = type;
        this.IgnoreAttribute = ignoreAttribute;
        this.ApiControllerAttribute = apiControllerAttribute;
        this.RouteAttribute = routeAttribute;
    }
}
