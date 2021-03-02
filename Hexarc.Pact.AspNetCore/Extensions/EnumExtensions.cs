using System;

namespace Hexarc.Pact.AspNetCore.Extensions
{
    internal static class EnumExtensions
    {
        public static T? Parse<T>(String? value) where T : struct, Enum
        {
            if (Enum.TryParse(typeof(T), value, out var convention)) return (T?) convention!;
            else return default;
        }
    }
}
