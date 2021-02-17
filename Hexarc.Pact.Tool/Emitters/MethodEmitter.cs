using System;
using System.Linq;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Internals.SyntaxOperations;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class MethodEmitter
    {
        private TypeReferenceEmitter TypeReferenceEmitter { get; }

        public MethodEmitter(TypeReferenceEmitter typeReferenceEmitter) =>
            this.TypeReferenceEmitter = typeReferenceEmitter;

        public MethodDeclarationSyntax Emit(Method method) =>
            MethodDeclaration(
                    this.TypeReferenceEmitter.Emit(method.ResultType),
                    Identifier(method.Name))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.AsyncKeyword)
                    ))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            method.Parameters
                                .Select(x =>
                                    (SyntaxNodeOrToken)Parameter(Identifier(x.Name))
                                        .WithType(this.TypeReferenceEmitter.Emit(x.Type)))
                                .Separate(method.Parameters.Length, Token(SyntaxKind.CommaToken))
                        )))
                .WithBody(
                    Block(
                        SingletonList(
                            ThrowStatement(
                                ObjectCreationExpression(
                                        IdentifierName(typeof(NotImplementedException).FullName!))
                                    .WithArgumentList(
                                        ArgumentList())))));
    }
}
