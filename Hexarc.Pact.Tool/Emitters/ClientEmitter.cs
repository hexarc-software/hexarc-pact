using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Extensions;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters;

public sealed class ClientEmitter
{
    public EmittedEntity Emit(ClientSettings clientSettings, Controller[] controllers) =>
        new(clientSettings.ClientClassName,
            TryWrapInNamespace(
                clientSettings.ClientClassNamespace,
                this.EmitClientClass(clientSettings, controllers)));

    private ClassDeclarationSyntax EmitClientClass(ClientSettings clientSettings, Controller[] controllers) =>
        ClassDeclaration(Identifier(clientSettings.ClientClassName))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.SealedKeyword),
                    Token(SyntaxKind.PartialKeyword)))
            .WithBaseList(
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(
                            IdentifierNameFromType(typeof(ClientBase))))))
            .WithMembers(List(this.EmitMembers(clientSettings, controllers)));

    private IEnumerable<MemberDeclarationSyntax> EmitMembers(ClientSettings clientSettings, Controller[] controllers) =>
        this.EmitControllerProperties(controllers)
            .Concat(EnumerableFactory.FromOne(this.EmitConstructor(clientSettings, controllers)));

    private MemberDeclarationSyntax EmitConstructor(ClientSettings clientSettings, Controller[] controllers) =>
        ConstructorDeclaration(Identifier(clientSettings.ClientClassName))
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(this.EmitConstructorParameters())
            .WithInitializer(this.EmitConstructorInitializer())
            .WithBody(this.EmitConstructorBody(controllers));

    private ParameterListSyntax EmitConstructorParameters() =>
        ParameterList(
            SeparatedList<ParameterSyntax>(
                SingletonList(
                    Parameter(Identifier("httpClient"))
                        .WithType(IdentifierNameFromType(typeof(HttpClient))))));

    private ConstructorInitializerSyntax EmitConstructorInitializer() =>
        ConstructorInitializer(
            SyntaxKind.BaseConstructorInitializer,
            ArgumentList(
                SeparatedList<ArgumentSyntax>(
                    SingletonList(
                        Argument(IdentifierName("httpClient"))))));

    private BlockSyntax EmitConstructorBody(Controller[] controllers) =>
        Block(controllers.Select(this.EmitControllerPropertyInitializer));

    private ExpressionStatementSyntax EmitControllerPropertyInitializer(Controller controller) =>
        ExpressionStatement(
            AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    ThisExpression(),
                    IdentifierName(controller.Name.StripSuffix("Controller"))),
                ObjectCreationExpression(
                        IdentifierName(controller.FullName))
                    .WithArgumentList(
                        ArgumentList(
                            SingletonSeparatedList<ArgumentSyntax>(
                                Argument(ThisExpression()))))));

    private IEnumerable<PropertyDeclarationSyntax> EmitControllerProperties(Controller[] controllers) =>
        controllers.Select(this.EmitControllerProperty);

    private PropertyDeclarationSyntax EmitControllerProperty(Controller controller) =>
        PropertyDeclaration(IdentifierName(controller.FullName), controller.Name.StripSuffix("Controller"))
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithAccessorList(
                AccessorList(
                    SingletonList<AccessorDeclarationSyntax>(
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Semicolon))));
}
