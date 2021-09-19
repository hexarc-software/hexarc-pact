namespace Hexarc.Pact.Protocol.Types;

/// <summary>
/// Contains the common logic for all object-like types.
/// </summary>
public abstract class ObjectType : DistinctType
{
    /// <summary>
    /// Gets the type parameters.
    /// </summary>
    public String[]? TypeParameters { get; }

    /// <summary>
    /// Gets the type properties.
    /// </summary>
    public ObjectProperty[] Properties { get; }

    /// <summary>
    /// Creates an instance of the <see cref="ObjectType"/> class.
    /// </summary>
    /// <param name="id">The unique type id.</param>
    /// <param name="namespace">The type namespace.</param>
    /// <param name="name">The type name.</param>
    /// <param name="isReference">The type reference semantic marker.</param>
    /// <param name="typeParameters">The type parameters.</param>
    /// <param name="properties">The type own properties.</param>
    protected ObjectType(
        Guid id, String? @namespace, String name, Boolean isReference,
        String[]? typeParameters, ObjectProperty[] properties
    ) : base(id, @namespace, name, isReference) =>
        (this.TypeParameters, this.Properties) = (typeParameters, properties);
}
