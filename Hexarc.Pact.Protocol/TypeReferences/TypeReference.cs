using Hexarc.Annotations;

namespace Hexarc.Pact.Protocol.TypeReferences;

/// <summary>
/// Contains the common logic for all derived Hexarc Pact type references.
/// </summary>
[UnionTag(nameof(Kind))]
[UnionCase(typeof(ArrayTypeReference), TypeReferenceKind.Array)]
[UnionCase(typeof(DictionaryTypeReference), TypeReferenceKind.Dictionary)]
[UnionCase(typeof(DistinctTypeReference), TypeReferenceKind.Distinct)]
[UnionCase(typeof(TypeParameterReference), TypeReferenceKind.TypeParameter)]
[UnionCase(typeof(NullableTypeReference), TypeReferenceKind.Nullable)]
[UnionCase(typeof(TaskTypeReference), TypeReferenceKind.Task)]
[UnionCase(typeof(PrimitiveTypeReference), TypeReferenceKind.Primitive)]
[UnionCase(typeof(DynamicTypeReference), TypeReferenceKind.Dynamic)]
[UnionCase(typeof(LiteralTypeReference), TypeReferenceKind.Literal)]
[UnionCase(typeof(TupleTypeReference), TypeReferenceKind.Tuple)]
public abstract class TypeReference
{
    /// <summary>
    /// Gets the discriminator to identify the type reference subclass across particular environments.
    /// </summary>
    public abstract String Kind { get; }
}
