using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Internals.SyntaxOperations;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class ApiEmitter
    {
        private AdhocWorkspace Workspace { get; } = new();

        private TypeRegistry TypeRegistry { get; }

        private DistinctTypeEmitter DistinctTypeEmitter { get; }

        public ApiEmitter(TypeRegistry typeRegistry, DistinctTypeEmitter distinctTypeEmitter) =>
            (this.TypeRegistry, this.DistinctTypeEmitter) = (typeRegistry, distinctTypeEmitter);

        public IEnumerable<EmittedSource> EmitTypes() =>
            this.TypeRegistry.EnumerateDistinctTypes()
                .Select(this.DistinctTypeEmitter.Emit)
                .GroupBy(x => x.Name, x => x.MemberDeclarations)
                .Select(x => new EmittedDistinctType(x.Key, x.SelectMany(m => m).ToArray()))
                .Select(this.EmitType);

        private EmittedSource EmitType(EmittedDistinctType type) =>
            new(this.EmitCsharpFileName(type.Name), this.EmitSourceText(this.EmitCompilationUnion(type)));

        private CompilationUnitSyntax EmitCompilationUnion(EmittedDistinctType type) =>
            CompilationUnit()
                .WithMembers(List(type.MemberDeclarations))
                .WithLeadingTrivia(EmitCodegenComment());

        private SourceText EmitSourceText(CompilationUnitSyntax unit) =>
            Formatter.Format(unit, this.Workspace).GetText();

        private String EmitCsharpFileName(String name) => $"{name}.cs";
    }
}
