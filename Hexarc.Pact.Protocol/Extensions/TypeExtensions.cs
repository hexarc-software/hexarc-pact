using System;

namespace Hexarc.Pact.Protocol.Extensions
{
    /// <summary>
    /// Extensions for the System.Type class.
    /// </summary>
    public static class TypeExtensions
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

        public static String FullNameWithoutGenericArity(this Type type)
        {
            var name = type.FullName ?? throw new InvalidOperationException();
            var index = name.IndexOf("`", StringComparison.Ordinal);
            return index == -1 ? name : name.Substring(0, index);
        }

        /// <summary>
        /// Checks the type has the reference semantic or not.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Returns true if the type has the reference semantic either false.</returns>
        public static Boolean IsReference(this Type type) =>
            type.IsClass || type.IsInterface;
    }
}
