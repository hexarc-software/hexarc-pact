using System;

namespace Hexarc.Rpc.Protocol.Types
{
    /// <summary>
    /// Describes a dictionary type that can be provided by the Hexarc RPC protocol.
    /// </summary>
    public sealed class DictionaryType : Type
    {
        public override String Kind { get; } = TypeKind.ArrayLike;

        /// <summary>
        /// Creates an instance of the DictionaryType class.
        /// </summary>
        /// <param name="id">The unique type id.</param>
        /// <param name="namespace">The type namespace.</param>
        /// <param name="name">The type name.</param>
        public DictionaryType(Guid id, String? @namespace, String name) :
            base(id, @namespace, name) { }

        /// <summary>
        /// Creates an instance of the DictionaryType class.
        /// </summary>
        /// <param name="type">The native .NET type to create the class instance from.</param>
        public DictionaryType(System.Type type) :
            this(type.GUID, type.Namespace, type.Name) { }
    }
}
