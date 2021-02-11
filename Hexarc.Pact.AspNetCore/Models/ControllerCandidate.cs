using System;
using Hexarc.Pact.AspNetCore.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.AspNetCore.Models
{
    internal sealed class ControllerCandidate
    {
        public Type Type { get; }

        public IgnoreAttribute? IgnoreAttribute { get; }

        public ApiControllerAttribute? ApiControllerAttribute { get; }

        public RouteAttribute? RouteAttribute { get; }

        public Boolean IsPactCompatible =>
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
