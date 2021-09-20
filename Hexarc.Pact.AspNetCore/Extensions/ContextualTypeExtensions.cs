namespace Hexarc.Pact.AspNetCore.Extensions;

using Namotion.Reflection;

/// <summary>
/// Extensions for the Namotion.Reflection.ContextualType class.
/// </summary>
public static class ContextualTypeExtensions
{
    /// <summary>
    /// Extracts the tuple element names if provided.
    /// </summary>
    /// <param name="contextualType">The contextual type for a ValueTuple type.</param>
    /// <returns>Returns the tuple element names or null.</returns>
    public static IList<String?>? GetTupleElementNames(this ContextualType contextualType) =>
        contextualType.GetContextAttribute<TupleElementNamesAttribute>()?.TransformNames;

    /// <summary>
    /// Extracts the generic arguments from the tuple contextual type.
    /// </summary>
    /// <param name="contextualType">The contextual type for a ValueTuple type.</param>
    /// <returns>Returns the generic arguments from the tuple contextual type.</returns>
    public static ContextualType[] GetTupleArguments(this ContextualType contextualType) =>
        contextualType.EnumerateFlattenTupleArguments().ToArray();

    private static IEnumerable<ContextualType> EnumerateFlattenTupleArguments(this ContextualType contextualType)
    {
        // We have to flat a tuple generic arguments in the case of a tuple with eight elements.
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
