namespace Hexarc.Pact.AspNetCore.Internals;

public static class KnownTypes
{
    public static IEnumerable<PrimitiveType> GetPrimitiveTypes()
    {
        yield return new PrimitiveType(typeof(Boolean));
        yield return new PrimitiveType(typeof(Byte));
        yield return new PrimitiveType(typeof(SByte));
        yield return new PrimitiveType(typeof(Char));
        yield return new PrimitiveType(typeof(Int16));
        yield return new PrimitiveType(typeof(UInt16));
        yield return new PrimitiveType(typeof(Int32));
        yield return new PrimitiveType(typeof(UInt32));
        yield return new PrimitiveType(typeof(Int64));
        yield return new PrimitiveType(typeof(UInt64));
        yield return new PrimitiveType(typeof(Single));
        yield return new PrimitiveType(typeof(Double));
        yield return new PrimitiveType(typeof(Decimal));
        yield return new PrimitiveType(typeof(String));
        yield return new PrimitiveType(typeof(Guid));
        yield return new PrimitiveType(typeof(DateTime));
    }

    public static IEnumerable<ArrayLikeType> GetArrayLikeTypes()
    {
        yield return new ArrayLikeType(typeof(IEnumerable<>));
        yield return new ArrayLikeType(typeof(ICollection<>));
        yield return new ArrayLikeType(typeof(IReadOnlyCollection<>));
        yield return new ArrayLikeType(typeof(List<>));
        yield return new ArrayLikeType(typeof(IList<>));
        yield return new ArrayLikeType(typeof(IReadOnlyList<>));
        yield return new ArrayLikeType(typeof(HashSet<>));
        yield return new ArrayLikeType(typeof(ISet<>));
        yield return new ArrayLikeType(typeof(IReadOnlySet<>));
    }

    public static IEnumerable<DictionaryType> GetDictionaryTypes()
    {
        yield return new DictionaryType(typeof(Dictionary<,>));
        yield return new DictionaryType(typeof(IDictionary<,>));
        yield return new DictionaryType(typeof(ReadOnlyDictionary<,>));
        yield return new DictionaryType(typeof(IReadOnlyDictionary<,>));
    }

    public static IEnumerable<DynamicType> GetDynamicTypes()
    {
        yield return new DynamicType(typeof(Object));
        yield return new DynamicType(typeof(JsonElement));
    }

    public static IEnumerable<TaskType> GetTaskTypes()
    {
        yield return new TaskType(typeof(Task));
        yield return new TaskType(typeof(ValueTask));
        yield return new TaskType(typeof(Task<>));
        yield return new TaskType(typeof(ValueTask<>));
    }

    public static IEnumerable<System.Type> GetTupleTypes()
    {
        yield return typeof(ValueTuple<>);
        yield return typeof(ValueTuple<,>);
        yield return typeof(ValueTuple<,,>);
        yield return typeof(ValueTuple<,,,>);
        yield return typeof(ValueTuple<,,,,>);
        yield return typeof(ValueTuple<,,,,,>);
        yield return typeof(ValueTuple<,,,,,,>);
        yield return typeof(ValueTuple<,,,,,,,>);
    }
}
