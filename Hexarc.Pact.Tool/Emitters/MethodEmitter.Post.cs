using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hexarc.Pact.Protocol.Api;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed partial class MethodEmitter
    {
        private BlockSyntax EmitPostMethodBody(Method method)
        {
            var isVoidRequest = method.Parameters.Length == 0;
            var isVoidResponse = method.ReturnType.ResultType is null;
            return (isVoidRequest, isVoidResponse) switch
            {
                (true, true) => EmitDoPostVoidRequestWithVoidResponse(method),
                (true, false) => EmitDoPostVoidRequestWithJsonResponse(method),
                (false, true) => EmitDoPostJsonRequestWithVoidResponse(method),
                (false, false) => EmitDoPostJsonRequestWithJsonResponse(method)
            };
        }

        private BlockSyntax EmitDoPostVoidRequestWithVoidResponse(Method method) =>
            Block(SingletonList(ExpressionStatement(AwaitExpression(
                InvocationExpression(this.EmitDoPostVoidRequestWithVoidResponseAccess(method))
                    .WithArgumentList(this.EmitPostVoidRequestArguments(method))))));

        private BlockSyntax EmitDoPostVoidRequestWithJsonResponse(Method method) =>
            Block(SingletonList(ReturnStatement(AwaitExpression(
                InvocationExpression(this.EmitDoPostVoidRequestWithJsonResponseAccess(method))
                    .WithArgumentList(this.EmitPostVoidRequestArguments(method))))));

        private BlockSyntax EmitDoPostJsonRequestWithVoidResponse(Method method) =>
            Block(SingletonList(ExpressionStatement(AwaitExpression(
                InvocationExpression(this.EmitDoPostJsonRequestWithVoidResponseAccess(method))
                    .WithArgumentList(this.EmitPostJsonRequestArguments(method))))));

        private BlockSyntax EmitDoPostJsonRequestWithJsonResponse(Method method) =>
            Block(SingletonList(ReturnStatement(AwaitExpression(
                InvocationExpression(this.EmitDoPostJsonRequestWithJsonResponseAccess(method))
                    .WithArgumentList(this.EmitPostJsonRequestArguments(method))))));

        private MemberAccessExpressionSyntax EmitDoPostVoidRequestWithVoidResponseAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: IdentifierName("DoPostVoidRequestWithVoidResponse"));

        private MemberAccessExpressionSyntax EmitDoPostVoidRequestWithJsonResponseAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier("DoPostVoidRequestWithJsonResponse"))
                    .WithTypeArgumentList(
                        this.EmitPostVoidRequestWithJsonResponseParameters(method)));

        private MemberAccessExpressionSyntax EmitDoPostJsonRequestWithVoidResponseAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier("DoPostJsonRequestWithVoidResponse"))
                    .WithTypeArgumentList(
                        this.EmitPostJsonRequestWithVoidResponseParameters(method)));

        private MemberAccessExpressionSyntax EmitDoPostJsonRequestWithJsonResponseAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier("DoPostJsonRequestWithJsonResponse"))
                    .WithTypeArgumentList(
                        this.EmitPostJsonRequestWithJsonResponseParameters(method)));

        private TypeArgumentListSyntax EmitPostVoidRequestWithJsonResponseParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.ReturnType.ResultType!)));

        private TypeArgumentListSyntax EmitPostJsonRequestWithVoidResponseParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.Parameters.First().Type)));

        private TypeArgumentListSyntax EmitPostJsonRequestWithJsonResponseParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.Parameters.First().Type),
                    this.TypeReferenceEmitter.Emit(method.ReturnType.ResultType!)));

        private ArgumentListSyntax EmitPostJsonRequestArguments(Method method) =>
            ArgumentList(
                SeparatedListWithCommas<ArgumentSyntax>(
                    Argument(LiteralExpressionFromString(method.Path)),
                    Argument(IdentifierName(method.Parameters.First().Name)),
                    Argument(IdentifierName("headers"))));

        private ArgumentListSyntax EmitPostVoidRequestArguments(Method method) =>
            ArgumentList(
                SeparatedListWithCommas<ArgumentSyntax>(
                    Argument(LiteralExpressionFromString(method.Path)),
                    Argument(IdentifierName("headers"))));
    }
}
