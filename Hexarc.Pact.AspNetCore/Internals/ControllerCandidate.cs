using Microsoft.AspNetCore.Mvc;
using Type = System.Type;

namespace Hexarc.Pact.AspNetCore.Internals;

public sealed class ControllerCandidate
{
    public Type Type { get; }

    public PactIgnoreAttribute? IgnoreAttribute { get; }

    public ApiControllerAttribute? ApiControllerAttribute { get; }

    public RouteAttribute? RouteAttribute { get; }

    public Boolean IsPactCompatible =>
        this.Type.IsSubclassOf(typeof(ControllerBase)) &&
        this.IgnoreAttribute is null &&
        this.ApiControllerAttribute is not null &&
        this.RouteAttribute is not null;

    public ControllerCandidate(
        Type type,
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
