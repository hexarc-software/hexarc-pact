using Hexarc.Annotations;
using Hexarc.Pact.Protocol.Types;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.Tool.Internals;
using Hexarc.Pact.Tool.Models;
using static Hexarc.Pact.Tool.Syntax.SyntaxFactory;

namespace Hexarc.Pact.Tool.Emitters;

public sealed class DistinctTypeEmitter
{
    private TypeRegistry TypeRegistry { get; }

    private TypeReferenceEmitter TypeReferenceEmitter { get; }

    public DistinctTypeEmitter(TypeRegistry typeRegistry, TypeReferenceEmitter typeReferenceEmitter) =>
        (this.TypeRegistry, this.TypeReferenceEmitter) = (typeRegistry, typeReferenceEmitter);

    public EmittedEntity Emit(DistinctType distinctType) => distinctType switch
    {
        EnumType @enum => this.EmitEnumType(@enum),
        StringEnumType stringEnum => this.EmitStringEnumType(stringEnum),
        StructType @struct => this.EmitStructType(@struct),
        ClassType @class => this.EmitClassType(@class),
        UnionType union => this.EmitUnionType(union),
        _ => throw new InvalidOperationException($"Could not emit a Hexarc Pact type from {distinctType}")
    };

    private EmittedEntity EmitStringEnumType(StringEnumType type) =>
        new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitStringEnumDeclaration(type)));

    private EmittedEntity EmitEnumType(EnumType type) =>
        new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitEnumDeclaration(type)));

    private EmittedEntity EmitStructType(StructType type) =>
        new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitStructDeclaration(type)));

    private EmittedEntity EmitClassType(ClassType type) =>
        new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitClassDeclaration(type)));

    private EmittedEntity EmitUnionType(UnionType type) =>
        new(type.Name, TryWrapInNamespace(type.Namespace, this.EmitUnionDeclaration(type)));

    private EnumDeclarationSyntax EmitStringEnumDeclaration(StringEnumType type) =>
        EnumDeclaration(type.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .WithAttributeLists(
                SingletonList(
                    AttributeList(
                        SingletonSeparatedList(this.EmitStringEnumAttribute()))))
            .WithMembers(
                SeparatedList<EnumMemberDeclarationSyntax>(
                    this.EmitStringEnumMemberDeclarations(type.Members)));

    private AttributeSyntax EmitStringEnumAttribute() =>
        Attribute(IdentifierNameFromType(typeof(JsonConverterAttribute)))
            .WithArgumentList(
                AttributeArgumentList(
                    SingletonSeparatedList<AttributeArgumentSyntax>(
                        AttributeArgument(
                            TypeOfExpression(
                                IdentifierNameFromType(typeof(JsonStringEnumConverter)))))));

    private IEnumerable<EnumMemberDeclarationSyntax> EmitStringEnumMemberDeclarations(String[] members) =>
        members.Select(this.EmitStringEnumMemberDeclaration);

    private EnumMemberDeclarationSyntax EmitStringEnumMemberDeclaration(String member) =>
        EnumMemberDeclaration(member);

    private EnumDeclarationSyntax EmitEnumDeclaration(EnumType type) =>
        EnumDeclaration(type.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .WithMembers(
                SeparatedList<EnumMemberDeclarationSyntax>(
                    this.EmitEnumMemberDeclarations(type.Members)));

    private IEnumerable<EnumMemberDeclarationSyntax> EmitEnumMemberDeclarations(EnumMember[] members) =>
        members.Select(this.EmitEnumMemberDeclaration);

    private EnumMemberDeclarationSyntax EmitEnumMemberDeclaration(EnumMember member) =>
        EnumMemberDeclaration(member.Name)
            .WithEqualsValue(
                EqualsValueClause(
                    LiteralExpression(
                        SyntaxKind.NumericLiteralExpression,
                        Literal(member.Value))));

    private StructDeclarationSyntax EmitStructDeclaration(StructType type) =>
        StructDeclaration(type.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword)))
            .WithTypeParameterList(
                type.TypeParameters is not null
                    ? this.EmitGenericParameters(type.TypeParameters)
                    : default)
            .WithMembers(
                this.EmitObjectPropertyDeclarations(type.Properties, type.Namespace));

    private ClassDeclarationSyntax EmitClassDeclaration(ClassType type) =>
        ClassDeclaration(type.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.SealedKeyword)))
            .WithTypeParameterList(
                type.TypeParameters is not null
                    ? this.EmitGenericParameters(type.TypeParameters)
                    : default)
            .WithMembers(
                this.EmitObjectPropertyDeclarations(type.Properties, type.Namespace));

    private MemberDeclarationSyntax[] EmitUnionDeclaration(UnionType type) =>
        Enumerable.Empty<MemberDeclarationSyntax>()
            .Concat(EnumerableFactory.FromOne(this.EmitUnionBaseDeclaration(type)))
            .Concat(type.Cases.Select(x => this.EmitUnionCaseDeclaration(x, type.Name)))
            .ToArray();

    private ClassDeclarationSyntax EmitUnionBaseDeclaration(UnionType type) =>
        ClassDeclaration(type.Name)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.AbstractKeyword)))
            .WithAttributeLists(
                this.EmitUnionAttributes(type))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    this.EmitAbstractUnionTagPropertyDeclaration(type.TagName)));

    private ClassDeclarationSyntax EmitUnionCaseDeclaration(ClassType type, String baseTypeName) =>
        this.EmitClassDeclaration(type)
            .WithBaseList(
                BaseList(
                    SingletonSeparatedList<BaseTypeSyntax>(
                        SimpleBaseType(
                            IdentifierName(baseTypeName)))));

    private SyntaxList<AttributeListSyntax> EmitUnionAttributes(UnionType type) =>
        List<AttributeListSyntax>()
            .Add(AttributeList(SingletonSeparatedList(this.EmitUnionTagAttribute(type.TagName))))
            .AddRange(type.Cases.Select(x => AttributeList(SingletonSeparatedList(this.EmitUnionCaseAttribute(x)))));

    private AttributeSyntax EmitUnionTagAttribute(String tagName) =>
        Attribute(IdentifierNameFromType(typeof(UnionTagAttribute)))
            .WithArgumentList(
                AttributeArgumentList(
                    SingletonSeparatedList<AttributeArgumentSyntax>(
                        AttributeArgument(
                            NameOfExpression(
                                Argument(IdentifierName(tagName)))))));

    private AttributeSyntax EmitUnionCaseAttribute(ClassType type) =>
        Attribute(IdentifierNameFromType(typeof(UnionCaseAttribute)))
            .WithArgumentList(
                AttributeArgumentList(
                    SeparatedListWithCommas<AttributeArgumentSyntax>(
                        AttributeArgument(TypeOfExpression(IdentifierName(type.Name))),
                        AttributeArgument(
                            LiteralExpressionFromString(type.Properties
                                .Select(x => x.Type)
                                .OfType<LiteralTypeReference>()
                                .First().Name)))));

    private TypeParameterListSyntax EmitGenericParameters(String[] genericParameters) =>
        TypeParameterList(SeparatedListWithCommas(genericParameters.Select(this.EmitGenericParameter).ToArray()));

    private TypeParameterSyntax EmitGenericParameter(String genericParameter) =>
        TypeParameter(Identifier(genericParameter));

    private SyntaxList<MemberDeclarationSyntax> EmitObjectPropertyDeclarations(ObjectProperty[] properties, String? currentNamespace) =>
        List<MemberDeclarationSyntax>(properties.Select(x => this.EmitObjectPropertyDeclaration(x, currentNamespace)));

    private PropertyDeclarationSyntax EmitObjectPropertyDeclaration(ObjectProperty property, String? currentNamespace) =>
        (property.Type, property.Name) switch
        {
            var (reference, propertyName) when reference is LiteralTypeReference literal =>
                this.EmitConcreteUnionTagPropertyDeclaration(literal, propertyName),
            var (reference, propertyName) =>
                this.EmitObjectPropertyDeclaration(reference, propertyName, currentNamespace)
        };

    private PropertyDeclarationSyntax EmitAbstractUnionTagPropertyDeclaration(String propertyName) =>
        PropertyDeclaration(
                IdentifierNameFromType(typeof(String)),
                propertyName)
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.AbstractKeyword)))
            .WithAccessorList(
                AccessorList(
                    SingletonList<AccessorDeclarationSyntax>(
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Semicolon))));

    private PropertyDeclarationSyntax EmitConcreteUnionTagPropertyDeclaration(LiteralTypeReference reference, String propertyName) =>
        PropertyDeclaration(
                this.TypeReferenceEmitter.Emit(reference),
                Identifier(propertyName))
            .WithModifiers(
                TokenList(
                    Token(SyntaxKind.PublicKeyword),
                    Token(SyntaxKind.OverrideKeyword)))
            .WithAccessorList(
                AccessorList(
                    SingletonList<AccessorDeclarationSyntax>(
                        AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(Semicolon))))
            .WithInitializer(
                EqualsValueClause(
                    LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        Literal(reference.Name))))
            .WithSemicolonToken(Semicolon);

    private PropertyDeclarationSyntax EmitObjectPropertyDeclaration(TypeReference reference, String propertyName, String? currentNamespace)
    {
        return this.IsNotNullReferenceType(reference)
            ? EmitRoot()
                .WithInitializer(
                    EqualsValueClause(
                        PostfixUnaryExpression(
                            SyntaxKind.SuppressNullableWarningExpression,
                            LiteralExpression(
                                SyntaxKind.DefaultLiteralExpression,
                                Token(SyntaxKind.DefaultKeyword)))))
                .WithSemicolonToken(Semicolon)
            : EmitRoot();

        PropertyDeclarationSyntax EmitRoot() =>
            PropertyDeclaration(
                    this.TypeReferenceEmitter.Emit(reference, currentNamespace),
                    Identifier(propertyName))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(
                    AccessorList(
                        List<AccessorDeclarationSyntax>(
                            new[]
                            {
                                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(Semicolon),
                                AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(Semicolon)
                            })));
    }

    private Boolean IsNotNullReferenceType(TypeReference reference) => reference switch
    {
        ArrayTypeReference { ArrayLikeTypeId: null } => true,
        ArrayTypeReference { ArrayLikeTypeId: {} typeId } => this.TypeRegistry.GetType(typeId).IsReference,
        DictionaryTypeReference dictionary => this.TypeRegistry.GetType(dictionary.TypeId).IsReference,
        DistinctTypeReference distinct => this.TypeRegistry.GetType(distinct.TypeId).IsReference,
        DynamicTypeReference dynamic => this.TypeRegistry.GetType(dynamic.TypeId).IsReference,
        TypeParameterReference => true,
        LiteralTypeReference => true,
        NullableTypeReference => false,
        PrimitiveTypeReference primitive => this.TypeRegistry.GetType(primitive.TypeId).IsReference,
        TaskTypeReference task => task.ResultType is not null && this.IsNotNullReferenceType(task.ResultType),
        _ => throw new NotSupportedException()
    };
}
