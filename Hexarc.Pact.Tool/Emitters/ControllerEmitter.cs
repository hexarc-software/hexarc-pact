using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Hexarc.Pact.Client;
using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters
{
    public sealed class ControllerEmitter
    {
        private MethodEmitter MethodEmitter { get; }

        public ControllerEmitter(MethodEmitter methodEmitter) =>
            this.MethodEmitter = methodEmitter;

        public EmittedEntity Emit(Controller controller) =>
            new(controller.Name,
                TryWrapInNamespace(
                    controller.Namespace,
                    this.EmitControllerDeclaration(controller)));

        private ClassDeclarationSyntax EmitControllerDeclaration(Controller controller) =>
            ClassDeclaration(
                    Identifier(controller.Name))
                .WithBaseList(
                    BaseList(
                        SingletonSeparatedList<BaseTypeSyntax>(
                            SimpleBaseType(
                                IdentifierName(typeof(ControllerBase).FullName!)))))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.SealedKeyword),
                        Token(SyntaxKind.PartialKeyword)))
                .WithMembers(
                    List<MemberDeclarationSyntax>(this.EmitMembers(controller)));

        private IEnumerable<MemberDeclarationSyntax> EmitMembers(Controller controller) =>
            EnumerableFactory.FromOne<MemberDeclarationSyntax>(this.EmitConstructor(controller))
                .Concat(this.EmitMethods(controller.Methods));

        private ConstructorDeclarationSyntax EmitConstructor(Controller controller) =>
            ConstructorDeclaration(Identifier(controller.Name))
                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(this.EmitConstructorParameters(controller))
                .WithInitializer(this.EmitConstructorInitializer())
                .WithBody(Block());

        private ParameterListSyntax EmitConstructorParameters(Controller controller) =>
            ParameterList(
                SeparatedListWithCommas(
                    Parameter(Identifier("client"))
                        .WithType(IdentifierNameFromType(typeof(ClientBase))),
                    Parameter(Identifier("controllerPath"))
                        .WithType(IdentifierNameFromType(typeof(String)))
                        .WithDefault(
                            EqualsValueClause(
                                LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    Literal(controller.Path))))));

        private ConstructorInitializerSyntax EmitConstructorInitializer() =>
            ConstructorInitializer(
                SyntaxKind.BaseConstructorInitializer,
                ArgumentList(
                    SeparatedListWithCommas(
                        Argument(IdentifierName("client")),
                        Argument(IdentifierName("controllerPath")))));

        private IEnumerable<MethodDeclarationSyntax> EmitMethods(Method[] methods) =>
            methods.Select(this.MethodEmitter.Emit);
    }
}
