using System;
using System.Reflection;
using Hexarc.Rpc.Server.Models;

namespace Hexarc.Rpc.Server.Extensions
{
    /// <summary>
    /// Extensions for the PropertyInfo class.
    /// </summary>
    internal static class PropertyInfoExtensions
    {
        /// <summary>
        /// Checks if a given property info is a union tag.
        /// </summary>
        /// <param name="propertyInfo">The property to check.</param>
        /// <param name="tag">The union tag object to check against.</param>
        /// <returns>Returns true if the property is the union tag.</returns>
        public static Boolean IsUnionTag(this PropertyInfo propertyInfo, UnionTag tag) =>
            propertyInfo.Name == tag.Name;
    }
}
