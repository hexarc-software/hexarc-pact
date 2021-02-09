using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Hexarc.Rpc.Server.Attributes;

namespace Hexarc.Rpc.Server.Models
{
    public sealed class MethodCandidate
    {
        public MethodInfo MethodInfo { get; }

        public IgnoreAttribute? IgnoreAttribute { get; }

        public HttpMethodAttribute? HttpMethodAttribute { get; }

        public RouteAttribute? RouteAttribute { get; }

        public Boolean IsSupportedHttpMethod =>
            this.HttpMethodAttribute is HttpGetAttribute ||
            this.HttpMethodAttribute is HttpPostAttribute;

        public Boolean IsRpcCompatible =>
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
