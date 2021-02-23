using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Internals;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

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
                SeparatedListWithCommas(parameters
                    .Select(this.EmitMethodParameter)
                    .Concat(EnumerableFactory.FromOne(this.EmitMethodHeadersParameter()))
                    .ToArray()));

        private ParameterSyntax EmitMethodParameter(MethodParameter parameter) =>
            Parameter(Identifier(parameter.Name))
                .WithType(this.TypeReferenceEmitter.Emit(parameter.Type));

        private ParameterSyntax EmitMethodHeadersParameter() =>
            Parameter(Identifier("headers"))
                .WithType(
                    NullableType(
                        GenericWithArgument(
                            IdentifierFromType(typeof(IEnumerable<>)),
                            GenericWithArguments(
                                IdentifierFromType(typeof(KeyValuePair<,>)),
                                IdentifierNameFromType(typeof(String)),
                                IdentifierNameFromType(typeof(String))))))
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
