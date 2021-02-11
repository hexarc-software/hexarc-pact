using System;

namespace Hexarc.Pact.Protocol.Types
{
    /// <summary>
    /// Contains the Hexarc Pact type kinds that are used as discriminator values.
    /// </summary>
    public static class TypeKind
    {
        /// <summary>
        /// The primitive type literal.
        /// </summary>
        public const String Primitive = nameof(Primitive);

        /// <summary>
        /// The array-like type literal.
        /// </summary>
        public const String ArrayLike = nameof(ArrayLike);

        /// <summary>
        /// The dictionary type literal.
        /// </summary>
        public const String Dictionary = nameof(Dictionary);

        /// <summary>
        /// The task type literal.
        /// </summary>
        public const String Task = nameof(Task);

        /// <summary>
        /// The enum type literal.
        /// </summary>
        public const String Enum = nameof(Enum);

        /// <summary>
        /// The structure type literal.
        /// </summary>
        public const String Struct = nameof(Struct);

        /// <summary>
        /// The class type literal.
        /// </summary>
        public const String Class = nameof(Class);

        /// <summary>
        /// The union type literal.
        /// </summary>
        public const String Union = nameof(Union);
    }
}
