namespace Hexarc.Pact.AspNetCore.Internals;

using Type = System.Type;

public sealed class ContextualType
{
    public Type Type { get; }

    public Boolean IsNullable { get; }

    public ContextualType[] GenericArguments { get; }

    public ContextualType? ElementType { get; }

    private ICustomAttributeProvider? CustomAttributeProvider { get; }

    public ContextualType(NullabilityInfo nullabilityInfo, ICustomAttributeProvider? customAttributeProvider = default)
    {
        var underlyingType = Nullable.GetUnderlyingType(nullabilityInfo.Type);
        this.Type = underlyingType ?? nullabilityInfo.Type;
        this.ElementType = nullabilityInfo.ElementType is null ? default : new ContextualType(nullabilityInfo.ElementType);
        this.IsNullable = nullabilityInfo.ReadState is NullabilityState.Nullable;
        this.CustomAttributeProvider = customAttributeProvider;
        this.GenericArguments = nullabilityInfo.GenericTypeArguments
            .Select(x => new ContextualType(x))
            .ToArray();
    }

    /// <summary>
    /// Extracts the tuple element names if provided.
    /// </summary>
    /// <returns>Returns the tuple element names or null.</returns>
    public IList<String?>? GetTupleElementNames() =>
        this.GetCustomAttribute<TupleElementNamesAttribute>()?.TransformNames;

    private T? GetCustomAttribute<T>(Boolean inherit = false) =>
        this.CustomAttributeProvider is not null
            ? this.CustomAttributeProvider.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault()
            : default;

    /// <summary>
    /// Extracts the generic arguments from the tuple type.
    /// </summary>
    /// <returns>Returns the generic arguments from the tuple type.</returns>
    public ContextualType[] GetTupleArguments() =>
        this.EnumerateFlattenTupleArguments().ToArray();

    private IEnumerable<ContextualType> EnumerateFlattenTupleArguments()
    {
        // We have to flat a tuple generic arguments in the case of a tuple with eight elements.
        // If the eighth element is presented it contains a folded tuple
        // with the rest tuple generic arguments from the top level definition.

        var allArguments = this.GenericArguments;
        var hasRest = allArguments.Length == 8;

        var regularArguments = hasRest ? allArguments[..7] : allArguments;
        foreach (var argument in regularArguments) yield return argument;

        if (!hasRest) yield break;

        var restArguments = allArguments[7].EnumerateFlattenTupleArguments();
        foreach (var argument in restArguments) yield return argument;
    }
}
