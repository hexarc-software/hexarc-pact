namespace Hexarc.Pact.Protocol.Types;

/// <summary>
/// Contains the common logic for all distinct types.
/// </summary>
public abstract class DistinctType : Type
{
    /// <summary>
    /// Creates an instance of the <see cref="DistinctType"/> class.
    /// </summary>
    /// <param name="id">The unique type id.</param>
    /// <param name="namespace">The type namespace.</param>
    /// <param name="name">The type name.</param>
    /// <param name="isReference">The type reference semantic marker.</param>
    protected DistinctType(Guid id, String? @namespace, String name, Boolean isReference) :
        base(id, @namespace, name, isReference) { }
}
