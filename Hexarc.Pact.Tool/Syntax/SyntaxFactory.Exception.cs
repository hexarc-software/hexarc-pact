using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax
{
    public static partial class SyntaxFactory
    {
        public static ThrowStatementSyntax ThrowExceptionStatement(Type exceptionType) =>
            ThrowStatement(
                ObjectCreationExpression(
                        IdentifierNameFromType(exceptionType))
                    .WithArgumentList(
                        ArgumentList()));
    }
}
