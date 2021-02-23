using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Hexarc.Pact.Tool.Syntax
{
    public static partial class SyntaxFactory
    {
        public static GenericNameSyntax GenericWithArgument(SyntaxToken name, TypeSyntax argument) =>
            GenericName(name,
                TypeArgumentList(
                    SingletonSeparatedList(argument)));

        public static GenericNameSyntax GenericWithArguments(SyntaxToken name, TypeSyntax[] arguments) =>
            GenericName(name,
                TypeArgumentList(
                    SeparatedListWithCommas(arguments)));
    }
}
