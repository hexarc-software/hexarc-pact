using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes an enum type that can be provided by the Hexarc Pact protocol.
    /// </summary>
    public sealed class EnumType : DistinctType
    {
        /// <summary>
        /// Gets the EnumType kind.
        /// </summary>
        public override String Kind { get; } = TypeKind.Enum;

        /// <summary>
        /// Gets the enum members.
        /// </summary>
        public EnumMember[] Members { get; }

        /// <summary>
        /// Creates an instance of the EnumType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        /// <param name="members">The enum members.</param>
        public EnumType(Guid id, String? @namespace, String name, EnumMember[] members) :
            base(id, @namespace, name, false) => this.Members = members;
    }
}
