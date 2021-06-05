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
        private BlockSyntax EmitPostMethodBody(Method method) =>
            method.ReturnType.ResultType is null
                ? this.EmitPostJsonVoidMethodBody(method)
                : this.EmitPostJsonMethodBody(method);

        private BlockSyntax EmitPostJsonVoidMethodBody(Method method) =>
            Block(SingletonList(ExpressionStatement(AwaitExpression(
                InvocationExpression(this.EmitPostJsonVoidAccess(method))
                    .WithArgumentList(this.EmitPostJsonArguments(method))))));

        private MemberAccessExpressionSyntax EmitPostJsonVoidAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier(this.PickPostJsonVoidMethodName()))
                    .WithTypeArgumentList(
                        this.EmitPostJsonVoidParameters(method)));

        private BlockSyntax EmitPostJsonMethodBody(Method method) =>
            Block(SingletonList(ReturnStatement(AwaitExpression(
                InvocationExpression(this.EmitPostJsonAccess(method))
                    .WithArgumentList(this.EmitPostJsonArguments(method))))));

        private MemberAccessExpressionSyntax EmitPostJsonAccess(Method method) =>
            MemberAccessExpression(
                kind: SyntaxKind.SimpleMemberAccessExpression,
                expression: ThisExpression(),
                name: GenericName(Identifier(this.PickPostJsonMethodName(method.ReturnType)))
                    .WithTypeArgumentList(
                        this.EmitPostJsonParameters(method)));

        private TypeArgumentListSyntax EmitPostJsonVoidParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.Parameters.First().Type)));

        private TypeArgumentListSyntax EmitPostJsonParameters(Method method) =>
            TypeArgumentList(
                SeparatedListWithCommas(
                    this.TypeReferenceEmitter.Emit(method.Parameters.First().Type),
                    this.TypeReferenceEmitter.Emit(method.ReturnType.ResultType!)));

        private ArgumentListSyntax EmitPostJsonArguments(Method method) =>
            ArgumentList(
                SeparatedListWithCommas<ArgumentSyntax>(
                    Argument(LiteralExpressionFromString(method.Path)),
                    Argument(IdentifierName(method.Parameters.First().Name)),
                    Argument(IdentifierName("headers"))));

        private String PickPostJsonMethodName(TaskTypeReference returnType) =>
            returnType.ResultType is NullableTypeReference ? "PostJsonOrNull" : "PostJson";

        private String PickPostJsonVoidMethodName() => "PostJsonVoid";
    }
}
