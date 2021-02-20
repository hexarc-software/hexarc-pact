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
    }
}
