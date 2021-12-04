using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Protocol.TypeReferences;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters;

public sealed partial class MethodEmitter
{
    private BlockSyntax EmitGetMethodBody(Method method) =>
        method.ReturnType.ResultType is null
            ? this.EmitDoGetRequestWithVoidResponse(method)
            : this.EmitDoGetRequestWithJsonResponse(method);

    private BlockSyntax EmitDoGetRequestWithVoidResponse(Method method) =>
        Block(SingletonList(ExpressionStatement(AwaitExpression(
            InvocationExpression(this.EmitDoGetRequestWithVoidResponseAccess())
                .WithArgumentList(this.EmitGetMethodArguments(method))))));

    private MemberAccessExpressionSyntax EmitDoGetRequestWithVoidResponseAccess() =>
        MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            ThisExpression(),
            IdentifierName("DoGetRequestWithVoidResponse"));

    private BlockSyntax EmitDoGetRequestWithJsonResponse(Method method) =>
        Block(SingletonList(ReturnStatement(AwaitExpression(
            InvocationExpression(this.EmitDoGetRequestWithJsonResponseAccess(method.ReturnType))
                .WithArgumentList(this.EmitGetMethodArguments(method))))));

    private MemberAccessExpressionSyntax EmitDoGetRequestWithJsonResponseAccess(TaskTypeReference returnType) =>
        MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            ThisExpression(),
            GenericName(Identifier("DoGetRequestWithJsonResponse"))
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
}
