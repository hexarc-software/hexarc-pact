using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hexarc.Pact.Protocol.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed partial class MethodEmitter
    {
        private BlockSyntax EmitPostJsonMethodBody(Method method) =>
            Block(SingletonList(ReturnStatement(this.EmitAwaitPostJson(method))));

        private AwaitExpressionSyntax EmitAwaitPostJson(Method method) =>
            AwaitExpression(
                InvocationExpression(this.EmitPostJsonAccess(method))
                    .WithArgumentList(this.EmitPostJsonArguments(method)));

        private MemberAccessExpressionSyntax EmitPostJsonAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier("PostJson"))
                    .WithTypeArgumentList(
                        this.EmitPostJsonParameters(method)));

        private TypeArgumentListSyntax EmitPostJsonParameters(Method method) =>
            TypeArgumentList(
                SeparatedList<TypeSyntax>(
                    new SyntaxNodeOrTokenList(
                        this.TypeReferenceEmitter.Emit(method.Parameters.First().Type),
                        Token(SyntaxKind.CommaToken),
                        this.TypeReferenceEmitter.Emit(method.Result.Type.ResultType))));

        private ArgumentListSyntax EmitPostJsonArguments(Method method) =>
            ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    new SyntaxNodeOrTokenList(
                        Argument(
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal(method.Path))),
                        Token(SyntaxKind.CommaToken),
                        Argument(
                            IdentifierName(method.Parameters.First().Name)))));
    }
}