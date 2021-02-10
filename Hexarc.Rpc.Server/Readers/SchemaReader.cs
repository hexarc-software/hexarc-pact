using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Rpc.Protocol.Api;
using Hexarc.Rpc.Protocol.Types;
using Hexarc.Rpc.Server.Attributes;
using Hexarc.Rpc.Server.Internals;
using Hexarc.Rpc.Server.Models;
using Controller = Hexarc.Rpc.Protocol.Api.Controller;

namespace Hexarc.Rpc.Server.Readers
{
    public sealed class SchemaReader
    {
        private DistinctTypeQueue DistinctTypeQueue { get; }

        private DistinctTypeReader DistinctTypeReader { get; }

        private ControllerReader ControllerReader { get; }

        public SchemaReader(
            DistinctTypeQueue distinctTypeQueue,
            DistinctTypeReader distinctTypeReader,
            ControllerReader controllerReader)
        {
            this.DistinctTypeQueue = distinctTypeQueue;
            this.DistinctTypeReader = distinctTypeReader;
            this.ControllerReader = controllerReader;
        }

        public Schema Read(Assembly assembly) =>
            new(this.ReadControllers(assembly.GetTypes()), this.ReadDistinctTypes());

        public Schema Read(System.Type[] controllerTypes) =>
            new(this.ReadControllers(controllerTypes), this.ReadDistinctTypes());

        private Controller[] ReadControllers(System.Type[] types) => types
            .Select(this.ReadControllerCandidate)
            .Where(x => x.IsRpcCompatible)
            .Select(x => this.ControllerReader.Read(x.Type, x.RouteAttribute!))
            .Where(x => x.Methods.Length != 0)
            .ToArray();

        private ControllerCandidate ReadControllerCandidate(System.Type type) =>
            new(type,
                type.GetCustomAttribute<IgnoreAttribute>(),
                type.GetCustomAttribute<ApiControllerAttribute>(),
                type.GetCustomAttribute<RouteAttribute>());

        private Type[] ReadDistinctTypes() =>
            this.EnumerateDistinctTypes().ToArray();

        private IEnumerable<Type> EnumerateDistinctTypes()
        {
            while (this.DistinctTypeQueue.TryDequeue(out var type))
            {
                yield return this.DistinctTypeReader.Read(type);
            }
        }
    }
}
