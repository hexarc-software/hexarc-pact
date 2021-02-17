using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class EmittedEntity
    {
        public String Name { get; }

        public MemberDeclarationSyntax[] MemberDeclarations { get; }

        public EmittedEntity(String name, MemberDeclarationSyntax memberDeclaration) =>
            (this.Name, this.MemberDeclarations) = (name, new[] { memberDeclaration });

        public EmittedEntity(String name, IEnumerable<MemberDeclarationSyntax> membersDeclarations) =>
            (this.Name, this.MemberDeclarations) = (name, membersDeclarations.ToArray());

        public EmittedEntity(String name, MemberDeclarationSyntax[] membersDeclarations) =>
            (this.Name, this.MemberDeclarations) = (name, membersDeclarations);
    }
}
