namespace Hexarc.Pact.Protocol.Types;

/// <summary>
/// Describes a union type that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class UnionType : DistinctType
{
    /// <summary>
    /// Gets the <see cref="UnionType"/> kind.
    /// </summary>
    public override String Kind => TypeKind.Union;

    /// <summary>
    /// Gets the union tag name.
    /// </summary>
    public String TagName { get; }

    /// <summary>
    /// Gets the union cases.
    /// </summary>
    public ClassType[] Cases { get; }

    /// <summary>
    /// Creates an instance of the <see cref="UnionType"/> class.
    /// </summary>
    /// <param name="id">The unique type id.</param>
    /// <param name="namespace">The type namespace.</param>
    /// <param name="name">The type name.</param>
    /// <param name="tagName">The union tag name.</param>
    /// <param name="cases">The union cases.</param>
    public UnionType(Guid id, String? @namespace, String name, String tagName, ClassType[] cases) :
        base(id, @namespace, name, true) => (this.TagName, this.Cases) = (tagName, cases);
}
