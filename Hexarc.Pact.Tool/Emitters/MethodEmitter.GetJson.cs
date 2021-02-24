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
                GenericName(Identifier(this.PickGetMethodName(method.ReturnType)))
                    .WithTypeArgumentList(this.EmitGetJsonParameters(method)));

        private TypeArgumentListSyntax EmitGetJsonParameters(Method method) =>
            TypeArgumentList(
                SeparatedList<SyntaxNode>(
                    new SyntaxNodeOrTokenList(
                        this.TypeReferenceEmitter.Emit(method.ReturnType.ResultType))));

        private ArgumentListSyntax EmitGetJsonArguments(Method method) =>
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

        private String PickGetMethodName(TaskTypeReference returnType) =>
            returnType.ResultType is NullableTypeReference ? "GetJsonOrNull" : "GetJson";
    }
}
