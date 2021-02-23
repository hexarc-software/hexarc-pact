using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.Tool.Internals;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;
using Type = Hexarc.Pact.Protocol.Types.Type;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class TypeReferenceEmitter
    {
        private TypeRegistry TypeRegistry { get; }

        public TypeReferenceEmitter(TypeRegistry typeRegistry) =>
            this.TypeRegistry = typeRegistry;

        public TypeSyntax Emit(TypeReference typeReference, String? currentNamespace = default) => typeReference switch
        {
            PrimitiveTypeReference primitive => this.EmitPrimitiveTypeReference(primitive, currentNamespace),
            DynamicTypeReference dynamic => this.EmitDynamicTypeReference(dynamic, currentNamespace),
            NullableTypeReference nullable => this.EmitNullableTypeReference(nullable, currentNamespace),
            ArrayTypeReference array => this.EmitArrayTypeReference(array, currentNamespace),
            DictionaryTypeReference dictionary => this.EmitDictionaryTypeReference(dictionary, currentNamespace),
            TaskTypeReference task => this.EmitTaskTypeReference(task, currentNamespace),
            GenericTypeReference generic => this.EmitGenericTypeReference(generic),
            LiteralTypeReference => this.EmitLiteralTypeReference(),
            DistinctTypeReference distinct => this.EmitDistinctTypeReference(distinct, currentNamespace),
            _ => throw new InvalidOperationException($"Could not emit a Hexarc Pact type reference from {typeReference}")
        };

        private IEnumerable<TypeSyntax> EmitMany(IEnumerable<TypeReference> typeReferences, String? currentNamespace) =>
            typeReferences.Select(typeReference => this.Emit(typeReference, currentNamespace));

        private NameSyntax EmitPrimitiveTypeReference(PrimitiveTypeReference reference, String? currentNamespace) =>
            this.EmitTypeName(this.TypeRegistry.GetPrimitiveType(reference.TypeId), currentNamespace);

        private NameSyntax EmitDynamicTypeReference(DynamicTypeReference reference, String? currentNamespace) =>
            this.EmitTypeName(this.TypeRegistry.GetDynamicTypeType(reference.TypeId), currentNamespace);

        private NullableTypeSyntax EmitNullableTypeReference(NullableTypeReference reference, String? currentNamespace) =>
            NullableType(this.Emit(reference.UnderlyingType, currentNamespace));

        private TypeSyntax EmitArrayTypeReference(ArrayTypeReference reference, String? currentNamespace) =>
            (reference.ArrayLikeTypeId, reference.ElementType) switch
            {
                (null, var elementType) => this.EmitPureArrayTypeReference(elementType, currentNamespace),
                var (arrayLikeTypeId, elementType) => this.EmitArrayLikeTypeReference(arrayLikeTypeId.Value, elementType, currentNamespace)
            };

        private ArrayTypeSyntax EmitPureArrayTypeReference(TypeReference elementType, String? currentNamespace) =>
            ArrayType(this.Emit(elementType, currentNamespace)!)
                .WithRankSpecifiers(
                    SingletonList(
                        ArrayRankSpecifier(
                            SingletonSeparatedList<ExpressionSyntax>(
                                OmittedArraySizeExpression()))));

        private GenericNameSyntax EmitArrayLikeTypeReference(Guid arrayLikeTypeId, TypeReference elementType, String? currentNamespace) =>
            this.EmitGenericTypeName(this.TypeRegistry.GetArrayLikeType(arrayLikeTypeId), elementType, currentNamespace);

        private GenericNameSyntax EmitDictionaryTypeReference(DictionaryTypeReference reference, String? currentNamespace) =>
            this.EmitGenericTypeName(
                this.TypeRegistry.GetDictionaryType(reference.TypeId),
                new[] { reference.KeyType, reference.ValueType },
                currentNamespace);

        private GenericNameSyntax EmitTaskTypeReference(TaskTypeReference reference, String? currentNamespace) =>
            this.EmitGenericTypeName(this.TypeRegistry.GetTaskType(reference.TypeId), reference.ResultType, currentNamespace);

        private NameSyntax EmitGenericTypeReference(GenericTypeReference reference) =>
            ParseName(reference.Name);

        private NameSyntax EmitLiteralTypeReference() =>
            ParseName(typeof(String).FullName!);

        private TypeSyntax EmitDistinctTypeReference(DistinctTypeReference reference, String? currentNamespace) =>
            (this.TypeRegistry.GetDistinctType(reference.TypeId), reference.GenericArguments) switch
            {
                (var type, null) => this.EmitTypeName(type, currentNamespace),
                var (type, genericArguments) => this.EmitGenericTypeName(type, genericArguments, currentNamespace)
            };

        private GenericNameSyntax EmitGenericTypeName(Type type, TypeReference[] genericArguments, String? currentNamespace) =>
            GenericName(this.EmitTypeIdentifier(type, currentNamespace))
                .WithTypeArgumentList(
                    TypeArgumentList(this.EmitGenericArguments(genericArguments, currentNamespace)));

        private GenericNameSyntax EmitGenericTypeName(Type type, TypeReference genericArgument, String? currentNamespace) =>
            GenericName(this.EmitTypeIdentifier(type, currentNamespace))
                .WithTypeArgumentList(
                    TypeArgumentList(this.EmitGenericArgument(genericArgument, currentNamespace)));

        private SeparatedSyntaxList<TypeSyntax> EmitGenericArgument(TypeReference argument, String? currentNamespace) =>
            SingletonSeparatedList<TypeSyntax>(this.Emit(argument, currentNamespace));

        private SeparatedSyntaxList<TypeSyntax> EmitGenericArguments(TypeReference[] arguments, String? currentNamespace) =>
            SeparatedListWithCommas(this.EmitMany(arguments, currentNamespace).ToArray());

        private NameSyntax EmitTypeName(Type type, String? currentNamespace) =>
            this.IsSameNamespace(type, currentNamespace) ? ParseName(type.Name) : ParseName(type.FullName);

        private SyntaxToken EmitTypeIdentifier(Type type, String? currentNamespace) =>
            this.IsSameNamespace(type, currentNamespace) ? Identifier(type.Name) : Identifier(type.FullName);

        private Boolean IsSameNamespace(Type type, String? currentNamespace) =>
            String.Equals(currentNamespace, type.Namespace, StringComparison.Ordinal);
    }
}
