namespace Hexarc.Pact.Protocol.TypeReferences;

/// <summary>
/// Contains the Hexarc Pact type reference kinds that are used as discriminator values.
/// </summary>
public static class TypeReferenceKind
{
    /// <summary>
    /// The nullable type reference literal.
    /// </summary>
    public const String Nullable = nameof(Nullable);

    /// <summary>
    /// The task type reference literal.
    /// </summary>
    public const String Task = nameof(Task);

    /// <summary>
    /// The type parameter reference literal.
    /// </summary>
    public const String TypeParameter = nameof(TypeParameter);

    /// <summary>
    /// The primitive type reference literal.
    /// </summary>
    public const String Primitive = nameof(Primitive);

    /// <summary>
    /// The dynamic type reference literal.
    /// </summary>
    public const String Dynamic = nameof(Dynamic);

    /// <summary>
    /// The array type reference literal.
    /// </summary>
    public const String Array = nameof(Array);

    /// <summary>
    /// The dictionary type reference literal.
    /// </summary>
    public const String Dictionary = nameof(Dictionary);

    /// <summary>
    /// The distinct type reference literal.
    /// </summary>
    public const String Distinct = nameof(Distinct);

    /// <summary>
    /// The literal type reference literal.
    /// </summary>
    public const String Literal = nameof(Literal);

    /// <summary>
    /// The literal type reference literal.
    /// </summary>
    public const String Tuple = nameof(Tuple);
}
