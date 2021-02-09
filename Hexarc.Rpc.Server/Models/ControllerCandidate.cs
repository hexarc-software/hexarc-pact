using System;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Rpc.Server.Attributes;

namespace Hexarc.Rpc.Server.Models
{
    public sealed class ControllerCandidate
    {
        public Type Type { get; }

        public IgnoreAttribute? IgnoreAttribute { get; }

        public ApiControllerAttribute? ApiControllerAttribute { get; }

        public RouteAttribute? RouteAttribute { get; }

        public Boolean IsRpcCompatible =>
            this.Type.IsSubclassOf(typeof(ControllerBase)) &&
            this.IgnoreAttribute is null &&
            this.ApiControllerAttribute is not null &&
            this.RouteAttribute is not null;

        public ControllerCandidate(
            Type type,
            IgnoreAttribute? ignoreAttribute,
            ApiControllerAttribute? apiControllerAttribute,
            RouteAttribute? routeAttribute)
        {
            this.Type = type;
            this.IgnoreAttribute = ignoreAttribute;
            this.ApiControllerAttribute = apiControllerAttribute;
            this.RouteAttribute = routeAttribute;
        }
    }
}
