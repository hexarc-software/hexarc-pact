namespace Hexarc.Pact.Tool.Internals;

public sealed class NameCollisionResolver
{
    private HashSet<String> ReservedNames { get; }

    private String EscapeSuffix { get; }

    public NameCollisionResolver(IEnumerable<String> reservedNames, String escapeSuffix) =>
        (this.ReservedNames, this.EscapeSuffix) = (new HashSet<String>(reservedNames), escapeSuffix);

    public NameCollisionResolver(Type type, String escapeSuffix) :
        this(type
            .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(x => x.Name), escapeSuffix) { }

    public String Resolve(String name) =>
        this.ReservedNames.Contains(name)
            ? this.Resolve($"{name}{this.EscapeSuffix}")
            : name;
}
