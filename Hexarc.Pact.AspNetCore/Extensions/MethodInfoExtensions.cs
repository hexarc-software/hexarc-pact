namespace Hexarc.Pact.AspNetCore.Extensions;

internal static class MethodInfoExtensions
{
    public static Boolean IsReturnAttributeDefined<T>(this MethodInfo methodInfo) where T : Attribute =>
        methodInfo.ReturnTypeCustomAttributes.IsDefined(typeof(T), false);
}
