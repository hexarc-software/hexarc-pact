using System;
using System.Text.Json.Serialization;
using Hexarc.Annotations;
using Hexarc.Pact.Protocol.Extensions;

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
        /// Gets the type reference semantic marker.
        /// </summary>
        public Boolean IsReference { get; }

        /// <summary>
        /// Gets the full type name.
        /// </summary>
        [JsonIgnore]
        public String FullName => String.IsNullOrEmpty(this.Namespace) ? this.Name : $"{this.Namespace}.{this.Name}";

        /// <summary>
        /// Creates an instance of the Type class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="isReference">The type reference semantic marker.</param>
        protected Type(Guid id, String? @namespace, String name, Boolean isReference)
        {
            this.Id = id;
            this.Namespace = @namespace;
            this.Name = name;
            this.IsReference = isReference;
        }

        /// <summary>
        /// Creates an instance of the Type class.
        /// </summary>
        /// <param name="type">The System.Type object to build the Hexarc Type data.</param>
        protected Type(System.Type type) :
            this(type.GUID, type.Namespace, type.NameWithoutGenericArity(), type.IsReference()) { }
    }
}
