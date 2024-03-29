namespace Hexarc.Pact.AspNetCore.Readers;

/// <summary>
/// This class provides an ability to read distinct types of the Hexarc Pact protocol.
/// </summary>
public sealed class DistinctTypeReader
{
    private TypeChecker TypeChecker { get; }

    private TypeReferenceReader TypeReferenceReader { get; }

    private NullabilityInfoContext NullabilityInfoContext { get; }

    /// <summary>
    /// Creates an instance of the DistinctTypeReader class.
    /// </summary>
    /// <param name="typeChecker">The type checker to determine what a target Hexarc Pact type for a given one.</param>
    /// <param name="typeReferenceReader">The type reference reader used during type reading.</param>
    /// <param name="nullabilityInfoContext">The nullability info context.</param>
    public DistinctTypeReader(
        TypeChecker typeChecker,
        TypeReferenceReader typeReferenceReader,
        NullabilityInfoContext nullabilityInfoContext)
    {
        this.TypeChecker = typeChecker;
        this.TypeReferenceReader = typeReferenceReader;
        this.NullabilityInfoContext = nullabilityInfoContext;
    }

    /// <summary>
    /// Reads a Hexarc Pact distinct type from a .NET system type.
    /// </summary>
    /// <param name="type">The .NET system type to read from.</param>
    /// <param name="namingConvention">The naming convention to apply to the type.</param>
    /// <returns>The Hexarc Pact distinct type read from the given .NET system type.</returns>
    public DistinctType Read(System.Type type, NamingConvention? namingConvention) => type switch
    {
        _ when this.TypeChecker.IsStringEnumType(type) => this.ReadStringEnumType(type),
        _ when this.TypeChecker.IsEnumType(type) => this.ReadEnumType(type),
        _ when this.TypeChecker.IsUnionType(type) => this.ReadUnionType(type, namingConvention),
        _ when this.TypeChecker.IsStructType(type) => this.ReadStructType(type, namingConvention),
        _ when this.TypeChecker.IsClassType(type) => this.ReadClassType(type, namingConvention),
        _ => throw new InvalidOperationException($"Could not read a Hexarc Pact type from {type}")
    };

    private StringEnumType ReadStringEnumType(System.Type type) =>
        new(type.GUID, type.Namespace, type.Name, Enum.GetNames(type));

    private EnumType ReadEnumType(System.Type type) =>
        new(type.GUID, type.Namespace, type.Name, this.ReadEnumMembers(type, Enum.GetNames(type)));

    private EnumMember[] ReadEnumMembers(System.Type type, String[] memberNames) =>
        memberNames.Select(x => this.ReadEnumMember(type, x)).ToArray();

    private EnumMember ReadEnumMember(System.Type type, String memberName) =>
        new(memberName, (Int32) Enum.Parse(type, memberName));

    private UnionType ReadUnionType(System.Type type, NamingConvention? namingConvention)
    {
        var tagName = type.GetCustomAttribute<UnionTagAttribute>()!
            .TagPropertyName
            .ToConventionalString(namingConvention);
        var cases = type.GetCustomAttributes<UnionCaseAttribute>()
            .Select(x => this.ReadUnionCase(x, tagName, namingConvention))
            .ToArray();
        return new UnionType(type.GUID, type.Namespace, type.Name, tagName, cases);
    }

    private StructType ReadStructType(System.Type type, NamingConvention? namingConvention) =>
        new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
            this.ReadGenericParameters(type.GetGenericArguments()),
            this.ReadObjectProperties(type.GetPublicInstanceProperties(), namingConvention));

    private ClassType ReadClassType(System.Type type, NamingConvention? namingConvention) =>
        new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
            this.ReadGenericParameters(type.GetGenericArguments()),
            this.ReadObjectProperties(type.GetPublicInstanceProperties(), namingConvention));

    private ClassType ReadClassType(System.Type type, UnionTag tag, NamingConvention? namingConvention) =>
        new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
            this.ReadGenericParameters(type.GetGenericArguments()),
            this.ReadObjectProperties(type.GetPublicInstanceProperties(), tag, namingConvention));

    private ClassType ReadUnionCase(UnionCaseAttribute @case, String tagName, NamingConvention? namingConvention) =>
        this.ReadClassType(@case.CaseType,
            new UnionTag(tagName.ToConventionalString(namingConvention), @case.TagPropertyValue),
            namingConvention);

    private String[]? ReadGenericParameters(System.Type[] genericParameters) =>
        genericParameters.Length != 0
            ? genericParameters.Any(x => !x.IsGenericParameter)
                ? throw new InvalidOperationException("Each object generic definition must be open")
                : genericParameters.Select(x => x.Name).ToArray()
            : default;

    private ObjectProperty[] ReadObjectProperties(PropertyInfo[] properties, UnionTag tag, NamingConvention? namingConvention) =>
        properties.Where(x => x.GetCustomAttribute<JsonIgnoreAttribute>() is null)
            .Select(x => this.ReadObjectProperty(x, tag, namingConvention)).ToArray();

    private ObjectProperty[] ReadObjectProperties(PropertyInfo[] properties, NamingConvention? namingConvention) =>
        properties.Where(x => x.GetCustomAttribute<JsonIgnoreAttribute>() is null)
            .Select(x => this.ReadObjectProperty(x, namingConvention)).ToArray();

    private ObjectProperty ReadObjectProperty(PropertyInfo property, UnionTag tag, NamingConvention? namingConvention) =>
        property.IsUnionTag(tag, namingConvention)
            ? this.ReadUnionTagProperty(tag, namingConvention)
            : this.ReadObjectProperty(property, namingConvention);

    private ObjectProperty ReadObjectProperty(PropertyInfo property, NamingConvention? namingConvention) =>
        new(this.ReadObjectPropertyType(property, namingConvention), property.GetName(namingConvention));

    private ObjectProperty ReadUnionTagProperty(UnionTag tag, NamingConvention? namingConvention) =>
        new(new LiteralTypeReference(tag.Value), tag.Name.ToConventionalString(namingConvention));

    private TypeReference ReadObjectPropertyType(PropertyInfo property, NamingConvention? namingConvention) =>
        this.TypeReferenceReader.Read(new ContextualType(this.NullabilityInfoContext.Create(property), property), namingConvention);
}
