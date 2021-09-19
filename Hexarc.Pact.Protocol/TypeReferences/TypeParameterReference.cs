namespace Hexarc.Pact.Protocol.TypeReferences;

/// <summary>
/// Describes a type parameter reference that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class TypeParameterReference : TypeReference
{
    /// <summary>
    /// Gets the <see cref="TypeParameterReference"/> kind.
    /// </summary>
    public override String Kind => TypeReferenceKind.TypeParameter;

    /// <summary>
    /// Gets the type parameter name.
    /// </summary>
    public String Name { get; }

    /// <summary>
    /// Creates an instance of the <see cref="TypeParameterReference"/> class.
    /// </summary>
    /// <param name="name">The name of the type parameter.</param>
    public TypeParameterReference(String name) =>
        this.Name = name;
}
