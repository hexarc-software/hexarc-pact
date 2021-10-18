namespace Hexarc.Pact.AspNetCore.Models;

using Type = System.Type;

public sealed class ContextualType
{
    public Type Type { get; }

    public NullabilityInfo NullabilityInfo { get; }

    public ICustomAttributeProvider? CustomAttributeProvider { get; }

    public Boolean IsNullable => this.NullabilityInfo.ReadState == NullabilityState.Nullable;

    public NullabilityInfo[] GenericArguments => this.NullabilityInfo.GenericTypeArguments;

    public ContextualType(Type type, NullabilityInfo nullabilityInfo, ICustomAttributeProvider? customAttributeProvider) =>
        (this.Type, this.NullabilityInfo, this.CustomAttributeProvider) = (type, nullabilityInfo, customAttributeProvider);

    public ContextualType(NullabilityInfo nullabilityInfo) : this(nullabilityInfo.Type, nullabilityInfo, default) { }

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
        foreach (var argument in regularArguments) yield return argument.ToContextualType();

        if (!hasRest) yield break;

        var restArguments = allArguments[7].ToContextualType().EnumerateFlattenTupleArguments();
        foreach (var argument in restArguments) yield return argument;
    }
}
