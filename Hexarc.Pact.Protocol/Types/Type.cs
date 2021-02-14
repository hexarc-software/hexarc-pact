using System;
using Hexarc.Annotations;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Contains the common logic for all derived Hexarc Pact types.
    /// </summary>
    [UnionTag(nameof(Kind))]
    [UnionCase(typeof(PrimitiveType), TypeKind.Primitive)]
    [UnionCase(typeof(DynamicType), TypeKind.Dynamic)]
    [UnionCase(typeof(ArrayLikeType), TypeKind.ArrayLike)]
    [UnionCase(typeof(DictionaryType), TypeKind.Dictionary)]
    [UnionCase(typeof(TaskType), TypeKind.Task)]
    [UnionCase(typeof(EnumType), TypeKind.Enum)]
    [UnionCase(typeof(StructType), TypeKind.Struct)]
    [UnionCase(typeof(ClassType), TypeKind.Class)]
    [UnionCase(typeof(UnionType), TypeKind.Union)]
    public abstract class Type
    {
        /// <summary>
        /// Gets the discriminator to identify the type subclass across particular environments.
        /// </summary>
        public abstract String Kind { get; }

        /// <summary>
        /// Gets the unique type id.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the namespace where the type is defined in.
        /// </summary>
        public String? Namespace { get; }

        /// <summary>
        /// Gets the type name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Creates an instance of the Type class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        protected Type(Guid id, String? @namespace, String name) =>
            (this.Id, this.Namespace, this.Name) = (id, @namespace, name);
    }
}
