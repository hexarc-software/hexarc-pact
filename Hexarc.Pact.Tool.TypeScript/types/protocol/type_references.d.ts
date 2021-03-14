declare namespace Pact.Protocol.TypeReferences {
  interface ArrayTypeReference {
    readonly kind: "Array";
    readonly arrayLikeTypeId?: string;
    readonly elementType: TypeReference;
  }

  interface DictionaryTypeReference {
    readonly kind: "Dictionary";
    readonly typeId: string;
    readonly keyType: TypeReference;
    readonly valueType: TypeReference;
  }

  interface DistinctTypeReference {
    readonly kind: "Distinct";
    readonly typeId: string;
    readonly typeArguments?: TypeReference[];
  }

  interface TypeParameterReference {
    readonly kind: "TypeParameter";
    readonly name: string;
  }

  interface NullableTypeReference {
    readonly kind: "Nullable";
    readonly underlyingType: TypeReference;
  }

  interface TaskTypeReference {
    readonly kind: "Task";
    readonly typeId?: string;
    readonly resultType: TypeReference;
  }

  interface PrimitiveTypeReference {
    readonly kind: "Primitive";
    readonly typeId: string;
  }

  interface DynamicTypeReference {
    readonly kind: "Dynamic";
    readonly typeId: string;
  }

  interface LiteralTypeReference {
    readonly kind: "Literal";
    readonly name: string;
  }

  type TypeReference =
    | ArrayTypeReference
    | DictionaryTypeReference
    | DistinctTypeReference
    | TypeParameterReference
    | NullableTypeReference
    | TaskTypeReference
    | PrimitiveTypeReference
    | DynamicTypeReference
    | LiteralTypeReference;
}
