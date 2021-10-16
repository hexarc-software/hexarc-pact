namespace Hexarc.Pact.AspNetCore.Extensions;

using Type = System.Type;

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
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToArray();

    /// <summary>
    /// Checks if the type supports converting to a json string enum.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>Returns true if the type supports converting to a json string enum either false.</returns>
    public static Boolean SupportJsonStringEnumConversion(this Type type) =>
        type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType == typeof(JsonStringEnumConverter);

    /// <summary>
    /// Extracts the tuple element names if provided.
    /// </summary>
    /// <param name="type">The ValueTuple type.</param>
    /// <returns>Returns the tuple element names or null.</returns>
    public static IList<String?>? GetTupleElementNames(this ContextualType type) =>
        type.GetCustomAttribute<TupleElementNamesAttribute>()?.TransformNames;

    public static ContextualType ToContextualType(this NullabilityInfo nullabilityInfo) =>
        new(nullabilityInfo);

    /// <summary>
    /// Extracts the generic arguments from the tuple type.
    /// </summary>
    /// <param name="type">The ValueTuple type.</param>
    /// <returns>Returns the generic arguments from the tuple type.</returns>
    public static ContextualType[] GetTupleArguments(this ContextualType type) =>
        type.EnumerateFlattenTupleArguments().ToArray();

    private static IEnumerable<ContextualType> EnumerateFlattenTupleArguments(this ContextualType type)
    {
        // We have to flat a tuple generic arguments in the case of a tuple with eight elements.
        // If the eighth element is presented it contains a folded tuple
        // with the rest tuple generic arguments from the top level definition.

        var allArguments = type.GenericArguments;
        var hasRest = allArguments.Length == 8;

        var regularArguments = hasRest ? allArguments[..7] : allArguments;
        foreach (var argument in regularArguments) yield return argument.ToContextualType();

        if (!hasRest) yield break;

        var restArguments = allArguments[7].ToContextualType().EnumerateFlattenTupleArguments();
        foreach (var argument in restArguments) yield return argument;
    }
}
