using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hexarc.Pact.Protocol.Types;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.Tool.Extensions;
using Hexarc.Pact.Tool.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class DistinctTypeEmitter
    {
        private TypeReferenceEmitter TypeReferenceEmitter { get; }

        public DistinctTypeEmitter(TypeReferenceEmitter typeReferenceEmitter) =>
            this.TypeReferenceEmitter = typeReferenceEmitter;

        public EmittedDistinctType Emit(DistinctType distinctType) => distinctType switch
        {
            EnumType @enum => this.EmitEnumType(@enum),
            StructType @struct => this.EmitStructType(@struct),
            ClassType @class => this.EmitClassType(@class),
            UnionType union => this.EmitUnionType(union),
            _ => throw new InvalidOperationException($"Could not emit a Hexarc Pact type from {distinctType}")
        };

        private EmittedDistinctType EmitEnumType(EnumType type) =>
            new(type.Name,
                this.WrapInNamespace(
                    type.Namespace,
                    this.EmitEnumSyntaxTree(type)));

        private EmittedDistinctType EmitStructType(StructType type) =>
            new(type.Name, this.WrapInNamespace(type.Namespace, this.EmitStructTypeSyntaxTree(type)));

        private EmittedDistinctType EmitClassType(ClassType type) =>
            new(type.Name, this.WrapInNamespace(type.Namespace, this.EmitClassTypeSyntaxTree(type)));

        private EmittedDistinctType EmitUnionType(UnionType type) =>
            new(type.Name, this.WrapInNamespace(type.Namespace, this.EmitUnionTypeSyntaxTree(type)));

        private MemberDeclarationSyntax EmitEnumSyntaxTree(EnumType type) =>
            EnumDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithMembers(
                    SeparatedList<EnumMemberDeclarationSyntax>(
                        this.EmitEnumMembers(type.Members)));

        private IEnumerable<EnumMemberDeclarationSyntax> EmitEnumMembers(EnumMember[] members) =>
            members.Select(this.EmitEnumMember);

        private EnumMemberDeclarationSyntax EmitEnumMember(EnumMember member) =>
            EnumMemberDeclaration(member.Name)
                .WithEqualsValue(
                    EqualsValueClause(
                        LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            Literal(member.Value))));

        private MemberDeclarationSyntax EmitStructTypeSyntaxTree(StructType type) =>
            StructDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithTypeParameterList(
                    type.GenericParameters is not null
                        ? this.EmitGenericParameters(type.GenericParameters)
                        : default)
                .WithMembers(
                    this.EmitObjectProperties(type.Properties, type.Namespace));

        private MemberDeclarationSyntax EmitClassTypeSyntaxTree(ClassType type) =>
            ClassDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.SealedKeyword)))
                .WithTypeParameterList(
                    type.GenericParameters is not null
                        ? this.EmitGenericParameters(type.GenericParameters)
                        : default!) //TODO: False nullability check
                .WithMembers(
                    this.EmitObjectProperties(type.Properties, type.Namespace));

        private SyntaxList<MemberDeclarationSyntax> EmitUnionTypeSyntaxTree(UnionType type) =>
            List<MemberDeclarationSyntax>()
                .Add(this.EmitBaseUnionTypeSyntaxTree(type))
                .AddRange(type.Cases.Select(this.EmitClassTypeSyntaxTree));

        private MemberDeclarationSyntax EmitBaseUnionTypeSyntaxTree(UnionType type) =>
            ClassDeclaration(type.Name)
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AbstractKeyword)))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        this.EmitAbstractUnionTagProperty(type.TagName)));

        private TypeParameterListSyntax EmitGenericParameters(String[] genericParameters) =>
            TypeParameterList(
                SeparatedList<TypeParameterSyntax>(
                    genericParameters
                        .Select(this.EmitGenericParameter)
                        .Separate(genericParameters.Length, Token(SyntaxKind.CommaToken))));

        private SyntaxNodeOrToken EmitGenericParameter(String genericParameter) =>
            TypeParameter(Identifier(genericParameter));

        private SyntaxList<MemberDeclarationSyntax> EmitObjectProperties(ObjectProperty[] properties, String? currentNamespace) =>
            List<MemberDeclarationSyntax>(properties.Select(x => this.EmitObjectProperty(x, currentNamespace)));

        private PropertyDeclarationSyntax EmitObjectProperty(ObjectProperty property, String? currentNamespace) =>
            (property.Type, property.Name) switch
            {
                var (reference, propertyName) when reference is LiteralTypeReference literal =>
                    this.EmitImplementedUnionTagProperty(literal, propertyName),
                var (reference, propertyName) =>
                    this.EmitObjectProperty(reference, propertyName, currentNamespace)
            };

        private PropertyDeclarationSyntax EmitAbstractUnionTagProperty(String propertyName) =>
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

        private PropertyDeclarationSyntax EmitImplementedUnionTagProperty(LiteralTypeReference reference, String propertyName) =>
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

        private PropertyDeclarationSyntax EmitObjectProperty(TypeReference reference, String propertyName, String? currentNamespace) =>
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

        private MemberDeclarationSyntax WrapInNamespace(String? @namespace, MemberDeclarationSyntax member) =>
            String.IsNullOrEmpty(@namespace)
                ? member
                : NamespaceDeclaration(IdentifierName(@namespace))
                    .WithMembers(
                        SingletonList<MemberDeclarationSyntax>(member));

        private SyntaxList<MemberDeclarationSyntax> WrapInNamespace(String? @namespace, SyntaxList<MemberDeclarationSyntax> members) =>
            String.IsNullOrEmpty(@namespace)
                ? List<MemberDeclarationSyntax>(members)
                : SingletonList<MemberDeclarationSyntax>(
                    NamespaceDeclaration(IdentifierName(@namespace))
                        .WithMembers(members));
    }
}
