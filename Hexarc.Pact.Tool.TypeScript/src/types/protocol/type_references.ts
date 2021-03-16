export interface ArrayTypeReference {
  readonly kind: "Array";
  readonly arrayLikeTypeId?: string;
  readonly elementType: TypeReference;
}

export interface DictionaryTypeReference {
  readonly kind: "Dictionary";
  readonly typeId: string;
  readonly keyType: TypeReference;
  readonly valueType: TypeReference;
}

export interface DistinctTypeReference {
  readonly kind: "Distinct";
  readonly typeId: string;
  readonly typeArguments?: TypeReference[];
}

export interface TypeParameterReference {
  readonly kind: "TypeParameter";
  readonly name: string;
}

export interface NullableTypeReference {
  readonly kind: "Nullable";
  readonly underlyingType: TypeReference;
}

export interface TaskTypeReference {
  readonly kind: "Task";
  readonly typeId?: string;
  readonly resultType: TypeReference;
}

export interface PrimitiveTypeReference {
  readonly kind: "Primitive";
  readonly typeId: string;
}

export interface DynamicTypeReference {
  readonly kind: "Dynamic";
  readonly typeId: string;
}

export interface LiteralTypeReference {
  readonly kind: "Literal";
  readonly name: string;
}

export type TypeReference =
  | ArrayTypeReference
  | DictionaryTypeReference
  | DistinctTypeReference
  | TypeParameterReference
  | NullableTypeReference
  | TaskTypeReference
  | PrimitiveTypeReference
  | DynamicTypeReference
  | LiteralTypeReference;
