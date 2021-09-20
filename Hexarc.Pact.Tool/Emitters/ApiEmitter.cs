namespace Hexarc.Pact.Tool.Emitters;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Text;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
        this.TypeRegistry = new TypeRegistry(this.Schema.Types);

        var typeReferenceEmitter = new TypeReferenceEmitter(this.TypeRegistry);
        var methodEmitter = new MethodEmitter(typeReferenceEmitter);

        this.DistinctTypeEmitter = new DistinctTypeEmitter(this.TypeRegistry, typeReferenceEmitter);
        this.ControllerEmitter = new ControllerEmitter(methodEmitter);
        this.ClientEmitter = new ClientEmitter();
    }

    public EmittedApi Emit() =>
        new(this.EmitClient(), this.EmitControllers(), this.EmitTypes());

    public EmittedSource EmitClient() =>
        this.EmitTypeSource(this.ClientEmitter.Emit(this.ClientSettings, this.Schema.Controllers));

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

    private CompilationUnitSyntax EmitCompilationUnion(EmittedEntity entity) =>
        CompilationUnit()
            .WithMembers(List(entity.MemberDeclarations))
            .WithEndOfFileToken(this.EmitEndOfFileToken())
            .WithLeadingTrivia(this.EmitBeginOfFileTrivia());

    private SourceText EmitSourceText(CompilationUnitSyntax unit) =>
        Formatter.Format(unit, this.Workspace).GetText();

    private SyntaxTriviaList EmitBeginOfFileTrivia() =>
        EmitCodegenComment()
            .Add(NullableEnableDirective)
            .Add(LineFeed);

    private SyntaxToken EmitEndOfFileToken() =>
        Token(TriviaList(LineFeed, this.NullableRestoreDirective),
            SyntaxKind.EndOfFileToken,
            TriviaList());

    public SyntaxTriviaList EmitCodegenComment() =>
        this.ClientSettings.GenerationOptions?.OmitTimestampComment is true
            ? this.EmitCodegenCommentWithoutTimestamp()
            : this.EmitCodegenCommentWithTimestamp();

    public SyntaxTriviaList EmitCodegenCommentWithTimestamp() =>
        TriviaList(
            Comment("// <auto-generated>"), LineFeed,
            Comment("//   This code was generated by the Hexarc Pact tool. Do not edit."), LineFeed,
            Comment($"//   Created: {DateTime.UtcNow:u}"), LineFeed,
            Comment("// </auto-generated>"), LineFeed, LineFeed);

    public SyntaxTriviaList EmitCodegenCommentWithoutTimestamp() =>
        TriviaList(
            Comment("// <auto-generated>"), LineFeed,
            Comment("//   This code was generated by the Hexarc Pact tool. Do not edit."), LineFeed,
            Comment("// </auto-generated>"), LineFeed, LineFeed);

    public SyntaxTrivia NullableRestoreDirective { get; } =
        Trivia(
            NullableDirectiveTrivia(
                Token(SyntaxKind.RestoreKeyword),
                true));

    public SyntaxTrivia NullableEnableDirective { get; } =
        Trivia(
            NullableDirectiveTrivia(
                    Token(SyntaxKind.EnableKeyword),
                    true)
                .WithNullableKeyword(
                    Token(TriviaList(),
                        SyntaxKind.NullableKeyword,
                        TriviaList(
                            Space)))
                .WithEndOfDirectiveToken(
                    Token(
                        TriviaList(),
                        SyntaxKind.EndOfDirectiveToken,
                        TriviaList(
                            LineFeed))));

    private String EmitCsharpFileName(String name) => $"{name}.cs";
}
