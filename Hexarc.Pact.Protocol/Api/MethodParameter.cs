using System;
using Hexarc.Pact.Protocol.TypeReferences;

namespace Hexarc.Pact.Protocol.Api
{
    /// <summary>
    /// Provides information about an API endpoint method parameter.
    /// </summary>
    public sealed class MethodParameter
    {
        /// <summary>
        /// Gets the method parameter type.
        /// </summary>
        public TypeReference Type { get; }

        /// <summary>
        /// Gets the method parameter name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Creates an instance of the MethodParameter class.
        /// </summary>
        /// <param name="type">The method parameter type.</param>
        /// <param name="name">The method parameter name.</param>
        public MethodParameter(TypeReference type, String name) =>
            (this.Type, this.Name) = (type, name);
    }
}
