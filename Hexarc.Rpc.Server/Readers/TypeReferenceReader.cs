using System;
using System.Linq;
using System.Reflection;
using Hexarc.Rpc.Protocol.TypeReferences;
using Hexarc.Rpc.Server.Internals;

namespace Hexarc.Rpc.Server.Readers
{
    public sealed class TypeReferenceReader
    {
        private TypeChecker TypeChecker { get; }

        private DistinctTypeQueue DistinctTypeQueue { get; }

        public TypeReferenceReader(TypeChecker typeChecker, DistinctTypeQueue distinctTypeQueue) =>
            (this.TypeChecker, this.DistinctTypeQueue) = (typeChecker, distinctTypeQueue);

        public TypeReference Read(Type type) => type switch
        {
            var x when this.TypeChecker.IsNullableValueType(x) => this.ReadNullableValueTypeReference(x),
            var x when this.TypeChecker.IsTaskType(x) => this.ReadTaskTypeReference(x),
            var x when this.TypeChecker.IsGenericParameter(x) => this.ReadGenericTypeReference(x),
            var x when this.TypeChecker.IsArrayType(x) => this.ReadArrayTypeReference(x),
            var x when this.TypeChecker.IsArrayLikeType(x) => this.ReadArrayLikeTypeReference(x),
            var x when this.TypeChecker.IsDictionaryType(x) => this.ReadDictionaryTypeReference(x),
            var x when this.TypeChecker.IsPrimitiveType(x) => this.ReadPrimitiveTypeReference(x),
            var x => this.ReadDistinctTypeReference(x)
        };

        private NullableTypeReference ReadNullableValueTypeReference(Type type) =>
            new(this.Read(Nullable.GetUnderlyingType(type)!));

        private TaskTypeReference ReadTaskTypeReference(Type type) =>
            new(type.GUID, this.Read(type.GetGenericArguments().First()));

        private GenericTypeReference ReadGenericTypeReference(MemberInfo type) =>
            new(type.Name);

        private ArrayTypeReference ReadArrayTypeReference(Type type) =>
            new(this.Read(type.GetElementType()!));

        private ArrayTypeReference ReadArrayLikeTypeReference(Type type) =>
            new(type.GUID, this.Read(type.GetGenericArguments().First()));

        private DictionaryTypeReference ReadDictionaryTypeReference(Type type) =>
            new(type.GUID, type.GetGenericArguments().Select(this.Read).ToArray());

        private PrimitiveTypeReference ReadPrimitiveTypeReference(Type type) =>
            new(type.GUID);

        private TypeReference[]? ReadGenericArguments(Type[] genericTypes) =>
            genericTypes.Length != 0 ? Array.ConvertAll(genericTypes, this.Read) : default;

        private DistinctTypeReference ReadDistinctTypeReference(Type type)
        {
            this.DistinctTypeQueue.Enqueue(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
            return new DistinctTypeReference(type.GUID, this.ReadGenericArguments(type.GetGenericArguments()));
        }
    }
}
