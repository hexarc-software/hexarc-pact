namespace Hexarc.Pact.AspNetCore.Readers;

using Type = System.Type;

public sealed class TypeReferenceReader
{
    private TypeChecker TypeChecker { get; }

    private DistinctTypeQueue DistinctTypeQueue { get; }

    private ReaderState? State { get; set; }

    private NamingConvention? NamingConvention { get; set; }

    public TypeReferenceReader(TypeChecker typeChecker, DistinctTypeQueue distinctTypeQueue) =>
        (this.TypeChecker, this.DistinctTypeQueue) = (typeChecker, distinctTypeQueue);

    public TypeReference Read(ContextualType contextualType, NamingConvention? namingConvention) =>
        this.ResetState().ApplyNamingConvention(namingConvention).ReadInternal(contextualType);

    private TypeReference ReadInternal(ContextualType contextualType) => contextualType switch
    {
        var x when x.IsNullable => this.ReadNullableTypeReference(contextualType),
        _ => this.ReadUnwrapped(contextualType)
    };

    private TypeReference ReadUnwrapped(ContextualType contextualType) => contextualType switch
    {
        var x when this.TypeChecker.IsActionResultOfT(x.Type) => this.ReadFromActionResultOfT(x),
        var x when this.TypeChecker.IsTaskType(x.Type) => this.ReadTaskTypeReference(x),
        var x when this.TypeChecker.IsTypeParameter(x.Type) => this.ReadTypeParameterReference(x.Type),
        var x when this.TypeChecker.IsArrayType(x.Type) => this.ReadArrayTypeReference(x),
        var x when this.TypeChecker.IsArrayLikeType(x.Type) => this.ReadArrayLikeTypeReference(x),
        var x when this.TypeChecker.IsDictionaryType(x.Type) => this.ReadDictionaryTypeReference(x),
        var x when this.TypeChecker.IsPrimitiveType(x.Type) => this.ReadPrimitiveTypeReference(x.Type),
        var x when this.TypeChecker.IsTupleType(x.Type) => this.ReadTupleTypeReference(x),
        var x when this.TypeChecker.IsDynamicType(x.Type) => this.ReadDynamicTypeReference(x.Type),
        var x => this.ReadDistinctTypeReference(x)
    };

    private NullableTypeReference ReadNullableTypeReference(ContextualType contextualType) =>
        new(this.ReadUnwrapped(contextualType));

    private TypeReference ReadFromActionResultOfT(ContextualType contextualType) =>
        this.ReadInternal(contextualType.GenericArguments.First());

    private TaskTypeReference ReadTaskTypeReference(ContextualType contextualType) =>
        contextualType.GenericArguments.Length == 0
            ? new TaskTypeReference(contextualType.Type.GUID)
            : new TaskTypeReference(
                contextualType.Type.GUID,
                this.ReadInternal(contextualType.GenericArguments.First()));

    private TypeParameterReference ReadTypeParameterReference(Type type) =>
        new(type.Name);

    private ArrayTypeReference ReadArrayTypeReference(ContextualType contextualType) =>
        new(this.ReadInternal(contextualType.ElementType!));

    private ArrayTypeReference ReadArrayLikeTypeReference(ContextualType contextualType) =>
        new(contextualType.Type.GUID, this.ReadInternal(contextualType.GenericArguments.First()));

    private DictionaryTypeReference ReadDictionaryTypeReference(ContextualType contextualType) =>
        new(contextualType.Type.GUID,
            contextualType.GenericArguments
                .Select(this.ReadInternal)
                .ToArray());

    private PrimitiveTypeReference ReadPrimitiveTypeReference(Type type) =>
        new(type.GUID);

    private TupleTypeReference ReadTupleTypeReference(ContextualType contextualType) =>
        this.GetOrCreateState(contextualType).IsAnonymousTuple
            ? this.ReadAnonymousTupleTypeReference(contextualType)
            : this.ReadNamedTupleTypeReference(contextualType);

    private TupleTypeReference ReadNamedTupleTypeReference(ContextualType contextualType)
    {
        var elementTypes = contextualType.GetTupleArguments();
        var elementNames = this.State!.ExtractNextTupleElementNames(elementTypes.Length);
        return new TupleTypeReference(this.ReadTupleElements(elementTypes, elementNames));
    }

    private TupleTypeReference ReadAnonymousTupleTypeReference(ContextualType contextualType)
    {
        var elementTypes = contextualType.GetTupleArguments();
        return new TupleTypeReference(this.ReadTupleElements(elementTypes));
    }

    private TupleElement[] ReadTupleElements(ContextualType[] elementTypes, String?[] elementNames) =>
        elementTypes.Select((x, i) => this.ReadTupleElement(x, elementNames[i], this.NamingConvention)).ToArray();

    private TupleElement[] ReadTupleElements(ContextualType[] elementTypes) =>
        elementTypes.Select(this.ReadTupleElement).ToArray();

    private TupleElement ReadTupleElement(ContextualType elementType) =>
        this.ReadTupleElement(elementType, default, default);

    private TupleElement ReadTupleElement(
        ContextualType elementType, String? name, NamingConvention? namingConvention
    ) => new(this.ReadInternal(elementType), name?.ToConventionalString(namingConvention));

    private DynamicTypeReference ReadDynamicTypeReference(Type type) =>
        new(type.GUID);

    private DistinctTypeReference ReadDistinctTypeReference(ContextualType contextualType)
    {
        var type = contextualType.Type;
        var genericArguments = contextualType.GenericArguments;
        this.DistinctTypeQueue.Enqueue(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        return new DistinctTypeReference(type.GUID, this.ReadGenericArguments(genericArguments));
    }

    private TypeReference[]? ReadGenericArguments(ContextualType[] genericArguments) =>
        genericArguments.Length != 0 ? Array.ConvertAll(genericArguments, this.ReadInternal) : default;

    private TypeReferenceReader ApplyNamingConvention(NamingConvention? namingConvention)
    {
        this.NamingConvention = namingConvention;
        return this;
    }

    private ReaderState GetOrCreateState(ContextualType tupleType) =>
        this.State ??= ReaderState.Create(tupleType);

    private TypeReferenceReader ResetState()
    {
        this.State = default;
        return this;
    }

    private sealed class ReaderState
    {
        private String?[]? TupleElementNames { get; }

        private Int32 TupleElementOffset { get; set; }

        public Boolean IsAnonymousTuple => this.TupleElementNames is null;

        private ReaderState(String?[]? tupleElementNames, Int32 tupleElementOffset) =>
            (this.TupleElementNames, this.TupleElementOffset) = (tupleElementNames, tupleElementOffset);

        public String?[] ExtractNextTupleElementNames(Int32 tupleElementCount)
        {
            try
            {
                return this.TupleElementNames!.Skip(this.TupleElementOffset).Take(tupleElementCount).ToArray();
            }
            finally
            {
                this.TupleElementOffset += tupleElementCount;
            }
        }

        public static ReaderState Create(ContextualType tupleType) =>
            new(tupleType.GetTupleElementNames()?.ToArray(), 0);
    }
}
