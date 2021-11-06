namespace Hexarc.Pact.Tool.Syntax;

public static partial class SyntaxFactory
{
    public static ImplicitArrayCreationExpressionSyntax ImplicitArrayWithElements(ExpressionSyntax[] elements) =>
        ImplicitArrayCreationExpression(
            InitializerExpression(
                SyntaxKind.ArrayInitializerExpression,
                SeparatedListWithCommas(elements)));

    public static InvocationExpressionSyntax ArrayEmptyCall(Type elementType) =>
        InvocationExpression(
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierNameFromType(typeof(Array)),
                GenericWithArgument(
                    Identifier(nameof(Array.Empty)),
                    IdentifierName(elementType.FullName!))));
}
