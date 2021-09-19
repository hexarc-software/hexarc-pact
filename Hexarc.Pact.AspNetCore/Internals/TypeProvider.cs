using Hexarc.Pact.Protocol.Types;

namespace Hexarc.Pact.AspNetCore.Internals;

public sealed class TypeProvider
{
    public IReadOnlyDictionary<Guid, PrimitiveType> PrimitiveTypes { get; }

    public IReadOnlyDictionary<Guid, ArrayLikeType> ArrayLikeTypes { get; }

    public IReadOnlyDictionary<Guid, DictionaryType> DictionaryTypes { get; }

    public IReadOnlyDictionary<Guid, DynamicType> DynamicTypes { get; }

    public IReadOnlyDictionary<Guid, TaskType> TaskTypes { get; }

    public IReadOnlyDictionary<Guid, System.Type> TupleTypes { get; }

    public TypeProvider(
        IReadOnlyDictionary<Guid, PrimitiveType> primitiveTypes,
        IReadOnlyDictionary<Guid, ArrayLikeType> arrayLikeTypes,
        IReadOnlyDictionary<Guid, DictionaryType> dictionaryTypes,
        IReadOnlyDictionary<Guid, DynamicType> dynamicTypes,
        IReadOnlyDictionary<Guid, TaskType> taskTypes,
        IReadOnlyDictionary<Guid, System.Type> tupleTypes)
    {
        this.PrimitiveTypes = primitiveTypes;
        this.ArrayLikeTypes = arrayLikeTypes;
        this.DictionaryTypes = dictionaryTypes;
        this.DynamicTypes = dynamicTypes;
        this.TaskTypes = taskTypes;
        this.TupleTypes = tupleTypes;
    }

    public TypeProvider() : this(
        KnownTypes.GetPrimitiveTypes().ToDictionary(x => x.Id),
        KnownTypes.GetArrayLikeTypes().ToDictionary(x => x.Id),
        KnownTypes.GetDictionaryTypes().ToDictionary(x => x.Id),
        KnownTypes.GetDynamicTypes().ToDictionary(x => x.Id),
        KnownTypes.GetTaskTypes().ToDictionary(x => x.Id),
        KnownTypes.GetTupleTypes().ToDictionary(x => x.GUID))
    { }
}
