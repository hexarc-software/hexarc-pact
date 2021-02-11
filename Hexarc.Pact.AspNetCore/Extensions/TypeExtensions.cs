using System;
using System.Reflection;

namespace Hexarc.Pact.AspNetCore.Extensions
{
    /// <summary>
    /// Extensions for the System.Type class.
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Extracts the instance public properties from the type instance.
        /// </summary>
        /// <param name="type">The type to extract properties.</param>
        /// <returns>The result that contains extracted properties.</returns>
        public static PropertyInfo[] GetPublicInstanceProperties(this Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
