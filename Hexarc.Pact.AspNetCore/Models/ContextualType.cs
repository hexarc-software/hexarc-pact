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

    public T? GetCustomAttribute<T>(Boolean inherit = false) =>
        this.CustomAttributeProvider is not null
            ? this.CustomAttributeProvider.GetCustomAttributes(inherit).OfType<T>().FirstOrDefault()
            : default;
}
