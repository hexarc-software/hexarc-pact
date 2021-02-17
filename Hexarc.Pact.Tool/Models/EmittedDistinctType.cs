using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class EmittedDistinctType
    {
        public String Name { get; }

        public MemberDeclarationSyntax[] MemberDeclarations { get; }

        public EmittedDistinctType(String name, MemberDeclarationSyntax memberDeclaration) =>
            (this.Name, this.MemberDeclarations) = (name, new[] { memberDeclaration });

        public EmittedDistinctType(String name, IEnumerable<MemberDeclarationSyntax> membersDeclarations) =>
            (this.Name, this.MemberDeclarations) = (name, membersDeclarations.ToArray());

        public EmittedDistinctType(String name, MemberDeclarationSyntax[] membersDeclarations) =>
            (this.Name, this.MemberDeclarations) = (name, membersDeclarations.ToArray());
    }
}
