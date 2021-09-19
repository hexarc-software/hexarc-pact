using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax;

public static partial class SyntaxFactory
{
    public static InvocationExpressionSyntax NameOfExpression(String argument) =>
        NameOfExpression(IdentifierName(argument));

    public static InvocationExpressionSyntax NameOfExpression(IdentifierNameSyntax argument) =>
        NameOfExpression(Argument(argument));

    public static InvocationExpressionSyntax NameOfExpression(ArgumentSyntax argument) =>
        InvocationExpression(IdentifierName("nameof"))
            .WithArgumentList(ArgumentList(SingletonSeparatedList(argument)));
}
