using System.Collections.Generic;
using System.Linq;
using Namotion.Reflection;

namespace Hexarc.Pact.AspNetCore.Extensions
{
    /// <summary>
    /// Extensions for the Namotion.Reflection.ContextualType class.
    /// </summary>
    public static class ContextualTypeExtensions
    {
        public static ContextualType[] GetTupleArguments(this ContextualType contextualType) =>
            contextualType.EnumerateFlattenTupleArguments().ToArray();

        private static IEnumerable<ContextualType> EnumerateFlattenTupleArguments(this ContextualType contextualType)
        {
            // We have to flat a tuple generic arguments
            // in the case of a tuple of eight elements.
            // If the eighth element is presented it contains a folded tuple
            // with the rest tuple generic arguments from the top level definition.

            var allArguments = contextualType.GenericArguments;
            var hasRest = allArguments.Length == 8;

            var regularArguments = hasRest ? allArguments[..7] : allArguments;
            foreach (var argument in regularArguments) yield return argument;

            if (!hasRest) yield break;

            var restArguments = allArguments[7].EnumerateFlattenTupleArguments();
            foreach (var argument in restArguments) yield return argument;
        }
    }
}
