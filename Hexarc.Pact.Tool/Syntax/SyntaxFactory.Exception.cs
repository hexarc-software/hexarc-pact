namespace Hexarc.Pact.Tool.Syntax;

public static partial class SyntaxFactory
{
    public static ThrowStatementSyntax ThrowExceptionStatement(Type exceptionType) =>
        ThrowStatement(
            ObjectCreationExpression(
                    IdentifierNameFromType(exceptionType))
                .WithArgumentList(
                    ArgumentList()));
}
