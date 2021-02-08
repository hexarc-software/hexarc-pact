using System;

namespace Hexarc.Rpc.Protocol.Types
{
    /// <summary>
    /// Describes a union type that can be provided by the Hexarc RPC protocol.
    /// </summary>
    public sealed class UnionType : DistinctType
    {
        public override String Kind { get; } = TypeKind.Union;

        /// <summary>
        /// Gets the union tag name.
        /// </summary>
        public String TagName { get; }

        /// <summary>
        /// Gets the union cases.
        /// </summary>
        public ClassType[] Cases { get; }

        /// <summary>
        /// Creates an instance of the UnionType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="tagName">The union tag name.</param>
        /// <param name="cases">The union cases.</param>
        public UnionType(Guid id, String? @namespace, String name, String tagName, ClassType[] cases) :
            base(id, @namespace, name) => (this.TagName, this.Cases) = (tagName, cases);
    }
}
