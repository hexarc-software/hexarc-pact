using System;

namespace Hexarc.Rpc.Server.Models
{
    /// <summary>
    /// The type describes a union tag used in a discriminated union case.
    /// </summary>
    internal sealed class UnionTag
    {
        /// <summary>
        /// Gets the union tag property name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets the union tag property value.
        /// </summary>
        public String Value { get; }

        /// <summary>
        /// Creates an instance of the UnionTag class.
        /// </summary>
        /// <param name="name">The union tag name.</param>
        /// <param name="value">The union tag value.</param>
        public UnionTag(String name, String value) =>
            (this.Name, this.Value) = (name, value);
    }
}
