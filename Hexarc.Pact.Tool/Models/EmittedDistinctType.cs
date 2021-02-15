using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class EmittedDistinctType
    {
        public String Name { get; }

        public SyntaxList<MemberDeclarationSyntax> MembersDeclarations { get; }

        public EmittedDistinctType(String name, MemberDeclarationSyntax membersDeclaration) =>
            (this.Name, this.MembersDeclarations) = (name, SingletonList(membersDeclaration));

        public EmittedDistinctType(String name, SyntaxList<MemberDeclarationSyntax> membersDeclarations) =>
            (this.Name, this.MembersDeclarations) = (name, membersDeclarations);
    }
}
