using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

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
                name: GenericName(Identifier(this.PickPostMethodName(method.ReturnType)))
                    .WithTypeArgumentList(
                        this.EmitPostJsonParameters(method)));

        private TypeArgumentListSyntax EmitPostJsonParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.Parameters.First().Type),
                    this.TypeReferenceEmitter.Emit(method.ReturnType.ResultType)));

        private ArgumentListSyntax EmitPostJsonArguments(Method method) =>
            ArgumentList(
                SeparatedListWithCommas<ArgumentSyntax>(
                    Argument(LiteralExpressionFromString(method.Path)),
                    Argument(IdentifierName(method.Parameters.First().Name)),
                    Argument(IdentifierName("headers"))));

        private String PickPostMethodName(TaskTypeReference returnType) =>
            returnType.ResultType is NullableTypeReference ? "PostJsonOrNull" : "PostJson";
    }
}
