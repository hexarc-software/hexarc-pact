using System;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed partial class MethodEmitter
    {
        private BlockSyntax EmitGetMethodBody(Method method) =>
            method.ReturnType.ResultType is null
                ? this.EmitGetVoidMethodBody(method)
                : this.EmitGetJsonMethodBody(method);

        private BlockSyntax EmitGetVoidMethodBody(Method method) =>
            Block(SingletonList(ExpressionStatement(AwaitExpression(
                InvocationExpression(this.EmitGetVoidAccess(method.ReturnType))
                    .WithArgumentList(this.EmitGetMethodArguments(method))))));

        private MemberAccessExpressionSyntax EmitGetVoidAccess(TaskTypeReference returnType) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ThisExpression(),
                IdentifierName(this.PickGetVoidMethodName(returnType)));

        private BlockSyntax EmitGetJsonMethodBody(Method method) =>
            Block(SingletonList(ReturnStatement(AwaitExpression(
                InvocationExpression(this.EmitGetJsonAccess(method.ReturnType))
                    .WithArgumentList(this.EmitGetMethodArguments(method))))));

        private MemberAccessExpressionSyntax EmitGetJsonAccess(TaskTypeReference returnType) =>
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                ThisExpression(),
                GenericName(Identifier(this.PickGetJsonMethodName(returnType)))
                    .WithTypeArgumentList(this.EmitGetJsonParameters(returnType)));

        private TypeArgumentListSyntax EmitGetJsonParameters(TaskTypeReference returnType) =>
            TypeArgumentList(
                SeparatedList<SyntaxNode>(
                    new SyntaxNodeOrTokenList(
                        this.TypeReferenceEmitter.Emit(returnType.ResultType!))));

        private ArgumentListSyntax EmitGetMethodArguments(Method method) =>
            ArgumentList(
                SeparatedListWithCommas(
                    Argument(LiteralExpressionFromString(method.Path)),
                    Argument(method.Parameters.Length == 0
                        ? ArrayEmptyCall(typeof(GetMethodParameter))
                        : this.EmitGetMethodParameters(method.Parameters)),
                    Argument(IdentifierName("headers"))));

        private ImplicitArrayCreationExpressionSyntax EmitGetMethodParameters(MethodParameter[] parameters) =>
            ImplicitArrayWithElements(parameters.Select(this.EmitGetMethodParameter).ToArray());

        private ExpressionSyntax EmitGetMethodParameter(MethodParameter parameter) =>
            ObjectCreationExpression(
                    IdentifierNameFromType(typeof(GetMethodParameter)))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedListWithCommas(
                            Argument(NameOfExpression(parameter.Name)),
                            Argument(IdentifierName(parameter.Name)))));

        private String PickGetJsonMethodName(TaskTypeReference returnType) =>
            returnType.ResultType is NullableTypeReference ? "GetJsonOrNull" : "GetJson";

        private String PickGetVoidMethodName(TaskTypeReference returnType) => "GetVoid";
    }
}
