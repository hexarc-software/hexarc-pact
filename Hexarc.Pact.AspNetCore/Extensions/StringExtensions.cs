using Hexarc.Pact.AspNetCore.Models;

namespace Hexarc.Pact.AspNetCore.Extensions;

internal static class StringExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static String ToConventionalString(this String value, NamingConvention? convention) => convention switch
    {
        NamingConvention.CamelCase => JsonNamingPolicy.CamelCase.ConvertName(value),
        _ => value
    };
}
