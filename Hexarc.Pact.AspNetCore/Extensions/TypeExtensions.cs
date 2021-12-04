using Type = System.Type;

namespace Hexarc.Pact.AspNetCore.Extensions;

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

    public static ContextualType ToContextualType(this NullabilityInfo nullabilityInfo) =>
        new(nullabilityInfo);
}
