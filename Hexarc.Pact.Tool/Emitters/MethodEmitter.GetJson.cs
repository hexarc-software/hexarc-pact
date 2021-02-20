using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.SyntaxFactories.NameOfSyntaxFactory;
using static Hexarc.Pact.Tool.SyntaxFactories.ArraySyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed partial class MethodEmitter
    {
        private BlockSyntax EmitGetJsonMethodBody(Method method) =>
            Block(SingletonList(ReturnStatement(this.EmitAwaitGetJson(method))));

        private AwaitExpressionSyntax EmitAwaitGetJson(Method method) =>
            AwaitExpression(
                InvocationExpression(this.EmitGetJsonAccess(method))
                    .WithArgumentList(this.EmitGetJsonArguments(method)));

        private MemberAccessExpressionSyntax EmitGetJsonAccess(Method method) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ThisExpression(),
                GenericName(Identifier("GetJson"))
                    .WithTypeArgumentList(this.EmitGetJsonParameters(method)));

        private TypeArgumentListSyntax EmitGetJsonParameters(Method method) =>
            TypeArgumentList(
                SeparatedList<SyntaxNode>(
                    new SyntaxNodeOrTokenList(
                        this.TypeReferenceEmitter.Emit(method.Result.Type.ResultType))));

        private ArgumentListSyntax EmitGetJsonArguments(Method method) =>
            ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    new SyntaxNodeOrTokenList(
                        Argument(
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal(method.Path))),
                        Token(SyntaxKind.CommaToken),
                        Argument(method.Parameters.Length == 0
                            ? ArrayEmptyExpression(typeof(GetMethodParameter))
                            : this.EmitGetMethodParameters(method.Parameters)))));

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
    }
}
