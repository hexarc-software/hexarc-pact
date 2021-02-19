using System;

namespace Hexarc.Pact.Tool.Extensions
{
    public static class StringExtensions
    {
        public static String StripSuffix(this String value, String suffix) =>
            value.EndsWith(suffix) && value.Length > suffix.Length
                ? value.Substring(0, value.Length - suffix.Length)
                : value;
    }
}
