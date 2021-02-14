using System;

namespace Hexarc.Pact.Protocol.Internals
{
    /// <summary>
    /// Extensions for the System.Type class.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Extracts the type name without generic arity if presented.
        /// </summary>
        /// <param name="type">The type to extract the name.</param>
        /// <returns>The type name without generic arity.</returns>
        public static String NameWithoutGenericArity(this Type type)
        {
            var name = type.Name;
            var index = name.IndexOf("`", StringComparison.Ordinal);
            return index == -1 ? name : name.Substring(0, index);
        }
    }
}
