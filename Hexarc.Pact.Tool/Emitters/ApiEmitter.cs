using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;
using Hexarc.Pact.Protocol.Api;
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

        private ControllerEmitter ControllerEmitter { get; }

        private ClientEmitter ClientEmitter { get; }

        public ApiEmitter(
            TypeRegistry typeRegistry,
            DistinctTypeEmitter distinctTypeEmitter,
            ControllerEmitter controllerEmitter,
            ClientEmitter clientEmitter)
        {
            this.TypeRegistry = typeRegistry;
            this.DistinctTypeEmitter = distinctTypeEmitter;
            this.ControllerEmitter = controllerEmitter;
            this.ClientEmitter = clientEmitter;
        }

        public EmittedSource EmitClient() =>
            throw new NotImplementedException();

        public IEnumerable<EmittedSource> EmitControllers(Controller[] controllers) =>
            controllers.Select(this.ControllerEmitter.Emit)
                .Select(this.EmitController);

        private EmittedSource EmitController(EmittedDistinctType type) =>
            new(this.EmitCsharpFileName(type.Name), this.EmitSourceText(this.EmitCompilationUnion(type)));

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
                .WithEndOfFileToken(this.EndOfFileToken)
                .WithLeadingTrivia(this.EmitBeginOfFileTrivia());

        private SyntaxTriviaList EmitBeginOfFileTrivia() =>
            EmitCodegenComment()
                .Add(NullableEnableDirective)
                .Add(LineFeed);

        private SyntaxToken EndOfFileToken { get; } =
            Token(TriviaList(LineFeed, NullableRestoreDirective),
                SyntaxKind.EndOfFileToken,
                TriviaList());

        private SourceText EmitSourceText(CompilationUnitSyntax unit) =>
            Formatter.Format(unit, this.Workspace).GetText();

        private String EmitCsharpFileName(String name) => $"{name}.cs";
    }
}
