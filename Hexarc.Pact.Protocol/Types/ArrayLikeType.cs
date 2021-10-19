namespace Hexarc.Pact.Protocol.Types;

/// <summary>
/// Describes an array-like type that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class ArrayLikeType : Type
{
    /// <summary>
    /// Gets the <see cref="ArrayLikeType"/> kind.
    /// </summary>
    public override String Kind => TypeKind.ArrayLike;

    /// <summary>
    /// Creates an instance of the <see cref="ArrayLikeType"/> class.
    /// </summary>
    /// <param name="id">The unique type id.</param>
    /// <param name="namespace">The type namespace.</param>
    /// <param name="name">The type name.</param>
    /// <param name="isReference">The type reference semantic marker.</param>
    [JsonConstructor]
    public ArrayLikeType(Guid id, String? @namespace, String name, Boolean isReference) :
        base(id, @namespace, name, isReference) { }

    /// <summary>
    /// Creates an instance of the <see cref="ArrayLikeType"/> class.
    /// </summary>
    /// <param name="type">The native .NET type to create the class instance from.</param>
    public ArrayLikeType(System.Type type) :
        this(type.GUID, type.Namespace, type.NameWithoutGenericArity(), type.IsReference()) { }
}
