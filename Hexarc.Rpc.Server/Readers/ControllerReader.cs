using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Hexarc.Rpc.Protocol.Api;
using Hexarc.Rpc.Server.Attributes;
using Hexarc.Rpc.Server.Models;
using Controller = Hexarc.Rpc.Protocol.Api.Controller;

namespace Hexarc.Rpc.Server.Readers
{
    public sealed class ControllerReader
    {
        private MethodReader MethodReader { get; }

        public ControllerReader(MethodReader methodReader) =>
            this.MethodReader = methodReader;

        public Controller Read(Type type, RouteAttribute routeAttribute) =>
            new(type.Namespace, type.Name, this.ReadPath(routeAttribute), this.ReadMethods(type));

        private String ReadPath(RouteAttribute routeAttribute) =>
            routeAttribute.Template.StartsWith("/") ? routeAttribute.Template : $"/{routeAttribute.Template}";

        private Method[] ReadMethods(Type type) =>
            type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Select(this.ReadMethodCandidate)
                .Where(x => x.IsRpcCompatible)
                .Select(x => this.MethodReader.Read(x.MethodInfo, x.HttpMethodAttribute!, x.RouteAttribute!))
                .ToArray();

        private MethodCandidate ReadMethodCandidate(MethodInfo methodInfo) =>
            new(methodInfo,
                methodInfo.GetCustomAttribute<IgnoreAttribute>(),
                methodInfo.GetCustomAttribute<HttpMethodAttribute>(),
                methodInfo.GetCustomAttribute<RouteAttribute>());
    }
}
