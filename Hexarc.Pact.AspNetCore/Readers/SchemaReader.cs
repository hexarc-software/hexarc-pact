using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.Types;
using Hexarc.Pact.Protocol.TypeProviders;
using Hexarc.Pact.AspNetCore.Attributes;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Controller = Hexarc.Pact.Protocol.Api.Controller;

namespace Hexarc.Pact.AspNetCore.Readers
{
    public sealed class SchemaReader
    {
        private DistinctTypeQueue DistinctTypeQueue { get; }

        private DistinctTypeReader DistinctTypeReader { get; }

        private ControllerReader ControllerReader { get; }

        private PrimitiveTypeProvider PrimitiveTypeProvider { get; }

        private DynamicTypeProvider DynamicTypeProvider { get; }

        private ArrayLikeTypeProvider ArrayLikeTypeProvider { get; }

        private DictionaryTypeProvider DictionaryTypeProvider { get; }

        private TaskTypeProvider TaskTypeProvider { get; }

        public SchemaReader(
            DistinctTypeQueue distinctTypeQueue,
            DistinctTypeReader distinctTypeReader,
            ControllerReader controllerReader,
            PrimitiveTypeProvider primitiveTypeProvider,
            DynamicTypeProvider dynamicTypeProvider,
            ArrayLikeTypeProvider arrayLikeTypeProvider,
            DictionaryTypeProvider dictionaryTypeProvider,
            TaskTypeProvider taskTypeProvider)
        {
            this.DistinctTypeQueue = distinctTypeQueue;
            this.DistinctTypeReader = distinctTypeReader;
            this.ControllerReader = controllerReader;
            this.PrimitiveTypeProvider = primitiveTypeProvider;
            this.DynamicTypeProvider = dynamicTypeProvider;
            this.ArrayLikeTypeProvider = arrayLikeTypeProvider;
            this.DictionaryTypeProvider = dictionaryTypeProvider;
            this.TaskTypeProvider = taskTypeProvider;
        }

        public Schema Read(Assembly assembly, NamingConvention? namingConvention = default) =>
            new(this.ReadControllers(assembly.GetTypes(), namingConvention), this.ReadTypes(namingConvention));

        /// <summary>
        /// Reads the API schema from given types.
        /// </summary>
        /// <param name="types">The types to search for controllers.</param>
        /// <param name="namingConvention">The naming convention applied to types.</param>
        /// <returns>The API schema read from the given types.</returns>
        public Schema Read(IEnumerable<System.Type> types, NamingConvention? namingConvention = default) =>
            new(this.ReadControllers(types, namingConvention), this.ReadTypes(namingConvention));

        private Controller[] ReadControllers(IEnumerable<System.Type> types, NamingConvention? namingConvention) => types
            .Select(this.ReadControllerCandidate)
            .Where(x => x.IsPactCompatible)
            .Select(x => this.ControllerReader.Read(x.Type, x.RouteAttribute!, namingConvention))
            .Where(x => x.Methods.Length != 0)
            .ToArray();

        private ControllerCandidate ReadControllerCandidate(System.Type type) =>
            new(type,
                type.GetCustomAttribute<PactIgnoreAttribute>(),
                type.GetCustomAttribute<ApiControllerAttribute>(),
                type.GetCustomAttribute<RouteAttribute>());

        private Type[] ReadTypes(NamingConvention? namingConvention) =>
            this.EnumerateDistinctTypes(namingConvention)
                .Concat(this.PrimitiveTypeProvider.Enumerate())
                .Concat(this.DynamicTypeProvider.Enumerate())
                .Concat(this.ArrayLikeTypeProvider.Enumerate())
                .Concat(this.DictionaryTypeProvider.Enumerate())
                .Concat(this.TaskTypeProvider.Enumerate())
                .ToArray();

        private IEnumerable<Type> EnumerateDistinctTypes(NamingConvention? namingConvention)
        {
            while (this.DistinctTypeQueue.TryDequeue(out var type))
            {
                yield return this.DistinctTypeReader.Read(type, namingConvention);
            }
        }
    }
}
