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
using static Hexarc.Pact.Tool.Emitters.SyntaxOperations;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class ApiEmitter
    {
        private AdhocWorkspace Workspace { get; } = new();

        private ClientSettings ClientSettings { get; }

        private Schema Schema { get; }

        private TypeRegistry TypeRegistry { get; }

        private DistinctTypeEmitter DistinctTypeEmitter { get; }

        private ControllerEmitter ControllerEmitter { get; }

        private ClientEmitter ClientEmitter { get; }

        public ApiEmitter(ClientSettings clientSettings, Schema schema)
        {
            this.ClientSettings = clientSettings;
            this.Schema = schema;
            this.TypeRegistry = TypeRegistry.FromTypes(this.Schema.Types);

            var typeReferenceEmitter = new TypeReferenceEmitter(this.TypeRegistry);
            var methodEmitter = new MethodEmitter(typeReferenceEmitter);

            this.DistinctTypeEmitter = new DistinctTypeEmitter(typeReferenceEmitter);
            this.ControllerEmitter = new ControllerEmitter(methodEmitter);
            this.ClientEmitter = new ClientEmitter();
        }

        public EmittedApi Emit() =>
            new(this.EmitClient(), this.EmitControllers(), this.EmitTypes());

        public EmittedSource EmitClient() =>
            this.EmitTypeSource(this.ClientEmitter.Emit());

        public IEnumerable<EmittedSource> EmitControllers() =>
            this.Schema.Controllers.Select(this.ControllerEmitter.Emit)
                .Select(this.EmitControllerSource);

        private EmittedSource EmitControllerSource(EmittedEntity controllerEntity) =>
            new(this.EmitCsharpFileName(controllerEntity.Name),
                this.EmitSourceText(this.EmitCompilationUnion(controllerEntity)));

        public IEnumerable<EmittedSource> EmitTypes() =>
            this.TypeRegistry.EnumerateDistinctTypes()
                .Select(this.DistinctTypeEmitter.Emit)
                .GroupBy(x => x.Name, x => x.MemberDeclarations)
                .Select(x => new EmittedEntity(x.Key, x.SelectMany(m => m)))
                .Select(this.EmitTypeSource);

        private EmittedSource EmitTypeSource(EmittedEntity typeEntity) =>
            new(this.EmitCsharpFileName(typeEntity.Name),
                this.EmitSourceText(this.EmitCompilationUnion(typeEntity)));

        // TODO: Move compilation unit
        private CompilationUnitSyntax EmitCompilationUnion(EmittedEntity entity) =>
            CompilationUnit()
                .WithMembers(List(entity.MemberDeclarations))
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
