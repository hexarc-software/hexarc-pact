namespace Hexarc.Pact.Protocol.Types;

/// <summary>
/// Describes a string enum type that can be provided by the Hexarc Pact protocol.
/// </summary>
public sealed class StringEnumType : DistinctType
{
    /// <summary>
    /// Gets the <see cref="StringEnumType"/> kind.
    /// </summary>
    public override String Kind => TypeKind.StringEnum;

    /// <summary>
    /// Gets the string enum members.
    /// </summary>
    public String[] Members { get; }

    /// <summary>
    /// Creates an instance of the <see cref="StringEnumType"/> class.
    /// </summary>
    /// <param name="id">The unique type id.</param>
    /// <param name="namespace">The type namespace.</param>
    /// <param name="name">The type name.</param>
    /// <param name="members">The string enum members.</param>
    public StringEnumType(Guid id, String? @namespace, String name, String[] members) :
        base(id, @namespace, name, false) => this.Members = members;
}
