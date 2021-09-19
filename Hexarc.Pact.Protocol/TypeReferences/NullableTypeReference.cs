namespace Hexarc.Pact.Protocol.TypeReferences;

/// <summary>
/// Describes a nullable type reference that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class NullableTypeReference : TypeReference
{
    /// <summary>
    /// Gets the <see cref="NullableTypeReference"/> kind.
    /// </summary>
    public override String Kind => TypeReferenceKind.Nullable;

    /// <summary>
    /// Gets the unwrapped underlying type.
    /// </summary>
    public TypeReference UnderlyingType { get; }

    /// <summary>
    /// Creates an instance of the <see cref="NullableTypeReference"/> class.
    /// </summary>
    /// <param name="underlyingType">The unwrapped underlying type of the given nullable type.</param>
    public NullableTypeReference(TypeReference underlyingType) =>
        this.UnderlyingType = underlyingType;
}
