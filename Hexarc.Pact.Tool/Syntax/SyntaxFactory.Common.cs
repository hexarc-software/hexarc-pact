using Hexarc.Pact.Protocol.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax;

public static partial class SyntaxFactory
{
    public static SyntaxToken Comma { get; } = Token(SyntaxKind.CommaToken);

    public static SyntaxToken Semicolon { get; } = Token(SyntaxKind.SemicolonToken);

    public static IEnumerable<TNode> Repeat<TNode>(TNode node, Int32 count) =>
        Enumerable.Repeat(node, count);

    public static SeparatedSyntaxList<TNode> SeparatedListWithCommas<TNode>(params TNode[] nodes) where TNode : SyntaxNode =>
        SeparatedList<TNode>(nodes, Repeat(Comma, nodes.Length - 1));

    public static IdentifierNameSyntax IdentifierNameFromType<T>() =>
        IdentifierNameFromType(typeof(T));

    public static IdentifierNameSyntax IdentifierNameFromType(Type type) =>
        IdentifierName(type.FullNameWithoutGenericArity());

    public static SyntaxToken IdentifierFromType<T>() =>
        IdentifierFromType(typeof(T));

    public static SyntaxToken IdentifierFromType(Type type) =>
        Identifier(type.FullNameWithoutGenericArity());

    public static LiteralExpressionSyntax LiteralExpressionFromString(String value) =>
        LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(value));
}
