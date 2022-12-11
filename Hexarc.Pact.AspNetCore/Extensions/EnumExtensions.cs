namespace Hexarc.Pact.AspNetCore.Extensions;

internal static class EnumExtensions
{
    public static T? Parse<T>(String? value) where T : struct, Enum
    {
        if (Enum.TryParse(typeof(T), value, out var result)) return (T?) result;
        else return default;
    }
}
