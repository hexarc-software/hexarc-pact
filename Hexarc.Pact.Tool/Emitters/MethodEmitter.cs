using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Extensions;

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
                    this.TypeReferenceEmitter.Emit(method.Result.Type),
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
                    .Separate(parameters.Length, Token(SyntaxKind.CommaToken))));

        private SyntaxNodeOrToken EmitMethodParameter(MethodParameter parameter) =>
            Parameter(Identifier(parameter.Name))
                .WithType(this.TypeReferenceEmitter.Emit(parameter.Type));

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
