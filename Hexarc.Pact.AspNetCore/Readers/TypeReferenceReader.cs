using System;
using System.Linq;
using Namotion.Reflection;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.AspNetCore.Extensions;

namespace Hexarc.Pact.AspNetCore.Readers
{
    public sealed class TypeReferenceReader
    {
        private TypeChecker TypeChecker { get; }

        private DistinctTypeQueue DistinctTypeQueue { get; }

        public TypeReferenceReader(TypeChecker typeChecker, DistinctTypeQueue distinctTypeQueue) =>
            (this.TypeChecker, this.DistinctTypeQueue) = (typeChecker, distinctTypeQueue);

        public TypeReference Read(ContextualType contextualType) => contextualType switch
        {
            { Nullability: Nullability.Nullable, IsNullableType: false } => this.ReadNullableReferenceTypeReference(contextualType),
            _ => this.ReadUnwrapped(contextualType)
        };

        private TypeReference ReadUnwrapped(ContextualType contextualType) => contextualType switch
        {
            var x when this.TypeChecker.IsActionResultOfT(x.OriginalType) => this.ReadFromActionResultOfT(x),
            var x when this.TypeChecker.IsNullableValueType(x) => this.ReadNullableValueTypeReference(x),
            var x when this.TypeChecker.IsTaskType(x.OriginalType) => this.ReadTaskTypeReference(x),
            var x when this.TypeChecker.IsTypeParameter(x.OriginalType) => this.ReadTypeParameterReference(x),
            var x when this.TypeChecker.IsArrayType(x.OriginalType) => this.ReadArrayTypeReference(x),
            var x when this.TypeChecker.IsArrayLikeType(x.OriginalType) => this.ReadArrayLikeTypeReference(x),
            var x when this.TypeChecker.IsDictionaryType(x.OriginalType) => this.ReadDictionaryTypeReference(x),
            var x when this.TypeChecker.IsPrimitiveType(x.OriginalType) => this.ReadPrimitiveTypeReference(x),
            var x when this.TypeChecker.IsTupleType(x.OriginalType) => this.ReadTupleTypeReference(x),
            var x when this.TypeChecker.IsDynamicType(x.OriginalType) => this.ReadDynamicTypeReference(x),
            var x => this.ReadDistinctTypeReference(x)
        };

        private NullableTypeReference ReadNullableReferenceTypeReference(ContextualType contextualType) =>
            new(this.ReadUnwrapped(contextualType));

        private TypeReference ReadFromActionResultOfT(ContextualType contextualType) =>
            this.Read(contextualType.GenericArguments.First());

        private NullableTypeReference ReadNullableValueTypeReference(ContextualType contextualType) =>
            new(this.Read(contextualType.Type.ToContextualType()));

        private TaskTypeReference ReadTaskTypeReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.GUID, this.Read(contextualType.GenericArguments.First()));

        private TypeParameterReference ReadTypeParameterReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.Name);

        private ArrayTypeReference ReadArrayTypeReference(ContextualType contextualType) =>
            new(this.Read(contextualType.ElementType!));

        private ArrayTypeReference ReadArrayLikeTypeReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.GUID, this.Read(contextualType.GenericArguments.First()));

        private DictionaryTypeReference ReadDictionaryTypeReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.GUID, contextualType.GenericArguments.Select(this.Read).ToArray());

        private PrimitiveTypeReference ReadPrimitiveTypeReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.GUID);

        private TupleTypeReference ReadTupleTypeReference(ContextualType contextualType) =>
            new(this.ReadTupleElements(contextualType.GetTupleArguments()));

        private TupleElement[] ReadTupleElements(ContextualType[] elementTypes) =>
            elementTypes.Select(this.ReadTupleElement).ToArray();
        private TupleElement ReadTupleElement(ContextualType elementType) =>
            this.ReadTupleElement(elementType, default, default);

        private TupleElement ReadTupleElement(
            ContextualType elementType, String? name, NamingConvention? namingConvention
        ) => new(this.Read(elementType), name?.ToConventionalString(namingConvention));

        private DynamicTypeReference ReadDynamicTypeReference(ContextualType contextualType) =>
            new(contextualType.OriginalType.GUID);

        private DistinctTypeReference ReadDistinctTypeReference(ContextualType contextualType)
        {
            var type = contextualType.OriginalType;
            this.DistinctTypeQueue.Enqueue(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
            return new DistinctTypeReference(contextualType.OriginalType.GUID, this.ReadGenericArguments(contextualType.GenericArguments));
        }

        private TypeReference[]? ReadGenericArguments(ContextualType[] genericTypes) =>
            genericTypes.Length != 0 ? Array.ConvertAll(genericTypes, this.Read) : default;
    }
}
