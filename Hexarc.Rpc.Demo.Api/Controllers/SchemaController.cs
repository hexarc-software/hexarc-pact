using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Hexarc.Rpc.Protocol.Api;
using Hexarc.Rpc.Protocol.TypeProviders;
using Hexarc.Rpc.Server.Attributes;
using Hexarc.Rpc.Server.Internals;
using Hexarc.Rpc.Server.Readers;

namespace Hexarc.Rpc.Demo.Api.Controllers
{
    [ApiController, Route("Schema"), Ignore]
    public sealed class SchemaController : ControllerBase
    {
        [HttpGet, Route(nameof(GetSchema))]
        public Schema GetSchema()
        {
            var distinctTypeQueue = new DistinctTypeQueue();
            var primitiveTypeProvider = new PrimitiveTypeProvider();
            var arrayLikeTypeProvider = new ArrayLikeTypeProvider();
            var dictionaryTypeProvider = new DictionaryTypeProvider();
            var taskTypeProvider = new TaskTypeProvider();
            var typeChecker = new TypeChecker(primitiveTypeProvider, arrayLikeTypeProvider, dictionaryTypeProvider, taskTypeProvider);
            var typeReferenceReader = new TypeReferenceReader(typeChecker, distinctTypeQueue);
            var distinctTypeReader = new DistinctTypeReader(typeChecker, typeReferenceReader);
            var methodReader = new MethodReader(typeChecker, typeReferenceReader);
            var controllerReader = new ControllerReader(methodReader);
            var schemaReader = new SchemaReader(distinctTypeQueue, distinctTypeReader, controllerReader);
            return schemaReader.Read(Assembly.GetExecutingAssembly());
        }
    }
}
