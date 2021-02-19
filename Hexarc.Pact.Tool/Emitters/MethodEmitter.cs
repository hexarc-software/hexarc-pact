using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Extensions;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.SyntaxFactories.ArraySyntaxFactory;
using static Hexarc.Pact.Tool.SyntaxFactories.NameOfSyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class MethodEmitter
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

        private BlockSyntax EmitGetJsonMethodBody(Method method) =>
            Block();

        private BlockSyntax EmitPostJsonMethodBody(Method method) =>
            Block(
                SingletonList<StatementSyntax>(
                    ReturnStatement(
                        AwaitExpression(
                            InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        ThisExpression(),
                                        GenericName(
                                                Identifier("PostJson"))
                                            .WithTypeArgumentList(
                                                TypeArgumentList(
                                                    SeparatedList<TypeSyntax>(
                                                        new SyntaxNodeOrTokenList(
                                                            this.TypeReferenceEmitter.Emit(method.Parameters.First()
                                                                .Type),
                                                            Token(SyntaxKind.CommaToken),
                                                            this.TypeReferenceEmitter.Emit(
                                                                method.Result.Type.ResultType)))))))
                                .WithArgumentList(
                                    ArgumentList(
                                        SeparatedList<ArgumentSyntax>(
                                            new SyntaxNodeOrTokenList(
                                                Argument(
                                                    LiteralExpression(
                                                        SyntaxKind.StringLiteralExpression,
                                                        Literal(method.Path))),
                                                Token(SyntaxKind.CommaToken),
                                                Argument(
                                                    IdentifierName(method.Parameters.First().Name))))))))));

        private ImplicitArrayCreationExpressionSyntax EmitGetMethodParameters(MethodParameter[] parameters) =>
            NewImplicitArrayExpression(parameters.Select(this.EmitGetMethodParameter).ToArray());

        private SyntaxNodeOrToken EmitGetMethodParameter(MethodParameter parameter) =>
            ObjectCreationExpression(
                    IdentifierName(typeof(GetMethodParameter).FullName!))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrTokenList(
                                Argument(NameOfExpression(parameter.Name)),
                                Token(SyntaxKind.CommaToken),
                                Argument(IdentifierName(parameter.Name))))));

        private static BlockSyntax NotImplementedExceptionBlock { get; } =
            Block(
                SingletonList(
                    ThrowStatement(
                        ObjectCreationExpression(
                                IdentifierName(typeof(NotImplementedException).FullName!))
                            .WithArgumentList(
                                ArgumentList()))));
    }
}
