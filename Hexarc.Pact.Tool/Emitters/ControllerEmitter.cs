using System.Linq;
using System.Collections.Generic;

using Hexarc.Pact.Protocol.Api;
using Hexarc.Pact.Tool.Models;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Hexarc.Pact.Tool.Emitters.SyntaxOperations;

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
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.SealedKeyword)
                    ))
                .WithMembers(
                    List<MemberDeclarationSyntax>(this.EmitMethods(controller.Methods)));

        private IEnumerable<MethodDeclarationSyntax> EmitMethods(Method[] methods) =>
            methods.Select(this.MethodEmitter.Emit);
    }
}
