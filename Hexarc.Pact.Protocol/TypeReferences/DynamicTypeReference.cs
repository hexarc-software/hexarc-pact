namespace Hexarc.Pact.Protocol.TypeReferences;

/// <summary>
/// Describes a dynamic type reference that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class DynamicTypeReference : TypeReference
{
    /// <summary>
    /// Gets the <see cref="DynamicTypeReference"/> kind.
    /// </summary>
    public override String Kind => TypeReferenceKind.Dynamic;

    /// <summary>
    /// Gets the unique dynamic type id.
    /// </summary>
    public Guid TypeId { get; }

    /// <summary>
    /// Creates an instance of the <see cref="DynamicTypeReference"/> class.
    /// </summary>
    /// <param name="typeId">The unique dynamic type id.</param>
    public DynamicTypeReference(Guid typeId) =>
        this.TypeId = typeId;
}
