using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax
{
    public static partial class SyntaxFactory
    {
        public static SyntaxToken Comma { get; } = Token(SyntaxKind.CommaToken);

        public static IEnumerable<TNode> Repeat<TNode>(TNode node, Int32 count) =>
            Enumerable.Repeat(node, count);

        public static SeparatedSyntaxList<TNode> SeparatedListWithCommas<TNode>(TNode[] nodes) where TNode : SyntaxNode =>
            SeparatedList<TNode>(nodes, Repeat(Comma, nodes.Length - 1));
    }
}
