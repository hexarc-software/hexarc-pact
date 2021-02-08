using System;
using Hexarc.Annotations;

namespace Hexarc.Rpc.Protocol.TypeReferences
{
    /// <summary>
    /// Contains the common logic for all derived Hexarc RPC type references.
    /// </summary>
    [UnionTag(nameof(Kind))]
    [UnionCase(typeof(ArrayTypeReference), TypeReferenceKind.Array)]
    [UnionCase(typeof(DictionaryTypeReference), TypeReferenceKind.Dictionary)]
    [UnionCase(typeof(DistinctTypeReference), TypeReferenceKind.Distinct)]
    [UnionCase(typeof(GenericTypeReference), TypeReferenceKind.Generic)]
    [UnionCase(typeof(NullableTypeReference), TypeReferenceKind.Nullable)]
    [UnionCase(typeof(TaskTypeReference), TypeReferenceKind.Task)]
    [UnionCase(typeof(PrimitiveTypeReference), TypeReferenceKind.Primitive)]
    [UnionCase(typeof(LiteralTypeReference), TypeReferenceKind.Literal)]
    public abstract class TypeReference
    {
        /// <summary>
        /// Gets the discriminator to identify the type reference subclass across particular environments.
        /// </summary>
        public abstract String Kind { get; }
    }
}
