using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax
{
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
                    IdentifierName(typeof(Array).FullName!),
                    GenericWithArgument(
                        Identifier(nameof(Array.Empty)),
                        IdentifierName(elementType.FullName!))));
    }
}
