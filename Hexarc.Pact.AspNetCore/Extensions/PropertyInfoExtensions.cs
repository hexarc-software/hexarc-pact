namespace Hexarc.Pact.AspNetCore.Extensions;

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
    /// <param name="namingConvention">The naming convention for the property.</param>
    /// <returns>Returns true if the property is the union tag.</returns>
    /// <remarks>
    /// The union tag must be provided with the specified naming convention.
    /// The provided naming convention will be applied only to the property name.
    /// </remarks>
    public static Boolean IsUnionTag(this PropertyInfo propertyInfo, UnionTag tag, NamingConvention? namingConvention) =>
        propertyInfo.Name.ToConventionalString(namingConvention) == tag.Name;

    public static Boolean TryReadJsonPropertyName(this PropertyInfo propertyInfo, [NotNullWhen(true)] out String? name)
    {
        var attribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
        if (attribute is null)
        {
            name = default;
            return false;
        }
        else
        {
            name = attribute.Name;
            return true;
        }
    }

    public static String GetName(this PropertyInfo propertyInfo, NamingConvention? namingConvention)
    {
        if (propertyInfo.TryReadJsonPropertyName(out var name)) return name;
        else return propertyInfo.Name.ToConventionalString(namingConvention);
    }
}
