using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.SyntaxFactories
{
    public static class ExceptionSyntaxFactory
    {
        public static ThrowStatementSyntax ThrowExceptionStatement(Type exceptionType) =>
            ThrowStatement(
                ObjectCreationExpression(
                        IdentifierName(exceptionType.FullName!))
                    .WithArgumentList(
                        ArgumentList()));
    }
}