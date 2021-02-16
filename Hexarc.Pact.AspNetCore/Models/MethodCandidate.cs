using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Hexarc.Pact.AspNetCore.Attributes;

namespace Hexarc.Pact.AspNetCore.Models
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

        public Boolean IsPactCompatible =>
            this.IgnoreAttribute is null &&
            this.RouteAttribute is not null &&
            this.IsSupportedHttpMethod;

        public Boolean IsNullableReferenceResult { get; }

        public MethodCandidate(
            MethodInfo methodInfo,
            IgnoreAttribute? ignoreAttribute,
            HttpMethodAttribute? httpMethodAttribute,
            RouteAttribute? routeAttribute,
            Boolean isNullableReferenceResult)
        {
            this.MethodInfo = methodInfo;
            this.IgnoreAttribute = ignoreAttribute;
            this.HttpMethodAttribute = httpMethodAttribute;
            this.RouteAttribute = routeAttribute;
            this.IsNullableReferenceResult = isNullableReferenceResult;
        }
    }
}
