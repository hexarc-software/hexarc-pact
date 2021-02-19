using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Hexarc.Pact.Tool.Extensions;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.SyntaxFactories
{
    public static class ArraySyntaxFactory
    {
        public static ImplicitArrayCreationExpressionSyntax NewImplicitArrayExpression(SyntaxNodeOrToken[] elements) =>
            ImplicitArrayCreationExpression(
                InitializerExpression(
                    SyntaxKind.ArrayInitializerExpression,
                    SeparatedList<ExpressionSyntax>(
                        elements.Separate(elements.Length, Token(SyntaxKind.CommaToken)))));

        public static InvocationExpressionSyntax ArrayEmptyExpression(Type elementType) =>
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(typeof(Array).FullName!),
                    GenericName(nameof(Array.Empty))
                        .WithTypeArgumentList(
                            TypeArgumentList(
                                SingletonSeparatedList<TypeSyntax>(
                                    IdentifierName(elementType.FullName!))))));
    }
}
