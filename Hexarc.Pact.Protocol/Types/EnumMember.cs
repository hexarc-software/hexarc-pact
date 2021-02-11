using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Describes an enum member object.
    /// </summary>
    public sealed class EnumMember
    {
        /// <summary>
        /// Gets the enum member name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the enum member value.
        /// </summary>
        public Int32 Value { get; }

        /// <summary>
        /// Creates an instance of the EnumMember class.
        /// </summary>
        /// <param name="name">The enum member name.</param>
        /// <param name="value">The enum member value.</param>
        public EnumMember(String name, Int32 value) =>
            (this.Name, this.Value) = (name, value);
    }
}
