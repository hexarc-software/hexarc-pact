namespace Hexarc.Pact.AspNetCore.Extensions;

using Hexarc.Pact.AspNetCore.Models;

internal static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static String ToConventionalString(this String value, NamingConvention? convention) => convention switch
    {
        NamingConvention.CamelCase => JsonNamingPolicy.CamelCase.ConvertName(value),
        _ => value
    };
}
