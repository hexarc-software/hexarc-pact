using Hexarc.Pact.Protocol.Types;
using Type = Hexarc.Pact.Protocol.Types.Type;

namespace Hexarc.Pact.Tool.Internals;

public sealed class TypeRegistry
{
    private Dictionary<Guid, Type> Types { get; }

    private Dictionary<Guid, PrimitiveType> PrimitiveTypes { get; }

    private Dictionary<Guid, DynamicType> DynamicTypes { get; }

    private Dictionary<Guid, ArrayLikeType> ArrayLikeTypes { get; }

    private Dictionary<Guid, DictionaryType> DictionaryTypes { get; }

    private Dictionary<Guid, TaskType> TaskTypes { get; }

    private Dictionary<Guid, EnumType> EnumTypes { get; }

    private Dictionary<Guid, StringEnumType> StringEnumTypes { get; }

    private Dictionary<Guid, StructType> StructTypes { get; }

    private Dictionary<Guid, ClassType> ClassTypes { get; }

    private Dictionary<Guid, UnionType> UnionTypes { get; }

    public TypeRegistry(Type[] types)
    {
        var groups = types.GroupBy(x => x.Kind).ToDictionary(x => x.Key, x=> x.ToArray());
        this.Types = types.ToDictionary(x => x.Id);
        this.PrimitiveTypes = ExtractGroup<PrimitiveType>(groups, TypeKind.Primitive);
        this.DynamicTypes = ExtractGroup<DynamicType>(groups, TypeKind.Dynamic);
        this.ArrayLikeTypes = ExtractGroup<ArrayLikeType>(groups, TypeKind.ArrayLike);
        this.DictionaryTypes = ExtractGroup<DictionaryType>(groups, TypeKind.Dictionary);
        this.TaskTypes = ExtractGroup<TaskType>(groups, TypeKind.Task);
        this.EnumTypes = ExtractGroup<EnumType>(groups, TypeKind.Enum);
        this.StringEnumTypes = ExtractGroup<StringEnumType>(groups, TypeKind.StringEnum);
        this.StructTypes = ExtractGroup<StructType>(groups, TypeKind.Struct);
        this.ClassTypes = ExtractGroup<ClassType>(groups, TypeKind.Class);
        this.UnionTypes = ExtractGroup<UnionType>(groups, TypeKind.Union);
    }

    public TypeRegistry(IEnumerable<Type> types) : this(types as Type[] ?? types.ToArray()) { }

    private static Dictionary<Guid, TType> ExtractGroup<TType>(Dictionary<String, Type[]> groups, String groupKind) where TType : Type =>
        groups.GetValueOrDefault(groupKind, Array.Empty<Type>())
            .Cast<TType>()
            .ToDictionary(x => x.Id);

    public Type GetType(Guid typeId) => this.Types[typeId];

    public PrimitiveType GetPrimitiveType(Guid typeId) => this.PrimitiveTypes[typeId];

    public DynamicType GetDynamicTypeType(Guid typeId) => this.DynamicTypes[typeId];

    public ArrayLikeType GetArrayLikeType(Guid typeId) => this.ArrayLikeTypes[typeId];

    public DictionaryType GetDictionaryType(Guid typeId) => this.DictionaryTypes[typeId];

    public TaskType GetTaskType(Guid typeId) => this.TaskTypes[typeId];

    public TaskType GetTaskType(Guid? typeId) => typeId is null
        ? new TaskType(typeof(Task<>))
        : this.GetTaskType(typeId.Value);

    public EnumType GetEnumType(Guid typeId) => this.EnumTypes[typeId];

    public StringEnumType GetStringEnumType(Guid typeId) => this.StringEnumTypes[typeId];

    public StructType GetStructType(Guid typeId) => this.StructTypes[typeId];

    public ClassType GetClassType(Guid typeId) => this.ClassTypes[typeId];

    public UnionType GetUnionType(Guid typeId) => this.UnionTypes[typeId];

    public DistinctType GetDistinctType(Guid typeId)
    {
        if (this.EnumTypes.TryGetValue(typeId, out var @enum)) return @enum;
        if (this.StringEnumTypes.TryGetValue(typeId, out var stringEnum)) return stringEnum;
        if (this.StructTypes.TryGetValue(typeId, out var @struct)) return @struct;
        if (this.ClassTypes.TryGetValue(typeId, out var @class)) return @class;
        if (this.UnionTypes.TryGetValue(typeId, out var union)) return union;
        throw new KeyNotFoundException();
    }

    public IEnumerable<DistinctType> EnumerateDistinctTypes() =>
        this.EnumTypes.Values.Cast<DistinctType>()
            .Concat(this.StringEnumTypes.Values)
            .Concat(this.StructTypes.Values)
            .Concat(this.ClassTypes.Values)
            .Concat(this.UnionTypes.Values);
}
