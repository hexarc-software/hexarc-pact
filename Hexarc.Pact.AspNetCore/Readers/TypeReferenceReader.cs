namespace Hexarc.Pact.AspNetCore.Readers;

using Namotion.Reflection;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.AspNetCore.Extensions;

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
        { Nullability: Nullability.Nullable, IsNullableType: false } => this.ReadNullableReferenceTypeReference(contextualType),
        _ => this.ReadUnwrapped(contextualType)
    };

    private TypeReference ReadUnwrapped(ContextualType contextualType) => contextualType switch
    {
        var x when this.TypeChecker.IsActionResultOfT(x.OriginalType) => this.ReadFromActionResultOfT(x),
        var x when this.TypeChecker.IsNullableValueType(x) => this.ReadNullableValueTypeReference(x),
        var x when this.TypeChecker.IsTaskType(x.OriginalType) => this.ReadTaskTypeReference(x),
        var x when this.TypeChecker.IsTypeParameter(x.OriginalType) => this.ReadTypeParameterReference(x),
        var x when this.TypeChecker.IsArrayType(x.OriginalType) => this.ReadArrayTypeReference(x),
        var x when this.TypeChecker.IsArrayLikeType(x.OriginalType) => this.ReadArrayLikeTypeReference(x),
        var x when this.TypeChecker.IsDictionaryType(x.OriginalType) => this.ReadDictionaryTypeReference(x),
        var x when this.TypeChecker.IsPrimitiveType(x.OriginalType) => this.ReadPrimitiveTypeReference(x),
        var x when this.TypeChecker.IsTupleType(x.OriginalType) => this.ReadTupleTypeReference(x),
        var x when this.TypeChecker.IsDynamicType(x.OriginalType) => this.ReadDynamicTypeReference(x),
        var x => this.ReadDistinctTypeReference(x)
    };

    private NullableTypeReference ReadNullableReferenceTypeReference(ContextualType contextualType) =>
        new(this.ReadUnwrapped(contextualType));

    private TypeReference ReadFromActionResultOfT(ContextualType contextualType) =>
        this.ReadInternal(contextualType.GenericArguments.First());

    private NullableTypeReference ReadNullableValueTypeReference(ContextualType contextualType) =>
        new(this.ReadInternal(contextualType.Type.ToContextualType()));

    private TaskTypeReference ReadTaskTypeReference(ContextualType contextualType) =>
        contextualType.GenericArguments.Length == 0
            ? new TaskTypeReference(contextualType.OriginalType.GUID)
            : new TaskTypeReference(contextualType.OriginalType.GUID, this.ReadInternal(contextualType.GenericArguments.First()));

    private TypeParameterReference ReadTypeParameterReference(ContextualType contextualType) =>
        new(contextualType.OriginalType.Name);

    private ArrayTypeReference ReadArrayTypeReference(ContextualType contextualType) =>
        new(this.ReadInternal(contextualType.ElementType!));

    private ArrayTypeReference ReadArrayLikeTypeReference(ContextualType contextualType) =>
        new(contextualType.OriginalType.GUID, this.ReadInternal(contextualType.GenericArguments.First()));

    private DictionaryTypeReference ReadDictionaryTypeReference(ContextualType contextualType) =>
        new(contextualType.OriginalType.GUID, contextualType.GenericArguments.Select(this.ReadInternal).ToArray());

    private PrimitiveTypeReference ReadPrimitiveTypeReference(ContextualType contextualType) =>
        new(contextualType.OriginalType.GUID);

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

    private DynamicTypeReference ReadDynamicTypeReference(ContextualType contextualType) =>
        new(contextualType.OriginalType.GUID);

    private DistinctTypeReference ReadDistinctTypeReference(ContextualType contextualType)
    {
        var type = contextualType.OriginalType;
        this.DistinctTypeQueue.Enqueue(type.IsGenericType ? type.GetGenericTypeDefinition() : type);
        return new DistinctTypeReference(contextualType.OriginalType.GUID, this.ReadGenericArguments(contextualType.GenericArguments));
    }

    private TypeReference[]? ReadGenericArguments(ContextualType[] genericTypes) =>
        genericTypes.Length != 0 ? Array.ConvertAll(genericTypes, this.ReadInternal) : default;

    private TypeReferenceReader ApplyNamingConvention(NamingConvention? namingConvention)
    {
        this.NamingConvention = namingConvention;
        return this;
    }

    private ReaderState GetOrCreateState(ContextualType tupleContextualType) =>
        this.State ??= ReaderState.Create(tupleContextualType);

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

        public static ReaderState Create(ContextualType tupleContextualType) =>
            new(tupleContextualType.GetTupleElementNames()?.ToArray(), 0);
    }
}
