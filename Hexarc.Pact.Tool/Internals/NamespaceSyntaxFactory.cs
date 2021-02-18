using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Internals
{
    public static class NamespaceSyntaxFactory
    {
        public static MemberDeclarationSyntax TryWrapInNamespace(String? @namespace, MemberDeclarationSyntax memberDeclaration) =>
            String.IsNullOrEmpty(@namespace) ? memberDeclaration : WrapInNamespace(@namespace, memberDeclaration);

        public static IEnumerable<MemberDeclarationSyntax> TryWrapInNamespace(String? @namespace, IEnumerable<MemberDeclarationSyntax> memberDeclarations) =>
            String.IsNullOrEmpty(@namespace) ? memberDeclarations : EnumerableFactory.FromOne(WrapInNamespace(@namespace, memberDeclarations));

        public static NamespaceDeclarationSyntax WrapInNamespace(String @namespace, MemberDeclarationSyntax memberDeclaration) =>
            NamespaceDeclaration(IdentifierName(@namespace)).WithMembers(SingletonList(memberDeclaration));

        public static NamespaceDeclarationSyntax WrapInNamespace(String @namespace, IEnumerable<MemberDeclarationSyntax> memberDeclarations) =>
            NamespaceDeclaration(IdentifierName(@namespace)).WithMembers(List(memberDeclarations));
    }
}
