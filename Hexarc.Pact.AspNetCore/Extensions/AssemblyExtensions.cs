namespace Hexarc.Pact.AspNetCore.Extensions;

using Type = System.Type;

internal static class AssemblyExtensions
{
    public static IEnumerable<Type> GetPactScopedTypes(this Assembly assembly, HashSet<String> scopes) =>
        assembly.GetTypes()
            .Where(x => x.GetCustomAttributes<PactScopeAttribute>()
                .Any(a => scopes.Contains(a.Scope)));
}
