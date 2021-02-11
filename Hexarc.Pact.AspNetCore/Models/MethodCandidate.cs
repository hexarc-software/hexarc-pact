using System;
using System.Reflection;
using Hexarc.Pact.AspNetCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Hexarc.Pact.AspNetCore.Models
{
    internal sealed class MethodCandidate
    {
        public MethodInfo MethodInfo { get; }

        public IgnoreAttribute? IgnoreAttribute { get; }

        public HttpMethodAttribute? HttpMethodAttribute { get; }

        public RouteAttribute? RouteAttribute { get; }

        public Boolean IsSupportedHttpMethod =>
            this.HttpMethodAttribute is HttpGetAttribute ||
            this.HttpMethodAttribute is HttpPostAttribute;

        public Boolean IsPactCompatible =>
            this.IgnoreAttribute is null &&
            this.RouteAttribute is not null &&
            this.IsSupportedHttpMethod;

        public MethodCandidate(
            MethodInfo methodInfo,
            IgnoreAttribute? ignoreAttribute,
            HttpMethodAttribute? httpMethodAttribute,
            RouteAttribute? routeAttribute)
        {
            this.MethodInfo = methodInfo;
            this.IgnoreAttribute = ignoreAttribute;
            this.HttpMethodAttribute = httpMethodAttribute;
            this.RouteAttribute = routeAttribute;
        }
    }
}
