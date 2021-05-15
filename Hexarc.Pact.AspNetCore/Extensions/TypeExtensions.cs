using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Namotion.Reflection;

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
        public static ContextualPropertyInfo[] GetPublicInstanceProperties(this Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x => x.ToContextualProperty())
                .ToArray();

        /// <summary>
        /// Checks if the type supports converting to a json string enum.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Returns true if the type supports converting to a json string enum either false.</returns>
        public static Boolean SupportJsonStringEnumConversion(this Type type) =>
            type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType == typeof(JsonStringEnumConverter);

        public static ContextualType[] GetTupleArguments(this ContextualType contextualType) =>
            contextualType.EnumerateFlattenTupleArguments().ToArray();

        private static IEnumerable<ContextualType> EnumerateFlattenTupleArguments(this ContextualType contextualType)
        {
            var allArguments = contextualType.GenericArguments;
            var hasTail = allArguments.Length == 8;

            var regularArguments = hasTail ? allArguments[..7] : allArguments;
            foreach (var argument in regularArguments) yield return argument;

            if (!hasTail) yield break;

            var restArguments = allArguments[7].EnumerateFlattenTupleArguments();
            foreach (var argument in restArguments) yield return argument;
        }
    }
}
