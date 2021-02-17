using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Annotations;
using Hexarc.Pact.Protocol.Types;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.Tool.Extensions;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Emitters.SyntaxOperations;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class DistinctTypeEmitter
    {
        private TypeReferenceEmitter TypeReferenceEmitter { get; }

        public DistinctTypeEmitter(TypeReferenceEmitter typeReferenceEmitter) =>
            this.TypeReferenceEmitter = typeReferenceEmitter;

        public EmittedEntity Emit(DistinctType distinctType) => distinctType switch
        {
            EnumType @enum => this.EmitEnumType(@enum),
            StructType @struct => this.EmitStructType(@struct),
            ClassType @class => this.EmitClassType(@class),
            UnionType union => this.EmitUnionType(union),
            _ => throw new InvalidOperationException($"Could not emit a Hexarc Pact type from {distinctType}")
        };

        private EmittedEntity EmitEnumType(EnumType type) =>
            new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitEnumDeclaration(type)));

        private EmittedEntity EmitStructType(StructType type) =>
            new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitStructDeclaration(type)));

        private EmittedEntity EmitClassType(ClassType type) =>
            new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitClassDeclaration(type)));

        private EmittedEntity EmitUnionType(UnionType type) =>
            new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitUnionDeclaration(type)));

        private EnumDeclarationSyntax EmitEnumDeclaration(EnumType type) =>
            EnumDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithMembers(
                    SeparatedList<EnumMemberDeclarationSyntax>(
                        this.EmitEnumMemberDeclarations(type.Members)));

        private IEnumerable<EnumMemberDeclarationSyntax> EmitEnumMemberDeclarations(EnumMember[] members) =>
            members.Select(this.EmitEnumMemberDeclaration);

        private EnumMemberDeclarationSyntax EmitEnumMemberDeclaration(EnumMember member) =>
            EnumMemberDeclaration(member.Name)
                .WithEqualsValue(
                    EqualsValueClause(
                        LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(member.Value))));

        private StructDeclarationSyntax EmitStructDeclaration(StructType type) =>
            StructDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithTypeParameterList(
                    type.GenericParameters is not null
                        ? this.EmitGenericParameters(type.GenericParameters)
                        : default)
                .WithMembers(
                    this.EmitObjectPropertyDeclarations(type.Properties, type.Namespace));

        private ClassDeclarationSyntax EmitClassDeclaration(ClassType type) =>
            ClassDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.SealedKeyword)))
                .WithTypeParameterList(
                    type.GenericParameters is not null
                        ? this.EmitGenericParameters(type.GenericParameters)
                        : default)
                .WithMembers(
                    this.EmitObjectPropertyDeclarations(type.Properties, type.Namespace));

        private MemberDeclarationSyntax[] EmitUnionDeclaration(UnionType type) =>
            Enumerable.Empty<MemberDeclarationSyntax>()
                .Concat(EnumerableFactory.FromOne(this.EmitUnionBaseDeclaration(type)))
                .Concat(type.Cases.Select(x => this.EmitUnionCaseDeclaration(x, type.Name)))
                .ToArray();

        private ClassDeclarationSyntax EmitUnionBaseDeclaration(UnionType type) =>
            ClassDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AbstractKeyword)))
                .WithAttributeLists(
                    this.EmitUnionAttributes(type))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        this.EmitAbstractUnionTagPropertyDeclaration(type.TagName)));

        private ClassDeclarationSyntax EmitUnionCaseDeclaration(ClassType type, String baseTypeName) =>
            this.EmitClassDeclaration(type)
                .WithBaseList(
                    BaseList(
                        SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                IdentifierName(baseTypeName)))));

        private SyntaxList<AttributeListSyntax> EmitUnionAttributes(UnionType type) =>
            List<AttributeListSyntax>()
                .Add(AttributeList(SingletonSeparatedList(this.EmitUnionTagAttribute(type.TagName))))
                .AddRange(type.Cases.Select(x => AttributeList(SingletonSeparatedList(this.EmitUnionCaseAttribute(x)))));

        private AttributeSyntax EmitUnionTagAttribute(String tagName) =>
            Attribute(IdentifierName(typeof(UnionTagAttribute).FullName!))
                .WithArgumentList(
                    AttributeArgumentList(
                        SingletonSeparatedList<AttributeArgumentSyntax>(
                            AttributeArgument(
                                InvocationExpression(
                                        IdentifierName("nameof"))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList<ArgumentSyntax>(
                                                Argument(IdentifierName(tagName)))))))));

        private AttributeSyntax EmitUnionCaseAttribute(ClassType type) =>
            Attribute(IdentifierName(typeof(UnionCaseAttribute).FullName!))
                .WithArgumentList(
                    AttributeArgumentList(
                        SeparatedList<AttributeArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                AttributeArgument(
                                    TypeOfExpression(IdentifierName(type.Name))),
                                Token(SyntaxKind.CommaToken),
                                AttributeArgument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(type.Properties
                                            .Select(x => x.Type)
                                            .OfType<LiteralTypeReference>()
                                            .First().Name)))
                            })));

        private TypeParameterListSyntax EmitGenericParameters(String[] genericParameters) =>
            TypeParameterList(
                SeparatedList<TypeParameterSyntax>(
                    genericParameters
                        .Select(this.EmitGenericParameter)
                        .Separate(genericParameters.Length, Token(SyntaxKind.CommaToken))));

        private SyntaxNodeOrToken EmitGenericParameter(String genericParameter) =>
            TypeParameter(Identifier(genericParameter));

        private SyntaxList<MemberDeclarationSyntax> EmitObjectPropertyDeclarations(ObjectProperty[] properties, String? currentNamespace) =>
            List<MemberDeclarationSyntax>(properties.Select(x => this.EmitObjectPropertyDeclaration(x, currentNamespace)));

        private PropertyDeclarationSyntax EmitObjectPropertyDeclaration(ObjectProperty property, String? currentNamespace) =>
            (property.Type, property.Name) switch
            {
                var (reference, propertyName) when reference is LiteralTypeReference literal =>
                    this.EmitConcreteUnionTagPropertyDeclaration(literal, propertyName),
                var (reference, propertyName) =>
                    this.EmitObjectPropertyDeclaration(reference, propertyName, currentNamespace)
            };

        private PropertyDeclarationSyntax EmitAbstractUnionTagPropertyDeclaration(String propertyName) =>
            PropertyDeclaration(
                    IdentifierName(typeof(String).FullName!),
                    propertyName)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AbstractKeyword)))
                .WithAccessorList(
                    AccessorList(
                        SingletonList<AccessorDeclarationSyntax>(
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(
                                    Token(SyntaxKind.SemicolonToken)))));

        private PropertyDeclarationSyntax EmitConcreteUnionTagPropertyDeclaration(LiteralTypeReference reference, String propertyName) =>
            PropertyDeclaration(
                    this.TypeReferenceEmitter.Emit(reference),
                    Identifier(propertyName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.OverrideKeyword)))
                .WithAccessorList(
                    AccessorList(
                        SingletonList<AccessorDeclarationSyntax>(
                            AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(
                                    Token(SyntaxKind.SemicolonToken)))))
                .WithInitializer(
                    EqualsValueClause(
                        LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal(reference.Name))))
                .WithSemicolonToken(
                    Token(SyntaxKind.SemicolonToken));

        private PropertyDeclarationSyntax EmitObjectPropertyDeclaration(TypeReference reference, String propertyName, String? currentNamespace) =>
            PropertyDeclaration(
                    this.TypeReferenceEmitter.Emit(reference, currentNamespace),
                    Identifier(propertyName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    AccessorList(
                        List<AccessorDeclarationSyntax>(
                            new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken)),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                            })));
    }
}
