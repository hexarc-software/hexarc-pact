using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Extensions;
using Hexarc.Pact.Tool.Internals;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.SyntaxFactories.ExceptionSyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed partial class MethodEmitter
    {
        private TypeReferenceEmitter TypeReferenceEmitter { get; }

        public MethodEmitter(TypeReferenceEmitter typeReferenceEmitter) =>
            this.TypeReferenceEmitter = typeReferenceEmitter;

        public MethodDeclarationSyntax Emit(Method method) =>
            MethodDeclaration(
                    this.TypeReferenceEmitter.Emit(method.ReturnType),
                    Identifier(method.Name))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AsyncKeyword)
                    ))
                .WithParameterList(this.EmitMethodParameters(method.Parameters))
                .WithBody(this.EmitMethodBody(method));

        private ParameterListSyntax EmitMethodParameters(MethodParameter[] parameters) =>
            ParameterList(
                SeparatedList<ParameterSyntax>(parameters
                    .Select(this.EmitMethodParameter)
                    .Concat(EnumerableFactory.FromOne<SyntaxNodeOrToken>(this.EmitMethodHeadersParameter()))
                    .Separate(parameters.Length + 1, Token(SyntaxKind.CommaToken))));

        private SyntaxNodeOrToken EmitMethodParameter(MethodParameter parameter) =>
            Parameter(Identifier(parameter.Name))
                .WithType(this.TypeReferenceEmitter.Emit(parameter.Type));

        private SyntaxNodeOrToken EmitMethodHeadersParameter() =>
            Parameter(
                    Identifier("headers"))
                .WithType(
                    NullableType(
                        GenericName(
                                Identifier(typeof(IEnumerable<>).FullName!.StripSuffix("`1")))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        GenericName(
                                                Identifier(typeof(KeyValuePair<,>).FullName!.StripSuffix("`2")))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SeparatedList<TypeSyntax>(
                                                        new SyntaxNodeOrToken[]
                                                        {
                                                            IdentifierName(typeof(String).FullName!),
                                                            Token(SyntaxKind.CommaToken),
                                                            IdentifierName(typeof(String).FullName!)
                                                        }))))))))
                .WithDefault(
                    EqualsValueClause(
                        LiteralExpression(
                            SyntaxKind.DefaultLiteralExpression,
                            Token(SyntaxKind.DefaultKeyword))));

        private BlockSyntax EmitMethodBody(Method method) => method.HttpMethod switch
        {
            HttpMethod.Get => this.EmitGetJsonMethodBody(method),
            HttpMethod.Post => this.EmitPostJsonMethodBody(method),
            _ => NotImplementedExceptionBlock
        };

        private static BlockSyntax NotImplementedExceptionBlock { get; } =
            Block(SingletonList(ThrowExceptionStatement(typeof(NotImplementedException))));
    }
}
